// -------------------------------------------------------------------------------------
// <copyright file="LensSetting.cs">
//   Copyright (c) 2026
//   Licensed under the MIT License. See LICENSE file in the project root.
// </copyright>
// -------------------------------------------------------------------------------------

using Timer = System.Timers.Timer;

namespace StrangeLens
{
   using System;
   using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Diagnostics;
   using System.IO;
   using System.Linq;
   using System.Reflection;
   using System.Runtime.CompilerServices;
   using System.Text.Json;

   using Microsoft.Win32;

   public partial class Lens : INotifyPropertyChanged
   {
      /// <summary>Coalesces rapid successive edits (e.g., a burst of scroll-wheel zoom notches)
      ///    into a single save, without the multi-second lag that used to make cross-process sync
      ///    feel sluggish.</summary>
      private const int SaveDebounceMs = 500;

      public static readonly int[] GridOpacityOptions = [20, 40, 60, 80, 100];

      public static readonly int[] PrecisionSpeedOptions = [10, 25, 45, 70];

      private static readonly JsonSerializerOptions jsonOptions = new()
         {
            WriteIndented = true,
         };

      private static Lens? instance;

      /// <summary>Property names changed locally since the last successful <see cref="Save"/>.
      ///    <see cref="Load(string)"/> skips reassigning anything still pending here, so a reload
      ///    triggered by (this process's own or another process's) file write can never clobber an
      ///    edit this process hasn't flushed to disk yet.</summary>
      private readonly ConcurrentDictionary<string, byte> pendingProperties = new();

      private readonly Timer saveTimer;

      private DistractionFreeSettings distractionFree = new();

      private int gridOpacity = 20;

      private byte gridSize = 4;

      private GridStyleOption gridStyle = GridStyleOption.Dash;

      private short height = 160;

      private bool infoShow12Bit = true;

      private bool infoShowHex = true;

      private bool infoShowHsl = true;

      private bool infoShowMouse = true;

      private bool infoShowRgb = true;

      private bool infoShowSize = true;

      private bool infoShowWeb = true;

      private bool infoShowZoom = true;

      private byte magnification = 4;

      private int precisionSpeed = 45;

      private ScalingModeOption scalingMode = ScalingModeOption.NearestNeighbor;

      private string theme = "system";

      private short width = 150;

      private Lens()
      {
         this.saveTimer = new Timer(SaveDebounceMs)
            {
               AutoReset = false,
            };
         this.saveTimer.Elapsed += (_, _) => this.RunOnOwnerThread(this.Save);
         this.PropertyChanged += (_, _) =>
            {
               this.saveTimer.Stop();
               this.saveTimer.Start();
            };
      }

      public event PropertyChangedEventHandler? PropertyChanged;

      public static Lens Instance => instance ??= new Lens();

      /// <summary>Config-file-only; not editable via any Settings UI. See
      ///    <see cref="DistractionFreeSettings"/>.</summary>
      public DistractionFreeSettings DistractionFree
      {
         get => this.distractionFree;
         private set => this.distractionFree = value;
      }

      /// <summary>Whether distraction-free mode (Ctrl+Alt+Shift+D, focus-scoped to <c>LensForm</c>
      ///    ) is currently on. In-memory only -- persists for the life of the process but resets
      ///    to off on the next launch; never written to settings.json.</summary>
      public bool DistractionFreeActive { get; set; }

      public int GridOpacity
      {
         get => this.gridOpacity;
         set =>
            this.SetPersisted(
               ref this.gridOpacity,
               Array.IndexOf(GridOpacityOptions, value) >= 0 ? value : this.gridOpacity);
      }

      public byte GridSize
      {
         get => this.gridSize;
         set => this.SetPersisted(ref this.gridSize, value.Clamp(Defaults.MinGridSize, Defaults.MaxGridSize));
      }

      public GridStyleOption GridStyle
      {
         get => this.gridStyle;
         set =>
            this.SetPersisted(
               ref this.gridStyle,
               Enum.IsDefined(typeof(GridStyleOption), value) ? value : this.gridStyle);
      }

      public short Height
      {
         get => this.height;
         set =>
            this.SetPersisted(
               ref this.height,
               (short)((value.Clamp(Defaults.MinHeight, Defaults.MaxHeight) / Defaults.SizeIncrement)
                       * Defaults.SizeIncrement));
      }

      public bool InfoShow12Bit
      {
         get => this.infoShow12Bit;
         set => this.SetPersisted(ref this.infoShow12Bit, value);
      }

      public bool InfoShowHex
      {
         get => this.infoShowHex;
         set => this.SetPersisted(ref this.infoShowHex, value);
      }

      public bool InfoShowHsl
      {
         get => this.infoShowHsl;
         set => this.SetPersisted(ref this.infoShowHsl, value);
      }

      public bool InfoShowMouse
      {
         get => this.infoShowMouse;
         set => this.SetPersisted(ref this.infoShowMouse, value);
      }

      public bool InfoShowRgb
      {
         get => this.infoShowRgb;
         set => this.SetPersisted(ref this.infoShowRgb, value);
      }

      public bool InfoShowSize
      {
         get => this.infoShowSize;
         set => this.SetPersisted(ref this.infoShowSize, value);
      }

      public bool InfoShowWeb
      {
         get => this.infoShowWeb;
         set => this.SetPersisted(ref this.infoShowWeb, value);
      }

      public bool InfoShowZoom
      {
         get => this.infoShowZoom;
         set => this.SetPersisted(ref this.infoShowZoom, value);
      }

      public byte Magnification
      {
         get => this.magnification;
         set =>
            this.SetPersisted(
               ref this.magnification,
               value.Clamp(Defaults.MinMagnification, Defaults.MaxMagnification));
      }

      public int PrecisionSpeed
      {
         get => this.precisionSpeed;
         set =>
            this.SetPersisted(
               ref this.precisionSpeed,
               Array.IndexOf(PrecisionSpeedOptions, value) >= 0 ? value : this.precisionSpeed);
      }

      public ScalingModeOption Scaling
      {
         get => this.scalingMode;
         set =>
            this.SetPersisted(
               ref this.scalingMode,
               Enum.IsDefined(typeof(ScalingModeOption), value) ? value : this.scalingMode);
      }

      public string Theme
      {
         get => this.theme;
         private set =>
            this.SetPersisted(
               ref this.theme,
               string.IsNullOrEmpty(value) ? "system" :
               (value == "system") || (value == "dark") || (value == "light") ? value :
               IsOsDarkMode() ? "dark" : "light");
      }

      public short Width
      {
         get => this.width;
         set =>
            this.SetPersisted(
               ref this.width,
               (short)((value.Clamp(Defaults.MinWidth, Defaults.MaxWidth) / Defaults.SizeIncrement)
                       * Defaults.SizeIncrement));
      }

      private static string SettingsFilePath
      {
         get
         {
            var company = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
            if (string.IsNullOrWhiteSpace(company))
            {
               company = "Strange";
            }

            var product = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>()?.Product
                          ?? "StrangeLens";
            return Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
               company,
               product,
               "settings.json");
         }
      }

      public static bool IsOsDarkMode()
      {
         try
         {
            using var key = Registry.CurrentUser.OpenSubKey(
               @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            return key?.GetValue("AppsUseLightTheme") is 0;
         }
         catch
         {
            return false;
         }
      }

      public void Load()
      {
         this.Load(SettingsFilePath);
      }

      public void Save()
      {
         var path = SettingsFilePath;

         // Snapshot exactly which properties this save is flushing -- not a blanket
         // Clear() -- so a change that lands mid-save stays pending for the next cycle
         // instead of being forgotten.
         var flushing = this.pendingProperties.Keys.ToArray();
         try
         {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            var data = new SettingsData
               {
                  Width = this.width,
                  Height = this.height,
                  Magnification = this.magnification,
                  GridSize = this.gridSize,
                  GridStyle = (int)this.gridStyle,
                  GridOpacity = this.gridOpacity,
                  Scaling = (int)this.scalingMode,
                  PrecisionSpeed = this.precisionSpeed,
                  InfoShowHex = this.infoShowHex,
                  InfoShowRgb = this.infoShowRgb,
                  InfoShowHsl = this.infoShowHsl,
                  InfoShow12Bit = this.infoShow12Bit,
                  InfoShowWeb = this.infoShowWeb,
                  InfoShowMouse = this.infoShowMouse,
                  InfoShowSize = this.infoShowSize,
                  InfoShowZoom = this.infoShowZoom,
                  Theme = this.theme,
                  DistractionFree = this.distractionFree,
               };
            File.WriteAllText(path, JsonSerializer.Serialize(data, jsonOptions));

            foreach (var propertyName in flushing)
            {
               this.pendingProperties.TryRemove(propertyName, out _);
            }
         }
         catch (Exception ex)
         {
            Debug.WriteLine($"Failed to save settings: {ex.Message}");
         }
      }

      internal static void ResetForTesting()
      {
         instance?.saveTimer.Stop();
         instance?.saveTimer.Dispose();
         instance = null;
      }

      internal void Load(string path)
      {
         if (!File.Exists(path))
         {
            return;
         }

         try
         {
            var data = JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(path), jsonOptions);
            if (data == null)
            {
               return;
            }

            // Skip any property with an edit made here that hasn't been flushed to disk
            // yet -- otherwise a reload (our own echoed save, or the other process's)
            // can clobber it with the stale value this file still has for it.
            this.LoadIfNotPending(nameof(this.Width), () => this.Width = data.Width);
            this.LoadIfNotPending(nameof(this.Height), () => this.Height = data.Height);
            this.LoadIfNotPending(nameof(this.Magnification), () => this.Magnification = data.Magnification);
            this.LoadIfNotPending(nameof(this.GridSize), () => this.GridSize = data.GridSize);
            this.LoadIfNotPending(nameof(this.GridStyle), () => this.GridStyle = (GridStyleOption)data.GridStyle);
            this.LoadIfNotPending(nameof(this.GridOpacity), () => this.GridOpacity = data.GridOpacity);
            this.LoadIfNotPending(nameof(this.Scaling), () => this.Scaling = (ScalingModeOption)data.Scaling);
            this.LoadIfNotPending(nameof(this.PrecisionSpeed), () => this.PrecisionSpeed = data.PrecisionSpeed);
            this.LoadIfNotPending(nameof(this.InfoShowHex), () => this.InfoShowHex = data.InfoShowHex);
            this.LoadIfNotPending(nameof(this.InfoShowRgb), () => this.InfoShowRgb = data.InfoShowRgb);
            this.LoadIfNotPending(nameof(this.InfoShowHsl), () => this.InfoShowHsl = data.InfoShowHsl);
            this.LoadIfNotPending(nameof(this.InfoShow12Bit), () => this.InfoShow12Bit = data.InfoShow12Bit);
            this.LoadIfNotPending(nameof(this.InfoShowWeb), () => this.InfoShowWeb = data.InfoShowWeb);
            this.LoadIfNotPending(nameof(this.InfoShowMouse), () => this.InfoShowMouse = data.InfoShowMouse);
            this.LoadIfNotPending(nameof(this.InfoShowSize), () => this.InfoShowSize = data.InfoShowSize);
            this.LoadIfNotPending(nameof(this.InfoShowZoom), () => this.InfoShowZoom = data.InfoShowZoom);

            this.LoadIfNotPending(nameof(this.Theme), () => this.Theme = data.Theme);

            // Config-file-only, never edited at runtime -- no pending-edit race to guard against.
            this.DistractionFree = data.DistractionFree ?? new DistractionFreeSettings();

            Debug.WriteLine($"Settings loaded from {path}");
         }
         catch (Exception ex)
         {
            Debug.WriteLine($"Failed to load settings: {ex.Message}");
         }
      }

      private void LoadIfNotPending(string propertyName, Action apply)
      {
         if (this.pendingProperties.ContainsKey(propertyName))
         {
            Debug.WriteLine($"Skipped loading {propertyName}: local change not yet saved");
            return;
         }

         apply();
      }

      private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
      {
         this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      private void SetPersisted<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
      {
         if (EqualityComparer<T>.Default.Equals(field, value))
         {
            return;
         }

         field = value;
         this.pendingProperties[propertyName!] = 0;
         Debug.WriteLine($"{propertyName} = {value}");
         this.OnPropertyChanged(propertyName);
      }
   }
}
