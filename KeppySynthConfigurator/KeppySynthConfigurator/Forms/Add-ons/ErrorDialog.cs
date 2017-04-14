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

        public SecretDialog(Image icon, System.Media.SystemSound sound, String title, String message)
        {
            try
            {
                SoundToPlay = sound;
                InitializeComponent();
                CurrentIcon.Image = icon;
                Text = String.Format("Keppy's Synthesizer - {0}", title);
                MessageText.Text = String.Format("{0}", message);
            }
            catch (Exception ex)
            {
                CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.wi;
                Text = "Keppy's Synthesizer - Dialog error";
                MessageText.Text = "An unknown error has occurred while initializing the window.\nPlease try again later.\n\nPress OK to close this window.";
                Program.DebugToConsole(true, MessageText.Text, ex);
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
    }
}
