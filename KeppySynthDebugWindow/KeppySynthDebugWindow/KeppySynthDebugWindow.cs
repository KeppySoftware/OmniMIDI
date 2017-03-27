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
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;

namespace KeppySynthDebugWindow
{
    public partial class KeppySynthDebugWindow : Form
    {
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
        ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("SELECT * FROM CIM_Processor");
        ManagementObjectSearcher mosGPU = new ManagementObjectSearcher("SELECT * FROM CIM_VideoController");
        ManagementObjectSearcher mosEnc = new ManagementObjectSearcher("SELECT * FROM CIM_Chassis");
        string cpubit = "32";
        int cpuclock = 0;
        string cpumanufacturer = "Unknown";
        string cpuname = "Unknown";
        string gpuchip = "Unknown";
        string gpuname = "Unknown";
        string gpuver = "N/A";
        string gpuvram = "0";
        string enclosure = "Unknown";
        int coreCount = 0;

        public KeppySynthDebugWindow()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true); // AAAAA I hate flickering
            Form.CheckForIllegalCrossThreadCalls = false; // Didn't want to bother making a delegate, this works too.
        }

        private void KeppySynthDebugWindow_Load(object sender, EventArgs e)
        {
            Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll"); // Gets Keppy's Synthesizer version
            GetWindowsInfoData(); // Get info about your Windows installation
            SynthDbg.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
            PCSpecs.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the tab
            DebugWorker.RunWorkerAsync(); // Creates a thread to show the info
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

        private string CPUArch(int Value)
        {
            if (Value == 0)
                return "x86";
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
                OSInfo.OSVERSIONINFOEX osVersionInfo = new OSInfo.OSVERSIONINFOEX();
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSInfo.OSVERSIONINFOEX));
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
                foreach (ManagementObject moGPU in mosGPU.Get())
                {
                    try
                    {
                        gpuchip = moGPU["VideoProcessor"].ToString();
                        gpuname = moGPU["Name"].ToString();
                        gpuvram = (long.Parse(moGPU["AdapterRAM"].ToString()) / 1048576).ToString();
                        gpuver = moGPU["DriverVersion"].ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
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

                if (!OSInfo.GetVersionEx(ref osVersionInfo))
                {
                    WinLogo.Image = Properties.Resources.unknown;
                }
                else
                {
                    int p = (int)Environment.OSVersion.Platform;
                    if ((p == 4) || (p == 6) || (p == 128))
                        WinLogo.Image = Properties.Resources.other;
                    else
                    {
                        if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0)
                            WinLogo.Image = Properties.Resources.wvista;
                        else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                            WinLogo.Image = Properties.Resources.w7;
                        else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 2)
                        {
                            if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                                WinLogo.Image = Properties.Resources.ws2012;
                            else
                                WinLogo.Image = Properties.Resources.w8;
                        }
                        else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 3)
                        {
                            if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                                WinLogo.Image = Properties.Resources.ws2012;
                            else
                                WinLogo.Image = Properties.Resources.w81;
                        }
                        else if (Environment.OSVersion.Version.Major == 10)
                        {
                            if (osVersionInfo.wProductType == OSInfo.VER_NT_SERVER)
                                WinLogo.Image = Properties.Resources.ws2016;
                            else
                                WinLogo.Image = Properties.Resources.w10;
                        }
                        else
                            WinLogo.Image = Properties.Resources.unknown;
                    }
                }

                if (cpumanufacturer == "GenuineIntel")
                    CPULogo.Image = Properties.Resources.intel;
                else if (cpumanufacturer == "AuthenticAMD")
                    CPULogo.Image = Properties.Resources.amd;
                else if (cpumanufacturer == "CentaurHauls" || cpumanufacturer == "VIA VIA VIA ")
                    CPULogo.Image = Properties.Resources.via;
                else if (cpumanufacturer == "VMwareVMware")
                    CPULogo.Image = Properties.Resources.vmware;
                else if (cpumanufacturer == " lrpepyh vr")
                    CPULogo.Image = Properties.Resources.parallels;
                else if (cpumanufacturer == "KVMKVMKVM" || cpumanufacturer.Contains("KVMKVMKVM"))
                    CPULogo.Image = Properties.Resources.kvm;
                else if (cpumanufacturer == "Microsoft Hv")
                    CPULogo.Image = Properties.Resources.ws2012;
                else
                    CPULogo.Image = Properties.Resources.unknown;

                if (Environment.Is64BitOperatingSystem == true) { bit = "AMD64"; } else { bit = "i386"; }  // Gets Windows architecture  

                if (cpuclock < 1000)
                    Frequency = String.Format("{0}MHz", cpuclock);
                else
                    Frequency = String.Format("{0}GHz", ((float)cpuclock / 1000).ToString("0.00"));

                COS.Text = String.Format("{0}{1} ({2}, {3})", OSInfo.GetOSName(), OSInfo.GetOSProductType(), FullVersion, bit);
                CPU.Text = String.Format("{0} ({1} processor)", cpuname, cpubit);
                CPUInfo.Text = String.Format("Made by {0}, {1} cores and {2} threads, {3}", cpumanufacturer, coreCount, Environment.ProcessorCount, Frequency);
                GPU.Text = gpuname;
                GPUInternalChip.Text = gpuchip;
                GPUInfo.Text = String.Format("{0}MB VRAM, driver version {1}", gpuvram, gpuver);
                MT.Text = enclosure;
            }
            catch { }
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
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(String.Format("Keppy's Synthesizer version {0}", Driver.FileVersion));
                sb.AppendLine("========= Debug information =========");
                sb.AppendLine(String.Format("Driver version: {0}", Driver.FileVersion));
                sb.AppendLine(String.Format("{0} {1}", CMALabel.Text, CMA.Text));
                sb.AppendLine(String.Format("{0} {1}", AVLabel.Text, AV.Text));
                sb.AppendLine(String.Format("{0} {1}", RTLabel.Text, RT.Text));
                sb.AppendLine(String.Format("{0} {1}", DDSLabel.Text, DDS.Text));
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

        private void CopyToClipboard_Click(object sender, EventArgs e) // Allows you to copy the content of the richtextbox to clipboard
        {
            CopyToClipBoardCmd();
        }

        private void CopyToClip_Click(object sender, EventArgs e) // Allows you to copy the content of the richtextbox to clipboard
        {
            CopyToClipBoardCmd();
        }

        private void Exit_Click(object sender, EventArgs e) // Exit? lel
        {
            Application.ExitThread(); // R.I.P. debug
        }

        private void DebugWorker_DoWork(object sender, DoWorkEventArgs e) // The worker
        {
            while (true)
            {
                System.Threading.Thread.Sleep(100); // Let it sleep, otherwise it'll eat all ya CPU resources :P
                try
                {                 
                    Process thisProc = Process.GetCurrentProcess(); // Go to the next function for an explanation
                    thisProc.PriorityClass = ProcessPriorityClass.Idle; // Tells Windows that the process doesn't require a lot of resources     
                    string currentapp = Watchdog.GetValue("currentapp", "None").ToString(); // Gets app's name. If the name of the app is invalid, it'll return "Not available"
                    string bitapp = Watchdog.GetValue("bit", "...").ToString(); // Gets app's architecture. If the app doesn't return a value, it'll return "Unknown"
                    int sndbfvalue = Convert.ToInt32(Settings.GetValue("sndbfvalue", 0)); // Size of the decoded data, in bytes
                    string currentappreturn;
                    string bitappreturn;
                    try
                    {
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
                        CMA.Text = String.Format("{0} ({1})", currentappreturn, bitappreturn); // Removes garbage characters
                        AV.Text = String.Format("{0}", Debug.GetValue("currentvoices0", "0").ToString()); // Get current active voices
                        if (Convert.ToInt32(Settings.GetValue("encmode", "0")) == 1)
                        {
                            RT.Text = "Unavailable"; // If BASS is in encoding mode, BASS usage will stay at constant 100%.
                        }
                        else
                        {
                            if (Convert.ToInt32(Debug.GetValue("currentcpuusage0", "0").ToString()) > Convert.ToInt32(Settings.GetValue("cpu", "75").ToString()) && Settings.GetValue("cpu", "75").ToString() != "0")
                                RT.Text = String.Format("{0}% (Beyond limit: {1}%)", Debug.GetValue("currentcpuusage0").ToString(), Settings.GetValue("cpu", "75").ToString());
                            else
                                RT.Text = String.Format("{0}%", Debug.GetValue("currentcpuusage0", "0").ToString()); // Else, it'll give you the info about how many cycles it needs to work.
                        }
                        if (Convert.ToInt32(Settings.GetValue("xaudiodisabled", "0")) == 1)
                        {
                            DDSLabel.Enabled = false;
                            DDS.Enabled = false;
                            DDS.Text = "Unavailable";
                        }
                        else
                        {
                            DDSLabel.Enabled = true;
                            DDS.Enabled = true;
                            DDS.Text = String.Format("{0} ({1} x 4)", (sndbfvalue * 4), sndbfvalue);
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
        }

        private void MemoryThread_Tick(object sender, EventArgs e)
        {
            try
            {
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
            catch { }
        }

        private void OpenConfigurator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthConfigurator.exe");
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

        private void debugwintop_Click(object sender, EventArgs e)
        {
            if (debugwintop.Checked)
            {
                debugwintop.Checked = false;
                TopMost = false;
            }
            else
            {
                debugwintop.Checked = true;
                TopMost = true;
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
