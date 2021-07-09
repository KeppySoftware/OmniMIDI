namespace OmniMIDIConfigurator{
    partial class DefaultAudioOutput
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
            this.DevicesList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Quit = new System.Windows.Forms.Button();
            this.SwitchDefaultAudio = new System.Windows.Forms.CheckBox();
            this.UseNewWASAPI = new System.Windows.Forms.Button();
            this.ReduceBootUpDelay = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // DevicesList
            // 
            this.DevicesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DevicesList.FormattingEnabled = true;
            this.DevicesList.Location = new System.Drawing.Point(94, 11);
            this.DevicesList.Name = "DevicesList";
            this.DevicesList.Size = new System.Drawing.Size(315, 21);
            this.DevicesList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current output:";
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(334, 84);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(75, 23);
            this.Quit.TabIndex = 3;
            this.Quit.Text = "OK";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // SwitchDefaultAudio
            // 
            this.SwitchDefaultAudio.AutoSize = true;
            this.SwitchDefaultAudio.Location = new System.Drawing.Point(11, 41);
            this.SwitchDefaultAudio.Name = "SwitchDefaultAudio";
            this.SwitchDefaultAudio.Size = new System.Drawing.Size(221, 17);
            this.SwitchDefaultAudio.TabIndex = 4;
            this.SwitchDefaultAudio.Text = "Switch default audio device automatically";
            this.SwitchDefaultAudio.UseVisualStyleBackColor = true;
            this.SwitchDefaultAudio.CheckedChanged += new System.EventHandler(this.SwitchDefaultAudio_CheckedChanged);
            // 
            // UseNewWASAPI
            // 
            this.UseNewWASAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UseNewWASAPI.Location = new System.Drawing.Point(224, 84);
            this.UseNewWASAPI.Name = "UseNewWASAPI";
            this.UseNewWASAPI.Size = new System.Drawing.Size(104, 23);
            this.UseNewWASAPI.TabIndex = 5;
            this.UseNewWASAPI.Text = "Use new WASAPI";
            this.UseNewWASAPI.UseVisualStyleBackColor = true;
            this.UseNewWASAPI.Visible = false;
            this.UseNewWASAPI.Click += new System.EventHandler(this.UseNewWASAPI_Click);
            // 
            // ReduceBootUpDelay
            // 
            this.ReduceBootUpDelay.AutoSize = true;
            this.ReduceBootUpDelay.Location = new System.Drawing.Point(11, 60);
            this.ReduceBootUpDelay.Name = "ReduceBootUpDelay";
            this.ReduceBootUpDelay.Size = new System.Drawing.Size(403, 17);
            this.ReduceBootUpDelay.TabIndex = 6;
            this.ReduceBootUpDelay.Text = "Reduce boot-up delay with DirectSound (Will disable automatic buffer detection)";
            this.ReduceBootUpDelay.UseVisualStyleBackColor = true;
            this.ReduceBootUpDelay.CheckedChanged += new System.EventHandler(this.ReduceBootUpDelay_CheckedChanged);
            // 
            // DefaultAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(420, 118);
            this.Controls.Add(this.ReduceBootUpDelay);
            this.Controls.Add(this.UseNewWASAPI);
            this.Controls.Add(this.SwitchDefaultAudio);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevicesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultAudioOutput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change default {0} output";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox DevicesList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.CheckBox SwitchDefaultAudio;
        private System.Windows.Forms.Button UseNewWASAPI;
        private System.Windows.Forms.CheckBox ReduceBootUpDelay;
    }
}