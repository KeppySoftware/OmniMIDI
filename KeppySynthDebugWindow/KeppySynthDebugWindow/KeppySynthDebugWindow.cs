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
using Microsoft.Win32;


namespace KeppySynthDebugWindow
{
    public partial class KeppySynthDebugWindow : Form
    {
        private static KeppySynthDebugWindow inst;
        public static FileVersionInfo Driver { get; set; }

        public StringBuilder sb = new StringBuilder();

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        public static KeppySynthDebugWindow GetForm
        {
            get
            {
                if (inst == null || inst.IsDisposed)
                {
                    inst = new KeppySynthDebugWindow();
                }
                else
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    Application.OpenForms["KeppySynthDebugWindow"].BringToFront();
                }
                return inst;
            }
        }

        public KeppySynthDebugWindow()
        {
            InitializeComponent();
        }

        private void KeppySynthDebugWindow_Load(object sender, EventArgs e)
        {
            Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll");
            ContextMenu = MainCont;
            richTextBox1.ContextMenu = MainCont;
        }

        private void DebugRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                Process thisProc = Process.GetCurrentProcess();
                thisProc.PriorityClass = ProcessPriorityClass.Idle;
                RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer", false);
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", false);
                RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", false);
                RegistryKey WinVer = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
                string FullVersion = Environment.OSVersion.Version.Major.ToString() + "." + Environment.OSVersion.Version.Minor.ToString() + "." + Environment.OSVersion.Version.Build.ToString();
                string bit;

                string currentapp = Watchdog.GetValue("currentapp", "Not available").ToString();
                string bitapp = Watchdog.GetValue("bit", "Unknown").ToString();

                if (currentapp == "") {
                    OpenAppLocat.Enabled = false;
                    currentapp = "Not loaded yet";
                }
                else
                {
                    OpenAppLocat.Enabled = true;
                }
                if (bitapp == "")
                    bitapp = "N/A";

                if (Environment.Is64BitOperatingSystem == true)
                    bit = "x64";
                else
                    bit = "x86";

                ComputerInfo CI = new ComputerInfo();
                ulong avmem = ulong.Parse(CI.AvailablePhysicalMemory.ToString());
                ulong tlmem = ulong.Parse(CI.TotalPhysicalMemory.ToString());
                int avmemint = Convert.ToInt32(avmem / (1024 * 1024));
                int tlmemint = Convert.ToInt32(tlmem / (1024 * 1024));
                double percentage = avmem * 100.0 / tlmem;

                try
                {
                    richTextBox1.Suspend();
                    sb.Clear();
                    richTextBox1.Clear();

                    sb.Append(String.Format("Keppy's Synthesizer Debug Window - Version {0}", Driver.FileVersion.ToString()));
                    sb.Append(Environment.NewLine);
                    sb.Append("---------------------------------------------------------");
                    sb.Append(Environment.NewLine);
                    sb.Append(String.Format("Operating system: {0} ({1}, {2})", (string)WinVer.GetValue("ProductName"), FullVersion, bit));
                    sb.Append(Environment.NewLine);
                    sb.Append(String.Format("Total memory: {0}", (tlmem / (1024 * 1024) + "MB").ToString()));
                    sb.Append(Environment.NewLine);
                    sb.Append(String.Format("Available memory: {0} ({1}% available)", (avmem / (1024 * 1024) + "MB").ToString(), Math.Round(percentage, 1).ToString()));
                    sb.Append(Environment.NewLine);
                    sb.Append("---------------------------------------------------------");
                    sb.Append(Environment.NewLine);
                    sb.Append(String.Format("Current MIDI app: {0} ({1})", System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters()), bitapp.RemoveGarbageCharacters()));
                    sb.Append(Environment.NewLine);
                    sb.Append(String.Format("Active voices: {0}", Debug.GetValue("currentvoices0").ToString()));
                    sb.Append(Environment.NewLine);
                    if (Convert.ToInt32(Settings.GetValue("encmode")) == 1)
                    {
                        sb.Append("BASS CPU usage: Unavailable");
                    }
                    else
                    {
                        sb.Append(String.Format("Rendering time: {0}%", Debug.GetValue("currentcpuusage0").ToString()));
                    }
                    if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 0)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append(String.Format("Decoded data size (bytes): {0}", Debug.GetValue("int").ToString()));
                    }
                }
                finally
                {
                    Debug.Close();
                    Settings.Close();
                    Watchdog.Close();
                    WinVer.Close();
                    richTextBox1.Text = sb.ToString();
                    richTextBox1.Resume();
                    richTextBox1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void OpenAppLocat_Click(object sender, EventArgs e)
        {
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", false);
            string currentapp = Watchdog.GetValue("currentapp", "Not available").ToString();
            Process.Start(System.IO.Path.GetDirectoryName(currentapp.RemoveGarbageCharacters()));
            Watchdog.Close();
        }

        private void CopyToClipboard_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string line in richTextBox1.Lines) { sb.AppendLine(line); }

            Thread thread = new Thread(() => Clipboard.SetText(sb.ToString()));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            MessageBox.Show("Info copied to clipboard.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }
    }

    public static class ControlExtensions
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static void Suspend(this Control control)
        {
            LockWindowUpdate(control.Handle);
        }

        public static void Resume(this Control control)
        {
            LockWindowUpdate(IntPtr.Zero);
        }

    }
}

public static class RegexConvert
{
    public static string RemoveGarbageCharacters(this string input)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9()!'-_.\\\\ ]");
        return rgx.Replace(input, "");
    }
}
