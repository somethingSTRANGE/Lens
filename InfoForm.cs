// -------------------------------------------------------------------------------------
// <copyright file="InfoForm.cs" company="Greyborn Studios LLC">
//   Copyright 2015-2026 Greyborn Studios LLC. All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lens
{
   /// <summary>
   ///   Layered, non-activatable, click-through overlay that displays color and cursor info
   ///   beside the lens window. Rendered via <c>UpdateLayeredWindow</c> with the same
   ///   Gaussian drop shadow used by <see cref="LensForm"/>.
   /// </summary>
   internal class InfoForm : Form
   {
      // ── Shadow parameters — must match LensForm. ───────────────────────────────────────
      private const int   ShadowBlur     = 16;
      private const float ShadowSigma    = 4.5f;
      private const int   ShadowOffsetX  = 0;
      private const int   ShadowOffsetY  = 6;
      private const byte  ShadowMaxAlpha = 160;
      private const int   ShadowMarginL  = ShadowBlur;
      private const int   ShadowMarginT  = ShadowBlur - ShadowOffsetY;  // 10
      private const int   ShadowMarginR  = ShadowBlur;
      private const int   ShadowMarginB  = ShadowBlur + ShadowOffsetY;  // 22

      // ── Fixed content dimensions. ───────────────────────────────────────────────────────
      internal const int ContentW = 215;
      internal const int ContentH = 128;

      /// <summary>Horizontal gap (px) between the lens content edge and this panel.</summary>
      internal const int PanelGap = 20;

      // ── Win32 interop. ─────────────────────────────────────────────────────────────────
      private const uint ULW_ALPHA = 0x00000002;

      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      private struct BLENDFUNCTION
      {
         public byte BlendOp, BlendFlags, SourceConstantAlpha, AlphaFormat;
      }

      [StructLayout(LayoutKind.Sequential)]
      private struct BITMAPINFOHEADER
      {
         public uint biSize; public int biWidth, biHeight;
         public ushort biPlanes, biBitCount; public uint biCompression;
         public uint biSizeImage; public int biXPelsPerMeter, biYPelsPerMeter;
         public uint biClrUsed, biClrImportant;
      }

      [StructLayout(LayoutKind.Sequential)]
      private struct BITMAPINFO { public BITMAPINFOHEADER bmiHeader; }

      [DllImport("Gdi32.dll", ExactSpelling = true, SetLastError = true)]
      private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
      [DllImport("Gdi32.dll", ExactSpelling = true, SetLastError = true)]
      private static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint usage,
         out IntPtr ppvBits, IntPtr hSection, uint offset);
      [DllImport("Gdi32.dll", ExactSpelling = true)]
      private static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);
      [DllImport("Gdi32.dll", ExactSpelling = true, SetLastError = true)]
      private static extern bool DeleteDC(IntPtr hdc);
      [DllImport("Gdi32.dll", ExactSpelling = true, SetLastError = true)]
      private static extern bool DeleteObject(IntPtr hobj);
      [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
      private static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst,
         ref Size psize, IntPtr hdcSrc, ref Point pptSrc, uint crKey,
         ref BLENDFUNCTION pblend, uint dwFlags);
      [DllImport("user32.dll", SetLastError = true)]
      private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
         int x, int y, int cx, int cy, uint uFlags);

      // ── State. ─────────────────────────────────────────────────────────────────────────
      private readonly InfoControl infoData;
      private Font valueFont;

      // Content DC — ContentW × ContentH.
      private IntPtr layeredMemDC = IntPtr.Zero;
      private IntPtr layeredBitmap = IntPtr.Zero;
      private IntPtr layeredBits = IntPtr.Zero;

      // Final (shadow + content) DC — totalW × totalH.
      private IntPtr finalMemDC = IntPtr.Zero;
      private IntPtr finalBitmap = IntPtr.Zero;
      private IntPtr finalBits = IntPtr.Zero;
      private int finalW, finalH;

      private byte[] shadowAlpha;
      private int cachedShadowContentW = -1, cachedShadowContentH = -1;

      // ── Constructor. ───────────────────────────────────────────────────────────────────
      public InfoForm(InfoControl infoData)
      {
         this.infoData = infoData;
         this.FormBorderStyle = FormBorderStyle.None;
         this.ShowInTaskbar   = false;
         this.StartPosition   = FormStartPosition.Manual;
         // Window size includes shadow margins on all sides.
         this.ClientSize = new Size(ContentW + ShadowMarginL + ShadowMarginR,
                                    ContentH + ShadowMarginT + ShadowMarginB);
         // Start off-screen; shown lazily by UpdateAndPosition after first position set.
         this.Location = new Point(-32000, -32000);
         this.valueFont = new Font("JetBrains Mono", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
      }

      // Never activates on Show() — must remain true for the window to be non-activatable.
      protected override bool ShowWithoutActivation => true;

      protected override CreateParams CreateParams
      {
         get
         {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x00080000; // WS_EX_LAYERED — required for UpdateLayeredWindow
            cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE — never activated by the OS
            return cp;
         }
      }

      // Fully non-interactive: mouse input falls through to whatever is underneath,
      // and the window cannot be activated by click.
      protected override void WndProc(ref Message m)
      {
         const int WM_MOUSEACTIVATE = 0x0021;
         const int WM_NCHITTEST     = 0x0084;
         const int MA_NOACTIVATE    = 3;
         const int HTTRANSPARENT    = -1;
         switch (m.Msg)
         {
            case WM_MOUSEACTIVATE: m.Result = (IntPtr)MA_NOACTIVATE; return;
            case WM_NCHITTEST:     m.Result = (IntPtr)HTTRANSPARENT; return;
         }
         base.WndProc(ref m);
      }

      protected override void OnClosing(CancelEventArgs e)
      {
         this.valueFont?.Dispose();
         this.valueFont = null;
         this.FreeLayeredResources();
         this.FreeFinalResources();
         this.shadowAlpha = null;
         base.OnClosing(e);
      }

      // ── Public update entry-point, called each frame from LensForm.RenderFrame. ────────
      public void UpdateAndPosition(Point cursorPos, Color color, Rectangle contentBounds)
      {
         this.infoData.UpdateInfo(cursorPos, color);

         // Info mirrors the lens side — already flipped by LensForm.RenderFrame.
         bool lensIsRightOfCursor = contentBounds.Left >= cursorPos.X;
         int infoContentLeft = lensIsRightOfCursor
            ? contentBounds.Right + PanelGap
            : contentBounds.Left - PanelGap - ContentW;
         int infoContentTop = contentBounds.Top;

         int totalW = ContentW + ShadowMarginL + ShadowMarginR;
         int totalH = ContentH + ShadowMarginT + ShadowMarginB;
         var winPos = new Point(infoContentLeft - ShadowMarginL, infoContentTop - ShadowMarginT);

         this.EnsureLayeredResources();
         this.EnsureFinalResources(totalW, totalH);
         this.RenderContent();
         this.CompositeFinalFrame(totalW, totalH);
         this.CommitLayeredWindow(winPos, totalW, totalH);

         if (!this.Visible)
         {
            // SetWindowPos SWP_NOACTIVATE | SWP_SHOWWINDOW — guaranteed no activation.
            const uint SWP_NOSIZE     = 0x0001;
            const uint SWP_NOMOVE     = 0x0002;
            const uint SWP_NOACTIVATE = 0x0010;
            const uint SWP_SHOWWINDOW = 0x0040;
            SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0,
               SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
         }
      }

      // ── Rendering. ─────────────────────────────────────────────────────────────────────

      private void RenderContent()
      {
         using var g = Graphics.FromHdc(this.layeredMemDC);
         g.TextRenderingHint = TextRenderingHint.AntiAlias;
         g.SmoothingMode     = SmoothingMode.HighQuality;

         // Background: diagonal gradient, black → #333333.
         var bgRect = new Rectangle(0, 0, ContentW, ContentH);
         using (var bg = new LinearGradientBrush(bgRect, Color.Black, Color.FromArgb(51, 51, 51), 45f))
            g.FillRectangle(bg, bgRect);

         var d          = this.infoData;
         var labelFont  = SystemFonts.DefaultFont;
         var labelColor = Color.FromArgb(0xFF, 0xFF, 0xE1); // SystemColors.Info default

         using var labelBrush = new SolidBrush(labelColor);
         using var valueBrush = new SolidBrush(Color.White);
         var ellipsis = new StringFormat { Trimming = StringTrimming.EllipsisCharacter,
                                           FormatFlags = StringFormatFlags.NoWrap };

         void Row(string label, string value, int labelY, int valueY, int valueW = 128)
         {
            g.DrawString(label, labelFont, labelBrush, 4f, labelY);
            g.DrawString(value, this.valueFont, valueBrush,
               new RectangleF(50, valueY, valueW, 16), ellipsis);
         }

         Row("HEX",   d.ValueColorHex,  6,   4,  128);
         Row("RGB",   d.ValueColorRGB,   22,  20);
         Row("HSL",   d.ValueColorHSL,   38,  36);

         if (d.HasColorName)
            Row("Name", d.ValueColorName, 54, 52);

         Row("Mouse", d.MousePosition,   76,  74);
         Row("Size",  d.LensSize,         92,  90);
         Row("Zoom",  d.ZoomFactor,       108, 106);

         // Color swatch — 24×24 filled square with 1px border at (181, 7).
         var swatch = new Rectangle(181, 7, 24, 24);
         using (var swatchBrush = new SolidBrush(d.SwatchColor))
            g.FillRectangle(swatchBrush, swatch);
         g.DrawRectangle(Pens.Black, swatch.X, swatch.Y, swatch.Width - 1, swatch.Height - 1);
      }

      // ── GDI resource management (mirrors LensForm). ────────────────────────────────────

      private void EnsureLayeredResources()
      {
         if (this.layeredMemDC == IntPtr.Zero)
            this.layeredMemDC = CreateCompatibleDC(IntPtr.Zero);
         if (this.layeredBitmap != IntPtr.Zero) return;

         var bmi = MakeBmi(ContentW, ContentH);
         this.layeredBitmap = CreateDIBSection(IntPtr.Zero, ref bmi, 0, out this.layeredBits, IntPtr.Zero, 0);
         SelectObject(this.layeredMemDC, this.layeredBitmap);
      }

      private void FreeLayeredResources()
      {
         if (this.layeredBitmap != IntPtr.Zero)
         {
            DeleteObject(this.layeredBitmap);
            this.layeredBitmap = IntPtr.Zero;
            this.layeredBits   = IntPtr.Zero;
         }
         if (this.layeredMemDC != IntPtr.Zero) { DeleteDC(this.layeredMemDC); this.layeredMemDC = IntPtr.Zero; }
      }

      private void EnsureFinalResources(int w, int h)
      {
         if (this.finalMemDC == IntPtr.Zero)
            this.finalMemDC = CreateCompatibleDC(IntPtr.Zero);
         if (this.finalBitmap != IntPtr.Zero && this.finalW == w && this.finalH == h) return;

         if (this.finalBitmap != IntPtr.Zero) { DeleteObject(this.finalBitmap); this.finalBits = IntPtr.Zero; }
         var bmi = MakeBmi(w, h);
         this.finalBitmap = CreateDIBSection(IntPtr.Zero, ref bmi, 0, out this.finalBits, IntPtr.Zero, 0);
         SelectObject(this.finalMemDC, this.finalBitmap);
         this.finalW = w;
         this.finalH = h;
      }

      private void FreeFinalResources()
      {
         if (this.finalBitmap != IntPtr.Zero) { DeleteObject(this.finalBitmap); this.finalBitmap = IntPtr.Zero; this.finalBits = IntPtr.Zero; }
         if (this.finalMemDC  != IntPtr.Zero) { DeleteDC(this.finalMemDC);  this.finalMemDC  = IntPtr.Zero; }
      }

      private static BITMAPINFO MakeBmi(int w, int h) => new BITMAPINFO
      {
         bmiHeader = new BITMAPINFOHEADER
         {
            biSize        = (uint)Marshal.SizeOf<BITMAPINFOHEADER>(),
            biWidth       = w,
            biHeight      = -h, // negative = top-down
            biPlanes      = 1,
            biBitCount    = 32,
            biCompression = 0   // BI_RGB
         }
      };

      private void CompositeFinalFrame(int tw, int th)
      {
         if (this.finalBits == IntPtr.Zero || this.layeredBits == IntPtr.Zero) return;

         this.EnsureShadow();

         // Clear final to transparent.
         int totalPx = tw * th;
         for (int i = 0; i < totalPx; i++)
            Marshal.WriteInt32(this.finalBits, i * 4, 0);

         // Write shadow (pre-multiplied black with per-pixel alpha).
         var alpha = this.shadowAlpha;
         for (int i = 0; i < totalPx; i++)
         {
            byte a = alpha[i];
            if (a > 0)
               Marshal.WriteInt32(this.finalBits, i * 4, unchecked((int)((uint)a << 24)));
         }

         // Stamp content at (ShadowMarginL, ShadowMarginT), forcing alpha=255.
         int cStride = ContentW * 4;
         int fStride = tw * 4;
         for (int y = 0; y < ContentH; y++)
         for (int x = 0; x < ContentW; x++)
         {
            int src = Marshal.ReadInt32(this.layeredBits, y * cStride + x * 4);
            int dst = (src & 0x00FFFFFF) | unchecked((int)0xFF000000u);
            Marshal.WriteInt32(this.finalBits, (y + ShadowMarginT) * fStride + (x + ShadowMarginL) * 4, dst);
         }
      }

      private void CommitLayeredWindow(Point winPos, int w, int h)
      {
         var winSize = new Size(w, h);
         var srcPos  = Point.Empty;
         var blend   = new BLENDFUNCTION { BlendOp = 0, BlendFlags = 0, SourceConstantAlpha = 255, AlphaFormat = 1 };
         UpdateLayeredWindow(this.Handle, IntPtr.Zero, ref winPos, ref winSize,
            this.finalMemDC, ref srcPos, 0, ref blend, ULW_ALPHA);
      }

      // ── Shadow (Gaussian blur). ────────────────────────────────────────────────────────

      private void EnsureShadow()
      {
         if (this.shadowAlpha != null &&
             this.cachedShadowContentW == ContentW &&
             this.cachedShadowContentH == ContentH)
            return;

         this.cachedShadowContentW = ContentW;
         this.cachedShadowContentH = ContentH;

         int tw = ContentW + ShadowMarginL + ShadowMarginR;
         int th = ContentH + ShadowMarginT + ShadowMarginB;

         int sx = ShadowMarginL + ShadowOffsetX;
         int sy = ShadowMarginT + ShadowOffsetY;
         var src = new float[tw * th];
         for (int y = sy; y < sy + ContentH; y++)
         for (int x = sx; x < sx + ContentW; x++)
            src[y * tw + x] = 1f;

         var temp   = GaussianBlur1D(src,  tw, th, ShadowSigma, horizontal: true);
         var result = GaussianBlur1D(temp, tw, th, ShadowSigma, horizontal: false);

         this.shadowAlpha = new byte[tw * th];
         for (int i = 0; i < result.Length; i++)
            this.shadowAlpha[i] = (byte)Math.Round(result[i] * ShadowMaxAlpha);
      }

      private static float[] GaussianBlur1D(float[] src, int w, int h, float sigma, bool horizontal)
      {
         int radius = (int)Math.Ceiling(sigma * 3);
         var kernel = new float[2 * radius + 1];
         float sum = 0;
         for (int i = -radius; i <= radius; i++)
         {
            kernel[i + radius] = (float)Math.Exp(-i * i / (2.0 * sigma * sigma));
            sum += kernel[i + radius];
         }
         for (int i = 0; i < kernel.Length; i++) kernel[i] /= sum;

         var dst = new float[w * h];
         if (horizontal)
         {
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
               float val = 0;
               for (int k = -radius; k <= radius; k++)
               {
                  int xx = x + k;
                  if (xx >= 0 && xx < w) val += src[y * w + xx] * kernel[k + radius];
               }
               dst[y * w + x] = val;
            }
         }
         else
         {
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
               float val = 0;
               for (int k = -radius; k <= radius; k++)
               {
                  int yy = y + k;
                  if (yy >= 0 && yy < h) val += src[yy * w + x] * kernel[k + radius];
               }
               dst[y * w + x] = val;
            }
         }
         return dst;
      }
   }
}
