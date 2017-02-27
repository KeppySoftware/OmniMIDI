using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthInformation : Form
    {
        public KeppySynthInformation()
        {
            InitializeComponent();
        }

        private DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        private void KeppyDriverInformation_Load(object sender, EventArgs e)
        {
            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll");
            Text = String.Format("Keppy's Synthesizer {0} (Build time: {1}, GMT +1)", Driver.FileVersion.ToString(), GetLinkerTime(Assembly.GetExecutingAssembly(), TimeZoneInfo.Local));
            Label8.Text = String.Format("Keppy's Synthesizer {0}\nUser-mode MIDI driver for Windows", Driver.FileVersion.ToString());
        }

        private void CFU_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                Functions.CheckForUpdates(true, false);
            }
            else
            {
                Functions.CheckForUpdates(false, false);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LicenseButton_Click(object sender, EventArgs e)
        {
            Process.Start(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\license.txt");
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
