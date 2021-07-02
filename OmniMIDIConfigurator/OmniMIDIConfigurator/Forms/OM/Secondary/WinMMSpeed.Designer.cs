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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinMMSpeed));
            this.label1 = new System.Windows.Forms.Label();
            this.ReturnOK = new System.Windows.Forms.Button();
            this.SpeedHackVal = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.DefaultBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedHackVal)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Speed percentage:";
            // 
            // ReturnOK
            // 
            this.ReturnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnOK.Location = new System.Drawing.Point(235, 153);
            this.ReturnOK.Name = "ReturnOK";
            this.ReturnOK.Size = new System.Drawing.Size(75, 23);
            this.ReturnOK.TabIndex = 22;
            this.ReturnOK.Text = "OK";
            this.ReturnOK.UseVisualStyleBackColor = true;
            this.ReturnOK.Click += new System.EventHandler(this.ReturnOK_Click);
            // 
            // SpeedHackVal
            // 
            this.SpeedHackVal.DecimalPlaces = 6;
            this.SpeedHackVal.Location = new System.Drawing.Point(147, 109);
            this.SpeedHackVal.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.SpeedHackVal.Name = "SpeedHackVal";
            this.SpeedHackVal.Size = new System.Drawing.Size(120, 20);
            this.SpeedHackVal.TabIndex = 25;
            this.SpeedHackVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(298, 83);
            this.label2.TabIndex = 26;
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DefaultBtn
            // 
            this.DefaultBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DefaultBtn.Location = new System.Drawing.Point(154, 153);
            this.DefaultBtn.Name = "DefaultBtn";
            this.DefaultBtn.Size = new System.Drawing.Size(75, 23);
            this.DefaultBtn.TabIndex = 27;
            this.DefaultBtn.Text = "Default";
            this.DefaultBtn.UseVisualStyleBackColor = true;
            this.DefaultBtn.Click += new System.EventHandler(this.DefaultBtn_Click);
            // 
            // WinMMSpeed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 188);
            this.ControlBox = false;
            this.Controls.Add(this.DefaultBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SpeedHackVal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReturnOK);
            this.Name = "WinMMSpeed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windows Multimedia Wrapper SpeedHack";
            ((System.ComponentModel.ISupportInitialize)(this.SpeedHackVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ReturnOK;
        private System.Windows.Forms.NumericUpDown SpeedHackVal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DefaultBtn;
    }
}