namespace Lens
{
   partial class SettingsForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
         this.valueAutoClose = new System.Windows.Forms.CheckBox();
         this.valueHideCursor = new System.Windows.Forms.CheckBox();
         this.valueMagnification = new System.Windows.Forms.NumericUpDown();
         this.buttonShow = new System.Windows.Forms.Button();
         this.labelMagnification = new System.Windows.Forms.Label();
         this.valueNearestNeighbor = new System.Windows.Forms.CheckBox();
         this.labelSpeedFactor = new System.Windows.Forms.Label();
         this.valueSpeedFactor = new System.Windows.Forms.NumericUpDown();
         this.colorGrid = new System.Windows.Forms.ColorDialog();
         this.valueGridColor = new System.Windows.Forms.Button();
         this.groupGrid = new System.Windows.Forms.GroupBox();
         this.valueGridSize = new System.Windows.Forms.NumericUpDown();
         this.labelGridSize = new System.Windows.Forms.Label();
         this.labelGridColor = new System.Windows.Forms.Label();
         this.labelGridStyle = new System.Windows.Forms.Label();
         this.valueGridStyle = new System.Windows.Forms.ComboBox();
         this.groupZoom = new System.Windows.Forms.GroupBox();
         this.iconPicture = new System.Windows.Forms.PictureBox();
         this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
         this.contextMenu = new System.Windows.Forms.ContextMenu();
         ((System.ComponentModel.ISupportInitialize)(this.valueMagnification)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.valueSpeedFactor)).BeginInit();
         this.groupGrid.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.valueGridSize)).BeginInit();
         this.groupZoom.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).BeginInit();
         this.SuspendLayout();
         // 
         // valueAutoClose
         // 
         this.valueAutoClose.Checked = true;
         this.valueAutoClose.CheckState = System.Windows.Forms.CheckState.Checked;
         this.valueAutoClose.Location = new System.Drawing.Point(88, 207);
         this.valueAutoClose.Name = "valueAutoClose";
         this.valueAutoClose.Size = new System.Drawing.Size(152, 20);
         this.valueAutoClose.TabIndex = 1;
         this.valueAutoClose.Text = "Auto Close";
         this.valueAutoClose.UseVisualStyleBackColor = true;
         // 
         // valueHideCursor
         // 
         this.valueHideCursor.Checked = true;
         this.valueHideCursor.CheckState = System.Windows.Forms.CheckState.Checked;
         this.valueHideCursor.Location = new System.Drawing.Point(10, 40);
         this.valueHideCursor.Name = "valueHideCursor";
         this.valueHideCursor.Size = new System.Drawing.Size(136, 20);
         this.valueHideCursor.TabIndex = 2;
         this.valueHideCursor.Text = "Hide Cursor";
         this.valueHideCursor.UseVisualStyleBackColor = true;
         // 
         // valueMagnification
         // 
         this.valueMagnification.Location = new System.Drawing.Point(106, 18);
         this.valueMagnification.Name = "valueMagnification";
         this.valueMagnification.Size = new System.Drawing.Size(40, 20);
         this.valueMagnification.TabIndex = 5;
         this.valueMagnification.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // buttonShow
         // 
         this.buttonShow.Location = new System.Drawing.Point(12, 87);
         this.buttonShow.Name = "buttonShow";
         this.buttonShow.Size = new System.Drawing.Size(64, 140);
         this.buttonShow.TabIndex = 0;
         this.buttonShow.Text = "Open";
         this.buttonShow.UseVisualStyleBackColor = true;
         this.buttonShow.Click += new System.EventHandler(this.button_Show_Click);
         // 
         // labelMagnification
         // 
         this.labelMagnification.Location = new System.Drawing.Point(6, 20);
         this.labelMagnification.Name = "labelMagnification";
         this.labelMagnification.Size = new System.Drawing.Size(94, 20);
         this.labelMagnification.TabIndex = 4;
         this.labelMagnification.Text = "Magnification";
         // 
         // valueNearestNeighbor
         // 
         this.valueNearestNeighbor.Checked = true;
         this.valueNearestNeighbor.CheckState = System.Windows.Forms.CheckState.Checked;
         this.valueNearestNeighbor.Location = new System.Drawing.Point(10, 62);
         this.valueNearestNeighbor.Name = "valueNearestNeighbor";
         this.valueNearestNeighbor.Size = new System.Drawing.Size(136, 20);
         this.valueNearestNeighbor.TabIndex = 3;
         this.valueNearestNeighbor.Text = "Nearest Neighbor";
         this.valueNearestNeighbor.UseVisualStyleBackColor = true;
         // 
         // labelSpeedFactor
         // 
         this.labelSpeedFactor.Location = new System.Drawing.Point(6, 86);
         this.labelSpeedFactor.Name = "labelSpeedFactor";
         this.labelSpeedFactor.Size = new System.Drawing.Size(72, 20);
         this.labelSpeedFactor.TabIndex = 6;
         this.labelSpeedFactor.Text = "Speed Factor";
         // 
         // valueSpeedFactor
         // 
         this.valueSpeedFactor.Location = new System.Drawing.Point(106, 84);
         this.valueSpeedFactor.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
         this.valueSpeedFactor.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
         this.valueSpeedFactor.Name = "valueSpeedFactor";
         this.valueSpeedFactor.Size = new System.Drawing.Size(40, 20);
         this.valueSpeedFactor.TabIndex = 7;
         this.valueSpeedFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.valueSpeedFactor.Value = new decimal(new int[] { 4, 0, 0, 0 });
         // 
         // colorGrid
         // 
         this.colorGrid.AnyColor = true;
         this.colorGrid.FullOpen = true;
         this.colorGrid.SolidColorOnly = true;
         // 
         // valueGridColor
         // 
         this.valueGridColor.BackColor = System.Drawing.Color.Black;
         this.valueGridColor.Location = new System.Drawing.Point(126, 46);
         this.valueGridColor.Name = "valueGridColor";
         this.valueGridColor.Size = new System.Drawing.Size(20, 20);
         this.valueGridColor.TabIndex = 8;
         this.valueGridColor.UseVisualStyleBackColor = false;
         this.valueGridColor.Click += new System.EventHandler(this.button1_Click);
         // 
         // groupGrid
         // 
         this.groupGrid.Controls.Add(this.valueGridSize);
         this.groupGrid.Controls.Add(this.labelGridSize);
         this.groupGrid.Controls.Add(this.labelGridColor);
         this.groupGrid.Controls.Add(this.labelGridStyle);
         this.groupGrid.Controls.Add(this.valueGridStyle);
         this.groupGrid.Controls.Add(this.valueGridColor);
         this.groupGrid.Location = new System.Drawing.Point(88, 129);
         this.groupGrid.Name = "groupGrid";
         this.groupGrid.Size = new System.Drawing.Size(152, 72);
         this.groupGrid.TabIndex = 10;
         this.groupGrid.TabStop = false;
         this.groupGrid.Text = "Grid";
         // 
         // valueGridSize
         // 
         this.valueGridSize.Location = new System.Drawing.Point(42, 46);
         this.valueGridSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
         this.valueGridSize.Name = "valueGridSize";
         this.valueGridSize.Size = new System.Drawing.Size(40, 20);
         this.valueGridSize.TabIndex = 15;
         this.valueGridSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
         // 
         // labelGridSize
         // 
         this.labelGridSize.Location = new System.Drawing.Point(6, 48);
         this.labelGridSize.Name = "labelGridSize";
         this.labelGridSize.Size = new System.Drawing.Size(30, 20);
         this.labelGridSize.TabIndex = 14;
         this.labelGridSize.Text = "Size";
         // 
         // labelGridColor
         // 
         this.labelGridColor.Location = new System.Drawing.Point(88, 48);
         this.labelGridColor.Name = "labelGridColor";
         this.labelGridColor.Size = new System.Drawing.Size(32, 20);
         this.labelGridColor.TabIndex = 13;
         this.labelGridColor.Text = "Color";
         // 
         // labelGridStyle
         // 
         this.labelGridStyle.Location = new System.Drawing.Point(6, 22);
         this.labelGridStyle.Name = "labelGridStyle";
         this.labelGridStyle.Size = new System.Drawing.Size(30, 20);
         this.labelGridStyle.TabIndex = 12;
         this.labelGridStyle.Text = "Style";
         // 
         // valueGridStyle
         // 
         this.valueGridStyle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
         this.valueGridStyle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
         this.valueGridStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.valueGridStyle.FormattingEnabled = true;
         this.valueGridStyle.Location = new System.Drawing.Point(42, 18);
         this.valueGridStyle.Name = "valueGridStyle";
         this.valueGridStyle.Size = new System.Drawing.Size(104, 21);
         this.valueGridStyle.TabIndex = 11;
         // 
         // groupZoom
         // 
         this.groupZoom.Controls.Add(this.valueHideCursor);
         this.groupZoom.Controls.Add(this.valueNearestNeighbor);
         this.groupZoom.Controls.Add(this.labelSpeedFactor);
         this.groupZoom.Controls.Add(this.valueMagnification);
         this.groupZoom.Controls.Add(this.valueSpeedFactor);
         this.groupZoom.Controls.Add(this.labelMagnification);
         this.groupZoom.Location = new System.Drawing.Point(88, 12);
         this.groupZoom.Name = "groupZoom";
         this.groupZoom.Size = new System.Drawing.Size(152, 111);
         this.groupZoom.TabIndex = 11;
         this.groupZoom.TabStop = false;
         this.groupZoom.Text = "Zoom";
         // 
         // iconPicture
         //
         this.iconPicture.Location = new System.Drawing.Point(12, 12);
         this.iconPicture.Name = "iconPicture";
         this.iconPicture.Size = new System.Drawing.Size(64, 64);
         this.iconPicture.TabIndex = 12;
         this.iconPicture.TabStop = false;
         this.iconPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         // 
         // notifyIcon
         // 
         this.notifyIcon.Text = "Lens";
         this.notifyIcon.Visible = true;
         this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
         this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
         // 
         // SettingsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.ClientSize = new System.Drawing.Size(252, 237);
         this.Controls.Add(this.iconPicture);
         this.Controls.Add(this.groupZoom);
         this.Controls.Add(this.groupGrid);
         this.Controls.Add(this.buttonShow);
         this.Controls.Add(this.valueAutoClose);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Location = new System.Drawing.Point(15, 15);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "SettingsForm";
         this.Text = "Lens";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
         ((System.ComponentModel.ISupportInitialize)(this.valueMagnification)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.valueSpeedFactor)).EndInit();
         this.groupGrid.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.valueGridSize)).EndInit();
         this.groupZoom.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).EndInit();
         this.ResumeLayout(false);
      }

      private System.Windows.Forms.ContextMenu contextMenu;

      private System.Windows.Forms.NotifyIcon notifyIcon;

      private System.Windows.Forms.ComboBox valueGridStyle;

      private System.Windows.Forms.PictureBox iconPicture;

      private System.Windows.Forms.GroupBox groupZoom;

      private System.Windows.Forms.Label labelGridStyle;
      private System.Windows.Forms.Label labelGridColor;
      private System.Windows.Forms.Label labelGridSize;
      private System.Windows.Forms.NumericUpDown valueGridSize;

      private System.Windows.Forms.GroupBox groupGrid;

      private System.Windows.Forms.ColorDialog colorGrid;
      private System.Windows.Forms.Button valueGridColor;

      private System.Windows.Forms.NumericUpDown valueSpeedFactor;
      private System.Windows.Forms.Label labelSpeedFactor;

      private System.Windows.Forms.Label labelMagnification;
      private System.Windows.Forms.CheckBox valueNearestNeighbor;

      private System.Windows.Forms.NumericUpDown valueMagnification;
      private System.Windows.Forms.Button buttonShow;

      private System.Windows.Forms.CheckBox valueAutoClose;
      private System.Windows.Forms.CheckBox valueHideCursor;

      #endregion
   }
}
