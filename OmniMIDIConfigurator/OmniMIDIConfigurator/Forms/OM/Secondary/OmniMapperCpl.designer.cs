namespace OmniMIDIConfigurator
{
    partial class OmniMapperCpl
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
            this.MIDIOutList = new System.Windows.Forms.ComboBox();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.PFLabel = new System.Windows.Forms.Label();
            this.Pi386 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PAMD64 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PAarch64 = new System.Windows.Forms.PictureBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Pi386)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PAMD64)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PAarch64)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map mapper to device:";
            // 
            // MIDIOutList
            // 
            this.MIDIOutList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIOutList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MIDIOutList.FormattingEnabled = true;
            this.MIDIOutList.Location = new System.Drawing.Point(14, 32);
            this.MIDIOutList.Name = "MIDIOutList";
            this.MIDIOutList.Size = new System.Drawing.Size(465, 23);
            this.MIDIOutList.TabIndex = 3;
            this.MIDIOutList.SelectedIndexChanged += new System.EventHandler(this.MIDIOutList_SelectedIndexChanged);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBtn.Location = new System.Drawing.Point(393, 155);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(87, 27);
            this.ApplyBtn.TabIndex = 4;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // PFLabel
            // 
            this.PFLabel.AutoSize = true;
            this.PFLabel.Location = new System.Drawing.Point(10, 77);
            this.PFLabel.Name = "PFLabel";
            this.PFLabel.Size = new System.Drawing.Size(119, 15);
            this.PFLabel.TabIndex = 5;
            this.PFLabel.Text = "Supported platforms:";
            // 
            // Pi386
            // 
            this.Pi386.Location = new System.Drawing.Point(14, 98);
            this.Pi386.Name = "Pi386";
            this.Pi386.Size = new System.Drawing.Size(19, 18);
            this.Pi386.TabIndex = 6;
            this.Pi386.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "x86 (i386)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "x64 (AMD64)";
            // 
            // PAMD64
            // 
            this.PAMD64.Location = new System.Drawing.Point(14, 123);
            this.PAMD64.Name = "PAMD64";
            this.PAMD64.Size = new System.Drawing.Size(19, 18);
            this.PAMD64.TabIndex = 8;
            this.PAMD64.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "ARM64 (AArch64)";
            // 
            // PAarch64
            // 
            this.PAarch64.Location = new System.Drawing.Point(14, 149);
            this.PAarch64.Name = "PAarch64";
            this.PAarch64.Size = new System.Drawing.Size(19, 18);
            this.PAarch64.TabIndex = 10;
            this.PAarch64.TabStop = false;
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(299, 155);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(87, 27);
            this.CancelBtn.TabIndex = 12;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OmniMapperCpl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(493, 194);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PAarch64);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PAMD64);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Pi386);
            this.Controls.Add(this.PFLabel);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.MIDIOutList);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OmniMapperCpl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change OmniMapper settings";
            this.Load += new System.EventHandler(this.OmniMapperCpl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pi386)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PAMD64)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PAarch64)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox MIDIOutList;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Label PFLabel;
        private System.Windows.Forms.PictureBox Pi386;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox PAMD64;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox PAarch64;
        private System.Windows.Forms.Button CancelBtn;
    }
}