using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace KeppyDriverConfigurator
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool ok;
            Mutex m = new Mutex(true, "KeppyDriverConfigurator", out ok);
            if (!ok)
            {
                System.Media.SystemSounds.Hand.Play();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KeppyDriverConfiguratorMain());
            GC.KeepAlive(m);
        }
    }
}
