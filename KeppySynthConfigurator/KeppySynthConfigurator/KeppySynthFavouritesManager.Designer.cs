namespace KeppySynthConfigurator
{
    partial class KeppySynthFavouritesManager
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
            this.ClearFolderList = new System.Windows.Forms.Button();
            this.ManualListLabel = new System.Windows.Forms.Label();
            this.ManualFolder = new System.Windows.Forms.TextBox();
            this.FolderDef = new System.Windows.Forms.Label();
            this.FolderAdvancedMode = new System.Windows.Forms.CheckBox();
            this.RemoveFolder = new System.Windows.Forms.Button();
            this.AddFolder = new System.Windows.Forms.Button();
            this.FolderList = new System.Windows.Forms.ListBox();
            this.AddFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // ClearFolderList
            // 
            this.ClearFolderList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ClearFolderList.Location = new System.Drawing.Point(561, 62);
            this.ClearFolderList.Name = "ClearFolderList";
            this.ClearFolderList.Size = new System.Drawing.Size(122, 23);
            this.ClearFolderList.TabIndex = 40;
            this.ClearFolderList.Text = "Clear the  list";
            this.ClearFolderList.UseVisualStyleBackColor = true;
            // 
            // ManualListLabel
            // 
            this.ManualListLabel.Enabled = false;
            this.ManualListLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ManualListLabel.Location = new System.Drawing.Point(12, 65);
            this.ManualListLabel.Name = "ManualListLabel";
            this.ManualListLabel.Size = new System.Drawing.Size(50, 13);
            this.ManualListLabel.TabIndex = 38;
            this.ManualListLabel.Text = "Full path:";
            this.ManualListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ManualFolder
            // 
            this.ManualFolder.Enabled = false;
            this.ManualFolder.Location = new System.Drawing.Point(64, 62);
            this.ManualFolder.Name = "ManualFolder";
            this.ManualFolder.Size = new System.Drawing.Size(269, 20);
            this.ManualFolder.TabIndex = 37;
            // 
            // FolderDef
            // 
            this.FolderDef.AutoSize = true;
            this.FolderDef.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.FolderDef.Location = new System.Drawing.Point(12, 16);
            this.FolderDef.Name = "FolderDef";
            this.FolderDef.Size = new System.Drawing.Size(260, 13);
            this.FolderDef.TabIndex = 36;
            this.FolderDef.Text = "Add a folder to the favourites by clicking \"Add folder\".";
            // 
            // FolderAdvancedMode
            // 
            this.FolderAdvancedMode.AutoSize = true;
            this.FolderAdvancedMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.FolderAdvancedMode.Location = new System.Drawing.Point(15, 45);
            this.FolderAdvancedMode.Name = "FolderAdvancedMode";
            this.FolderAdvancedMode.Size = new System.Drawing.Size(212, 17);
            this.FolderAdvancedMode.TabIndex = 35;
            this.FolderAdvancedMode.Text = "I want to add the folder\'s path by myself";
            this.FolderAdvancedMode.UseVisualStyleBackColor = true;
            this.FolderAdvancedMode.CheckedChanged += new System.EventHandler(this.FolderAdvancedMode_CheckedChanged);
            // 
            // RemoveFolder
            // 
            this.RemoveFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RemoveFolder.Location = new System.Drawing.Point(561, 37);
            this.RemoveFolder.Name = "RemoveFolder";
            this.RemoveFolder.Size = new System.Drawing.Size(122, 23);
            this.RemoveFolder.TabIndex = 34;
            this.RemoveFolder.Text = "Remove folder";
            this.RemoveFolder.UseVisualStyleBackColor = true;
            this.RemoveFolder.Click += new System.EventHandler(this.RemoveFolder_Click);
            // 
            // AddFolder
            // 
            this.AddFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.AddFolder.Location = new System.Drawing.Point(561, 12);
            this.AddFolder.Name = "AddFolder";
            this.AddFolder.Size = new System.Drawing.Size(122, 23);
            this.AddFolder.TabIndex = 33;
            this.AddFolder.Text = "Add folder";
            this.AddFolder.UseVisualStyleBackColor = true;
            this.AddFolder.Click += new System.EventHandler(this.AddFolder_Click);
            // 
            // FolderList
            // 
            this.FolderList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FolderList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FolderList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FolderList.FormattingEnabled = true;
            this.FolderList.HorizontalScrollbar = true;
            this.FolderList.ItemHeight = 15;
            this.FolderList.Location = new System.Drawing.Point(0, 99);
            this.FolderList.Margin = new System.Windows.Forms.Padding(0);
            this.FolderList.Name = "FolderList";
            this.FolderList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FolderList.Size = new System.Drawing.Size(695, 287);
            this.FolderList.TabIndex = 32;
            // 
            // KeppyDriverFavouritesManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 386);
            this.Controls.Add(this.ClearFolderList);
            this.Controls.Add(this.ManualListLabel);
            this.Controls.Add(this.ManualFolder);
            this.Controls.Add(this.FolderDef);
            this.Controls.Add(this.FolderAdvancedMode);
            this.Controls.Add(this.RemoveFolder);
            this.Controls.Add(this.AddFolder);
            this.Controls.Add(this.FolderList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeppyDriverFavouritesManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Favourites manager";
            this.Load += new System.EventHandler(this.KeppyDriverFavouritesManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button ClearFolderList;
        internal System.Windows.Forms.Label ManualListLabel;
        internal System.Windows.Forms.TextBox ManualFolder;
        internal System.Windows.Forms.Label FolderDef;
        internal System.Windows.Forms.CheckBox FolderAdvancedMode;
        internal System.Windows.Forms.Button RemoveFolder;
        internal System.Windows.Forms.Button AddFolder;
        internal System.Windows.Forms.ListBox FolderList;
        private System.Windows.Forms.FolderBrowserDialog AddFolderDialog;
    }
}