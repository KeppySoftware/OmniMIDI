namespace OmniMIDIMixerWindow.Forms
{
    partial class UpdateFreq
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
            this.UpdateFreqSet = new System.Windows.Forms.NumericUpDown();
            this.RefreshValMs = new System.Windows.Forms.Label();
            this.SaveValue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateFreqSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Set the update frequency in Hertz:";
            // 
            // UpdateFreqSet
            // 
            this.UpdateFreqSet.Location = new System.Drawing.Point(187, 12);
            this.UpdateFreqSet.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.UpdateFreqSet.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpdateFreqSet.Name = "UpdateFreqSet";
            this.UpdateFreqSet.Size = new System.Drawing.Size(120, 20);
            this.UpdateFreqSet.TabIndex = 1;
            this.UpdateFreqSet.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpdateFreqSet.ValueChanged += new System.EventHandler(this.UpdateFreqSet_ValueChanged);
            // 
            // RefreshValMs
            // 
            this.RefreshValMs.AutoSize = true;
            this.RefreshValMs.Location = new System.Drawing.Point(12, 47);
            this.RefreshValMs.Name = "RefreshValMs";
            this.RefreshValMs.Size = new System.Drawing.Size(188, 13);
            this.RefreshValMs.TabIndex = 2;
            this.RefreshValMs.Text = "The peak meter will refresh every ?ms.";
            // 
            // SaveValue
            // 
            this.SaveValue.Location = new System.Drawing.Point(232, 42);
            this.SaveValue.Name = "SaveValue";
            this.SaveValue.Size = new System.Drawing.Size(75, 23);
            this.SaveValue.TabIndex = 3;
            this.SaveValue.Text = "OK";
            this.SaveValue.UseVisualStyleBackColor = true;
            this.SaveValue.Click += new System.EventHandler(this.SaveValue_Click);
            // 
            // UpdateFreq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 79);
            this.Controls.Add(this.SaveValue);
            this.Controls.Add(this.RefreshValMs);
            this.Controls.Add(this.UpdateFreqSet);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateFreq";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set update frequency";
            this.Load += new System.EventHandler(this.UpdateFreq_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UpdateFreqSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown UpdateFreqSet;
        private System.Windows.Forms.Label RefreshValMs;
        private System.Windows.Forms.Button SaveValue;
    }
}