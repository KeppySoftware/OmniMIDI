using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthOutputWAVDir : Form
    {
        public KeppySynthOutputWAVDir()
        {
            InitializeComponent();
        }


        private void KeppyDriverOutputWAVDir_Load(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            DefaultDir.Text = Settings.GetValue("lastexportfolder", "<no default directory selected>").ToString();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(DefaultDir.Text))
                {
                    this.NewOutputDir.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                else
                {
                    this.NewOutputDir.InitialDirectory = DefaultDir.Text;
                }
                if (this.NewOutputDir.ShowDialog() == DialogResult.OK)
                {
                    RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                    Settings.SetValue("lastexportfolder", Path.GetDirectoryName(NewOutputDir.FileName), RegistryValueKind.String);
                    NewDir.Text = Path.GetDirectoryName(NewOutputDir.FileName);
                    DefaultDir.Text = Path.GetDirectoryName(NewOutputDir.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
