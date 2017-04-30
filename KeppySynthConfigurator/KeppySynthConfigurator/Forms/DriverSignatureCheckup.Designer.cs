namespace KeppySynthConfigurator
{
    partial class DriverSignatureCheckup
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
            this.Driver32Current = new System.Windows.Forms.TextBox();
            this.Driver32Expected = new System.Windows.Forms.TextBox();
            this.Driver64Expected = new System.Windows.Forms.TextBox();
            this.Driver64Current = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Driver32Status = new System.Windows.Forms.PictureBox();
            this.Driver64Status = new System.Windows.Forms.PictureBox();
            this.ClosePls = new System.Windows.Forms.Button();
            this.BothDriverStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Driver32Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Driver64Status)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Signature for 32-bit driver:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Expected:";
            // 
            // Driver32Current
            // 
            this.Driver32Current.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Driver32Current.BackColor = System.Drawing.Color.Black;
            this.Driver32Current.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Driver32Current.ForeColor = System.Drawing.Color.White;
            this.Driver32Current.Location = new System.Drawing.Point(142, 12);
            this.Driver32Current.Name = "Driver32Current";
            this.Driver32Current.ReadOnly = true;
            this.Driver32Current.Size = new System.Drawing.Size(455, 18);
            this.Driver32Current.TabIndex = 2;
            this.Driver32Current.Text = "(Driver not installed)";
            this.Driver32Current.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Driver32Expected
            // 
            this.Driver32Expected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Driver32Expected.BackColor = System.Drawing.Color.Black;
            this.Driver32Expected.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Driver32Expected.ForeColor = System.Drawing.Color.White;
            this.Driver32Expected.Location = new System.Drawing.Point(142, 38);
            this.Driver32Expected.Name = "Driver32Expected";
            this.Driver32Expected.ReadOnly = true;
            this.Driver32Expected.Size = new System.Drawing.Size(455, 18);
            this.Driver32Expected.TabIndex = 3;
            this.Driver32Expected.Text = "(Driver not installed)";
            this.Driver32Expected.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Driver64Expected
            // 
            this.Driver64Expected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Driver64Expected.BackColor = System.Drawing.Color.Black;
            this.Driver64Expected.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Driver64Expected.ForeColor = System.Drawing.Color.White;
            this.Driver64Expected.Location = new System.Drawing.Point(142, 101);
            this.Driver64Expected.Name = "Driver64Expected";
            this.Driver64Expected.ReadOnly = true;
            this.Driver64Expected.Size = new System.Drawing.Size(455, 18);
            this.Driver64Expected.TabIndex = 7;
            this.Driver64Expected.Text = "(Driver not installed)";
            this.Driver64Expected.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Driver64Current
            // 
            this.Driver64Current.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Driver64Current.BackColor = System.Drawing.Color.Black;
            this.Driver64Current.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Driver64Current.ForeColor = System.Drawing.Color.White;
            this.Driver64Current.Location = new System.Drawing.Point(142, 75);
            this.Driver64Current.Name = "Driver64Current";
            this.Driver64Current.ReadOnly = true;
            this.Driver64Current.Size = new System.Drawing.Size(455, 18);
            this.Driver64Current.TabIndex = 6;
            this.Driver64Current.Text = "(Driver not installed)";
            this.Driver64Current.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Expected:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Signature for 64-bit driver:";
            // 
            // Driver32Status
            // 
            this.Driver32Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Driver32Status.Image = global::KeppySynthConfigurator.Properties.Resources.successicon;
            this.Driver32Status.Location = new System.Drawing.Point(603, 12);
            this.Driver32Status.Name = "Driver32Status";
            this.Driver32Status.Size = new System.Drawing.Size(44, 44);
            this.Driver32Status.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Driver32Status.TabIndex = 8;
            this.Driver32Status.TabStop = false;
            this.Driver32Status.Click += new System.EventHandler(this.Driver32Status_Click);
            // 
            // Driver64Status
            // 
            this.Driver64Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Driver64Status.Image = global::KeppySynthConfigurator.Properties.Resources.successicon;
            this.Driver64Status.Location = new System.Drawing.Point(603, 75);
            this.Driver64Status.Name = "Driver64Status";
            this.Driver64Status.Size = new System.Drawing.Size(44, 44);
            this.Driver64Status.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Driver64Status.TabIndex = 9;
            this.Driver64Status.TabStop = false;
            this.Driver64Status.Click += new System.EventHandler(this.Driver64Status_Click);
            // 
            // ClosePls
            // 
            this.ClosePls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClosePls.Location = new System.Drawing.Point(572, 131);
            this.ClosePls.Name = "ClosePls";
            this.ClosePls.Size = new System.Drawing.Size(75, 23);
            this.ClosePls.TabIndex = 10;
            this.ClosePls.Text = "OK";
            this.ClosePls.UseVisualStyleBackColor = true;
            this.ClosePls.Click += new System.EventHandler(this.ClosePls_Click);
            // 
            // BothDriverStatus
            // 
            this.BothDriverStatus.BackColor = System.Drawing.Color.Transparent;
            this.BothDriverStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BothDriverStatus.ForeColor = System.Drawing.Color.Pink;
            this.BothDriverStatus.Location = new System.Drawing.Point(13, 122);
            this.BothDriverStatus.Name = "BothDriverStatus";
            this.BothDriverStatus.Size = new System.Drawing.Size(553, 41);
            this.BothDriverStatus.TabIndex = 11;
            this.BothDriverStatus.Text = "I don\'t know the status, derp.";
            this.BothDriverStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BothDriverStatus.Click += new System.EventHandler(this.BothDriverStatus_Click);
            // 
            // DriverSignatureCheckup
            // 
            this.AcceptButton = this.ClosePls;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(659, 166);
            this.Controls.Add(this.BothDriverStatus);
            this.Controls.Add(this.ClosePls);
            this.Controls.Add(this.Driver64Status);
            this.Controls.Add(this.Driver32Status);
            this.Controls.Add(this.Driver64Expected);
            this.Controls.Add(this.Driver64Current);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Driver32Expected);
            this.Controls.Add(this.Driver32Current);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DriverSignatureCheckup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Driver signature checkup";
            this.Load += new System.EventHandler(this.DriverSignatureCheckup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Driver32Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Driver64Status)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Driver32Current;
        private System.Windows.Forms.TextBox Driver32Expected;
        private System.Windows.Forms.TextBox Driver64Expected;
        private System.Windows.Forms.TextBox Driver64Current;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox Driver32Status;
        private System.Windows.Forms.PictureBox Driver64Status;
        private System.Windows.Forms.Button ClosePls;
        private System.Windows.Forms.Label BothDriverStatus;
    }
}