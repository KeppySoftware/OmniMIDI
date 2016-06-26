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

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverBlacklistSystem : Form
    {
        public string BlacklistPath { get; set; }
        public string DefBlacklistPath { get; set; }
        public string blacklistnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();
        public string blacklistoldlocation = System.Environment.GetEnvironmentVariable("LOCALAPPDATA").ToString();

        public KeppyDriverBlacklistSystem()
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

        private void KeppyDriverBlacklistSystem_Load(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(blacklistoldlocation + "\\Keppy's Driver\\blacklist\\"))
            {
                Directory.CreateDirectory(blacklistnewlocation + "\\Keppy's Driver\\blacklist\\");
                File.Move(blacklistoldlocation + "\\Keppy's Driver\\blacklist\\keppymididrv.blacklist", blacklistnewlocation + "\\Keppy's Driver\\blacklist\\keppymididrv.blacklist");
                File.Delete(blacklistoldlocation + "\\Keppy's Driver\\blacklist\\keppymididrv.blacklist");
                Directory.Delete(blacklistoldlocation + "\\Keppy's Driver\\blacklist\\");
            }

            // Initialize blacklist
            BlacklistPath = blacklistnewlocation + "\\Keppy's Driver\\blacklist\\keppymididrv.blacklist";
            DefBlacklistPath = System.Environment.GetEnvironmentVariable("WINDIR") + "\\keppymididrv.defaultblacklist";

            if (!System.IO.Directory.Exists(blacklistnewlocation + "\\Keppy's Driver\\blacklist\\"))
            {
                System.IO.Directory.CreateDirectory(blacklistnewlocation + "\\Keppy's Driver\\blacklist\\");
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

        private void BlackListAdvancedMode_CheckedChanged(object sender, EventArgs e)
        {
            if (BlackListAdvancedMode.Checked == true)
            {
                BlackListDef.Text = "Type the name of the program in the textbox.";
                AddBlackList.Text = "Add executable";
                ManualBlackListLabel.Enabled = true;
                ManualBlackList.Enabled = true;
            }
            else
            {
                BlackListDef.Text = "Select a program by clicking \"Add executable(s)\".";
                AddBlackList.Text = "Add executable(s)";
                ManualBlackListLabel.Enabled = false;
                ManualBlackList.Enabled = false;
            }
        }

        private void AddBlackList_Click(object sender, EventArgs e)
        {
            if (BlackListAdvancedMode.Checked == true)
            {
                ProgramsBlackList.Items.Add(ManualBlackList.Text);
            }
            else
            {
                AddBlacklistedProgram.FileName = "";
                if (AddBlacklistedProgram.ShowDialog() == DialogResult.OK)
                {
                    foreach (string str in AddBlacklistedProgram.FileNames)
                    {
                        ProgramsBlackList.Items.Add(str);
                    }
                }
            }
            SaveBlackList();
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

        private void DefBlackListEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to edit the default list?\nThis is not recommended.\n\n(If you still want to edit it, it's recommended to open the file with Notepad++)", "Editing the default blacklist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                System.Diagnostics.Process process = null;
                System.Diagnostics.ProcessStartInfo processStartInfo;
                processStartInfo = new System.Diagnostics.ProcessStartInfo();
                processStartInfo.FileName = "notepad.exe";
                processStartInfo.Arguments = DefBlacklistPath;
                processStartInfo.Verb = "runas";
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                processStartInfo.UseShellExecute = true;
                try
                {
                    process = System.Diagnostics.Process.Start(processStartInfo);
                }
                catch
                {
                    
                }
                finally
                {
                    if (process != null)
                    {
                        process.Dispose();
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }        
        }
    }
}
