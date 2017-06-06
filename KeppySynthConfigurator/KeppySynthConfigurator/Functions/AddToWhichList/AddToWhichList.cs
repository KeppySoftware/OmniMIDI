using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class AddToWhichList : Form
    {
        public string AddToFollowingList { get; set; }
        public int Index { get; set; }

        public AddToWhichList(String SF)
        {
            InitializeComponent();
            InfoMessage.Text = String.Format(InfoMessage.Text, Path.GetFileNameWithoutExtension(SF));
            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("extra8lists", 0)) == 1)
            {
                ListSel.Items.Add("List 9");
                ListSel.Items.Add("List 10");
                ListSel.Items.Add("List 11");
                ListSel.Items.Add("List 12");
                ListSel.Items.Add("List 13");
                ListSel.Items.Add("List 14");
                ListSel.Items.Add("List 15");
                ListSel.Items.Add("List 16");
            }
        }

        private void AddToWhichList_Load(object sender, EventArgs e)
        {
            ListSel.SelectedIndex = 0;
        }

        private void AddToList_Click(object sender, EventArgs e)
        {
            if (ListSel.SelectedIndex == 0)
                AddToFollowingList = KeppySynthConfiguratorMain.List1Path;
            else if (ListSel.SelectedIndex == 1)
                AddToFollowingList = KeppySynthConfiguratorMain.List2Path;
            else if (ListSel.SelectedIndex == 2)
                AddToFollowingList = KeppySynthConfiguratorMain.List3Path;
            else if (ListSel.SelectedIndex == 3)
                AddToFollowingList = KeppySynthConfiguratorMain.List4Path;
            else if (ListSel.SelectedIndex == 4)
                AddToFollowingList = KeppySynthConfiguratorMain.List5Path;
            else if (ListSel.SelectedIndex == 5)
                AddToFollowingList = KeppySynthConfiguratorMain.List6Path;
            else if (ListSel.SelectedIndex == 6)
                AddToFollowingList = KeppySynthConfiguratorMain.List7Path;
            else if (ListSel.SelectedIndex == 7)
                AddToFollowingList = KeppySynthConfiguratorMain.List8Path;
            else if (ListSel.SelectedIndex == 8)
                AddToFollowingList = KeppySynthConfiguratorMain.List9Path;
            else if (ListSel.SelectedIndex == 9)
                AddToFollowingList = KeppySynthConfiguratorMain.List10Path;
            else if (ListSel.SelectedIndex == 10)
                AddToFollowingList = KeppySynthConfiguratorMain.List11Path;
            else if (ListSel.SelectedIndex == 11)
                AddToFollowingList = KeppySynthConfiguratorMain.List12Path;
            else if (ListSel.SelectedIndex == 12)
                AddToFollowingList = KeppySynthConfiguratorMain.List13Path;
            else if (ListSel.SelectedIndex == 13)
                AddToFollowingList = KeppySynthConfiguratorMain.List14Path;
            else if (ListSel.SelectedIndex == 14)
                AddToFollowingList = KeppySynthConfiguratorMain.List15Path;
            else if (ListSel.SelectedIndex == 15)
                AddToFollowingList = KeppySynthConfiguratorMain.List16Path;
            else
                AddToFollowingList = KeppySynthConfiguratorMain.List1Path;

            Index = ListSel.SelectedIndex;

            DialogResult = DialogResult.OK;
        }
    }
}
