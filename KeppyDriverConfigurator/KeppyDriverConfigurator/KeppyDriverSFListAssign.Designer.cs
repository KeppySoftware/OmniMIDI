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
            this.label1 = new System.Windows.Forms.Label();
            this.DefMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAnAppToTheListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSelectedAppsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AddApp = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Lis1 = new System.Windows.Forms.TabPage();
            this.Lis2 = new System.Windows.Forms.TabPage();
            this.Lis3 = new System.Windows.Forms.TabPage();
            this.Lis4 = new System.Windows.Forms.TabPage();
            this.Lis5 = new System.Windows.Forms.TabPage();
            this.Lis6 = new System.Windows.Forms.TabPage();
            this.Lis7 = new System.Windows.Forms.TabPage();
            this.Lis8 = new System.Windows.Forms.TabPage();
            this.listBox8 = new System.Windows.Forms.ListBox();
            this.listBox7 = new System.Windows.Forms.ListBox();
            this.listBox6 = new System.Windows.Forms.ListBox();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DefMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Lis1.SuspendLayout();
            this.Lis2.SuspendLayout();
            this.Lis3.SuspendLayout();
            this.Lis4.SuspendLayout();
            this.Lis5.SuspendLayout();
            this.Lis6.SuspendLayout();
            this.Lis7.SuspendLayout();
            this.Lis8.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "List 1\'s apps:";
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
            this.DefMenu.Size = new System.Drawing.Size(289, 120);
            // 
            // addAnAppToTheListToolStripMenuItem
            // 
            this.addAnAppToTheListToolStripMenuItem.Name = "addAnAppToTheListToolStripMenuItem";
            this.addAnAppToTheListToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.addAnAppToTheListToolStripMenuItem.Text = "Add an app to the list (Absolute path)...";
            this.addAnAppToTheListToolStripMenuItem.Click += new System.EventHandler(this.addAnAppToTheListToolStripMenuItem_Click);
            // 
            // removeSelectedAppsToolStripMenuItem
            // 
            this.removeSelectedAppsToolStripMenuItem.Name = "removeSelectedAppsToolStripMenuItem";
            this.removeSelectedAppsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.removeSelectedAppsToolStripMenuItem.Text = "Remove selected app(s)";
            this.removeSelectedAppsToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedAppsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(196, 6);
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            this.clearListToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.clearListToolStripMenuItem.Text = "Clear list";
            this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "List 2\'s apps:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "List 3\'s apps:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(477, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "List 4\'s apps:";
            // 
            // AddApp
            // 
            this.AddApp.FileName = "Select an application file...";
            this.AddApp.Filter = "Executables (.exe)|*.exe";
            this.AddApp.Multiselect = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(486, 52);
            this.label5.TabIndex = 15;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Lis1);
            this.tabControl1.Controls.Add(this.Lis2);
            this.tabControl1.Controls.Add(this.Lis3);
            this.tabControl1.Controls.Add(this.Lis4);
            this.tabControl1.Controls.Add(this.Lis5);
            this.tabControl1.Controls.Add(this.Lis6);
            this.tabControl1.Controls.Add(this.Lis7);
            this.tabControl1.Controls.Add(this.Lis8);
            this.tabControl1.Location = new System.Drawing.Point(12, 72);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(619, 294);
            this.tabControl1.TabIndex = 16;
            // 
            // Lis1
            // 
            this.Lis1.Controls.Add(this.listBox1);
            this.Lis1.Location = new System.Drawing.Point(4, 22);
            this.Lis1.Name = "Lis1";
            this.Lis1.Size = new System.Drawing.Size(611, 268);
            this.Lis1.TabIndex = 0;
            this.Lis1.Text = "List 1\'s apps";
            this.Lis1.UseVisualStyleBackColor = true;
            // 
            // Lis2
            // 
            this.Lis2.Controls.Add(this.listBox2);
            this.Lis2.Location = new System.Drawing.Point(4, 22);
            this.Lis2.Name = "Lis2";
            this.Lis2.Size = new System.Drawing.Size(611, 268);
            this.Lis2.TabIndex = 1;
            this.Lis2.Text = "List 2\'s apps";
            this.Lis2.UseVisualStyleBackColor = true;
            // 
            // Lis3
            // 
            this.Lis3.Controls.Add(this.listBox3);
            this.Lis3.Location = new System.Drawing.Point(4, 22);
            this.Lis3.Name = "Lis3";
            this.Lis3.Size = new System.Drawing.Size(611, 268);
            this.Lis3.TabIndex = 2;
            this.Lis3.Text = "List 3\'s apps";
            this.Lis3.UseVisualStyleBackColor = true;
            // 
            // Lis4
            // 
            this.Lis4.Controls.Add(this.listBox4);
            this.Lis4.Location = new System.Drawing.Point(4, 22);
            this.Lis4.Name = "Lis4";
            this.Lis4.Size = new System.Drawing.Size(611, 268);
            this.Lis4.TabIndex = 3;
            this.Lis4.Text = "List 4\'s apps";
            this.Lis4.UseVisualStyleBackColor = true;
            // 
            // Lis5
            // 
            this.Lis5.Controls.Add(this.listBox5);
            this.Lis5.Location = new System.Drawing.Point(4, 22);
            this.Lis5.Name = "Lis5";
            this.Lis5.Size = new System.Drawing.Size(611, 268);
            this.Lis5.TabIndex = 4;
            this.Lis5.Text = "List 5\'s apps";
            this.Lis5.UseVisualStyleBackColor = true;
            // 
            // Lis6
            // 
            this.Lis6.Controls.Add(this.listBox6);
            this.Lis6.Location = new System.Drawing.Point(4, 22);
            this.Lis6.Name = "Lis6";
            this.Lis6.Size = new System.Drawing.Size(611, 268);
            this.Lis6.TabIndex = 5;
            this.Lis6.Text = "List 6\'s apps";
            this.Lis6.UseVisualStyleBackColor = true;
            // 
            // Lis7
            // 
            this.Lis7.Controls.Add(this.listBox7);
            this.Lis7.Location = new System.Drawing.Point(4, 22);
            this.Lis7.Name = "Lis7";
            this.Lis7.Size = new System.Drawing.Size(611, 268);
            this.Lis7.TabIndex = 6;
            this.Lis7.Text = "List 7\'s apps";
            this.Lis7.UseVisualStyleBackColor = true;
            // 
            // Lis8
            // 
            this.Lis8.Controls.Add(this.listBox8);
            this.Lis8.Location = new System.Drawing.Point(4, 22);
            this.Lis8.Name = "Lis8";
            this.Lis8.Size = new System.Drawing.Size(611, 268);
            this.Lis8.TabIndex = 7;
            this.Lis8.Text = "List 8\'s apps";
            this.Lis8.UseVisualStyleBackColor = true;
            // 
            // listBox8
            // 
            this.listBox8.ContextMenuStrip = this.DefMenu;
            this.listBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox8.FormattingEnabled = true;
            this.listBox8.Location = new System.Drawing.Point(0, 0);
            this.listBox8.Name = "listBox8";
            this.listBox8.ScrollAlwaysVisible = true;
            this.listBox8.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox8.Size = new System.Drawing.Size(611, 268);
            this.listBox8.TabIndex = 14;
            // 
            // listBox7
            // 
            this.listBox7.ContextMenuStrip = this.DefMenu;
            this.listBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox7.FormattingEnabled = true;
            this.listBox7.Location = new System.Drawing.Point(0, 0);
            this.listBox7.Name = "listBox7";
            this.listBox7.ScrollAlwaysVisible = true;
            this.listBox7.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox7.Size = new System.Drawing.Size(611, 268);
            this.listBox7.TabIndex = 13;
            // 
            // listBox6
            // 
            this.listBox6.ContextMenuStrip = this.DefMenu;
            this.listBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox6.FormattingEnabled = true;
            this.listBox6.Location = new System.Drawing.Point(0, 0);
            this.listBox6.Name = "listBox6";
            this.listBox6.ScrollAlwaysVisible = true;
            this.listBox6.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox6.Size = new System.Drawing.Size(611, 268);
            this.listBox6.TabIndex = 12;
            // 
            // listBox5
            // 
            this.listBox5.ContextMenuStrip = this.DefMenu;
            this.listBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox5.FormattingEnabled = true;
            this.listBox5.Location = new System.Drawing.Point(0, 0);
            this.listBox5.Name = "listBox5";
            this.listBox5.ScrollAlwaysVisible = true;
            this.listBox5.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox5.Size = new System.Drawing.Size(611, 268);
            this.listBox5.TabIndex = 11;
            // 
            // listBox4
            // 
            this.listBox4.ContextMenuStrip = this.DefMenu;
            this.listBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(0, 0);
            this.listBox4.Name = "listBox4";
            this.listBox4.ScrollAlwaysVisible = true;
            this.listBox4.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox4.Size = new System.Drawing.Size(611, 268);
            this.listBox4.TabIndex = 8;
            // 
            // listBox3
            // 
            this.listBox3.ContextMenuStrip = this.DefMenu;
            this.listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(0, 0);
            this.listBox3.Name = "listBox3";
            this.listBox3.ScrollAlwaysVisible = true;
            this.listBox3.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox3.Size = new System.Drawing.Size(611, 268);
            this.listBox3.TabIndex = 6;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.DefMenu;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(611, 268);
            this.listBox1.TabIndex = 2;
            // 
            // listBox2
            // 
            this.listBox2.ContextMenuStrip = this.DefMenu;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.ScrollAlwaysVisible = true;
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox2.Size = new System.Drawing.Size(611, 268);
            this.listBox2.TabIndex = 4;
            // 
            // addAnAppToTheListAppNameOnlyToolStripMenuItem
            // 
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Name = "addAnAppToTheListAppNameOnlyToolStripMenuItem";
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Text = "Add an app to the list (App name only)...";
            this.addAnAppToTheListAppNameOnlyToolStripMenuItem.Click += new System.EventHandler(this.addAnAppToTheListAppNameOnlyToolStripMenuItem_Click);
            // 
            // KeppyDriverSFListAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 378);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
            this.tabControl1.ResumeLayout(false);
            this.Lis1.ResumeLayout(false);
            this.Lis2.ResumeLayout(false);
            this.Lis3.ResumeLayout(false);
            this.Lis4.ResumeLayout(false);
            this.Lis5.ResumeLayout(false);
            this.Lis6.ResumeLayout(false);
            this.Lis7.ResumeLayout(false);
            this.Lis8.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip DefMenu;
        private System.Windows.Forms.ToolStripMenuItem addAnAppToTheListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedAppsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog AddApp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Lis1;
        private System.Windows.Forms.TabPage Lis2;
        private System.Windows.Forms.TabPage Lis3;
        private System.Windows.Forms.TabPage Lis4;
        private System.Windows.Forms.TabPage Lis5;
        private System.Windows.Forms.TabPage Lis6;
        private System.Windows.Forms.TabPage Lis7;
        private System.Windows.Forms.TabPage Lis8;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.ListBox listBox5;
        private System.Windows.Forms.ListBox listBox6;
        private System.Windows.Forms.ListBox listBox7;
        private System.Windows.Forms.ListBox listBox8;
        private System.Windows.Forms.ToolStripMenuItem addAnAppToTheListAppNameOnlyToolStripMenuItem;
    }
}