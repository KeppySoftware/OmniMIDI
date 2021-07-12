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
            this.LatencyWarning = new OmniMIDIConfigurator.LinkLabelEx();
            this.ASIODevicesSupport = new OmniMIDIConfigurator.LinkLabelEx();
            this.ChanSetBox = new System.Windows.Forms.GroupBox();
            this.RightCh = new System.Windows.Forms.ComboBox();
            this.LeftCh = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InfoGroupBox.SuspendLayout();
            this.ChanSetBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(432, 297);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(75, 23);
            this.Quit.TabIndex = 7;
            this.Quit.Text = "Apply";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // DefOut
            // 
            this.DefOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DefOut.Location = new System.Drawing.Point(11, 280);
            this.DefOut.Name = "DefOut";
            this.DefOut.Size = new System.Drawing.Size(339, 13);
            this.DefOut.TabIndex = 6;
            this.DefOut.Text = "Default ASIO output: Loading...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current output:";
            // 
            // DevicesList
            // 
            this.DevicesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DevicesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DevicesList.FormattingEnabled = true;
            this.DevicesList.Location = new System.Drawing.Point(95, 11);
            this.DevicesList.Name = "DevicesList";
            this.DevicesList.Size = new System.Drawing.Size(326, 21);
            this.DevicesList.TabIndex = 4;
            // 
            // DeviceCP
            // 
            this.DeviceCP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceCP.Location = new System.Drawing.Point(277, 297);
            this.DeviceCP.Name = "DeviceCP";
            this.DeviceCP.Size = new System.Drawing.Size(149, 23);
            this.DeviceCP.TabIndex = 8;
            this.DeviceCP.Text = "Open device\'s control panel";
            this.DeviceCP.UseVisualStyleBackColor = true;
            this.DeviceCP.Click += new System.EventHandler(this.DeviceCP_Click);
            // 
            // MaxThreads
            // 
            this.MaxThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MaxThreads.AutoSize = true;
            this.MaxThreads.Location = new System.Drawing.Point(11, 302);
            this.MaxThreads.Name = "MaxThreads";
            this.MaxThreads.Size = new System.Drawing.Size(230, 13);
            this.MaxThreads.TabIndex = 9;
            this.MaxThreads.Text = "ASIO is allowed to use a maximum of 0 threads.";
            // 
            // StatusLab
            // 
            this.StatusLab.AutoSize = true;
            this.StatusLab.Location = new System.Drawing.Point(6, 91);
            this.StatusLab.Name = "StatusLab";
            this.StatusLab.Size = new System.Drawing.Size(40, 13);
            this.StatusLab.TabIndex = 11;
            this.StatusLab.Text = "Status:";
            // 
            // Status
            // 
            this.Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Status.Location = new System.Drawing.Point(43, 91);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(442, 26);
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
            this.InfoGroupBox.Location = new System.Drawing.Point(10, 59);
            this.InfoGroupBox.Name = "InfoGroupBox";
            this.InfoGroupBox.Size = new System.Drawing.Size(499, 129);
            this.InfoGroupBox.TabIndex = 13;
            this.InfoGroupBox.TabStop = false;
            this.InfoGroupBox.Text = "ASIO output info";
            // 
            // BufferInfo
            // 
            this.BufferInfo.AutoSize = true;
            this.BufferInfo.Location = new System.Drawing.Point(62, 73);
            this.BufferInfo.Name = "BufferInfo";
            this.BufferInfo.Size = new System.Drawing.Size(54, 13);
            this.BufferInfo.TabIndex = 16;
            this.BufferInfo.Text = "Loading...";
            // 
            // Outputs
            // 
            this.Outputs.AutoSize = true;
            this.Outputs.Location = new System.Drawing.Point(51, 55);
            this.Outputs.Name = "Outputs";
            this.Outputs.Size = new System.Drawing.Size(54, 13);
            this.Outputs.TabIndex = 15;
            this.Outputs.Text = "Loading...";
            // 
            // Inputs
            // 
            this.Inputs.AutoSize = true;
            this.Inputs.Location = new System.Drawing.Point(43, 37);
            this.Inputs.Name = "Inputs";
            this.Inputs.Size = new System.Drawing.Size(54, 13);
            this.Inputs.TabIndex = 14;
            this.Inputs.Text = "Loading...";
            // 
            // DeviceName
            // 
            this.DeviceName.AutoSize = true;
            this.DeviceName.Location = new System.Drawing.Point(77, 19);
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Size = new System.Drawing.Size(54, 13);
            this.DeviceName.TabIndex = 13;
            this.DeviceName.Text = "Loading...";
            // 
            // BufferInfoLab
            // 
            this.BufferInfoLab.AutoSize = true;
            this.BufferInfoLab.Location = new System.Drawing.Point(6, 73);
            this.BufferInfoLab.Name = "BufferInfoLab";
            this.BufferInfoLab.Size = new System.Drawing.Size(58, 13);
            this.BufferInfoLab.TabIndex = 3;
            this.BufferInfoLab.Text = "Buffer info:";
            // 
            // OutputsLab
            // 
            this.OutputsLab.AutoSize = true;
            this.OutputsLab.Location = new System.Drawing.Point(6, 55);
            this.OutputsLab.Name = "OutputsLab";
            this.OutputsLab.Size = new System.Drawing.Size(47, 13);
            this.OutputsLab.TabIndex = 2;
            this.OutputsLab.Text = "Outputs:";
            // 
            // InputsLab
            // 
            this.InputsLab.AutoSize = true;
            this.InputsLab.Location = new System.Drawing.Point(6, 37);
            this.InputsLab.Name = "InputsLab";
            this.InputsLab.Size = new System.Drawing.Size(39, 13);
            this.InputsLab.TabIndex = 1;
            this.InputsLab.Text = "Inputs:";
            // 
            // DeviceNameLab
            // 
            this.DeviceNameLab.AutoSize = true;
            this.DeviceNameLab.Location = new System.Drawing.Point(6, 19);
            this.DeviceNameLab.Name = "DeviceNameLab";
            this.DeviceNameLab.Size = new System.Drawing.Size(73, 13);
            this.DeviceNameLab.TabIndex = 0;
            this.DeviceNameLab.Text = "Device name:";
            // 
            // ASIODirectFeed
            // 
            this.ASIODirectFeed.AutoSize = true;
            this.ASIODirectFeed.Location = new System.Drawing.Point(95, 38);
            this.ASIODirectFeed.Name = "ASIODirectFeed";
            this.ASIODirectFeed.Size = new System.Drawing.Size(301, 17);
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
            // LatencyWarning
            // 
            this.LatencyWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LatencyWarning.Image = global::OmniMIDIConfigurator.Properties.Resources.wi;
            this.LatencyWarning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LatencyWarning.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.LatencyWarning.Location = new System.Drawing.Point(371, 276);
            this.LatencyWarning.Name = "LatencyWarning";
            this.LatencyWarning.Size = new System.Drawing.Size(136, 19);
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
            this.ASIODevicesSupport.Location = new System.Drawing.Point(427, 8);
            this.ASIODevicesSupport.Name = "ASIODevicesSupport";
            this.ASIODevicesSupport.Size = new System.Drawing.Size(85, 26);
            this.ASIODevicesSupport.TabIndex = 10;
            this.ASIODevicesSupport.TabStop = true;
            this.ASIODevicesSupport.Text = "List of supported\r\nASIO devices";
            this.ASIODevicesSupport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ASIODevicesSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ASIODevicesSupport_LinkClicked);
            // 
            // ChanSetBox
            // 
            this.ChanSetBox.Controls.Add(this.RightCh);
            this.ChanSetBox.Controls.Add(this.LeftCh);
            this.ChanSetBox.Controls.Add(this.label3);
            this.ChanSetBox.Controls.Add(this.label2);
            this.ChanSetBox.Location = new System.Drawing.Point(10, 192);
            this.ChanSetBox.Name = "ChanSetBox";
            this.ChanSetBox.Size = new System.Drawing.Size(499, 76);
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
            this.RightCh.Location = new System.Drawing.Point(161, 42);
            this.RightCh.Name = "RightCh";
            this.RightCh.Size = new System.Drawing.Size(326, 21);
            this.RightCh.TabIndex = 18;
            // 
            // LeftCh
            // 
            this.LeftCh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftCh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LeftCh.FormattingEnabled = true;
            this.LeftCh.Location = new System.Drawing.Point(161, 19);
            this.LeftCh.Name = "LeftCh";
            this.LeftCh.Size = new System.Drawing.Size(326, 21);
            this.LeftCh.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Right channel (Input 1):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Left channel (Input 0):";
            // 
            // DefaultASIOAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 331);
            this.Controls.Add(this.ChanSetBox);
            this.Controls.Add(this.LatencyWarning);
            this.Controls.Add(this.ASIODirectFeed);
            this.Controls.Add(this.InfoGroupBox);
            this.Controls.Add(this.ASIODevicesSupport);
            this.Controls.Add(this.MaxThreads);
            this.Controls.Add(this.DeviceCP);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.DefOut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevicesList);
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
    }
}