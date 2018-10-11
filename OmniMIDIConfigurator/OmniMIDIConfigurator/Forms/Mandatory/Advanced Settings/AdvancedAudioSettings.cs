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
    public partial class AdvancedAudioSettings : Form
    {
        public AdvancedAudioSettings()
        {
            InitializeComponent();
        }

        private void AdvancedAudioSettings_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DisableNotesFadeOut", 0)) == 1)
                FadeoutDisable.Checked = true;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("MonoRendering", 0)) == 1)
                MonophonicFunc.Checked = true;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DontMissNotes", 0)) == 1)
                SlowDownPlayback.Checked = true;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("KSDAPIEnabled", 1)) == 1)
                KSDAPIBox.Checked = true;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0)) == 1)
                HMode.Checked = true;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("NotesCatcherWithAudio", 0)) == 1)
                OldBuff.Checked = true;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("SleepStates", 1)) == 0)
                NoSleep.Checked = true;

            int floatingpointaudioval = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioBitDepth", 1));
            if (floatingpointaudioval == 1)
                AudioBitDepth.SelectedIndex = 0;
            else if (floatingpointaudioval == 2 || floatingpointaudioval == 0)
                AudioBitDepth.SelectedIndex = 1;
            else if (floatingpointaudioval == 3)
                AudioBitDepth.SelectedIndex = 2;
            else
                AudioBitDepth.SelectedIndex = 0;

            OldBuff.Enabled = !(OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0);
            NoSleep.Enabled = !(OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0);
            ChangeDefaultOutput.Enabled = (
                OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 1 ||
                OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 2 ||
                OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 3
                );

            CAE.Text = String.Format(CAE.Text, OmniMIDIConfiguratorMain.Delegate.AudioEngBox.Text);
        }

        private void AudioBitDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AudioBitDepth.SelectedIndex == 0)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", 1);
            else if (AudioBitDepth.SelectedIndex == 1)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", 2);
            else if (AudioBitDepth.SelectedIndex == 2)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", 3);
            else
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", 1);
        }

        private void MonophonicFunc_CheckedChanged(object sender, EventArgs e)
        {
            if (MonophonicFunc.Checked)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MonoRendering", "1", RegistryValueKind.DWord);
            else
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MonoRendering", "0", RegistryValueKind.DWord);
        }

        private void FadeoutDisable_CheckedChanged(object sender, EventArgs e)
        {
            if (FadeoutDisable.Checked)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("DisableNotesFadeOut", "1", RegistryValueKind.DWord);
            else
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("DisableNotesFadeOut", "0", RegistryValueKind.DWord);
        }

        private void SlowDownPlayback_CheckedChanged(object sender, EventArgs e)
        {
            if (SlowDownPlayback.Checked)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("DontMissNotes", "1", RegistryValueKind.DWord);
            else
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("DontMissNotes", "0", RegistryValueKind.DWord);
        }

        private void KSDAPIBox_CheckedChanged(object sender, EventArgs e)
        {
            if (KSDAPIBox.Checked)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("KSDAPIEnabled", "1", RegistryValueKind.DWord);
            else
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("KSDAPIEnabled", "0", RegistryValueKind.DWord);
        }

        private void HMode_CheckedChanged(object sender, EventArgs e)
        {
            if (HMode.Checked)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("HyperPlayback", "1", RegistryValueKind.DWord);
            else
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("HyperPlayback", "0", RegistryValueKind.DWord);
        }

        private void OldBuff_CheckedChanged(object sender, EventArgs e)
        {
            if (OldBuff.Checked)
                Functions.OldBufferMode(1);
            else
                Functions.OldBufferMode(0);
        }

        private void NoSleep_CheckedChanged(object sender, EventArgs e)
        {
            if (NoSleep.Checked)
                Functions.SleepStates(0);
            else
                Functions.SleepStates(1);
        }

        private void ChangeDefaultOutput_Click(object sender, EventArgs e)
        {
            if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.Text == "DirectX Audio")
            {
                DefaultOutput frm = new DefaultOutput(false);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.Text == "WASAPI")
            {
                DefaultOutput frm = new DefaultOutput(true);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.Text == "ASIO")
            {
                DefaultASIOAudioOutput frm = new DefaultASIOAudioOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
        }

        private void KSDAPIBoxWhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you uncheck this option, some apps might be forced to fallback to the stock Windows Multimedia API, which increases latency." +
                "\nKeep in mind that not all KDMAPI-ready apps do check for this value, and they might use it whether you want them to or not." +
                "\n\n(This value will not affect the Windows Multimedia Wrapper.)", 
                "OmniMIDI - Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HModeWhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicking this checkbox will remove all the checks done to the events, for example transposing and other settings in the configurator.\n" +
                "The events will be sent straight to the buffer, and played immediately.\n\n" +
                "The \"Slow down playback instead of skipping notes\" checkbox will not work, while this mode is enabled, along with other event processing-related functions.\n\n" +
                "WARNING: Playing too much with the live changes while this setting is enabled might crash the threads, rendering the synth unusable until a full restart of the application!",
                "OmniMIDI - Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChangePitchShifting_Click(object sender, EventArgs e)
        {
            PitchShifting frm = new PitchShifting();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
