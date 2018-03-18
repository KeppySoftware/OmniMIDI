namespace KeppySynthConfigurator
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
            this.VolIntView = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.VolTrackBar)).BeginInit();
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
            this.VolTrackBar.Size = new System.Drawing.Size(314, 45);
            this.VolTrackBar.TabIndex = 16;
            this.VolTrackBar.TickFrequency = 500;
            this.VolTrackBar.Scroll += new System.EventHandler(this.VolTrackBar_Scroll);
            // 
            // ReturnOK
            // 
            this.ReturnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnOK.Location = new System.Drawing.Point(251, 90);
            this.ReturnOK.Name = "ReturnOK";
            this.ReturnOK.Size = new System.Drawing.Size(75, 23);
            this.ReturnOK.TabIndex = 17;
            this.ReturnOK.Text = "OK";
            this.ReturnOK.UseVisualStyleBackColor = true;
            this.ReturnOK.Click += new System.EventHandler(this.ReturnOK_Click);
            // 
            // VolIntView
            // 
            this.VolIntView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VolIntView.BackColor = System.Drawing.Color.Transparent;
            this.VolIntView.Enabled = false;
            this.VolIntView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolIntView.Location = new System.Drawing.Point(13, 69);
            this.VolIntView.Name = "VolIntView";
            this.VolIntView.Size = new System.Drawing.Size(313, 12);
            this.VolIntView.TabIndex = 18;
            this.VolIntView.Text = "000.00%";
            this.VolIntView.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // PreciseControlVol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(338, 125);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VolIntView);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TrackBar VolTrackBar;
        private System.Windows.Forms.Button ReturnOK;
        public System.Windows.Forms.Label VolIntView;
        private System.Windows.Forms.Label label1;
    }
}