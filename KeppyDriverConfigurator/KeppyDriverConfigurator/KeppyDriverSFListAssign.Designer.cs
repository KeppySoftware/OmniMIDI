namespace KeppyDriverConfigurator
{
    partial class KeppyDriverSFListAssign
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeppyDriverSFListAssign));
            this.DefMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAnAppToTheListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSelectedAppsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddApp = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.Lis = new System.Windows.Forms.ListBox();
            this.SelectedListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DefMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // DefMenu
            // 
            this.DefMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAnAppToTheListToolStripMenuItem,
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem,
            this.removeSelectedAppsToolStripMenuItem,
            this.toolStripSeparator1,
            this.clearListToolStripMenuItem});
            this.DefMenu.Name = "DefMenu";
            this.DefMenu.Size = new System.Drawing.Size(289, 98);
            // 
            // addAnAppToTheListToolStripMenuItem
            // 
            this.addAnAppToTheListToolStripMenuItem.Name = "addAnAppToTheListToolStripMenuItem";
            this.addAnAppToTheListToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.addAnAppToTheListToolStripMenuItem.Text = "Add an app to the list (Absolute path)...";
            this.addAnAppToTheListToolStripMenuItem.Click += new System.EventHandler(this.addAnAppToTheListToolStripMenuItem_Click);
            // 
            // addAnAppToTheListAppNameOnlyToolStripMenuItem
            // 
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Name = "addAnAppToTheListAppNameOnlyToolStripMenuItem";
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Text = "Add an app to the list (App name only)...";
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Click += new System.EventHandler(this.addAnAppToTheListAppNameOnlyToolStripMenuItem_Click);
            // 
            // removeSelectedAppsToolStripMenuItem
            // 
            this.removeSelectedAppsToolStripMenuItem.Name = "removeSelectedAppsToolStripMenuItem";
            this.removeSelectedAppsToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.removeSelectedAppsToolStripMenuItem.Text = "Remove selected app(s)";
            this.removeSelectedAppsToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedAppsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(285, 6);
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            this.clearListToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.clearListToolStripMenuItem.Text = "Clear list";
            this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
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
            this.Lis.ContextMenuStrip = this.DefMenu;
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
            // KeppyDriverSFListAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 378);
            this.Controls.Add(this.SelectedListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Lis);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeppyDriverSFListAssign";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assign a soundfont list to a specific app";
            this.Load += new System.EventHandler(this.KeppyDriverSFListAssign_Load);
            this.DefMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip DefMenu;
        private System.Windows.Forms.ToolStripMenuItem addAnAppToTheListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedAppsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog AddApp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem addAnAppToTheListAppNameOnlyToolStripMenuItem;
        private System.Windows.Forms.ListBox Lis;
        private System.Windows.Forms.ComboBox SelectedListBox;
        private System.Windows.Forms.Label label1;
    }
}