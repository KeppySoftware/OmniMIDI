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
    public partial class KeppyDriverBufferSystem : Form
    {
        public KeppyDriverBufferSystem()
        {
            InitializeComponent();
        }

        private void KeppyDriverBufferSystem_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                if (Convert.ToInt32(Settings.GetValue("legacybuf")) == 0)
                {
                    NewBuf.Checked = true;
                }
                else
                {
                    OldBuf.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OK_Click(object sender, System.EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                if (NewBuf.Checked == true && OldBuf.Checked == false)
                {
                    Settings.SetValue("legacybuf", 0, RegistryValueKind.DWord);
                }
                else if (NewBuf.Checked == false && OldBuf.Checked == true)
                {
                    Settings.SetValue("legacybuf", 1, RegistryValueKind.DWord);
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
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                NewBuf.Checked = true;
                Settings.SetValue("legacybuf", 0, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OldBufferWarning_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Some apps might be incompatible with this buffer, and might crash or not work at all!\n(Ex. ZDoom/Zandronum and other games based on the Doom engine)\n\nBe careful while using it.", "About the old buffer system", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
