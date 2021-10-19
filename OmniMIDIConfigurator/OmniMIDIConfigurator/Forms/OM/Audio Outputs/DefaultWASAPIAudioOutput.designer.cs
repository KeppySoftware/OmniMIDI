namespace OmniMIDIConfigurator
{
    partial class DefaultWASAPIAudioOutput
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
            this.Quit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DevicesList = new System.Windows.Forms.ComboBox();
            this.IDLab = new System.Windows.Forms.Label();
            this.InfoGroupBox = new System.Windows.Forms.GroupBox();
            this.ID = new System.Windows.Forms.Label();
            this.BufferInfo = new System.Windows.Forms.Label();
            this.Channels = new System.Windows.Forms.Label();
            this.Freq = new System.Windows.Forms.Label();
            this.DeviceName = new System.Windows.Forms.Label();
            this.BufferInfoLab = new System.Windows.Forms.Label();
            this.ChannelsLab = new System.Windows.Forms.Label();
            this.FreqLab = new System.Windows.Forms.Label();
            this.DeviceNameLab = new System.Windows.Forms.Label();
            this.ExclusiveMode = new System.Windows.Forms.CheckBox();
            this.OldWASAPIMode = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AsyncMode = new System.Windows.Forms.CheckBox();
            this.NoDoubleBuffering = new System.Windows.Forms.CheckBox();
            this.WASAPIRawMode = new System.Windows.Forms.CheckBox();
            this.InfoGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(506, 307);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(87, 27);
            this.Quit.TabIndex = 7;
            this.Quit.Text = "Apply";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current output:";
            // 
            // DevicesList
            // 
            this.DevicesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DevicesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DevicesList.FormattingEnabled = true;
            this.DevicesList.Location = new System.Drawing.Point(110, 13);
            this.DevicesList.Name = "DevicesList";
            this.DevicesList.Size = new System.Drawing.Size(482, 23);
            this.DevicesList.TabIndex = 4;
            this.DevicesList.SelectedIndexChanged += new System.EventHandler(this.DevicesList_SelectedIndexChanged);
            // 
            // IDLab
            // 
            this.IDLab.AutoSize = true;
            this.IDLab.Location = new System.Drawing.Point(7, 105);
            this.IDLab.Name = "IDLab";
            this.IDLab.Size = new System.Drawing.Size(21, 15);
            this.IDLab.TabIndex = 11;
            this.IDLab.Text = "ID:";
            // 
            // InfoGroupBox
            // 
            this.InfoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoGroupBox.Controls.Add(this.ID);
            this.InfoGroupBox.Controls.Add(this.BufferInfo);
            this.InfoGroupBox.Controls.Add(this.Channels);
            this.InfoGroupBox.Controls.Add(this.Freq);
            this.InfoGroupBox.Controls.Add(this.DeviceName);
            this.InfoGroupBox.Controls.Add(this.BufferInfoLab);
            this.InfoGroupBox.Controls.Add(this.ChannelsLab);
            this.InfoGroupBox.Controls.Add(this.IDLab);
            this.InfoGroupBox.Controls.Add(this.FreqLab);
            this.InfoGroupBox.Controls.Add(this.DeviceNameLab);
            this.InfoGroupBox.Location = new System.Drawing.Point(10, 44);
            this.InfoGroupBox.Name = "InfoGroupBox";
            this.InfoGroupBox.Size = new System.Drawing.Size(582, 133);
            this.InfoGroupBox.TabIndex = 13;
            this.InfoGroupBox.TabStop = false;
            this.InfoGroupBox.Text = "WASAPI output info";
            // 
            // ID
            // 
            this.ID.AutoSize = true;
            this.ID.Location = new System.Drawing.Point(29, 105);
            this.ID.Name = "ID";
            this.ID.Size = new System.Drawing.Size(45, 15);
            this.ID.TabIndex = 17;
            this.ID.Text = "IDINFO";
            // 
            // BufferInfo
            // 
            this.BufferInfo.AutoSize = true;
            this.BufferInfo.Location = new System.Drawing.Point(72, 84);
            this.BufferInfo.Name = "BufferInfo";
            this.BufferInfo.Size = new System.Drawing.Size(55, 15);
            this.BufferInfo.TabIndex = 16;
            this.BufferInfo.Text = "BUFINFO";
            // 
            // Channels
            // 
            this.Channels.AutoSize = true;
            this.Channels.Location = new System.Drawing.Point(68, 63);
            this.Channels.Name = "Channels";
            this.Channels.Size = new System.Drawing.Size(41, 15);
            this.Channels.TabIndex = 15;
            this.Channels.Text = "CHAN";
            // 
            // Freq
            // 
            this.Freq.AutoSize = true;
            this.Freq.Location = new System.Drawing.Point(75, 43);
            this.Freq.Name = "Freq";
            this.Freq.Size = new System.Drawing.Size(35, 15);
            this.Freq.TabIndex = 14;
            this.Freq.Text = "FREQ";
            // 
            // DeviceName
            // 
            this.DeviceName.AutoSize = true;
            this.DeviceName.Location = new System.Drawing.Point(83, 22);
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Size = new System.Drawing.Size(79, 15);
            this.DeviceName.TabIndex = 13;
            this.DeviceName.Text = "DEVICENAME";
            // 
            // BufferInfoLab
            // 
            this.BufferInfoLab.AutoSize = true;
            this.BufferInfoLab.Location = new System.Drawing.Point(7, 84);
            this.BufferInfoLab.Name = "BufferInfoLab";
            this.BufferInfoLab.Size = new System.Drawing.Size(66, 15);
            this.BufferInfoLab.TabIndex = 3;
            this.BufferInfoLab.Text = "Buffer info:";
            // 
            // ChannelsLab
            // 
            this.ChannelsLab.AutoSize = true;
            this.ChannelsLab.Location = new System.Drawing.Point(7, 63);
            this.ChannelsLab.Name = "ChannelsLab";
            this.ChannelsLab.Size = new System.Drawing.Size(59, 15);
            this.ChannelsLab.TabIndex = 2;
            this.ChannelsLab.Text = "Channels:";
            // 
            // FreqLab
            // 
            this.FreqLab.AutoSize = true;
            this.FreqLab.Location = new System.Drawing.Point(7, 43);
            this.FreqLab.Name = "FreqLab";
            this.FreqLab.Size = new System.Drawing.Size(65, 15);
            this.FreqLab.TabIndex = 1;
            this.FreqLab.Text = "Frequency:";
            // 
            // DeviceNameLab
            // 
            this.DeviceNameLab.AutoSize = true;
            this.DeviceNameLab.Location = new System.Drawing.Point(7, 22);
            this.DeviceNameLab.Name = "DeviceNameLab";
            this.DeviceNameLab.Size = new System.Drawing.Size(71, 15);
            this.DeviceNameLab.TabIndex = 0;
            this.DeviceNameLab.Text = "Device type:";
            // 
            // ExclusiveMode
            // 
            this.ExclusiveMode.AutoSize = true;
            this.ExclusiveMode.Location = new System.Drawing.Point(10, 22);
            this.ExclusiveMode.Name = "ExclusiveMode";
            this.ExclusiveMode.Size = new System.Drawing.Size(523, 19);
            this.ExclusiveMode.TabIndex = 14;
            this.ExclusiveMode.Text = "Use device in exclusive mode (Reduces latency, but prevents other apps from rende" +
    "ring audio)";
            this.ExclusiveMode.UseVisualStyleBackColor = true;
            // 
            // OldWASAPIMode
            // 
            this.OldWASAPIMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OldWASAPIMode.Location = new System.Drawing.Point(383, 307);
            this.OldWASAPIMode.Name = "OldWASAPIMode";
            this.OldWASAPIMode.Size = new System.Drawing.Size(119, 27);
            this.OldWASAPIMode.TabIndex = 15;
            this.OldWASAPIMode.Text = "Use old WASAPI";
            this.OldWASAPIMode.UseVisualStyleBackColor = true;
            this.OldWASAPIMode.Click += new System.EventHandler(this.OldWASAPIMode_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.AsyncMode);
            this.groupBox1.Controls.Add(this.NoDoubleBuffering);
            this.groupBox1.Controls.Add(this.WASAPIRawMode);
            this.groupBox1.Controls.Add(this.ExclusiveMode);
            this.groupBox1.Location = new System.Drawing.Point(10, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 115);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WASAPI output settings";
            // 
            // AsyncMode
            // 
            this.AsyncMode.AutoSize = true;
            this.AsyncMode.Location = new System.Drawing.Point(10, 88);
            this.AsyncMode.Name = "AsyncMode";
            this.AsyncMode.Size = new System.Drawing.Size(535, 19);
            this.AsyncMode.TabIndex = 17;
            this.AsyncMode.Text = "Send data to the device in asynchronous mode (Can reduce latency but also introdu" +
    "ce crackling)";
            this.AsyncMode.UseVisualStyleBackColor = true;
            // 
            // NoDoubleBuffering
            // 
            this.NoDoubleBuffering.AutoSize = true;
            this.NoDoubleBuffering.Location = new System.Drawing.Point(10, 44);
            this.NoDoubleBuffering.Name = "NoDoubleBuffering";
            this.NoDoubleBuffering.Size = new System.Drawing.Size(500, 19);
            this.NoDoubleBuffering.TabIndex = 16;
            this.NoDoubleBuffering.Text = "Disable double buffering (Reduces latency but disables the audio monitoring in th" +
    "e mixer)";
            this.NoDoubleBuffering.UseVisualStyleBackColor = true;
            // 
            // WASAPIRawMode
            // 
            this.WASAPIRawMode.AutoSize = true;
            this.WASAPIRawMode.Location = new System.Drawing.Point(10, 66);
            this.WASAPIRawMode.Name = "WASAPIRawMode";
            this.WASAPIRawMode.Size = new System.Drawing.Size(428, 19);
            this.WASAPIRawMode.TabIndex = 15;
            this.WASAPIRawMode.Text = "Enable raw mode (Disables APO filters/effects, only works on Windows 8.1+)";
            this.WASAPIRawMode.UseVisualStyleBackColor = true;
            // 
            // DefaultWASAPIAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 344);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OldWASAPIMode);
            this.Controls.Add(this.InfoGroupBox);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevicesList);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultWASAPIAudioOutput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change default WASAPI output";
            this.Load += new System.EventHandler(this.DefaultWASAPIAudioOutput_Load);
            this.InfoGroupBox.ResumeLayout(false);
            this.InfoGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DevicesList;
        private System.Windows.Forms.Label IDLab;
        private System.Windows.Forms.GroupBox InfoGroupBox;
        private System.Windows.Forms.Label BufferInfo;
        private System.Windows.Forms.Label Channels;
        private System.Windows.Forms.Label Freq;
        private System.Windows.Forms.Label DeviceName;
        private System.Windows.Forms.Label BufferInfoLab;
        private System.Windows.Forms.Label ChannelsLab;
        private System.Windows.Forms.Label FreqLab;
        private System.Windows.Forms.Label DeviceNameLab;
        private System.Windows.Forms.CheckBox ExclusiveMode;
        private System.Windows.Forms.Label ID;
        private System.Windows.Forms.Button OldWASAPIMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox NoDoubleBuffering;
        private System.Windows.Forms.CheckBox WASAPIRawMode;
        private System.Windows.Forms.CheckBox AsyncMode;
    }
}