namespace OmniMIDIConfigurator
{
    partial class AddToWhichList
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
            this.InfoMessage = new System.Windows.Forms.Label();
            this.ListSel = new System.Windows.Forms.ComboBox();
            this.AddToList = new System.Windows.Forms.Button();
            this.DoNotAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InfoMessage
            // 
            this.InfoMessage.AutoSize = true;
            this.InfoMessage.Location = new System.Drawing.Point(12, 9);
            this.InfoMessage.Name = "InfoMessage";
            this.InfoMessage.Size = new System.Drawing.Size(316, 30);
            this.InfoMessage.TabIndex = 0;
            this.InfoMessage.Text = "Select the list you want to add the following SoundFont to.\r\nSoundFont: {0}";
            // 
            // ListSel
            // 
            this.ListSel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ListSel.FormattingEnabled = true;
            this.ListSel.Items.AddRange(new object[] {
            "Shared list",
            "List 2",
            "List 3",
            "List 4",
            "List 5",
            "List 6",
            "List 7",
            "List 8"});
            this.ListSel.Location = new System.Drawing.Point(15, 50);
            this.ListSel.Name = "ListSel";
            this.ListSel.Size = new System.Drawing.Size(354, 23);
            this.ListSel.TabIndex = 1;
            // 
            // AddToList
            // 
            this.AddToList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddToList.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AddToList.Location = new System.Drawing.Point(294, 84);
            this.AddToList.Name = "AddToList";
            this.AddToList.Size = new System.Drawing.Size(75, 23);
            this.AddToList.TabIndex = 2;
            this.AddToList.Text = "Add";
            this.AddToList.UseVisualStyleBackColor = true;
            this.AddToList.Click += new System.EventHandler(this.AddToList_Click);
            // 
            // DoNotAdd
            // 
            this.DoNotAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DoNotAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoNotAdd.Location = new System.Drawing.Point(213, 84);
            this.DoNotAdd.Name = "DoNotAdd";
            this.DoNotAdd.Size = new System.Drawing.Size(75, 23);
            this.DoNotAdd.TabIndex = 3;
            this.DoNotAdd.Text = "Cancel";
            this.DoNotAdd.UseVisualStyleBackColor = true;
            // 
            // AddToWhichList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.DoNotAdd;
            this.ClientSize = new System.Drawing.Size(381, 119);
            this.ControlBox = false;
            this.Controls.Add(this.DoNotAdd);
            this.Controls.Add(this.AddToList);
            this.Controls.Add(this.ListSel);
            this.Controls.Add(this.InfoMessage);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddToWhichList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select SoundFont list";
            this.Load += new System.EventHandler(this.AddToWhichList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoMessage;
        private System.Windows.Forms.ComboBox ListSel;
        private System.Windows.Forms.Button AddToList;
        private System.Windows.Forms.Button DoNotAdd;
    }
}