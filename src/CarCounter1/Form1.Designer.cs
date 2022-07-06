namespace CarCounter1
{
    partial class Canvas1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.BtnSync = new MetroFramework.Controls.MetroTile();
            this.BtnSave = new MetroFramework.Controls.MetroTile();
            this.BtnOpen = new MetroFramework.Controls.MetroTile();
            this.BtnStop = new MetroFramework.Controls.MetroTile();
            this.BtnStart = new MetroFramework.Controls.MetroTile();
            this.Fps = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            this.MainContainer.BackColor = System.Drawing.SystemColors.HighlightText;
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(10, 60);
            this.MainContainer.Margin = new System.Windows.Forms.Padding(50);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.MainContainer.Panel1.Controls.Add(this.BtnSync);
            this.MainContainer.Panel1.Controls.Add(this.BtnSave);
            this.MainContainer.Panel1.Controls.Add(this.BtnOpen);
            this.MainContainer.Panel1.Controls.Add(this.BtnStop);
            this.MainContainer.Panel1.Controls.Add(this.BtnStart);
            this.MainContainer.Panel1.Controls.Add(this.Fps);
            this.MainContainer.Panel1.Font = new System.Drawing.Font("Bahnschrift Condensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainContainer.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.MainContainer.Panel2.Controls.Add(this.splitContainer1);
            this.MainContainer.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.MainContainer.Size = new System.Drawing.Size(1244, 611);
            this.MainContainer.SplitterDistance = 276;
            this.MainContainer.TabIndex = 8;
            // 
            // BtnSync
            // 
            this.BtnSync.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSync.Location = new System.Drawing.Point(10, 281);
            this.BtnSync.Name = "BtnSync";
            this.BtnSync.Size = new System.Drawing.Size(256, 50);
            this.BtnSync.TabIndex = 19;
            this.BtnSync.Text = "Sync to Cloud";
            this.BtnSync.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnSync.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.BtnSync.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.BtnSync.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(10, 214);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(256, 50);
            this.BtnSave.TabIndex = 18;
            this.BtnSave.Text = "Save Log";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnSave.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.BtnSave.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.BtnSave.UseVisualStyleBackColor = true;
            // 
            // BtnOpen
            // 
            this.BtnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOpen.Location = new System.Drawing.Point(10, 147);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(256, 50);
            this.BtnOpen.TabIndex = 18;
            this.BtnOpen.Text = "Open File (Dev only)";
            this.BtnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnOpen.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.BtnOpen.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.BtnOpen.UseVisualStyleBackColor = true;
            // 
            // BtnStop
            // 
            this.BtnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStop.Location = new System.Drawing.Point(10, 80);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(256, 50);
            this.BtnStop.TabIndex = 17;
            this.BtnStop.Text = "S&top";
            this.BtnStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnStop.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.BtnStop.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.BtnStop.UseVisualStyleBackColor = true;
            // 
            // BtnStart
            // 
            this.BtnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStart.Location = new System.Drawing.Point(10, 13);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(256, 50);
            this.BtnStart.TabIndex = 16;
            this.BtnStart.Text = "&Start";
            this.BtnStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnStart.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.BtnStart.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.BtnStart.UseVisualStyleBackColor = true;
            // 
            // Fps
            // 
            this.Fps.AutoSize = true;
            this.Fps.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Fps.Font = new System.Drawing.Font("Bahnschrift Condensed", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Fps.Location = new System.Drawing.Point(10, 567);
            this.Fps.Name = "Fps";
            this.Fps.Size = new System.Drawing.Size(0, 34);
            this.Fps.TabIndex = 15;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(10, 10);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.metroLabel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Size = new System.Drawing.Size(944, 591);
            this.splitContainer1.SplitterDistance = 63;
            this.splitContainer1.TabIndex = 0;
            // 
            // metroLabel1
            // 
            this.metroLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroLabel1.Font = new System.Drawing.Font("Bahnschrift", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.metroLabel1.Location = new System.Drawing.Point(0, 0);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(944, 63);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "Vehicle Counter on Cctv 001";
            this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroLabel1.UseStyleColors = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(944, 524);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Green;
            // 
            // Canvas1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.MainContainer);
            this.Name = "Canvas1";
            this.Padding = new System.Windows.Forms.Padding(10, 60, 10, 10);
            this.Text = "Car Counter v1.0";
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel1.PerformLayout();
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private SplitContainer MainContainer;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private Label Fps;
        private SplitContainer splitContainer1;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroTile BtnStart;
        private MetroFramework.Controls.MetroTile BtnSync;
        private MetroFramework.Controls.MetroTile BtnSave;
        private MetroFramework.Controls.MetroTile BtnOpen;
        private MetroFramework.Controls.MetroTile BtnStop;
    }
}