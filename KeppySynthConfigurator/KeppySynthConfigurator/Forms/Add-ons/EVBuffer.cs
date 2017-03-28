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
    public partial class KeppySynthEVBuffer : Form
    {
        public KeppySynthEVBuffer()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (numericUpDown1.Value >= 65537)
                {
                    DialogResult dialogResult = MessageBox.Show("Using a value bigger than 65536 could increase the lag with certain applications.\n\nAre you sure you want to use the following value: " + numericUpDown1.Value + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                        Settings.SetValue("newevbuffvalue", numericUpDown1.Value, RegistryValueKind.DWord);
                        this.Close();
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else if (numericUpDown1.Value <= 1023)
                {
                    DialogResult dialogResult = MessageBox.Show("Using a value smaller than 1024 could make the application hang.\n\nAre you sure you want to use the following value: " + numericUpDown1.Value + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                        Settings.SetValue("newevbuffvalue", numericUpDown1.Value, RegistryValueKind.DWord);
                        this.Close();
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                    RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                    Settings.SetValue("newevbuffvalue", numericUpDown1.Value, RegistryValueKind.DWord);
                    this.Close();
                }
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
                numericUpDown1.Value = 16384;
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                Settings.SetValue("newevbuffvalue", 16384, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KeppyDriverEVBuffer_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                int potato = Convert.ToInt32(Settings.GetValue("newevbuffvalue", 16384));
                if (potato > 32768)
                {
                    numericUpDown1.Value = 32768;
                }
                else if (potato < 1)
                {
                    numericUpDown1.Value = 1;
                }
                else
                {
                    numericUpDown1.Value = potato;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
