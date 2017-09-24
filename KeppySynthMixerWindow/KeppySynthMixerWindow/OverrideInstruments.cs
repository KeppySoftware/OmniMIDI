using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthMixerWindow
{
    public partial class OverrideInstruments : Form
    {
        public static RegistryKey ChanOverride = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\ChanOverride", true);

        public OverrideInstruments()
        {
            InitializeComponent();
        }

        private void OverrideInstruments_Load(object sender, EventArgs e)
        {
            // Is override on or off
            if ((Int32)ChanOverride.GetValue("overrideinstruments", 0) == 1)
            {
                EnableOrNot.Checked = true;
                ChannelsBox.Enabled = true;
            }

            // Banks
            BC1.Value = (Int32)ChanOverride.GetValue("bc1", 0);
            BC2.Value = (Int32)ChanOverride.GetValue("bc2", 0);
            BC3.Value = (Int32)ChanOverride.GetValue("bc3", 0);
            BC4.Value = (Int32)ChanOverride.GetValue("bc4", 0);
            BC5.Value = (Int32)ChanOverride.GetValue("bc5", 0);
            BC6.Value = (Int32)ChanOverride.GetValue("bc6", 0);
            BC7.Value = (Int32)ChanOverride.GetValue("bc7", 0);
            BC8.Value = (Int32)ChanOverride.GetValue("bc8", 0);
            BC9.Value = (Int32)ChanOverride.GetValue("bc9", 0);
            BCD.Value = (Int32)ChanOverride.GetValue("bcd", 0);
            BC11.Value = (Int32)ChanOverride.GetValue("bc11", 0);
            BC12.Value = (Int32)ChanOverride.GetValue("bc12", 0);
            BC13.Value = (Int32)ChanOverride.GetValue("bc13", 0);
            BC14.Value = (Int32)ChanOverride.GetValue("bc14", 0);
            BC15.Value = (Int32)ChanOverride.GetValue("bc15", 0);
            BC16.Value = (Int32)ChanOverride.GetValue("bc16", 0);

            // Presets
            PC1.Value = (Int32)ChanOverride.GetValue("pc1", 0);
            PC2.Value = (Int32)ChanOverride.GetValue("pc2", 0);
            PC3.Value = (Int32)ChanOverride.GetValue("pc3", 0);
            PC4.Value = (Int32)ChanOverride.GetValue("pc4", 0);
            PC5.Value = (Int32)ChanOverride.GetValue("pc5", 0);
            PC6.Value = (Int32)ChanOverride.GetValue("pc6", 0);
            PC7.Value = (Int32)ChanOverride.GetValue("pc7", 0);
            PC8.Value = (Int32)ChanOverride.GetValue("pc8", 0);
            PC9.Value = (Int32)ChanOverride.GetValue("pc9", 0);
            PCD.Value = (Int32)ChanOverride.GetValue("pcd", 0);
            PC11.Value = (Int32)ChanOverride.GetValue("pc11", 0);
            PC12.Value = (Int32)ChanOverride.GetValue("pc12", 0);
            PC13.Value = (Int32)ChanOverride.GetValue("pc13", 0);
            PC14.Value = (Int32)ChanOverride.GetValue("pc14", 0);
            PC15.Value = (Int32)ChanOverride.GetValue("pc15", 0);
            PC16.Value = (Int32)ChanOverride.GetValue("pc16", 0);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            // Set override on or off
            if (EnableOrNot.Checked == true)
                ChanOverride.SetValue("overrideinstruments", 1, RegistryValueKind.DWord);
           else
                ChanOverride.SetValue("overrideinstruments", 0, RegistryValueKind.DWord);

            // Banks
            ChanOverride.SetValue("bc1", BC1.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc2", BC2.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc3", BC3.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc4", BC4.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc5", BC5.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc6", BC6.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc7", BC7.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc8", BC8.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc9", BC9.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bcd", BCD.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc11", BC11.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc12", BC12.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc13", BC13.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc14", BC14.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc15", BC15.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc16", BC16.Value, RegistryValueKind.DWord);

            // Presets
            ChanOverride.SetValue("pc1", PC1.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc2", PC2.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc3", PC3.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc4", PC4.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc5", PC5.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc6", PC6.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc7", PC7.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc8", PC8.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc9", PC9.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pcd", PCD.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc11", PC11.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc12", PC12.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc13", PC13.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc14", PC14.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc15", PC15.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc16", PC16.Value, RegistryValueKind.DWord);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CheckMT32.Enabled = false;
            ChanOverride.Close();
            Close();
        }

        private void CheckMT32_Tick(object sender, EventArgs e)
        {
            if ((Int32)KeppySynthMixerWindow.Settings.GetValue("mt32mode", 0) == 1)
            {
                EnableOrNot.Enabled = false;
                ChannelsBox.Enabled = false;
                SaveBtn.Enabled = false;
                MT32IsEnabled.Visible = true;
            }
            else
            {
                EnableOrNot.Enabled = true;
                SaveBtn.Enabled = true;
                MT32IsEnabled.Visible = false;

                if (EnableOrNot.Checked == true)
                    ChannelsBox.Enabled = true;
                else
                    ChannelsBox.Enabled = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var helpFile = Path.Combine(Path.GetTempPath(), "help.txt");
            File.WriteAllText(helpFile, Properties.Resources.gmlist);
            Process.Start(helpFile);
        }
    }
}
