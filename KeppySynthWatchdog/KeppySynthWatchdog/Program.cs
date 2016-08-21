using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace KeppySynthWatchdog
{
    static class Program
    {
        static RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);

        [STAThread]
        static void Main()
        {
            if (Convert.ToInt32(Watchdog.GetValue("watchdog")) == 1)
            {
                if (Convert.ToInt32(Watchdog.GetValue("wdrun")) == 1)
                {
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
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Exit();
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Exit();
            }
        }
    }

    public class ProcessIcon : IDisposable
    {
        static RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);

        NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {
            if (Convert.ToInt32(Watchdog.GetValue("watchdognotify", 1)) == 1)
            {
                string currentapp = Watchdog.GetValue("currentapp", "Not available").ToString();
                string bit = Watchdog.GetValue("bit", "Unknown").ToString();
                ni.BalloonTipIcon = ToolTipIcon.Info;
                ni.BalloonTipTitle = "Keppy's Synthesizer is up and running";
                ni.BalloonTipText = String.Format("Current MIDI app:\n{0} ({1})", System.IO.Path.GetFileName(currentapp.RemoveGarbageCharacters()), bit.RemoveGarbageCharacters());
            }

            ni.MouseClick += new MouseEventHandler(ni_MouseClick);

            ni.Text = "Keppy's Synthesizer Watchdog";
            ni.Icon = KeppySynthWatchdog.Properties.Resources.gear;
            ni.Visible = true;

            if (Convert.ToInt32(Watchdog.GetValue("watchdognotify", 1)) == 1)
                ni.ShowBalloonTip(30000);

            ni.ContextMenu = new ContextMenus().Create();
        }

        public void Dispose()
        {
            ni.Icon = null;
            ni.Dispose();
            System.Windows.Forms.Application.DoEvents();
        }

        public void Exit(object sender, EventArgs e)
        {
            ni.Visible = false;

            Environment.Exit(-1);
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