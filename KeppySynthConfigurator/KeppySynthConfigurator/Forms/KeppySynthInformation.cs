using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthInformation : Form
    {
        public KeppySynthInformation()
        {
            InitializeComponent();
        }

        private void KeppyDriverInformation_Load(object sender, EventArgs e)
        {
            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll");
            Label8.Text = "Keppy's Synthesizer\nVersion " + Driver.FileVersion.ToString();
        }

        private void CFU_Click(object sender, EventArgs e)
        {
            KeppySynthUpdater frm = new KeppySynthUpdater();
            frm.ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void u4sforum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.un4seen.com/forum/?board=1.0");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string url = "";

            string business = "prapapappo1999@gmail.com";
            string description = "Donation";
            string country = "US";
            string currency = "USD";

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            Process.Start(url);
        }
    }
}
