namespace CarCounter1
{
    partial class Canvas
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
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.BtnSync = new MetroFramework.Controls.MetroButton();
            this.BtnSave = new MetroFramework.Controls.MetroButton();
            this.BtnOpen = new MetroFramework.Controls.MetroButton();
            this.BtnStop = new MetroFramework.Controls.MetroButton();
            this.BtnStart = new MetroFramework.Controls.MetroButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            this.MainContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainContainer.BackColor = System.Drawing.SystemColors.HighlightText;
            this.MainContainer.Location = new System.Drawing.Point(19, 31);
            this.MainContainer.Margin = new System.Windows.Forms.Padding(50);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.BackColor = System.Drawing.Color.Silver;
            this.MainContainer.Panel1.Controls.Add(this.BtnSync);
            this.MainContainer.Panel1.Controls.Add(this.BtnSave);
            this.MainContainer.Panel1.Controls.Add(this.BtnOpen);
            this.MainContainer.Panel1.Controls.Add(this.BtnStop);
            this.MainContainer.Panel1.Controls.Add(this.BtnStart);
            this.MainContainer.Panel1.Font = new System.Drawing.Font("Bahnschrift Condensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainContainer.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.BackColor = System.Drawing.Color.Silver;
            this.MainContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.MainContainer.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.MainContainer.Size = new System.Drawing.Size(1220, 630);
            this.MainContainer.SplitterDistance = 271;
            this.MainContainer.TabIndex = 8;
            // 
            // BtnSync
            // 
            this.BtnSync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnSync.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnSync.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnSync.Location = new System.Drawing.Point(10, 210);
            this.BtnSync.Margin = new System.Windows.Forms.Padding(0);
            this.BtnSync.Name = "BtnSync";
            this.BtnSync.Padding = new System.Windows.Forms.Padding(10);
            this.BtnSync.Size = new System.Drawing.Size(251, 50);
            this.BtnSync.TabIndex = 13;
            this.BtnSync.Text = "Sync to Cloud";
            // 
            // BtnSave
            // 
            this.BtnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnSave.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnSave.Location = new System.Drawing.Point(10, 160);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(0);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Padding = new System.Windows.Forms.Padding(10);
            this.BtnSave.Size = new System.Drawing.Size(251, 50);
            this.BtnSave.TabIndex = 11;
            this.BtnSave.Text = "Save Log";
            // 
            // BtnOpen
            // 
            this.BtnOpen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnOpen.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnOpen.Location = new System.Drawing.Point(10, 110);
            this.BtnOpen.Margin = new System.Windows.Forms.Padding(0);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Padding = new System.Windows.Forms.Padding(10);
            this.BtnOpen.Size = new System.Drawing.Size(251, 50);
            this.BtnOpen.TabIndex = 12;
            this.BtnOpen.Text = "Open File (Dev only)";
            // 
            // BtnStop
            // 
            this.BtnStop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnStop.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnStop.Location = new System.Drawing.Point(10, 60);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(0);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Padding = new System.Windows.Forms.Padding(10);
            this.BtnStop.Size = new System.Drawing.Size(251, 50);
            this.BtnStop.TabIndex = 8;
            this.BtnStop.Text = "&Stop";
            // 
            // BtnStart
            // 
            this.BtnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnStart.Font = new System.Drawing.Font("Bahnschrift", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnStart.Location = new System.Drawing.Point(10, 10);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(0);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Padding = new System.Windows.Forms.Padding(10);
            this.BtnStart.Size = new System.Drawing.Size(251, 50);
            this.BtnStart.TabIndex = 8;
            this.BtnStart.Text = "&Start";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.metroLabel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.79661F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.20339F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(925, 610);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(13, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(899, 538);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // metroLabel1
            // 
            this.metroLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroLabel1.Font = new System.Drawing.Font("Bahnschrift", 38F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.metroLabel1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.metroLabel1.Location = new System.Drawing.Point(13, 10);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(899, 46);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "Vehicle Counter on Cctv 001";
            this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.MainContainer);
            this.Name = "Canvas";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Car Counter v1.0";
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private SplitContainer MainContainer;
        private TableLayoutPanel tableLayoutPanel1;
        private MetroFramework.Controls.MetroButton BtnStop;
        private MetroFramework.Controls.MetroButton BtnStart;
        private MetroFramework.Controls.MetroButton BtnSave;
        private MetroFramework.Controls.MetroButton BtnOpen;
        private MetroFramework.Controls.MetroButton BtnSync;
        private MetroFramework.Controls.MetroLabel metroLabel1;
    }
}