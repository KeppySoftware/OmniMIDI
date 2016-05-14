using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverUpdater : Form
    {
        public KeppyDriverUpdater()
        {
            InitializeComponent();
        }

        private void KeppyDriverUpdater_Load(object sender, EventArgs e)
        {
            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppydrv\\keppydrv.dll");
            ThisVersion.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString();
        }

        private void UpdateCheck_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateCheck.Enabled = false;
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Driver/master/output/keppydriverupdate.txt");
                StreamReader reader = new StreamReader(stream);
                String newestversion = reader.ReadToEnd();
                FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppydrv\\keppydrv.dll");
                LatestVersion.Text = "Checking for updates, please wait...";
                ThisVersion.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString();
                Version x = null;
                Version.TryParse(newestversion.ToString(), out x);
                Version y = null;
                Version.TryParse(Driver.FileVersion.ToString(), out y);
                if (x > y)
                {
                    UpdateCheck.Enabled = true;
                    LatestVersion.Text = "New updates found! Version " + newestversion.ToString() + " is online!";
                    MessageBox.Show("New update found, press OK to open the release page.", "New update found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Process.Start("https://github.com/KaleidonKep99/Keppy-s-Driver/releases");
                }
                else if (x < y)
                {
                    UpdateCheck.Enabled = true;
                    LatestVersion.Text = "Seems that the version on GitHub (" + newestversion.ToString() + ") is older than the version you're currently using.\nReally strange huh?";
                    MessageBox.Show("Is this a joke? You have a newer version than the one currently released on GitHub...\n\nYou dirty hacker.", "Wowie.", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else
                {
                    UpdateCheck.Enabled = true;
                    LatestVersion.Text = "There are no updates available right now. Try checking later.";
                    MessageBox.Show("This release is already updated.", "No updates found.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                UpdateCheck.Enabled = true;
                FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppydrv\\keppydrv.dll");
                ThisVersion.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString();
                LatestVersion.Text = "Can not check for updates! You're offline, or maybe the website is temporarily down.";
                MessageBox.Show("Can not check for updates!\n\nSpecific .NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
