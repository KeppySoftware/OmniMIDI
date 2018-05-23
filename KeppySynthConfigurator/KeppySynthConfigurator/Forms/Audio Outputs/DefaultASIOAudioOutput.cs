using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// For device info
using Un4seen.Bass;
using Un4seen.BassAsio;

namespace KeppySynthConfigurator
{
    public partial class DefaultASIOAudioOutput : Form
    {
        public DefaultASIOAudioOutput()
        {
            InitializeComponent();
        }

        public static int GetASIODevicesCount()
        {
            int numbdev;
            BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();
            for (numbdev = 0; BassAsio.BASS_ASIO_GetDeviceInfo(numbdev, info); numbdev++);
            return numbdev;
        }

        private void DefaultASIOAudioOutput_Load(object sender, EventArgs e)
        {
            try
            {
                if (GetASIODevicesCount() < 1)
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Asterisk, "Error", "No ASIO devices installed!\n\nClick OK to close this window.", false, null);
                    Close();
                    Dispose();
                }

                BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();

                // Populate devices list
                for (int n = 0; BassAsio.BASS_ASIO_GetDeviceInfo(n, info); n++)
                    DevicesList.Items.Add(info.ToString());
                
                // Load previous device from registry
                String PreviousDevice = (String)KeppySynthConfiguratorMain.SynthSettings.GetValue("ASIOOutput", "None");
                DefOut.Text = String.Format("Def. ASIO output: {0}", PreviousDevice);

                DevicesList.SelectedIndex = 0;
                for (int n = 0; n < DevicesList.Items.Count; n++)
                {
                    if (DevicesList.Items[n].Equals(PreviousDevice))
                    {
                        DevicesList.SelectedIndex = n;
                        break;
                    }
                }

                MaxThreads.Text = String.Format("ASIO is allowed to use a maximum of {0} threads.", Environment.ProcessorCount);
                BassAsio.BASS_ASIO_Init(DevicesList.SelectedIndex, 0);

                DeviceTrigger(true);
                DevicesList.SelectedIndexChanged += new EventHandler(DevicesList_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.Message.ToString(), "Oh no! Keppy's Synthesizer encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice(1, 0, DevicesList.GetItemText(DevicesList.SelectedItem));
            BassAsio.BASS_ASIO_Free();
            BassAsio.BASS_ASIO_Init(DevicesList.SelectedIndex, 0);
            DeviceTrigger(false);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            BassAsio.BASS_ASIO_Free();
            Close();
            Dispose();
        }

        private void DeviceCP_Click(object sender, EventArgs e)
        {
            BassAsio.BASS_ASIO_ControlPanel();
        }

        private void ASIODevicesSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/KaleidonKep99/Keppy-s-Synthesizer#asio-support-details");
        }

        private int CompareToDatabase(String DeviceToCompare)
        {
            foreach (String Device in TestedASIODevices.Supported)
                if (DeviceToCompare.Contains(Device)) return SupportedDevice();

            foreach (String Device in TestedASIODevices.Unstable)
                if (DeviceToCompare.Contains(Device)) return UnstableDevice();

            foreach (String Device in TestedASIODevices.Unsupported)
                if (DeviceToCompare.Contains(Device)) return UnsupportedDevice();

            return UnknownDevice();
        }

        private void DeviceTrigger(Boolean startup)
        {
            int Trigger = CompareToDatabase(DevicesList.Text);

            if (!startup)
            {
                if (Trigger == DeviceStatus.DEVICE_UNSTABLE) MessageBox.Show("This device might crash the app, while in use.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (Trigger == DeviceStatus.DEVICE_UNSUPPORTED) MessageBox.Show("This device is unsupported.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int SupportedDevice()
        {
            Status.Font = new Font(Status.Font, FontStyle.Regular);
            Status.ForeColor = Color.DarkOliveGreen;
            Status.Text = "Supported device";
            return DeviceStatus.DEVICE_SUPPORTED;
        }

        private int UnstableDevice()
        {
            Status.Font = new Font(Status.Font, FontStyle.Bold);
            Status.ForeColor = Color.DarkOrange;
            Status.Text = "Supported device, might be unstable";
            return DeviceStatus.DEVICE_UNSTABLE;
        }

        private int UnsupportedDevice()
        {
            Status.Font = new Font(Status.Font, FontStyle.Bold);
            Status.ForeColor = Color.Red;
            Status.Text = "Unsupported device";
            return DeviceStatus.DEVICE_UNSUPPORTED;
        }

        private int UnknownDevice()
        {
            Status.Font = new Font(Status.Font, FontStyle.Bold);
            Status.ForeColor = Color.DarkGray;
            Status.Text = "Unknown device";
            return DeviceStatus.DEVICE_UNKNOWN;
        }
    }

    static class DeviceStatus
    {
        public const int DEVICE_SUPPORTED = 0;
        public const int DEVICE_UNSTABLE = 1;
        public const int DEVICE_UNSUPPORTED = 2;
        public const int DEVICE_UNKNOWN = 3;
    }

    static class TestedASIODevices
    {
        public static string[] Supported =
        {
            "ASUS Xonar D2 ASIO",
            "BEHRINGER USB AUDIO",
            "FL Studio ASIO",
            "Focusrite USB ASIO",
            "Jack",
            "JackRouter",
            "Native Instruments Komplete Audio 6",
            "ReaRoute ASIO",
            "USB Audio ASIO Driver",
            "USBPre 2.0 ASIO",
            "ZOOM R8 ASIO Driver"
        };

        public static string[] Unstable =
        {
            "AKIYAMA ASIO",
            "ASIO4ALL",
            "FlexASIO"
        };

        public static string[] Unsupported = 
        {
            "ASIO ADSP24(WDM)",
            "ASIO2WASAPI",
            "Realtek ASIO",
            "Voicemeeter"
        };
    }
}
