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

namespace OmniMIDIConfigurator 
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

                if (Type == 0)
                    CurrentIcon.Image = Properties.Resources.infoicon;
                else if (Type == 1)
                    CurrentIcon.Image = Properties.Resources.erroricon;
                else
                    CurrentIcon.Image = Properties.Resources.erroricon;

                Text = String.Format("OmniMIDI - {0}", title);
                MessageText.Text = String.Format("{0}", message);
                try { DebugInfoText.Text = ex.ToString(); }
                catch
                {
                    ExGroup.Visible = false;
                    SaveLog.Visible = false;
                    Size = new Size(444, 166);
                }
            }
            catch (Exception ex2)
            {
                CurrentIcon.Image = Properties.Resources.wi;
                Text = "OmniMIDI - Dialog error";
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

        private void SaveLog_Click(object sender, EventArgs e)
        {
            if (SaveLogDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && SaveLogDialog.FileName.Length > 0)
            {
                try
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter(SaveLogDialog.FileName);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(String.Format("Debug log of \"{0}\"", Text));
                    sb.AppendLine("=================");
                    sb.AppendLine("Friendly text:");
                    sb.AppendLine(MessageText.Text);
                    sb.AppendLine("");
                    sb.AppendLine("Exception reason:");
                    sb.AppendLine(DebugInfoText.Text);
                    sb.AppendLine("=================");
                    sb.AppendLine(String.Format("Date: {0}", DateTime.UtcNow.ToString("MMMM dd, yyyy - hh:mm:ss.fff tt")));
                    file.WriteLine(sb.ToString());

                    file.Close();

                    MessageBox.Show(String.Format("Log saved as \"{0}\" successfully.", SaveLogDialog.FileName), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can not save the log file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }
    }
}
