namespace SensorNetworkInterface.Class_Files
{
    partial class MainForm
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
            this.gMap = new GMap.NET.WindowsForms.GMapControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flagButton = new System.Windows.Forms.Button();
            this.stopFile = new System.Windows.Forms.Button();
            this.playFile = new System.Windows.Forms.Button();
            this.displayFile = new System.Windows.Forms.Button();
            this.audioList = new System.Windows.Forms.ListBox();
            this.spectrogram = new System.Windows.Forms.PictureBox();
            this.waveViewer1 = new NAudio.Gui.WaveViewer();
            this.consoleRTBox = new System.Windows.Forms.RichTextBox();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.setIPButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectrogram)).BeginInit();
            this.SuspendLayout();
            // 
            // gMap
            // 
            this.gMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gMap.Bearing = 0F;
            this.gMap.CanDragMap = true;
            this.gMap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMap.GrayScaleMode = false;
            this.gMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMap.LevelsKeepInMemmory = 5;
            this.gMap.Location = new System.Drawing.Point(0, 0);
            this.gMap.MarkersEnabled = true;
            this.gMap.MaxZoom = 2;
            this.gMap.MinZoom = 2;
            this.gMap.MouseWheelZoomEnabled = true;
            this.gMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMap.Name = "gMap";
            this.gMap.NegativeMode = false;
            this.gMap.PolygonsEnabled = true;
            this.gMap.RetryLoadTile = 0;
            this.gMap.RoutesEnabled = false;
            this.gMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMap.ShowTileGridLines = false;
            this.gMap.Size = new System.Drawing.Size(790, 671);
            this.gMap.TabIndex = 0;
            this.gMap.Zoom = 0D;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(3, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(798, 520);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gMap);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(790, 494);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Map";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flagButton);
            this.tabPage2.Controls.Add(this.stopFile);
            this.tabPage2.Controls.Add(this.playFile);
            this.tabPage2.Controls.Add(this.displayFile);
            this.tabPage2.Controls.Add(this.audioList);
            this.tabPage2.Controls.Add(this.spectrogram);
            this.tabPage2.Controls.Add(this.waveViewer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(790, 494);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Audio";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // flagButton
            // 
            this.flagButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flagButton.Location = new System.Drawing.Point(220, 457);
            this.flagButton.Name = "flagButton";
            this.flagButton.Size = new System.Drawing.Size(97, 31);
            this.flagButton.TabIndex = 6;
            this.flagButton.Text = "Flag File";
            this.flagButton.UseVisualStyleBackColor = true;
            this.flagButton.Click += new System.EventHandler(this.flagButton_Click);
            // 
            // stopFile
            // 
            this.stopFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopFile.Location = new System.Drawing.Point(220, 420);
            this.stopFile.Name = "stopFile";
            this.stopFile.Size = new System.Drawing.Size(97, 31);
            this.stopFile.TabIndex = 5;
            this.stopFile.Text = "Stop File";
            this.stopFile.UseVisualStyleBackColor = true;
            this.stopFile.Click += new System.EventHandler(this.stopFile_Click);
            // 
            // playFile
            // 
            this.playFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.playFile.Location = new System.Drawing.Point(220, 383);
            this.playFile.Name = "playFile";
            this.playFile.Size = new System.Drawing.Size(97, 31);
            this.playFile.TabIndex = 4;
            this.playFile.Text = "Play File";
            this.playFile.UseVisualStyleBackColor = true;
            this.playFile.Click += new System.EventHandler(this.playFile_Click);
            // 
            // displayFile
            // 
            this.displayFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.displayFile.Location = new System.Drawing.Point(220, 344);
            this.displayFile.Name = "displayFile";
            this.displayFile.Size = new System.Drawing.Size(97, 33);
            this.displayFile.TabIndex = 3;
            this.displayFile.Text = "Display File";
            this.displayFile.UseVisualStyleBackColor = true;
            this.displayFile.Click += new System.EventHandler(this.displayFile_Click);
            // 
            // audioList
            // 
            this.audioList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.audioList.FormattingEnabled = true;
            this.audioList.Location = new System.Drawing.Point(7, 344);
            this.audioList.Name = "audioList";
            this.audioList.Size = new System.Drawing.Size(207, 147);
            this.audioList.TabIndex = 2;
            // 
            // spectrogram
            // 
            this.spectrogram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spectrogram.Location = new System.Drawing.Point(3, 3);
            this.spectrogram.Name = "spectrogram";
            this.spectrogram.Size = new System.Drawing.Size(784, 335);
            this.spectrogram.TabIndex = 1;
            this.spectrogram.TabStop = false;
            // 
            // waveViewer1
            // 
            this.waveViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waveViewer1.Location = new System.Drawing.Point(323, 344);
            this.waveViewer1.Name = "waveViewer1";
            this.waveViewer1.SamplesPerPixel = 128;
            this.waveViewer1.Size = new System.Drawing.Size(461, 144);
            this.waveViewer1.StartPosition = ((long)(0));
            this.waveViewer1.TabIndex = 0;
            this.waveViewer1.WaveStream = null;
            // 
            // consoleRTBox
            // 
            this.consoleRTBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.consoleRTBox.Location = new System.Drawing.Point(13, 539);
            this.consoleRTBox.Name = "consoleRTBox";
            this.consoleRTBox.Size = new System.Drawing.Size(608, 170);
            this.consoleRTBox.TabIndex = 2;
            this.consoleRTBox.Text = "";
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(628, 539);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(83, 20);
            this.ipAddress.TabIndex = 0;
            this.ipAddress.Text = "10.1.25.106";
            this.ipAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // setIPButton
            // 
            this.setIPButton.Location = new System.Drawing.Point(717, 539);
            this.setIPButton.Name = "setIPButton";
            this.setIPButton.Size = new System.Drawing.Size(93, 20);
            this.setIPButton.TabIndex = 2;
            this.setIPButton.Text = "Set IP Address";
            this.setIPButton.UseVisualStyleBackColor = true;
            this.setIPButton.Click += new System.EventHandler(this.setIPButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(627, 565);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(183, 72);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(628, 643);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(182, 66);
            this.disconnectButton.TabIndex = 4;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(822, 721);
            this.Controls.Add(this.setIPButton);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.ipAddress);
            this.Controls.Add(this.consoleRTBox);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GUBotDev Sensor Network Interface";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spectrogram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public GMap.NET.WindowsForms.GMapControl gMap;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private NAudio.Gui.WaveViewer waveViewer1;
        private System.Windows.Forms.PictureBox spectrogram;
        private System.Windows.Forms.Button flagButton;
        private System.Windows.Forms.Button stopFile;
        private System.Windows.Forms.Button playFile;
        private System.Windows.Forms.Button displayFile;
        private System.Windows.Forms.ListBox audioList;
        private System.Windows.Forms.RichTextBox consoleRTBox;
        private System.Windows.Forms.TextBox ipAddress;
        private System.Windows.Forms.Button setIPButton;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
    }
}

