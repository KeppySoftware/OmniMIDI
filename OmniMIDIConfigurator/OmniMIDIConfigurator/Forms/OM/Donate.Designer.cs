namespace OmniMIDIConfigurator
{
    partial class Donate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Donate));
            this.DTxt = new System.Windows.Forms.Label();
            this.ShowMeNext = new System.Windows.Forms.Button();
            this.DontShowAnymore = new System.Windows.Forms.Button();
            this.DonateBtn = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DonateBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // DTxt
            // 
            this.DTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DTxt.Location = new System.Drawing.Point(14, 10);
            this.DTxt.Name = "DTxt";
            this.DTxt.Size = new System.Drawing.Size(362, 192);
            this.DTxt.TabIndex = 0;
            this.DTxt.Text = resources.GetString("DTxt.Text");
            // 
            // ShowMeNext
            // 
            this.ShowMeNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowMeNext.Location = new System.Drawing.Point(17, 205);
            this.ShowMeNext.Name = "ShowMeNext";
            this.ShowMeNext.Size = new System.Drawing.Size(175, 27);
            this.ShowMeNext.TabIndex = 5;
            this.ShowMeNext.Text = "Remind me next month";
            this.ShowMeNext.UseVisualStyleBackColor = true;
            this.ShowMeNext.Click += new System.EventHandler(this.ShowMeNext_Click);
            // 
            // DontShowAnymore
            // 
            this.DontShowAnymore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DontShowAnymore.Location = new System.Drawing.Point(17, 237);
            this.DontShowAnymore.Name = "DontShowAnymore";
            this.DontShowAnymore.Size = new System.Drawing.Size(175, 27);
            this.DontShowAnymore.TabIndex = 4;
            this.DontShowAnymore.Text = "Never show this again";
            this.DontShowAnymore.UseVisualStyleBackColor = true;
            this.DontShowAnymore.Click += new System.EventHandler(this.DontShowAnymore_Click);
            // 
            // DonateBtn
            // 
            this.DonateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DonateBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DonateBtn.Location = new System.Drawing.Point(198, 205);
            this.DonateBtn.Name = "DonateBtn";
            this.DonateBtn.Size = new System.Drawing.Size(177, 58);
            this.DonateBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DonateBtn.TabIndex = 3;
            this.DonateBtn.TabStop = false;
            this.DonateBtn.Click += new System.EventHandler(this.DonateBtn_Click);
            // 
            // Donate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 272);
            this.ControlBox = false;
            this.Controls.Add(this.ShowMeNext);
            this.Controls.Add(this.DontShowAnymore);
            this.Controls.Add(this.DonateBtn);
            this.Controls.Add(this.DTxt);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Donate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Donate";
            ((System.ComponentModel.ISupportInitialize)(this.DonateBtn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label DTxt;
        private System.Windows.Forms.Button ShowMeNext;
        private System.Windows.Forms.Button DontShowAnymore;
        private System.Windows.Forms.PictureBox DonateBtn;
    }
}