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
            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("IgnoreAllNotes", 0)) == 1)
            {
                AllNotesIgnore.Checked = true;
                SysExIgnore.Checked = true;
            }

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("IgnoreNotesBetweenVel", 0)) == 1)
            {
                IgnoreNotes.Checked = true;
                IgnoreNotesInterval.Enabled = true;
            }

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("CapFramerate", 1)) == 1)
                CapFram.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("LimitTo88Keys", 0)) == 1)
                Limit88.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("IgnoreSysEx", 0)) == 1)
                SysExIgnore.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("FullVelocityMode", 0)) == 1)
                FullVelocityMode.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("MT32Mode", 0)) == 1)
                MT32Mode.Checked = true;

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
                KeppySynthConfiguratorMain.SynthSettings.SetValue("LimitTo88Keys", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("LimitTo88Keys", "0", RegistryValueKind.DWord);
        }

        private void SysExIgnore_CheckedChanged(object sender, EventArgs e)
        {
            if (SysExIgnore.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("IgnoreSysEx", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("IgnoreSysEx", "0", RegistryValueKind.DWord);
        }

        private void AllNotesIgnore_CheckedChanged(object sender, EventArgs e)
        {
            if (AllNotesIgnore.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("IgnoreAllNotes", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("IgnoreAllNotes", "0", RegistryValueKind.DWord);
        }

        private void IgnoreNotes_CheckedChanged(object sender, EventArgs e)
        {
            if (IgnoreNotes.Checked)
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("IgnoreNotesBetweenVel", "1", RegistryValueKind.DWord);
                IgnoreNotesInterval.Enabled = true;
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("IgnoreNotesBetweenVel", "0", RegistryValueKind.DWord);
                IgnoreNotesInterval.Enabled = false;
            }
        }

        private void FullVelocityMode_CheckedChanged(object sender, EventArgs e)
        {
            if (FullVelocityMode.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("FullVelocityMode", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("FullVelocityMode", "0", RegistryValueKind.DWord);
        }

        private void MT32Mode_CheckedChanged(object sender, EventArgs e)
        {
            if (MT32Mode.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("MT32Mode", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("MT32Mode", "0", RegistryValueKind.DWord);
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
