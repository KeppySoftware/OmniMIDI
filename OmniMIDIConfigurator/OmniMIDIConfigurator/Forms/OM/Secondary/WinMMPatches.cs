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

        private void PatchFunc(DialogResult Res, String Type, String Status)
        {
            switch (Res)
            {
                case DialogResult.OK:
                    PatchStatusLabel.ForeColor = Color.DarkGreen;
                    PatchStatusLabel.Text = String.Format("{0} patch installed! {1}", Type, Status);
                    break;
                case DialogResult.No:
                    PatchStatusLabel.ForeColor = Color.DarkRed;
                    PatchStatusLabel.Text = String.Format("Error! (E{0})", Status);
                    break;
                case DialogResult.Abort:
                    PatchStatusLabel.ForeColor = Color.DarkGray;
                    PatchStatusLabel.Text = "Aborted.";
                    break;
            }
        }

        private void AdditionalOptions(bool DAW, bool Visible)
        {
            if (DAW)
            {
                i386DAW.Visible = Visible;
                AMD64DAW.Visible = Visible;
                AArch64DAW.Visible = Visible;
                return;
            }

            i386BM.Visible = Visible;
            AMD64BM.Visible = Visible;
            AArch64BM.Visible = Visible;
        } 

        private void WMMW_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;

            if (Control.ModifierKeys == Keys.Shift)
            {
                AdditionalOptions(false, true);
                return;
            }

            PatchFunc(Functions.ApplyWinMMWRPPatch(false, PA.FILE_ARCH.UNK, out Status), "Black MIDI", Status);
        }

        private void WMMD_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;

            if (Control.ModifierKeys == Keys.Shift)
            {
                AdditionalOptions(true, true);
                return;
            }

            PatchFunc(Functions.ApplyWinMMWRPPatch(true, PA.FILE_ARCH.UNK, out Status), "DAW", Status);
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

        private void i386BM_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            PatchFunc(Functions.ApplyWinMMWRPPatch(false, PA.FILE_ARCH.i386, out Status), "Black MIDI", Status);
            AdditionalOptions(false, false);
        }

        private void AMD64BM_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            PatchFunc(Functions.ApplyWinMMWRPPatch(false, PA.FILE_ARCH.AMD64, out Status), "Black MIDI", Status);
            AdditionalOptions(false, false);
        }

        private void AArch64BM_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            PatchFunc(Functions.ApplyWinMMWRPPatch(false, PA.FILE_ARCH.AArch64, out Status), "Black MIDI", Status);
            AdditionalOptions(false, false);
        }

        private void i386DAW_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            PatchFunc(Functions.ApplyWinMMWRPPatch(true, PA.FILE_ARCH.i386, out Status), "DAW", Status);
            AdditionalOptions(true, false);
        }

        private void AMD64DAW_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            PatchFunc(Functions.ApplyWinMMWRPPatch(true, PA.FILE_ARCH.AMD64, out Status), "DAW", Status);
            AdditionalOptions(true, false);
        }

        private void AArch64DAW_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            PatchFunc(Functions.ApplyWinMMWRPPatch(true, PA.FILE_ARCH.AArch64, out Status), "DAW", Status);
            AdditionalOptions(true, false);
        }

        private void BMPatch_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show("This patch should be used when pure raw performance is needed, and you don't need to communicate with other MIDI out devices.\n\n" +
                "You should use this patch if you want the best MIDI performance with Black MIDIs.", "OmniMIDI - Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DAWPatch_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show("This patch should be used when you want good performance without losing access to other MIDI out devices.\n\n" +
                "You should use this patch if you want to use KDMAPI together with other MIDI out devices, such as on FL Studio or Cubase.", "OmniMIDI - Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OKClose_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
