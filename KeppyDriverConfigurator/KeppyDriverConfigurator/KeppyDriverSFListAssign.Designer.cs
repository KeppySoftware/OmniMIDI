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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Back = new System.Windows.Forms.Button();
            this.Next = new System.Windows.Forms.Button();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.listBox6 = new System.Windows.Forms.ListBox();
            this.listBox7 = new System.Windows.Forms.ListBox();
            this.listBox8 = new System.Windows.Forms.ListBox();
            this.AddApp = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.DefMenu.SuspendLayout();
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
            this.removeSelectedAppsToolStripMenuItem,
            this.toolStripSeparator1,
            this.clearListToolStripMenuItem});
            this.DefMenu.Name = "DefMenu";
            this.DefMenu.Size = new System.Drawing.Size(200, 76);
            // 
            // addAnAppToTheListToolStripMenuItem
            // 
            this.addAnAppToTheListToolStripMenuItem.Name = "addAnAppToTheListToolStripMenuItem";
            this.addAnAppToTheListToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.addAnAppToTheListToolStripMenuItem.Text = "Add an app to the list...";
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
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.DefMenu;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 88);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(149, 277);
            this.listBox1.TabIndex = 2;
            // 
            // listBox2
            // 
            this.listBox2.ContextMenuStrip = this.DefMenu;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(170, 88);
            this.listBox2.Name = "listBox2";
            this.listBox2.ScrollAlwaysVisible = true;
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox2.Size = new System.Drawing.Size(149, 277);
            this.listBox2.TabIndex = 4;
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
            // listBox3
            // 
            this.listBox3.ContextMenuStrip = this.DefMenu;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(325, 88);
            this.listBox3.Name = "listBox3";
            this.listBox3.ScrollAlwaysVisible = true;
            this.listBox3.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox3.Size = new System.Drawing.Size(149, 277);
            this.listBox3.TabIndex = 6;
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
            // listBox4
            // 
            this.listBox4.ContextMenuStrip = this.DefMenu;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(480, 88);
            this.listBox4.Name = "listBox4";
            this.listBox4.ScrollAlwaysVisible = true;
            this.listBox4.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox4.Size = new System.Drawing.Size(149, 277);
            this.listBox4.TabIndex = 8;
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
            // Back
            // 
            this.Back.Enabled = false;
            this.Back.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Back.Location = new System.Drawing.Point(547, 24);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(38, 34);
            this.Back.TabIndex = 9;
            this.Back.Text = "<";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // Next
            // 
            this.Next.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Next.Location = new System.Drawing.Point(591, 24);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(38, 34);
            this.Next.TabIndex = 10;
            this.Next.Text = ">";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // listBox5
            // 
            this.listBox5.ContextMenuStrip = this.DefMenu;
            this.listBox5.FormattingEnabled = true;
            this.listBox5.Location = new System.Drawing.Point(15, 88);
            this.listBox5.Name = "listBox5";
            this.listBox5.ScrollAlwaysVisible = true;
            this.listBox5.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox5.Size = new System.Drawing.Size(149, 277);
            this.listBox5.TabIndex = 11;
            this.listBox5.Visible = false;
            // 
            // listBox6
            // 
            this.listBox6.ContextMenuStrip = this.DefMenu;
            this.listBox6.FormattingEnabled = true;
            this.listBox6.Location = new System.Drawing.Point(170, 88);
            this.listBox6.Name = "listBox6";
            this.listBox6.ScrollAlwaysVisible = true;
            this.listBox6.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox6.Size = new System.Drawing.Size(149, 277);
            this.listBox6.TabIndex = 12;
            this.listBox6.Visible = false;
            // 
            // listBox7
            // 
            this.listBox7.ContextMenuStrip = this.DefMenu;
            this.listBox7.FormattingEnabled = true;
            this.listBox7.Location = new System.Drawing.Point(325, 88);
            this.listBox7.Name = "listBox7";
            this.listBox7.ScrollAlwaysVisible = true;
            this.listBox7.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox7.Size = new System.Drawing.Size(149, 277);
            this.listBox7.TabIndex = 13;
            this.listBox7.Visible = false;
            // 
            // listBox8
            // 
            this.listBox8.ContextMenuStrip = this.DefMenu;
            this.listBox8.FormattingEnabled = true;
            this.listBox8.Location = new System.Drawing.Point(480, 88);
            this.listBox8.Name = "listBox8";
            this.listBox8.ScrollAlwaysVisible = true;
            this.listBox8.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox8.Size = new System.Drawing.Size(149, 277);
            this.listBox8.TabIndex = 14;
            this.listBox8.Visible = false;
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
            // KeppyDriverSFListAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 378);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox8);
            this.Controls.Add(this.listBox7);
            this.Controls.Add(this.listBox6);
            this.Controls.Add(this.listBox5);
            this.Controls.Add(this.Next);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.listBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
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
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.ListBox listBox5;
        private System.Windows.Forms.ListBox listBox6;
        private System.Windows.Forms.ListBox listBox7;
        private System.Windows.Forms.ListBox listBox8;
        private System.Windows.Forms.OpenFileDialog AddApp;
        private System.Windows.Forms.Label label5;
    }
}