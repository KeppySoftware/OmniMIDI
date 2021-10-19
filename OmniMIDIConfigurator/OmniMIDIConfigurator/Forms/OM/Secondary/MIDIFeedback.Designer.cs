
namespace OmniMIDIConfigurator
{
    partial class MIDIFeedback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MIDIFeedback));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MIDIOutDevs = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.EnableFeedback = new System.Windows.Forms.CheckBox();
            this.FeedbackWhitelist = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AddBtn = new System.Windows.Forms.Button();
            this.RmvBtn = new System.Windows.Forms.Button();
            this.WhitelistPicker = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(435, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Feedback device:";
            // 
            // MIDIOutDevs
            // 
            this.MIDIOutDevs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIOutDevs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MIDIOutDevs.FormattingEnabled = true;
            this.MIDIOutDevs.Location = new System.Drawing.Point(14, 97);
            this.MIDIOutDevs.Name = "MIDIOutDevs";
            this.MIDIOutDevs.Size = new System.Drawing.Size(434, 23);
            this.MIDIOutDevs.TabIndex = 2;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(366, 420);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(87, 27);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // EnableFeedback
            // 
            this.EnableFeedback.AutoSize = true;
            this.EnableFeedback.Location = new System.Drawing.Point(14, 128);
            this.EnableFeedback.Name = "EnableFeedback";
            this.EnableFeedback.Size = new System.Drawing.Size(146, 19);
            this.EnableFeedback.TabIndex = 4;
            this.EnableFeedback.Text = "Enable feedback mode";
            this.EnableFeedback.UseVisualStyleBackColor = true;
            // 
            // FeedbackWhitelist
            // 
            this.FeedbackWhitelist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FeedbackWhitelist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FeedbackWhitelist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FeedbackWhitelist.FormattingEnabled = true;
            this.FeedbackWhitelist.HorizontalScrollbar = true;
            this.FeedbackWhitelist.IntegralHeight = false;
            this.FeedbackWhitelist.Location = new System.Drawing.Point(10, 180);
            this.FeedbackWhitelist.Margin = new System.Windows.Forms.Padding(0);
            this.FeedbackWhitelist.Name = "FeedbackWhitelist";
            this.FeedbackWhitelist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FeedbackWhitelist.Size = new System.Drawing.Size(442, 232);
            this.FeedbackWhitelist.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "Feedback mode whitelist:";
            // 
            // AddBtn
            // 
            this.AddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddBtn.Location = new System.Drawing.Point(9, 420);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(73, 27);
            this.AddBtn.TabIndex = 24;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // RmvBtn
            // 
            this.RmvBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RmvBtn.Location = new System.Drawing.Point(90, 420);
            this.RmvBtn.Name = "RmvBtn";
            this.RmvBtn.Size = new System.Drawing.Size(73, 27);
            this.RmvBtn.TabIndex = 25;
            this.RmvBtn.Text = "Remove";
            this.RmvBtn.UseVisualStyleBackColor = true;
            this.RmvBtn.Click += new System.EventHandler(this.RmvBtn_Click);
            // 
            // WhitelistPicker
            // 
            this.WhitelistPicker.Filter = "PE/Executable files|*.exe;";
            // 
            // MIDIFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 456);
            this.ControlBox = false;
            this.Controls.Add(this.RmvBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FeedbackWhitelist);
            this.Controls.Add(this.EnableFeedback);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MIDIOutDevs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MIDIFeedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MIDI feedback mode";
            this.Load += new System.EventHandler(this.MIDIFeedback_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MIDIOutDevs;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.CheckBox EnableFeedback;
        internal System.Windows.Forms.ListBox FeedbackWhitelist;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button RmvBtn;
        private System.Windows.Forms.OpenFileDialog WhitelistPicker;
    }
}