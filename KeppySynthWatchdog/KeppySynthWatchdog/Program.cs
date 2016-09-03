using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace KeppySynthWatchdog
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
            if (Convert.ToInt32(Watchdog.GetValue("watchdog")) == 1)
            {
                if (Convert.ToInt32(Watchdog.GetValue("wdrun")) == 1)
                {
                    Watchdog.Close();
                    bool ok;
                    Mutex m = new Mutex(true, "KeppySynthWatchdog", out ok);
                    if (!ok)
                    {
                        return;
                    }
                    NotifyIcon ni;
                    ni = new NotifyIcon();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    using (ProcessIcon pi = new ProcessIcon())
                    {
                        pi.Display();
                        Application.Run();
                    }
                    GC.KeepAlive(m);
                }
                else
                {
                    Watchdog.Close();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.ExitThread();
                }
            }
            else
            {
                Watchdog.Close();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ExitThread();
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
        }
    }

    public class ProcessIcon : IDisposable
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        public static NotifyIcon ni;

        public void Display()
        {
            ni = new NotifyIcon();
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
            if (Convert.ToInt32(Watchdog.GetValue("watchdognotify", 1)) == 1)
            {
                string currentapp = Watchdog.GetValue("currentapp", "Not available").ToString();
                string bit = Watchdog.GetValue("bit", "Unknown").ToString();

                if (System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters()) == "")
                {
                    ni.BalloonTipClicked += ni_BalloonTipClicked;
                    ni.BalloonTipIcon = ToolTipIcon.Warning;
                    ni.BalloonTipTitle = "Keppy's Synthesizer encountered an issue";
                    ni.BalloonTipText = "Can not get the current MIDI app name.";
                }
                else
                {
                    ni.BalloonTipIcon = ToolTipIcon.Info;
                    ni.BalloonTipTitle = "Keppy's Synthesizer is up and running";
                    ni.BalloonTipText = String.Format("Current MIDI app:\n{0} ({1})", System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters()), bit.RemoveGarbageCharacters());
                }
            }

            ni.MouseClick += new MouseEventHandler(ni_MouseClick);

            ni.Text = "Keppy's Synthesizer ~ Watchdog";
            ni.Icon = KeppySynthWatchdog.Properties.Resources.gear;
            ni.Visible = true;

            if (Convert.ToInt32(Watchdog.GetValue("watchdognotify", 1)) == 1)
                ni.ShowBalloonTip(30000);

            Watchdog.Close();

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(ContextMenus.CheckPop);
            aTimer.Interval = 1;
            aTimer.Enabled = true;

            System.Timers.Timer bTimer = new System.Timers.Timer();
            bTimer.Elapsed += new ElapsedEventHandler(ContextMenus.CheckPop);
            bTimer.Interval = 1;
            bTimer.Enabled = true;

            ni.ContextMenu = new ContextMenus().Create();
        }

        void CleanGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        void ni_BalloonTipClicked(object sender, EventArgs e)
        {
            string currentpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Process.Start(currentpath + "\\KeppySynthConfigurator.exe", null);
        }

        public void Dispose()
        {
            ni.Icon = null;
            ni.Visible = false;
            ni.Dispose();
            System.Windows.Forms.Application.DoEvents();
            Application.Exit();
            Environment.Exit(0);
        }

        public void Exit(object sender, EventArgs e)
        {
            ni.Visible = false;
            Application.Exit();
            Environment.Exit(0);
        }

        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string currentpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Process.Start(currentpath + "\\KeppySynthConfigurator.exe", null);
            }
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