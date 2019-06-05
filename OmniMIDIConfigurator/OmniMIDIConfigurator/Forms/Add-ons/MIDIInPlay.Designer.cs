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
            this.RefreshInputs = new System.Windows.Forms.Button();
            this.DataLog = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ActivityPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(366, 79);
            this.label1.TabIndex = 0;
            this.label1.Text = "From within this test window, you can test if the MIDI inputs work properly,\r\nand" +
    " if OmniMIDI detects the events.\r\nKeep this window open to keep the stream alive" +
    ", or close it to close the stream too.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "MIDI input: ";
            // 
            // MIDIInList
            // 
            this.MIDIInList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIInList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MIDIInList.FormattingEnabled = true;
            this.MIDIInList.Location = new System.Drawing.Point(70, 115);
            this.MIDIInList.Name = "MIDIInList";
            this.MIDIInList.Size = new System.Drawing.Size(248, 21);
            this.MIDIInList.TabIndex = 2;
            this.MIDIInList.SelectedIndexChanged += new System.EventHandler(this.MIDIInList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Status:";
            // 
            // ActivityPanel
            // 
            this.ActivityPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActivityPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ActivityPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ActivityPanel.Controls.Add(this.ActivityLabel);
            this.ActivityPanel.Location = new System.Drawing.Point(70, 91);
            this.ActivityPanel.Name = "ActivityPanel";
            this.ActivityPanel.Size = new System.Drawing.Size(308, 21);
            this.ActivityPanel.TabIndex = 4;
            // 
            // ActivityLabel
            // 
            this.ActivityLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActivityLabel.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityLabel.ForeColor = System.Drawing.Color.White;
            this.ActivityLabel.Location = new System.Drawing.Point(0, 0);
            this.ActivityLabel.Name = "ActivityLabel";
            this.ActivityLabel.Size = new System.Drawing.Size(304, 17);
            this.ActivityLabel.TabIndex = 0;
            this.ActivityLabel.Text = "No activity.";
            this.ActivityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ActivityLabel.UseCompatibleTextRendering = true;
            // 
            // StatusTimer
            // 
            this.StatusTimer.Enabled = true;
            this.StatusTimer.Interval = 1;
            this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
            // 
            // RefreshInputs
            // 
            this.RefreshInputs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshInputs.Location = new System.Drawing.Point(321, 114);
            this.RefreshInputs.Name = "RefreshInputs";
            this.RefreshInputs.Size = new System.Drawing.Size(57, 23);
            this.RefreshInputs.TabIndex = 5;
            this.RefreshInputs.Text = "Refresh";
            this.RefreshInputs.UseVisualStyleBackColor = true;
            this.RefreshInputs.Click += new System.EventHandler(this.RefreshInputs_Click);
            // 
            // DataLog
            // 
            this.DataLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataLog.BackColor = System.Drawing.Color.Black;
            this.DataLog.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataLog.ForeColor = System.Drawing.Color.White;
            this.DataLog.Location = new System.Drawing.Point(12, 167);
            this.DataLog.Name = "DataLog";
            this.DataLog.Size = new System.Drawing.Size(366, 185);
            this.DataLog.TabIndex = 6;
            this.DataLog.Text = "AAAAAAAAAAA";
            this.DataLog.TextChanged += new System.EventHandler(this.DataLog_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Incoming data log:";
            // 
            // MIDIInPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(390, 364);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DataLog);
            this.Controls.Add(this.RefreshInputs);
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
        private System.Windows.Forms.Button RefreshInputs;
        private System.Windows.Forms.RichTextBox DataLog;
        private System.Windows.Forms.Label label4;
    }
}