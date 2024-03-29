﻿namespace OmniMIDIConfigurator{
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
            this.ReduceBootUpDelay = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // DevicesList
            // 
            this.DevicesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DevicesList.FormattingEnabled = true;
            this.DevicesList.Location = new System.Drawing.Point(110, 13);
            this.DevicesList.Name = "DevicesList";
            this.DevicesList.Size = new System.Drawing.Size(367, 23);
            this.DevicesList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current output:";
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(390, 97);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(87, 27);
            this.Quit.TabIndex = 3;
            this.Quit.Text = "OK";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // SwitchDefaultAudio
            // 
            this.SwitchDefaultAudio.AutoSize = true;
            this.SwitchDefaultAudio.Location = new System.Drawing.Point(13, 47);
            this.SwitchDefaultAudio.Name = "SwitchDefaultAudio";
            this.SwitchDefaultAudio.Size = new System.Drawing.Size(246, 19);
            this.SwitchDefaultAudio.TabIndex = 4;
            this.SwitchDefaultAudio.Text = "Switch default audio device automatically";
            this.SwitchDefaultAudio.UseVisualStyleBackColor = true;
            this.SwitchDefaultAudio.CheckedChanged += new System.EventHandler(this.SwitchDefaultAudio_CheckedChanged);
            // 
            // ReduceBootUpDelay
            // 
            this.ReduceBootUpDelay.AutoSize = true;
            this.ReduceBootUpDelay.Location = new System.Drawing.Point(13, 69);
            this.ReduceBootUpDelay.Name = "ReduceBootUpDelay";
            this.ReduceBootUpDelay.Size = new System.Drawing.Size(359, 19);
            this.ReduceBootUpDelay.TabIndex = 6;
            this.ReduceBootUpDelay.Text = "Reduce boot-up delay (Will disable automatic buffer detection)";
            this.ReduceBootUpDelay.UseVisualStyleBackColor = true;
            this.ReduceBootUpDelay.CheckedChanged += new System.EventHandler(this.ReduceBootUpDelay_CheckedChanged);
            // 
            // DefaultAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(490, 136);
            this.Controls.Add(this.ReduceBootUpDelay);
            this.Controls.Add(this.SwitchDefaultAudio);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevicesList);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultAudioOutput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change default BASS output";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox DevicesList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.CheckBox SwitchDefaultAudio;
        private System.Windows.Forms.CheckBox ReduceBootUpDelay;
    }
}