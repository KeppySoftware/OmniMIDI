using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class Donate : Form
    {
        public Donate()
        {
            InitializeComponent();

            DonateBtn.Image = Properties.Resources.DonateBtn;
        }

        private void DonateBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://paypal.me/Keppy99");
        }

        private void ShowMeNext_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DonationShownWhen = DateTime.Now;
            Properties.Settings.Default.DonationAlreadyShown = true;
            Properties.Settings.Default.Save();
            Close();
        }

        private void DontShowAnymore_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DonationShownWhen = DateTime.Now;
            Properties.Settings.Default.DonationAlreadyShown = true;
            Properties.Settings.Default.DonationDoNotShow = true;
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
