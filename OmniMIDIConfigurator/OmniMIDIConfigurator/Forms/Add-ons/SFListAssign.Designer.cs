namespace OmniMIDIConfigurator{
    partial class SFListAssign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFListAssign));
            this.AddApp = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.Lis = new System.Windows.Forms.ListBox();
            this.SelectedListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DefMenu = new System.Windows.Forms.ContextMenu();
            this.addAnAppToTheListToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.removeSelectedAppsToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.clearListToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // AddApp
            // 
            this.AddApp.FileName = "Select an application file...";
            this.AddApp.Filter = "Executables (.exe)|*.exe";
            this.AddApp.Multiselect = true;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(643, 58);
            this.label5.TabIndex = 15;
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lis
            // 
            this.Lis.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Lis.FormattingEnabled = true;
            this.Lis.Location = new System.Drawing.Point(0, 88);
            this.Lis.Name = "Lis";
            this.Lis.ScrollAlwaysVisible = true;
            this.Lis.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Lis.Size = new System.Drawing.Size(643, 290);
            this.Lis.TabIndex = 16;
            // 
            // SelectedListBox
            // 
            this.SelectedListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedListBox.FormattingEnabled = true;
            this.SelectedListBox.Items.AddRange(new object[] {
            "List 1",
            "List 2",
            "List 3",
            "List 4",
            "List 5",
            "List 6",
            "List 7",
            "List 8"});
            this.SelectedListBox.Location = new System.Drawing.Point(117, 61);
            this.SelectedListBox.Name = "SelectedListBox";
            this.SelectedListBox.Size = new System.Drawing.Size(521, 21);
            this.SelectedListBox.TabIndex = 37;
            this.SelectedListBox.SelectedIndexChanged += new System.EventHandler(this.SelectedListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Select applist to edit:";
            // 
            // DefMenu
            // 
            this.DefMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.addAnAppToTheListToolStripMenuItem,
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem,
            this.removeSelectedAppsToolStripMenuItem,
            this.menuItem4,
            this.clearListToolStripMenuItem});
            // 
            // addAnAppToTheListToolStripMenuItem
            // 
            this.addAnAppToTheListToolStripMenuItem.Index = 0;
            this.addAnAppToTheListToolStripMenuItem.Text = "Add an app to the list (Absolute path)...";
            this.addAnAppToTheListToolStripMenuItem.Click += new System.EventHandler(this.addAnAppToTheListToolStripMenuItem_Click);
            // 
            // addAnAppToTheListAppNameOnlyToolStripMenuItem
            // 
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Index = 1;
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Text = "Add an app to the list (App name only)...";
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Click += new System.EventHandler(this.addAnAppToTheListAppNameOnlyToolStripMenuItem_Click);
            // 
            // removeSelectedAppsToolStripMenuItem
            // 
            this.removeSelectedAppsToolStripMenuItem.Index = 2;
            this.removeSelectedAppsToolStripMenuItem.Text = "Remove selected app(s)";
            this.removeSelectedAppsToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedAppsToolStripMenuItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "-";
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Index = 4;
            this.clearListToolStripMenuItem.Text = "Clear list";
            this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
            // 
            // SFListAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(643, 378);
            this.Controls.Add(this.SelectedListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Lis);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OmniMIDISFListAssign";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assign a soundfont list to a specific app";
            this.Load += new System.EventHandler(this.KeppyDriverSFListAssign_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog AddApp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox Lis;
        private System.Windows.Forms.ComboBox SelectedListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenu DefMenu;
        private System.Windows.Forms.MenuItem addAnAppToTheListToolStripMenuItem;
        private System.Windows.Forms.MenuItem addAnAppToTheListAppNameOnlyToolStripMenuItem;
        private System.Windows.Forms.MenuItem removeSelectedAppsToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem clearListToolStripMenuItem;
    }
}