namespace KeppySynthConfigurator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.KSDAPI32 = new System.Windows.Forms.Button();
            this.KSDAPI64 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.WMMW32 = new System.Windows.Forms.Button();
            this.WMMW64 = new System.Windows.Forms.Button();
            this.DifferencesPatch = new System.Windows.Forms.LinkLabelEx();
            this.OKClose = new System.Windows.Forms.Button();
            this.UnpatchApp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.KSDAPI32);
            this.groupBox1.Controls.Add(this.KSDAPI64);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ReactOS WinMM with KSDirect API patch";
            // 
            // KSDAPI32
            // 
            this.KSDAPI32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KSDAPI32.Location = new System.Drawing.Point(7, 19);
            this.KSDAPI32.Name = "KSDAPI32";
            this.KSDAPI32.Size = new System.Drawing.Size(209, 23);
            this.KSDAPI32.TabIndex = 0;
            this.KSDAPI32.Text = "Patch a 32-bit app";
            this.KSDAPI32.UseVisualStyleBackColor = true;
            this.KSDAPI32.Click += new System.EventHandler(this.KSDAPI32_Click);
            // 
            // KSDAPI64
            // 
            this.KSDAPI64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KSDAPI64.Location = new System.Drawing.Point(7, 48);
            this.KSDAPI64.Name = "KSDAPI64";
            this.KSDAPI64.Size = new System.Drawing.Size(209, 23);
            this.KSDAPI64.TabIndex = 1;
            this.KSDAPI64.Text = "Patch a 64-bit app";
            this.KSDAPI64.UseVisualStyleBackColor = true;
            this.KSDAPI64.Click += new System.EventHandler(this.KSDAPI64_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.WMMW32);
            this.groupBox2.Controls.Add(this.WMMW64);
            this.groupBox2.Location = new System.Drawing.Point(12, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 78);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Windows Multimedia Wrapper patch";
            // 
            // WMMW32
            // 
            this.WMMW32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMW32.Location = new System.Drawing.Point(7, 19);
            this.WMMW32.Name = "WMMW32";
            this.WMMW32.Size = new System.Drawing.Size(209, 23);
            this.WMMW32.TabIndex = 2;
            this.WMMW32.Text = "Patch a 32-bit app";
            this.WMMW32.UseVisualStyleBackColor = true;
            this.WMMW32.Click += new System.EventHandler(this.WMMW32_Click);
            // 
            // WMMW64
            // 
            this.WMMW64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMW64.Location = new System.Drawing.Point(7, 48);
            this.WMMW64.Name = "WMMW64";
            this.WMMW64.Size = new System.Drawing.Size(209, 23);
            this.WMMW64.TabIndex = 3;
            this.WMMW64.Text = "Patch a 64-bit app";
            this.WMMW64.UseVisualStyleBackColor = true;
            this.WMMW64.Click += new System.EventHandler(this.WMMW64_Click);
            // 
            // DifferencesPatch
            // 
            this.DifferencesPatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DifferencesPatch.Location = new System.Drawing.Point(12, 179);
            this.DifferencesPatch.Name = "DifferencesPatch";
            this.DifferencesPatch.Size = new System.Drawing.Size(223, 13);
            this.DifferencesPatch.TabIndex = 3;
            this.DifferencesPatch.TabStop = true;
            this.DifferencesPatch.Text = "What\'s the difference between them?";
            this.DifferencesPatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DifferencesPatch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DifferencesPatch_LinkClicked);
            // 
            // OKClose
            // 
            this.OKClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKClose.Location = new System.Drawing.Point(198, 201);
            this.OKClose.Name = "OKClose";
            this.OKClose.Size = new System.Drawing.Size(37, 23);
            this.OKClose.TabIndex = 5;
            this.OKClose.Text = "OK";
            this.OKClose.UseVisualStyleBackColor = true;
            this.OKClose.Click += new System.EventHandler(this.OKClose_Click);
            // 
            // UnpatchApp
            // 
            this.UnpatchApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UnpatchApp.Location = new System.Drawing.Point(12, 201);
            this.UnpatchApp.Name = "UnpatchApp";
            this.UnpatchApp.Size = new System.Drawing.Size(180, 23);
            this.UnpatchApp.TabIndex = 4;
            this.UnpatchApp.Text = "Unpatch an already patched app";
            this.UnpatchApp.UseVisualStyleBackColor = true;
            this.UnpatchApp.Click += new System.EventHandler(this.UnpatchApp_Click);
            // 
            // WinMMPatches
            // 
            this.AcceptButton = this.OKClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(247, 234);
            this.Controls.Add(this.UnpatchApp);
            this.Controls.Add(this.OKClose);
            this.Controls.Add(this.DifferencesPatch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinMMPatches";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windows Multimedia patches";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button KSDAPI32;
        private System.Windows.Forms.Button KSDAPI64;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button WMMW32;
        private System.Windows.Forms.Button WMMW64;
        private System.Windows.Forms.LinkLabelEx DifferencesPatch;
        private System.Windows.Forms.Button OKClose;
        private System.Windows.Forms.Button UnpatchApp;
    }
}