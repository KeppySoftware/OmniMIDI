namespace KeppySynthConfigurator
{
    partial class RevbNChorForm
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
            this.ResDef = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.EnableRCOverride = new System.Windows.Forms.CheckBox();
            this.ReverbL = new System.Windows.Forms.Label();
            this.ChorusL = new System.Windows.Forms.Label();
            this.ReverbV = new System.Windows.Forms.NumericUpDown();
            this.ChorusV = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ReverbV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChorusV)).BeginInit();
            this.SuspendLayout();
            // 
            // ResDef
            // 
            this.ResDef.Location = new System.Drawing.Point(301, 107);
            this.ResDef.Name = "ResDef";
            this.ResDef.Size = new System.Drawing.Size(91, 23);
            this.ResDef.TabIndex = 8;
            this.ResDef.Text = "Restore default";
            this.ResDef.UseVisualStyleBackColor = true;
            this.ResDef.Click += new System.EventHandler(this.ResDef_Click);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(398, 107);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 7;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 54);
            this.label1.TabIndex = 6;
            this.label1.Text = "Here you can change the default reverb and chorus used when playing the MIDI note" +
    " events.\r\n\r\nYou first have to enable the override:          ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // EnableRCOverride
            // 
            this.EnableRCOverride.AutoSize = true;
            this.EnableRCOverride.Location = new System.Drawing.Point(318, 39);
            this.EnableRCOverride.Name = "EnableRCOverride";
            this.EnableRCOverride.Size = new System.Drawing.Size(15, 14);
            this.EnableRCOverride.TabIndex = 9;
            this.EnableRCOverride.UseVisualStyleBackColor = true;
            this.EnableRCOverride.CheckedChanged += new System.EventHandler(this.EnableRCOverride_CheckedChanged);
            // 
            // ReverbL
            // 
            this.ReverbL.AutoSize = true;
            this.ReverbL.Location = new System.Drawing.Point(198, 66);
            this.ReverbL.Name = "ReverbL";
            this.ReverbL.Size = new System.Drawing.Size(45, 13);
            this.ReverbL.TabIndex = 10;
            this.ReverbL.Text = "Reverb:";
            // 
            // ChorusL
            // 
            this.ChorusL.AutoSize = true;
            this.ChorusL.Location = new System.Drawing.Point(200, 85);
            this.ChorusL.Name = "ChorusL";
            this.ChorusL.Size = new System.Drawing.Size(43, 13);
            this.ChorusL.TabIndex = 11;
            this.ChorusL.Text = "Chorus:";
            // 
            // ReverbV
            // 
            this.ReverbV.Location = new System.Drawing.Point(245, 64);
            this.ReverbV.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.ReverbV.Name = "ReverbV";
            this.ReverbV.Size = new System.Drawing.Size(38, 20);
            this.ReverbV.TabIndex = 12;
            this.ReverbV.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ReverbV.ValueChanged += new System.EventHandler(this.ReverbV_ValueChanged);
            // 
            // ChorusV
            // 
            this.ChorusV.Location = new System.Drawing.Point(245, 83);
            this.ChorusV.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.ChorusV.Name = "ChorusV";
            this.ChorusV.Size = new System.Drawing.Size(38, 20);
            this.ChorusV.TabIndex = 13;
            this.ChorusV.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ChorusV.ValueChanged += new System.EventHandler(this.ChorusV_ValueChanged);
            // 
            // RevbNChorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(485, 142);
            this.Controls.Add(this.ChorusV);
            this.Controls.Add(this.ReverbV);
            this.Controls.Add(this.ChorusL);
            this.Controls.Add(this.ReverbL);
            this.Controls.Add(this.EnableRCOverride);
            this.Controls.Add(this.ResDef);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RevbNChorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reverb and chorus";
            this.Load += new System.EventHandler(this.RevbNChorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReverbV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChorusV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ResDef;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox EnableRCOverride;
        private System.Windows.Forms.Label ReverbL;
        private System.Windows.Forms.Label ChorusL;
        private System.Windows.Forms.NumericUpDown ReverbV;
        private System.Windows.Forms.NumericUpDown ChorusV;
    }
}