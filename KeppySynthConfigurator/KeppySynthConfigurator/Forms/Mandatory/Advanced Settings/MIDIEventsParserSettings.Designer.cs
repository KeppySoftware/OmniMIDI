namespace KeppySynthConfigurator
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
            this.ABS = new System.Windows.Forms.GroupBox();
            this.IgnoreNotes = new System.Windows.Forms.CheckBox();
            this.FullVelocityMode = new System.Windows.Forms.CheckBox();
            this.SysExIgnore = new System.Windows.Forms.CheckBox();
            this.AllNotesIgnore = new System.Windows.Forms.CheckBox();
            this.AOS = new System.Windows.Forms.GroupBox();
            this.CapFram = new System.Windows.Forms.CheckBox();
            this.Limit88 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RevbNChor = new System.Windows.Forms.Button();
            this.IgnoreNotesInterval = new System.Windows.Forms.Button();
            this.changeTheSizeOfTheEVBufferToolStripMenuItem = new System.Windows.Forms.Button();
            this.CAE = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.ABS.SuspendLayout();
            this.AOS.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ABS
            // 
            this.ABS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ABS.Controls.Add(this.IgnoreNotes);
            this.ABS.Controls.Add(this.FullVelocityMode);
            this.ABS.Controls.Add(this.SysExIgnore);
            this.ABS.Controls.Add(this.AllNotesIgnore);
            this.ABS.Location = new System.Drawing.Point(12, 79);
            this.ABS.Name = "ABS";
            this.ABS.Size = new System.Drawing.Size(345, 99);
            this.ABS.TabIndex = 11;
            this.ABS.TabStop = false;
            this.ABS.Text = "Ignore specific stuff/Set full velocity";
            // 
            // IgnoreNotes
            // 
            this.IgnoreNotes.AutoSize = true;
            this.IgnoreNotes.Location = new System.Drawing.Point(6, 57);
            this.IgnoreNotes.Name = "IgnoreNotes";
            this.IgnoreNotes.Size = new System.Drawing.Size(233, 17);
            this.IgnoreNotes.TabIndex = 7;
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
            // SysExIgnore
            // 
            this.SysExIgnore.AutoSize = true;
            this.SysExIgnore.Location = new System.Drawing.Point(6, 19);
            this.SysExIgnore.Name = "SysExIgnore";
            this.SysExIgnore.Size = new System.Drawing.Size(151, 17);
            this.SysExIgnore.TabIndex = 5;
            this.SysExIgnore.Text = "Ignore all SysEx messages";
            this.SysExIgnore.UseVisualStyleBackColor = true;
            this.SysExIgnore.CheckedChanged += new System.EventHandler(this.SysExIgnore_CheckedChanged);
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
            this.Limit88.TabIndex = 0;
            this.Limit88.Text = "Limit key range to 88 keys (Excluding drums channel)";
            this.Limit88.UseVisualStyleBackColor = true;
            this.Limit88.CheckedChanged += new System.EventHandler(this.Limit88_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RevbNChor);
            this.groupBox1.Controls.Add(this.IgnoreNotesInterval);
            this.groupBox1.Controls.Add(this.changeTheSizeOfTheEVBufferToolStripMenuItem);
            this.groupBox1.Location = new System.Drawing.Point(12, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 77);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other settings";
            // 
            // RevbNChor
            // 
            this.RevbNChor.Location = new System.Drawing.Point(91, 46);
            this.RevbNChor.Name = "RevbNChor";
            this.RevbNChor.Size = new System.Drawing.Size(160, 23);
            this.RevbNChor.TabIndex = 2;
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
            this.IgnoreNotesInterval.TabIndex = 1;
            this.IgnoreNotesInterval.Text = "Set velocity range to ignore";
            this.IgnoreNotesInterval.UseVisualStyleBackColor = true;
            this.IgnoreNotesInterval.Click += new System.EventHandler(this.IgnoreNotesInterval_Click);
            // 
            // changeTheSizeOfTheEVBufferToolStripMenuItem
            // 
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Location = new System.Drawing.Point(6, 17);
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Name = "changeTheSizeOfTheEVBufferToolStripMenuItem";
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Size = new System.Drawing.Size(160, 23);
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.TabIndex = 0;
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Text = "Change EV buffer size";
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.UseVisualStyleBackColor = true;
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Click += new System.EventHandler(this.changeTheSizeOfTheEVBufferToolStripMenuItem_Click);
            // 
            // CAE
            // 
            this.CAE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CAE.AutoSize = true;
            this.CAE.Enabled = false;
            this.CAE.Location = new System.Drawing.Point(15, 278);
            this.CAE.Name = "CAE";
            this.CAE.Size = new System.Drawing.Size(125, 13);
            this.CAE.TabIndex = 14;
            this.CAE.Text = "Current audio engine: {0}";
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(282, 273);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 13;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // MIDIEventsParserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 308);
            this.Controls.Add(this.ABS);
            this.Controls.Add(this.AOS);
            this.Controls.Add(this.groupBox1);
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
            this.AOS.ResumeLayout(false);
            this.AOS.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ABS;
        private System.Windows.Forms.CheckBox FullVelocityMode;
        private System.Windows.Forms.CheckBox SysExIgnore;
        private System.Windows.Forms.CheckBox AllNotesIgnore;
        private System.Windows.Forms.GroupBox AOS;
        private System.Windows.Forms.CheckBox CapFram;
        private System.Windows.Forms.CheckBox Limit88;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button IgnoreNotesInterval;
        private System.Windows.Forms.Button changeTheSizeOfTheEVBufferToolStripMenuItem;
        private System.Windows.Forms.Label CAE;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button RevbNChor;
        private System.Windows.Forms.CheckBox IgnoreNotes;
    }
}