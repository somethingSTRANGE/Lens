// -------------------------------------------------------------------------------------
// <copyright file="SettingsForm.cs" company="Strange Entertainment LLC">
//   Copyright 2004-2023 Strange Entertainment LLC. All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lens
{
   public partial class SettingsForm : Form
   {
      private const int HotkeyToggle = 1;
      private const uint ModCtrlAltShift = 0x0007; // MOD_CONTROL | MOD_ALT | MOD_SHIFT

      [DllImport("user32.dll")] private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
      [DllImport("user32.dll")] private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

      private LensForm activeLens;
      private int clickCount;
      private System.Windows.Forms.Timer clickTimer;
      private bool shouldExitApplication;

      public SettingsForm()
      {
         Debug.WriteLine("SETTINGS FORM CONSTRUCTOR - before InitializeComponent");

         this.InitializeComponent();

         Debug.WriteLine("SETTINGS FORM CONSTRUCTOR - after InitializeComponent");

         this.clickTimer = new System.Windows.Forms.Timer();
         this.clickTimer.Tick += this.ClickTimer_Elapsed;

         var dataSource = Lens.Instance;

         this.valueAutoClose.DataBindings.Add(nameof(this.valueAutoClose.Checked), dataSource,
            nameof(dataSource.AutoClose), false, DataSourceUpdateMode.OnPropertyChanged);

         this.valueGridSize.Minimum = Lens.Defaults.MinGridSize;
         this.valueGridSize.Maximum = Lens.Defaults.MaxGridSize;
         this.valueGridSize.DataBindings.Add(nameof(this.valueGridSize.Value), dataSource,
            nameof(dataSource.GridSize), false, DataSourceUpdateMode.OnPropertyChanged);

         this.valueGridColor.DataBindings.Add(nameof(this.valueGridColor.BackColor), dataSource,
            nameof(dataSource.GridColor), false, DataSourceUpdateMode.OnPropertyChanged);

         LoadGridStyleItems(this.valueGridStyle);
         this.valueGridStyle.DataBindings.Add(nameof(this.valueGridStyle.SelectedIndex), dataSource,
            nameof(dataSource.GridStyle), false, DataSourceUpdateMode.OnPropertyChanged);

         // TODO: Remove valueHideCursor from the Settings panel. The lens is now positioned beside
         // the cursor rather than under it, so hiding the cursor no longer serves any purpose.
         this.valueHideCursor.DataBindings.Add(nameof(this.valueHideCursor.Checked), dataSource,
            nameof(dataSource.HideCursor), false, DataSourceUpdateMode.OnPropertyChanged);

         this.valueMagnification.Minimum = Lens.Defaults.MinMagnification;
         this.valueMagnification.Maximum = Lens.Defaults.MaxMagnification;
         this.valueMagnification.DataBindings.Add(nameof(this.valueMagnification.Value), dataSource,
            nameof(dataSource.Magnification), false, DataSourceUpdateMode.OnPropertyChanged);

         this.valueNearestNeighbor.DataBindings.Add(nameof(this.valueNearestNeighbor.Checked), dataSource,
            nameof(dataSource.NearestNeighbor), false, DataSourceUpdateMode.OnPropertyChanged);

         this.valueSpeedFactor.Minimum = Lens.Defaults.MinSpeedFactor;
         this.valueSpeedFactor.Maximum = Lens.Defaults.MaxSpeedFactor;
         this.valueSpeedFactor.DataBindings.Add(nameof(this.valueSpeedFactor.Value), dataSource,
            nameof(dataSource.SpeedFactor), false, DataSourceUpdateMode.OnPropertyChanged);

         // -----------------------

         // this.components = new System.ComponentModel.Container();
         // this.contextMenu = new System.Windows.Forms.ContextMenu();
         var menuItemOpen = new MenuItem { DefaultItem = true, Text = "Show" };
         var menuItemSettings = new MenuItem
               { Text = "&Settings...", Shortcut = Shortcut.CtrlShiftS, ShowShortcut = true };
         var menuItemSeparator = new MenuItem { Text = "-" };
         var menuItemExit = new MenuItem
            {
               // Site = null,
               // Name = null,
               // Tag = null,
               // BarBreak = false,
               // Break = false,
               // Checked = false,
               DefaultItem = false,
               // OwnerDraw = false,
               Enabled = true,
               // Index = 3,
               // MdiList = false,
               // MergeType = MenuMerge.Add,
               // MergeOrder = 0,
               // RadioCheck = false,
               Text = "E&xit",
               // Shortcut = Shortcut.None,
               ShowShortcut = true,
               Visible = true
            };

         // menuItemExit.Index = 0;
         // menuItem.Text = "E&xit";
         menuItemExit.Click += this.menuItemExit_Click;
         menuItemSettings.Click += this.menuItemSettings_Click;
         menuItemOpen.Click += this.menuItemOpen_Click;

         this.contextMenu.MenuItems.AddRange(new[]
               { menuItemOpen, menuItemSettings, menuItemSeparator, menuItemExit });

         // this.notifyIcon = new NotifyIcon(this.components);
         this.notifyIcon.Icon = this.Icon;
         this.notifyIcon.ContextMenu = this.contextMenu;
         this.iconPicture.Image = new Icon(this.Icon, 64, 64).ToBitmap();

         // WinForms creates the Win32 handle lazily on first Show(). Force it now so
         // OnHandleCreated fires immediately and RegisterHotKey works from the start.
         this.CreateHandle();
      }

      private void ClickTimer_Elapsed(object sender, EventArgs e)
      {
         this.clickTimer.Stop();

         if (this.clickCount >= 2)
         {
            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;

            this.OpenSettings(); // Double click the system tray icon
         }
         else
         {
            this.ToggleLens();
         }

         this.clickCount = 0;
      }

      private void OpenSettings()
      {
         Console.WriteLine("Open Settings");
         this.Show();
         this.Activate();
      }

      private static void LoadGridStyleItems(ComboBox cb)
      {
         Debug.WriteLine("LOAD GRID STYLE ITEMS");
         cb.DataSource = Enum.GetValues(typeof(GridStyleOptions)).Cast<Enum>().Select(value => new
            {
               (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
                  typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description,
               value
            }).OrderBy(item => item.value).ToList();
         cb.DisplayMember = "Description";
         cb.ValueMember = "value";
      }

      private void button_Show_Click(object sender, EventArgs e)
      {
         this.ToggleLens();
      }

      protected override void OnHandleCreated(EventArgs e)
      {
         base.OnHandleCreated(e);
         if (!RegisterHotKey(this.Handle, HotkeyToggle, ModCtrlAltShift, (uint)Keys.Z))
            Debug.WriteLine($"RegisterHotKey failed: error {Marshal.GetLastWin32Error()}");
      }

      protected override void OnFormClosed(FormClosedEventArgs e)
      {
         base.OnFormClosed(e);
         UnregisterHotKey(this.Handle, HotkeyToggle);
      }

      protected override void WndProc(ref Message m)
      {
         const int WmHotkey = 0x0312;
         if (m.Msg == WmHotkey && m.WParam.ToInt32() == HotkeyToggle)
            this.ToggleLens();
         base.WndProc(ref m);
      }

      private void ToggleLens()
      {
         if (this.activeLens != null)
         {
            this.activeLens.Close();
            return;
         }
         var lensForm = new LensForm { TargetLocation = Cursor.Position };
         lensForm.FormClosed += (s, e) => this.activeLens = null;
         this.activeLens = lensForm;
         lensForm.Show();
         lensForm.Activate();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         this.colorGrid.Color = this.valueGridColor.BackColor;
         if (this.colorGrid.ShowDialog() == DialogResult.OK)
            this.valueGridColor.BackColor = this.colorGrid.Color;
      }

      private void menuItemExit_Click(object sender, EventArgs e)
      {
         this.shouldExitApplication = true;
         this.Close();
         Application.Exit();
      }

      private void menuItemOpen_Click(object sender, EventArgs e)
      {
         this.ToggleLens();
      }

      private void menuItemSettings_Click(object sender, EventArgs e)
      {
         this.OpenSettings(); // Click the context menu item
      }

      private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Left) return;


         this.clickCount++;
         this.clickTimer.Stop();
         this.clickTimer.Interval = SystemInformation.DoubleClickTime;
         this.clickTimer.Start();
      }

      private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (!this.shouldExitApplication)
         {
            this.CloseToSystemTray(e);
         }
         else
         {
            Console.WriteLine("Exiting Application");
         }
      }

      private void CloseToSystemTray(CancelEventArgs e)
      {
         Console.WriteLine("Closing to System Tray");
         e.Cancel = true;
         this.Hide();
      }
   }
}