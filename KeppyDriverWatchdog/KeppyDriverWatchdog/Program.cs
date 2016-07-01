using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
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
    }
}
