namespace OmniMIDIConfigurator
{
    partial class DefaultASIOAudioOutput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefaultASIOAudioOutput));
            this.Quit = new System.Windows.Forms.Button();
            this.DefOut = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DevicesList = new System.Windows.Forms.ComboBox();
            this.DeviceCP = new System.Windows.Forms.Button();
            this.MaxThreads = new System.Windows.Forms.Label();
            this.StatusLab = new System.Windows.Forms.Label();
            this.Status = new System.Windows.Forms.Label();
            this.InfoGroupBox = new System.Windows.Forms.GroupBox();
            this.BufferInfo = new System.Windows.Forms.Label();
            this.Outputs = new System.Windows.Forms.Label();
            this.Inputs = new System.Windows.Forms.Label();
            this.DeviceName = new System.Windows.Forms.Label();
            this.BufferInfoLab = new System.Windows.Forms.Label();
            this.OutputsLab = new System.Windows.Forms.Label();
            this.InputsLab = new System.Windows.Forms.Label();
            this.DeviceNameLab = new System.Windows.Forms.Label();
            this.ASIODirectFeed = new System.Windows.Forms.CheckBox();
            this.ButtonsDesc = new System.Windows.Forms.ToolTip(this.components);
            this.DisableASIOFreqWarn = new System.Windows.Forms.CheckBox();
            this.LeaveASIODeviceFreq = new System.Windows.Forms.CheckBox();
            this.ChanSetBox = new System.Windows.Forms.GroupBox();
            this.RightCh = new System.Windows.Forms.ComboBox();
            this.LeftCh = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LatencyWarning = new OmniMIDIConfigurator.LinkLabelEx();
            this.ASIODevicesSupport = new OmniMIDIConfigurator.LinkLabelEx();
            this.InfoGroupBox.SuspendLayout();
            this.ChanSetBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(504, 423);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(87, 27);
            this.Quit.TabIndex = 7;
            this.Quit.Text = "Apply";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // DefOut
            // 
            this.DefOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DefOut.Location = new System.Drawing.Point(13, 404);
            this.DefOut.Name = "DefOut";
            this.DefOut.Size = new System.Drawing.Size(395, 15);
            this.DefOut.TabIndex = 6;
            this.DefOut.Text = "Default ASIO output: Loading...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
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
            this.DevicesList.Location = new System.Drawing.Point(111, 13);
            this.DevicesList.Name = "DevicesList";
            this.DevicesList.Size = new System.Drawing.Size(380, 23);
            this.DevicesList.TabIndex = 4;
            // 
            // DeviceCP
            // 
            this.DeviceCP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceCP.Location = new System.Drawing.Point(323, 423);
            this.DeviceCP.Name = "DeviceCP";
            this.DeviceCP.Size = new System.Drawing.Size(174, 27);
            this.DeviceCP.TabIndex = 8;
            this.DeviceCP.Text = "Open device\'s control panel";
            this.DeviceCP.UseVisualStyleBackColor = true;
            this.DeviceCP.Click += new System.EventHandler(this.DeviceCP_Click);
            // 
            // MaxThreads
            // 
            this.MaxThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MaxThreads.AutoSize = true;
            this.MaxThreads.Location = new System.Drawing.Point(13, 429);
            this.MaxThreads.Name = "MaxThreads";
            this.MaxThreads.Size = new System.Drawing.Size(258, 15);
            this.MaxThreads.TabIndex = 9;
            this.MaxThreads.Text = "ASIO is allowed to use a maximum of 0 threads.";
            // 
            // StatusLab
            // 
            this.StatusLab.AutoSize = true;
            this.StatusLab.Location = new System.Drawing.Point(7, 105);
            this.StatusLab.Name = "StatusLab";
            this.StatusLab.Size = new System.Drawing.Size(42, 15);
            this.StatusLab.TabIndex = 11;
            this.StatusLab.Text = "Status:";
            // 
            // Status
            // 
            this.Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Status.Location = new System.Drawing.Point(50, 105);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(516, 30);
            this.Status.TabIndex = 12;
            this.Status.Text = "Loading...";
            // 
            // InfoGroupBox
            // 
            this.InfoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoGroupBox.Controls.Add(this.BufferInfo);
            this.InfoGroupBox.Controls.Add(this.Outputs);
            this.InfoGroupBox.Controls.Add(this.Inputs);
            this.InfoGroupBox.Controls.Add(this.DeviceName);
            this.InfoGroupBox.Controls.Add(this.BufferInfoLab);
            this.InfoGroupBox.Controls.Add(this.Status);
            this.InfoGroupBox.Controls.Add(this.OutputsLab);
            this.InfoGroupBox.Controls.Add(this.StatusLab);
            this.InfoGroupBox.Controls.Add(this.InputsLab);
            this.InfoGroupBox.Controls.Add(this.DeviceNameLab);
            this.InfoGroupBox.Location = new System.Drawing.Point(12, 45);
            this.InfoGroupBox.Name = "InfoGroupBox";
            this.InfoGroupBox.Size = new System.Drawing.Size(582, 149);
            this.InfoGroupBox.TabIndex = 13;
            this.InfoGroupBox.TabStop = false;
            this.InfoGroupBox.Text = "ASIO output info";
            // 
            // BufferInfo
            // 
            this.BufferInfo.AutoSize = true;
            this.BufferInfo.Location = new System.Drawing.Point(72, 84);
            this.BufferInfo.Name = "BufferInfo";
            this.BufferInfo.Size = new System.Drawing.Size(59, 15);
            this.BufferInfo.TabIndex = 16;
            this.BufferInfo.Text = "Loading...";
            // 
            // Outputs
            // 
            this.Outputs.AutoSize = true;
            this.Outputs.Location = new System.Drawing.Point(59, 63);
            this.Outputs.Name = "Outputs";
            this.Outputs.Size = new System.Drawing.Size(59, 15);
            this.Outputs.TabIndex = 15;
            this.Outputs.Text = "Loading...";
            // 
            // Inputs
            // 
            this.Inputs.AutoSize = true;
            this.Inputs.Location = new System.Drawing.Point(50, 43);
            this.Inputs.Name = "Inputs";
            this.Inputs.Size = new System.Drawing.Size(59, 15);
            this.Inputs.TabIndex = 14;
            this.Inputs.Text = "Loading...";
            // 
            // DeviceName
            // 
            this.DeviceName.AutoSize = true;
            this.DeviceName.Location = new System.Drawing.Point(90, 22);
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Size = new System.Drawing.Size(59, 15);
            this.DeviceName.TabIndex = 13;
            this.DeviceName.Text = "Loading...";
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
            // OutputsLab
            // 
            this.OutputsLab.AutoSize = true;
            this.OutputsLab.Location = new System.Drawing.Point(7, 63);
            this.OutputsLab.Name = "OutputsLab";
            this.OutputsLab.Size = new System.Drawing.Size(53, 15);
            this.OutputsLab.TabIndex = 2;
            this.OutputsLab.Text = "Outputs:";
            // 
            // InputsLab
            // 
            this.InputsLab.AutoSize = true;
            this.InputsLab.Location = new System.Drawing.Point(7, 43);
            this.InputsLab.Name = "InputsLab";
            this.InputsLab.Size = new System.Drawing.Size(43, 15);
            this.InputsLab.TabIndex = 1;
            this.InputsLab.Text = "Inputs:";
            // 
            // DeviceNameLab
            // 
            this.DeviceNameLab.AutoSize = true;
            this.DeviceNameLab.Location = new System.Drawing.Point(7, 22);
            this.DeviceNameLab.Name = "DeviceNameLab";
            this.DeviceNameLab.Size = new System.Drawing.Size(78, 15);
            this.DeviceNameLab.TabIndex = 0;
            this.DeviceNameLab.Text = "Device name:";
            // 
            // ASIODirectFeed
            // 
            this.ASIODirectFeed.AutoSize = true;
            this.ASIODirectFeed.Location = new System.Drawing.Point(7, 22);
            this.ASIODirectFeed.Name = "ASIODirectFeed";
            this.ASIODirectFeed.Size = new System.Drawing.Size(337, 19);
            this.ASIODirectFeed.TabIndex = 14;
            this.ASIODirectFeed.Text = "Make ASIO feed itself off of the audio stream automatically";
            this.ButtonsDesc.SetToolTip(this.ASIODirectFeed, resources.GetString("ASIODirectFeed.ToolTip"));
            this.ASIODirectFeed.UseVisualStyleBackColor = true;
            // 
            // ButtonsDesc
            // 
            this.ButtonsDesc.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ButtonsDesc.ToolTipTitle = "Information";
            // 
            // DisableASIOFreqWarn
            // 
            this.DisableASIOFreqWarn.AutoSize = true;
            this.DisableASIOFreqWarn.Location = new System.Drawing.Point(7, 44);
            this.DisableASIOFreqWarn.Name = "DisableASIOFreqWarn";
            this.DisableASIOFreqWarn.Size = new System.Drawing.Size(335, 19);
            this.DisableASIOFreqWarn.TabIndex = 15;
            this.DisableASIOFreqWarn.Text = "Suppress the unsupported output frequency warning/error";
            this.ButtonsDesc.SetToolTip(this.DisableASIOFreqWarn, resources.GetString("DisableASIOFreqWarn.ToolTip"));
            this.DisableASIOFreqWarn.UseVisualStyleBackColor = true;
            // 
            // LeaveASIODeviceFreq
            // 
            this.LeaveASIODeviceFreq.AutoSize = true;
            this.LeaveASIODeviceFreq.Location = new System.Drawing.Point(7, 66);
            this.LeaveASIODeviceFreq.Name = "LeaveASIODeviceFreq";
            this.LeaveASIODeviceFreq.Size = new System.Drawing.Size(395, 19);
            this.LeaveASIODeviceFreq.TabIndex = 16;
            this.LeaveASIODeviceFreq.Text = "Prevent OmniMIDI from changing the ASIO device\'s output frequency";
            this.ButtonsDesc.SetToolTip(this.LeaveASIODeviceFreq, resources.GetString("LeaveASIODeviceFreq.ToolTip"));
            this.LeaveASIODeviceFreq.UseVisualStyleBackColor = true;
            // 
            // ChanSetBox
            // 
            this.ChanSetBox.Controls.Add(this.RightCh);
            this.ChanSetBox.Controls.Add(this.LeftCh);
            this.ChanSetBox.Controls.Add(this.label3);
            this.ChanSetBox.Controls.Add(this.label2);
            this.ChanSetBox.Location = new System.Drawing.Point(12, 198);
            this.ChanSetBox.Name = "ChanSetBox";
            this.ChanSetBox.Size = new System.Drawing.Size(582, 88);
            this.ChanSetBox.TabIndex = 16;
            this.ChanSetBox.TabStop = false;
            this.ChanSetBox.Text = "Channel settings";
            // 
            // RightCh
            // 
            this.RightCh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RightCh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RightCh.FormattingEnabled = true;
            this.RightCh.Location = new System.Drawing.Point(188, 48);
            this.RightCh.Name = "RightCh";
            this.RightCh.Size = new System.Drawing.Size(380, 23);
            this.RightCh.TabIndex = 18;
            // 
            // LeftCh
            // 
            this.LeftCh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftCh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LeftCh.FormattingEnabled = true;
            this.LeftCh.Location = new System.Drawing.Point(188, 22);
            this.LeftCh.Name = "LeftCh";
            this.LeftCh.Size = new System.Drawing.Size(380, 23);
            this.LeftCh.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Right channel (Input 1):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Left channel (Input 0):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LeaveASIODeviceFreq);
            this.groupBox1.Controls.Add(this.DisableASIOFreqWarn);
            this.groupBox1.Controls.Add(this.ASIODirectFeed);
            this.groupBox1.Location = new System.Drawing.Point(12, 293);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 98);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel settings";
            // 
            // LatencyWarning
            // 
            this.LatencyWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LatencyWarning.Image = global::OmniMIDIConfigurator.Properties.Resources.wi;
            this.LatencyWarning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LatencyWarning.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.LatencyWarning.Location = new System.Drawing.Point(433, 399);
            this.LatencyWarning.Name = "LatencyWarning";
            this.LatencyWarning.Size = new System.Drawing.Size(159, 22);
            this.LatencyWarning.TabIndex = 15;
            this.LatencyWarning.TabStop = true;
            this.LatencyWarning.Text = "Read me! It\'s important!";
            this.LatencyWarning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LatencyWarning.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LatencyWarning_LinkClicked);
            // 
            // ASIODevicesSupport
            // 
            this.ASIODevicesSupport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ASIODevicesSupport.AutoSize = true;
            this.ASIODevicesSupport.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.ASIODevicesSupport.Location = new System.Drawing.Point(498, 9);
            this.ASIODevicesSupport.Name = "ASIODevicesSupport";
            this.ASIODevicesSupport.Size = new System.Drawing.Size(96, 30);
            this.ASIODevicesSupport.TabIndex = 10;
            this.ASIODevicesSupport.TabStop = true;
            this.ASIODevicesSupport.Text = "List of supported\r\nASIO devices";
            this.ASIODevicesSupport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ASIODevicesSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ASIODevicesSupport_LinkClicked);
            // 
            // DefaultASIOAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 463);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ChanSetBox);
            this.Controls.Add(this.LatencyWarning);
            this.Controls.Add(this.InfoGroupBox);
            this.Controls.Add(this.ASIODevicesSupport);
            this.Controls.Add(this.MaxThreads);
            this.Controls.Add(this.DeviceCP);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.DefOut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevicesList);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultASIOAudioOutput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change default ASIO output";
            this.Shown += new System.EventHandler(this.DefaultASIOAudioOutput_Shown);
            this.InfoGroupBox.ResumeLayout(false);
            this.InfoGroupBox.PerformLayout();
            this.ChanSetBox.ResumeLayout(false);
            this.ChanSetBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.Label DefOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DevicesList;
        private System.Windows.Forms.Button DeviceCP;
        private System.Windows.Forms.Label MaxThreads;
        private OmniMIDIConfigurator.LinkLabelEx ASIODevicesSupport;
        private System.Windows.Forms.Label StatusLab;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.GroupBox InfoGroupBox;
        private System.Windows.Forms.Label BufferInfo;
        private System.Windows.Forms.Label Outputs;
        private System.Windows.Forms.Label Inputs;
        private System.Windows.Forms.Label DeviceName;
        private System.Windows.Forms.Label BufferInfoLab;
        private System.Windows.Forms.Label OutputsLab;
        private System.Windows.Forms.Label InputsLab;
        private System.Windows.Forms.Label DeviceNameLab;
        private System.Windows.Forms.CheckBox ASIODirectFeed;
        private OmniMIDIConfigurator.LinkLabelEx LatencyWarning;
        private System.Windows.Forms.ToolTip ButtonsDesc;
        private System.Windows.Forms.GroupBox ChanSetBox;
        private System.Windows.Forms.ComboBox RightCh;
        private System.Windows.Forms.ComboBox LeftCh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox DisableASIOFreqWarn;
        private System.Windows.Forms.CheckBox LeaveASIODeviceFreq;
    }
}