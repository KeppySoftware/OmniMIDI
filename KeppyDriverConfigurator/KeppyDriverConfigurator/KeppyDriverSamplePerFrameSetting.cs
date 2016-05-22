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

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverSamplePerFrameSetting : Form
    {
        public KeppyDriverSamplePerFrameSetting()
        {
            InitializeComponent();
        }

        private void KeppyDriverSamplePerFrameSetting_Load(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            LeSetting.Value = Convert.ToInt32(Settings.GetValue("sndbfvalue", 100));
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            Settings.SetValue("sndbfvalue", LeSetting.Value, RegistryValueKind.DWord);
            this.Close();
        }

        private void DefVal_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            LeSetting.Value = 100;
            Settings.SetValue("sndbfvalue", LeSetting.Value, RegistryValueKind.DWord);
        }
    }
}
