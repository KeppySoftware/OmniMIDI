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
    public partial class KeppySynthPitchShifting : Form
    {
        public KeppySynthPitchShifting()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("mainkey", (NewPitch.Value + 12), RegistryValueKind.DWord);
            Dispose();
        }

        private void KeppySynthPitchShifting_Load(object sender, EventArgs e)
        {
            NewPitch.Value = (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("mainkey", "12")) - 12);
        }
    }
}
