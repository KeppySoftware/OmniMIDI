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
        List<AudioDevice> Devs = new List<AudioDevice>();

        public DefaultAudioOutput(bool IsItWasapi)
        {
            InitializeComponent();

            try
            {
                BASS_DEVICEINFO info = new BASS_DEVICEINFO();
                string SelectedDevice = null;

                try
                {
                    // Parse previous selected device
                    SelectedDevice = (String)Program.SynthSettings.GetValue("AudioOutput", "default");
                }
                catch
                {
                    // The old setting isn't a string???
                    // Overwrite old setting with the correct type of registry value
                    Program.SynthSettings.SetValue("AudioOutput", "default", Microsoft.Win32.RegistryValueKind.String);
                    SelectedDevice = "default";
                }

                if (IsItWasapi)
                {
                    Text = String.Format(Text, "WASAPI");
                    UseNewWASAPI.Visible = true;
                    ReduceBootUpDelay.Enabled = false;
                }
                else Text = String.Format(Text, "DirectSound");

                SwitchDefaultAudio.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FollowDefaultAudioDevice", 0));
                ReduceBootUpDelay.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("ReduceBootUpDelay", 0));

                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_DEV_DEFAULT, 0);

                // Add default output
                Devs.Add(new AudioDevice() { Name = "Default Windows audio output", ID = "default" });

                // Scan for output devices
                for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
                {
                    AudioDevice TmpDev = new AudioDevice();

                    // Store device name and internal ID
                    TmpDev.Name = info.name;

                    // if n is 0, that means the device is "nosound"
                    TmpDev.ID = (n != 0) ? info.driver : "nosound";

                    // Store it in the devices list
                    Devs.Add(TmpDev);
                }

                // Set the combobox to display items from the devices list,
                // and make it only display their names
                DevicesList.DataSource = Devs;
                DevicesList.DisplayMember = "Name";

                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_DEV_DEFAULT, 1);

                // Load previous device from registry
                String PreviousDevice = (String)Program.SynthSettings.GetValue("AudioOutput", "default");
                for (int n = 0; n < DevicesList.Items.Count; n++)
                {
                    AudioDevice TmpDev = (AudioDevice)DevicesList.Items[n];
                    if (TmpDev.ID.Equals(PreviousDevice))
                    {
                        DevicesList.SelectedIndex = n;
                        break;
                    }

                    // No device found, set to 0
                    if (n == (DevicesList.Items.Count - 1))
                        DevicesList.SelectedIndex = 0;
                }

                DevicesList.SelectedIndexChanged += new System.EventHandler(this.DevicesList_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice(AudioEngine.DSOUND_OR_WASAPI, ((AudioDevice)DevicesList.Items[DevicesList.SelectedIndex]).ID);
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

    class AudioDevice
    {
        public string Name { get; set; }
        public string ID { get; set; }
    }
}
