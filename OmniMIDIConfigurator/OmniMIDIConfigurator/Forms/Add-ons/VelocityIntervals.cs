using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace OmniMIDIConfigurator
{
    public partial class KeppySynthVelocityIntervals : Form
    {
        public static int previouslovel;
        public static int previoushivel;
        public static bool Confirmed = false;

        public KeppySynthVelocityIntervals()
        {
            InitializeComponent();
        }

        private void KeppySynthVelocityIntervals_Load(object sender, EventArgs e)
        {
            try
            {
                previouslovel = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("MinVelIgnore", "0"));
                previoushivel = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("MaxVelIgnore", "1"));
                LoVel.Value = previouslovel;
                HiVel.Value = previoushivel;
                PrevSett.Text = String.Format("Previous settings: Lo. {0}, Hi. {1}", previouslovel, previoushivel);
            }
            catch
            {
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MinVelIgnore", "1", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MaxVelIgnore", "1", RegistryValueKind.DWord);
                LoVel.Value = previouslovel = 1;
                HiVel.Value = previoushivel = 1;
                PrevSett.Text = String.Format("Previous settings: Lo. {0}, Hi. {1}", previouslovel, previoushivel);
            }
        }

        private void LoVel_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int x = Convert.ToInt32(LoVel.Value);
                int y = Convert.ToInt32(HiVel.Value);

                if (x > y)
                {
                    LoVel.Value = x - 1;
                }

                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MinVelIgnore", Convert.ToInt32(LoVel.Value), RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void HiVel_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int x = Convert.ToInt32(LoVel.Value);
                int y = Convert.ToInt32(HiVel.Value);

                if (x > y)
                {
                    HiVel.Value = x;
                }

                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MaxVelIgnore", Convert.ToInt32(HiVel.Value), RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            Confirmed = true;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Confirmed = false;
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Confirmed == false)
            {
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MinVelIgnore", previouslovel, RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MaxVelIgnore", previoushivel, RegistryValueKind.DWord);
            }
            Dispose();
        }
    }
}
