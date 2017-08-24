using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.Permissions;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthBlacklistSystem : Form
    {
        public string LastBrowserPath { get; set; }
        public string BlacklistPath { get; set; }
        public string DefBlacklistPath { get; set; }
        public string blacklistnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();
        public string blacklistoldlocation = System.Environment.GetEnvironmentVariable("LOCALAPPDATA").ToString();

        public KeppySynthBlacklistSystem()
        {
            InitializeComponent();
        }

        private void SaveBlackList()
        {
            using (StreamWriter sw = new StreamWriter(BlacklistPath))
            {
                try
                {
                    foreach (var item in ProgramsBlackList.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while saving the blacklist!\n\n.NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void InitializeLastPath()
        {
            try
            {
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                if (SynthPaths.GetValue("lastpathblacklist", null) != null)
                {
                    LastBrowserPath = SynthPaths.GetValue("lastpathblacklist").ToString();
                    AddBlacklistedProgram.InitialDirectory = LastBrowserPath;
                }
                else
                {
                    SynthPaths.SetValue("lastpathblacklist", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    LastBrowserPath = SynthPaths.GetValue("lastpathblacklist").ToString();
                    AddBlacklistedProgram.InitialDirectory = LastBrowserPath;
                }
                SynthPaths.Close();
            }
            catch
            {
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                SynthPaths.SetValue("lastpathblacklist", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                LastBrowserPath = SynthPaths.GetValue("lastpathblacklist").ToString();
                AddBlacklistedProgram.InitialDirectory = LastBrowserPath;
                SynthPaths.Close();
            }
        }

        private void KeppyDriverBlacklistSystem_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("noblacklistmsg", "0")) == 1) NoBlockMessage.Checked = true;
            else NoBlockMessage.Checked = false;

            if (System.IO.Directory.Exists(blacklistoldlocation + "\\Keppy's Synthesizer\\blacklist\\"))
            {
                Directory.CreateDirectory(blacklistnewlocation + "\\Keppy's Synthesizer\\blacklist\\");
                File.Move(blacklistoldlocation + "\\Keppy's Synthesizer\\blacklist\\keppymididrv.blacklist", blacklistnewlocation + "\\Keppy's Synthesizer\\blacklist\\keppymididrv.blacklist");
                File.Delete(blacklistoldlocation + "\\Keppy's Synthesizer\\blacklist\\keppymididrv.blacklist");
                Directory.Delete(blacklistoldlocation + "\\Keppy's Synthesizer\\blacklist\\");
            }

            ProgramsBlackList.ContextMenu = BlacklistContext;
            InitializeLastPath();

            // Initialize blacklist
            BlacklistPath = blacklistnewlocation + "\\Keppy's Synthesizer\\blacklist\\keppymididrv.blacklist";
            DefBlacklistPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\keppysynth.dbl";

            if (!System.IO.Directory.Exists(blacklistnewlocation + "\\Keppy's Synthesizer\\blacklist\\"))
            {
                System.IO.Directory.CreateDirectory(blacklistnewlocation + "\\Keppy's Synthesizer\\blacklist\\");
                File.Create(BlacklistPath).Dispose();
            }
            else
            {
                try
                {
                    // Import the blacklist file
                    using (StreamReader r = new StreamReader(BlacklistPath))
                    {
                        string line;
                        while ((line = r.ReadLine()) != null)
                        {
                            ProgramsBlackList.Items.Add(line); // The program is copying the entire text file to the List I's listbox.
                        }
                    }
                }
                catch
                {
                    File.Create(BlacklistPath).Dispose();
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

        private void ProgramsBlackList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (ProgramsBlackList.SelectedIndex != -1)
                {
                    ProgramsBlackList.Items.RemoveAt(ProgramsBlackList.SelectedIndex);
                }
                e.Handled = true;
            }
        }

        private void AddBlackList_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                String Program = null;
                ShowInputDialog(ref Program);
                if (Program != null || Program != "")
                {
                    ProgramsBlackList.Items.Add(Program);
                    SaveBlackList();
                }
            }
            else
            {
                AddBlacklistedProgram.FileName = "";
                if (AddBlacklistedProgram.ShowDialog() == DialogResult.OK)
                {
                    foreach (string str in AddBlacklistedProgram.FileNames)
                    {
                        RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                        LastBrowserPath = Path.GetDirectoryName(str);
                        SynthPaths.SetValue("lastpathblacklist", LastBrowserPath, RegistryValueKind.String);
                        SynthPaths.Close();
                        ProgramsBlackList.Items.Add(str);
                    }
                    SaveBlackList();
                }
            }
        }

        private void AEr_Click(object sender, EventArgs e)
        {
            using (var form = new BlacklistSystemProcesses())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string[] val = form.BanThesePlease.ToArray();
                    foreach (string item in val) ProgramsBlackList.Items.Add(item);
                    SaveBlackList();
                }
            }
        }

        private void RemoveBlackList_Click(object sender, EventArgs e)
        {
            for (int i = ProgramsBlackList.SelectedIndices.Count - 1; i >= 0; i--)
            {
                ProgramsBlackList.Items.RemoveAt(ProgramsBlackList.SelectedIndices[i]);
            }
            SaveBlackList();
        }

        private void ClearBlacklist_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the blacklist?", "Keppy's Synthesizer Configurator ~ Clear blacklist", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    ProgramsBlackList.Items.Clear();
                    File.Delete(BlacklistPath);
                    var TempFile = File.Create(BlacklistPath);
                    TempFile.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while saving the blacklist!\n\n.NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private void EDBLi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to edit the default list?\nThis is not recommended.\n\n(If you still want to edit it, it's recommended to open the file with Notepad++)", "Editing the default blacklist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start(DefBlacklistPath);
            }
        }

        private void UDBLi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResultR = MessageBox.Show("Do you want to update/restore the default blacklist?", "Restore the default blacklist", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResultR == DialogResult.Yes)
            {
                Program.DebugToConsole(false, "Downloading the default blacklist", null);
                string dbl = "https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Synthesizer/master/output/keppysynth.dbl";
                Forms.DLEngine frm = new Forms.DLEngine(null, "Downloading the default blacklist", dbl, Path.GetDirectoryName(DefBlacklistPath), 2, false);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
        }

        private void NoBlockMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (NoBlockMessage.Checked == true)
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("noblacklistmsg", "1", Microsoft.Win32.RegistryValueKind.DWord);
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("noblacklistmsg", "0", Microsoft.Win32.RegistryValueKind.DWord);
            }
        }
    }
}
