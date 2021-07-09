using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
// For device info
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace OmniMIDIConfigurator
{
    public partial class DefaultWASAPIAudioOutput : Form
    {
        List<WASAPIDevice> Devices = new List<WASAPIDevice>();

        private bool IsWindows81OrNewer()
        {
            if (Environment.OSVersion.Version.Major < 10)
            {
                if (Environment.OSVersion.Version.Major == 6)
                {
                    if (Environment.OSVersion.Version.Minor < 3)
                        return false;
                    else
                        return true;
                }
                else return false;
            }
            else return true;
        }

        public DefaultWASAPIAudioOutput()
        {
            try
            {
                if (GetWASAPIDevicesCount() < 1)
                {
                    Program.ShowError(4, "Error", "No WASAPI devices detected.\n\nClick OK to close this window.", null);
                    Close();
                }

                InitializeComponent();

                BASS_WASAPI_DEVICEINFO info = new BASS_WASAPI_DEVICEINFO();

                // Populate devices list
                Devices.Add(new WASAPIDevice { Name = "Default Windows output", ID = -1, RealID = "{0.0.0.00000000}.{00000000-0000-0000-0000-000000000000}" });
                for (int n = 0; BassWasapi.BASS_WASAPI_GetDeviceInfo(n, info); n++)
                {
                    if (info.flags.HasFlag(BASSWASAPIDeviceInfo.BASS_DEVICE_ENABLED) &&
                        !info.flags.HasFlag(BASSWASAPIDeviceInfo.BASS_DEVICE_LOOPBACK) &&
                        !info.flags.HasFlag(BASSWASAPIDeviceInfo.BASS_DEVICE_INPUT))
                    {
                        if (info.flags.HasFlag(BASSWASAPIDeviceInfo.BASS_DEVICE_DEFAULT))
                            Devices[0].ID = n;

                        Devices.Add(new WASAPIDevice { Name = info.name, ID = n, RealID = info.id });
                    }
                }

                DevicesList.DataSource = Devices;
                DevicesList.DisplayMember = "Name";

                DevicesList.SelectedIndex = 0;

                ExclusiveMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("WASAPIExclusive", 0));
                WASAPIRawMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("WASAPIRawMode", 0));
                NoDoubleBuffering.Checked = !Convert.ToBoolean(Program.SynthSettings.GetValue("WASAPIDoubleBuf", 1));
                AsyncMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("WASAPIAsyncMode", 0));

                if (!IsWindows81OrNewer())
                {
                    WASAPIRawMode.Checked = false;
                    WASAPIRawMode.Enabled = false;

                    Program.SynthSettings.SetValue("WASAPIRAWMode", "0", RegistryValueKind.DWord);
                }

                // Load previous device from registry
                String PreviousDevice = (String)Program.SynthSettings.GetValue("WASAPIOutput", "{0.0.0.00000000}.{00000000-0000-0000-0000-000000000000}");
                for (int n = 0; n < DevicesList.Items.Count; n++)
                {
                    WASAPIDevice TmpDev = (WASAPIDevice)DevicesList.Items[n];
                    if (TmpDev.RealID.Equals(PreviousDevice))
                    {
                        DevicesList.SelectedIndex = n;
                        break;
                    }
                }

                GetWASAPIDeviceInfo();

                DevicesList.SelectedIndexChanged += new EventHandler(DevicesList_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        public static int GetWASAPIDevicesCount()
        {
            int AvailableDevices = 0;
            int numbdev = 0;
            BASS_WASAPI_DEVICEINFO info = new BASS_WASAPI_DEVICEINFO();

            for (numbdev = 0; BassWasapi.BASS_WASAPI_GetDeviceInfo(numbdev, info); numbdev++)
            {
                if (info.flags.HasFlag(BASSWASAPIDeviceInfo.BASS_DEVICE_ENABLED) &&
                    !info.flags.HasFlag(BASSWASAPIDeviceInfo.BASS_DEVICE_LOOPBACK) &&
                    !info.flags.HasFlag(BASSWASAPIDeviceInfo.BASS_DEVICE_INPUT))
                    AvailableDevices++;
            }

            return AvailableDevices;
        }

        private void GetWASAPIDeviceInfo()
        {
            try
            {
                BASS_WASAPI_DEVICEINFO fInfo = new BASS_WASAPI_DEVICEINFO();

                BassWasapi.BASS_WASAPI_GetDeviceInfo(((WASAPIDevice)DevicesList.SelectedItem).ID, fInfo);

                DeviceName.Text =
                    String.Format("{0}", GetTypeOfWASAPIDevice(fInfo.type));
                Freq.Text =
                    String.Format("{0}Hz in shared-mode", fInfo.mixfreq);
                Channels.Text =
                    String.Format("{0} channels in shared-mode", fInfo.mixchans);
                BufferInfo.Text =
                    String.Format("Minimum update period of {0} seconds, default period is {0} seconds",
                                  fInfo.minperiod, fInfo.defperiod);
                ID.Text = fInfo.id;
            }
            catch { GetDummyWASAPIInfo(); }
        }

        private void GetDummyWASAPIInfo()
        {
            DeviceName.Text = String.Format("The device refused to be interrogated by BASSWASAPI. ({0})", Bass.BASS_ErrorGetCode().ToString());
            Freq.Text = "N/A";
            Channels.Text = "N/A";
            BufferInfo.Text = "N/A";
        }

        private string GetTypeOfWASAPIDevice(BASSWASAPIDeviceType Type)
        {
            switch (Type)
            {
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_DIGITAL:
                    return "Virtual audio hardware";
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_HANDSET:
                    return "Handset/phone hardware";
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_HDMI:
                    return "Video device through HDMI";
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_HEADPHONES:
                    return "Earphones/Headphones";
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_HEADSET:
                    return "Wireless headset or speaker";
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_LINELEVEL:
                    return "Line level device";
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_SPDIF:
                    return "Sony/Philips Digital Interface (RCA or TOSLINK/EIAJ)";
                case BASSWASAPIDeviceType.BASS_WASAPI_TYPE_SPEAKERS:
                    return "Speakers/Monitors";
                default:
                    return "Unknown";
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetWASAPIDeviceInfo();
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("WASAPIExclusive", ExclusiveMode.Checked, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("WASAPIDoubleBuf", !NoDoubleBuffering.Checked, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("WASAPIRAWMode", WASAPIRawMode.Checked, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("WASAPIAsyncMode", AsyncMode.Checked, RegistryValueKind.DWord);

            BassWasapi.BASS_WASAPI_Free();
            Functions.SetDefaultDevice(AudioEngine.WASAPI_ENGINE, ((WASAPIDevice)DevicesList.SelectedItem).RealID);
            Close();
            Dispose();
        }

        private void OldWASAPIMode_Click(object sender, EventArgs e)
        {
            DialogResult RES = Program.ShowError(1, "Use old WASAPI", "Are you sure you want to switch to the old WASAPI engine?\n\nYou can switch back anytime.", null);

            if (RES == DialogResult.Yes)
            {
                Program.SynthSettings.SetValue("OldWASAPIMode", "1", Microsoft.Win32.RegistryValueKind.DWord);
                DialogResult = RES;
                Close();
            }
        }
    }

    class WASAPIDevice
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string RealID { get; set; }
    }
}
