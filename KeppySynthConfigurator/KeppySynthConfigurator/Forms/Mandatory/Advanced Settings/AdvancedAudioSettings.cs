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
    public partial class AdvancedAudioSettings : Form
    {
        public AdvancedAudioSettings()
        {
            InitializeComponent();
        }

        private void AdvancedAudioSettings_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("fadeoutdisable", 0)) == 1)
                FadeoutDisable.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0)) == 1)
                MonophonicFunc.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("vms2emu", 0)) == 1)
                SlowDownPlayback.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("allowksdapi", 1)) == 1)
                KSDAPIBox.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("hypermode", 0)) == 1)
                HMode.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("oldbuffermode", 0)) == 1)
                OldBuff.Checked = true;

            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("rco", 1)) == 0)
                NoSleep.Checked = true;

            int floatingpointaudioval = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1));
            if (floatingpointaudioval == 1)
                AudioBitDepth.SelectedIndex = 0;
            else if (floatingpointaudioval == 2 || floatingpointaudioval == 0)
                AudioBitDepth.SelectedIndex = 1;
            else if (floatingpointaudioval == 3)
                AudioBitDepth.SelectedIndex = 2;
            else
                AudioBitDepth.SelectedIndex = 0;

            OldBuff.Enabled = !(KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0);
            NoSleep.Enabled = !(KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0);
            ChangeDefaultOutput.Enabled = (
                KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 1 ||
                KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 2 ||
                KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 3
                );

            CAE.Text = String.Format(CAE.Text, KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text);
        }

        private void AudioBitDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AudioBitDepth.SelectedIndex == 0)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("32bit", 1);
            else if (AudioBitDepth.SelectedIndex == 1)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("32bit", 2);
            else if (AudioBitDepth.SelectedIndex == 2)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("32bit", 3);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("32bit", 1);
        }

        private void MonophonicFunc_CheckedChanged(object sender, EventArgs e)
        {
            if (MonophonicFunc.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("monorendering", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("monorendering", "0", RegistryValueKind.DWord);
        }

        private void FadeoutDisable_CheckedChanged(object sender, EventArgs e)
        {
            if (FadeoutDisable.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("fadeoutdisable", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("fadeoutdisable", "0", RegistryValueKind.DWord);
        }

        private void SlowDownPlayback_CheckedChanged(object sender, EventArgs e)
        {
            if (SlowDownPlayback.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("vms2emu", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("vms2emu", "0", RegistryValueKind.DWord);
        }

        private void KSDAPIBox_CheckedChanged(object sender, EventArgs e)
        {
            if (KSDAPIBox.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("allowksdapi", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("allowksdapi", "0", RegistryValueKind.DWord);
        }

        private void HMode_CheckedChanged(object sender, EventArgs e)
        {
            if (HMode.Checked)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("hypermode", "1", RegistryValueKind.DWord);
            else
                KeppySynthConfiguratorMain.SynthSettings.SetValue("hypermode", "0", RegistryValueKind.DWord);
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
            if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "DirectSound")
            {
                KeppySynthDefaultOutput frm = new KeppySynthDefaultOutput(false);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "WASAPI")
            {
                KeppySynthDefaultOutput frm = new KeppySynthDefaultOutput(true);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "ASIO")
            {
                DefaultASIOAudioOutput frm = new DefaultASIOAudioOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
        }

        private void KSDAPIBoxWhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you uncheck this option, some apps might be forced to fallback to the stock Windows Multimedia API, which increases latency." +
                "\nKeep in mind that not all KSDAPI-ready apps do check for this value, and they might use it whether you want them to or not." +
                "\n\n(This value will not affect the Windows Multimedia Wrapper.)", 
                "Keppy's Synthesizer - Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HModeWhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicking this checkbox will remove all the checks done to the events, for example transposing and other settings in the configurator.\n" +
                "The events will be sent straight to the buffer, and played immediately.\n\n" +
                "The \"Slow down playback instead of skipping notes\" checkbox will not work, while this mode is enabled, along with other event processing-related functions.",
                "Keppy's Synthesizer - Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChangePitchShifting_Click(object sender, EventArgs e)
        {
            KeppySynthPitchShifting frm = new KeppySynthPitchShifting();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
