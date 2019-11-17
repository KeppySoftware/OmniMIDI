using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class WinMMPatches : Form
    {
        public WinMMPatches()
        {
            InitializeComponent();
        }

        private void WMMW_Click(object sender, EventArgs e)
        {
            switch (Functions.ApplyWinMMWRPPatch(false))
            {
                case DialogResult.OK:
                    PatchStatusLabel.ForeColor = Color.DarkGreen;
                    PatchStatusLabel.Text = "Stock patch installed!";
                    break;
                case DialogResult.No:
                    PatchStatusLabel.ForeColor = Color.DarkRed;
                    PatchStatusLabel.Text = "Error!";
                    break;
                case DialogResult.Abort:
                    PatchStatusLabel.ForeColor = Color.DarkGray;
                    PatchStatusLabel.Text = "Aborted.";
                    break;
            }
        }

        private void WMMD_Click(object sender, EventArgs e)
        {
            switch (Functions.ApplyWinMMWRPPatch(false))
            {
                case DialogResult.OK:
                    PatchStatusLabel.ForeColor = Color.DarkGreen;
                    PatchStatusLabel.Text = "DAW patch installed!";
                    break;
                case DialogResult.No:
                    PatchStatusLabel.ForeColor = Color.DarkRed;
                    PatchStatusLabel.Text = "Error!";
                    break;
                case DialogResult.Abort:
                    PatchStatusLabel.ForeColor = Color.DarkGray;
                    PatchStatusLabel.Text = "Aborted.";
                    break;
            }
        }

        private void UnpatchApp_Click(object sender, EventArgs e)
        {
            switch (Functions.RemoveWinMMPatch())
            {
                case DialogResult.OK:
                    PatchStatusLabel.ForeColor = Color.DarkGreen;
                    PatchStatusLabel.Text = "Patch removed successfully!";
                    break;
                case DialogResult.No:
                    PatchStatusLabel.ForeColor = Color.DarkRed;
                    PatchStatusLabel.Text = "Error!";
                    break;
                case DialogResult.Abort:
                    PatchStatusLabel.ForeColor = Color.DarkGray;
                    PatchStatusLabel.Text = "Aborted.";
                    break;
            }
        }

        private void BMPatch_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show("This patch should be used when pure raw performance is needed, and you don't need to communicate with other MIDI out devices.\n\n" +
                "You should use this patch if you want the best MIDI performance with Black MIDIs.", "OmniMIDI - Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DAWPatch_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show("This patch should be used when you want good performance without losing access to other MIDI out devices.\n\n" +
                "You should use this patch if you want to use KSDAPI together with other MIDI out devices, such as on FL Studio or Cubase.", "OmniMIDI - Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OKClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WinMMPatches_Load(object sender, EventArgs e)
        {

        }
    }
}
