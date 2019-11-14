using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace OmniMIDIConfigurator
{
    public partial class SoundFontListEditor : UserControl
    {
        public static SoundFontListEditor Delegate;
        static bool Sizing = false;
        static FileSystemWatcher CSFWatcher;
        static Int32 SelectedIndexCSF;

        public SoundFontListEditor()
        {
            InitializeComponent();

            Delegate = this;

            // Spawn listener for shared list
            CSFWatcher = new FileSystemWatcher();
            CSFWatcher.Path = Path.GetDirectoryName(Program.ListsPath[0]);
            CSFWatcher.Filter = Path.GetFileName(Program.ListsPath[0]);
            CSFWatcher.NotifyFilter = NotifyFilters.LastWrite;
            CSFWatcher.Changed += new FileSystemEventHandler(OnChanged);
            CSFWatcher.Created += new FileSystemEventHandler(OnChanged);

            SFlg.BackgroundImage = Properties.Resources.Question;
            CLi.BackgroundImage = Properties.Resources.ClearIcon;
            AddSF.BackgroundImage = Properties.Resources.AddSFIcon;
            RmvSF.BackgroundImage = Properties.Resources.RmvSFIcon;
            MvU.BackgroundImage = Properties.Resources.MvUpIcon;
            MvD.BackgroundImage = Properties.Resources.MvDwIcon;
            LoadToApp.BackgroundImage = Properties.Resources.ReloadIcon;
            EnableSF.BackgroundImage = Properties.Resources.EnableIcon;
            DisableSF.BackgroundImage = Properties.Resources.DisableIcon;
            IEL.BackgroundImage = Properties.Resources.ImportIcon;
            EL.BackgroundImage = Properties.Resources.ExportIcon;

            SelectedListBox.SelectedIndex = Properties.Settings.Default.LastListSelected;
            if (SelectedListBox.SelectedIndex == 0) CSFWatcher.EnableRaisingEvents = true;
        }

        private void SoundFontListEditor_Load(object sender, EventArgs e)
        {
            // Nothing lul
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (SelectedIndexCSF == 0)
            {
                this.Invoke((Action)delegate {
                    SoundFontListExtension.ChangeList(SelectedIndexCSF, null, false, true);
                });    
            }
        }

        private void ReloadListAfterError(Exception ex)
        {
            DialogResult RES = Program.ShowError(
                3,
                "Error",
                String.Format(
                    "Oh snap!\nThe configurator encountered an error while editing the following list:\n{0}\n\nDo you want to reload the list?",
                    Program.ListsPath[SelectedListBox.SelectedIndex]
                ),
                null
            );

            if (RES == DialogResult.Yes)
                SoundFontListExtension.ChangeList(SelectedListBox.SelectedIndex, null, false, false);
        }

        private void ColorButton(Button button, Pen pen, PaintEventArgs e)
        {
            // Wew.
            Rectangle rect = button.ClientRectangle;
            rect.Width--;
            rect.Height--;
            e.Graphics.DrawRectangle(pen, rect);
        }

        private void ButtonAddRemove(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, Pens.Violet, e);
        }

        private void ButtonUpDown(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, Pens.Sienna, e);
        }

        private void ButtonEnable(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, Pens.Green, e);
        }

        private void ButtonDisable(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, Pens.Red, e);
        }

        private void ButtonLoad(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, Pens.PowderBlue, e);
        }

        private void ImportListButton(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, new Pen(Color.FromArgb(114, 141, 208)), e);
        }

        private void ClearListButton(object sender, PaintEventArgs e)
        {
            // Ye, it's not really that easy to explain...
            ColorButton((Button)sender, Pens.BlueViolet, e);
        }

        private void SoundFontListGuideButton(object sender, PaintEventArgs e)
        {
            // Ye, it's not really that easy to explain...
            ColorButton((Button)sender, Pens.Coral, e);
        }

        private void SFEDSwitch(bool status)
        {
            try
            {
                if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                {
                    for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        if (Lis.SelectedItems[i].ForeColor != (status ? SoundFontListExtension.SFEnabled : SoundFontListExtension.SFDisabled))
                        {
                            Lis.SelectedItems[i].ForeColor = (status ? SoundFontListExtension.SFEnabled : SoundFontListExtension.SFDisabled);
                        }
                    }

                    SoundFontListExtension.SaveList(SelectedListBox.SelectedIndex, null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }
        }

        private enum MoveDirection { Up = -1, Down = 1 };
        private void MoveListViewItems(ListView sender, MoveDirection direction)
        {
            try
            {
                int dir = (int)direction;
                int opp = dir * -1;

                bool valid = sender.SelectedItems.Count > 0 &&
                            ((direction == MoveDirection.Down && (sender.SelectedItems[sender.SelectedItems.Count - 1].Index < sender.Items.Count - 1))
                            || (direction == MoveDirection.Up && (sender.SelectedItems[0].Index > 0)));

                if (valid)
                {
                    foreach (ListViewItem item in sender.SelectedItems)
                    {
                        string str = item.Text;
                        int index = item.Index + dir;

                        sender.Items.RemoveAt(item.Index);
                        sender.Items.Insert(index, item);
                    }
                    SoundFontListExtension.SaveList(SelectedListBox.SelectedIndex, null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }
        }

        private void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexCSF = SelectedListBox.SelectedIndex;
            SoundFontListExtension.ChangeList(SelectedListBox.SelectedIndex, null, false, false);
            Properties.Settings.Default.LastListSelected = SelectedListBox.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void SFlg_Click(object sender, EventArgs e)
        {
            new TextReader("SoundFonts list guide", Properties.Resources.sflistguide).ShowDialog();
        }

        private void CLi_Click(object sender, EventArgs e)
        {
            DialogResult RES = Program.ShowError(1, String.Format("Clear {0}", SelectedListBox.Text), "Are you sure you want to clear this list?", null);
            if (RES == DialogResult.Yes)
            {
                try
                {
                    Lis.Items.Clear();
                    SoundFontListExtension.SaveList(SelectedListBox.SelectedIndex, null);
                    Program.ShowError(0, "Cleaning finished", String.Format("The list has been cleaned successfully.", ExternalListExport.FileName), null);

                }
                catch (Exception ex)
                {
                    ReloadListAfterError(ex);
                }
            }
        }

        private void AddSF_Click(object sender, EventArgs e)
        {
            try
            {
                if (SoundFontImport.ShowDialog(this) == DialogResult.OK)
                {
                    ListViewItem[] iSFs = SoundFontListExtension.AddSFToList(null, SoundFontImport.FileNames, BankPresetOverride.Checked);

                    if (iSFs != null)
                    {
                        foreach (ListViewItem iSF in iSFs)
                            Lis.Items.Add(iSF);

                        SoundFontListExtension.SaveList(SelectedListBox.SelectedIndex, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }
        }

        private void RmvSF_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                {
                    foreach (int index in Lis.SelectedIndices.Cast<int>().Select(x => x).Reverse())
                        Lis.Items.RemoveAt(index);

                    SoundFontListExtension.SaveList(SelectedListBox.SelectedIndex, null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }
        }

        private void LoadToApp_Click(object sender, EventArgs e)
        {
            Functions.LoadSoundFontList(SelectedListBox.SelectedIndex);
        }

        private void MvU_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Up);
        }

        private void MvD_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Down);
        }

        private void EnableSF_Click(object sender, EventArgs e)
        {
            SFEDSwitch(true);
        }

        private void DisableSF_Click(object sender, EventArgs e)
        {
            SFEDSwitch(false);
        }

        private void IEL_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            ExternalListImport.InitialDirectory = Properties.Settings.Default.LastImportExportPath;

            if (ExternalListImport.ShowDialog(this) == DialogResult.OK)
            {
                Properties.Settings.Default.LastImportExportPath = Path.GetDirectoryName(ExternalListImport.FileNames[0]);
                Properties.Settings.Default.Save();

                foreach (string ListW in ExternalListImport.FileNames)
                    SoundFontListExtension.ChangeList(-1, ListW, true, false);

                SoundFontListExtension.SaveList(SelectedListBox.SelectedIndex, null);

                Program.ShowError(0, "Import finished", "The selected lists have been imported successfully to the currently selected list in the configurator.", null);
            }
        }

        private void EL_Click(object sender, EventArgs e)
        {
            ExternalListExport.FileName = "";
            ExternalListExport.InitialDirectory = Properties.Settings.Default.LastImportExportPath;

            if (ExternalListExport.ShowDialog(this) == DialogResult.OK)
            {
                Properties.Settings.Default.LastImportExportPath = Path.GetDirectoryName(ExternalListExport.FileName);
                Properties.Settings.Default.Save();

                SoundFontListExtension.SaveList(-1, ExternalListExport.FileName);

                Program.ShowError(0, "Export finished", String.Format("The list has been exported successfully to \"{0}\".", ExternalListExport.FileName), null);
            }
        }
    }
}