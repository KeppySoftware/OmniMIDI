using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class MIDIEventsParserSettings : Form
    {
        public MIDIEventsParserSettings()
        {
            InitializeComponent();
        }

        private void MIDIEventsParserSettings_Load(object sender, EventArgs e)
        {
            AllNotesIgnore.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreAllNotes", 0));
            SysExIgnore.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreAllNotes", 0));

            IgnoreNotes.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreNotesBetweenVel", 0));
            IgnoreNotesInterval.Enabled = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreNotesBetweenVel", 0));

            CapFram.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("CapFramerate", 1));
            Limit88.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("LimitTo88Keys", 0));
            SysExIgnore.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreSysEx", 0));
            FullVelocityMode.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("FullVelocityMode", 0));
            MT32Mode.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("MT32Mode", 0));
            CloseStreamMidiOutClose.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("CloseStreamMidiOutClose", 1));

            CAE.Text = String.Format(CAE.Text, OmniMIDIConfiguratorMain.Delegate.AudioEngBox.Text);
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
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("LimitTo88Keys", Convert.ToInt32(Limit88.Checked), RegistryValueKind.DWord);
        }

        private void SysExIgnore_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreSysEx", Convert.ToInt32(SysExIgnore.Checked), RegistryValueKind.DWord);
        }

        private void AllNotesIgnore_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreAllEvents", Convert.ToInt32(AllNotesIgnore.Checked), RegistryValueKind.DWord);
        }

        private void IgnoreNotes_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreNotesBetweenVel", Convert.ToInt32(IgnoreNotes.Checked), RegistryValueKind.DWord);
            IgnoreNotesInterval.Enabled = IgnoreNotes.Checked;
        }

        private void FullVelocityMode_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("FullVelocityMode", Convert.ToInt32(FullVelocityMode.Checked), RegistryValueKind.DWord);
        }

        private void MT32Mode_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("MT32Mode", Convert.ToInt32(MT32Mode.Checked), RegistryValueKind.DWord);
        }

        private void CloseStreamMidiOutClose_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("CloseStreamMidiOutClose", Convert.ToInt32(CloseStreamMidiOutClose.Checked), RegistryValueKind.DWord);
        }

        private void IgnoreNotesInterval_Click(object sender, EventArgs e)
        {
            new VelocityIntervals().ShowDialog();
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
