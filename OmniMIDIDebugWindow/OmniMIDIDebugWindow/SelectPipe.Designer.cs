namespace OmniMIDIDebugWindow
{
    partial class SelectPipe
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
            this.OMPipes = new System.Windows.Forms.ListBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.Reload = new System.Windows.Forms.PictureBox();
            this.PipesFoundTxt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Reload)).BeginInit();
            this.SuspendLayout();
            // 
            // OMPipes
            // 
            this.OMPipes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OMPipes.FormattingEnabled = true;
            this.OMPipes.Location = new System.Drawing.Point(12, 25);
            this.OMPipes.Name = "OMPipes";
            this.OMPipes.Size = new System.Drawing.Size(271, 147);
            this.OMPipes.TabIndex = 0;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(208, 181);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.Location = new System.Drawing.Point(127, 181);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 2;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // Reload
            // 
            this.Reload.BackgroundImage = global::OmniMIDIDebugWindow.Properties.Resources.ReloadIcon;
            this.Reload.Location = new System.Drawing.Point(12, 181);
            this.Reload.Name = "Reload";
            this.Reload.Size = new System.Drawing.Size(23, 23);
            this.Reload.TabIndex = 3;
            this.Reload.TabStop = false;
            this.Reload.Click += new System.EventHandler(this.Reload_Click);
            // 
            // PipesFoundTxt
            // 
            this.PipesFoundTxt.AutoSize = true;
            this.PipesFoundTxt.Location = new System.Drawing.Point(12, 7);
            this.PipesFoundTxt.Name = "PipesFoundTxt";
            this.PipesFoundTxt.Size = new System.Drawing.Size(61, 13);
            this.PipesFoundTxt.TabIndex = 4;
            this.PipesFoundTxt.Text = "Checking...";
            // 
            // SelectPipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 216);
            this.ControlBox = false;
            this.Controls.Add(this.PipesFoundTxt);
            this.Controls.Add(this.Reload);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.OMPipes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectPipe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select debug pipe";
            this.Load += new System.EventHandler(this.SelectPipe_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Reload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox OMPipes;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.PictureBox Reload;
        private System.Windows.Forms.Label PipesFoundTxt;
    }
}