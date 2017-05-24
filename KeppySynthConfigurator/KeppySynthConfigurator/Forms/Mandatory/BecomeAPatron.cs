using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class BecomeAPatron : Form
    {
        public BecomeAPatron()
        {
            InitializeComponent();
            ButterBoy.Visible = Properties.Settings.Default.ButterBoy;
        }

        private void DontShowAnymore_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BecomeAPatronNow_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.patreon.com/KaleidonKep99");
            Close();
        }

        private void DontShowAnymore_MouseHover(object sender, EventArgs e)
        {
            ButterBoy.Image = Properties.Resources.bbcrying;
        }

        private void DontShowAnymore_MouseLeave(object sender, EventArgs e)
        {
            ButterBoy.Image = Properties.Resources.bbhappy;
        }

        private void BecomeAPatronNow_MouseHover(object sender, EventArgs e)
        {
            ButterBoy.Image = Properties.Resources.bbsurprised;
        }

        private void BecomeAPatronNow_MouseLeave(object sender, EventArgs e)
        {
            ButterBoy.Image = Properties.Resources.bbhappy;
        }
    }
}
