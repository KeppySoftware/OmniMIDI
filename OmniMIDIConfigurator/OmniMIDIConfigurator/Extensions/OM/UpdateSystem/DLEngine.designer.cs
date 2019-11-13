namespace OmniMIDIConfigurator.Forms
{
    partial class DLEngine
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
            this.Status = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.DLPercent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Status
            // 
            this.Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status.ForeColor = System.Drawing.Color.White;
            this.Status.Location = new System.Drawing.Point(12, 7);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(209, 17);
            this.Status.TabIndex = 0;
            this.Status.Text = "Downloading update 0.0.0.0, please wait...";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(190, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Red;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.ForeColor = System.Drawing.Color.White;
            this.CancelBtn.Location = new System.Drawing.Point(209, 29);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(74, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // DLPercent
            // 
            this.DLPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DLPercent.ForeColor = System.Drawing.Color.LightBlue;
            this.DLPercent.Location = new System.Drawing.Point(227, 7);
            this.DLPercent.Name = "DLPercent";
            this.DLPercent.Size = new System.Drawing.Size(56, 17);
            this.DLPercent.TabIndex = 3;
            this.DLPercent.Text = "0%";
            this.DLPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DLEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(295, 63);
            this.ControlBox = false;
            this.Controls.Add(this.DLPercent);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Status);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(295, 63);
            this.MinimumSize = new System.Drawing.Size(295, 63);
            this.Name = "DLEngine";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keppy\'s Synthesizer - Update found";
            this.Load += new System.EventHandler(this.OmniMIDIUpdateDL_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label DLPercent;
    }
}