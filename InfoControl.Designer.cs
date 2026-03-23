using System.ComponentModel;

namespace Lens
{
   partial class InfoControl
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }

         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.labelColorRGB = new System.Windows.Forms.Label();
         this.labelColorHex = new System.Windows.Forms.Label();
         this.labelColorHSL = new System.Windows.Forms.Label();
         this.labelMousePosition = new System.Windows.Forms.Label();
         this.valueMousePosition = new System.Windows.Forms.Label();
         this.valueColorHSL = new System.Windows.Forms.Label();
         this.valueColorHex = new System.Windows.Forms.Label();
         this.valueColorRGB = new System.Windows.Forms.Label();
         this.valueZoomFactor = new System.Windows.Forms.Label();
         this.labelZoomFactor = new System.Windows.Forms.Label();
         this.valueColorName = new System.Windows.Forms.Label();
         this.labelColorName = new System.Windows.Forms.Label();
         this.valueSize = new System.Windows.Forms.Label();
         this.labelSize = new System.Windows.Forms.Label();
         this.valueColor = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // labelColorRGB
         // 
         this.labelColorRGB.BackColor = System.Drawing.Color.Transparent;
         this.labelColorRGB.ForeColor = System.Drawing.SystemColors.Info;
         this.labelColorRGB.Location = new System.Drawing.Point(4, 22);
         this.labelColorRGB.Name = "labelColorRGB";
         this.labelColorRGB.Size = new System.Drawing.Size(40, 16);
         this.labelColorRGB.TabIndex = 0;
         this.labelColorRGB.Text = "RGB";
         // 
         // labelColorHex
         // 
         this.labelColorHex.BackColor = System.Drawing.Color.Transparent;
         this.labelColorHex.ForeColor = System.Drawing.SystemColors.Info;
         this.labelColorHex.Location = new System.Drawing.Point(4, 6);
         this.labelColorHex.Name = "labelColorHex";
         this.labelColorHex.Size = new System.Drawing.Size(40, 16);
         this.labelColorHex.TabIndex = 1;
         this.labelColorHex.Text = "HEX";
         // 
         // labelColorHSL
         // 
         this.labelColorHSL.BackColor = System.Drawing.Color.Transparent;
         this.labelColorHSL.ForeColor = System.Drawing.SystemColors.Info;
         this.labelColorHSL.Location = new System.Drawing.Point(4, 38);
         this.labelColorHSL.Name = "labelColorHSL";
         this.labelColorHSL.Size = new System.Drawing.Size(40, 16);
         this.labelColorHSL.TabIndex = 2;
         this.labelColorHSL.Text = "HSL";
         // 
         // labelMousePosition
         // 
         this.labelMousePosition.BackColor = System.Drawing.Color.Transparent;
         this.labelMousePosition.ForeColor = System.Drawing.SystemColors.Info;
         this.labelMousePosition.Location = new System.Drawing.Point(4, 76);
         this.labelMousePosition.Name = "labelMousePosition";
         this.labelMousePosition.Size = new System.Drawing.Size(48, 16);
         this.labelMousePosition.TabIndex = 3;
         this.labelMousePosition.Text = "Mouse";
         // 
         // valueMousePosition
         // 
         this.valueMousePosition.BackColor = System.Drawing.Color.Transparent;
         this.valueMousePosition.Font = new System.Drawing.Font("JetBrains Mono", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
         this.valueMousePosition.Location = new System.Drawing.Point(50, 74);
         this.valueMousePosition.Name = "valueMousePosition";
         this.valueMousePosition.Size = new System.Drawing.Size(160, 16);
         this.valueMousePosition.TabIndex = 7;
         this.valueMousePosition.Text = "0000, 0000";
         // 
         // valueColorHSL
         // 
         this.valueColorHSL.AutoEllipsis = true;
         this.valueColorHSL.BackColor = System.Drawing.Color.Transparent;
         this.valueColorHSL.Font = new System.Drawing.Font("JetBrains Mono", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
         this.valueColorHSL.Location = new System.Drawing.Point(50, 36);
         this.valueColorHSL.Name = "valueColorHSL";
         this.valueColorHSL.Size = new System.Drawing.Size(160, 16);
         this.valueColorHSL.TabIndex = 6;
         this.valueColorHSL.Text = "000.0, 00.0%, 00.0%";
         // 
         // valueColorHex
         // 
         this.valueColorHex.BackColor = System.Drawing.Color.Transparent;
         this.valueColorHex.Font = new System.Drawing.Font("JetBrains Mono", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
         this.valueColorHex.Location = new System.Drawing.Point(50, 4);
         this.valueColorHex.Name = "valueColorHex";
         this.valueColorHex.Size = new System.Drawing.Size(140, 16);
         this.valueColorHex.TabIndex = 5;
         this.valueColorHex.Text = "#336699";
         // 
         // valueColorRGB
         // 
         this.valueColorRGB.BackColor = System.Drawing.Color.Transparent;
         this.valueColorRGB.Font = new System.Drawing.Font("JetBrains Mono", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
         this.valueColorRGB.Location = new System.Drawing.Point(50, 20);
         this.valueColorRGB.Name = "valueColorRGB";
         this.valueColorRGB.Size = new System.Drawing.Size(160, 16);
         this.valueColorRGB.TabIndex = 4;
         this.valueColorRGB.Text = "000, 000, 000";
         // 
         // valueZoomFactor
         // 
         this.valueZoomFactor.BackColor = System.Drawing.Color.Transparent;
         this.valueZoomFactor.Font = new System.Drawing.Font("JetBrains Mono", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
         this.valueZoomFactor.Location = new System.Drawing.Point(50, 106);
         this.valueZoomFactor.Name = "valueZoomFactor";
         this.valueZoomFactor.Size = new System.Drawing.Size(160, 16);
         this.valueZoomFactor.TabIndex = 9;
         this.valueZoomFactor.Text = "x16";
         // 
         // labelZoomFactor
         // 
         this.labelZoomFactor.BackColor = System.Drawing.Color.Transparent;
         this.labelZoomFactor.ForeColor = System.Drawing.SystemColors.Info;
         this.labelZoomFactor.Location = new System.Drawing.Point(4, 108);
         this.labelZoomFactor.Name = "labelZoomFactor";
         this.labelZoomFactor.Size = new System.Drawing.Size(40, 16);
         this.labelZoomFactor.TabIndex = 8;
         this.labelZoomFactor.Text = "Zoom";
         // 
         // valueColorName
         // 
         this.valueColorName.AutoEllipsis = true;
         this.valueColorName.BackColor = System.Drawing.Color.Transparent;
         this.valueColorName.Font = new System.Drawing.Font("JetBrains Mono", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
         this.valueColorName.Location = new System.Drawing.Point(50, 52);
         this.valueColorName.Name = "valueColorName";
         this.valueColorName.Size = new System.Drawing.Size(160, 16);
         this.valueColorName.TabIndex = 11;
         this.valueColorName.Text = "Black";
         // 
         // labelColorName
         // 
         this.labelColorName.BackColor = System.Drawing.Color.Transparent;
         this.labelColorName.ForeColor = System.Drawing.SystemColors.Info;
         this.labelColorName.Location = new System.Drawing.Point(4, 54);
         this.labelColorName.Name = "labelColorName";
         this.labelColorName.Size = new System.Drawing.Size(40, 16);
         this.labelColorName.TabIndex = 10;
         this.labelColorName.Text = "Name";
         // 
         // valueSize
         // 
         this.valueSize.BackColor = System.Drawing.Color.Transparent;
         this.valueSize.Font = new System.Drawing.Font("JetBrains Mono", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
         this.valueSize.Location = new System.Drawing.Point(50, 90);
         this.valueSize.Name = "valueSize";
         this.valueSize.Size = new System.Drawing.Size(160, 16);
         this.valueSize.TabIndex = 13;
         this.valueSize.Text = "0000, 0000";
         // 
         // labelSize
         // 
         this.labelSize.BackColor = System.Drawing.Color.Transparent;
         this.labelSize.ForeColor = System.Drawing.SystemColors.Info;
         this.labelSize.Location = new System.Drawing.Point(4, 92);
         this.labelSize.Name = "labelSize";
         this.labelSize.Size = new System.Drawing.Size(48, 16);
         this.labelSize.TabIndex = 12;
         this.labelSize.Text = "Size";
         // 
         // valueColor
         // 
         this.valueColor.BackColor = System.Drawing.Color.White;
         this.valueColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.valueColor.Location = new System.Drawing.Point(181, 7);
         this.valueColor.Name = "valueColor";
         this.valueColor.Size = new System.Drawing.Size(24, 24);
         this.valueColor.TabIndex = 14;
         // 
         // InfoControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DimGray;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.valueColor);
         this.Controls.Add(this.valueSize);
         this.Controls.Add(this.labelSize);
         this.Controls.Add(this.valueColorName);
         this.Controls.Add(this.labelColorName);
         this.Controls.Add(this.valueZoomFactor);
         this.Controls.Add(this.labelZoomFactor);
         this.Controls.Add(this.valueMousePosition);
         this.Controls.Add(this.valueColorHSL);
         this.Controls.Add(this.valueColorHex);
         this.Controls.Add(this.valueColorRGB);
         this.Controls.Add(this.labelMousePosition);
         this.Controls.Add(this.labelColorHSL);
         this.Controls.Add(this.labelColorHex);
         this.Controls.Add(this.labelColorRGB);
         this.ForeColor = System.Drawing.SystemColors.HighlightText;
         this.Name = "InfoControl";
         this.Size = new System.Drawing.Size(215, 128);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.InfoControl_Paint);
         this.ResumeLayout(false);
      }

      private System.Windows.Forms.Label valueColor;

      private System.Windows.Forms.Label valueSize;
      private System.Windows.Forms.Label labelSize;

      private System.Windows.Forms.Label valueColorName;
      private System.Windows.Forms.Label labelColorName;

      private System.Windows.Forms.Label valueColorHSL;
      private System.Windows.Forms.Label valueMousePosition;

      private System.Windows.Forms.Label labelMousePosition;
      private System.Windows.Forms.Label valueColorHex;
      private System.Windows.Forms.Label valueColorRGB;

      private System.Windows.Forms.Label labelColorRGB;
      private System.Windows.Forms.Label labelColorHSL;

      private System.Windows.Forms.Label labelColorHex;

      private System.Windows.Forms.Label valueZoomFactor;
      private System.Windows.Forms.Label labelZoomFactor;

      #endregion
   }
}
