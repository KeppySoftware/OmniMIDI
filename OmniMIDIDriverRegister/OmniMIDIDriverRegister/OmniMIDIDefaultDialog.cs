using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OmniMIDIDriverRegister
{
    public partial class OmniMIDIDefaultDialog : Form
    {
        private FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\OmniMIDI\\OmniMIDI.dll");

        public OmniMIDIDefaultDialog()
        {
            InitializeComponent();
        }

        private void KSDefaultDialog_Load(object sender, EventArgs e)
        {
            VER.Text = String.Format("OMDriverRegister\nv{0}.{1}.{2}.{3}", Driver.FileMajorPart, Driver.FileMinorPart, Driver.FileBuildPart, Driver.FilePrivatePart);
        }

        private void RD_Click(object sender, EventArgs e)
        {
            Program.Register();

            /*
            if (Environment.Is64BitOperatingSystem)
            {
                Program.Register(false, "x86", Program.clsid32);
                Program.Register(false, "x64", Program.clsid64);
            }
            else
            {
                Program.Register(false, "x86", Program.clsid32);
            }
            */
        }

        private void UnRD_Click(object sender, EventArgs e)
        {
            Program.Unregister();

            /*
            if (Environment.Is64BitOperatingSystem)
            {
                Program.Unregister(false, "x86", Program.clsid32);
                Program.Unregister(false, "x64", Program.clsid64);
            }
            else
            {
                Program.Unregister(false, "x86", Program.clsid32);
            }
            */
        }
    }
}
