using System;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class SelectBranch : Form
    {
        public SelectBranch()
        {
            InitializeComponent();

            BranchSel.Items.Add(Properties.Settings.Default.PreReleaseBranch[0]);
            BranchSel.Items.Add(Properties.Settings.Default.StableBranch[0]);
            BranchSel.Items.Add(Properties.Settings.Default.SlowBranch[0]);

            label2.Text = String.Format(label2.Text, 
                Properties.Settings.Default.PreReleaseBranch[0].Replace(" branch", ""),
                Properties.Settings.Default.StableBranch[0].Replace(" branch", ""),
                Properties.Settings.Default.SlowBranch[0].Replace(" branch", ""));
        }

        private string ReturnBranch(String Branch)
        {
            return String.Format("Selected branch: {0}", Branch);
        }

        private void SelectBranch_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.PreReleaseBranch[1])
            {
                BranchSel.Text = Properties.Settings.Default.PreReleaseBranch[0];
                CurrentBranch.Text = ReturnBranch(Properties.Settings.Default.PreReleaseBranch[0]);
            }
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.StableBranch[1])
            {
                BranchSel.Text = Properties.Settings.Default.StableBranch[0];
                CurrentBranch.Text = ReturnBranch(Properties.Settings.Default.StableBranch[0]);
            }
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.SlowBranch[1])
            {
                BranchSel.Text = Properties.Settings.Default.SlowBranch[0];
                CurrentBranch.Text = ReturnBranch(Properties.Settings.Default.SlowBranch[0]);
            }
            else
            {
                BranchSel.Text = "Choose a branch";
                CurrentBranch.Text = ReturnBranch("None selected");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BranchSel.Text.Equals(Properties.Settings.Default.PreReleaseBranch[0]))
            {
                Properties.Settings.Default.UpdateBranch = Properties.Settings.Default.PreReleaseBranch[1];
                Properties.Settings.Default.Save();
            }
            else if (BranchSel.Text == Properties.Settings.Default.StableBranch[0])
            {
                Properties.Settings.Default.UpdateBranch = Properties.Settings.Default.StableBranch[1];
                Properties.Settings.Default.Save();
            }         
            else if (BranchSel.Text == Properties.Settings.Default.SlowBranch[0])
            {
                Properties.Settings.Default.UpdateBranch = Properties.Settings.Default.SlowBranch[1];
                Properties.Settings.Default.Save();
            }

            if (String.IsNullOrEmpty(BranchSel.Text))
            {
                MessageBox.Show("Select a branch first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Ignore any change, and let the user select on next boot
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;

            if (BranchSel.Text.Equals(Properties.Settings.Default.PreReleaseBranch[0]))
            {
                Properties.Settings.Default.UpdateBranch = Properties.Settings.Default.PreReleaseBranch[1];
                Properties.Settings.Default.Save();
            }
            else if (BranchSel.Text == Properties.Settings.Default.StableBranch[0])
            {
                Properties.Settings.Default.UpdateBranch = Properties.Settings.Default.StableBranch[1];
                Properties.Settings.Default.Save();
            }
            else if (BranchSel.Text == Properties.Settings.Default.SlowBranch[0])
            {
                Properties.Settings.Default.UpdateBranch = Properties.Settings.Default.SlowBranch[1];
                Properties.Settings.Default.Save();
            }
            else if (String.IsNullOrEmpty(BranchSel.Text))
            {
                e.Cancel = true;
                MessageBox.Show("Select a branch first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.OnFormClosing(e);
        }
    }
}
