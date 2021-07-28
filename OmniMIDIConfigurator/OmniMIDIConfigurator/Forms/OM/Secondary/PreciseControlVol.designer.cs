namespace OmniMIDIConfigurator
{
    partial class PreciseControlVol
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
            this.VolTrackBar = new System.Windows.Forms.TrackBar();
            this.ReturnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.VolValN = new System.Windows.Forms.NumericUpDown();
            this.LogarithmVol = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.VolTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolValN)).BeginInit();
            this.SuspendLayout();
            // 
            // VolTrackBar
            // 
            this.VolTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VolTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.VolTrackBar.Location = new System.Drawing.Point(12, 37);
            this.VolTrackBar.Maximum = 10000;
            this.VolTrackBar.Name = "VolTrackBar";
            this.VolTrackBar.Size = new System.Drawing.Size(298, 45);
            this.VolTrackBar.TabIndex = 16;
            this.VolTrackBar.TickFrequency = 500;
            this.VolTrackBar.Scroll += new System.EventHandler(this.ValueChanged);
            // 
            // ReturnOK
            // 
            this.ReturnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnOK.Location = new System.Drawing.Point(235, 76);
            this.ReturnOK.Name = "ReturnOK";
            this.ReturnOK.Size = new System.Drawing.Size(75, 23);
            this.ReturnOK.TabIndex = 17;
            this.ReturnOK.Text = "OK";
            this.ReturnOK.UseVisualStyleBackColor = true;
            this.ReturnOK.Click += new System.EventHandler(this.ReturnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Set the exact value you want the knob to use:";
            // 
            // VolValN
            // 
            this.VolValN.Location = new System.Drawing.Point(252, 10);
            this.VolValN.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.VolValN.Name = "VolValN";
            this.VolValN.Size = new System.Drawing.Size(58, 20);
            this.VolValN.TabIndex = 20;
            this.VolValN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VolValN.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.VolValN.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // LogarithmVol
            // 
            this.LogarithmVol.AutoSize = true;
            this.LogarithmVol.Location = new System.Drawing.Point(12, 80);
            this.LogarithmVol.Name = "LogarithmVol";
            this.LogarithmVol.Size = new System.Drawing.Size(142, 17);
            this.LogarithmVol.TabIndex = 21;
            this.LogarithmVol.Text = "Use logarithmic changes";
            this.LogarithmVol.UseVisualStyleBackColor = true;
            this.LogarithmVol.CheckedChanged += new System.EventHandler(this.LogarithmVol_CheckedChanged);
            // 
            // PreciseControlVol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(318, 107);
            this.ControlBox = false;
            this.Controls.Add(this.LogarithmVol);
            this.Controls.Add(this.VolValN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReturnOK);
            this.Controls.Add(this.VolTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreciseControlVol";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fine tune the volume knob";
            ((System.ComponentModel.ISupportInitialize)(this.VolTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolValN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TrackBar VolTrackBar;
        private System.Windows.Forms.Button ReturnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown VolValN;
        private System.Windows.Forms.CheckBox LogarithmVol;
    }
}