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
            DisableCookedPlayer.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DisableCookedPlayer", 0));

            AllNotesIgnore.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0));
            SysExIgnore.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0));

            AllNotesIgnore.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreAllNotes", 0));
            SysExIgnore.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreAllNotes", 0));

            IgnoreNotes.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0));
            IgnoreNotes.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreNotesBetweenVel", 0));
            IgnoreNotesInterval.Enabled = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreNotesBetweenVel", 0));

            CapFram.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("CapFramerate", 1));

            Limit88.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0));
            Limit88.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("LimitTo88Keys", 0));

            SysExIgnore.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreSysEx", 0));

            FullVelocityMode.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0));
            FullVelocityMode.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("FullVelocityMode", 0));

            MT32Mode.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("MT32Mode", 0));

            OverrideNoteLength.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0));
            OverrideNoteLength.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("OverrideNoteLength", 0));
            NoteLengthValue.Enabled = (!Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0)) && Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("OverrideNoteLength", 0)));
            NoteLengthValue.Value = Convert.ToDecimal((double)Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("NoteLengthValue", 5)) / 1000.0);

            DelayNoteOff.Enabled = !Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0));
            DelayNoteOff.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DelayNoteOff", 0));
            NoteOffDelayValue.Enabled = (!Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0)) && Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DelayNoteOff", 0)));
            NoteOffDelayValue.Value = Convert.ToDecimal((double)Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DelayNoteOffValue", 5)) / 1000.0);

            CAE.Text = String.Format(CAE.Text, OmniMIDIConfiguratorMain.Delegate.AudioEngBox.Text);
        }

        private void CapFram_CheckedChanged(object sender, EventArgs e)
        {
            if (CapFram.Checked)
                Functions.SetFramerate(1);
            else
                Functions.SetFramerate(0);
        }

        private void DisableCookedPlayer_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DisableCookedPlayer", Convert.ToInt32(DisableCookedPlayer.Checked), RegistryValueKind.DWord);
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

        private void OverrideNoteLength_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("OverrideNoteLength", Convert.ToInt32(OverrideNoteLength.Checked), RegistryValueKind.DWord);
            NoteLengthValue.Enabled = OverrideNoteLength.Checked;
        }

        private void NoteLengthValue_ValueChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("NoteLengthValue", Convert.ToInt32(NoteLengthValue.Value * 1000));
        }

        private void DelayNoteOff_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DelayNoteOff", Convert.ToInt32(DelayNoteOff.Checked), RegistryValueKind.DWord);
            NoteOffDelayValue.Enabled = DelayNoteOff.Checked;
        }

        private void NoteOffDelayValue_ValueChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DelayNoteOffValue", Convert.ToInt32(NoteOffDelayValue.Value * 1000));
        }

        private void midiOutCloseDisabled_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This option doesn't guarantee that all the notes will be turned off immediately after the specified amount of time on the left numericbox." +
                "\nPedal hold and other special events might delay the noteoff event even more.",
                "OmniMIDI - Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
