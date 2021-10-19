
namespace OmniMIDIConfigurator
{
    partial class XAOutputSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XAOutputSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SPFVal = new System.Windows.Forms.NumericUpDown();
            this.OKBtn = new System.Windows.Forms.Button();
            this.ResetBtn = new System.Windows.Forms.Button();
            this.SPFSweepRatioVal = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.EstLat = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SPFVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SPFSweepRatioVal)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(397, 105);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Samples per frame:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SPFVal
            // 
            this.SPFVal.Location = new System.Drawing.Point(206, 138);
            this.SPFVal.Maximum = new decimal(new int[] {
            262144,
            0,
            0,
            0});
            this.SPFVal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SPFVal.Name = "SPFVal";
            this.SPFVal.Size = new System.Drawing.Size(124, 23);
            this.SPFVal.TabIndex = 2;
            this.SPFVal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SPFVal.ValueChanged += new System.EventHandler(this.SPFVal_ValueChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(323, 252);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(87, 27);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // ResetBtn
            // 
            this.ResetBtn.Location = new System.Drawing.Point(229, 252);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(87, 27);
            this.ResetBtn.TabIndex = 4;
            this.ResetBtn.Text = "Reset";
            this.ResetBtn.UseVisualStyleBackColor = true;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // SPFSweepRatioVal
            // 
            this.SPFSweepRatioVal.Location = new System.Drawing.Point(206, 164);
            this.SPFSweepRatioVal.Maximum = new decimal(new int[] {
            262144,
            0,
            0,
            0});
            this.SPFSweepRatioVal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SPFSweepRatioVal.Name = "SPFSweepRatioVal";
            this.SPFSweepRatioVal.Size = new System.Drawing.Size(124, 23);
            this.SPFSweepRatioVal.TabIndex = 6;
            this.SPFSweepRatioVal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SPFSweepRatioVal.ValueChanged += new System.EventHandler(this.SPFSweepRatioVal_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Sweep ratio:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // EstLat
            // 
            this.EstLat.Location = new System.Drawing.Point(14, 203);
            this.EstLat.Name = "EstLat";
            this.EstLat.Size = new System.Drawing.Size(397, 27);
            this.EstLat.TabIndex = 7;
            this.EstLat.Text = "Estimated latency: 0.0ms";
            this.EstLat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // XAOutputSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 292);
            this.Controls.Add(this.EstLat);
            this.Controls.Add(this.SPFSweepRatioVal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ResetBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SPFVal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XAOutputSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Microsoft XAudio 2.9 settings";
            ((System.ComponentModel.ISupportInitialize)(this.SPFVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SPFSweepRatioVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown SPFVal;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button ResetBtn;
        private System.Windows.Forms.NumericUpDown SPFSweepRatioVal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label EstLat;
    }
}