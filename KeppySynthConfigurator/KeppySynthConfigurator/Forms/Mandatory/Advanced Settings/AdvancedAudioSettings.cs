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

            if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0)
            {
                OldBuff.Enabled = false;
                NoSleep.Enabled = false;
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 1)
            {
                OldBuff.Enabled = true;
                NoSleep.Enabled = true;
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 2)
            {
                OldBuff.Enabled = false;
                NoSleep.Enabled = false;
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 3)
            {
                AudioBitDepth.Enabled = false;
                AudioBitDepthLabel.Enabled = false;
                OldBuff.Enabled = false;
                NoSleep.Enabled = false;
            }
            else
            {
                ChangeDefaultOutput.Enabled = false;
            }

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
            if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "XAudio2")
            {
                MessageBox.Show("XAudio2 automatically switches between devices, when you change the default one through the \"Audio device\" applet in Windows.", "Keppy's Synthesizer - Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "DirectSound")
            {
                KeppySynthDefaultOutput frm = new KeppySynthDefaultOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "ASIO")
            {
                DefaultASIOAudioOutput frm = new DefaultASIOAudioOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "WASAPI")
            {
                DefaultWASAPIAudioOutput frm = new DefaultWASAPIAudioOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
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
