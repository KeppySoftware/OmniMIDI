namespace OmniMIDIConfigurator
{
    partial class MIDIEventsParserSettings
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
            this.ABS = new System.Windows.Forms.GroupBox();
            this.DisableCookedPlayer = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NoteOffDelayValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.OverrideNoteLengthWA2 = new System.Windows.Forms.PictureBox();
            this.DelayNoteOff = new System.Windows.Forms.CheckBox();
            this.NoteLengthValueMS = new System.Windows.Forms.Label();
            this.NoteLengthValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.OverrideNoteLengthWA1 = new System.Windows.Forms.PictureBox();
            this.OverrideNoteLength = new System.Windows.Forms.CheckBox();
            this.MT32Mode = new System.Windows.Forms.CheckBox();
            this.IgnoreNotes = new System.Windows.Forms.CheckBox();
            this.FullVelocityMode = new System.Windows.Forms.CheckBox();
            this.AllNotesIgnore = new System.Windows.Forms.CheckBox();
            this.AOS = new System.Windows.Forms.GroupBox();
            this.CapFram = new System.Windows.Forms.CheckBox();
            this.Limit88 = new System.Windows.Forms.CheckBox();
            this.OS = new System.Windows.Forms.GroupBox();
            this.EVBufDialog = new System.Windows.Forms.Button();
            this.RevbNChor = new System.Windows.Forms.Button();
            this.IgnoreNotesInterval = new System.Windows.Forms.Button();
            this.CAE = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.Requirements = new System.Windows.Forms.ToolTip(this.components);
            this.ABS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoteOffDelayValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteLengthValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA1)).BeginInit();
            this.AOS.SuspendLayout();
            this.OS.SuspendLayout();
            this.SuspendLayout();
            // 
            // ABS
            // 
            this.ABS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ABS.Controls.Add(this.DisableCookedPlayer);
            this.ABS.Controls.Add(this.label3);
            this.ABS.Controls.Add(this.NoteOffDelayValue);
            this.ABS.Controls.Add(this.label2);
            this.ABS.Controls.Add(this.OverrideNoteLengthWA2);
            this.ABS.Controls.Add(this.DelayNoteOff);
            this.ABS.Controls.Add(this.NoteLengthValueMS);
            this.ABS.Controls.Add(this.NoteLengthValue);
            this.ABS.Controls.Add(this.label1);
            this.ABS.Controls.Add(this.OverrideNoteLengthWA1);
            this.ABS.Controls.Add(this.OverrideNoteLength);
            this.ABS.Controls.Add(this.MT32Mode);
            this.ABS.Controls.Add(this.IgnoreNotes);
            this.ABS.Controls.Add(this.FullVelocityMode);
            this.ABS.Controls.Add(this.AllNotesIgnore);
            this.ABS.Location = new System.Drawing.Point(12, 79);
            this.ABS.Name = "ABS";
            this.ABS.Size = new System.Drawing.Size(345, 163);
            this.ABS.TabIndex = 11;
            this.ABS.TabStop = false;
            this.ABS.Text = "Ignore specific stuff/Set full velocity";
            // 
            // DisableCookedPlayer
            // 
            this.DisableCookedPlayer.AutoSize = true;
            this.DisableCookedPlayer.Location = new System.Drawing.Point(6, 19);
            this.DisableCookedPlayer.Name = "DisableCookedPlayer";
            this.DisableCookedPlayer.Size = new System.Drawing.Size(268, 17);
            this.DisableCookedPlayer.TabIndex = 20;
            this.DisableCookedPlayer.Text = "Disable CookedPlayer (MIDI_IO_COOKED support)";
            this.DisableCookedPlayer.UseVisualStyleBackColor = true;
            this.DisableCookedPlayer.CheckedChanged += new System.EventHandler(this.DisableCookedPlayer_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(332, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "s";
            // 
            // NoteOffDelayValue
            // 
            this.NoteOffDelayValue.DecimalPlaces = 3;
            this.NoteOffDelayValue.Location = new System.Drawing.Point(267, 132);
            this.NoteOffDelayValue.Name = "NoteOffDelayValue";
            this.NoteOffDelayValue.Size = new System.Drawing.Size(65, 20);
            this.NoteOffDelayValue.TabIndex = 11;
            this.NoteOffDelayValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NoteOffDelayValue.ValueChanged += new System.EventHandler(this.NoteOffDelayValue_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Length:";
            // 
            // OverrideNoteLengthWA2
            // 
            this.OverrideNoteLengthWA2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OverrideNoteLengthWA2.Image = global::OmniMIDIConfigurator.Properties.Resources.wi;
            this.OverrideNoteLengthWA2.Location = new System.Drawing.Point(161, 134);
            this.OverrideNoteLengthWA2.Name = "OverrideNoteLengthWA2";
            this.OverrideNoteLengthWA2.Size = new System.Drawing.Size(14, 14);
            this.OverrideNoteLengthWA2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OverrideNoteLengthWA2.TabIndex = 16;
            this.OverrideNoteLengthWA2.TabStop = false;
            this.OverrideNoteLengthWA2.Click += new System.EventHandler(this.midiOutCloseDisabled_Click);
            // 
            // DelayNoteOff
            // 
            this.DelayNoteOff.AutoSize = true;
            this.DelayNoteOff.Location = new System.Drawing.Point(6, 133);
            this.DelayNoteOff.Name = "DelayNoteOff";
            this.DelayNoteOff.Size = new System.Drawing.Size(156, 17);
            this.DelayNoteOff.TabIndex = 10;
            this.DelayNoteOff.Text = "Add delay to noteoff events";
            this.Requirements.SetToolTip(this.DelayNoteOff, "This doesn\'t work while minimum playback mode is enabled.\r\n");
            this.DelayNoteOff.UseVisualStyleBackColor = true;
            this.DelayNoteOff.CheckedChanged += new System.EventHandler(this.DelayNoteOff_CheckedChanged);
            // 
            // NoteLengthValueMS
            // 
            this.NoteLengthValueMS.Location = new System.Drawing.Point(332, 114);
            this.NoteLengthValueMS.Name = "NoteLengthValueMS";
            this.NoteLengthValueMS.Size = new System.Drawing.Size(9, 13);
            this.NoteLengthValueMS.TabIndex = 14;
            this.NoteLengthValueMS.Text = "s";
            // 
            // NoteLengthValue
            // 
            this.NoteLengthValue.DecimalPlaces = 3;
            this.NoteLengthValue.Location = new System.Drawing.Point(267, 113);
            this.NoteLengthValue.Name = "NoteLengthValue";
            this.NoteLengthValue.Size = new System.Drawing.Size(65, 20);
            this.NoteLengthValue.TabIndex = 9;
            this.NoteLengthValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NoteLengthValue.ValueChanged += new System.EventHandler(this.NoteLengthValue_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(225, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Length:";
            // 
            // OverrideNoteLengthWA1
            // 
            this.OverrideNoteLengthWA1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OverrideNoteLengthWA1.Image = global::OmniMIDIConfigurator.Properties.Resources.wi;
            this.OverrideNoteLengthWA1.Location = new System.Drawing.Point(174, 115);
            this.OverrideNoteLengthWA1.Name = "OverrideNoteLengthWA1";
            this.OverrideNoteLengthWA1.Size = new System.Drawing.Size(14, 14);
            this.OverrideNoteLengthWA1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OverrideNoteLengthWA1.TabIndex = 11;
            this.OverrideNoteLengthWA1.TabStop = false;
            this.OverrideNoteLengthWA1.Click += new System.EventHandler(this.midiOutCloseDisabled_Click);
            // 
            // OverrideNoteLength
            // 
            this.OverrideNoteLength.AutoSize = true;
            this.OverrideNoteLength.Location = new System.Drawing.Point(6, 114);
            this.OverrideNoteLength.Name = "OverrideNoteLength";
            this.OverrideNoteLength.Size = new System.Drawing.Size(169, 17);
            this.OverrideNoteLength.TabIndex = 8;
            this.OverrideNoteLength.Text = "Override length of note events";
            this.Requirements.SetToolTip(this.OverrideNoteLength, "This doesn\'t work while minimum playback mode is enabled.\r\n");
            this.OverrideNoteLength.UseVisualStyleBackColor = true;
            this.OverrideNoteLength.CheckedChanged += new System.EventHandler(this.OverrideNoteLength_CheckedChanged);
            // 
            // MT32Mode
            // 
            this.MT32Mode.AutoSize = true;
            this.MT32Mode.Location = new System.Drawing.Point(6, 95);
            this.MT32Mode.Name = "MT32Mode";
            this.MT32Mode.Size = new System.Drawing.Size(140, 17);
            this.MT32Mode.TabIndex = 7;
            this.MT32Mode.Text = "Enable the MT-32 mode";
            this.MT32Mode.UseVisualStyleBackColor = true;
            this.MT32Mode.CheckedChanged += new System.EventHandler(this.MT32Mode_CheckedChanged);
            // 
            // IgnoreNotes
            // 
            this.IgnoreNotes.AutoSize = true;
            this.IgnoreNotes.Location = new System.Drawing.Point(6, 57);
            this.IgnoreNotes.Name = "IgnoreNotes";
            this.IgnoreNotes.Size = new System.Drawing.Size(233, 17);
            this.IgnoreNotes.TabIndex = 5;
            this.IgnoreNotes.Text = "Ignore notes in between two velocity values";
            this.IgnoreNotes.UseVisualStyleBackColor = true;
            this.IgnoreNotes.CheckedChanged += new System.EventHandler(this.IgnoreNotes_CheckedChanged);
            // 
            // FullVelocityMode
            // 
            this.FullVelocityMode.AutoSize = true;
            this.FullVelocityMode.Location = new System.Drawing.Point(6, 76);
            this.FullVelocityMode.Name = "FullVelocityMode";
            this.FullVelocityMode.Size = new System.Drawing.Size(169, 17);
            this.FullVelocityMode.TabIndex = 6;
            this.FullVelocityMode.Text = "Set all the notes to full velocity";
            this.FullVelocityMode.UseVisualStyleBackColor = true;
            this.FullVelocityMode.CheckedChanged += new System.EventHandler(this.FullVelocityMode_CheckedChanged);
            // 
            // AllNotesIgnore
            // 
            this.AllNotesIgnore.AutoSize = true;
            this.AllNotesIgnore.Location = new System.Drawing.Point(6, 38);
            this.AllNotesIgnore.Name = "AllNotesIgnore";
            this.AllNotesIgnore.Size = new System.Drawing.Size(130, 17);
            this.AllNotesIgnore.TabIndex = 4;
            this.AllNotesIgnore.Text = "Ignore all MIDI events";
            this.AllNotesIgnore.UseVisualStyleBackColor = true;
            this.AllNotesIgnore.CheckedChanged += new System.EventHandler(this.AllNotesIgnore_CheckedChanged);
            // 
            // AOS
            // 
            this.AOS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AOS.Controls.Add(this.CapFram);
            this.AOS.Controls.Add(this.Limit88);
            this.AOS.Location = new System.Drawing.Point(12, 12);
            this.AOS.Name = "AOS";
            this.AOS.Size = new System.Drawing.Size(345, 61);
            this.AOS.TabIndex = 10;
            this.AOS.TabStop = false;
            this.AOS.Text = "Parser settings";
            // 
            // CapFram
            // 
            this.CapFram.AutoSize = true;
            this.CapFram.Location = new System.Drawing.Point(6, 19);
            this.CapFram.Name = "CapFram";
            this.CapFram.Size = new System.Drawing.Size(165, 17);
            this.CapFram.TabIndex = 1;
            this.CapFram.Text = "Cap input framerate to 60FPS";
            this.CapFram.UseVisualStyleBackColor = true;
            this.CapFram.CheckedChanged += new System.EventHandler(this.CapFram_CheckedChanged);
            // 
            // Limit88
            // 
            this.Limit88.AutoSize = true;
            this.Limit88.Location = new System.Drawing.Point(6, 38);
            this.Limit88.Name = "Limit88";
            this.Limit88.Size = new System.Drawing.Size(276, 17);
            this.Limit88.TabIndex = 2;
            this.Limit88.Text = "Limit key range to 88 keys (Excluding drums channel)";
            this.Limit88.UseVisualStyleBackColor = true;
            this.Limit88.CheckedChanged += new System.EventHandler(this.Limit88_CheckedChanged);
            // 
            // OS
            // 
            this.OS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OS.Controls.Add(this.EVBufDialog);
            this.OS.Controls.Add(this.RevbNChor);
            this.OS.Controls.Add(this.IgnoreNotesInterval);
            this.OS.Location = new System.Drawing.Point(12, 248);
            this.OS.Name = "OS";
            this.OS.Size = new System.Drawing.Size(345, 75);
            this.OS.TabIndex = 12;
            this.OS.TabStop = false;
            this.OS.Text = "Other settings";
            // 
            // EVBufDialog
            // 
            this.EVBufDialog.Location = new System.Drawing.Point(90, 44);
            this.EVBufDialog.Name = "EVBufDialog";
            this.EVBufDialog.Size = new System.Drawing.Size(162, 23);
            this.EVBufDialog.TabIndex = 14;
            this.EVBufDialog.Text = "Change size of the EV buffer";
            this.EVBufDialog.UseVisualStyleBackColor = true;
            this.EVBufDialog.Click += new System.EventHandler(this.EVBufDialog_Click);
            // 
            // RevbNChor
            // 
            this.RevbNChor.Location = new System.Drawing.Point(6, 17);
            this.RevbNChor.Name = "RevbNChor";
            this.RevbNChor.Size = new System.Drawing.Size(160, 23);
            this.RevbNChor.TabIndex = 12;
            this.RevbNChor.Text = "Set reverb and chorus";
            this.RevbNChor.UseVisualStyleBackColor = true;
            this.RevbNChor.Click += new System.EventHandler(this.RevbNChor_Click);
            // 
            // IgnoreNotesInterval
            // 
            this.IgnoreNotesInterval.Enabled = false;
            this.IgnoreNotesInterval.Location = new System.Drawing.Point(177, 17);
            this.IgnoreNotesInterval.Name = "IgnoreNotesInterval";
            this.IgnoreNotesInterval.Size = new System.Drawing.Size(162, 23);
            this.IgnoreNotesInterval.TabIndex = 13;
            this.IgnoreNotesInterval.Text = "Set velocity range to ignore";
            this.IgnoreNotesInterval.UseVisualStyleBackColor = true;
            this.IgnoreNotesInterval.Click += new System.EventHandler(this.IgnoreNotesInterval_Click);
            // 
            // CAE
            // 
            this.CAE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CAE.AutoSize = true;
            this.CAE.Enabled = false;
            this.CAE.Location = new System.Drawing.Point(15, 337);
            this.CAE.Name = "CAE";
            this.CAE.Size = new System.Drawing.Size(125, 13);
            this.CAE.TabIndex = 14;
            this.CAE.Text = "Current audio engine: {0}";
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(282, 332);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 15;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // Requirements
            // 
            this.Requirements.AutomaticDelay = 100;
            this.Requirements.AutoPopDelay = 10000;
            this.Requirements.InitialDelay = 100;
            this.Requirements.IsBalloon = true;
            this.Requirements.ReshowDelay = 20;
            this.Requirements.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Requirements.ToolTipTitle = "Information";
            // 
            // MIDIEventsParserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 367);
            this.Controls.Add(this.ABS);
            this.Controls.Add(this.AOS);
            this.Controls.Add(this.OS);
            this.Controls.Add(this.CAE);
            this.Controls.Add(this.OKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MIDIEventsParserSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MIDI events parser settings";
            this.Load += new System.EventHandler(this.MIDIEventsParserSettings_Load);
            this.ABS.ResumeLayout(false);
            this.ABS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoteOffDelayValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteLengthValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA1)).EndInit();
            this.AOS.ResumeLayout(false);
            this.AOS.PerformLayout();
            this.OS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ABS;
        private System.Windows.Forms.CheckBox FullVelocityMode;
        private System.Windows.Forms.CheckBox AllNotesIgnore;
        private System.Windows.Forms.GroupBox AOS;
        private System.Windows.Forms.CheckBox CapFram;
        private System.Windows.Forms.CheckBox Limit88;
        private System.Windows.Forms.GroupBox OS;
        private System.Windows.Forms.Button IgnoreNotesInterval;
        private System.Windows.Forms.Label CAE;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button RevbNChor;
        private System.Windows.Forms.CheckBox IgnoreNotes;
        private System.Windows.Forms.ToolTip Requirements;
        private System.Windows.Forms.Button EVBufDialog;
        private System.Windows.Forms.CheckBox MT32Mode;
        private System.Windows.Forms.PictureBox OverrideNoteLengthWA1;
        private System.Windows.Forms.CheckBox OverrideNoteLength;
        private System.Windows.Forms.NumericUpDown NoteLengthValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label NoteLengthValueMS;
        private System.Windows.Forms.NumericUpDown NoteOffDelayValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox OverrideNoteLengthWA2;
        private System.Windows.Forms.CheckBox DelayNoteOff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox DisableCookedPlayer;
    }
}