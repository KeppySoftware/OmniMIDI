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
using Microsoft.Win32;

namespace KeppySynthDebugWindow
{
    public partial class KeppySynthDebugWindow : Form
    {
        public static FileVersionInfo Driver { get; set; }

        public KeppySynthDebugWindow()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true); // AAAAA I hate flickering
            Form.CheckForIllegalCrossThreadCalls = false; // Didn't want to bother making a delegate, this works too.
        }

        private void KeppySynthDebugWindow_Load(object sender, EventArgs e)
        {
            Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll"); // Gets Keppy's Synthesizer version
            ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the form
            richTextBox1.ContextMenu = MainCont; // Assign ContextMenu (Not the strip one) to the richtextbox
            DebugWorker.RunWorkerAsync(); // Creates a thread to show the info
        }

        private void OpenAppLocat_Click(object sender, EventArgs e) // Opens the directory of the current app that's using Keppy's Synthesizer
        {
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", false);
            string currentapp = Watchdog.GetValue("currentapp", "Not available").ToString();
            Process.Start(System.IO.Path.GetDirectoryName(currentapp.RemoveGarbageCharacters()));
            Watchdog.Close();
        }

        private void CopyToClipboard_Click(object sender, EventArgs e) // Allows you to copy the content of the richtextbox to clipboard
        {
            StringBuilder sb = new StringBuilder();

            foreach (string line in richTextBox1.Lines) { sb.AppendLine(line); }

            Thread thread = new Thread(() => Clipboard.SetText(sb.ToString())); // Creates another thread, otherwise the form locks up while copying the richtextbox
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            MessageBox.Show("Info copied to clipboard.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information); // Done, now get out
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
                    StringBuilder sb = new StringBuilder(); // Creates a string builder, because adding lines one by one to the richtextbox is unefficient
                    Process thisProc = Process.GetCurrentProcess(); // Go to the next function for an explanation
                    thisProc.PriorityClass = ProcessPriorityClass.Idle; // Tells Windows that the process doesn't require a lot of resources
                    RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer", false);
                    RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", false);
                    RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", false);
                    RegistryKey WinVer = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
                    string FullVersion = Environment.OSVersion.Version.Major.ToString() + "." + Environment.OSVersion.Version.Minor.ToString() + "." + Environment.OSVersion.Version.Build.ToString();
                    string bit;

                    string currentapp = Watchdog.GetValue("currentapp", "Not available").ToString(); // Gets app's name. If the name of the app is invalid, it'll return "Not available"
                    string bitapp = Watchdog.GetValue("bit", "Unknown").ToString(); // Gets app's architecture. If the app doesn't return a value, it'll return "Unknown"

                    // This happens when there's no app using the driver
                    if (currentapp == "")
                    {
                        OpenAppLocat.Enabled = false;
                        currentapp = "Not loaded yet";
                    }
                    else { OpenAppLocat.Enabled = true; }
                    if (bitapp == "") { bitapp = "N/A"; }
                    // This happens when there's no app using the driver

                    if (Environment.Is64BitOperatingSystem == true) { bit = "x64"; } else {  bit = "x86"; }  // Gets Windows architecture              

                    // Some info about the computer
                    ComputerInfo CI = new ComputerInfo();
                    ulong avmem = ulong.Parse(CI.AvailablePhysicalMemory.ToString());
                    ulong tlmem = ulong.Parse(CI.TotalPhysicalMemory.ToString());
                    int avmemint = Convert.ToInt32(avmem / (1024 * 1024));
                    int tlmemint = Convert.ToInt32(tlmem / (1024 * 1024));
                    double percentage = avmem * 100.0 / tlmem;
                    // Some info about the computer

                    try
                    {
                        // Time to write all the stuff to the string builder
                        sb.Append(String.Format("Keppy's Synthesizer Debug Window - Version {0}", Driver.FileVersion.ToString()));
                        sb.Append(Environment.NewLine);
                        sb.Append("---------------------------------------------------------"); // MINUSMINUSMINUSMINUSMINUSMINUS
                        sb.Append(Environment.NewLine);
                        sb.Append(String.Format("O.S.: {0} ({1}, {2})", (string)WinVer.GetValue("ProductName"), FullVersion, bit));
                        sb.Append(Environment.NewLine);
                        sb.Append(String.Format("Total memory: {0}", (tlmem / (1024 * 1024) + "MB").ToString()));
                        sb.Append(Environment.NewLine);
                        sb.Append(String.Format("Available memory: {0} ({1}% available)", (avmem / (1024 * 1024) + "MB").ToString(), Math.Round(percentage, 1).ToString()));
                        sb.Append(Environment.NewLine);
                        sb.Append("---------------------------------------------------------"); // MINUSMINUSMINUSMINUSMINUSMINUSx2
                        sb.Append(Environment.NewLine);
                        sb.Append(String.Format("Current MIDI app: {0} ({1})", System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters()), bitapp.RemoveGarbageCharacters())); // Removes garbage characters
                        sb.Append(Environment.NewLine);
                        sb.Append(String.Format("Active voices: {0}", Debug.GetValue("currentvoices0").ToString())); // Get current active voices
                        sb.Append(Environment.NewLine);
                        if (Convert.ToInt32(Settings.GetValue("encmode")) == 1)
                        {
                            sb.Append("BASS CPU usage: Unavailable"); // If BASS is in encoding mode, BASS usage will stay at constant 100%.
                        }
                        else
                        {
                            if (Convert.ToInt32(Debug.GetValue("currentcpuusage0").ToString()) > Convert.ToInt32(Settings.GetValue("cpu", "75").ToString()) && Settings.GetValue("cpu", "75").ToString() != "0")
                                sb.Append(String.Format("Rendering time: {0}% (Beyond limit: {1}%)", Debug.GetValue("currentcpuusage0").ToString(), Settings.GetValue("cpu", "75").ToString()));
                            else
                                sb.Append(String.Format("Rendering time: {0}%", Debug.GetValue("currentcpuusage0").ToString())); // Else, it'll give you the info about how many cycles it needs to work.
                        }
                        if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 0)
                        {
                            // If you're using XAudio, it'll show you the size of a frame.
                            sb.Append(Environment.NewLine);
                            sb.Append(String.Format("Decoded data size (bytes): {0} ({1} x 4)", Debug.GetValue("int").ToString(), (Convert.ToInt32(Debug.GetValue("int").ToString()) / 4).ToString()));
                        }
                    }
                    finally
                    {
                        // Ok everything's done, let's close the registry keys to avoid memory leaks, and let's put the string builder output to the richboxtext.
                        Debug.Close();
                        Settings.Close();
                        Watchdog.Close();
                        WinVer.Close();
                        richTextBox1.Text = sb.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // If something goes wrong, here's an error handler
                    MessageBox.Show(ex.Message.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.ExitThread();
                }
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("KeppySynthConfigurator").Length > 0)
            {
                MessageBox.Show("The configurator is already opened!", "Keppy's Synthesizer Debug Window - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthConfigurator.exe");
            }
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
