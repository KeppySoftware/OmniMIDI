namespace OmniMIDIConfigurator
{
    partial class AdvancedAudioSettings
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
            this.AOS = new System.Windows.Forms.GroupBox();
            this.AudioBitDepth = new System.Windows.Forms.ComboBox();
            this.AudioBitDepthLabel = new System.Windows.Forms.Label();
            this.MonophonicFunc = new System.Windows.Forms.CheckBox();
            this.FadeoutDisable = new System.Windows.Forms.CheckBox();
            this.ABS = new System.Windows.Forms.GroupBox();
            this.HModeWhat = new System.Windows.Forms.PictureBox();
            this.HMode = new System.Windows.Forms.CheckBox();
            this.KSDAPIBoxWhat = new System.Windows.Forms.PictureBox();
            this.KSDAPIBox = new System.Windows.Forms.CheckBox();
            this.SlowDownPlayback = new System.Windows.Forms.CheckBox();
            this.OldBuff = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChangePitchShifting = new System.Windows.Forms.Button();
            this.ChangeDefaultOutput = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CAE = new System.Windows.Forms.Label();
            this.Requirements = new System.Windows.Forms.ToolTip(this.components);
            this.AOS.SuspendLayout();
            this.ABS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HModeWhat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KSDAPIBoxWhat)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AOS
            // 
            this.AOS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AOS.Controls.Add(this.AudioBitDepth);
            this.AOS.Controls.Add(this.AudioBitDepthLabel);
            this.AOS.Controls.Add(this.MonophonicFunc);
            this.AOS.Controls.Add(this.FadeoutDisable);
            this.AOS.Location = new System.Drawing.Point(12, 12);
            this.AOS.Name = "AOS";
            this.AOS.Size = new System.Drawing.Size(345, 92);
            this.AOS.TabIndex = 0;
            this.AOS.TabStop = false;
            this.AOS.Text = "Audio output settings";
            // 
            // AudioBitDepth
            // 
            this.AudioBitDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AudioBitDepth.FormattingEnabled = true;
            this.AudioBitDepth.Items.AddRange(new object[] {
            "32-bit float",
            "16-bit integer",
            "8-bit integer"});
            this.AudioBitDepth.Location = new System.Drawing.Point(89, 20);
            this.AudioBitDepth.Name = "AudioBitDepth";
            this.AudioBitDepth.Size = new System.Drawing.Size(85, 21);
            this.AudioBitDepth.TabIndex = 1;
            this.Requirements.SetToolTip(this.AudioBitDepth, "Changing this setting requires a restart of the audio stream.");
            this.AudioBitDepth.SelectedIndexChanged += new System.EventHandler(this.AudioBitDepth_SelectedIndexChanged);
            // 
            // AudioBitDepthLabel
            // 
            this.AudioBitDepthLabel.AutoSize = true;
            this.AudioBitDepthLabel.Location = new System.Drawing.Point(6, 23);
            this.AudioBitDepthLabel.Name = "AudioBitDepthLabel";
            this.AudioBitDepthLabel.Size = new System.Drawing.Size(81, 13);
            this.AudioBitDepthLabel.TabIndex = 2;
            this.AudioBitDepthLabel.Text = "Audio bit depth:";
            this.Requirements.SetToolTip(this.AudioBitDepthLabel, "Changing this setting requires the user to restart the MIDI application.");
            // 
            // MonophonicFunc
            // 
            this.MonophonicFunc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MonophonicFunc.AutoSize = true;
            this.MonophonicFunc.Location = new System.Drawing.Point(6, 48);
            this.MonophonicFunc.Name = "MonophonicFunc";
            this.MonophonicFunc.Size = new System.Drawing.Size(153, 17);
            this.MonophonicFunc.TabIndex = 2;
            this.MonophonicFunc.Text = "Use monophonic rendering";
            this.Requirements.SetToolTip(this.MonophonicFunc, "Changing this setting requires a restart of the audio stream.");
            this.MonophonicFunc.UseVisualStyleBackColor = true;
            this.MonophonicFunc.CheckedChanged += new System.EventHandler(this.MonophonicFunc_CheckedChanged);
            // 
            // FadeoutDisable
            // 
            this.FadeoutDisable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FadeoutDisable.AutoSize = true;
            this.FadeoutDisable.Location = new System.Drawing.Point(6, 67);
            this.FadeoutDisable.Name = "FadeoutDisable";
            this.FadeoutDisable.Size = new System.Drawing.Size(217, 17);
            this.FadeoutDisable.TabIndex = 3;
            this.FadeoutDisable.Text = "Disable fade-out when killing an old note";
            this.FadeoutDisable.UseVisualStyleBackColor = true;
            this.FadeoutDisable.CheckedChanged += new System.EventHandler(this.FadeoutDisable_CheckedChanged);
            // 
            // ABS
            // 
            this.ABS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ABS.Controls.Add(this.HModeWhat);
            this.ABS.Controls.Add(this.HMode);
            this.ABS.Controls.Add(this.KSDAPIBoxWhat);
            this.ABS.Controls.Add(this.KSDAPIBox);
            this.ABS.Controls.Add(this.SlowDownPlayback);
            this.ABS.Controls.Add(this.OldBuff);
            this.ABS.Location = new System.Drawing.Point(12, 110);
            this.ABS.Name = "ABS";
            this.ABS.Size = new System.Drawing.Size(345, 100);
            this.ABS.TabIndex = 4;
            this.ABS.TabStop = false;
            this.ABS.Text = "Audio buffer settings";
            // 
            // HModeWhat
            // 
            this.HModeWhat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HModeWhat.Image = global::OmniMIDIConfigurator.Properties.Resources.wi;
            this.HModeWhat.Location = new System.Drawing.Point(182, 38);
            this.HModeWhat.Name = "HModeWhat";
            this.HModeWhat.Size = new System.Drawing.Size(14, 14);
            this.HModeWhat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.HModeWhat.TabIndex = 10;
            this.HModeWhat.TabStop = false;
            this.HModeWhat.Click += new System.EventHandler(this.HModeWhat_Click);
            // 
            // HMode
            // 
            this.HMode.AutoSize = true;
            this.HMode.Location = new System.Drawing.Point(6, 37);
            this.HMode.Name = "HMode";
            this.HMode.Size = new System.Drawing.Size(177, 17);
            this.HMode.TabIndex = 5;
            this.HMode.Text = "Enable minimum playback mode";
            this.HMode.UseVisualStyleBackColor = true;
            this.HMode.CheckedChanged += new System.EventHandler(this.HMode_CheckedChanged);
            // 
            // KSDAPIBoxWhat
            // 
            this.KSDAPIBoxWhat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KSDAPIBoxWhat.Image = global::OmniMIDIConfigurator.Properties.Resources.what;
            this.KSDAPIBoxWhat.Location = new System.Drawing.Point(249, 19);
            this.KSDAPIBoxWhat.Name = "KSDAPIBoxWhat";
            this.KSDAPIBoxWhat.Size = new System.Drawing.Size(14, 14);
            this.KSDAPIBoxWhat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.KSDAPIBoxWhat.TabIndex = 8;
            this.KSDAPIBoxWhat.TabStop = false;
            this.KSDAPIBoxWhat.Click += new System.EventHandler(this.KSDAPIBoxWhat_Click);
            // 
            // KSDAPIBox
            // 
            this.KSDAPIBox.AutoSize = true;
            this.KSDAPIBox.Location = new System.Drawing.Point(6, 18);
            this.KSDAPIBox.Name = "KSDAPIBox";
            this.KSDAPIBox.Size = new System.Drawing.Size(244, 17);
            this.KSDAPIBox.TabIndex = 4;
            this.KSDAPIBox.Text = "Allow apps to use the Keppy\'s Direct MIDI API";
            this.KSDAPIBox.UseVisualStyleBackColor = true;
            this.KSDAPIBox.CheckedChanged += new System.EventHandler(this.KSDAPIBox_CheckedChanged);
            // 
            // SlowDownPlayback
            // 
            this.SlowDownPlayback.AutoSize = true;
            this.SlowDownPlayback.Location = new System.Drawing.Point(6, 56);
            this.SlowDownPlayback.Name = "SlowDownPlayback";
            this.SlowDownPlayback.Size = new System.Drawing.Size(258, 17);
            this.SlowDownPlayback.TabIndex = 6;
            this.SlowDownPlayback.Text = "Slow down events processing instead of skipping";
            this.Requirements.SetToolTip(this.SlowDownPlayback, "This doesn\'t work while minimum playback mode is enabled.");
            this.SlowDownPlayback.UseVisualStyleBackColor = true;
            this.SlowDownPlayback.CheckedChanged += new System.EventHandler(this.SlowDownPlayback_CheckedChanged);
            // 
            // OldBuff
            // 
            this.OldBuff.AutoSize = true;
            this.OldBuff.Location = new System.Drawing.Point(6, 75);
            this.OldBuff.Name = "OldBuff";
            this.OldBuff.Size = new System.Drawing.Size(335, 17);
            this.OldBuff.TabIndex = 7;
            this.OldBuff.Text = "Run events processer and audio engine on the same thread/core";
            this.Requirements.SetToolTip(this.OldBuff, "This doesn\'t work in .WAV mode.");
            this.OldBuff.UseVisualStyleBackColor = true;
            this.OldBuff.CheckedChanged += new System.EventHandler(this.OldBuff_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ChangePitchShifting);
            this.groupBox1.Controls.Add(this.ChangeDefaultOutput);
            this.groupBox1.Location = new System.Drawing.Point(12, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 48);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other settings";
            // 
            // ChangePitchShifting
            // 
            this.ChangePitchShifting.Location = new System.Drawing.Point(177, 17);
            this.ChangePitchShifting.Name = "ChangePitchShifting";
            this.ChangePitchShifting.Size = new System.Drawing.Size(162, 23);
            this.ChangePitchShifting.TabIndex = 10;
            this.ChangePitchShifting.Text = "Change transpose value";
            this.ChangePitchShifting.UseVisualStyleBackColor = true;
            this.ChangePitchShifting.Click += new System.EventHandler(this.ChangePitchShifting_Click);
            // 
            // ChangeDefaultOutput
            // 
            this.ChangeDefaultOutput.Location = new System.Drawing.Point(6, 17);
            this.ChangeDefaultOutput.Name = "ChangeDefaultOutput";
            this.ChangeDefaultOutput.Size = new System.Drawing.Size(160, 23);
            this.ChangeDefaultOutput.TabIndex = 9;
            this.ChangeDefaultOutput.Text = "Change default audio output";
            this.Requirements.SetToolTip(this.ChangeDefaultOutput, "Changing this setting requires the user to restart the MIDI application.");
            this.ChangeDefaultOutput.UseVisualStyleBackColor = true;
            this.ChangeDefaultOutput.Click += new System.EventHandler(this.ChangeDefaultOutput_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(282, 274);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CAE
            // 
            this.CAE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CAE.AutoSize = true;
            this.CAE.Enabled = false;
            this.CAE.Location = new System.Drawing.Point(15, 279);
            this.CAE.Name = "CAE";
            this.CAE.Size = new System.Drawing.Size(125, 13);
            this.CAE.TabIndex = 9;
            this.CAE.Text = "Current audio engine: {0}";
            // 
            // Requirements
            // 
            this.Requirements.AutomaticDelay = 100;
            this.Requirements.AutoPopDelay = 10000;
            this.Requirements.InitialDelay = 100;
            this.Requirements.IsBalloon = true;
            this.Requirements.ReshowDelay = 20;
            this.Requirements.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Requirements.ToolTipTitle = "Requirement";
            // 
            // AdvancedAudioSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(369, 309);
            this.Controls.Add(this.CAE);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ABS);
            this.Controls.Add(this.AOS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedAudioSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced audio settings";
            this.Load += new System.EventHandler(this.AdvancedAudioSettings_Load);
            this.AOS.ResumeLayout(false);
            this.AOS.PerformLayout();
            this.ABS.ResumeLayout(false);
            this.ABS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HModeWhat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KSDAPIBoxWhat)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox AOS;
        private System.Windows.Forms.Label AudioBitDepthLabel;
        private System.Windows.Forms.CheckBox MonophonicFunc;
        private System.Windows.Forms.CheckBox FadeoutDisable;
        private System.Windows.Forms.ComboBox AudioBitDepth;
        private System.Windows.Forms.GroupBox ABS;
        private System.Windows.Forms.CheckBox SlowDownPlayback;
        private System.Windows.Forms.CheckBox OldBuff;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ChangePitchShifting;
        private System.Windows.Forms.Button ChangeDefaultOutput;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Label CAE;
        private System.Windows.Forms.ToolTip Requirements;
        private System.Windows.Forms.PictureBox KSDAPIBoxWhat;
        private System.Windows.Forms.CheckBox KSDAPIBox;
        private System.Windows.Forms.PictureBox HModeWhat;
        private System.Windows.Forms.CheckBox HMode;
    }
}