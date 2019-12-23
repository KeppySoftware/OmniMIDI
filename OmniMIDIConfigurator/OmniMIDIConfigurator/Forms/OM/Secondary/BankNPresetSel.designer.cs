namespace OmniMIDIConfigurator
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
            this.DesBankVal = new System.Windows.Forms.NumericUpDown();
            this.DesPresetVal = new System.Windows.Forms.NumericUpDown();
            this.SelectedSFLabel = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.WikipediaLink = new OmniMIDIConfigurator.LinkLabelEx();
            this.SrcBankVal = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SrcPresetVal = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.XGMode = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DesBankVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DesPresetVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcBankVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPresetVal)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.ConfirmBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfirmBut.Location = new System.Drawing.Point(277, 203);
            this.ConfirmBut.Name = "ConfirmBut";
            this.ConfirmBut.Size = new System.Drawing.Size(75, 23);
            this.ConfirmBut.TabIndex = 1;
            this.ConfirmBut.Text = "Confirm";
            this.ConfirmBut.UseVisualStyleBackColor = true;
            this.ConfirmBut.Click += new System.EventHandler(this.ConfirmBut_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Des. Bank:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(183, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Des. Preset:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DesBankVal
            // 
            this.DesBankVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DesBankVal.Location = new System.Drawing.Point(79, 95);
            this.DesBankVal.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.DesBankVal.Name = "DesBankVal";
            this.DesBankVal.Size = new System.Drawing.Size(88, 20);
            this.DesBankVal.TabIndex = 4;
            this.DesBankVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DesPresetVal
            // 
            this.DesPresetVal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.DesPresetVal.Location = new System.Drawing.Point(259, 95);
            this.DesPresetVal.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.DesPresetVal.Name = "DesPresetVal";
            this.DesPresetVal.Size = new System.Drawing.Size(88, 20);
            this.DesPresetVal.TabIndex = 5;
            this.DesPresetVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SelectedSFLabel
            // 
            this.SelectedSFLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SelectedSFLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SelectedSFLabel.Location = new System.Drawing.Point(0, 234);
            this.SelectedSFLabel.Name = "SelectedSFLabel";
            this.SelectedSFLabel.Size = new System.Drawing.Size(360, 40);
            this.SelectedSFLabel.TabIndex = 6;
            this.SelectedSFLabel.Text = "Selected soundfont:\r\nPotato.sf2";
            this.SelectedSFLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.Location = new System.Drawing.Point(196, 203);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // WikipediaLink
            // 
            this.WikipediaLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WikipediaLink.AutoSize = true;
            this.WikipediaLink.Location = new System.Drawing.Point(12, 208);
            this.WikipediaLink.Name = "WikipediaLink";
            this.WikipediaLink.Size = new System.Drawing.Size(148, 13);
            this.WikipediaLink.TabIndex = 8;
            this.WikipediaLink.TabStop = true;
            this.WikipediaLink.Text = "GM Level 1 banks/presets list";
            this.WikipediaLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WikipediaLink_LinkClicked);
            // 
            // SrcBankVal
            // 
            this.SrcBankVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SrcBankVal.Location = new System.Drawing.Point(79, 70);
            this.SrcBankVal.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.SrcBankVal.Name = "SrcBankVal";
            this.SrcBankVal.Size = new System.Drawing.Size(88, 20);
            this.SrcBankVal.TabIndex = 11;
            this.SrcBankVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(186, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Src. Preset:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Src. Bank:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SrcPresetVal
            // 
            this.SrcPresetVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SrcPresetVal.Location = new System.Drawing.Point(259, 69);
            this.SrcPresetVal.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.SrcPresetVal.Name = "SrcPresetVal";
            this.SrcPresetVal.Size = new System.Drawing.Size(88, 20);
            this.SrcPresetVal.TabIndex = 13;
            this.SrcPresetVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(246, 52);
            this.label6.TabIndex = 14;
            this.label6.Text = "Src.: Source of the bank/preset to import\r\nDes.: Bank/Preset to assign to the imp" +
    "orted one.\r\nLeave Src. Bank and Src. Preset to 0 for SFZ files.\r\n-1 means to loa" +
    "d all the presets/banks.";
            // 
            // XGMode
            // 
            this.XGMode.AutoSize = true;
            this.XGMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.XGMode.Location = new System.Drawing.Point(136, 122);
            this.XGMode.Name = "XGMode";
            this.XGMode.Size = new System.Drawing.Size(208, 17);
            this.XGMode.TabIndex = 24;
            this.XGMode.Text = "Use bank 127 for drumkits in XG mode\r\n";
            this.XGMode.UseVisualStyleBackColor = true;
            // 
            // BankNPresetSel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(360, 274);
            this.ControlBox = false;
            this.Controls.Add(this.XGMode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SrcPresetVal);
            this.Controls.Add(this.SrcBankVal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.WikipediaLink);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SelectedSFLabel);
            this.Controls.Add(this.DesPresetVal);
            this.Controls.Add(this.DesBankVal);
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
            this.Text = "Select a bank and a preset for the soundfont";
            ((System.ComponentModel.ISupportInitialize)(this.DesBankVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DesPresetVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcBankVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPresetVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ConfirmBut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown DesBankVal;
        private System.Windows.Forms.NumericUpDown DesPresetVal;
        private System.Windows.Forms.Label SelectedSFLabel;
        private System.Windows.Forms.Button CancelBtn;
        private OmniMIDIConfigurator.LinkLabelEx WikipediaLink;
        private System.Windows.Forms.NumericUpDown SrcBankVal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown SrcPresetVal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox XGMode;
    }
}