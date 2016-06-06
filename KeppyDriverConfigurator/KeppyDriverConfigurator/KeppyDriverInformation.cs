using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverInformation : Form
    {
        public KeppyDriverInformation()
        {
            InitializeComponent();
        }

        private void KeppyDriverInformation_Load(object sender, EventArgs e)
        {
            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppydrv\\keppydrv.dll");
            Label8.Text = "Keppy's Driver by Keppy Studios\nVersion " + Driver.FileVersion.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void u4sforum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.un4seen.com/forum/?board=1.0");
        }
    }
}
