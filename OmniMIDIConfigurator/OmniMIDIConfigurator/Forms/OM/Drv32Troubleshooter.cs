using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class Drv32Troubleshooter : Form
    {
        List<Drv32Dev> Devs32;
        List<Drv32Dev> Devs64;

        public Drv32Troubleshooter()
        {
            InitializeComponent();

            Opacity = 0;

            if (!CheckDevs()) Opacity = 100;
            else Load += (s, e) => Close();
        }

        private bool CheckDevs()
        {
            bool Status32 = false, Status64 = false;

            Devs32 = new List<Drv32Dev>();
            for (int i = 1; i < 10; i++)
            {
                try
                {
                    Devs32.Add(new Drv32Dev()
                    {
                        ID = String.Format("midi{0}", (i == 1) ? "" : i.ToString()),
                        Library = Functions.CLSID32.GetValue(String.Format("midi{0}", i), "wdmaud.drv").ToString()
                    });
                }
                catch { }
            }

            foreach (Drv32Dev Dev in Devs32)
            {
                if (Dev.Library.Equals("OmniMIDI.dll\0"))
                {
                    Status32 = true;
                    break;
                }
            }

            Drv32L.Items.Clear();

            foreach (Drv32Dev Dev in Devs32)
                Drv32L.Items.Add(Dev.ID).SubItems.Add(Dev.Library);

            if (Environment.Is64BitOperatingSystem)
            {
                Devs64 = new List<Drv32Dev>();

                for (int i = 1; i < 10; i++)
                {
                    try
                    {
                        Devs64.Add(new Drv32Dev()
                        {
                            ID = String.Format("midi{0}", (i == 1) ? "" : i.ToString()),
                            Library = Functions.CLSID64.GetValue(String.Format("midi{0}", i), "wdmaud.drv").ToString()
                        });
                    }
                    catch { }
                }

                foreach (Drv32Dev Dev in Devs64)
                {
                    if (Dev.Library.Equals("OmniMIDI.dll\0"))
                    {
                        Status64 = true;
                        break;
                    }
                }

                Drv64L.Items.Clear();

                foreach (Drv32Dev Dev in Devs64)
                    Drv64L.Items.Add(Dev.ID).SubItems.Add(Dev.Library);
            }
            else Devs64 = new List<Drv32Dev>();

            return (Status32 == true && Status64 == true);
        }

        private void FixBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDriverRegister.exe", "/unregisterv").WaitForExit();
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDriverRegister.exe", "/register").WaitForExit();
                FixBtn.Enabled = !CheckDevs();
            }
            catch { }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    class Drv32Dev
    {
        public string ID { get; set; }
        public string Library { get; set; }
    }
}
