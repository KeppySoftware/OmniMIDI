namespace KeppySynthConfigurator
{
    partial class KeppySynthVelocityIntervals
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeppySynthVelocityIntervals));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HiVel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.PrevSett = new System.Windows.Forms.Label();
            this.LoVel = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.HiVel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoVel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 53);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Lowest:";
            // 
            // HiVel
            // 
            this.HiVel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.HiVel.Location = new System.Drawing.Point(313, 74);
            this.HiVel.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.HiVel.Name = "HiVel";
            this.HiVel.Size = new System.Drawing.Size(55, 20);
            this.HiVel.TabIndex = 4;
            this.HiVel.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.HiVel.ValueChanged += new System.EventHandler(this.HiVel_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Highest:";
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyBtn.Location = new System.Drawing.Point(398, 107);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(75, 23);
            this.ApplyBtn.TabIndex = 5;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Location = new System.Drawing.Point(317, 107);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // PrevSett
            // 
            this.PrevSett.AutoSize = true;
            this.PrevSett.Location = new System.Drawing.Point(15, 112);
            this.PrevSett.Name = "PrevSett";
            this.PrevSett.Size = new System.Drawing.Size(145, 13);
            this.PrevSett.TabIndex = 7;
            this.PrevSett.Text = "Previous settings: Lo. 1, Hi. 1";
            // 
            // LoVel
            // 
            this.LoVel.Cursor = System.Windows.Forms.Cursors.Default;
            this.LoVel.Location = new System.Drawing.Point(169, 74);
            this.LoVel.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.LoVel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LoVel.Name = "LoVel";
            this.LoVel.Size = new System.Drawing.Size(55, 20);
            this.LoVel.TabIndex = 8;
            this.LoVel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LoVel.ValueChanged += new System.EventHandler(this.LoVel_ValueChanged);
            // 
            // KeppySynthVelocityIntervals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(485, 142);
            this.Controls.Add(this.LoVel);
            this.Controls.Add(this.PrevSett);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.HiVel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeppySynthVelocityIntervals";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set velocity values to ignore";
            this.Load += new System.EventHandler(this.KeppySynthVelocityIntervals_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HiVel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoVel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown HiVel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label PrevSett;
        private System.Windows.Forms.NumericUpDown LoVel;
    }
}