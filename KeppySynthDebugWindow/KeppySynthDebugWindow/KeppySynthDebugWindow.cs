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
        }

        private void DebugRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                Process thisProc = Process.GetCurrentProcess();
                thisProc.PriorityClass = ProcessPriorityClass.Idle;
                RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer", false);
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", false);
                RegistryKey WinVer = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
                string FullVersion = Environment.OSVersion.Version.Major.ToString() + "." + Environment.OSVersion.Version.Minor.ToString() + "." + Environment.OSVersion.Version.Build.ToString();
                string bit;

                if (Environment.Is64BitOperatingSystem == true)
                    bit = ", x64";
                else
                    bit = ", x86";

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

                    sb.Append("Keppy's Synthesizer Debug Window - Version " + Driver.FileVersion.ToString());
                    sb.Append(Environment.NewLine);
                    sb.Append("---------------------------------------------------------");
                    sb.Append(Environment.NewLine);
                    sb.Append("Operating system: " + (string)WinVer.GetValue("ProductName") + " (" + FullVersion + bit + ")");
                    sb.Append(Environment.NewLine);
                    sb.Append("Total memory: " + (tlmem / (1024 * 1024) + "MB").ToString());
                    sb.Append(Environment.NewLine);
                    sb.Append("Available memory: " + (avmem / (1024 * 1024) + "MB").ToString() + " (" + Math.Round(percentage, 1).ToString() + "% available)");
                    sb.Append(Environment.NewLine);
                    sb.Append("---------------------------------------------------------");
                    sb.Append(Environment.NewLine);
                    sb.Append("Active voices: " + Debug.GetValue("currentvoices0").ToString());
                    sb.Append(Environment.NewLine);
                    if (Convert.ToInt32(Settings.GetValue("encmode")) == 1)
                    {
                        sb.Append("BASS CPU usage: Unavailable");
                    }
                    else
                    {
                        if (Convert.ToInt32(Debug.GetValue("buffull")) == 0)
                        {
                            sb.Append("Rendering time: " + Debug.GetValue("currentcpuusage0").ToString() + "%");
                        }
                        else
                        {
                            sb.Append("Rendering time: " + Debug.GetValue("currentcpuusage0").ToString() + "% (Buffer is full)");
                        }

                    }

                    if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 1)
                    {

                    }
                    else
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append("Decoded data size (bytes): " + Debug.GetValue("int").ToString());
                    }
                }
                finally
                {
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
