// -------------------------------------------------------------------------------------
// <copyright file="LensSetting.cs" company="Strange Entertainment LLC">
//   Copyright 2004-2023 Strange Entertainment LLC. All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Lens.Properties;

namespace Lens
{
   public class Lens : INotifyPropertyChanged
   {
      private static Lens instance;

      private Lens()
      {
      }

      public static Lens Instance => instance ?? (instance = new Lens());

      public bool AutoClose
      {
         get => Settings.Default.AutoClose;
         set => this.SetProperty(nameof(Settings.Default.AutoClose), value);
      }

      public Color GridColor
      {
         get => Settings.Default.GridColor;
         set => this.SetProperty(nameof(Settings.Default.GridColor), value);
      }

      public byte GridSize
      {
         get => Settings.Default.GridSize;
         set => this.SetProperty(nameof(Settings.Default.GridSize),
            value.Clamp(Defaults.MinGridSize, Defaults.MaxGridSize));
      }

      public int GridStyle
      {
         get => Settings.Default.GridStyle;
         set => this.SetProperty(nameof(Settings.Default.GridStyle),
            value.Clamp((int)GridStyleOptions.None, (int)GridStyleOptions.DashDotDot));
      }

      public short Height
      {
         get => Settings.Default.Height;
         set => this.SetProperty(nameof(Settings.Default.Height),
            (short)(value.Clamp(Defaults.MinHeight, Defaults.MaxHeight) / Defaults.SizeIncrement *
                    Defaults.SizeIncrement));
      }

      public bool HideCursor
      {
         get => Settings.Default.HideCursor;
         set => this.SetProperty(nameof(Settings.Default.HideCursor), value);
      }

      public byte Magnification
      {
         get => Settings.Default.Magnification;
         set => this.SetProperty(nameof(Settings.Default.Magnification),
            value.Clamp(Defaults.MinMagnification, Defaults.MaxMagnification));
      }

      public bool NearestNeighbor
      {
         get => Settings.Default.NearestNeighbor;
         set => this.SetProperty(nameof(Settings.Default.NearestNeighbor), value);
      }

      public byte SpeedFactor
      {
         get => Settings.Default.SpeedFactor;
         set => this.SetProperty(nameof(Settings.Default.SpeedFactor),
            value.Clamp(Defaults.MinSpeedFactor, Defaults.MaxSpeedFactor));
      }

      public short Width
      {
         get => Settings.Default.Width;
         set => this.SetProperty(nameof(Settings.Default.Width),
            (short)(value.Clamp(Defaults.MinWidth, Defaults.MaxWidth) / Defaults.SizeIncrement *
                    Defaults.SizeIncrement));
      }

      // ── Info panel display toggles — not persisted; UI to be added later. ──────────────
      public bool InfoShowHex   { get; set; } = true;
      public bool InfoShowRgb   { get; set; } = true;
      public bool InfoShowHsl   { get; set; } = true;
      public bool InfoShow12Bit { get; set; } = true;
      public bool InfoShowWeb   { get; set; } = true;
      public bool InfoShowMouse { get; set; } = true;
      public bool InfoShowSize  { get; set; } = true;
      public bool InfoShowZoom  { get; set; } = true;

      public event PropertyChangedEventHandler PropertyChanged;

      private void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      private bool SetProperty<T>(string settingName, T value, [CallerMemberName] string propertyName = null)
      {
         var original = (T)Settings.Default[settingName];
         if (EqualityComparer<T>.Default.Equals(original, value)) return false;
         Settings.Default[settingName] = value;
         Settings.Default.Save();
         Debug.WriteLine($"{propertyName} = {value}");
         this.OnPropertyChanged(propertyName);
         return true;
      }

      private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
      {
         if (EqualityComparer<T>.Default.Equals(field, value)) return false;
         field = value;
         this.OnPropertyChanged(propertyName);
         return true;
      }

      public static class Defaults
      {
         public const byte MaxGridSize = 16;
         public const byte MinGridSize = 1;

         public const byte MaxMouseSpeed = 20;
         public const byte MinMouseSpeed = 1;

         public const byte MaxMagnification = 16;
         public const byte MinMagnification = 2;

         public const short MaxHeight = 400;
         public const short MinHeight = 100;

         public const short MaxWidth = 400;
         public const short MinWidth = 100;

         public const byte MaxSpeedFactor = 10;
         public const byte MinSpeedFactor = 1;

         public const byte SizeIncrement = 20;

         static Defaults()
         {
            Debug.Assert(SizeIncrement % 2 == 0);
            Debug.Assert(MaxHeight % SizeIncrement == 0);
            Debug.Assert(MinHeight % SizeIncrement == 0);
            Debug.Assert(MaxWidth % SizeIncrement == 0);
            Debug.Assert(MinWidth % SizeIncrement == 0);
         }
      }
   }
}
