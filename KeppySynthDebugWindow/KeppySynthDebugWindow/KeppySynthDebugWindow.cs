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
using System.IO.Pipes;
using System.IO;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.Reflection;

namespace KeppySynthDebugWindow
{
    public partial class KeppySynthDebugWindow : Form
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA
           lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FindClose(IntPtr hFindFile);

        // Topmost
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr Handle, IntPtr HandleInsertAfter, int PosX, int PosY, int SizeX, int SizeY, uint Flags);

        static readonly IntPtr TOPMOST = new IntPtr(-1);
        static readonly IntPtr NOTOPMOST = new IntPtr(-2);
        const UInt32 KEEPPOS = 2 | 1;

        // Voices
        UInt64[] CHs = new UInt64[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // Debug information
        private BindingList<String> KSPipes = new BindingList<String>();
        string currentappreturn;
        string bitappreturn;
        const int tryConnectTimeout = 15000;

        // Required for KS
        FileVersionInfo Driver { get; set; }
        RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer", false);
        RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", false);
        RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", false);
        RegistryKey WinVer = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
        String LogPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Keppy's Synthesizer\\DebugOutput.txt";

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

        private string ParseEgg()
        {
            Random RND = new Random();
            int ThisOne = RND.Next(0, Properties.Settings.Default.LeMessages.Count - 1);
            return Properties.Settings.Default.LeMessages[ThisOne];
        }

        private void KeppySynthDebugWindow_Load(object sender, EventArgs e)
        {
            try
            {
                Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll"); // Gets Keppy's Synthesizer version
                VersionLabel.Text = String.Format("Keppy's Synthesizer {0}\n{1}", Driver.FileVersion, ParseEgg());
                GetWindowsInfoData(); // Get info about your Windows installation
                SynthDbg.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
                ChannelVoices.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
                ThreadTime.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
                PCSpecs.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
                Tabs.SelectedIndex = 1;
                Tabs.SelectedIndex = 0;

                CheckMem.RunWorkerAsync();

                CheckDebugPorts();
                SelectedDebug.SelectedIndex = 0;
                SelectedDebug_SelectionChangeCommitted(null, null);

                DebugInfo.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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

            if (length > 0) return size;
            else return "No usage.";
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
                return Properties.Resources.w8;
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
                    if (Environment.OSVersion.Version.Major < 6)
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
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER) WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2012.");
                        else WinLogoTT.SetToolTip(WinLogo, "You're using Windows 8.");

                        return Properties.Resources.w8;
                    }
                    else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 3)
                    {
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER) WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2012 R2.");
                        else WinLogoTT.SetToolTip(WinLogo, "You're using Windows 8.1.");

                        return Properties.Resources.w8;
                    }
                    else if (Environment.OSVersion.Version.Major == 10)
                    {
                        if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER) WinLogoTT.SetToolTip(WinLogo, "You're using Windows Server 2016.");
                        else WinLogoTT.SetToolTip(WinLogo, "You're using Windows 10.");

                        return Properties.Resources.w8;
                    }
                    else
                    {
                        WinLogoTT.SetToolTip(WinLogo, "You're using an unknown OS.");
                        return Properties.Resources.other;
                    }
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
            Process.Start(Path.GetDirectoryName(CurrentApp.RemoveGarbageCharacters()));
        }

        private void CopyToClipBoardCmd() // Copies content of window to clipboard
        {
            try
            {
                GetInfo();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(String.Format("Keppy's Synthesizer version {0}", Driver.FileVersion));
                sb.AppendLine("========= Debug information =========");
                sb.AppendLine(String.Format("Driver version: {0}", Driver.FileVersion));
                sb.AppendLine(String.Format("{0} {1}", CMALabel.Text, CMA.Text));
                sb.AppendLine(String.Format("{0} {1}", AVLabel.Text, AV.Text));
                sb.AppendLine(String.Format("{0} {1}", AvVLabel.Text, AvV.Text));
                sb.AppendLine(String.Format("{0} {1}", RTLabel.Text, RT.Text));
                sb.AppendLine(String.Format("{0} {1}", ASIOL.Text, ASIOLLabel.Text));
                sb.AppendLine(String.Format("{0} {1}", RAMUsageVLabel.Text, RAMUsageV.Text));
                sb.AppendLine(String.Format("{0} {1}", HCountVLabel.Text, HCountV.Text));
                sb.AppendLine(String.Format("{0} {1}", KSDAPILabel.Text, KSDAPI.Text));
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

        private void UpdateActiveVoicesPerChannel(StreamReader StreamDebugReader)
        {
            for (int i = 0; i <= 15; ++i) if (!ReadPipeUInt64(StreamDebugReader, String.Format("CV{0}", i), ref CHs[i])) CHs[i] = 0;
        }

        private string GetActiveVoices()
        {
            try
            {
                return String.Format("{0}", ((CHs[0] + CHs[1] + CHs[2] + CHs[3] + CHs[4] + CHs[5] + CHs[6] + CHs[7] + CHs[8] + CHs[9] + CHs[10] + CHs[11] + CHs[12] + CHs[13] + CHs[14] + CHs[15])).ToString());
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
                return String.Format("{0} V/f", ((CHs[0] + CHs[1] + CHs[2] + CHs[3] + CHs[4] + CHs[5] + CHs[6] + CHs[7] + CHs[8] + CHs[9] + CHs[10] + CHs[11] + CHs[12] + CHs[13] + CHs[14] + CHs[15]) / 16.6666666666667f).ToString("0.00"));
            }
            catch
            {
                return "0 V/f";
            }
        }

        private string DebugName(string value)
        {
            int A = value.IndexOf(" = ");
            if (A == -1) return "";
            return value.Substring(0, A);
        }

        private string DebugValue(string value)
        {
            int A = value.LastIndexOf(" = ");
            if (A == -1) return "";
            int A2 = A + (" = ").Length;
            if (A2 >= value.Length) return "";
            return value.Substring(A2);
        }

        private bool ReadPipeKSDAPI(StreamReader StreamDebugReader)
        {
            try
            {
                string temp = StreamDebugReader.ReadLine();
          
                if (DebugName(temp).Equals("KSDirect")) KSDAPIStatus = Convert.ToBoolean(Convert.ToInt32(DebugValue(temp)));
                return true;
            }
            catch { return false; }
        }

        private bool ReadPipeString(StreamReader StreamDebugReader, String RequestedValue, ref String ValueToChange)
        {
            try
            {
                string temp = StreamDebugReader.ReadLine();
                if (DebugName(temp).Equals(RequestedValue)) ValueToChange = DebugValue(temp); 
                return true;
            }
            catch { return false; }
        }

        private bool ReadPipeSingle(StreamReader StreamDebugReader, String RequestedValue, ref Single ValueToChange)
        {
            try
            {
                string temp = StreamDebugReader.ReadLine();
                if (DebugName(temp).Equals(RequestedValue)) ValueToChange = Convert.ToSingle(DebugValue(temp)) / 1000000.0f;
                return true;
            }
            catch { return false; }
        }

        private bool ReadPipeDouble(StreamReader StreamDebugReader, String RequestedValue, ref Double ValueToChange)
        {
            try
            {
                string temp = StreamDebugReader.ReadLine();
                if (DebugName(temp).Equals(RequestedValue)) ValueToChange = Convert.ToDouble(DebugValue(temp)) / 1000000.0;
                return true;
            }
            catch { return false; }
        }

        private bool ReadPipeUInt64(StreamReader StreamDebugReader, String RequestedValue, ref UInt64 ValueToChange)
        {
            try
            {
                string temp = StreamDebugReader.ReadLine();
                if (DebugName(temp).Equals(RequestedValue)) ValueToChange = Convert.ToUInt64(DebugValue(temp));
                return true;
            }
            catch { return false; }
        }

        String CurrentApp = "Nothing";
        String BitApp = "N/A";
        Single CurCPU = 0.0f;
        UInt64 Handles = 0;
        UInt64 RAMUsage = 0;
        Boolean KSDAPIStatus = false;
        Double Td1 = 0.0;
        Double Td2 = 0.0;
        Double Td3 = 0.0;
        Double Td4 = 0.0;
        Double ASIOInLat = 0;
        Double ASIOOutLat = 0;
        private void ParseInfoFromPipe(StreamReader StreamDebugReader)
        {
            try
            {
                if (!ReadPipeString(StreamDebugReader, "CurrentApp", ref CurrentApp)) CurrentApp = "Nothing";
                if (!ReadPipeString(StreamDebugReader, "BitApp", ref BitApp)) BitApp = "N/A";
                if (!ReadPipeSingle(StreamDebugReader, "CurCPU", ref CurCPU)) CurCPU = 0.0f;
                if (!ReadPipeUInt64(StreamDebugReader, "Handles", ref Handles)) Handles = 0;
                if (!ReadPipeUInt64(StreamDebugReader, "RAMUsage", ref RAMUsage)) RAMUsage = 0;
                if (!ReadPipeKSDAPI(StreamDebugReader)) KSDAPIStatus = false;
                if (!ReadPipeDouble(StreamDebugReader, "Td1", ref Td1)) Td1 = 0.0f;
                if (!ReadPipeDouble(StreamDebugReader, "Td2", ref Td2)) Td2 = 0.0f;
                if (!ReadPipeDouble(StreamDebugReader, "Td3", ref Td3)) Td3 = 0.0f;
                if (!ReadPipeDouble(StreamDebugReader, "Td4", ref Td4)) Td4 = 0.0f;
                if (!ReadPipeDouble(StreamDebugReader, "ASIOInLat", ref ASIOInLat)) ASIOInLat = 0.0f;
                if (!ReadPipeDouble(StreamDebugReader, "ASIOOutLat", ref ASIOOutLat)) ASIOOutLat = 0.0f;
                UpdateActiveVoicesPerChannel(StreamDebugReader);
            }
            catch (Exception ex)
            {
                // If something goes wrong, here's an error handler
                MessageBox.Show(ex.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
        }

        private bool DoesPipeStillExist(int requestedpipe)
        {
            try
            {
                String PipeToAdd;
                WIN32_FIND_DATA lpFindFileData;

                var ptr = FindFirstFile(@"\\.\pipe\*", out lpFindFileData);
                PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                if (PipeToAdd.Contains(String.Format("KSDEBUG{0}", requestedpipe))) return true;

                while (FindNextFile(ptr, out lpFindFileData))
                {
                    PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                    if (PipeToAdd.Contains(String.Format("KSDEBUG{0}", requestedpipe))) return true;
                }
                FindClose(ptr);

                return false;
            }
            catch { return false; }
        }

        static string NoPipes = "No pipes available";
        private void CheckDebugPorts()
        {
            List<String> KSPipesCheck = new List<String>();

            Int32 PreviousCount = 1;
            String PreviousItem = NoPipes;

            if (KSPipes.Count > 0)
            {
                PreviousCount = KSPipes.Count;
                PreviousItem = KSPipes[0];
            }

            try
            {
                String PipeToAdd;
                WIN32_FIND_DATA lpFindFileData;

                var ptr = FindFirstFile(@"\\.\pipe\*", out lpFindFileData);
                PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                if (PipeToAdd.Contains("KSDEBUG"))
                    KSPipesCheck.Add(PipeToAdd);

                while (FindNextFile(ptr, out lpFindFileData))
                {
                    PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                    if (PipeToAdd.Contains("KSDEBUG"))
                        KSPipesCheck.Add(PipeToAdd);
                }
                FindClose(ptr);

                KSPipesCheck.Sort();
            }
            catch (Exception ex)
            {
                // If something goes wrong, here's an error handler
                MessageBox.Show(ex.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }

            if (KSPipesCheck.Count < 1)
            {
                SelectedDebug.DataSource = null;
                KSPipes = new BindingList<String>() { NoPipes };
                SelectedDebug.Enabled = false;
                SelectedDebug.DataSource = KSPipes;
            }
            else
            {
                if (PreviousCount != KSPipesCheck.Count || PreviousItem == NoPipes)
                {
                    SelectedDebug.DataSource = null;
                    KSPipes = new BindingList<String>(KSPipesCheck);
                    SelectedDebug.DataSource = KSPipes;
                    if (!PreviousItem.Equals(NoPipes)) SelectedDebug.Text = PreviousItem;
                }
                SelectedDebug.Enabled = true;
            }

        }

        private void GetInfo()
        {
            if (Tabs.SelectedIndex == 0)
            {
                // Time to write all the stuff to the string builder
                if (Path.GetFileName(CurrentApp.RemoveGarbageCharacters()) == "0")
                {
                    OpenAppLocat.Enabled = false;
                    currentappreturn = "None";
                }
                else currentappreturn = System.IO.Path.GetFileName(CurrentApp.RemoveGarbageCharacters());

                if (BitApp.RemoveGarbageCharacters() == "0") bitappreturn = "...";
                else bitappreturn = BitApp.RemoveGarbageCharacters();

                HCountV.Text = String.Format("{0} handles", Handles);
                RAMUsageV.Text = GetCurrentRAMUsage(RAMUsage);
                CMA.Text = String.Format("{0} ({1})", currentappreturn, bitappreturn); // Removes garbage characters

                Int32 AVColor = (int)Math.Round((double)(100 * Convert.ToInt32(GetActiveVoices())) / Convert.ToInt32(Settings.GetValue("polyphony", "512")));

                if (Convert.ToInt32(GetActiveVoices()) > Convert.ToInt32(Settings.GetValue("polyphony", "512")))
                    AV.Font = new Font(AV.Font, FontStyle.Bold);
                else
                    AV.Font = new Font(AV.Font, FontStyle.Regular);

                AV.ForeColor = ValueBlend.GetBlendedColor(AVColor.LimitIntToRange(0, 100));
                AV.Text = GetActiveVoices();
                AvV.Text = GetAverageVoices();

                if (Convert.ToInt32(Settings.GetValue("encmode", "0")) == 1)
                {
                    RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Italic);
                    RT.Text = "Unavailable"; // If BASS is in encoding mode, BASS usage will stay at constant 100%.
                }
                else
                {
                    Int32 RTColor = (int)Math.Round((double)(100 * CurCPU) / Convert.ToInt32(Settings.GetValue("cpu", "75")));

                    if ((CurCPU > Convert.ToInt32(Settings.GetValue("cpu", "75"))) && (Convert.ToInt32(Settings.GetValue("cpu", "75")) != 0))
                    {
                        RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Bold);
                        RT.Text = String.Format("{0}% (Beyond limit!)", CurCPU.ToString("0.0"), Settings.GetValue("cpu", "75").ToString());
                    }
                    else
                    {
                        RT.Font = new System.Drawing.Font(RT.Font, System.Drawing.FontStyle.Regular);
                        RT.Text = String.Format("{0}%", CurCPU.ToString("0.0")); // Else, it'll give you the info about how many cycles it needs to work.
                    }

                    RT.ForeColor = ValueBlend.GetBlendedColor(RTColor.LimitIntToRange(0, 100));
                }

                if (Convert.ToInt32(Settings.GetValue("xaudiodisabled", "0")) == 2) ASIOL.Text = String.Format("Input {0}ms, Output {1}ms", ASIOInLat, ASIOOutLat);
                else ASIOL.Text = "Not in use.";

                KSDAPI.Text = KSDAPIStatus ? "Using KSDirect API." : "KSDirect API is inactive.";
            }
            else if (Tabs.SelectedIndex == 1)
            {
                String FormatForVoices = "{0} voices";
                CHV1.Text = String.Format(FormatForVoices, CHs[0]);
                CHV2.Text = String.Format(FormatForVoices, CHs[1]);
                CHV3.Text = String.Format(FormatForVoices, CHs[2]);
                CHV4.Text = String.Format(FormatForVoices, CHs[3]);
                CHV5.Text = String.Format(FormatForVoices, CHs[4]);
                CHV6.Text = String.Format(FormatForVoices, CHs[5]);
                CHV7.Text = String.Format(FormatForVoices, CHs[6]);
                CHV8.Text = String.Format(FormatForVoices, CHs[7]);
                CHV9.Text = String.Format(FormatForVoices, CHs[8]);
                CHV10.Text = String.Format(FormatForVoices, CHs[9]);
                CHV11.Text = String.Format(FormatForVoices, CHs[10]);
                CHV12.Text = String.Format(FormatForVoices, CHs[11]);
                CHV13.Text = String.Format(FormatForVoices, CHs[12]);
                CHV14.Text = String.Format(FormatForVoices, CHs[13]);
                CHV15.Text = String.Format(FormatForVoices, CHs[14]);
                CHV16.Text = String.Format(FormatForVoices, CHs[15]);
            }
            else if (Tabs.SelectedIndex == 2)
            {
                MTRT.Text = String.Format("{0}ms", Td1.LimitDoubleToRange(0.0, 1E7));
                AERTi.Text = String.Format("{0}ms", Td2);
                SLRT.Text = String.Format("{0}ms", Td3);
                NCRT.Text = String.Format("{0}ms", Td4);
            }
            else if (Tabs.SelectedIndex == 3)
            {
                TM.Text = String.Format("{0} ({1} bytes)", (tlmem / (1024 * 1024) + "MB").ToString(), tlmem.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")));
                AM.Text = String.Format("{0} ({1:0.#}%, {2} bytes)", (avmem / (1024 * 1024) + "MB").ToString(), Math.Round(percentage, 1).ToString(), avmem.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")));
            }
        }

        private void DebugInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentlyConnected)
                {
                    GetInfo();
                    CheckDebugPorts();
                }
                else
                {
                    try {
                        GetInfo();
                        CheckDebugPorts();
                        DebugInfoCheck.RunWorkerAsync();
                    } catch { }
                } 
            }
            catch (Exception ex)
            {
                // If something goes wrong, here's an error handler
                MessageBox.Show(ex.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
        }

        int SelectedDebugVal = 1;
        bool CurrentlyConnected = false;
        private void DebugInfoCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            using (NamedPipeClientStream PipeClient = new NamedPipeClientStream(".", String.Format("KSDEBUG{0}", SelectedDebugVal), PipeDirection.In, PipeOptions.Asynchronous))
            {
                PipeClient.Connect();
                if (PipeClient.IsConnected)
                {
                    CurrentlyConnected = true;
                    using (StreamReader StreamDebugReader = new StreamReader(PipeClient))
                    {
                        while (PipeClient.IsConnected)
                        {
                            try
                            {
                                if (DebugInfoCheck.CancellationPending) break;
                                ParseInfoFromPipe(StreamDebugReader);
                            }
                            catch (Exception ex)
                            {
                                // If something goes wrong, here's an error handler
                                MessageBox.Show(ex.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.ExitThread();
                            }
                        }
                    }
                }

                PipeClient.Dispose();
            }

            CurrentlyConnected = false;
        }

        private void DebugInfoCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DebugInfoCheck.RunWorkerAsync();
        }

        private void SelectedDebug_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SwitchPipe(false);
        }

        private void SelectedDebug_SelectedIndexChanged(object sender, EventArgs e)
        {
            SwitchPipe(true);
        }

        private void SwitchPipe(bool silent)
        {
            try
            {
                Int32 SelectedValueToCheck = Convert.ToInt32(Regex.Match((String)SelectedDebug.Items[SelectedDebug.SelectedIndex], @"\d+").Value);
                if (DoesPipeStillExist(SelectedValueToCheck))
                {
                    SelectedDebugVal = SelectedValueToCheck;
                    if (DebugInfoCheck.IsBusy) DebugInfoCheck.CancelAsync();
                    else DebugInfoCheck.RunWorkerAsync();
                }
                else { if (!silent) MessageBox.Show("This debug pipe is not available anymore.", "Keppy's Synthesizer - Info", MessageBoxButtons.OK, MessageBoxIcon.Information); }
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

        static ulong avmem = 0;
        static ulong tlmem = 0;
        static ulong avmemint = 0;
        static ulong tlmemint = 0;
        static double percentage = 0.0;
        private void CheckMem_DoWork(object sender, DoWorkEventArgs e)
        {
            ComputerInfo CI = new ComputerInfo();
            while (CI != null)
            {
                avmem = CI.AvailablePhysicalMemory;
                tlmem = CI.TotalPhysicalMemory;
                avmemint = avmem / (1024 * 1024);
                tlmemint = tlmem / (1024 * 1024);
                percentage = avmem * 100.0 / tlmem;
                Thread.Sleep(50);
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

public static class ValueBlend
{
    public static Color GetBlendedColor(int percentage)
    {
        if (percentage < 50)
            return Interpolate(Color.FromArgb(32, 150, 0), Color.FromArgb(175, 111, 0), percentage / 50.0);
        return Interpolate(Color.FromArgb(175, 111, 0), Color.FromArgb(209, 0, 31), (percentage - 50) / 50.0);
    }

    private static Color Interpolate(Color color1, Color color2, double fraction)
    {
        double r = Interpolate(color1.R, color2.R, fraction);
        double g = Interpolate(color1.G, color2.G, fraction);
        double b = Interpolate(color1.B, color2.B, fraction);
        return Color.FromArgb((int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
    }

    private static double Interpolate(double d1, double d2, double fraction)
    {
        return d1 + (d2 - d1) * fraction;
    }
}

public static class InputExtensions
{
    public static int LimitIntToRange(
        this int value, int inclusiveMinimum, int inclusiveMaximum)
    {
        if (value < inclusiveMinimum) { return inclusiveMinimum; }
        if (value > inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }

    public static double LimitDoubleToRange(
    this double value, double inclusiveMinimum, double inclusiveMaximum)
    {
        if (value < inclusiveMinimum) { return inclusiveMinimum; }
        if (value > inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }
}