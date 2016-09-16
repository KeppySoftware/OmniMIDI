using System.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;


namespace KeppySynthDebugWindow
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool ok;
            Mutex m = new Mutex(true, "KeppySynthDebugWindow", out ok);
            if (!ok)
            {
                return; // If there's already another instance running, it'll just quit.
            }
            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.Idle;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KeppySynthDebugWindow());
            GC.KeepAlive(m);
        }
    }
}
