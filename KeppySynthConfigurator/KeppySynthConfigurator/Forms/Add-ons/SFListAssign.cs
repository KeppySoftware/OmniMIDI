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

namespace KeppySynthConfigurator
{
    public partial class KeppySynthSFListAssign : Form
    {
        public string LastBrowserPath { get; set; }

        public static string soundfontnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();
        public static string PathToAllLists = soundfontnewlocation + "\\Keppy's Synthesizer\\applists";
        public string[] ListsPath = new string[]
        {
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidi.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidib.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidic.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidid.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidie.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidif.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidig.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidih.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidii.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidij.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidik.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidil.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidim.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidin.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidio.applist",
            soundfontnewlocation + "\\Keppy's Synthesizer\\applists\\keppymidip.applist"
        };

        public int whichone { get; set; }

        public string CurrentList { get; set; }

        public KeppySynthSFListAssign()
        {
            InitializeComponent();
            RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
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
        }

        private void ChangeList(string WhichList)
        {
            try
            {
                if (!System.IO.Directory.Exists(PathToAllLists))
                {
                    Directory.CreateDirectory(PathToAllLists);
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
            DialogResult dialogResult = MessageBox.Show(String.Format("Are you sure you want to clear the list {0}?", whichone), "Assign a soundfont list to a specific app", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Lis.Items.Clear();
                SaveList(CurrentList);
            }
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
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
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
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                SynthPaths.SetValue("lastpathsfassign", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                LastBrowserPath = SynthPaths.GetValue("lastpathsfassign").ToString();
                AddApp.InitialDirectory = LastBrowserPath;
                SynthPaths.Close();      
            }
        }

        private void KeppyDriverSFListAssign_Load(object sender, EventArgs e)
        {
            SelectedListBox.Text = "List 1";
            Lis.ContextMenu = DefMenu;
            InitializeLastPath();
        }

        private void addAnAppToTheListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddApp.InitialDirectory = LastBrowserPath;
            if (AddApp.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in AddApp.FileNames)
                {
                    RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                    LastBrowserPath = Path.GetDirectoryName(str);
                    SynthPaths.SetValue("lastpathsfassign", LastBrowserPath, RegistryValueKind.String);
                    SynthPaths.Close();
                    AddAppToList(str);
                }
            }
        }

        private void addAnAppToTheListAppNameOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            RemoveAppFromList(ListsPath[SelectedListBox.SelectedIndex]);
        }

        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAppList();
        }

        private void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentList = ListsPath[SelectedListBox.SelectedIndex];
            whichone = SelectedListBox.SelectedIndex + 1;
            ChangeList(CurrentList);
        }
    }
}
