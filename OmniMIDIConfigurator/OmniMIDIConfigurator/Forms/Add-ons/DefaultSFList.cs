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

namespace OmniMIDIConfigurator
{
    public partial class KeppySynthDefaultSFList : Form
    {
        public KeppySynthDefaultSFList()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Settings", true);
                Settings.SetValue("defaultsflist", numericUpDown1.Value, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Default_Click(object sender, EventArgs e)
        {
            try
            {
                numericUpDown1.Value = 1;
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Settings", true);
                Settings.SetValue("defaultsflist", 1, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KeppyDriverDefaultSFList_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Settings", true);
                int potato = Convert.ToInt32(Settings.GetValue("DefaultSFList", 1));
                numericUpDown1.Value = potato;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
