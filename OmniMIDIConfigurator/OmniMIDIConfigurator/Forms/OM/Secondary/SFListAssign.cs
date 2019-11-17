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

namespace OmniMIDIConfigurator
{
    public partial class SFListAssign : Form
    {
        public static string soundfontnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();
        public static string PathToAllLists = soundfontnewlocation + "\\OmniMIDI\\applists";
        public string[] ListsPath = new string[]
        {
            soundfontnewlocation + "\\OmniMIDI\\applists\\OmniMIDI_B.applist",
            soundfontnewlocation + "\\OmniMIDI\\applists\\OmniMIDI_C.applist",
            soundfontnewlocation + "\\OmniMIDI\\applists\\OmniMIDI_D.applist",
            soundfontnewlocation + "\\OmniMIDI\\applists\\OmniMIDI_E.applist",
            soundfontnewlocation + "\\OmniMIDI\\applists\\OmniMIDI_F.applist",
            soundfontnewlocation + "\\OmniMIDI\\applists\\OmniMIDI_G.applist"
        };

        public string CurrentList { get; set; }

        public SFListAssign()
        {
            InitializeComponent();
        }

        private void ChangeList(string WhichList)
        {
            try
            {
                if (!Directory.Exists(PathToAllLists))
                {
                    Directory.CreateDirectory(PathToAllLists);
                    File.Create(WhichList).Dispose();
                    Lis.Items.Clear();
                    return;
                }
                if (!File.Exists(WhichList))
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
            DialogResult dialogResult = MessageBox.Show(String.Format("Are you sure you want to clear the list {0}?", SelectedListBox.SelectedIndex), "Assign a soundfont list to a specific app", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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

        private void KeppyDriverSFListAssign_Load(object sender, EventArgs e)
        {
            SelectedListBox.Text = "List 2";
            Lis.ContextMenu = DefMenu;
            AddApp.InitialDirectory = Properties.Settings.Default.LastBrowserPath;
        }

        private void addAnAppToTheListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddApp.InitialDirectory = Properties.Settings.Default.LastBrowserPath;

            if (AddApp.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.LastBrowserPath = Path.GetDirectoryName(AddApp.FileNames[0]);
                Properties.Settings.Default.Save();

                foreach (string str in AddApp.FileNames)
                    AddAppToList(str);
            }
        }

        private void addAnAppToTheListAppNameOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddApp.InitialDirectory = Properties.Settings.Default.LastBrowserPath;
            if (AddApp.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in AddApp.FileNames)
                    AddAppNameOnlyToList(str);

                Properties.Settings.Default.LastBrowserPath = Path.GetDirectoryName(AddApp.FileNames[0]);
                Properties.Settings.Default.Save();
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
            ChangeList(CurrentList);
        }
    }
}
