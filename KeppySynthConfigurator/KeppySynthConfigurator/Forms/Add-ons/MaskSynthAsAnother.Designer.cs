﻿namespace KeppySynthConfigurator
{
    partial class MaskSynthAsAnother
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Names = new System.Windows.Forms.ComboBox();
            this.OK = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.AddNewNamePl0x = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 54);
            this.label1.TabIndex = 1;
            this.label1.Text = "Some apps might be hardwired to a specific synthesizer.\r\nYou can try and fool the" +
    "m by renaming Keppy\'s Synthesizer to another synthesizer/driver.\r\n\r\nSelect a mas" +
    "k in the list below:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mask:";
            // 
            // Names
            // 
            this.Names.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Names.FormattingEnabled = true;
            this.Names.Items.AddRange(new object[] {
            "AWE64 MIDI Synth",
            "CoolSoft VirtualMIDISynth",
            "Creative OPL3 FM",
            "Keppy\'s Synthesizer (Default)",
            "Microsoft GS Wavetable Synth",
            "SB AWE32 MIDI Synth",
            "SB Live! Synth A",
            "SB Live! Synth B",
            "VirtualMIDISynth #1",
            "VirtualMIDISynth #2",
            "VirtualMIDISynth #3",
            "VirtualMIDISynth #4",
            "Windows OPL3 Synth",
            "YMF262 Synth Emulator",
            "Yamaha S-YXG50 SoftSynthesizer"});
            this.Names.Location = new System.Drawing.Point(163, 75);
            this.Names.Name = "Names";
            this.Names.Size = new System.Drawing.Size(193, 21);
            this.Names.TabIndex = 3;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(398, 107);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(317, 107);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // AddNewNamePl0x
            // 
            this.AddNewNamePl0x.AutoSize = true;
            this.AddNewNamePl0x.LinkColor = System.Drawing.Color.Teal;
            this.AddNewNamePl0x.Location = new System.Drawing.Point(17, 112);
            this.AddNewNamePl0x.Name = "AddNewNamePl0x";
            this.AddNewNamePl0x.Size = new System.Drawing.Size(186, 13);
            this.AddNewNamePl0x.TabIndex = 6;
            this.AddNewNamePl0x.TabStop = true;
            this.AddNewNamePl0x.Text = "Can you add another name to the list?";
            this.AddNewNamePl0x.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AddNewNamePl0x_LinkClicked);
            // 
            // MaskSynthAsAnother
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(485, 142);
            this.Controls.Add(this.AddNewNamePl0x);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Names);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaskSynthAsAnother";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mask synthesizer as another";
            this.Load += new System.EventHandler(this.MaskSynthAsAnother_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Names;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.LinkLabel AddNewNamePl0x;
    }
}