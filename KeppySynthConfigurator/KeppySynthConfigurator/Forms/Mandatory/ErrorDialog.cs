using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace KeppySynthConfigurator 
{
    public partial class SecretDialog : Form
    {
        System.Media.SystemSound SoundToPlay;

        public SecretDialog(Int32 Type, System.Media.SystemSound sound, String title, String message, Exception ex)
        {
            try
            {
                SoundToPlay = sound;
                InitializeComponent();
                if (!Properties.Settings.Default.ButterBoy)
                {
                    if (Type == 0)
                        CurrentIcon.Image = Properties.Resources.infoicon;
                    else if (Type == 1)
                        CurrentIcon.Image = Properties.Resources.erroricon;
                    else
                        CurrentIcon.Image = Properties.Resources.erroricon;
                }
                else
                {
                    if (Type == 0)
                        CurrentIcon.Image = Properties.Resources.bbhappy;
                    else if (Type == 1)
                        CurrentIcon.Image = Properties.Resources.bbcrying;
                    else
                        CurrentIcon.Image = Properties.Resources.bbsad;
                }
                Text = String.Format("Keppy's Synthesizer - {0}", title);
                MessageText.Text = String.Format("{0}", message);
                try { DebugInfoText.Text = ex.ToString(); } catch { DebugInfo.Visible = false; DebugInfoText.Visible = false; }
            }
            catch (Exception ex2)
            {
                CurrentIcon.Image = Properties.Resources.wi;
                Text = "Keppy's Synthesizer - Dialog error";
                MessageText.Text = "An unknown error has occurred while initializing the window.\nPlease try again later.\n\nPress OK to close this window.";
                Program.DebugToConsole(true, MessageText.Text, ex2);
            }
        }

        private void UpdateYesNo_Load(object sender, EventArgs e)
        {
            SoundToPlay.Play();
        }

        private void NoBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
            Dispose();
        }

        private void DebugInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (DebugInfo.Checked)
            {
                Size = new Size(400, 365);
                DebugInfoText.Visible = true;
            }
            else
            {
                Size = new Size(400, 162);
                DebugInfoText.Visible = false;
            }
        }
    }
}
