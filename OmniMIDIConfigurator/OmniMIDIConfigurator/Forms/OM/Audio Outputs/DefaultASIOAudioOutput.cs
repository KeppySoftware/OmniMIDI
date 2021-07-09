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
using System.Threading.Tasks;
using System.Windows.Forms;
// For device info
using Un4seen.Bass;
using Un4seen.BassAsio;

namespace OmniMIDIConfigurator
{
    public partial class DefaultASIOAudioOutput : Form
    {
        List<ASIODevice> Devices = new List<ASIODevice>();

        public DefaultASIOAudioOutput()
        {
            InitializeComponent();
        }

        private void AnalyzeDevice(ref BASS_ASIO_DEVICEINFO dInfo, int n)
        {
   
        }

        private void DefaultASIOAudioOutput_Shown(object sender, EventArgs e)
        {
            try
            {
                ASIODirectFeed.Checked = Convert.ToInt32(Program.SynthSettings.GetValue("ASIODirectFeed", 0)) == 1;

                this.Text = "Probing ASIO devices, please wait...";
                this.Enabled = false;

                if (GetASIODevicesCount() < 1)
                {
                    Program.ShowError(4, "Error", "No ASIO devices installed!\n\nClick OK to close this window.", null);
                    Close();
                }

                BASS_ASIO_DEVICEINFO dInfo = new BASS_ASIO_DEVICEINFO();

                // Populate devices list
                for (int n = 0; BassAsio.BASS_ASIO_GetDeviceInfo(n, dInfo); n++)
                {
                    Task ASIOT = Task.Run(() =>
                    {
                        try
                        {
                            BassAsio.BASS_ASIO_Init(n, BASSASIOInit.BASS_ASIO_THREAD);

                            BASS_ASIO_INFO fInfo = BassAsio.BASS_ASIO_GetInfo();

                            ASIODevice NewDev = new ASIODevice();

                            NewDev.Name = dInfo.name;
                            NewDev.Driver = dInfo.driver;
                            NewDev.ID = n;

                            if (fInfo != null)
                            {
                                NewDev.ChannelsL = new List<BASS_ASIO_CHANNELINFO>();

                                NewDev.Status = GetDeviceDescription(NewDev.Name);
                                NewDev.Inputs = fInfo.inputs;
                                NewDev.Outputs = fInfo.outputs;
                                NewDev.BufMin = fInfo.bufmin;
                                NewDev.BufMax = fInfo.bufmax;
                                NewDev.BufPref = fInfo.bufmax;
                                NewDev.BufGran = fInfo.bufgran;

                                for (int n2 = 0; n2 < fInfo.outputs; n2++)
                                {
                                    BASS_ASIO_CHANNELINFO cInfo = null;
                                    
                                    cInfo = BassAsio.BASS_ASIO_ChannelGetInfo(false, n2);

                                    if (cInfo != null)
                                        NewDev.ChannelsL.Add(cInfo);
                                }

                                NewDev.ChannelsR = new List<BASS_ASIO_CHANNELINFO>(NewDev.ChannelsL);
                            }
                            else
                            {
                                NewDev.Status = new ASIOStatus { Description = BassAsio.BASS_ASIO_ErrorGetCode().ToString(), Flag = DevStatusEnum.DEVICE_UNKNOWN };
                                NewDev.NoData = true;
                            }

                            Devices.Add(NewDev);

                            BassAsio.BASS_ASIO_Free();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    });

                    if (!ASIOT.Wait(3000))
                    {
                        // This should not happen.
                        ASIODevice NewDev = new ASIODevice();
                        NewDev.Name = "UNKNOWN";
                        NewDev.Driver = "UNKNOWN_DRV";
                        NewDev.ID = n;
                        NewDev.Inputs = 0;
                        NewDev.Outputs = 0;
                        NewDev.BufMin = 0;
                        NewDev.BufMax = 0;
                        NewDev.BufPref = 0;
                        NewDev.BufGran = 0;
                        NewDev.Status = new ASIOStatus { Description = "DRIVER_TIMEOUT", Flag = DevStatusEnum.DEVICE_UNKNOWN };
                        NewDev.NoData = true;
                        Devices.Add(NewDev);
                    }
                }

                DevicesList.DataSource = Devices;
                DevicesList.DisplayMember = "Name";

                // Load previous device from registry
                String PreviousDevice = (String)Program.SynthSettings.GetValue("ASIOOutput", "None");
                for (int n = 0; n < DevicesList.Items.Count; n++)
                {
                    ASIODevice TmpDev = (ASIODevice)DevicesList.Items[n];
                    if (TmpDev.Name.Equals(PreviousDevice))
                    {
                        DevicesList.SelectedIndex = n;
                        DefOut.Text = String.Format("Default ASIO output: {0}", PreviousDevice);
                        break;
                    }
                }

                MaxThreads.Text = String.Format("ASIO is allowed to use a maximum of {0} threads.", Environment.ProcessorCount);

                DevicesList.SelectedIndexChanged += new EventHandler(DevicesList_SelectedIndexChanged);

                GetASIODeviceInfo();

                this.Text = "Change default ASIO output";
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private ASIOStatus GetDeviceDescription(String DeviceToCompare)
        {
            DevStatusEnum Status = DevStatusEnum.DEVICE_UNKNOWN;
            String Desc = String.Empty;

            foreach (String Device in DownloadDatabase(TestedASIODevices.Supported))
            {
                String[] Content = Device.Split('|');
                if (DeviceToCompare.Contains(Content[0]))
                {
                    Status = DevStatusEnum.DEVICE_SUPPORTED;
                    Desc = Content[1];
                }
            }

            foreach (String Device in DownloadDatabase(TestedASIODevices.Unstable))
            {
                String[] Content = Device.Split('|');
                if (DeviceToCompare.Contains(Content[0]))
                {
                    Status = DevStatusEnum.DEVICE_UNSTABLE;
                    Desc = Content[1];
                }
            }

            foreach (String Device in DownloadDatabase(TestedASIODevices.Unsupported))
            {
                String[] Content = Device.Split('|');
                if (DeviceToCompare.Contains(Content[0]))
                {
                    Status = DevStatusEnum.DEVICE_UNSUPPORTED;
                    Desc = Content[1];
                }
            }

            return new ASIOStatus { Flag = Status, Description = Desc };
        }

        private void GetASIODeviceInfo()
        {
            ASIODevice TmpDev = (ASIODevice)DevicesList.SelectedItem;

            // Set the name of the driver, before the check
            DeviceName.Text =
                String.Format("{0} (Driver library: {1})", TmpDev.Name, Path.GetFileName(TmpDev.Driver).ToUpperInvariant());

            if (TmpDev.NoData)
            {
                // Some of the info were missing, the device didn't want to be interrogated
                Inputs.Text = "N/A";
                Outputs.Text = "N/A";
                BufferInfo.Text = "N/A";

                Status.ForeColor = Color.DarkGray;
                Status.Font = new Font(Status.Font, FontStyle.Bold);
                Status.Text = String.Format("BASSASIO failed to probe the driver, and returned {0}.", TmpDev.Status.Description);

                LeftCh.DataSource = null;
                RightCh.DataSource = null;
                LeftCh.Enabled = false;
                RightCh.Enabled = false;
            }
            else
            {
                // Get all the info
                Inputs.Text =
                    String.Format("{0} input channels available.", TmpDev.Inputs);
                Outputs.Text =
                    String.Format("{0} output channels available.", TmpDev.Outputs);
                BufferInfo.Text =
                    String.Format("Min/Max size {0}/{1} samples, set to {2} samples (Granularity value: {3})",
                                  TmpDev.BufMin, TmpDev.BufMax, TmpDev.BufPref, GetASIOGranularity(TmpDev.BufGran));

                // Set the color
                switch (TmpDev.Status.Flag)
                {
                    case DevStatusEnum.DEVICE_SUPPORTED:
                        Status.ForeColor = Color.DarkOliveGreen;
                        break;
                    case DevStatusEnum.DEVICE_UNSTABLE:
                        Status.ForeColor = Color.DarkOrange;
                        break;
                    case DevStatusEnum.DEVICE_UNSUPPORTED:
                        Status.ForeColor = Color.Red;
                        break;
                    default:
                        Status.ForeColor = Color.DarkGray;
                        break;
                }

                Status.Font = new Font(Status.Font, (TmpDev.Status.Flag == DevStatusEnum.DEVICE_SUPPORTED) ? FontStyle.Regular : FontStyle.Bold);
                Status.Text = TmpDev.Status.Description;

                LeftCh.DataSource = TmpDev.ChannelsL;
                RightCh.DataSource = TmpDev.ChannelsR;
                LeftCh.Enabled = /* true */ false;
                RightCh.Enabled = /* true */ false;
            }

            Invalidate();
            Refresh();
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceCP.Enabled = true;
            GetASIODeviceInfo();
        }

        private void ASIODirectFeed_CheckedChanged(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("ASIODirectFeed", Convert.ToInt32(ASIODirectFeed.Checked), RegistryValueKind.DWord);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice(AudioEngine.ASIO_ENGINE, ((ASIODevice)DevicesList.SelectedItem).Name);
        }

        private void DeviceCP_Click(object sender, EventArgs e)
        {
            BASS_ASIO_INFO fInfo;
            ASIODevice Dev = (ASIODevice)DevicesList.SelectedItem;

            BassAsio.BASS_ASIO_Init(Dev.ID, BASSASIOInit.BASS_ASIO_DEFAULT);

            Boolean StatusCP = BassAsio.BASS_ASIO_ControlPanel();
            BASSError Err = BassAsio.BASS_ASIO_ErrorGetCode();

            if (!StatusCP && Err == BASSError.BASS_ERROR_INIT)
            {
                MessageBox.Show(
                    String.Format("The ASIO driver was unable to start.\nError: {0}\n\nPress OK to continue.", Err.ToString()),
                    "ASIO control panel - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DeviceCP.Enabled = false;
                return;
            }

            fInfo = BassAsio.BASS_ASIO_GetInfo();
            Dev.Inputs = fInfo.inputs;
            Dev.Outputs = fInfo.outputs;
            Dev.BufMin = fInfo.bufmin;
            Dev.BufMax = fInfo.bufmax;
            Dev.BufPref = fInfo.bufmax;
            Dev.BufGran = fInfo.bufgran;

            BassAsio.BASS_ASIO_Free();
        }

        private void ASIODevicesSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(String.Format("{0}#asio-support-details", Properties.Settings.Default.ProjectLink));
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


        private void LatencyWarning_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Changing the buffer size/update rate of your ASIO device will affect the MIDI timings of your host app!\n" +
                "That means that the MIDI events might be delayed before the sound is actually played on your speakers.\n\n" +
                "Always keep the buffer size to the smallest size possible, and the update rate to the highest value your sound card can handle with no dropouts, " +
                "if you want to avoid this issue.", "OmniMIDI Configurator ~ ASIO warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    class TestedASIODevices
    {
        public static string Supported = "https://raw.githubusercontent.com/KeppySoftware/OmniMIDI/master/OmniMIDIASIOSupportList/Supported.txt";
        public static string Unstable = "https://raw.githubusercontent.com/KeppySoftware/OmniMIDI/master/OmniMIDIASIOSupportList/Unstable.txt";
        public static string Unsupported = "https://raw.githubusercontent.com/KeppySoftware/OmniMIDI/master/OmniMIDIASIOSupportList/Unsupported.txt";
    }

    enum DevStatusEnum
    {
        DEVICE_SUPPORTED,
        DEVICE_UNSTABLE,
        DEVICE_UNSUPPORTED,
        DEVICE_UNKNOWN
    }

    class ASIOStatus
    {
        public DevStatusEnum Flag { get; set; }
        public string Description { get; set; }
    }

    class ASIODevice
    {
        public string Name { get; set; }
        public string Driver { get; set; }
        public ASIOStatus Status { get; set; }
        public int Inputs { get; set; }
        public int Outputs { get; set; }
        public int BufMin { get; set; }
        public int BufMax { get; set; }
        public int BufPref { get; set; }
        public int BufGran { get; set; }
        public bool NoData { get; set; }
        public int ID { get; set; }
        public List<BASS_ASIO_CHANNELINFO> ChannelsR { get; set; }
        public List<BASS_ASIO_CHANNELINFO> ChannelsL { get; set; }
    }
}
