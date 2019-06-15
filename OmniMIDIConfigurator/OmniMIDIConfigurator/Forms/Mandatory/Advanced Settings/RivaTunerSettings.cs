using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class RivaTunerSettings : Form
    {
        public string LastBrowserPath { get; set; }
        public static string RTSSOSDListPath = String.Format("{0}\\OmniMIDI\\lists\\OmniMIDI.osdlist", Environment.GetEnvironmentVariable("USERPROFILE"));

        public RivaTunerSettings()
        {
            InitializeComponent();
        }

        private void SaveList()
        {
            using (StreamWriter sw = new StreamWriter(RTSSOSDListPath))
            {
                try
                {
                    foreach (var item in ProgramsList.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while saving the list!\n\n.NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void InitializeLastPath()
        {
            try
            {
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);
                if (SynthPaths.GetValue("lastpathrivatunerosd", null) != null)
                {
                    LastBrowserPath = SynthPaths.GetValue("lastpathrivatunerosd").ToString();
                    AddAllowedProgram.InitialDirectory = LastBrowserPath;
                }
                else
                {
                    SynthPaths.SetValue("lastpathrivatunerosd", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    LastBrowserPath = SynthPaths.GetValue("lastpathrivatunerosd").ToString();
                    AddAllowedProgram.InitialDirectory = LastBrowserPath;
                }
                SynthPaths.Close();
            }
            catch
            {
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Paths");
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);
                SynthPaths.SetValue("lastpathrivatunerosd", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                LastBrowserPath = SynthPaths.GetValue("lastpathrivatunerosd").ToString();
                AddAllowedProgram.InitialDirectory = LastBrowserPath;
                SynthPaths.Close();
            }
        }

        private void RivaTunerSettings_Load(object sender, EventArgs e)
        {
            ProgramsList.ContextMenu = RivaOSDContext;
            InitializeLastPath();

            if (!File.Exists(RTSSOSDListPath))
                File.Create(RTSSOSDListPath).Dispose();
            else
            {
                // Import the file
                using (StreamReader r = new StreamReader(RTSSOSDListPath))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        ProgramsList.Items.Add(line); // The program is copying the entire text file to the listbox.
                    }
                }
            }
        }

        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.MinimizeBox = false;
            inputBox.MaximizeBox = false;
            inputBox.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "App name/path";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        private void AE_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                String Program = null;
                ShowInputDialog(ref Program);
                if (Program != null || Program != "")
                {
                    ProgramsList.Items.Add(Program);
                    SaveList();
                }
            }
            else
            {
                AddAllowedProgram.FileName = "";
                if (AddAllowedProgram.ShowDialog() == DialogResult.OK)
                {
                    foreach (string str in AddAllowedProgram.FileNames)
                    {
                        RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);
                        LastBrowserPath = Path.GetDirectoryName(str);
                        SynthPaths.SetValue("lastpathrivatunerosd", LastBrowserPath, RegistryValueKind.String);
                        SynthPaths.Close();
                        ProgramsList.Items.Add(str);
                    }
                    SaveList();
                }
            }
        }

        private void AEr_Click(object sender, EventArgs e)
        {
            using (var form = new AddProcesses())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string[] val = form.BanThesePlease.ToArray();
                    foreach (string item in val) ProgramsList.Items.Add(item);
                    SaveList();
                }
            }
        }

        private void RE_Click(object sender, EventArgs e)
        {
            for (int i = ProgramsList.SelectedIndices.Count - 1; i >= 0; i--)
            {
                ProgramsList.Items.RemoveAt(ProgramsList.SelectedIndices[i]);
            }

            SaveList();
        }

        private void CBLi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the list?", "OmniMIDI Configurator ~ Clear RivaTuner OSD list", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    ProgramsList.Items.Clear();
                    File.Delete(RTSSOSDListPath);
                    File.Create(RTSSOSDListPath).Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while saving the list!\n\n.NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
