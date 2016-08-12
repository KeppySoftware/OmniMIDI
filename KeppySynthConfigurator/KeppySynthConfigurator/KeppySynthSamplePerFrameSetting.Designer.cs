namespace KeppySynthConfigurator
{
    partial class KeppySynthSamplePerFrameSetting
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
            this.Confirm = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.LeSetting = new System.Windows.Forms.NumericUpDown();
            this.DefVal = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LeSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Change the maximum samples per frame value of the driver, to remove clicking nois" +
    "es or to reduce the latency.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(461, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "WARNING: Playing with this value can potentially cause crashes.\r\nI (KaleidonKep99" +
    ") am not responsible for data loss or problems like that.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Confirm
            // 
            this.Confirm.Location = new System.Drawing.Point(398, 107);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(75, 23);
            this.Confirm.TabIndex = 2;
            this.Confirm.Text = "OK";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(147, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Maximum samples per frame:";
            // 
            // LeSetting
            // 
            this.LeSetting.Location = new System.Drawing.Point(294, 75);
            this.LeSetting.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.LeSetting.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LeSetting.Name = "LeSetting";
            this.LeSetting.Size = new System.Drawing.Size(45, 21);
            this.LeSetting.TabIndex = 4;
            this.LeSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LeSetting.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // DefVal
            // 
            this.DefVal.Location = new System.Drawing.Point(12, 107);
            this.DefVal.Name = "DefVal";
            this.DefVal.Size = new System.Drawing.Size(89, 23);
            this.DefVal.TabIndex = 5;
            this.DefVal.Text = "Default value";
            this.DefVal.UseVisualStyleBackColor = true;
            this.DefVal.Click += new System.EventHandler(this.DefVal_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(107, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(285, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "The smaller the value, the less latency you have.\r\n(You\'ll probably be forced to " +
    "increase the buffer size)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KeppyDriverSamplePerFrameSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(485, 142);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DefVal);
            this.Controls.Add(this.LeSetting);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeppyDriverSamplePerFrameSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change the maximum samples per frame";
            this.Load += new System.EventHandler(this.KeppyDriverSamplePerFrameSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LeSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown LeSetting;
        private System.Windows.Forms.Button DefVal;
        private System.Windows.Forms.Label label4;
    }
}