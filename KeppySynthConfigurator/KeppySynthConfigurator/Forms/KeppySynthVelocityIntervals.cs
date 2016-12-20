using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthVelocityIntervals : Form
    {
        public static int previouslovel;
        public static int previoushivel;
        public static bool Confirmed;

        public KeppySynthVelocityIntervals()
        {
            InitializeComponent();
        }

        private void KeppySynthVelocityIntervals_Load(object sender, EventArgs e)
        {
            Confirmed = false;
            LoVel.Value = 1;
            HiVel.Value = 127;
            previouslovel = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("lovelign", "1"));
            previoushivel = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("hivelign", "1"));
            LoVel.Value = previouslovel;
            HiVel.Value = previoushivel;
            PrevSett.Text = String.Format("Previous settings: Lo. {0}, Hi. {1}", previouslovel, previoushivel);
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
                else
                {
                    // Everything is fine
                }

                KeppySynthConfiguratorMain.SynthSettings.SetValue("lovelign", Convert.ToInt32(LoVel.Value), RegistryValueKind.DWord);
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
                else
                {
                    // Everything is fine
                }

                KeppySynthConfiguratorMain.SynthSettings.SetValue("hivelign", Convert.ToInt32(HiVel.Value), RegistryValueKind.DWord);
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
                KeppySynthConfiguratorMain.SynthSettings.SetValue("lovelign", previouslovel, RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("hivelign", previoushivel, RegistryValueKind.DWord);
            }
            Dispose();
        }
    }
}
