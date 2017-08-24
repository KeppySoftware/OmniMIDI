/*
 * 
 * Keppy's Synthesizer Debug Window
 * by KaleidonKep99
 * 
 * Full of potatoes
 *
 */

using System;
using System.Management;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace KeppySynthDebugWindow
{
    public partial class KeppySynthDebugWindow : Form
    {
        // Topmost
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr Handle, IntPtr HandleInsertAfter, int PosX, int PosY, int SizeX, int SizeY, uint Flags);

        static readonly IntPtr TOPMOST = new IntPtr(-1);
        static readonly IntPtr NOTOPMOST = new IntPtr(-2);
        const UInt32 KEEPPOS = 2 | 1;

        // Voices
        int ch1;
        int ch2;
        int ch3;
        int ch4;
        int ch5;
        int ch6;
        int ch7;
        int ch8;
        int ch9;
        int ch10;
        int ch11;
        int ch12;
        int ch13;
        int ch14;
        int ch15;
        int ch16;

        // Debug information
        string currentapp;
        string bitapp;
        UInt64 ramusage;
        Int32 handlecount;
        Int32 sndbfvalue;
        string currentappreturn;
        string bitappreturn;

        // Required for KS
        FileVersionInfo Driver { get; set; }
        RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer", false);
        RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", false);
        RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", false);
        RegistryKey WinVer = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
        String LogPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Keppy's Synthesizer\\DebugOutput.txt";
        String Credits = "Copyright Ⓒ 2011\nKaleidonKep99, Kode54 & Mudlord";

        // Windows information
        ComputerInfo CI = new ComputerInfo();
        string FullVersion;
        string bit;

        // CPU/GPU information
        ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Processor");
        ManagementObjectSearcher mosGPU = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_VideoController");
        ManagementObjectSearcher mosEnc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Chassis");
        string cpubit = "32";
        int cpuclock = 0;
        string cpumanufacturer = "Unknown";
        string cpuname = "Unknown";
        string gpuchip = "Unknown";
        string gpuname = "Unknown";
        string gpuver = "N/A";
        UInt32 gpuvram = 0;
        string enclosure = "Unknown";
        int coreCount = 0;

        public KeppySynthDebugWindow()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true); // AAAAA I hate flickering
        }

        private void KeppySynthDebugWindow_Load(object sender, EventArgs e)
        {
            Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll"); // Gets Keppy's Synthesizer version
            CurrentKSVer.ToolTipTitle = String.Format("Keppy's Synthesizer {0}", Driver.FileVersion);
            CurrentKSVer.SetToolTip(KSLogo, Credits);
            CurrentKSVer.SetToolTip(KSLogoVoc, Credits);
            CurrentKSVer.SetToolTip(KSLogoThrd, Credits);
            CurrentKSVer.SetToolTip(VersionLabel, Credits);
            VersionLabel.Text = String.Format("Keppy's Synthesizer {0}", Driver.FileVersion);
            GetWindowsInfoData(); // Get info about your Windows installation
            SynthDbg.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
            ChannelVoices.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
            ThreadTime.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
            PCSpecs.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
            Tabs.SelectedIndex = 1;
            Tabs.SelectedIndex = 0;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Program.BringToFrontMessage)
            {
                WinAPI.ShowWindow(Handle, WinAPI.SW_RESTORE);
                WinAPI.SetForegroundWindow(Handle);
            }
            base.WndProc(ref m);
        }

        private string GetCurrentRAMUsage(UInt64 length)
        {
            string size;
            try
            {
                if (length >= 1099511627776)
                {
                    if (length >= 1099511627776 && length < 10995116277760)
                        size = ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString("0.00 TB");
                    else
                        size = ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString("0.0 TB");
                }
                else if (length >= 1073741824)
                {
                    if (length >= 1073741824 && length < 10737418240)
                        size = (((length / 1024f) / 1024f) / 1024f).ToString("0.00 GB");
                    else
                        size = (((length / 1024f) / 1024f) / 1024f).ToString("0.0 GB");
                }
                else if (length >= 1048576)
                {
                    if (length >= 1048576 && length < 10485760)
                        size = ((length / 1024f) / 1024f).ToString("0.00 MB");
                    else
                        size = ((length / 1024f) / 1024f).ToString("0.0 MB");
                }
                else if (length >= 1024)
                {
                    if (length >= 1024 && length < 10240)
                        size = (length / 1024f).ToString("0.00 KB");
                    else
                        size = (length / 1024f).ToString("0.0 KB");
                }
                else
                {
                    if (length >= 1 && length < 1024)
                        size = (length).ToString("0.00 B");
                    else
                        size = (length / 1024f).ToString("0.0 B");
                }
            }
            catch { size = "-"; }
            return size;
        }

        private System.Drawing.Bitmap CPUImage()
        {
            if (cpumanufacturer == "GenuineIntel")
            {
                CPULogoTT.SetToolTip(CPULogo, "You're using an Intel CPU.");
                return Properties.Resources.intel;
            }
            else if (cpumanufacturer == "AuthenticAMD")
            {
                CPULogoTT.SetToolTip(CPULogo, "You're using an AMD CPU.");
                return Properties.Resources.amd;
            }
            else if (cpumanufacturer == "CentaurHauls" || cpumanufacturer == "VIA VIA VIA ")
            {
                CPULogoTT.SetToolTip(CPULogo, "You're using a VIA CPU.");
                return Properties.Resources.via;
            }
            else if (cpumanufacturer == "VMwareVMware")
            {
                CPULogoTT.SetToolTip(CPULogo, "You're running the app inside a VMware virtual machine.");
                return Properties.Resources.vmware;
            }
            else if (cpumanufacturer == " lrpepyh vr")
            {
                CPULogoTT.SetToolTip(CPULogo, "You're running the app inside a Parallels virtual machine.");
                return Properties.Resources.parallels;
            }
            else if (cpumanufacturer == "KVMKVMKVM" || cpumanufacturer.Contains("KVMKVMKVM"))
            {
                CPULogoTT.SetToolTip(CPULogo, "You're running the app inside a KVM.");
                return Properties.Resources.kvm;
            }
            else if (cpumanufacturer == "Microsoft Hv")
            {
                CPULogoTT.SetToolTip(CPULogo, "You're running the app inside a Hyper-V virtual machine.");
                return Properties.Resources.ws2012;
            }
            else
            {
                CPULogoTT.SetToolTip(CPULogo, "You're using an unknown CPU.");
                return Properties.Resources.unknown;
            }
        }

        private System.Drawing.Bitmap WinImage()
        {
            OSInfo.OSVERSIONINFOEX osVersionInfo = new OSInfo.OSVERSIONINFOEX();
            osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSInfo.OSVERSIONINFOEX));
            if (!OSInfo.GetVersionEx(ref osVersionInfo))
            {
                WinLogoTT.SetToolTip(WinLogo, "You're using an unknown OS.");
                return Properties.Resources.unknown;
            }
            else
            {
                int p = (int)Environment.OSVersion.Platform;
                if ((p == 4) || (p == 6) || (p == 128))
                {
                    WinLogoTT.SetToolTip(WinLogo, "You're using an unknown OS.");
                    return Properties.Resources.other;
                }
                else
                {
                    if (Environment.OSVersion.Version.Major == 5)
                    {
                        WinLogoTT.SetToolTip(WinLogo, "You're using an unsupported OS.");
                        return Properties.Resources.other;
                    }
                    if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0)
                    {
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2008.");
                        else
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows Vista.");

                        if (VisualStyleInformation.IsEnabledByUser == true)
                            return Properties.Resources.wvista;
                        else
                            return Properties.Resources.w9x;
                    }
                    else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                    {
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2008 R2.");
                        else
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows 7.");

                        if (VisualStyleInformation.IsEnabledByUser == true)
                            return Properties.Resources.w7;
                        else
                            return Properties.Resources.w9x;
                    }
                    else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 2)
                    {
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                        {
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2012.");
                            return Properties.Resources.ws2012;
                        }
                        else
                        {
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows 8.");
                            return Properties.Resources.w8;
                        }
                    }
                    else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 3)
                    {
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                        {
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2012 R2.");
                            return Properties.Resources.ws2012;
                        }
                        else
                        {
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows 8.1.");
                            return Properties.Resources.w81;
                        }
                    }
                    else if (Environment.OSVersion.Version.Major == 10)
                    {
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                        {
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2016.");
                            return Properties.Resources.ws2016;
                        }
                        else
                        {
                            WinLogoTT.SetToolTip(WinLogo, "You're using Windows 10.");
                            return Properties.Resources.w10;
                        }
                    }
                    else
                        return Properties.Resources.unknown;
                }
            }
        }

        private string CPUArch(int Value)
        {
            if (Value == 0)
                return "x86";
            else if (Value == 5)
                return "ARM";
            else if (Value == 6)
                return "IA64";
            else if (Value == 9)
                return "x64";
            else
                return "N/A";
        }

        private void GetWindowsInfoData()
        {
            try
            {
                String Frequency = "";
                // Get CPU info
                foreach (ManagementObject moProcessor in mosProcessor.Get())
                {
                    cpuclock = int.Parse(moProcessor["maxclockspeed"].ToString());
                    cpubit = CPUArch(int.Parse(moProcessor["Architecture"].ToString()));
                    cpuname = moProcessor["name"].ToString();
                    cpumanufacturer = moProcessor["manufacturer"].ToString();
                    coreCount += int.Parse(moProcessor["NumberOfCores"].ToString());
                }

                // Get GPU info
                try
                {
                    foreach (ManagementObject moGPU in mosGPU.Get())
                    {
                        gpuchip = moGPU["VideoProcessor"].ToString();
                        gpuname = moGPU["Name"].ToString();
                        gpuvram = Convert.ToUInt32(moGPU["AdapterRAM"]);
                        gpuver = moGPU["DriverVersion"].ToString();
                    }
                }
                catch
                {
                    // The GPU doesn't want to share its internal chip. Skip.
                    // This might be caused by outdated drivers, an integrated GPU, or by a GPU that doesn't support WMI data.
                    // (The latter is usually caused by using non-WDDM drivers)
                    GPUInternalChip.Enabled = false;
                    foreach (ManagementObject moGPU in mosGPU.Get())
                    {
                        gpuchip = "The graphics card refused to share this info";
                        gpuname = moGPU["Name"].ToString();
                        gpuvram = Convert.ToUInt32(moGPU["AdapterRAM"]);
                        gpuver = moGPU["DriverVersion"].ToString();
                    }
                }

                // Get enclosure info
                foreach (ManagementObject moEnc in mosEnc.Get())
                {
                    foreach (int i in (UInt16[])(moEnc["ChassisTypes"]))
                    {
                        enclosure = OSInfo.Chassis[i];
                    }
                }

                if (Environment.OSVersion.Version.Major == 10) // If OS is Windows 10, get UBR too
                {
                    FullVersion = String.Format("{0}.{1}.{2}.{3}",
                       Environment.OSVersion.Version.Major.ToString(), Environment.OSVersion.Version.Minor.ToString(),
                       Environment.OSVersion.Version.Build.ToString(), WinVer.GetValue("UBR", 0).ToString());
                }
                else // Else, give normal version number
                {
                    FullVersion = String.Format("{0}.{1}.{2}",
                       Environment.OSVersion.Version.Major.ToString(), Environment.OSVersion.Version.Minor.ToString(),
                       Environment.OSVersion.Version.Build.ToString());
                }

                WinLogo.Image = WinImage();
                CPULogo.Image = CPUImage();

                if (Environment.Is64BitOperatingSystem == true) { bit = "AMD64"; } else { bit = "i386"; }  // Gets Windows architecture  

                if (cpuclock < 1000)
                    Frequency = String.Format("{0}MHz", cpuclock);
                else
                    Frequency = String.Format("{0}GHz", ((float)cpuclock / 1000).ToString("0.00"));

                COS.Text = String.Format("{0} ({1}, {2})", OSInfo.Name, FullVersion, bit);
                CPU.Text = String.Format("{0} ({1} processor)", cpuname, cpubit);
                CPUInfo.Text = String.Format("Made by {0}, {1} cores and {2} threads, {3}", cpumanufacturer, coreCount, Environment.ProcessorCount, Frequency);
                GPU.Text = gpuname;
                GPUInternalChip.Text = gpuchip;
                GPUInfo.Text = String.Format("{0}MB VRAM, driver version {1}", (gpuvram / 1048576), gpuver);
                MT.Text = enclosure;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OpenAppLocat_Click(object sender, EventArgs e) // Opens the directory of the current app that's using Keppy's Synthesizer
        {
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", false);
            string currentapp = Watchdog.GetValue("currentapp", "Not available").ToString();
            Process.Start(System.IO.Path.GetDirectoryName(currentapp.RemoveGarbageCharacters()));
            Watchdog.Close();
        }

        private void CopyToClipBoardCmd() // Copies content of window to clipboard
        {
            try
            {
                ForceGetInfo();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(String.Format("Keppy's Synthesizer version {0}", Driver.FileVersion));
                sb.AppendLine("========= Debug information =========");
                sb.AppendLine(String.Format("Driver version: {0}", Driver.FileVersion));
                sb.AppendLine(String.Format("{0} {1}", CMALabel.Text, CMA.Text));
                sb.AppendLine(String.Format("{0} {1}", AVLabel.Text, AV.Text));
                sb.AppendLine(String.Format("{0} {1}", AvVLabel.Text, AvV.Text));
                sb.AppendLine(String.Format("{0} {1}", RTLabel.Text, RT.Text));
                if ((int)Settings.GetValue("xaudiodisabled", "0") == 2 || (int)Settings.GetValue("xaudiodisabled", "0") == 3) sb.AppendLine(String.Format("{0} {1}", AERTLabel.Text, AERT.Text));
                else sb.AppendLine(String.Format("{0} {1}", DDSLabel.Text, DDS.Text));
                sb.AppendLine(String.Format("{0} {1}", RAMUsageVLabel.Text, RAMUsageV.Text));
                sb.AppendLine(String.Format("{0} {1}", HCountVLabel.Text, HCountV.Text));
                sb.AppendLine("======= Channels  information =======");
                sb.AppendLine(String.Format("{0} {1}", CHV1L.Text, CHV1.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV2L.Text, CHV2.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV3L.Text, CHV3.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV4L.Text, CHV4.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV5L.Text, CHV5.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV6L.Text, CHV6.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV7L.Text, CHV7.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV8L.Text, CHV8.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV9L.Text, CHV9.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV10L.Text, CHV10.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV11L.Text, CHV11.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV12L.Text, CHV12.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV13L.Text, CHV13.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV14L.Text, CHV14.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV15L.Text, CHV15.Text));
                sb.AppendLine(String.Format("{0} {1}", CHV16L.Text, CHV16.Text));
                sb.AppendLine("======== System  information ========");
                sb.AppendLine(String.Format("Driver version: {0}", Driver.FileVersion));
                sb.AppendLine(String.Format("{0} {1}", COSLabel.Text, COS.Text));
                sb.AppendLine(String.Format("{0} {1}", CPULabel.Text, CPU.Text));
                sb.AppendLine(String.Format("{0} {1}", CPUInfoLabel.Text, CPUInfo.Text));
                sb.AppendLine(String.Format("{0} {1}", GPULabel.Text, GPU.Text));
                sb.AppendLine(String.Format("{0} {1}", GPUInternalChipLabel.Text, GPUInternalChip.Text));
                sb.AppendLine(String.Format("{0} {1}", GPUInfoLabel.Text, GPUInfo.Text));
                sb.AppendLine(String.Format("{0} {1}", TMLabel.Text, TM.Text));
                sb.AppendLine(String.Format("{0} {1}", AMLabel.Text, AM.Text));
                sb.AppendLine(String.Format("{0} {1}", MTLabel.Text, MT.Text));

                Thread thread = new Thread(() => Clipboard.SetText(sb.ToString())); // Creates another thread, otherwise the form locks up while copying the richtextbox
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch {
                // lel
            }
            finally
            {
                MessageBox.Show("Info copied to clipboard.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information); // Done, now get out
            }
        }

        private void CopyToClip_Click(object sender, EventArgs e) // Allows you to copy the content of the richtextbox to clipboard
        {
            CopyToClipBoardCmd();
        }

        private void Exit_Click(object sender, EventArgs e) // Exit? lel
        {
            Application.ExitThread(); // R.I.P. debug
        }

        private void UpdateActiveVoicesPerChannel()
        {
            try
            {
                ch1 = Convert.ToInt32(Debug.GetValue("chv1", "0").ToString());
                ch2 = Convert.ToInt32(Debug.GetValue("chv2", "0").ToString());
                ch3 = Convert.ToInt32(Debug.GetValue("chv3", "0").ToString());
                ch4 = Convert.ToInt32(Debug.GetValue("chv4", "0").ToString());
                ch5 = Convert.ToInt32(Debug.GetValue("chv5", "0").ToString());
                ch6 = Convert.ToInt32(Debug.GetValue("chv6", "0").ToString());
                ch7 = Convert.ToInt32(Debug.GetValue("chv7", "0").ToString());
                ch8 = Convert.ToInt32(Debug.GetValue("chv8", "0").ToString());
                ch9 = Convert.ToInt32(Debug.GetValue("chv9", "0").ToString());
                ch10 = Convert.ToInt32(Debug.GetValue("chv10", "0").ToString());
                ch11 = Convert.ToInt32(Debug.GetValue("chv11", "0").ToString());
                ch12 = Convert.ToInt32(Debug.GetValue("chv12", "0").ToString());
                ch13 = Convert.ToInt32(Debug.GetValue("chv13", "0").ToString());
                ch14 = Convert.ToInt32(Debug.GetValue("chv14", "0").ToString());
                ch15 = Convert.ToInt32(Debug.GetValue("chv15", "0").ToString());
                ch16 = Convert.ToInt32(Debug.GetValue("chv16", "0").ToString());
            }
            catch
            {
                ch1 = 0;
                ch2 = 0;
                ch3 = 0;
                ch4 = 0;
                ch5 = 0;
                ch6 = 0;
                ch7 = 0;
                ch8 = 0;
                ch9 = 0;
                ch10 = 0;
                ch11 = 0;
                ch12 = 0;
                ch13 = 0;
                ch14 = 0;
                ch15 = 0;
                ch16 = 0;
            }
        }

        private string GetActiveVoices()
        {
            try
            {
                return String.Format("{0}", ((ch1 + ch2 + ch3 + ch4 + ch5 + ch6 + ch6 + ch7 + ch8 + ch9 + ch10 + ch11 + ch12 + ch13 + ch14 + ch15 + ch16)).ToString());
            }
            catch
            {
                return "0";
            }
        }

        private string GetAverageVoices()
        {
            try
            {
                return String.Format("{0} V/f", ((ch1 + ch2 + ch3 + ch4 + ch5 + ch6 + ch7 + ch8 + ch9 + ch10 + ch11 + ch12 + ch13 + ch14 + ch15 + ch16) / 16.67f).ToString("0.0"));
            }
            catch
            {
                return "0 V/f";
            }
        }

        private void ForceGetInfo()
        {
            try
            {
                currentapp = Watchdog.GetValue("currentapp", "None").ToString(); // Gets app's name. If the name of the app is invalid, it'll return "Not available"
                bitapp = Watchdog.GetValue("bit", "...").ToString(); // Gets app's architecture. If the app doesn't return a value, it'll return "Unknown"
                ramusage = Convert.ToUInt64(Debug.GetValue("ramusage", 0).ToString()); // Gets app's working set size in bytes. (Eg. How much the app is using for both RAM and paging file)
                handlecount = Convert.ToInt32(Debug.GetValue("handlecount", 0).ToString()); // Gets app's handles count.
                sndbfvalue = Convert.ToInt32(Settings.GetValue("sndbfvalue", 0)); // Size of the decoded data, in bytes

                // Time to write all the stuff to the string builder
                if (System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters()) == "0")
                {
                    OpenAppLocat.Enabled = false;
                    currentappreturn = "None";
                }
                else
                {
                    currentappreturn = System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters());
                }

                if (bitapp.RemoveGarbageCharacters() == "0")
                {
                    bitappreturn = "...";
                }
                else
                {
                    bitappreturn = bitapp.RemoveGarbageCharacters();
                }

                HCountV.Text = String.Format("{0} handles", handlecount);
                RAMUsageV.Text = GetCurrentRAMUsage(ramusage);
                CMA.Text = String.Format("{0} ({1})", currentappreturn, bitappreturn); // Removes garbage characters

                // Get current active voices
                UpdateActiveVoicesPerChannel();
                if (Convert.ToInt32(GetActiveVoices()) > Convert.ToInt32(Settings.GetValue("polyphony", "512")))
                {
                    AV.Font = new System.Drawing.Font(AV.Font, System.Drawing.FontStyle.Bold);
                    AV.ForeColor = Color.DarkRed;
                }
                else
                {
                    AV.Font = new System.Drawing.Font(AV.Font, System.Drawing.FontStyle.Regular);
                    AV.ForeColor = SystemColors.ControlText;
                }
                AV.Text = GetActiveVoices();
                AvV.Text = GetAverageVoices();

                if (Convert.ToInt32(Settings.GetValue("encmode", "0")) == 1)
                {
                    RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Italic);
                    RT.Text = "Unavailable"; // If BASS is in encoding mode, BASS usage will stay at constant 100%.
                }
                else
                {
                    if (Convert.ToInt32(Debug.GetValue("currentcpuusage0", "0").ToString()) > Convert.ToInt32(Settings.GetValue("cpu", "75").ToString()) && Settings.GetValue("cpu", "75").ToString() != "0")
                    {
                        RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Bold);
                        RT.ForeColor = Color.DarkRed;
                        RT.Text = String.Format("{0}% (Beyond limit: {1}%)", Debug.GetValue("currentcpuusage0").ToString(), Settings.GetValue("cpu", "75").ToString());
                    }
                    else
                    {
                        RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Regular);
                        RT.ForeColor = SystemColors.ControlText;
                        RT.Text = String.Format("{0}%", Debug.GetValue("currentcpuusage0", "0").ToString()); // Else, it'll give you the info about how many cycles it needs to work.
                    }
                }

                if (Convert.ToInt32(Settings.GetValue("xaudiodisabled", "0")) == 0)
                {
                    DDSLabel.Visible = true;
                    DDS.Visible = true;
                    AERTLabel.Visible = false;
                    AERT.Visible = false;
                    DDSLabel.Enabled = true;
                    DDS.Enabled = true;
                    DDS.Text = String.Format("{0} ({1} x 4)", (sndbfvalue * 4), sndbfvalue);
                }
                else if (Convert.ToInt32(Settings.GetValue("xaudiodisabled", "0")) == 1)
                {
                    DDSLabel.Visible = true;
                    DDS.Visible = true;
                    AERTLabel.Visible = false;
                    AERT.Visible = false;
                    DDSLabel.Enabled = false;
                    DDS.Enabled = false;
                    DDS.Text = "Unavailable";
                }
                else
                {
                    DDSLabel.Visible = false;
                    DDS.Visible = false;
                    AERTLabel.Visible = true;
                    AERT.Visible = true;
                    AERT.Text = String.Format("{0}%", Debug.GetValue("currentcpuusageE0", "0").ToString());
                }

                MTRT.Text = String.Format("{0}ms", Debug.GetValue("td1", 0).ToString());
                AERTi.Text = String.Format("{0}ms", Debug.GetValue("td2", 0).ToString());
                SLRT.Text = String.Format("{0}ms", Debug.GetValue("td3", 0).ToString());
                NCRT.Text = String.Format("{0}ms", Debug.GetValue("td4", 0).ToString());

                UpdateActiveVoicesPerChannel();
                String FormatForVoices = "{0} voices";
                CHV1.Text = String.Format(FormatForVoices, ch1);
                CHV2.Text = String.Format(FormatForVoices, ch2);
                CHV3.Text = String.Format(FormatForVoices, ch3);
                CHV4.Text = String.Format(FormatForVoices, ch4);
                CHV5.Text = String.Format(FormatForVoices, ch5);
                CHV6.Text = String.Format(FormatForVoices, ch6);
                CHV7.Text = String.Format(FormatForVoices, ch7);
                CHV8.Text = String.Format(FormatForVoices, ch8);
                CHV9.Text = String.Format(FormatForVoices, ch9);
                CHV10.Text = String.Format(FormatForVoices, ch10);
                CHV11.Text = String.Format(FormatForVoices, ch11);
                CHV12.Text = String.Format(FormatForVoices, ch12);
                CHV13.Text = String.Format(FormatForVoices, ch13);
                CHV14.Text = String.Format(FormatForVoices, ch14);
                CHV15.Text = String.Format(FormatForVoices, ch15);
                CHV16.Text = String.Format(FormatForVoices, ch16);

                Process thisProc = Process.GetCurrentProcess(); // Go to the next function for an explanation
                thisProc.PriorityClass = ProcessPriorityClass.Idle; // Tells Windows that the process doesn't require a lot of resources     

                // This thread just takes the available and total memory info from Windows, then outputs them in the 2nd tab

                ComputerInfo CI = new ComputerInfo();
                ulong avmem = CI.AvailablePhysicalMemory;
                ulong tlmem = CI.TotalPhysicalMemory;
                ulong avmemint = avmem / (1024 * 1024);
                ulong tlmemint = tlmem / (1024 * 1024);
                double percentage = avmem * 100.0 / tlmem;

                TM.Text = String.Format("{0} ({1} bytes)", (tlmem / (1024 * 1024) + "MB").ToString(), tlmem.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")));
                AM.Text = String.Format("{0} ({1}%, {2} bytes)", (avmem / (1024 * 1024) + "MB").ToString(), Math.Round(percentage, 1).ToString(), avmem.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")));

                CI = null;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void MemoryThread_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Tabs.SelectedIndex == 3)
                {
                    Process thisProc = Process.GetCurrentProcess(); // Go to the next function for an explanation
                    thisProc.PriorityClass = ProcessPriorityClass.Idle; // Tells Windows that the process doesn't require a lot of resources     

                    // This thread just takes the available and total memory info from Windows, then outputs them in the 2nd tab

                    ComputerInfo CI = new ComputerInfo();
                    ulong avmem = CI.AvailablePhysicalMemory;
                    ulong tlmem = CI.TotalPhysicalMemory;
                    ulong avmemint = avmem / (1024 * 1024);
                    ulong tlmemint = tlmem / (1024 * 1024);
                    double percentage = avmem * 100.0 / tlmem;

                    TM.Text = String.Format("{0} ({1} bytes)", (tlmem / (1024 * 1024) + "MB").ToString(), tlmem.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")));
                    AM.Text = String.Format("{0} ({1:0.#}%, {2} bytes)", (avmem / (1024 * 1024) + "MB").ToString(), Math.Round(percentage, 1).ToString(), avmem.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")));

                    CI = null;
                }
            }
            catch { }
        }

        private void OpenConfigurator_Click(object sender, EventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthConfigurator.exe");
        }

        int paintReps = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Thread.Sleep(1);

            if (paintReps++ % 500 == 0)
                Application.DoEvents();
        }

        // Snap feature

        private const int SnapDist = 25;

        private bool DoSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= SnapDist;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            Screen scn = Screen.FromPoint(this.Location);
            if (DoSnap(this.Left, scn.WorkingArea.Left)) this.Left = scn.WorkingArea.Left;
            if (DoSnap(this.Top, scn.WorkingArea.Top)) this.Top = scn.WorkingArea.Top;
            if (DoSnap(scn.WorkingArea.Right, this.Right)) this.Left = scn.WorkingArea.Right - this.Width;
            if (DoSnap(scn.WorkingArea.Bottom, this.Bottom)) this.Top = scn.WorkingArea.Bottom - this.Height;
        }

        private void DebugWinTop_Click(object sender, EventArgs e)
        {
            if (DebugWinTop.Checked)
            {
                DebugWinTop.Checked = false;
                SetWindowPos(this.Handle, NOTOPMOST, 0, 0, 0, 0, KEEPPOS);
            }
            else
            {
                DebugWinTop.Checked = true;
                SetWindowPos(this.Handle, TOPMOST, 0, 0, 0, 0, KEEPPOS);
            }
        }

        private void SonicMode_Click(object sender, EventArgs e)
        {
            if (!SonicMode.Checked)
            {
                SonicMode.Checked = true;
                DebugInfo.Interval = 1;
                MemoryThread.Interval = 1;
            }
            else
            {
                SonicMode.Checked = false;
                DebugInfo.Interval = 100;
                MemoryThread.Interval = 100;
            }
        }

        private void DebugInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (Tabs.SelectedIndex == 0)
                    {
                        currentapp = Watchdog.GetValue("currentapp", "None").ToString(); // Gets app's name. If the name of the app is invalid, it'll return "Not available"
                        bitapp = Watchdog.GetValue("bit", "...").ToString(); // Gets app's architecture. If the app doesn't return a value, it'll return "Unknown"
                        ramusage = Convert.ToUInt64(Debug.GetValue("ramusage", 0).ToString()); // Gets app's working set size in bytes. (Eg. How much the app is using for both RAM and paging file)
                        handlecount = Convert.ToInt32(Debug.GetValue("handlecount", 0).ToString()); // Gets app's handles count.
                        sndbfvalue = Convert.ToInt32(Settings.GetValue("sndbfvalue", 0)); // Size of the decoded data, in bytes

                        // Time to write all the stuff to the string builder
                        if (System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters()) == "0")
                        {
                            OpenAppLocat.Enabled = false;
                            currentappreturn = "None";
                        }
                        else
                        {
                            currentappreturn = System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters());
                        }

                        if (bitapp.RemoveGarbageCharacters() == "0")
                        {
                            bitappreturn = "...";
                        }
                        else
                        {
                            bitappreturn = bitapp.RemoveGarbageCharacters();
                        }

                        HCountV.Text = String.Format("{0} handles", handlecount);
                        RAMUsageV.Text = GetCurrentRAMUsage(ramusage);
                        CMA.Text = String.Format("{0} ({1})", currentappreturn, bitappreturn); // Removes garbage characters

                        // Get current active voices
                        UpdateActiveVoicesPerChannel();
                        if (Convert.ToInt32(GetActiveVoices()) > Convert.ToInt32(Settings.GetValue("polyphony", "512")))
                        {
                            AV.Font = new System.Drawing.Font(AV.Font, System.Drawing.FontStyle.Bold);
                            AV.ForeColor = Color.DarkRed;
                        }
                        else
                        {
                            AV.Font = new System.Drawing.Font(AV.Font, System.Drawing.FontStyle.Regular);
                            AV.ForeColor = SystemColors.ControlText;
                        }
                        AV.Text = GetActiveVoices();
                        AvV.Text = GetAverageVoices();

                        if (Convert.ToInt32(Settings.GetValue("encmode", "0")) == 1)
                        {
                            RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Italic);
                            RT.Text = "Unavailable"; // If BASS is in encoding mode, BASS usage will stay at constant 100%.
                        }
                        else
                        {
                            if (Convert.ToInt32(Debug.GetValue("currentcpuusage0", "0").ToString()) > Convert.ToInt32(Settings.GetValue("cpu", "75").ToString()) && Settings.GetValue("cpu", "75").ToString() != "0")
                            {
                                RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Bold);
                                RT.ForeColor = Color.DarkRed;
                                RT.Text = String.Format("{0}% (Beyond limit: {1}%)", Debug.GetValue("currentcpuusage0").ToString(), Settings.GetValue("cpu", "75").ToString());
                            }
                            else
                            {
                                RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Regular);
                                RT.ForeColor = SystemColors.ControlText;
                                RT.Text = String.Format("{0}%", Debug.GetValue("currentcpuusage0", "0").ToString()); // Else, it'll give you the info about how many cycles it needs to work.
                            }
                        }

                        if (Convert.ToInt32(Settings.GetValue("xaudiodisabled", "0")) == 0)
                        {
                            DDSLabel.Visible = true;
                            DDS.Visible = true;
                            AERTLabel.Visible = false;
                            AERT.Visible = false;
                            ASIOLLabel.Visible = false;
                            ASIOL.Visible = false;
                            DDSLabel.Enabled = true;
                            DDS.Enabled = true;
                            DDS.Text = String.Format("{0} ({1} x 4)", (sndbfvalue * 4), sndbfvalue);
                        }
                        else if (Convert.ToInt32(Settings.GetValue("xaudiodisabled", "0")) == 1)
                        {
                            DDSLabel.Visible = true;
                            DDS.Visible = true;
                            AERTLabel.Visible = false;
                            AERT.Visible = false;
                            ASIOLLabel.Visible = false;
                            ASIOL.Visible = false;
                            DDSLabel.Enabled = false;
                            DDS.Enabled = false;
                            DDS.Text = "Unavailable";
                        }
                        else if (Convert.ToInt32(Settings.GetValue("xaudiodisabled", "0")) == 2)
                        {
                            DDSLabel.Visible = false;
                            DDS.Visible = false;
                            AERTLabel.Visible = false;
                            AERT.Visible = false;
                            ASIOLLabel.Visible = true;
                            ASIOL.Visible = true;
                            ASIOL.Text = String.Format("Input {0}ms, Output {1}ms", Debug.GetValue("asioinlatency", "0").ToString(), Debug.GetValue("asiooutlatency", "0").ToString());
                        }
                        else
                        {
                            DDSLabel.Visible = false;
                            DDS.Visible = false;
                            AERTLabel.Visible = true;
                            AERT.Visible = true;
                            ASIOLLabel.Visible = false;
                            ASIOL.Visible = false;
                            AERT.Text = String.Format("{0}%", Debug.GetValue("currentcpuusageE0", "0").ToString());
                        }
                    }
                    else if (Tabs.SelectedIndex == 1)
                    {
                        UpdateActiveVoicesPerChannel();
                        String FormatForVoices = "{0} voices";
                        CHV1.Text = String.Format(FormatForVoices, ch1);
                        CHV2.Text = String.Format(FormatForVoices, ch2);
                        CHV3.Text = String.Format(FormatForVoices, ch3);
                        CHV4.Text = String.Format(FormatForVoices, ch4);
                        CHV5.Text = String.Format(FormatForVoices, ch5);
                        CHV6.Text = String.Format(FormatForVoices, ch6);
                        CHV7.Text = String.Format(FormatForVoices, ch7);
                        CHV8.Text = String.Format(FormatForVoices, ch8);
                        CHV9.Text = String.Format(FormatForVoices, ch9);
                        CHV10.Text = String.Format(FormatForVoices, ch10);
                        CHV11.Text = String.Format(FormatForVoices, ch11);
                        CHV12.Text = String.Format(FormatForVoices, ch12);
                        CHV13.Text = String.Format(FormatForVoices, ch13);
                        CHV14.Text = String.Format(FormatForVoices, ch14);
                        CHV15.Text = String.Format(FormatForVoices, ch15);
                        CHV16.Text = String.Format(FormatForVoices, ch16);
                    }
                    else if (Tabs.SelectedIndex == 2)
                    {
                        MTRT.Text = String.Format("{0}ms", Debug.GetValue("td1", 0).ToString());
                        AERTi.Text = String.Format("{0}ms", Debug.GetValue("td2", 0).ToString());
                        SLRT.Text = String.Format("{0}ms", Debug.GetValue("td3", 0).ToString());
                        NCRT.Text = String.Format("{0}ms", Debug.GetValue("td4", 0).ToString());
                    }
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                // If something goes wrong, here's an error handler
                MessageBox.Show(ex.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tabs.SelectedIndex == 3)
            {
                MemoryThread.Enabled = true;
                DebugInfo.Enabled = false;
            }
            else
            {
                MemoryThread.Enabled = false;
                DebugInfo.Enabled = true;
            }
        }
    }
}

public static class RegexConvert
{
    // Some stuff I use to remove garbage text from the strings
    public static string RemoveGarbageCharacters(this string input)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9()!'-_.\\\\ ]");
        return rgx.Replace(input, "");
    }
}
