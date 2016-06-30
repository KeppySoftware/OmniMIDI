using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace KeppyDriverWatchdog
{
    static class Program
    {
        [STAThread]
        static void Main()
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
    }
}
