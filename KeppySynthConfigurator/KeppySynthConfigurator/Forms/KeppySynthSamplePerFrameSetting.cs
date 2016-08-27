using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthSamplePerFrameSetting : Form
    {
        public KeppySynthSamplePerFrameSetting()
        {
            InitializeComponent();
        }

        private void KeppyDriverSamplePerFrameSetting_Load(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            LeSetting.Value = Convert.ToInt32(Settings.GetValue("sndbfvalue", 100));
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (LeSetting.Value == 100)
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                Settings.SetValue("sndbfvalue", LeSetting.Value, RegistryValueKind.DWord);
                this.Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to use this value?\n\n(If [after the change] the driver stops working, try increasing/decreasing the value.)", "Report a bug...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                    Settings.SetValue("sndbfvalue", LeSetting.Value, RegistryValueKind.DWord);
                    this.Close();
                }
                else if (dialogResult == DialogResult.No)
                {

                }
            }
        }

        private void DefVal_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            LeSetting.Value = 100;
            Settings.SetValue("sndbfvalue", LeSetting.Value, RegistryValueKind.DWord);
            MessageBox.Show("Value restored to default!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
