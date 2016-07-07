using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace KeppyDriverWatchdog
{
    static class Program
    {
        static RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Watchdog", true);

        [STAThread]
        static void Main()
        {
            if (Convert.ToInt32(Watchdog.GetValue("watchdog")) == 1)
            {
                if (Convert.ToInt32(Watchdog.GetValue("wdrun")) == 1)
                {
                    bool ok;
                    Mutex m = new Mutex(true, "KeppyDriverWatchdog", out ok);
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
        NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Text = "Keppy's Driver Watchdog";
            ni.Icon = KeppyDriverWatchdog.Properties.Resources.gear;
            ni.Visible = true;

            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        public void Dispose()
        {
            ni.Icon = null;
            ni.Dispose();
            System.Windows.Forms.Application.DoEvents();
        }

        void Exit(object sender, EventArgs e)
        {
            ni.Visible = false;

            Application.Exit();
        }

        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string currentpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Process.Start(currentpath + "\\KeppyDriverConfigurator.exe", null);
            }
        }
    }
}
