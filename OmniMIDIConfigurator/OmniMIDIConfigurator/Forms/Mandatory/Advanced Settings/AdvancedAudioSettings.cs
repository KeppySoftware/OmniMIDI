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
            SlowDownPlayback.Enabled = !HMode.Checked;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("NotesCatcherWithAudio", 0)) == 1)
                OldBuff.Checked = true;

            if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("SleepStates", 1)) == 0)
                NoSleep.Checked = true;

            int FPVal = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioBitDepth", 1));

            if (FPVal == 1)
                AudioBitDepth.SelectedIndex = 0;
            else if (FPVal == 2 || FPVal == 0)
                AudioBitDepth.SelectedIndex = 1;
            else if (FPVal == 3)
                AudioBitDepth.SelectedIndex = 2;

            OldBuff.Enabled = !(OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0);
            NoSleep.Enabled = !(OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0);
            ChangeDefaultOutput.Enabled = (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex != AudioEngine.AUDTOWAV);

            CAE.Text = String.Format(CAE.Text, OmniMIDIConfiguratorMain.Delegate.AudioEngBox.Text);
        }

        private void AudioBitDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AudioBitDepth.SelectedIndex <= 2)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", AudioBitDepth.SelectedIndex + 1);
            else
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", 1);
        }

        private void MonophonicFunc_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("MonoRendering", Convert.ToInt32(MonophonicFunc.Checked), RegistryValueKind.DWord);
        }

        private void FadeoutDisable_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DisableNotesFadeOut", Convert.ToInt32(FadeoutDisable.Checked), RegistryValueKind.DWord);
        }

        private void SlowDownPlayback_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DontMissNotes", Convert.ToInt32(SlowDownPlayback.Checked), RegistryValueKind.DWord);
        }

        private void KSDAPIBox_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("KDMAPIEnabled", Convert.ToInt32(KSDAPIBox.Checked), RegistryValueKind.DWord);
        }

        private void HMode_CheckedChanged(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("HyperPlayback", Convert.ToInt32(HMode.Checked), RegistryValueKind.DWord);
            SlowDownPlayback.Enabled = !HMode.Checked;
        }

        private void OldBuff_CheckedChanged(object sender, EventArgs e)
        {
            Functions.OldBufferMode(Convert.ToInt32(OldBuff.Checked));
        }

        private void NoSleep_CheckedChanged(object sender, EventArgs e)
        {
            Functions.SleepStates(Convert.ToInt32(!NoSleep.Checked));
        }

        private void ChangeDefaultOutput_Click(object sender, EventArgs e)
        {
            if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == AudioEngine.DSOUND_ENGINE)
            {
                DefaultOutput frm = new DefaultOutput(false);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == AudioEngine.WASAPI_ENGINE)
            {
                DefaultOutput frm = new DefaultOutput(true);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == AudioEngine.ASIO_ENGINE)
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
