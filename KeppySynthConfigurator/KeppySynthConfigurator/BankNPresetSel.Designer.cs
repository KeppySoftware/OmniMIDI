namespace KeppySynthConfigurator
{
    partial class BankNPresetSel
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
            this.ConfirmBut = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BankVal = new System.Windows.Forms.NumericUpDown();
            this.PresetVal = new System.Windows.Forms.NumericUpDown();
            this.SelectedSFLabel = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.WikipediaLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.BankVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PresetVal)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select a bank and a preset, from 0 to 127.\r\n\r\nUse \"Bank 0\" and \"Preset 0\" " +
    "for the standard \"Acoustic Grand Piano\",\r\nor if you don\'t know which one you sho" +
    "uld use.";
            // 
            // ConfirmBut
            // 
            this.ConfirmBut.Location = new System.Drawing.Point(277, 106);
            this.ConfirmBut.Name = "ConfirmBut";
            this.ConfirmBut.Size = new System.Drawing.Size(75, 23);
            this.ConfirmBut.TabIndex = 1;
            this.ConfirmBut.Text = "Confirm";
            this.ConfirmBut.UseVisualStyleBackColor = true;
            this.ConfirmBut.Click += new System.EventHandler(this.ConfirmBut_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bank";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(186, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Preset";
            // 
            // BankVal
            // 
            this.BankVal.Location = new System.Drawing.Point(45, 75);
            this.BankVal.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.BankVal.Name = "BankVal";
            this.BankVal.Size = new System.Drawing.Size(120, 20);
            this.BankVal.TabIndex = 4;
            this.BankVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PresetVal
            // 
            this.PresetVal.Location = new System.Drawing.Point(225, 75);
            this.PresetVal.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.PresetVal.Name = "PresetVal";
            this.PresetVal.Size = new System.Drawing.Size(120, 20);
            this.PresetVal.TabIndex = 5;
            this.PresetVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SelectedSFLabel
            // 
            this.SelectedSFLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SelectedSFLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SelectedSFLabel.Location = new System.Drawing.Point(0, 137);
            this.SelectedSFLabel.Name = "SelectedSFLabel";
            this.SelectedSFLabel.Size = new System.Drawing.Size(360, 40);
            this.SelectedSFLabel.TabIndex = 6;
            this.SelectedSFLabel.Text = "Selected soundfont:\r\nPotato.sf2";
            this.SelectedSFLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(196, 106);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // WikipediaLink
            // 
            this.WikipediaLink.AutoSize = true;
            this.WikipediaLink.Location = new System.Drawing.Point(12, 111);
            this.WikipediaLink.Name = "WikipediaLink";
            this.WikipediaLink.Size = new System.Drawing.Size(92, 13);
            this.WikipediaLink.TabIndex = 8;
            this.WikipediaLink.TabStop = true;
            this.WikipediaLink.Text = "Banks/Presets list";
            this.WikipediaLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WikipediaLink_LinkClicked);
            // 
            // BankNPresetSel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(360, 177);
            this.Controls.Add(this.WikipediaLink);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SelectedSFLabel);
            this.Controls.Add(this.PresetVal);
            this.Controls.Add(this.BankVal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ConfirmBut);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BankNPresetSel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a bank and a preset for the SFZ";
            this.Load += new System.EventHandler(this.PresetSel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BankVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PresetVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ConfirmBut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown BankVal;
        private System.Windows.Forms.NumericUpDown PresetVal;
        private System.Windows.Forms.Label SelectedSFLabel;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.LinkLabel WikipediaLink;
    }
}