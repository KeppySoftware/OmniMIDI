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
    public partial class KeppyDriverSFListAssign : Form
    {
        public string LastBrowserPath { get; set; }

        public static string soundfontnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();

        public string ListsPath = soundfontnewlocation + "\\Keppy's Driver\\applists";
        public string List1Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidi.applist";
        public string List2Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidib.applist";
        public string List3Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidic.applist";
        public string List4Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidid.applist";
        public string List5Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidie.applist";
        public string List6Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidif.applist";
        public string List7Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidig.applist";
        public string List8Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidih.applist";
        public string List9Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidii.applist";
        public string List10Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidij.applist";
        public string List11Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidik.applist";
        public string List12Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidil.applist";
        public string List13Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidim.applist";
        public string List14Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidin.applist";
        public string List15Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidio.applist";
        public string List16Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidip.applist";

        public int whichone { get; set; }

        public string CurrentList { get; set; }

        public KeppyDriverSFListAssign()
        {
            InitializeComponent();
            RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            if (Convert.ToInt32(SynthSettings.GetValue("extra8lists", 0)) == 1)
            {
                SelectedListBox.Items.Add("List 9");
                SelectedListBox.Items.Add("List 10");
                SelectedListBox.Items.Add("List 11");
                SelectedListBox.Items.Add("List 12");
                SelectedListBox.Items.Add("List 13");
                SelectedListBox.Items.Add("List 14");
                SelectedListBox.Items.Add("List 15");
                SelectedListBox.Items.Add("List 16");
            }
            else
            {

            }
        }

        private Control WhoTriggeredMe(object sender)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    Control sourceControl = owner.SourceControl; // sourceControl.Name is the shit
                    return sourceControl;
                }
            }
            return null;
        }

        private void ChangeList(string WhichList)
        {
            try
            {
                if (!System.IO.Directory.Exists(ListsPath))
                {
                    Directory.CreateDirectory(ListsPath);
                    File.Create(WhichList).Dispose();
                    Lis.Items.Clear();
                    return;
                }
                if (!System.IO.File.Exists(WhichList))
                {
                    File.Create(WhichList).Dispose();
                    Lis.Items.Clear();
                    return;
                }
                try
                {
                    using (StreamReader r = new StreamReader(WhichList))
                    {
                        string line;
                        Lis.Items.Clear();
                        while ((line = r.ReadLine()) != null)
                        {
                            Lis.Items.Add(line); // The program is copying the entire text file to the List I's listbox.
                        }
                    }
                }
                catch
                {
                    File.Create(WhichList).Dispose();
                    MessageBox.Show("The soundfont list was missing, so the configurator automatically created it for you.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal error during the execution of the program.\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }


        private void SaveList(String SelectedList)
        {
            using (StreamWriter sw = new StreamWriter(SelectedList))
            {
                foreach (var item in Lis.Items)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }

        private void ClearAppList()
        {
            Lis.Items.Clear();
            SaveList(CurrentList);
        }

        private void RemoveAppFromList(string selectedlist)
        {
            for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
            {
                Lis.Items.RemoveAt(Lis.SelectedIndices[i]);
            }
            SaveList(CurrentList);
        }

        private void AddAppToList(string str) 
        {
            Lis.Items.Add(str);
            SaveList(CurrentList);
        }

        private void AddAppNameOnlyToList(string str)
        {
            Lis.Items.Add(Path.GetFileName(str));
            SaveList(CurrentList);
        }

        private void InitializeLastPath()
        {
            try
            {
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Paths", true);
                if (SynthPaths.GetValue("lastpathsfassign", null) != null)
                {
                    LastBrowserPath = SynthPaths.GetValue("lastpathsfassign").ToString();
                    AddApp.InitialDirectory = LastBrowserPath;
                }
                else
                {
                    SynthPaths.SetValue("lastpathsfassign", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    LastBrowserPath = SynthPaths.GetValue("lastpathsfassign").ToString();
                    AddApp.InitialDirectory = LastBrowserPath;
                }
                SynthPaths.Close();      
            }
            catch {
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Driver\\Paths");
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Paths", true);
                SynthPaths.SetValue("lastpathsfassign", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                LastBrowserPath = SynthPaths.GetValue("lastpathsfassign").ToString();
                AddApp.InitialDirectory = LastBrowserPath;
                SynthPaths.Close();      
            }
        }

        private void KeppyDriverSFListAssign_Load(object sender, EventArgs e)
        {
            SelectedListBox.Text = "List 1";
            InitializeLastPath();
        }

        private void addAnAppToTheListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control LeControl = WhoTriggeredMe(sender);
            AddApp.InitialDirectory = LastBrowserPath;
            if (AddApp.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in AddApp.FileNames)
                {
                    RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Paths", true);
                    LastBrowserPath = Path.GetDirectoryName(str);
                    SynthPaths.SetValue("lastpathsfassign", LastBrowserPath, RegistryValueKind.String);
                    SynthPaths.Close();
                    AddAppToList(str);
                }
            }
        }

        private void addAnAppToTheListAppNameOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control LeControl = WhoTriggeredMe(sender);
            AddApp.InitialDirectory = LastBrowserPath;
            if (AddApp.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in AddApp.FileNames)
                {
                    AddAppNameOnlyToList(str);
                }
            }
        }

        private void removeSelectedAppsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control LeControl = WhoTriggeredMe(sender);
            RemoveAppFromList(LeControl.Name);
        }

        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control LeControl = WhoTriggeredMe(sender);
            ClearAppList();
        }

        private void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedListBox.Text == "List 1")
            {
                CurrentList = List1Path;
                whichone = 1;
                ChangeList(List1Path);
            }
            else if (SelectedListBox.Text == "List 2")
            {
                CurrentList = List2Path;
                whichone = 2;
                ChangeList(List2Path);
            }
            else if (SelectedListBox.Text == "List 3")
            {
                CurrentList = List3Path;
                whichone = 3;
                ChangeList(List3Path);
            }
            else if (SelectedListBox.Text == "List 4")
            {
                CurrentList = List4Path;
                whichone = 4;
                ChangeList(List4Path);
            }
            else if (SelectedListBox.Text == "List 5")
            {
                CurrentList = List5Path;
                whichone = 5;
                ChangeList(List5Path);
            }
            else if (SelectedListBox.Text == "List 6")
            {
                CurrentList = List6Path;
                whichone = 6;
                ChangeList(List6Path);
            }
            else if (SelectedListBox.Text == "List 7")
            {
                CurrentList = List7Path;
                whichone = 7;
                ChangeList(List7Path);
            }
            else if (SelectedListBox.Text == "List 8")
            {
                CurrentList = List8Path;
                whichone = 8;
                ChangeList(List8Path);
            }
            else if (SelectedListBox.Text == "List 9")
            {
                CurrentList = List9Path;
                whichone = 9;
                ChangeList(List9Path);
            }
            else if (SelectedListBox.Text == "List 10")
            {
                CurrentList = List10Path;
                whichone = 10;
                ChangeList(List10Path);
            }
            else if (SelectedListBox.Text == "List 11")
            {
                CurrentList = List11Path;
                whichone = 11;
                ChangeList(List11Path);
            }
            else if (SelectedListBox.Text == "List 12")
            {
                CurrentList = List12Path;
                whichone = 12;
                ChangeList(List12Path);
            }
            else if (SelectedListBox.Text == "List 13")
            {
                CurrentList = List13Path;
                whichone = 13;
                ChangeList(List13Path);
            }
            else if (SelectedListBox.Text == "List 14")
            {
                CurrentList = List14Path;
                whichone = 14;
                ChangeList(List14Path);
            }
            else if (SelectedListBox.Text == "List 15")
            {
                CurrentList = List15Path;
                whichone = 15;
                ChangeList(List15Path);
            }
            else if (SelectedListBox.Text == "List 16")
            {
                CurrentList = List16Path;
                whichone = 16;
                ChangeList(List16Path);
            }
        }
    }
}
