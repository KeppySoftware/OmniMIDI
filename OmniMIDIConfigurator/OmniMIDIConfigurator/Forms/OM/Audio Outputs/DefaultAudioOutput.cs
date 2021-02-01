using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// For device info
using Un4seen.Bass;

namespace OmniMIDIConfigurator
{
    public partial class DefaultAudioOutput : Form
    {
        bool IsIt = false;

        public DefaultAudioOutput(bool IsItWasapi)
        {
            InitializeComponent();
            IsIt = IsItWasapi;
        }

        private void DefaultOutput_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsIt)
                {
                    Text = String.Format(Text, "WASAPI");
                    UseNewWASAPI.Visible = true;
                    ReduceBootUpDelay.Enabled = false;
                }
                else Text = String.Format(Text, "DirectSound");

                int selecteddeviceprev = (int)Program.SynthSettings.GetValue("AudioOutput", 0);
                SwitchDefaultAudio.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FollowDefaultAudioDevice", 0));
                ReduceBootUpDelay.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("ReduceBootUpDelay", 0));

                BASS_DEVICEINFO info = new BASS_DEVICEINFO();
                DevicesList.Items.Add("Default Windows audio output");
                Bass.BASS_GetDeviceInfo(selecteddeviceprev - 1, info);
                Bass.BASS_GetDeviceInfo(-1, info);

                for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
                    DevicesList.Items.Add(info.ToString());

                try { DevicesList.SelectedIndex = selecteddeviceprev; }
                catch { DevicesList.SelectedIndex = 0; }

                DevicesList.SelectedIndexChanged += new System.EventHandler(this.DevicesList_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.Message.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice(AudioEngine.DSOUND_OR_WASAPI, DevicesList.SelectedIndex, null);
        }

        private void SwitchDefaultAudio_CheckedChanged(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("FollowDefaultAudioDevice", SwitchDefaultAudio.Checked, Microsoft.Win32.RegistryValueKind.DWord);
        }

        private void ReduceBootUpDelay_CheckedChanged(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("ReduceBootUpDelay", ReduceBootUpDelay.Checked, Microsoft.Win32.RegistryValueKind.DWord);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void UseNewWASAPI_Click(object sender, EventArgs e)
        {
            DialogResult RES = Program.ShowError(1, "Use new WASAPI", "Are you sure you want to switch to the new WASAPI engine?\n\nYou can switch back anytime.", null);

            if (RES == DialogResult.Yes)
            {
                Program.SynthSettings.SetValue("OldWASAPIMode", "0", Microsoft.Win32.RegistryValueKind.DWord);
                DialogResult = RES;
                Close();
            }
        }
    }
}
