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

        String ToFix = string.Empty;
        String Fixed = "No issues detected with the PnP registration of OmniMIDI.\nIt is being detected properly by Windows Multimedia API as it should.\n\nPress Close to exit from this dialog.";

        public Drv32Troubleshooter(bool Startup)
        {
            InitializeComponent();

            ToFix = FixLabel.Text;

            if (Startup) Opacity = 0;

            if (!CheckDevs())
            {
                FixLabel.Text = ToFix;
                Opacity = 100;
            }
            else
            {
                if (Startup)
                    Load += (s, e) => Close();

                FixLabel.Text = Fixed;
                FixBtn.Enabled = false;
            }
        }

        private bool CheckDevs()
        {
            bool Status32 = false, Status64 = false;

            Devs32 = new List<Drv32Dev>();

            // Add midimap and midi
            Devs32.Add(new Drv32Dev()
            {
                ID = "midimapper",
                Library = Functions.CLSID32.GetValue("midimapper", "midimap.dll").ToString()
            });

            Devs32.Add(new Drv32Dev()
            {
                ID = "midi",
                Library = Functions.CLSID32.GetValue("midi", "wdmaud.drv").ToString()
            });

            for (int i = 1; i < 10; i++)
            {
                try
                {
                    String Val = String.Format("midi{0}", i);
                    String MIDIDev = Functions.CLSID32.GetValue(Val, "wdmaud.drv").ToString();
                    Devs32.Add(new Drv32Dev()
                    {
                        ID = Val,
                        Library = MIDIDev
                    });

                    if (MIDIDev.Contains("OmniMIDI.dll")) Status32 = true;
                }
                catch { }
            }

            Drv32L.Items.Clear();

            foreach (Drv32Dev Dev in Devs32)
                Drv32L.Items.Add(Dev.ID).SubItems.Add(Dev.Library);

            if (Environment.Is64BitOperatingSystem)
            {
                Devs64 = new List<Drv32Dev>();

                Devs64.Add(new Drv32Dev()
                {
                    ID = "midimapper",
                    Library = Functions.CLSID64.GetValue("midimapper", "midimap.dll").ToString()
                });

                Devs64.Add(new Drv32Dev()
                {
                    ID = "midi",
                    Library = Functions.CLSID64.GetValue("midi", "wdmaud.drv").ToString()
                });

                for (int i = 1; i < 10; i++)
                {
                    try
                    {
                        String Val = String.Format("midi{0}", i);
                        String MIDIDev = Functions.CLSID64.GetValue(Val, "wdmaud.drv").ToString();
                        Devs64.Add(new Drv32Dev()
                        {
                            ID = Val,
                            Library = MIDIDev
                        });

                        if (MIDIDev.Contains("OmniMIDI.dll")) Status64 = true;
                    }
                    catch { }
                }

                Drv64L.Items.Clear();

                foreach (Drv32Dev Dev in Devs64)
                    Drv64L.Items.Add(Dev.ID).SubItems.Add(Dev.Library);
            }
            else
            {
                Devs64 = new List<Drv32Dev>();
                Drv64L.Enabled = false;
            }

            return (Status32 == true && Status64 == true);
        }

        private void FixBtn_Click(object sender, EventArgs e)
        {
            try
            {
                bool Registered = false;

                Functions.DriverRegistry(true, true);
                Functions.DriverRegistry(false, true);

                Registered = CheckDevs();
                FixBtn.Enabled = !Registered;
                FixLabel.Text = String.Format("{0}", Registered ? Fixed : ToFix);
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
