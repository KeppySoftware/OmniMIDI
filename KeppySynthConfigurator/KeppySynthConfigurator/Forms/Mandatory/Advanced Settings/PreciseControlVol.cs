using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class PreciseControlVol : Form
    {
        public int ReturnValue { get; private set; }

        public PreciseControlVol()
        {
            InitializeComponent();
            decimal VolVal = (decimal)KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value / 100;
            VolIntView.Text = String.Format("{0}%", VolVal.ToString("000.00"));
            VolTrackBar.Maximum = KeppySynthConfiguratorMain.Delegate.VolTrackBar.Maximum;
            VolTrackBar.Value = KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value;
        }

        private void VolTrackBar_Scroll(object sender, EventArgs e)
        {
            decimal VolVal = (decimal)VolTrackBar.Value / 100;
            VolIntView.Text = String.Format("{0}%", VolVal.ToString("000.00"));
            KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = VolTrackBar.Value;
        }

        private void ReturnOK_Click(object sender, EventArgs e)
        {
            Functions.UpdateDiscordPresence("Tampering with the settings", "play", 0, 0);
            KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = VolTrackBar.Value;
            Close();
        }

        private void PreciseControlVol_Load(object sender, EventArgs e)
        {
            Functions.UpdateDiscordPresence("Fine tuning the volume", "play", 0, 0);
        }
    }
}
