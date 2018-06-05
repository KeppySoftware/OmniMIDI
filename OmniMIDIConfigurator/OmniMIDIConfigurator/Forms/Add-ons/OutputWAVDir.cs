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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace OmniMIDIConfigurator
{
    public partial class KeppySynthOutputWAVDir : Form
    {
        private CommonOpenFileDialog NewOutputDir = new CommonOpenFileDialog();

        public KeppySynthOutputWAVDir()
        {
            InitializeComponent();
            NewOutputDir.IsFolderPicker = true;
        }

        private void KeppyDriverOutputWAVDir_Load(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Configuration", true);
            DefaultDir.Text = Settings.GetValue("AudToWAVFolder", "<no default directory selected>").ToString();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(DefaultDir.Text))
                    NewOutputDir.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);       
                else
                    NewOutputDir.InitialDirectory = DefaultDir.Text;

                if (NewOutputDir.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Configuration", true);
                    Settings.SetValue("AudToWAVFolder", Path.GetDirectoryName(NewOutputDir.FileName), RegistryValueKind.String);
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
