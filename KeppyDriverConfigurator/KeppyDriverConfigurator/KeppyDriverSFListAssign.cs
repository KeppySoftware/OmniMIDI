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
        public string List1Path { get; set; }
        public string List2Path { get; set; }
        public string List3Path { get; set; }
        public string List4Path { get; set; }
        public string List5Path { get; set; }
        public string List6Path { get; set; }
        public string List7Path { get; set; }
        public string List8Path { get; set; }
        public bool listswitch { get; set; }

        public KeppyDriverSFListAssign()
        {
            InitializeComponent();
        }

        private void Load1234()
        {
            try
            {
                using (StreamReader r = new StreamReader(List1Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox1.Items.Add(line); // The program is copying the entire text file to the List 1's listbox.
                    }
                }
                using (StreamReader r = new StreamReader(List2Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox2.Items.Add(line); // The program is copying the entire text file to the List 2's listbox.
                    }
                }
                using (StreamReader r = new StreamReader(List3Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox3.Items.Add(line); // The program is copying the entire text file to the List 3's listbox.
                    }
                }
                using (StreamReader r = new StreamReader(List4Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox4.Items.Add(line); // The program is copying the entire text file to the List 4's listbox.
                    }
                }
            }
            catch
            {

            }
        }

        private void Load5678()
        {
            try
            {
                using (StreamReader r = new StreamReader(List5Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox5.Items.Add(line); // The program is copying the entire text file to the List 5's listbox.
                    }
                }
                using (StreamReader r = new StreamReader(List6Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox6.Items.Add(line); // The program is copying the entire text file to the List 6's listbox.
                    }
                }
                using (StreamReader r = new StreamReader(List7Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox7.Items.Add(line); // The program is copying the entire text file to the List 7's listbox.
                    }
                }
                using (StreamReader r = new StreamReader(List8Path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox8.Items.Add(line); // The program is copying the entire text file to the List 8's listbox.
                    }
                }
            }
            catch
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

        private void SaveList(String SelectedList, ListBox OriginalList)
        {
            using (StreamWriter sw = new StreamWriter(SelectedList))
            {
                foreach (var item in OriginalList.Items)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }

        private void ClearAppListTrigger(ListBox list, string listtosave)
        {
            list.Items.Clear();
            SaveList(listtosave, list);
        }

        private void ClearAppList(string selectedlist)
        {
            if (selectedlist == "listBox1")
            {
                ClearAppListTrigger(listBox1, List1Path);
            }
            else if (selectedlist == "listBox2")
            {
                ClearAppListTrigger(listBox2, List2Path);
            }
            else if (selectedlist == "listBox3")
            {
                ClearAppListTrigger(listBox3, List3Path);
            }
            else if (selectedlist == "listBox4")
            {
                ClearAppListTrigger(listBox4, List4Path);
            }
            else if (selectedlist == "listBox5")
            {
                ClearAppListTrigger(listBox5, List5Path);
            }
            else if (selectedlist == "listBox6")
            {
                ClearAppListTrigger(listBox6, List6Path);
            }
            else if (selectedlist == "listBox7")
            {
                ClearAppListTrigger(listBox7, List7Path);
            }
            else if (selectedlist == "listBox8")
            {
                ClearAppListTrigger(listBox8, List8Path);
            }
        }

        private void RemoveAppFromListTrigger(ListBox list, string listtosave)
        {
            for (int i = list.SelectedIndices.Count - 1; i >= 0; i--)
            {
                list.Items.RemoveAt(list.SelectedIndices[i]);
            }
            SaveList(listtosave, list);
        }

        private void RemoveAppFromList(string selectedlist)
        {
            if (selectedlist == "listBox1")
            {
                RemoveAppFromListTrigger(listBox1, List1Path);
            }
            else if (selectedlist == "listBox2")
            {
                RemoveAppFromListTrigger(listBox2, List2Path);
            }
            else if (selectedlist == "listBox3")
            {
                RemoveAppFromListTrigger(listBox3, List3Path);
            }
            else if (selectedlist == "listBox4")
            {
                RemoveAppFromListTrigger(listBox4, List4Path);
            }
            else if (selectedlist == "listBox5")
            {
                RemoveAppFromListTrigger(listBox5, List5Path);
            }
            else if (selectedlist == "listBox6")
            {
                RemoveAppFromListTrigger(listBox6, List6Path);
            }
            else if (selectedlist == "listBox7")
            {
                RemoveAppFromListTrigger(listBox7, List7Path);
            }
            else if (selectedlist == "listBox8")
            {
                RemoveAppFromListTrigger(listBox8, List8Path);
            }
        }

        private void AddAppToListTrigger(ListBox list, string str, string listtosave)
        {
            list.Items.Add(str);
            SaveList(listtosave, list);
        }

        private void AddAppToList(string selectedlist, string str) {
            if (selectedlist == "listBox1")
            {
                AddAppToListTrigger(listBox1, str, List1Path);
            }
            else if (selectedlist == "listBox2")
            {
                AddAppToListTrigger(listBox2, str, List2Path);
            }
            else if (selectedlist == "listBox3")
            {
                AddAppToListTrigger(listBox3, str, List3Path);
            }
            else if (selectedlist == "listBox4")
            {
                AddAppToListTrigger(listBox4, str, List4Path);
            }
            else if (selectedlist == "listBox5")
            {
                AddAppToListTrigger(listBox5, str, List5Path);
            }
            else if (selectedlist == "listBox6")
            {
                AddAppToListTrigger(listBox6, str, List6Path);
            }
            else if (selectedlist == "listBox7")
            {
                AddAppToListTrigger(listBox7, str, List7Path);
            }
            else if (selectedlist == "listBox8")
            {
                AddAppToListTrigger(listBox8, str, List8Path);
            }
        }

        private void AddAppNameOnlyToListTrigger(ListBox list, string str, string listtosave)
        {
            list.Items.Add(Path.GetFileName(str));
            SaveList(listtosave, list);
        }

        private void AddAppNameOnlyToList(string selectedlist, string str)
        {
            if (selectedlist == "listBox1")
            {
                AddAppNameOnlyToListTrigger(listBox1, str, List1Path);
            }
            else if (selectedlist == "listBox2")
            {
                AddAppNameOnlyToListTrigger(listBox2, str, List2Path);
            }
            else if (selectedlist == "listBox3")
            {
                AddAppNameOnlyToListTrigger(listBox3, str, List3Path);
            }
            else if (selectedlist == "listBox4")
            {
                AddAppNameOnlyToListTrigger(listBox4, str, List4Path);
            }
            else if (selectedlist == "listBox5")
            {
                AddAppNameOnlyToListTrigger(listBox5, str, List5Path);
            }
            else if (selectedlist == "listBox6")
            {
                AddAppNameOnlyToListTrigger(listBox6, str, List6Path);
            }
            else if (selectedlist == "listBox7")
            {
                AddAppNameOnlyToListTrigger(listBox7, str, List7Path);
            }
            else if (selectedlist == "listBox8")
            {
                AddAppNameOnlyToListTrigger(listBox8, str, List8Path);
            }
        }

        private void InitializeLists()
        {
            string soundfontnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();

            // Initialize the eight list paths
            List1Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidi.applist";
            List2Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidib.applist";
            List3Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidic.applist";
            List4Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidid.applist";
            List5Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidie.applist";
            List6Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidif.applist";
            List7Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidig.applist";
            List8Path = soundfontnewlocation + "\\Keppy's Driver\\applists\\keppymidih.applist";

            // ======= Read soundfont lists
            try
            {
                if (!System.IO.Directory.Exists(soundfontnewlocation + "\\Keppy's Driver\\applists\\"))
                {
                    System.IO.Directory.CreateDirectory(soundfontnewlocation + "\\Keppy's Driver\\applists\\");
                    File.Create(List1Path).Dispose();
                    File.Create(List2Path).Dispose();
                    File.Create(List3Path).Dispose();
                    File.Create(List4Path).Dispose();
                    File.Create(List5Path).Dispose();
                    File.Create(List6Path).Dispose();
                    File.Create(List7Path).Dispose();
                    File.Create(List8Path).Dispose();
                }
                else
                {
                    try
                    {
                        using (StreamReader r = new StreamReader(List1Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox1.Items.Add(line); // The program is copying the entire text file to the List 1's listbox.
                            }
                        }
                        using (StreamReader r = new StreamReader(List2Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox2.Items.Add(line); // The program is copying the entire text file to the List 2's listbox.
                            }
                        }
                        using (StreamReader r = new StreamReader(List3Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox3.Items.Add(line); // The program is copying the entire text file to the List 3's listbox.
                            }
                        }
                        using (StreamReader r = new StreamReader(List4Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox4.Items.Add(line); // The program is copying the entire text file to the List 4's listbox.
                            }
                        }
                        using (StreamReader r = new StreamReader(List5Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox5.Items.Add(line); // The program is copying the entire text file to the List 5's listbox.
                            }
                        }
                        using (StreamReader r = new StreamReader(List6Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox6.Items.Add(line); // The program is copying the entire text file to the List 6's listbox.
                            }
                        }
                        using (StreamReader r = new StreamReader(List7Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox7.Items.Add(line); // The program is copying the entire text file to the List 7's listbox.
                            }
                        }
                        using (StreamReader r = new StreamReader(List8Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                listBox8.Items.Add(line); // The program is copying the entire text file to the List 8's listbox.
                            }
                        }
                    }
                    catch
                    {
                        // If the program fails at reading the lists, it'll create them for you
                        if (File.Exists(List1Path) == false)
                        {
                            File.Create(List1Path).Dispose();
                        }
                        if (File.Exists(List2Path) == false)
                        {
                            File.Create(List2Path).Dispose();
                        }
                        if (File.Exists(List3Path) == false)
                        {
                            File.Create(List3Path).Dispose();
                        }
                        if (File.Exists(List4Path) == false)
                        {
                            File.Create(List4Path).Dispose();
                        }
                        if (File.Exists(List5Path) == false)
                        {
                            File.Create(List5Path).Dispose();
                        }
                        if (File.Exists(List6Path) == false)
                        {
                            File.Create(List6Path).Dispose();
                        }
                        if (File.Exists(List7Path) == false)
                        {
                            File.Create(List7Path).Dispose();
                        }
                        if (File.Exists(List8Path) == false)
                        {
                            File.Create(List8Path).Dispose();
                        }
                        MessageBox.Show("One of the app lists were missing, so the configurator automatically restored them.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal error during the execution of the program.\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void KeppyDriverSFListAssign_Load(object sender, EventArgs e)
        {
            InitializeLists();
        }

        private void addAnAppToTheListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control LeControl = WhoTriggeredMe(sender);
            if (AddApp.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in AddApp.FileNames)
                {
                    AddAppToList(LeControl.Name, str);
                }
            }
        }

        private void addAnAppToTheListAppNameOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control LeControl = WhoTriggeredMe(sender);
            if (AddApp.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in AddApp.FileNames)
                {
                    AddAppNameOnlyToList(LeControl.Name, str);
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
            ClearAppList(LeControl.Name);
        }
    }
}
