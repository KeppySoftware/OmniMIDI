namespace OmniMIDIConfigurator
{
    partial class WinMMSpeed
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
            this.ReturnOK = new System.Windows.Forms.Button();
            this.SDTrackBar = new System.Windows.Forms.TrackBar();
            this.SDVal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SDTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Select the slow down ratio:";
            // 
            // ReturnOK
            // 
            this.ReturnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnOK.Location = new System.Drawing.Point(235, 76);
            this.ReturnOK.Name = "ReturnOK";
            this.ReturnOK.Size = new System.Drawing.Size(75, 23);
            this.ReturnOK.TabIndex = 22;
            this.ReturnOK.Text = "OK";
            this.ReturnOK.UseVisualStyleBackColor = true;
            this.ReturnOK.Click += new System.EventHandler(this.ReturnOK_Click);
            // 
            // SDTrackBar
            // 
            this.SDTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SDTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.SDTrackBar.Location = new System.Drawing.Point(12, 37);
            this.SDTrackBar.Maximum = 10000;
            this.SDTrackBar.Minimum = 1;
            this.SDTrackBar.Name = "SDTrackBar";
            this.SDTrackBar.Size = new System.Drawing.Size(298, 45);
            this.SDTrackBar.TabIndex = 21;
            this.SDTrackBar.TickFrequency = 250;
            this.SDTrackBar.Value = 1;
            this.SDTrackBar.Scroll += new System.EventHandler(this.SDTrackBar_Scroll);
            // 
            // SDVal
            // 
            this.SDVal.Location = new System.Drawing.Point(235, 7);
            this.SDVal.Name = "SDVal";
            this.SDVal.Size = new System.Drawing.Size(75, 23);
            this.SDVal.TabIndex = 24;
            this.SDVal.Text = "100%";
            this.SDVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WinMMSpeed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 111);
            this.ControlBox = false;
            this.Controls.Add(this.SDVal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReturnOK);
            this.Controls.Add(this.SDTrackBar);
            this.Name = "WinMMSpeed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Slow down the Windows Multimedia Wrapper";
            ((System.ComponentModel.ISupportInitialize)(this.SDTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ReturnOK;
        public System.Windows.Forms.TrackBar SDTrackBar;
        private System.Windows.Forms.Label SDVal;
    }
}