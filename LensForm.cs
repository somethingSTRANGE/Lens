// -------------------------------------------------------------------------------------
// <copyright file="LensForm.cs" company="Strange Entertainment LLC">
//   Copyright 2004-2023 Strange Entertainment LLC. All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PopupControl;

namespace Lens
{
   public partial class LensForm : Form
   {
      [SuppressMessage("ReSharper", "IdentifierTypo")] [SuppressMessage("ReSharper", "InconsistentNaming")]
      private const uint SPI_GETMOUSESPEED = 0x0070;

      [SuppressMessage("ReSharper", "IdentifierTypo")] [SuppressMessage("ReSharper", "InconsistentNaming")]
      private const uint SPI_SETMOUSESPEED = 0x0071;

      private readonly Timer timer;

      private int baseMouseSpeed;

      private InfoControl infoControl;

      private Popup infoPopup;

      private bool mouseDown;

      private Bitmap scrBmp;

      private Graphics scrGrp;

      private Point targetLocation;

      public LensForm()
      {
         this.InitializeComponent();

         this.SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.Opaque |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint, true);
         this.UpdateStyles();

         this.FormBorderStyle = FormBorderStyle.FixedSingle;
         this.ControlBox = false;
         this.ShowInTaskbar = false;
         this.TopMost = true;
         this.StartPosition = FormStartPosition.Manual;

         this.targetLocation = Cursor.Position;
         this.ApplyWidth();
         this.ApplyHeight();

         this.CopyScreen();

         this.baseMouseSpeed = SystemInformation.MouseSpeed;

         this.timer = new Timer { Interval = 55, Enabled = true };
         this.timer.Tick += (s, e) => this.Invalidate();

         this.infoPopup = new Popup(this.infoControl = new InfoControl());
         this.infoPopup.AutoClose = false;
         this.infoPopup.FocusOnOpen = false;
         this.infoPopup.ShowingAnimation = this.infoPopup.HidingAnimation = PopupAnimations.None;
      }

      public Point TargetLocation
      {
         get => this.targetLocation;
         set => this.targetLocation = value;
      }

      [DllImport("User32.dll")]
      private static extern bool
         SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

      public static void CursorHide()
      {
         Cursor.Hide();
      }

      public static void CursorShow()
      {
         Cursor.Show();
      }

      protected override void OnClosing(CancelEventArgs e)
      {
         this.ResetMouseSpeed();
         if (Lens.Instance.HideCursor) CursorShow();
         base.OnClosing(e);
      }

      protected override void OnMouseDown(MouseEventArgs e)
      {
         base.OnMouseDown(e);

         if (e.Button == MouseButtons.Left)
         {
            this.mouseDown = true;
            if (Lens.Instance.HideCursor) CursorHide();
         }
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
         base.OnMouseMove(e);
         this.TargetLocation = e.Location;
         this.Invalidate();
      }

      protected override void OnMouseUp(MouseEventArgs e)
      {
         base.OnMouseUp(e);

         this.mouseDown = false;
         if (Lens.Instance.HideCursor) CursorShow();
         if (Lens.Instance.AutoClose)
         {
            this.infoPopup.Close();
            this.Close();
         }
      }

      protected override void OnMouseWheel(MouseEventArgs e)
      {
         base.OnMouseWheel(e);

         switch (ModifierKeys)
         {
            case Keys.None:
               if (e.Delta > 0)
                  this.ChangeMagnification(1);
               else
                  this.ChangeMagnification(-1);
               break;
            case Keys.Control:
               if (e.Delta > 0)
                  this.IncreaseSize();
               else
                  this.DecreaseSize();
               break;
            case Keys.Alt:
               if (e.Delta > 0)
                  this.IncreaseGridSize();
               else
                  this.DecreaseGridSize();
               break;
         }
      }

      protected override void OnKeyDown(KeyEventArgs e)
      {
         base.OnKeyDown(e);

         switch (e.KeyCode)
         {
            case Keys.Right:
               this.ChangePosition(1, 0);
               break;
            case Keys.Up:
               this.ChangePosition(0, -1);
               break;
            case Keys.Down:
               this.ChangePosition(0, 1);
               break;
            case Keys.Left:
               this.ChangePosition(-1, 0);
               break;
            default:
               Debug.WriteLine("1: " + e + ", " + e.Handled + ", " + e.GetType() + ", " + e.KeyCode + ", " + e.KeyData + ", " + e.KeyValue);
               break;
         }
      }

      protected override void OnKeyPress(KeyPressEventArgs e)
      {
         base.OnKeyPress(e);

         Debug.WriteLine("2: " + e + ", " + e.Handled + ", " + e.GetType() + ", " + e.KeyChar + ", " + e.ToString());
      }

      protected override void OnKeyUp(KeyEventArgs e)
      {
         base.OnKeyUp(e);

         switch (e.KeyCode)
         {
            case Keys.Escape:
               this.infoPopup.Close();
               this.Close();
               break;
            case Keys.OemMinus:
               this.ChangeMagnification(-1);
               break;
            case Keys.Oemplus:
               this.ChangeMagnification(1);
               break;
            case Keys.OemOpenBrackets:
               this.ChangeWidth(-Lens.Defaults.SizeIncrement);
               break;
            case Keys.OemCloseBrackets:
               this.ChangeWidth(Lens.Defaults.SizeIncrement);
               break;
            case Keys.OemSemicolon:
               this.ChangeHeight(-Lens.Defaults.SizeIncrement);
               break;
            case Keys.OemQuotes:
               this.ChangeHeight(Lens.Defaults.SizeIncrement);
               break;
            case Keys.H:
               this.CopyToClipboardColorHSL();
               break;
            case Keys.R:
               this.CopyToClipboardColorRGB();
               break;
            case Keys.X:
               this.CopyToClipboardColorHex();
               break;
            default:
               Debug.WriteLine((ModifierKeys == Keys.Control ? "CTRL-" : string.Empty) +
                               (ModifierKeys == Keys.Alt ? "ALT-" : string.Empty) +
                               (ModifierKeys == Keys.Shift ? "SHIFT-" : string.Empty) + e.KeyCode + " (" +
                               ModifierKeys + ")");
               break;
         }
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         if (this.mouseDown)
            this.SetLocation();
         else
            this.CopyScreen();

         var graphics = e.Graphics;
         var lens = Lens.Instance;
         var width = lens.Width / 2;
         var height = lens.Height / 2;
         graphics.TranslateTransform(width, height);
         graphics.ScaleTransform(lens.Magnification, lens.Magnification);

         var (x, y) = Cursor.Position;
         graphics.TranslateTransform(-x, -y);
         graphics.Clear(this.BackColor);

         if (lens.NearestNeighbor)
         {
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
         }

         if (this.scrBmp != null)
         {
            graphics.DrawImage(this.scrBmp, 0, 0);

            DrawOrigin(graphics);
            DrawGrid(graphics);

            var color = this.scrBmp.GetPixel(x, y);
            this.infoControl.UpdateInfo(this, this.infoPopup, Cursor.Position, color);
         }
      }

      private static void DrawOrigin(Graphics graphics)
      {
         var (x, y) = Cursor.Position;
         var (top, bottom, left, right, _, _) = graphics.ClipBounds;
         var pen = new Pen(Lens.Instance.GridColor, 1f / Lens.Instance.Magnification);

         // origin lines
         graphics.DrawLine(pen, left, y, right, y);
         graphics.DrawLine(pen, x, top, x, bottom);

         // box around target pixel
         graphics.DrawLine(pen, x, y + 1, x + 1, y + 1);
         graphics.DrawLine(pen, x + 1, y, x + 1, y + 1);

      }

      private static void DrawGrid(Graphics graphics)
      {
         var lens = Lens.Instance;
         var gridStyle = (GridStyleOptions)lens.GridStyle;
         if (gridStyle == GridStyleOptions.None) return;

         var (x, y) = Cursor.Position;
         var (top, bottom, left, right, width, height) = graphics.ClipBounds;
         var gridSize = lens.GridSize;
         var pen = new Pen(Color.FromArgb(0x33, lens.GridColor), 1f / lens.Magnification)
            {
               DashStyle = gridStyle.DashStyle()
            };

         for (var i = 1; i < height / 2 / gridSize; i++)
         {
            graphics.DrawLine(pen, left, y - i * gridSize, right, y - i * gridSize);
            graphics.DrawLine(pen, left, y + i * gridSize, right, y + i * gridSize);
         }

         for (var i = 1; i < width / 2 / gridSize; i++)
         {
            graphics.DrawLine(pen, x - i * gridSize, top, x - i * gridSize, bottom);
            graphics.DrawLine(pen, x + i * gridSize, top, x + i * gridSize, bottom);
         }
      }

      private void ApplyHeight()
      {
         this.Height = Lens.Instance.Height + 2;
         this.Top = this.TargetLocation.Y - this.Height / 2;
         this.SetRegion();
      }

      private void ApplyWidth()
      {
         this.Width = Lens.Instance.Width + 2;
         this.Left = this.TargetLocation.X - this.Width / 2;
         this.SetRegion();
      }

      protected override void OnShown(EventArgs e)
      {
         base.OnShown(e);

         this.SetRegion();
         this.SetMouseSpeed();

         this.Capture = true;
         this.mouseDown = true;
         if (Lens.Instance.HideCursor) CursorHide();
         this.infoPopup.Show(this);
         this.Invalidate();
      }

      private void CopyScreen()
      {
         if (this.scrBmp == null)
         {
            var rectangle = SystemInformation.VirtualScreen;
            var size = rectangle.Size;
            this.scrBmp = new Bitmap(size.Width, size.Height);
            this.scrGrp = Graphics.FromImage(this.scrBmp);
         }

         this.scrGrp.CopyFromScreen(Point.Empty, Point.Empty, this.scrBmp.Size);
         Debug.WriteLine(this.scrBmp.Size + ", " + this.scrGrp.ClipBounds + ", " +
                         this.scrGrp.VisibleClipBounds);
      }

      private void CopyToClipboardColorHex()
      {
         Clipboard.SetText(this.infoControl.ValueColorHex);
      }

      private void CopyToClipboardColorRGB()
      {
         Clipboard.SetText($"rgb({this.infoControl.ValueColorRGB})");
      }

      private void CopyToClipboardColorHSL()
      {
         Clipboard.SetText($"hsl({this.infoControl.ValueColorHSL})");
      }

      private void DecreaseGridSize()
      {
         Lens.Instance.GridSize--;
      }

      private void DecreaseSize()
      {
         Debug.WriteLine("DECREASE FORM SIZE KEEPING ASPECT RATIO");
      }

      private void IncreaseGridSize()
      {
         Lens.Instance.GridSize++;
      }

      private void IncreaseSize()
      {
         Debug.WriteLine("INCREASE FORM SIZE KEEPING ASPECT RATIO");
      }


      private void SetRegion()
      {
         var gp = new GraphicsPath();
         gp.AddRectangle(new Rectangle(0, 0, this.Width, this.Height));
         this.Region = new Region(gp);
      }

      private void ChangeHeight(short amount)
      {
         Lens.Instance.Height += amount;
         this.ApplyHeight();
      }

      private void ChangeMagnification(short amount)
      {
         Lens.Instance.Magnification = (byte)(Lens.Instance.Magnification + amount);
         this.SetMouseSpeed();
      }

      private void ChangePosition(int x, int y)
      {
         var p = Cursor.Position;
         p.X += x;
         p.Y += y;
         Cursor.Position = p;
      }

      private void ChangeWidth(short amount)
      {
         Lens.Instance.Width += amount;
         this.ApplyWidth();
      }

      private void SetLocation()
      {
         var p = Cursor.Position;

         this.Left = p.X - this.Width / 2;
         this.Top = p.Y - this.Height / 2;
      }

      private void ResetMouseSpeed()
      {
         SystemParametersInfo(SPI_SETMOUSESPEED, 0, (uint)this.baseMouseSpeed, 0);
      }

      private void SetMouseSpeed()
      {
         var factor = (float)(Lens.Instance.Magnification - Lens.Defaults.MinMagnification) /
                      (Lens.Defaults.MaxMagnification - Lens.Defaults.MinMagnification);
         var minSpeed = Math.Max(this.baseMouseSpeed / Lens.Instance.SpeedFactor,
            Lens.Defaults.MinMouseSpeed);
         var mouseSpeed = (uint)Math.Round((this.baseMouseSpeed - minSpeed) * (1 - factor) + minSpeed);

         // The pvParam (mouseSpeed) parameter must point to an integer that
         // receives a value which ranges between 1 (slowest) and 20 (fastest).
         // A value of 10 is the default.
         SystemParametersInfo(SPI_SETMOUSESPEED, 0, mouseSpeed, 0);
      }
   }
}
