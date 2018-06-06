namespace OmniMIDIConfigurator
{
    partial class MIDIInPlay
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MIDIInList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ActivityPanel = new System.Windows.Forms.Panel();
            this.ActivityLabel = new System.Windows.Forms.Label();
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.ActivityPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(315, 94);
            this.label1.TabIndex = 0;
            this.label1.Text = "From within this test window, you can test if the MIDI inputs work properly,\r\nand" +
    " if OmniMIDI detects the events.\r\nKeep this window open to keep the stream alive" +
    ", or close it to close the stream too.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "MIDI input: ";
            // 
            // MIDIInList
            // 
            this.MIDIInList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIInList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MIDIInList.FormattingEnabled = true;
            this.MIDIInList.Location = new System.Drawing.Point(70, 120);
            this.MIDIInList.Name = "MIDIInList";
            this.MIDIInList.Size = new System.Drawing.Size(232, 21);
            this.MIDIInList.TabIndex = 2;
            this.MIDIInList.SelectedIndexChanged += new System.EventHandler(this.MIDIInList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Status:";
            // 
            // ActivityPanel
            // 
            this.ActivityPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActivityPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ActivityPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ActivityPanel.Controls.Add(this.ActivityLabel);
            this.ActivityPanel.Location = new System.Drawing.Point(70, 96);
            this.ActivityPanel.Name = "ActivityPanel";
            this.ActivityPanel.Size = new System.Drawing.Size(232, 21);
            this.ActivityPanel.TabIndex = 4;
            // 
            // ActivityLabel
            // 
            this.ActivityLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActivityLabel.Font = new System.Drawing.Font("Lucida Console", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityLabel.ForeColor = System.Drawing.Color.White;
            this.ActivityLabel.Location = new System.Drawing.Point(0, 0);
            this.ActivityLabel.Name = "ActivityLabel";
            this.ActivityLabel.Size = new System.Drawing.Size(228, 17);
            this.ActivityLabel.TabIndex = 0;
            this.ActivityLabel.Text = "No activity.";
            this.ActivityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatusTimer
            // 
            this.StatusTimer.Enabled = true;
            this.StatusTimer.Interval = 1;
            this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
            // 
            // MIDIInPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(314, 153);
            this.Controls.Add(this.ActivityPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MIDIInList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MIDIInPlay";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test MIDI input and output";
            this.Load += new System.EventHandler(this.MIDIInPlay_Load);
            this.ActivityPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MIDIInList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel ActivityPanel;
        private System.Windows.Forms.Label ActivityLabel;
        private System.Windows.Forms.Timer StatusTimer;
    }
}