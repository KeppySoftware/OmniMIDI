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


namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverDebugWindow : Form
    {
        private static KeppyDriverDebugWindow inst;
        public static FileVersionInfo Driver { get; set; }

        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        public static KeppyDriverDebugWindow GetForm
        {
            get
            {
                if (inst == null || inst.IsDisposed)
                {
                    inst = new KeppyDriverDebugWindow();
                }
                else
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    Application.OpenForms["KeppyDriverDebugWindow"].BringToFront();
                }
                return inst;
            }
        }

        public KeppyDriverDebugWindow()
        {
            InitializeComponent();
        }

        private void KeppyDriverDebugWindow_Load(object sender, EventArgs e)
        {
            Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppydrv\\keppydrv.dll");
        }

        private void DebugRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver", false);
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", false);
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
                    richTextBox1.Clear();

                    richTextBox1.AppendText("Keppy's Driver Debug Window - Version " + Driver.FileVersion.ToString());
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("---------------------------------------------------------");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("Operating system: " + (string)WinVer.GetValue("ProductName") + " (" + FullVersion + bit + ")");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("Total memory: " + (tlmem / (1024 * 1024) + "MB").ToString());
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("Available memory: " + (avmem / (1024 * 1024) + "MB").ToString() + " (" + Math.Round(percentage, 1).ToString() + "% available)");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("---------------------------------------------------------");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("Active voices: " + Debug.GetValue("currentvoices0").ToString());
                    richTextBox1.AppendText(Environment.NewLine);
                    if (Convert.ToInt32(Settings.GetValue("encmode")) == 1)
                    {
                        richTextBox1.AppendText("BASS CPU usage: Unavailable");
                    }
                    else
                    {
                        if (Convert.ToInt32(Debug.GetValue("buffull")) == 0) {
                            richTextBox1.AppendText("Rendering time: " + Debug.GetValue("currentcpuusage0").ToString() + "%");
                        }
                        else
                        {
                            richTextBox1.AppendText("Rendering time: " + Debug.GetValue("currentcpuusage0").ToString() + "% (Buffer is full)");
                        }
                           
                    }

                    if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 1)
                    {

                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine);
                        richTextBox1.AppendText("Decoded data size (bytes): " + Debug.GetValue("int").ToString());
                    }
                }
                finally
                {
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
