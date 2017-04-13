using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class SelectBranch : Form
    {
        public SelectBranch()
        {
            InitializeComponent();
        }

        private string ReturnBranch(String Branch)
        {
            return String.Format("Selected branch: {0}", Branch);
        }

        private void SelectBranch_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UpdateBranch == "canary")
            {
                BranchSel.Text = "Canary branch";
                CurrentBranch.Text = ReturnBranch("Canary branch");
            }
            else if (Properties.Settings.Default.UpdateBranch == "normal")
            {
                BranchSel.Text = "Normal branch (Default)";
                CurrentBranch.Text = ReturnBranch("Normal branch");
            }
            else if (Properties.Settings.Default.UpdateBranch == "delay")
            {
                BranchSel.Text = "Delayed branch";
                CurrentBranch.Text = ReturnBranch("Delayed branch");
            }
            else if (Properties.Settings.Default.UpdateBranch == "choose")
            {
                BranchSel.Text = "Choose a branch";
                CurrentBranch.Text = ReturnBranch("None selected");
            }
            else
            {
                BranchSel.Text = "Normal branch (Default)";
                CurrentBranch.Text = ReturnBranch("Normal branch");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BranchSel.Text == "Canary branch")
            {
                Properties.Settings.Default.UpdateBranch = "canary";
                Properties.Settings.Default.Save();
                Close();
            }
            else if (BranchSel.Text == "Normal branch (Default)")
            {
                Properties.Settings.Default.UpdateBranch = "normal";
                Properties.Settings.Default.Save();
                Close();
            }         
            else if (BranchSel.Text == "Delayed branch")
            {
                Properties.Settings.Default.UpdateBranch = "delay";
                Properties.Settings.Default.Save();
                Close();
            }
            else if (BranchSel.Text == "")
            {
                MessageBox.Show("Select a branch first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                Properties.Settings.Default.UpdateBranch = "normal";
                Properties.Settings.Default.Save();
                return;
            }

            if (BranchSel.Text == "Canary branch")
            {
                Properties.Settings.Default.UpdateBranch = "canary";
                Properties.Settings.Default.Save();
                return;
            }
            else if (BranchSel.Text == "Normal branch (Default)")
            {
                Properties.Settings.Default.UpdateBranch = "normal";
                Properties.Settings.Default.Save();
                return;
            }
            else if (BranchSel.Text == "Delayed branch")
            {
                Properties.Settings.Default.UpdateBranch = "delay";
                Properties.Settings.Default.Save();
                return;
            }
            else if (BranchSel.Text == "")
            {
                e.Cancel = true;
                MessageBox.Show("Select a branch first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
