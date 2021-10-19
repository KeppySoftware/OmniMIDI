namespace OmniMIDIConfigurator
{
    partial class WinMMPatches
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinMMPatches));
            this.OKClose = new System.Windows.Forms.Button();
            this.UnpatchApp = new System.Windows.Forms.Button();
            this.PatchStatusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.WMMW = new System.Windows.Forms.Button();
            this.BMPatch = new System.Windows.Forms.GroupBox();
            this.AArch64BM = new System.Windows.Forms.Button();
            this.AMD64BM = new System.Windows.Forms.Button();
            this.i386BM = new System.Windows.Forms.Button();
            this.WMMD = new System.Windows.Forms.Button();
            this.DAWPatch = new System.Windows.Forms.GroupBox();
            this.AArch64DAW = new System.Windows.Forms.Button();
            this.AMD64DAW = new System.Windows.Forms.Button();
            this.i386DAW = new System.Windows.Forms.Button();
            this.WinMMWRPDesc = new System.Windows.Forms.Label();
            this.BMPatch.SuspendLayout();
            this.DAWPatch.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKClose
            // 
            this.OKClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKClose.Location = new System.Drawing.Point(253, 231);
            this.OKClose.Name = "OKClose";
            this.OKClose.Size = new System.Drawing.Size(67, 23);
            this.OKClose.TabIndex = 5;
            this.OKClose.Text = "Close";
            this.OKClose.UseVisualStyleBackColor = true;
            this.OKClose.Click += new System.EventHandler(this.OKClose_Click);
            // 
            // UnpatchApp
            // 
            this.UnpatchApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UnpatchApp.Location = new System.Drawing.Point(12, 231);
            this.UnpatchApp.Name = "UnpatchApp";
            this.UnpatchApp.Size = new System.Drawing.Size(187, 23);
            this.UnpatchApp.TabIndex = 4;
            this.UnpatchApp.Text = "Unpatch an already patched app";
            this.UnpatchApp.UseVisualStyleBackColor = true;
            this.UnpatchApp.Click += new System.EventHandler(this.UnpatchApp_Click);
            // 
            // PatchStatusLabel
            // 
            this.PatchStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PatchStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatchStatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.PatchStatusLabel.Location = new System.Drawing.Point(52, 208);
            this.PatchStatusLabel.Name = "PatchStatusLabel";
            this.PatchStatusLabel.Size = new System.Drawing.Size(268, 16);
            this.PatchStatusLabel.TabIndex = 7;
            this.PatchStatusLabel.Text = "Waiting...";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Status:";
            // 
            // WMMW
            // 
            this.WMMW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMW.Location = new System.Drawing.Point(4, 15);
            this.WMMW.Name = "WMMW";
            this.WMMW.Size = new System.Drawing.Size(300, 24);
            this.WMMW.TabIndex = 2;
            this.WMMW.Text = "Patch an application";
            this.WMMW.UseVisualStyleBackColor = true;
            this.WMMW.Click += new System.EventHandler(this.WMMW_Click);
            this.WMMW.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.BMPatch_HelpRequested);
            // 
            // BMPatch
            // 
            this.BMPatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BMPatch.Controls.Add(this.AArch64BM);
            this.BMPatch.Controls.Add(this.AMD64BM);
            this.BMPatch.Controls.Add(this.i386BM);
            this.BMPatch.Controls.Add(this.WMMW);
            this.BMPatch.Location = new System.Drawing.Point(12, 107);
            this.BMPatch.Name = "BMPatch";
            this.BMPatch.Size = new System.Drawing.Size(308, 45);
            this.BMPatch.TabIndex = 8;
            this.BMPatch.TabStop = false;
            this.BMPatch.Text = "Standard performance improvement patch (BM)";
            this.BMPatch.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.BMPatch_HelpRequested);
            // 
            // AArch64BM
            // 
            this.AArch64BM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AArch64BM.Location = new System.Drawing.Point(204, 15);
            this.AArch64BM.Name = "AArch64BM";
            this.AArch64BM.Size = new System.Drawing.Size(99, 24);
            this.AArch64BM.TabIndex = 5;
            this.AArch64BM.Text = "ARM64 (AA64)";
            this.AArch64BM.UseVisualStyleBackColor = true;
            this.AArch64BM.Visible = false;
            this.AArch64BM.Click += new System.EventHandler(this.AArch64BM_Click);
            // 
            // AMD64BM
            // 
            this.AMD64BM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AMD64BM.Location = new System.Drawing.Point(104, 15);
            this.AMD64BM.Name = "AMD64BM";
            this.AMD64BM.Size = new System.Drawing.Size(100, 24);
            this.AMD64BM.TabIndex = 4;
            this.AMD64BM.Text = "x64 (AMD64)";
            this.AMD64BM.UseVisualStyleBackColor = true;
            this.AMD64BM.Visible = false;
            this.AMD64BM.Click += new System.EventHandler(this.AMD64BM_Click);
            // 
            // i386BM
            // 
            this.i386BM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.i386BM.Location = new System.Drawing.Point(5, 15);
            this.i386BM.Name = "i386BM";
            this.i386BM.Size = new System.Drawing.Size(99, 24);
            this.i386BM.TabIndex = 3;
            this.i386BM.Text = "x86 (i386)";
            this.i386BM.UseVisualStyleBackColor = true;
            this.i386BM.Visible = false;
            this.i386BM.Click += new System.EventHandler(this.i386BM_Click);
            // 
            // WMMD
            // 
            this.WMMD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMD.Location = new System.Drawing.Point(4, 15);
            this.WMMD.Name = "WMMD";
            this.WMMD.Size = new System.Drawing.Size(300, 24);
            this.WMMD.TabIndex = 2;
            this.WMMD.Text = "Patch an application";
            this.WMMD.UseVisualStyleBackColor = true;
            this.WMMD.Click += new System.EventHandler(this.WMMD_Click);
            this.WMMD.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DAWPatch_HelpRequested);
            // 
            // DAWPatch
            // 
            this.DAWPatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DAWPatch.Controls.Add(this.AArch64DAW);
            this.DAWPatch.Controls.Add(this.AMD64DAW);
            this.DAWPatch.Controls.Add(this.i386DAW);
            this.DAWPatch.Controls.Add(this.WMMD);
            this.DAWPatch.Location = new System.Drawing.Point(12, 155);
            this.DAWPatch.Name = "DAWPatch";
            this.DAWPatch.Size = new System.Drawing.Size(308, 45);
            this.DAWPatch.TabIndex = 9;
            this.DAWPatch.TabStop = false;
            this.DAWPatch.Text = "Special DAW patch for multi-device usage (DAW)";
            this.DAWPatch.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DAWPatch_HelpRequested);
            // 
            // AArch64DAW
            // 
            this.AArch64DAW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AArch64DAW.Location = new System.Drawing.Point(204, 15);
            this.AArch64DAW.Name = "AArch64DAW";
            this.AArch64DAW.Size = new System.Drawing.Size(99, 24);
            this.AArch64DAW.TabIndex = 6;
            this.AArch64DAW.Text = "ARM64 (AA64)";
            this.AArch64DAW.UseVisualStyleBackColor = true;
            this.AArch64DAW.Visible = false;
            this.AArch64DAW.Click += new System.EventHandler(this.AArch64DAW_Click);
            // 
            // AMD64DAW
            // 
            this.AMD64DAW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AMD64DAW.Location = new System.Drawing.Point(104, 15);
            this.AMD64DAW.Name = "AMD64DAW";
            this.AMD64DAW.Size = new System.Drawing.Size(100, 24);
            this.AMD64DAW.TabIndex = 6;
            this.AMD64DAW.Text = "x64 (AMD64)";
            this.AMD64DAW.UseVisualStyleBackColor = true;
            this.AMD64DAW.Visible = false;
            this.AMD64DAW.Click += new System.EventHandler(this.AMD64DAW_Click);
            // 
            // i386DAW
            // 
            this.i386DAW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.i386DAW.Location = new System.Drawing.Point(5, 15);
            this.i386DAW.Name = "i386DAW";
            this.i386DAW.Size = new System.Drawing.Size(99, 24);
            this.i386DAW.TabIndex = 6;
            this.i386DAW.Text = "x86 (i386)";
            this.i386DAW.UseVisualStyleBackColor = true;
            this.i386DAW.Visible = false;
            this.i386DAW.Click += new System.EventHandler(this.i386DAW_Click);
            // 
            // WinMMWRPDesc
            // 
            this.WinMMWRPDesc.Location = new System.Drawing.Point(12, 9);
            this.WinMMWRPDesc.Name = "WinMMWRPDesc";
            this.WinMMWRPDesc.Size = new System.Drawing.Size(308, 95);
            this.WinMMWRPDesc.TabIndex = 10;
            this.WinMMWRPDesc.Text = resources.GetString("WinMMWRPDesc.Text");
            this.WinMMWRPDesc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // WinMMPatches
            // 
            this.AcceptButton = this.OKClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(332, 266);
            this.Controls.Add(this.WinMMWRPDesc);
            this.Controls.Add(this.DAWPatch);
            this.Controls.Add(this.BMPatch);
            this.Controls.Add(this.PatchStatusLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UnpatchApp);
            this.Controls.Add(this.OKClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinMMPatches";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windows Multimedia Wrapper";
            this.BMPatch.ResumeLayout(false);
            this.DAWPatch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OKClose;
        private System.Windows.Forms.Button UnpatchApp;
        private System.Windows.Forms.Label PatchStatusLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button WMMW;
        private System.Windows.Forms.GroupBox BMPatch;
        private System.Windows.Forms.Button WMMD;
        private System.Windows.Forms.GroupBox DAWPatch;
        private System.Windows.Forms.Label WinMMWRPDesc;
        private System.Windows.Forms.Button AArch64BM;
        private System.Windows.Forms.Button AMD64BM;
        private System.Windows.Forms.Button i386BM;
        private System.Windows.Forms.Button AArch64DAW;
        private System.Windows.Forms.Button AMD64DAW;
        private System.Windows.Forms.Button i386DAW;
    }
}