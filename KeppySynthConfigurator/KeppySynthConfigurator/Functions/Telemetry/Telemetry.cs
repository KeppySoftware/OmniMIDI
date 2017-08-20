using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class Telemetry : Form
    {
        ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Processor");
        ManagementObjectSearcher mosGPU = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_VideoController");
        RegistryKey CurrentVerKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
        UInt64 TotalRAM = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;

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
            MessageBox.Show("The telemetry (user statistics) conducted by the app will be used to improve the application and help " +
                "the developer fix problems in the future.\nIn order for telemetry to be registered, " +
                "Keppy's Synthesizer requires your username/nickname (to identify you), your CPU, GPU, RAM, and operating system.\n" +
                "These statistics will be kept completely anonymous and will not be disclosed.\n\n" +
                "Email is optional, but if you do not provide it, I will be unable to contact you",
                "Telemetry - Disclaimer", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                RealRAM = String.Format("{0} bytes ({1})", TotalRAM, Functions.ReturnLength((long)TotalRAM));

                string gpuname = "";
                uint gpuvram = 0;
                foreach (ManagementObject moGPU in mosGPU.Get())
                {
                    gpuname = moGPU["Name"].ToString();
                    gpuvram = Convert.ToUInt32(moGPU["AdapterRAM"]);
                }
                RealGPU = String.Format("{0} ({1} of VRAM)", gpuname, Functions.ReturnLength(gpuvram));

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
                        RealPatch = String.Format("{0}.{1}.{2} (Metro)",
                            Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor,
                            Environment.OSVersion.Version.Build);
                    }
                }

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
            // Do a check first
            if (String.IsNullOrWhiteSpace(NicknameVal.Text))
            {
                MessageBox.Show("Please use a valid (nick)name!", "Keppy's Synthesizer - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailVal.Text))
            {
                DialogResult dialogResult = MessageBox.Show("Without specifying your e-mail, I will not be able to communicate with you if an issue arises.\n\nAre you sure you would like to continue sending telemetry data without specifying your e-mail address?", "Keppy's Synthesizer - Telemetry warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No) return;
                else EmailVal.Text += "No e-mail address specified";
            }
            else
            {
                if (!TelemetryExt.IsValidEmail(EmailVal.Text))
                {
                    DialogResult dialogResult = MessageBox.Show("Without specifying a valid e-mail, I will not be able to communicate with you if an issue arises.\n\nAre you sure you would like to continue sending telemetry data without specifying a valid e-mail address?", "Keppy's Synthesizer - Telemetry warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.No) return;
                    else EmailVal.Text += " (Invalid e-mail address)";
                }
            }

            if (String.IsNullOrWhiteSpace(InstCPUVal.Text) ||
                String.IsNullOrWhiteSpace(InstRAMVal.Text) ||
                String.IsNullOrWhiteSpace(OSVal.Text) ||
                String.IsNullOrWhiteSpace(InstGPUVal.Text))
            {
                MessageBox.Show("Your PC specifications are mandatory!\nYou can't remove them.", "Keppy's Synthesizer - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LetUserEditSpecs.Checked = false;
                LetUserEditSpecs_CheckedChanged(sender, e);
                return;
            }

            this.Enabled = false;

            StringBuilder TelemetryData = new StringBuilder();

            TelemetryData.AppendLine(String.Format("Telemetry data sent by {0} in {1}.", NicknameVal.Text, DateTime.Now.ToString()));
            TelemetryData.AppendLine();
            TelemetryData.AppendLine("========= Personal information =========");
            TelemetryData.AppendLine(String.Format("Name: {0} (Windows username: {1})", NicknameVal.Text, Environment.UserName));
            TelemetryData.AppendLine(String.Format("E-mail: {0}", EmailVal.Text));
            TelemetryData.AppendLine(String.Format("Name: {0}", String.IsNullOrWhiteSpace(AgeVal.Text) ? "Not specified" : AgeVal.Text));
            TelemetryData.AppendLine(String.Format("Name: {0}", String.IsNullOrWhiteSpace(CountryVal.Text) ? "Not specified" : CountryVal.Text));
            TelemetryData.AppendLine();
            TelemetryData.AppendLine("========= PC specifications =========");
            TelemetryData.AppendLine(String.Format("Processor: {0}\nReal value: {1}\n", InstCPUVal.Text, RealCPU));
            TelemetryData.AppendLine(String.Format("Installed RAM: {0}\nReal value: {1}\n", InstRAMVal.Text, RealRAM));
            TelemetryData.AppendLine(String.Format("Operating system: {0}\nReal value: {1}\n", OSVal.Text, String.Format("{0}, {1}", RealOS, RealPatch)));
            TelemetryData.AppendLine(String.Format("Graphics card: {0}\nReal value: {1}", InstGPUVal.Text, RealGPU));
            TelemetryData.AppendLine();
            TelemetryData.AppendLine("========= Additional feedback =========");
            TelemetryData.AppendLine(String.IsNullOrWhiteSpace(CountryVal.Text) ? "No additional feedback." : AdditionalFeed.Text.ToString());

            if (TelemetryExt.SendInfoForTelemetry(Encoding.ASCII.GetBytes(TelemetryData.ToString())))
            {
                MessageBox.Show("The data have been sent!\nThank you for collaborating!\n\nPress OK to close this dialog.", "Keppy's Synthesizer - Telemetry complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CurrentVerKey.Close();
                Close();
            }
        }
    }
}
