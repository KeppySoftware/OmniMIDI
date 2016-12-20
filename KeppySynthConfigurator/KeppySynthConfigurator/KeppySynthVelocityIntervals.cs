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
        public static RegistryKey Settings;

        public KeppySynthVelocityIntervals()
        {
            InitializeComponent();
        }

        private void KeppySynthVelocityIntervals_Load(object sender, EventArgs e)
        {
            Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            LoVel.Value = 1;
            HiVel.Value = 127;
            LoVel.Value = Convert.ToInt32(Settings.GetValue("lovelign", "1"));
            HiVel.Value = Convert.ToInt32(Settings.GetValue("hivelign", "1"));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            Settings.SetValue("lovelign", Convert.ToInt32(LoVel.Value), RegistryValueKind.DWord);
            Settings.SetValue("hivelign", Convert.ToInt32(HiVel.Value), RegistryValueKind.DWord);
            Settings.Close();
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Settings.Close();
            Close();
        }
    }
}
