using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class WinMMPatches : Form
    {
        public WinMMPatches()
        {
            InitializeComponent();

            WMMW32.Enabled = true;
            if (Environment.Is64BitOperatingSystem) WMMW64.Enabled = true;
        }

        private void WMMW32_Click(object sender, EventArgs e)
        {
            if (Functions.ApplyWinMMWRPPatch(false))
            {
                PatchStatusLabel.ForeColor = Color.DarkGreen;
                PatchStatusLabel.Text = "The app has been successfully patched!";
            }
            else
            {
                PatchStatusLabel.ForeColor = Color.DarkRed;
                PatchStatusLabel.Text = "Error while patching app!";
            }
        }

        private void WMMW64_Click(object sender, EventArgs e)
        {
            if (Functions.ApplyWinMMWRPPatch(true))
            {
                PatchStatusLabel.ForeColor = Color.DarkGreen;
                PatchStatusLabel.Text = "Successfully patched!";
            }
            else
            {
                PatchStatusLabel.ForeColor = Color.DarkRed;
                PatchStatusLabel.Text = "Error!";
            }
        }

        private void UnpatchApp_Click(object sender, EventArgs e)
        {
            if (Functions.RemoveWinMMPatch())
            {
                PatchStatusLabel.ForeColor = Color.DarkGreen;
                PatchStatusLabel.Text = "Successfully unpatched!";
            }
            else
            {
                PatchStatusLabel.ForeColor = Color.DarkRed;
                PatchStatusLabel.Text = "Error!";
            }
        }

        private void DifferencesPatch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/KeppySoftware/Keppy-s-Synthesizer/wiki/What's-the-difference-between-the-WinMM-patches%3F");
        }

        private void OKClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
