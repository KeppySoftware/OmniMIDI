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
using Un4seen.BassAsio;

namespace OmniMIDIConfigurator
{
    public partial class DefaultASIOAudioOutput : Form
    {
        public DefaultASIOAudioOutput()
        {
            InitializeComponent();

            try
            {
                if (GetASIODevicesCount() < 1)
                {
                    Program.ShowError(4, "Error", "No ASIO devices installed!\n\nClick OK to close this window.", null);
                    Close();
                }

                BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();

                // Populate devices list
                for (int n = 0; BassAsio.BASS_ASIO_GetDeviceInfo(n, info); n++)
                    DevicesList.Items.Add(info.ToString());

                // Load previous device from registry
                String PreviousDevice = (String)Program.SynthSettings.GetValue("ASIOOutput", "None");
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

                BassAsio.BASS_ASIO_Init(DevicesList.SelectedIndex, BASSASIOInit.BASS_ASIO_THREAD | BASSASIOInit.BASS_ASIO_JOINORDER);
                GetASIODeviceInfo();

                DeviceTrigger(true);
                DevicesList.SelectedIndexChanged += new EventHandler(DevicesList_SelectedIndexChanged);

                /* 
                 * Unavailable at the moment
                 * 
                 * if (Convert.ToInt32(OmniMIDIConfiguratorMain.Program.SynthSettings.GetValue("ASIOSeparateThread", 0)) == 1)
                 * ASIOSeparateThread.Checked = true;
                 *
                 */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.Message.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        public static int GetASIODevicesCount()
        {
            int numbdev;
            BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();
            for (numbdev = 0; BassAsio.BASS_ASIO_GetDeviceInfo(numbdev, info); numbdev++);
            return numbdev;
        }

        private string GetASIOGranularity(Int32 Value)
        {
            switch (Value)
            {
                case -1:
                    return "Power of 2";
                case 0:
                    return "None";
                case 1:
                    return "1 sample";
                default:
                    return String.Format("{0} samples", Value);
            }
        }

        private void GetASIODeviceInfo()
        {
            try
            {
                BASS_ASIO_INFO dInfo = BassAsio.BASS_ASIO_GetInfo();
                BASS_ASIO_DEVICEINFO fInfo = new BASS_ASIO_DEVICEINFO();

                BassAsio.BASS_ASIO_GetDeviceInfo(DevicesList.SelectedIndex, fInfo);

                DeviceName.Text =
                    String.Format("{0} (Driver library: {1})", dInfo.name, Path.GetFileName(fInfo.driver).ToUpperInvariant());
                Inputs.Text =
                    String.Format("{0} input channels available.", dInfo.inputs);
                Outputs.Text =
                    String.Format("{0} output channels available.", dInfo.outputs);
                BufferInfo.Text =
                    String.Format("Min/Max size {0}/{1} samples, set to {2} samples (Granularity value: {3})",
                                  dInfo.bufmin, dInfo.bufmax, dInfo.bufpref, GetASIOGranularity(dInfo.bufgran));
            }
            catch { SetDummyASIOInfo(); }
        }

        private void SetDummyASIOInfo()
        {
            DeviceName.Text = String.Format("The device refused to be interrogated by BASSASIO. ({0})", BassAsio.BASS_ASIO_ErrorGetCode().ToString());
            Inputs.Text = "N/A";
            Outputs.Text = "N/A";
            BufferInfo.Text = "N/A";
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceCP.Enabled = true;

            BassAsio.BASS_ASIO_Free();
            BassAsio.BASS_ASIO_Init(DevicesList.SelectedIndex, BASSASIOInit.BASS_ASIO_THREAD | BASSASIOInit.BASS_ASIO_JOINORDER);
            GetASIODeviceInfo();

            DeviceTrigger(false);
        }

        private void ASIOSeparateThread_CheckedChanged(object sender, EventArgs e)
        {
            // Unavailable at the moment
            // OmniMIDIConfiguratorMain.Program.SynthSettings.SetValue("ASIOSeparateThread", ASIOSeparateThread.Checked ? "1" : "0", RegistryValueKind.DWord);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            BassAsio.BASS_ASIO_Free();
            Functions.SetDefaultDevice(AudioEngine.ASIO_ENGINE, 0, DevicesList.GetItemText(DevicesList.SelectedItem));
            Close();
            Dispose();
        }

        private void DeviceCP_Click(object sender, EventArgs e)
        {
            Boolean StatusCP = BassAsio.BASS_ASIO_ControlPanel();
            if (!StatusCP && BassAsio.BASS_ASIO_ErrorGetCode() != BASSError.BASS_OK)
            {
                MessageBox.Show(
                    String.Format(
                        "An error has occured while showing the control panel for this device.\nThe button will be disabled.\n\nError: {0}",
                        BassAsio.BASS_ASIO_ErrorGetCode()),
                    "ASIO control panel - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DeviceCP.Enabled = false;
                return;
            }
            GetASIODeviceInfo();
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

        private void LatencyWarning_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Changing the buffer size/update rate of your ASIO device will affect the MIDI timings of your host app!\n" +
                "That means that the MIDI events might be delayed before the sound is actually played on your speakers.\n\n" +
                "Always keep the buffer size to the smallest size possible, and the update rate to the highest value your sound card can handle with no dropouts, " +
                "if you want to avoid this issue.", "OmniMIDI Configurator ~ ASIO warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
