using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class Telemetry : Form
    {
        public const string Disclaimer = "The telemetry (user statistics) conducted by the app will be used to improve the application and help " +
                "the developer fix problems in the future.\n\nIn order for telemetry to be registered, " +
                "OmniMIDI requires your username/nickname (to identify you), your CPU, GPU, RAM, and operating system.\n" +
                "It will also read your MAC Address, to prevent spammers from abusing the feature, and automatically generate a Hardware ID by parsing info from your computer components.\n\n" +
                "These statistics will be kept completely anonymous and will not be disclosed.\n" +
                "E-mail is optional, but if you do not provide it, I will be unable to contact you.";

        static ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
        ManagementObjectSearcher mosSound = new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice");
        ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Processor");
        ManagementObjectSearcher mosGPU = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_VideoController");
        ManagementObject mosMotherboard = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());
        RegistryKey CurrentVerKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
        FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\OmniMIDI\\OmniMIDI.dll");
        UInt64 TotalRAM = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;


        String MACAddress = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                          where nic.OperationalStatus == OperationalStatus.Up
                          select nic.GetPhysicalAddress().ToString()
                       ).FirstOrDefault();

        String RealCPU = "";
        String RealRAM = "";
        String RealGPU = "";
        String RealOS = "";
        String RealPatch = "";

        public Telemetry()
        {
            InitializeComponent();
        }

        private void DiscLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(Disclaimer, "Telemetry - Disclaimer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Telemetry_Load(object sender, EventArgs e)
        {
            GetPCInfo();

            InstCPUVal.Text = RealCPU;
            InstRAMVal.Text = RealRAM;
            InstGPUVal.Text = RealGPU;
            OSVal.Text = String.Format("{0}, {1}", RealOS, RealPatch);
        }

        private bool GetPCInfo()
        {
            try
            {
                NicknameVal.Text = Environment.UserName;

                foreach (ManagementObject moProcessor in mosProcessor.Get()) RealCPU = moProcessor["name"].ToString();
                RealRAM = String.Format("{0} bytes ({1})", TotalRAM, Functions.ReturnLength((long)TotalRAM, false));

                string gpuname = "";
                uint gpuvram = 0;
                foreach (ManagementObject moGPU in mosGPU.Get())
                {
                    gpuname = moGPU["Name"].ToString();
                    gpuvram = Convert.ToUInt32(moGPU["AdapterRAM"]);
                }
                RealGPU = String.Format("{0} ({1} of VRAM)", gpuname, Functions.ReturnLength(gpuvram, false));

                RealOS = String.Format("{0} ({1})", OSInfo.Name, Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit");

                if (Environment.OSVersion.Version.Major == 10) // If OS is Windows 10, get UBR too
                {
                    RealPatch = String.Format("{0}.{1}.{2} (Update Build Revision {3})",
                       Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor,
                       Environment.OSVersion.Version.Build, CurrentVerKey.GetValue("UBR", 0).ToString());
                }
                else // Else, give normal version number
                {
                    if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor <= 1)
                    {
                        RealPatch = String.Format("{0}.{1}.{2} ({3})",
                            Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor,
                            Environment.OSVersion.Version.Build, Environment.OSVersion.ServicePack);
                    }
                    else
                    {
                        RealPatch = String.Format("{0}.{1}.{2} (Modern UI)",
                            Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor,
                            Environment.OSVersion.Version.Build);
                    }
                }

                SoundCards.SelectedIndex = 0;
                foreach (ManagementObject obj in mosSound.Get()) SoundCards.Items.Add(obj["Name"].ToString());

                return true;
            }
            catch { return false; }
        }

        private void LetUserEditSpecs_CheckedChanged(object sender, EventArgs e)
        {
            InstCPUVal.Enabled = LetUserEditSpecs.Checked;
            InstRAMVal.Enabled = LetUserEditSpecs.Checked;
            InstGPUVal.Enabled = LetUserEditSpecs.Checked;
            OSVal.Enabled = LetUserEditSpecs.Checked;

            if (!LetUserEditSpecs.Checked)
            {
                InstCPUVal.Text = RealCPU;
                InstRAMVal.Text = RealRAM;
                InstGPUVal.Text = RealGPU;
                OSVal.Text = String.Format("{0}, {1}", RealOS, RealPatch);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CurrentVerKey.Close();
            Close();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            Enabled = false;

            Int32 UserStatus = TelemetryExt.IsUserBanned();
            if (UserStatus != TelemetryStatus.USER_OK)
            {
                if (UserStatus == TelemetryStatus.USER_BANNED)
                {
                    MessageBox.Show(
                        String.Format("You're not allowed to send telemetry data.{0}", TelemetryStatus.Debug ? String.Format("\n\nReason: {0}", TelemetryStatus.Reasons[TelemetryStatus.TypeOfBan]) : ""),
                        "OmniMIDI - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Unable to run the telemetry.\n\nPlease try again later.", "OmniMIDI - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                }

                return;
            }

            // Do a check first
            if (String.IsNullOrWhiteSpace(NicknameVal.Text))
            {
                MessageBox.Show("Please use a valid (nick)name!", "OmniMIDI - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Enabled = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailVal.Text))
            {
                DialogResult dialogResult = MessageBox.Show("Without specifying your e-mail, I will not be able to communicate with you if an issue arises.\n\nAre you sure you would like to continue sending telemetry data without specifying your e-mail address?", "OmniMIDI - Telemetry warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No) return;
                else EmailVal.Text += "No e-mail address specified";
            }
            else
            {
                if (!TelemetryExt.IsValidEmail(EmailVal.Text))
                {
                    DialogResult dialogResult = MessageBox.Show("Without specifying a valid e-mail, I will not be able to communicate with you if an issue arises.\n\nAre you sure you would like to continue sending telemetry data without specifying a valid e-mail address?", "OmniMIDI - Telemetry warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.No) return;
                    else EmailVal.Text += " (Invalid e-mail address)";
                }
            }

            if (String.IsNullOrWhiteSpace(InstCPUVal.Text) ||
                String.IsNullOrWhiteSpace(InstRAMVal.Text) ||
                String.IsNullOrWhiteSpace(OSVal.Text) ||
                String.IsNullOrWhiteSpace(InstGPUVal.Text))
            {
                MessageBox.Show("Your PC specifications are mandatory!\nYou can't remove them.", "OmniMIDI - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LetUserEditSpecs.Checked = false;
                LetUserEditSpecs_CheckedChanged(sender, e);
                Enabled = true;
                return;
            }

            if (SoundCards.SelectedIndex == 0)
            {
                DialogResult dialogResult = MessageBox.Show("Without specifying your default sound card, I will be hard for me to troubleshoot the issue.\n\nAre you sure you would like to continue sending telemetry data without specifying your default sound card?", "OmniMIDI - Telemetry warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No) return;
            }

            if (TelemetryExt.SendInfoForTelemetry(
                // User info
                NicknameVal.Text, 
                Environment.UserName, 
                Environment.MachineName,
                EmailVal.Text,
                String.Format("{0}", String.IsNullOrWhiteSpace(AgeVal.Text) ? "Not specified" : AgeVal.Text),
                String.Format("{0}", String.IsNullOrWhiteSpace(CountryVal.Text) ? "Not specified" : CountryVal.Text),

                // Computer specifications
                String.Format("{0} (Real value: {1})", InstCPUVal.Text, RealCPU),
                String.Format("{0} (Real value: {1})", InstGPUVal.Text, RealGPU),
                String.Format("{0} (Real value: {1})", InstRAMVal.Text, RealRAM),
                String.Format("{0} (Real value: {1})", OSVal.Text, String.Format("{0}, {1}", RealOS, RealPatch)),
                SoundCards.Text.ToString(),
                TelemetryExt.ParseHWID(),
                MACAddress,

                // Other info
                DateTime.Now.ToString("dd MMMM yyyy", TelemetryExt.cultureTelemetry),
                TelemetryExt.RandomID.Next(0, 2147483647).ToString("0000000000"),
                String.Format("{0}.{1}.{2}.{3}", Driver.FileMajorPart, Driver.FileMinorPart, Driver.FileBuildPart, Driver.FilePrivatePart),

                // Feedback
                String.IsNullOrWhiteSpace(AdditionalFeed.Text) ? "No additional feedback." : AdditionalFeed.Text.ToString().Replace(System.Environment.NewLine, " "),

                // ?
                BugReport.Checked))
            {
                MessageBox.Show("The data have been sent!\nThank you for collaborating!\n\nPress OK to close this dialog.", "OmniMIDI - Telemetry complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            CurrentVerKey.Close();
            Close();
        }
    }
}
