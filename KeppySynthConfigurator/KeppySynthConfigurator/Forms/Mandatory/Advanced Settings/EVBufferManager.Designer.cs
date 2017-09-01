namespace KeppySynthConfigurator
{
    partial class EVBufferManager
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
            this.BytesVal = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.RatioVal = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ApplySettings = new System.Windows.Forms.Button();
            this.GetRAMSize = new System.Windows.Forms.CheckBox();
            this.WarningPanel = new System.Windows.Forms.Panel();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.WarningSign = new System.Windows.Forms.PictureBox();
            this.ResetSettings = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ExemptRealTime = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.BytesVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RatioVal)).BeginInit();
            this.WarningPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WarningSign)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(423, 34);
            this.label1.TabIndex = 0;
            this.label1.Text = "Set a custom size/ratio for the EV Buffer here.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(0, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(423, 39);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tampering with this setting may lead to\r\nunexpected data loss and system crashes," +
    " \r\nand you\'re responsible for anything that might happen.\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BytesVal
            // 
            this.BytesVal.Location = new System.Drawing.Point(112, 97);
            this.BytesVal.Maximum = new decimal(new int[] {
            0,
            128,
            0,
            0});
            this.BytesVal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BytesVal.Name = "BytesVal";
            this.BytesVal.Size = new System.Drawing.Size(101, 20);
            this.BytesVal.TabIndex = 2;
            this.BytesVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BytesVal.ThousandsSeparator = true;
            this.BytesVal.Value = new decimal(new int[] {
            0,
            128,
            0,
            0});
            this.BytesVal.ValueChanged += new System.EventHandler(this.BytesVal_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "/";
            // 
            // RatioVal
            // 
            this.RatioVal.Location = new System.Drawing.Point(237, 97);
            this.RatioVal.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.RatioVal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RatioVal.Name = "RatioVal";
            this.RatioVal.Size = new System.Drawing.Size(71, 20);
            this.RatioVal.TabIndex = 4;
            this.RatioVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RatioVal.ThousandsSeparator = true;
            this.RatioVal.Value = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.RatioVal.ValueChanged += new System.EventHandler(this.RatioVal_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(148, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "bytes";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(261, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "ratio";
            // 
            // ApplySettings
            // 
            this.ApplySettings.Location = new System.Drawing.Point(336, 170);
            this.ApplySettings.Name = "ApplySettings";
            this.ApplySettings.Size = new System.Drawing.Size(75, 23);
            this.ApplySettings.TabIndex = 7;
            this.ApplySettings.Text = "Apply";
            this.ApplySettings.UseVisualStyleBackColor = true;
            this.ApplySettings.Click += new System.EventHandler(this.ApplySettings_Click);
            // 
            // GetRAMSize
            // 
            this.GetRAMSize.AutoSize = true;
            this.GetRAMSize.Location = new System.Drawing.Point(12, 174);
            this.GetRAMSize.Name = "GetRAMSize";
            this.GetRAMSize.Size = new System.Drawing.Size(167, 17);
            this.GetRAMSize.TabIndex = 8;
            this.GetRAMSize.Text = "Get maximum bytes from RAM";
            this.GetRAMSize.UseVisualStyleBackColor = true;
            this.GetRAMSize.CheckedChanged += new System.EventHandler(this.GetRAMSize_CheckedChanged);
            // 
            // WarningPanel
            // 
            this.WarningPanel.Controls.Add(this.WarningLabel);
            this.WarningPanel.Controls.Add(this.WarningSign);
            this.WarningPanel.Location = new System.Drawing.Point(12, 128);
            this.WarningPanel.Name = "WarningPanel";
            this.WarningPanel.Size = new System.Drawing.Size(399, 32);
            this.WarningPanel.TabIndex = 9;
            this.WarningPanel.Visible = false;
            // 
            // WarningLabel
            // 
            this.WarningLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WarningLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WarningLabel.Location = new System.Drawing.Point(32, 0);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(367, 32);
            this.WarningLabel.TabIndex = 1;
            this.WarningLabel.Text = "SAS";
            this.WarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WarningLabel.Click += new System.EventHandler(this.WarningLabel_Click);
            // 
            // WarningSign
            // 
            this.WarningSign.Dock = System.Windows.Forms.DockStyle.Left;
            this.WarningSign.ErrorImage = global::KeppySynthConfigurator.Properties.Resources.wi;
            this.WarningSign.Image = global::KeppySynthConfigurator.Properties.Resources.wi;
            this.WarningSign.Location = new System.Drawing.Point(0, 0);
            this.WarningSign.Name = "WarningSign";
            this.WarningSign.Size = new System.Drawing.Size(32, 32);
            this.WarningSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WarningSign.TabIndex = 0;
            this.WarningSign.TabStop = false;
            // 
            // ResetSettings
            // 
            this.ResetSettings.Location = new System.Drawing.Point(255, 170);
            this.ResetSettings.Name = "ResetSettings";
            this.ResetSettings.Size = new System.Drawing.Size(75, 23);
            this.ResetSettings.TabIndex = 8;
            this.ResetSettings.Text = "Reset";
            this.ResetSettings.UseVisualStyleBackColor = true;
            this.ResetSettings.Click += new System.EventHandler(this.ResetSettings_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExemptRealTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 205);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(423, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ExemptRealTime
            // 
            this.ExemptRealTime.Enabled = false;
            this.ExemptRealTime.Name = "ExemptRealTime";
            this.ExemptRealTime.Size = new System.Drawing.Size(226, 17);
            this.ExemptRealTime.Text = "You can\'t change this setting in real-time.";
            // 
            // EVBufferManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 227);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ResetSettings);
            this.Controls.Add(this.WarningPanel);
            this.Controls.Add(this.GetRAMSize);
            this.Controls.Add(this.ApplySettings);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RatioVal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BytesVal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EVBufferManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change size of the EV buffer";
            this.Load += new System.EventHandler(this.EVBufferManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BytesVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RatioVal)).EndInit();
            this.WarningPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarningSign)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown BytesVal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown RatioVal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ApplySettings;
        private System.Windows.Forms.CheckBox GetRAMSize;
        private System.Windows.Forms.Panel WarningPanel;
        private System.Windows.Forms.PictureBox WarningSign;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.Button ResetSettings;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ExemptRealTime;
    }
}