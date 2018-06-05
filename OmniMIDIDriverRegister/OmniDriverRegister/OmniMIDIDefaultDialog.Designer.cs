namespace OmniMIDIDriverRegister
{
    partial class OmniMIDIDefaultDialog
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
            this.WD = new System.Windows.Forms.Label();
            this.RD = new System.Windows.Forms.Button();
            this.UnRD = new System.Windows.Forms.Button();
            this.VER = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WD
            // 
            this.WD.Dock = System.Windows.Forms.DockStyle.Top;
            this.WD.Location = new System.Drawing.Point(0, 0);
            this.WD.Name = "WD";
            this.WD.Size = new System.Drawing.Size(284, 44);
            this.WD.TabIndex = 0;
            this.WD.Text = "What do you want to do with OmniMIDI?";
            this.WD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RD
            // 
            this.RD.Location = new System.Drawing.Point(12, 47);
            this.RD.Name = "RD";
            this.RD.Size = new System.Drawing.Size(75, 38);
            this.RD.TabIndex = 1;
            this.RD.Text = "Register\r\ndriver";
            this.RD.UseVisualStyleBackColor = true;
            this.RD.Click += new System.EventHandler(this.RD_Click);
            // 
            // UnRD
            // 
            this.UnRD.Location = new System.Drawing.Point(197, 47);
            this.UnRD.Name = "UnRD";
            this.UnRD.Size = new System.Drawing.Size(75, 38);
            this.UnRD.TabIndex = 2;
            this.UnRD.Text = "Unregister\r\ndriver";
            this.UnRD.UseVisualStyleBackColor = true;
            this.UnRD.Click += new System.EventHandler(this.UnRD_Click);
            // 
            // VER
            // 
            this.VER.Location = new System.Drawing.Point(93, 47);
            this.VER.Name = "VER";
            this.VER.Size = new System.Drawing.Size(98, 38);
            this.VER.TabIndex = 3;
            this.VER.Text = "KSDriverRegister\r\nv0.0.0.0";
            this.VER.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OmniMIDIDefaultDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 96);
            this.Controls.Add(this.VER);
            this.Controls.Add(this.UnRD);
            this.Controls.Add(this.RD);
            this.Controls.Add(this.WD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OmniMIDIDefaultDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OmniMIDI Driver Register/Unregister tool";
            this.Load += new System.EventHandler(this.KSDefaultDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label WD;
        private System.Windows.Forms.Button RD;
        private System.Windows.Forms.Button UnRD;
        private System.Windows.Forms.Label VER;
    }
}