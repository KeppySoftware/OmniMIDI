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
            this.Quit = new System.Windows.Forms.Button();
            this.DefOut = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DevicesList = new System.Windows.Forms.ComboBox();
            this.DeviceCP = new System.Windows.Forms.Button();
            this.MaxThreads = new System.Windows.Forms.Label();
            this.ASIODevicesSupport = new System.Windows.Forms.LinkLabelEx();
            this.StatusLab = new System.Windows.Forms.Label();
            this.Status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(432, 87);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(75, 23);
            this.Quit.TabIndex = 7;
            this.Quit.Text = "OK";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // DefOut
            // 
            this.DefOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DefOut.Location = new System.Drawing.Point(11, 70);
            this.DefOut.Name = "DefOut";
            this.DefOut.Size = new System.Drawing.Size(339, 13);
            this.DefOut.TabIndex = 6;
            this.DefOut.Text = "Default Windows output: NaN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
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
            this.DevicesList.Size = new System.Drawing.Size(414, 21);
            this.DevicesList.TabIndex = 4;
            // 
            // DeviceCP
            // 
            this.DeviceCP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceCP.Location = new System.Drawing.Point(277, 87);
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
            this.MaxThreads.Location = new System.Drawing.Point(11, 92);
            this.MaxThreads.Name = "MaxThreads";
            this.MaxThreads.Size = new System.Drawing.Size(230, 13);
            this.MaxThreads.TabIndex = 9;
            this.MaxThreads.Text = "ASIO is allowed to use a maximum of 0 threads.";
            // 
            // ASIODevicesSupport
            // 
            this.ASIODevicesSupport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ASIODevicesSupport.AutoSize = true;
            this.ASIODevicesSupport.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.ASIODevicesSupport.Location = new System.Drawing.Point(356, 70);
            this.ASIODevicesSupport.Name = "ASIODevicesSupport";
            this.ASIODevicesSupport.Size = new System.Drawing.Size(153, 13);
            this.ASIODevicesSupport.TabIndex = 10;
            this.ASIODevicesSupport.TabStop = true;
            this.ASIODevicesSupport.Text = "List of supported ASIO devices";
            this.ASIODevicesSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ASIODevicesSupport_LinkClicked);
            // 
            // StatusLab
            // 
            this.StatusLab.Location = new System.Drawing.Point(11, 36);
            this.StatusLab.Name = "StatusLab";
            this.StatusLab.Size = new System.Drawing.Size(40, 13);
            this.StatusLab.TabIndex = 11;
            this.StatusLab.Text = "Status: NaN";
            // 
            // Status
            // 
            this.Status.Location = new System.Drawing.Point(48, 36);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(461, 13);
            this.Status.TabIndex = 12;
            this.Status.Text = "NaN";
            // 
            // DefaultASIOAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 121);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.StatusLab);
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
            this.Load += new System.EventHandler(this.DefaultASIOAudioOutput_Load);
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
        private System.Windows.Forms.LinkLabelEx ASIODevicesSupport;
        private System.Windows.Forms.Label StatusLab;
        private System.Windows.Forms.Label Status;
    }
}