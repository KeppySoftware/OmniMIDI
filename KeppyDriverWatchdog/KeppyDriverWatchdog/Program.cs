using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeppyDriverWatchdog
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            NotifyIcon ni;
            ni = new NotifyIcon();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();
                Application.Run();
            }
        }
    }
}
