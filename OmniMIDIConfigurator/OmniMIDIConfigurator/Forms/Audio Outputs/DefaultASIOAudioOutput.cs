using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
// For device info
using Un4seen.Bass;
using Un4seen.BassAsio;

namespace OmniMIDIConfigurator
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
                    Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Asterisk, "Error", "No ASIO devices installed!\n\nClick OK to close this window.", false, null);
                    Close();
                    Dispose();
                }

                BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();

                // Populate devices list
                for (int n = 0; BassAsio.BASS_ASIO_GetDeviceInfo(n, info); n++)
                    DevicesList.Items.Add(info.ToString());
                
                // Load previous device from registry
                String PreviousDevice = (String)OmniMIDIConfiguratorMain.SynthSettings.GetValue("ASIOOutput", "None");
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
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.Message.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice(AudioEngine.ASIO_ENGINE, 0, DevicesList.GetItemText(DevicesList.SelectedItem));
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
            Process.Start("https://github.com/KeppySoftware/OmniMIDI#asio-support-details");
        }

        private String[] DownloadDatabase(String Link)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var result = client.DownloadData(Link);
                    string str = Encoding.UTF8.GetString(result);
                    return str.Split(new[] { '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch
            {
                return new[] { "NULL123" };
            }
        }

        private int CompareToDatabase(String DeviceToCompare)
        {
            foreach (String Device in DownloadDatabase(TestedASIODevices.Supported))
            {
                String[] Content = Device.Split('|');
                if (DeviceToCompare.Contains(Content[0])) return SupportedDevice(Content[1]);
            }

            foreach (String Device in DownloadDatabase(TestedASIODevices.Unstable))
            {
                String[] Content = Device.Split('|');
                if (DeviceToCompare.Contains(Content[0])) return UnstableDevice(Content[1]);
            }

            foreach (String Device in DownloadDatabase(TestedASIODevices.Unsupported))
            {
                String[] Content = Device.Split('|');
                if (DeviceToCompare.Contains(Content[0])) return UnsupportedDevice(Content[1]);
            }

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

        private int SupportedDevice(String Info)
        {
            Status.Font = new Font(Status.Font, FontStyle.Regular);
            Status.ForeColor = Color.DarkOliveGreen;
            Status.Text = String.Format("Supported device. {0}", Info);
            return DeviceStatus.DEVICE_SUPPORTED;
        }

        private int UnstableDevice(String Info)
        {
            Status.Font = new Font(Status.Font, FontStyle.Bold);
            Status.ForeColor = Color.DarkOrange;
            Status.Text = String.Format("Supported device, might be unstable. {0}", Info);
            return DeviceStatus.DEVICE_UNSTABLE;
        }

        private int UnsupportedDevice(String Info)
        {
            Status.Font = new Font(Status.Font, FontStyle.Bold);
            Status.ForeColor = Color.Red;
            Status.Text = String.Format("Unsupported device. {0}", Info);
            return DeviceStatus.DEVICE_UNSUPPORTED;
        }

        private int UnknownDevice()
        {
            Status.Font = new Font(Status.Font, FontStyle.Bold);
            Status.ForeColor = Color.DarkGray;
            Status.Text = "Unknown device. Either it's missing from the database, or there's no Internet.";
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
        public static string Supported = "https://raw.githubusercontent.com/KeppySoftware/OmniMIDI/master/OmniMIDIASIOSupportList/Supported.txt";
        public static string Unstable = "https://raw.githubusercontent.com/KeppySoftware/OmniMIDI/master/OmniMIDIASIOSupportList/Unstable.txt";
        public static string Unsupported = "https://raw.githubusercontent.com/KeppySoftware/OmniMIDI/master/OmniMIDIASIOSupportList/Unsupported.txt";
    }
}
