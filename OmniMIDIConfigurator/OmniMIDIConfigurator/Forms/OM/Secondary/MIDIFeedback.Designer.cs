
namespace OmniMIDIConfigurator
{
    partial class MIDIFeedback
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
            this.MIDIOutDevs = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.EnableFeedback = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(314, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "You can make OmniMIDI forward the events to another MIDI out device, like a virtu" +
    "al cable (e.g. LoopMIDI) or another MIDI synthesizer (e.g. VirtualMIDISynth or M" +
    "icrosoft Wavetable Synth).";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Feedback device:";
            // 
            // MIDIOutDevs
            // 
            this.MIDIOutDevs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIOutDevs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MIDIOutDevs.FormattingEnabled = true;
            this.MIDIOutDevs.Location = new System.Drawing.Point(12, 77);
            this.MIDIOutDevs.Name = "MIDIOutDevs";
            this.MIDIOutDevs.Size = new System.Drawing.Size(314, 21);
            this.MIDIOutDevs.TabIndex = 2;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(251, 104);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // EnableFeedback
            // 
            this.EnableFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EnableFeedback.AutoSize = true;
            this.EnableFeedback.Location = new System.Drawing.Point(12, 108);
            this.EnableFeedback.Name = "EnableFeedback";
            this.EnableFeedback.Size = new System.Drawing.Size(136, 17);
            this.EnableFeedback.TabIndex = 4;
            this.EnableFeedback.Text = "Enable feedback mode";
            this.EnableFeedback.UseVisualStyleBackColor = true;
            // 
            // MIDIFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 139);
            this.ControlBox = false;
            this.Controls.Add(this.EnableFeedback);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MIDIOutDevs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MIDIFeedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MIDI feedback mode";
            this.Load += new System.EventHandler(this.MIDIFeedback_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MIDIOutDevs;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.CheckBox EnableFeedback;
    }
}