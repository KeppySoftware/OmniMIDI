using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class RevbNChorForm : Form
    {
        public RevbNChorForm()
        {
            InitializeComponent();
        }

        private void RevbNChorForm_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("RCOverride", 0)) == 1)
                EnableRCOverride.Checked = true;
            else
                EnableRCOverride.Checked = false;
            HandlerRC();
        }

        private void EnableRCOverride_CheckedChanged(object sender, EventArgs e)
        {
            HandlerRC();
        }

        private void HandlerRC()
        {
            if (EnableRCOverride.Checked)
            {
                ReverbL.Enabled = true;
                ReverbV.Enabled = true;
                ChorusL.Enabled = true;
                ChorusV.Enabled = true;
                KeppySynthConfiguratorMain.SynthSettings.SetValue("RCOverride", 1, RegistryValueKind.DWord);
            }
            else
            {
                ReverbL.Enabled = false;
                ReverbV.Enabled = false;
                ChorusL.Enabled = false;
                ChorusV.Enabled = false;
                KeppySynthConfiguratorMain.SynthSettings.SetValue("RCOverride", 0, RegistryValueKind.DWord);
            }
            ReverbV.Value = Between0And127(Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("Reverb", 64)));
            ChorusV.Value = Between0And127(Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("Chorus", 64)));
        }

        private int Between0And127(int integer)
        {
            if (integer < 0)
                return 0;
            else if (integer > 127)
                return 127;
            else
                return integer;
        }

        private void ReverbV_ValueChanged(object sender, EventArgs e)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("Reverb", ReverbV.Value, RegistryValueKind.DWord);
        }

        private void ChorusV_ValueChanged(object sender, EventArgs e)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("Chorus", ChorusV.Value, RegistryValueKind.DWord);
        }

        private void ResDef_Click(object sender, EventArgs e)
        {
            ReverbV.Value = 64;
            ChorusV.Value = 64;
            KeppySynthConfiguratorMain.SynthSettings.SetValue("Reverb", ReverbV.Value, RegistryValueKind.DWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("Chorus", ReverbV.Value, RegistryValueKind.DWord);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
