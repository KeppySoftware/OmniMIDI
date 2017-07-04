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
    public partial class MIDIEventsParserSettings : Form
    {
        public MIDIEventsParserSettings()
        {
            InitializeComponent();
        }

        private void MIDIEventsParserSettings_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("allnotesignore", 0)) == 1)
            {
                AllNotesIgnore.Checked = true;
                SysExIgnore.Checked = true;
            }

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("ignorenotes1", 0)) == 1)
            {
                IgnoreNotes.Checked = true;
                IgnoreNotesInterval.Enabled = true;
            }

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("capframerate", 1)) == 1)
                CapFram.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("limit88", 0)) == 1)
                Limit88.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sysexignore", 0)) == 1)
                SysExIgnore.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("fullvelocity", 0)) == 1)
                FullVelocityMode.Checked = true;

            CAE.Text = String.Format(CAE.Text, KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text);
        }

        private void CapFram_CheckedChanged(object sender, EventArgs e)
        {
            if (CapFram.Checked)
                Functions.SetFramerate(1);
            else
                Functions.SetFramerate(0);
        }

        private void Limit88_CheckedChanged(object sender, EventArgs e)
        {
            if (Limit88.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("limit88", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("limit88", "0", RegistryValueKind.DWord);
        }

        private void SysExIgnore_CheckedChanged(object sender, EventArgs e)
        {
            if (SysExIgnore.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sysexignore", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sysexignore", "0", RegistryValueKind.DWord);
        }

        private void AllNotesIgnore_CheckedChanged(object sender, EventArgs e)
        {
            if (AllNotesIgnore.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("allnotesignore", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("allnotesignore", "0", RegistryValueKind.DWord);
        }

        private void IgnoreNotes_CheckedChanged(object sender, EventArgs e)
        {
            if (IgnoreNotes.Checked)
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("ignorenotes1", "1", RegistryValueKind.DWord);
                IgnoreNotesInterval.Enabled = true;
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("ignorenotes1", "0", RegistryValueKind.DWord);
                IgnoreNotesInterval.Enabled = false;
            }
        }

        private void FullVelocityMode_CheckedChanged(object sender, EventArgs e)
        {
            if (FullVelocityMode.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("fullvelocity", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("fullvelocity", "0", RegistryValueKind.DWord);
        }

        private void IgnoreNotesInterval_Click(object sender, EventArgs e)
        {
            new KeppySynthVelocityIntervals().ShowDialog();
        }

        private void RevbNChor_Click(object sender, EventArgs e)
        {
            new RevbNChorForm().ShowDialog();
        }

        private void EVBufDialog_Click(object sender, EventArgs e)
        {
            new EVBufferManager().ShowDialog();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
