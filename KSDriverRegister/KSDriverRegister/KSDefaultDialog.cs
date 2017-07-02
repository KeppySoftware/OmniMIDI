using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KSDriverRegister
{
    public partial class KSDefaultDialog : Form
    {
        private FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll");

        public KSDefaultDialog()
        {
            InitializeComponent();
        }

        private void KSDefaultDialog_Load(object sender, EventArgs e)
        {
            VER.Text = String.Format("KSDriverRegister\nv{0}.{1}.{2}.{3}", Driver.FileMajorPart, Driver.FileMinorPart, Driver.FileBuildPart, Driver.FilePrivatePart);
        }

        private void RD_Click(object sender, EventArgs e)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                Program.Register(false, "x86", Program.clsid32);
                Program.Register(false, "x64", Program.clsid64);
            }
            else
            {
                Program.Register(false, "x86", Program.clsid32);
            }
        }

        private void UnRD_Click(object sender, EventArgs e)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                Program.Unregister(false, "x86", Program.clsid32);
                Program.Unregister(false, "x64", Program.clsid64);
            }
            else
            {
                Program.Unregister(false, "x86", Program.clsid32);
            }
        }
    }
}
