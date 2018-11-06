using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
// For SF info
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;
using System.Media;
using System.Net;
using System.Drawing.Text;
using Un4seen.BassAsio;
using Un4seen.BassWasapi;
using System.Threading;
using System.Text.RegularExpressions;

namespace OmniMIDIConfigurator
{
    public partial class OmniMIDIConfiguratorMain : Form
    {
        // Delegate for BasicFunc
        public static OmniMIDIConfiguratorMain Delegate;
        public static Boolean IsInternetAvailable = false;

        public static string LastBrowserPath { get; set; }
        public static string LastImportExportPath { get; set; }

        // SHA256
        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ReleaseID(StringBuilder SHA256Code, Int32 length);

        // Themes handler
        public static int CurrentTheme = -1;

        // Lists
        public static ListViewItem _itemDnD = null;
        public static string List1PathOld { get; set; }
        public static string List2PathOld { get; set; }
        public static string List3PathOld { get; set; }
        public static string List4PathOld { get; set; }
        public static string List5PathOld { get; set; }
        public static string List6PathOld { get; set; }
        public static string List7PathOld { get; set; }
        public static string List8PathOld { get; set; }

        // Buffer stuff
        public int CurrentIndexFreq { get; set; }

        // Lists
        public static string soundfontnewlocation = Environment.GetEnvironmentVariable("USERPROFILE");
        public static string AbsolutePath = soundfontnewlocation + "\\OmniMIDI";
        public static string PathToAllLists = soundfontnewlocation + "\\OmniMIDI\\lists";
        public static string DebugTextFiles = soundfontnewlocation + "\\OmniMIDI\\debug";
        public static string[] ListsPath = new string[]
        {
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_A.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_B.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_C.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_D.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_E.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_F.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_G.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_H.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_I.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_L.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_M.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_N.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_O.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_P.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_Q.omlist",
            soundfontnewlocation + "\\OmniMIDI\\lists\\OmniMIDI_R.omlist"
        };

        // Work
        public static string StatusTemplate = "{0}";
        public static List<string> tempList = new List<string> { };
        public static int applyfade = 0;
        public static int whichone { get; set; }
        public static string CurrentList { get; set; }
        public static bool AvoidSave = false;
        public static string[] Arguments = { } ;

        public static string[] RegValName = { "ch1", "ch2", "ch3", "ch4", "ch5", "ch6", "ch7", "ch8", "ch9", "ch10", "ch11", "ch12", "ch13", "ch14", "ch15", "ch16", "cha" };
        public static int[] RegValInt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static RegistryKey Mixer = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI", true);
        public static RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Configuration", true);
        public static RegistryKey Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Channels", true);
        public static RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Watchdog", true);
        public static RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);

        public OmniMIDIConfiguratorMain(String[] args)
        {
            InitializeComponent();
            StatusStrip.Padding = new Padding(StatusStrip.Padding.Left, StatusStrip.Padding.Top, StatusStrip.Padding.Left, StatusStrip.Padding.Bottom);
            Delegate = this;
            VolTrackBar.BackColor = Color.Empty;
            this.FormClosing += new FormClosingEventHandler(CloseConfigurator);
            Arguments = args;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == (int)Program.BringToFrontMessage)
            {
                WinAPI.ShowWindow(Handle, WinAPI.SW_RESTORE);
                WinAPI.SetForegroundWindow(Handle);
            }
        }

        private void OmniMIDIConfiguratorMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Bass.BASS_Free();
            Bass.FreeMe();
            SynthSettings.Close();
            Watchdog.Close();
        }

        private void SFZCompliant()
        {
            MessageBox.Show("This driver is \"SFZ format 2.0\" compliant.", "SFZ format support", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddSoundfontDragNDrop(String SelectedList, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            SFListFunc.AddSoundfontsToSelectedList(CurrentList, s);
        }

        public void AddSoundfontDragNDropTriv(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        // Here we go!
        private void OmniMIDIConfiguratorMain_Load(object sender, EventArgs e)
        {
            try
            {
                // Check for updates as soon as the form shows up
                Shown += CheckUpdatesStartUp;
                VolLabel.MouseWheel += VolumeMouseWheel;
                VolSimView.MouseWheel += VolumeMouseWheel;
                VolTrackBar.MouseWheel += VolumeMouseWheel;
                SettingsPresetsBtn.ContextMenu = SettingsPresets;
                TabsForTheControls.TabPages.Remove(DebugLog);

                // Initialize main menu
                Menu = SynthMenu;

                // SAS THEME HANDLER   
                Bass.LoadMe();
                ThemeCheck.RunWorkerAsync();
                // MIDI out selector disabler
                Functions.CheckMIDIMapper();

                if (Properties.Settings.Default.IsItPreRelease) Text += " (Pre-release build)";

                FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\OmniMIDI\\OmniMIDI.dll");
                VersionLabel.Text = String.Format("Version {0}.{1}.{2}.{3}", Driver.FileMajorPart, Driver.FileMinorPart, Driver.FileBuildPart, Driver.FilePrivatePart);

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

                VolTrackBar.ContextMenu = VolTrackBarMenu;
                VolLabel.ContextMenu = VolTrackBarMenu;
                VolSimView.ContextMenu = VolTrackBarMenu;

                TabsForTheControls.TabPages[0].ImageIndex = 0;
                TabsForTheControls.TabPages[1].ImageIndex = 1;

                whichone = 1;
                SelectedListBox.SelectedIndex = Properties.Settings.Default.LastListSelected;
                Functions.InitializeLastPath();

                Functions.LoadSettings(this);

                AudioEngBox_SelectedIndexChanged(null, null);

                if (tempList.Count > 0)
                {
                    foreach (string sf in tempList)
                    {
                        using (var tempForm = new AddToWhichList(sf))
                        {
                            var result = tempForm.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                CurrentList = tempForm.AddToFollowingList;
                                SelectedListBox.SelectedIndex = tempForm.Index;
                                List<string> SoundFontsToImport = new List<string> { };
                                SoundFontsToImport.Add(sf);
                                SFListFunc.AddSoundfontsToSelectedList(CurrentList, SoundFontsToImport.ToArray());
                            }
                        }
                    }
                    Application.ExitThread();
                }

                try
                {
                    foreach (String s in Arguments)
                    {
                        switch (s.ToUpper())
                        {
                            case "/AST":
                                TabsForTheControls.SelectedIndex = 1;
                                break;
                            case "/MIX":
                                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIMixerWindow.exe");
                                return;
                            default:
                                // do other stuff...
                                break;
                        }

                        if (Path.GetExtension(s).ToLowerInvariant() == ".sf2" || Path.GetExtension(s).ToLowerInvariant() == ".sfz" || Path.GetExtension(s).ToLowerInvariant() == ".sfpack")
                            tempList.Add(s);
                    }
                }
                catch (Exception ex)
                {
                    Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "Something went wrong during the starting process of the configurator.\n\nClick OK to continue.", true, ex);
                }
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "An error has occurred while loading the driver's settings.\n\nPress OK to reinstall the driver.", true, ex);
                var p = new Process();
                p.StartInfo.FileName = Application.ExecutablePath;
                p.StartInfo.Arguments = "/REI";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Application.ExitThread();
                return;
            }
        }

        private void CheckUpdatesStartUp(object sender, EventArgs e)
        {
            try { CheckUpdates.RunWorkerAsync(); } catch { }
        }

        private void VolumeMouseWheel(object sender, MouseEventArgs e)
        {
            Int32 Volume = VolTrackBar.Value;

            if (e.Delta > 0)
            {
                if (ModifierKeys == Keys.Shift) Volume += 500;
                else if (ModifierKeys == Keys.Control) Volume += 50;
                else Volume += 100;
            }
            else if (e.Delta < 0)
            {
                if (ModifierKeys == Keys.Shift) Volume -= 500;
                else if (ModifierKeys == Keys.Control) Volume -= 50;
                else Volume -= 100;
            }

            VolTrackBar.Value = Volume.LimitToRange(0, VolTrackBar.Maximum);
        }

        private void VolTrackBar_Scroll(object sender)
        {
            try
            {
                if (VolTrackBar.Value <= 49) VolSimView.ForeColor = Color.Red;
                else VolSimView.ForeColor = Color.FromArgb(255, 53, 0, 119);

                decimal VolVal = (decimal)VolTrackBar.Value / 100;
                VolSimView.Text = String.Format("{0}", Math.Round(VolVal, MidpointRounding.AwayFromZero).ToString());
                SynthSettings.SetValue("OutputVolume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, SystemSounds.Asterisk, "Error", "Error during access to the registry!", true, ex);
            }
        }

        private void VolumeBoost_Click(object sender, EventArgs e)
        {
            Functions.VolumeBoostSwitch();
        }

        private void FineTuneKnobIt_Click(object sender, EventArgs e)
        {
            new PreciseControlVol().ShowDialog();
        }

        private void ExportSettings_Click(object sender, EventArgs e)
        {
            if (ExportSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                Functions.ExportSettings(ExportSettingsDialog.FileName);
                MessageBox.Show("The settings have been exported to the selected registry file!", "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportSettings_Click(object sender, EventArgs e)
        {
            if (ImportSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                Functions.ImportSettings(this, ImportSettingsDialog.FileName);
                MessageBox.Show("The settings have been imported from the selected registry file!", "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CLi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the list?", "OmniMIDI Configurator ~ Clear list " + whichone.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Lis.Items.Clear();
                    File.Delete(CurrentList);
                    var TempFile = File.Create(CurrentList);
                    TempFile.Close();
                    if (Convert.ToInt32(Watchdog.GetValue("currentsflist")) == whichone)
                    {
                        Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
                    }
                }
                catch (Exception ex)
                {
                    SFListFunc.ReinitializeList(ex, CurrentList);
                }
            }
        }

        private void Lis_MouseDown(object sender, MouseEventArgs e)
        {
            Lis.PointToClient(new Point(e.X, e.Y));
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (Lis.SelectedIndices.Count == 1)
                        _itemDnD = Lis.GetItemAt(e.X, e.Y);
                    break;

                case MouseButtons.Right:
                    RightClickMenu.Show(Lis, new Point(e.X, e.Y));
                    break;

                case MouseButtons.Middle:
                    SelectedSFInfo(e);
                    break;
            }
        }

        private void Lis_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender != null || e != null)
                {
                    if (_itemDnD == null || Lis.SelectedIndices.Count > 1)
                        return;

                    Cursor = Cursors.UpArrow;

                    int lastItemBottom = Math.Min(e.Y, Lis.Items[Lis.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

                    ListViewItem itemOver = Lis.GetItemAt(0, lastItemBottom);

                    if (itemOver == null)
                        return;

                    Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
                    if (e.Y < rc.Top + (rc.Height / 2))
                    {
                        Lis.LineBefore = itemOver.Index;
                        Lis.LineAfter = -1;
                    }
                    else
                    {
                        Lis.LineBefore = -1;
                        Lis.LineAfter = itemOver.Index;
                    }
                }

                Lis.Invalidate();
            }
            catch { Lis.Invalidate(); }
        }

        private void Lis_MouseUp(object sender, MouseEventArgs e)
        {
            if (_itemDnD == null || Lis.SelectedIndices.Count > 1)
                return;

            try
            {
                int lastItemBottom = Math.Min(e.Y, Lis.Items[Lis.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

                ListViewItem itemOver = Lis.GetItemAt(0, lastItemBottom);

                if (itemOver == null)
                    return;

                Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);

                bool insertBefore;
                if (e.Y < rc.Top + (rc.Height / 2))
                {
                    insertBefore = true;
                }
                else
                {
                    insertBefore = false;
                }

                if (_itemDnD != itemOver)
                {
                    if (insertBefore)
                    {
                        Lis.Items.Remove(_itemDnD);
                        Lis.Items.Insert(itemOver.Index, _itemDnD);
                    }
                    else
                    {
                        Lis.Items.Remove(_itemDnD);
                        Lis.Items.Insert(itemOver.Index + 1, _itemDnD);
                    }
                }

                Lis.LineAfter =
                Lis.LineBefore = -1;

                Lis.Invalidate();
            }
            finally
            {
                _itemDnD = null;
                Cursor = Cursors.Default;
                SFListFunc.SaveList(CurrentList);
            }
        }

        private void OpenSFDefaultApp_Click(object sender, EventArgs e)
        {
            OpenSoundFont();
        }

        private void OpenSFMainDirectory_Click(object sender, EventArgs e)
        {
            OpenSoundFontDirectory();
        }

        private void SincInter_CheckedChanged(object sender, EventArgs e)
        {
            Functions.CheckSincEnabled();
        }

        private void ShowOutLevel_Click(object sender, EventArgs e)
        {
            if (ShowOutLevel.Checked != true)
            {
                SynthSettings.SetValue("volumemon", "1", RegistryValueKind.DWord);
                ShowOutLevel.Checked = true;
                Properties.Settings.Default.ShowOutputLevel = true;
                MixerBox.Visible = true;
                VolumeCheck.Enabled = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                ShowOutLevel.Checked = false;
                Properties.Settings.Default.ShowOutputLevel = false;
                MixerBox.Visible = false;
                VolumeCheck.Enabled = false;
                Properties.Settings.Default.Save();
            }
        }

        private void ShowMixerTools_Click(object sender, EventArgs e)
        {
            if (MeterFunc.CheckIfDedicatedMixerIsRunning(false)) return;

            if (ShowMixerTools.Checked != true)
            {
                MeterFunc.LoadChannelValues();
                this.ClientSize = new System.Drawing.Size(649, 630);
                SynthSettings.SetValue("volumemon", "1", RegistryValueKind.DWord);
                ShowOutLevel.Checked = true;
                ShowOutLevel.Enabled = false;
                ShowMixerTools.Checked = true;
                Properties.Settings.Default.ShowOutputLevel = false;
                Properties.Settings.Default.ShowMixerUnder = true;
                MixerBox.Visible = false;
                MixerPanel.Visible = true;
                VolumeCheck.Enabled = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(649, 442);
                ShowOutLevel.Checked = true;
                ShowOutLevel.Enabled = true;
                ShowMixerTools.Checked = false;
                Properties.Settings.Default.ShowOutputLevel = true;
                Properties.Settings.Default.ShowMixerUnder = false;
                MixerBox.Visible = true;
                MixerPanel.Visible = false;
                VolumeCheck.Enabled = true;
                Properties.Settings.Default.Save();
            }
        }

        private void DisableOLM_Click(object sender, EventArgs e)
        {
            ShowOutLevel.Checked = false;
            Properties.Settings.Default.ShowOutputLevel = false;
            MixerBox.Visible = false;
            VolumeCheck.Enabled = false;
            Properties.Settings.Default.Save();
        }

        private void OpenSoundFont()
        {
            try
            {
                int howmany = Lis.SelectedItems.Count;
                if (howmany == 1)
                {
                    String name = Lis.SelectedItems[0].Text.ToString();
                    if (SFListFunc.OpenSFWithDefaultApp(name)) Program.DebugToConsole(false, String.Format("Opened soundfont from list: {0}", name), null);
                    else Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", String.Format("The SoundFont \"{0}\" doesn't exist.", name), false, null);
                }
                else if (howmany > 1)
                {
                    for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        String name = Lis.SelectedItems[i].Text.ToString();
                        if (SFListFunc.OpenSFWithDefaultApp(name)) Program.DebugToConsole(false, String.Format("Opened soundfont from list: {0}", name), null);
                        else Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", String.Format("The SoundFont \"{0}\" doesn't exist.", name), false, null);
                    }
                }
            }
            catch (Exception ex)
            {
                SFListFunc.ReinitializeList(ex, CurrentList);
            }
        }

        private void OpenSoundFontDirectory()
        {
            try
            {
                int howmany = Lis.SelectedItems.Count;
                if (howmany == 1)
                {
                    String name = Lis.SelectedItems[0].Text.ToString();
                    if (SFListFunc.OpenSFDirectory(name)) Program.DebugToConsole(false, String.Format("Opened soundfont's root folder from list: {0}", name), null);
                    else Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", String.Format("The folder \"{0}\" doesn't exist.", Path.GetDirectoryName(name)), false, null);
                }
                else if (howmany > 1)
                {
                    for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        String name = Lis.SelectedItems[i].Text.ToString();
                        if (SFListFunc.OpenSFDirectory(name)) Program.DebugToConsole(false, String.Format("Opened soundfont's root folder from list: {0}", name), null);
                        else Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", String.Format("The folder \"{0}\" doesn't exist.", Path.GetDirectoryName(name)), false, null);
                    }
                }
            }
            catch (Exception ex)
            {
                SFListFunc.ReinitializeList(ex, CurrentList);
            }
        }

        private void AddSF_Click(object sender, EventArgs e)
        {
            try
            {
                SoundfontImport.InitialDirectory = LastBrowserPath;
                SoundfontImport.FileName = "";
                Functions.OpenFileDialogAddCustomPaths(SoundfontImport);
                if (SoundfontImport.ShowDialog(this) == DialogResult.OK)
                {
                    SFListFunc.SetLastPath(Path.GetDirectoryName(SoundfontImport.FileNames[0]));
                    SFListFunc.AddSoundfontsToSelectedList(CurrentList, SoundfontImport.FileNames);
                }
            }
            catch (Exception ex)
            {
                SFListFunc.ReinitializeList(ex, CurrentList);
            }
        }

        private void RmvSF_Click(object sender, EventArgs e)
        {
            try
            {
                Lis_MouseMove(null, null);
                if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                {
                    for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        String name = Lis.SelectedItems[i].Text.ToString();
                        Lis.Items.RemoveAt(Lis.SelectedIndices[i]);
                        Program.DebugToConsole(false, String.Format("Removed soundfont from list: {0}", name), null);
                        SFListFunc.SaveList(CurrentList);
                        SFListFunc.TriggerReload(false);
                    }
                }
            }
            catch (Exception ex)
            {
                SFListFunc.ReinitializeList(ex, CurrentList);
            }
        }

        private void Lis_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Apps)
                {
                    RightClickMenu.Show(Lis, new Point(Lis.SelectedItems[0].Position.X + 8, Lis.SelectedItems[0].Position.Y + 8));
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                    {
                        for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                        {
                            String name = Lis.SelectedItems[i].Text.ToString();
                            Lis.Items.RemoveAt(Lis.SelectedIndices[i]);
                            Program.DebugToConsole(false, String.Format("Removed soundfont from list: {0}", name), null);
                            SFListFunc.SaveList(CurrentList);
                            SFListFunc.TriggerReload(false);
                        }
                    }
                }
                else if (e.KeyCode == Keys.Up && e.Control)
                {
                    MoveListViewItems(Lis, MoveDirection.Up);
                }
                else if (e.KeyCode == Keys.Down && e.Control)
                {
                    MoveListViewItems(Lis, MoveDirection.Down);
                }
                else if (e.KeyCode == Keys.A && e.Control)
                {
                    Lis.MultiSelect = true;
                    foreach (ListViewItem item in Lis.Items)
                    {
                        item.Selected = true;
                    }
                }
                else if (e.KeyCode == Keys.C && e.Control)
                {
                    var builder = new StringBuilder();

                    if (Lis.SelectedItems.Count > 0)
                    {
                        builder.AppendLine("==OmniMIDISFList==");
                        foreach (ListViewItem item in Lis.SelectedItems)
                            builder.AppendLine(item.SubItems[0].Text);

                        Clipboard.SetText(builder.ToString());
                    }
                }
                else if (e.KeyCode == Keys.V && e.Control)
                {
                    String[] lines = Clipboard.GetText().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                    if (lines[0] == "==OmniMIDISFList==")
                    {
                        lines = lines.Skip(1).ToArray();
                        Array.Resize(ref lines, lines.Length - 1);
                        SFListFunc.AddSoundfontsToSelectedList(CurrentList, lines);
                    }
                }
            }
            catch (Exception ex)
            {
                SFListFunc.ReinitializeList(ex, CurrentList);
            }
        }

        private void CopyListToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in Lis.SelectedItems)
                builder.AppendLine(item.SubItems[0].Text);

            Clipboard.SetText(builder.ToString());
        }

        private void SelectedSFInfo(MouseEventArgs e)
        {
            AvoidSave = true;
            if (Lis.GetItemAt(e.X, e.Y) != null)
            {
                try
                {
                    _itemDnD = null;
                    Lis.LineAfter =
                    Lis.LineBefore = -1;
                    Lis.Invalidate();
                    Lis.SelectedItems.Clear();
                    Lis.GetItemAt(e.X, e.Y).Selected = true;
                    Program.DebugToConsole(false, String.Format("Currently showing info for soundfont: {0}", Lis.GetItemAt(e.X, e.Y).Text.ToString()), null);
                    SoundFontInfo frm = new SoundFontInfo(Lis.GetItemAt(e.X, e.Y).Text.ToString());
                    if (!SoundFontInfo.ERROR)
                    {
                        frm.ShowDialog(this);
                    }
                    SoundFontInfo.ERROR = false;
                    SoundFontInfo.Quitting = false;
                    frm.Dispose();
                }
                catch (Exception ex)
                {
                    Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex);
                    Environment.Exit(-1);
                }
            }
            AvoidSave = false;
        }

        private void MvU_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Up);
        }

        private void MvD_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Down);
        }

        private void EditSFSettings_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Lis.SelectedItems)
            {
                Boolean IsSFZ = (Path.GetExtension(item.Text) == ".sfz") ? false : true;
                Int32[] OldVals = new Int32[] {
                    Convert.ToInt32(item.SubItems[1].Text), Convert.ToInt32(item.SubItems[2].Text),
                    Convert.ToInt32(item.SubItems[3].Text), Convert.ToInt32(item.SubItems[4].Text),
                    (item.SubItems[5].Text == "Yes") ? 1 : 0
                };

                using (var form = new BankNPresetSel(Path.GetFileName(item.Text), true, IsSFZ, OldVals))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        item.SubItems[1].Text = form.PresetValueReturn.ToString();
                        item.SubItems[2].Text = form.BankValueReturn.ToString();
                        item.SubItems[3].Text = form.DesPresetValueReturn.ToString();
                        item.SubItems[4].Text = form.DesBankValueReturn.ToString();
                        item.SubItems[5].Text = form.XGModeC ? "Yes" : "No";
                    }
                }
            }
            SFListFunc.SaveList(CurrentList);
            SFListFunc.TriggerReload(false);
        }

        private enum MoveDirection { Up = -1, Down = 1 };
        private static void MoveListViewItems(ListView sender, MoveDirection direction)
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

                        if (direction == MoveDirection.Up) Program.DebugToConsole(false, String.Format("Moved SoundFont up: {0}", item.Text), null);
                        else Program.DebugToConsole(false, String.Format("Moved SoundFont down: {0}", item.Text), null);

                        sender.Items.RemoveAt(item.Index);
                        sender.Items.Insert(index, item);
                    }
                    SFListFunc.SaveList(CurrentList);
                    SFListFunc.TriggerReload(false);
                }
            }
            catch (Exception ex)
            {
                SFListFunc.ReinitializeList(ex, CurrentList);
            }
        }

        private void LoadToApp_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("currentsflist", whichone, RegistryValueKind.DWord);
            Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
            Program.DebugToConsole(false, String.Format("(Re)Loaded soundfont list {0}.", whichone), null);
        }

        private void EnableSF_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(true);
        }

        private void DisableSF_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(false);
        }

        private void menuItem35_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(true);
        }

        private void menuItem38_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(false);
        }

        private void menuItem54_Click(object sender, EventArgs e)
        {
            var builder = new StringBuilder();

            if (Lis.SelectedItems.Count > 0)
            {
                builder.AppendLine("==OmniMIDISFList==");
                foreach (ListViewItem item in Lis.SelectedItems)
                    builder.AppendLine(item.SubItems[0].Text);

                Clipboard.SetText(builder.ToString());
            }
        }

        private void menuItem55_Click(object sender, EventArgs e)
        {
            String[] lines = Clipboard.GetText().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (lines[0] == "==OmniMIDISFList==")
            {
                lines = lines.Skip(1).ToArray();
                Array.Resize(ref lines, lines.Length - 1);
                SFListFunc.AddSoundfontsToSelectedList(CurrentList, lines);
            }
        }

        private void SFEnableDisableSwitch(Boolean Enable)
        {
            try
            {
                if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                {
                    for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        if (Enable)
                        {
                            if (Lis.SelectedItems[i].ForeColor != SFListFunc.SFEnabled)
                            {
                                Lis.SelectedItems[i].ForeColor = SFListFunc.SFEnabled;
                                SFListFunc.SaveList(CurrentList);
                                SFListFunc.TriggerReload(false);
                                Program.DebugToConsole(false, String.Format("Enabled soundfont: {0}", Lis.SelectedItems[i].Text), null);
                            }
                        }
                        else
                        {
                            if (Lis.SelectedItems[i].ForeColor != SFListFunc.SFDisabled)
                            {
                                Lis.SelectedItems[i].ForeColor = SFListFunc.SFDisabled;
                                SFListFunc.SaveList(CurrentList);
                                SFListFunc.TriggerReload(false);
                                Program.DebugToConsole(false, String.Format("Disabled soundfont: {0}", Lis.SelectedItems[i].Text), null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SFListFunc.ReinitializeList(ex, CurrentList);
            }
        }

        private void IEL_Click(object sender, EventArgs e)
        {
            try
            {
                ExternalListImport.FileName = "";
                ExternalListImport.InitialDirectory = LastImportExportPath;
                if (ExternalListImport.ShowDialog(this) == DialogResult.OK)
                {
                    SFListFunc.SetLastImportExportPath(Path.GetDirectoryName(ExternalListImport.FileNames[0]));
                    foreach (string listw in ExternalListImport.FileNames)
                    {
                        using (StreamReader r = new StreamReader(listw))
                        {
                            Boolean AlreadyInitialized = false;

                            Boolean EnableState = false;
                            String SFPath = null;
                            Int32 SourcePreset = -1;
                            Int32 SourceBank = -1;
                            Int32 DestinationPreset = -1;
                            Int32 DestinationBank = 0;
                            Boolean XGDrumsetMode = false;

                            ListViewItem SF = new ListViewItem(new[] {
                                    "Unrecognizable SoundFont",
                                    "0", "0", "0", "0", "No",
                                    "Missing",
                                    "N/A"
                                    });
                            OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                            OmniMIDIConfiguratorMain.Delegate.Lis.Refresh();

                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                try
                                {
                                    if (line.Equals("sf.start"))
                                    {
                                        if (AlreadyInitialized) continue;

                                        // It begins, again...
                                        AlreadyInitialized = true;
                                    }
                                    else if (line.Equals("sf.end"))
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // Add it to the list.
                                        FileInfo file = new FileInfo(SFPath);
                                        SF = new ListViewItem(new[] {
                                        SFPath,
                                        SourcePreset.ToString(), SourceBank.ToString(), DestinationPreset.ToString(), DestinationBank.ToString(),
                                        (XGDrumsetMode ? "Yes" : "No"),
                                        SFListFunc.ReturnSoundFontFormat(Path.GetExtension(SFPath)),
                                        SFListFunc.ReturnSoundFontSize(SFPath, Path.GetExtension(SFPath), file.Length)
                                    });
                                        SF.ForeColor = EnableState ? SFListFunc.SFEnabled : SFListFunc.SFDisabled;
                                        Lis.Items.Add(SF);

                                        // Reset values
                                        EnableState = false;
                                        SFPath = null;
                                        SourcePreset = -1;
                                        SourceBank = -1;
                                        DestinationPreset = -1;
                                        DestinationBank = 0;
                                        XGDrumsetMode = false;

                                        AlreadyInitialized = false;
                                    }
                                    else if (SFListFunc.GetName(line).Equals("sf.path") && SFPath == null)
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // We've found the path! Parse it.
                                        SFPath = SFListFunc.GetValue(line);
                                    }
                                    else if (SFListFunc.GetName(line).Equals("sf.enabled"))
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // We've found the enable state! Crush it!
                                        EnableState = Convert.ToBoolean(Convert.ToInt32(SFListFunc.GetValue(line)));
                                    }
                                    else if (SFListFunc.GetName(line).Equals("sf.srcp"))
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // We've found the source preset! Take it!
                                        SourcePreset = Convert.ToInt32(SFListFunc.GetValue(line));
                                    }
                                    else if (SFListFunc.GetName(line).Equals("sf.srcb"))
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // We've found the source bank! Read it!
                                        SourceBank = Convert.ToInt32(SFListFunc.GetValue(line));
                                    }
                                    else if (SFListFunc.GetName(line).Equals("sf.desp"))
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // We've found the destination preset! Munch it!
                                        DestinationPreset = Convert.ToInt32(SFListFunc.GetValue(line));
                                    }
                                    else if (SFListFunc.GetName(line).Equals("sf.desb"))
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // We've found the destination preset! Munch it!
                                        DestinationBank = Convert.ToInt32(SFListFunc.GetValue(line));
                                    }
                                    else if (SFListFunc.GetName(line).Equals("sf.xgdrums"))
                                    {
                                        if (!AlreadyInitialized) continue;

                                        // We've found the enable state! Crush it!
                                        XGDrumsetMode = Convert.ToBoolean(Convert.ToInt32(SFListFunc.GetValue(line)));
                                    }
                                    else if (line.Contains("//") || line.Contains('#') || String.IsNullOrWhiteSpace(line)) continue;
                                    else
                                    {
                                        try
                                        {
                                            FileInfo file = new FileInfo(SFListFunc.StripSFZValues(line));
                                            String IsSFZ = (Path.GetExtension(SFListFunc.StripSFZValues(line)) == ".sfz") ? "0" : "-1";

                                            SF = new ListViewItem(new[] {
                                                SFListFunc.StripSFZValues(line),
                                                IsSFZ, IsSFZ, IsSFZ, "0", "No",
                                                SFListFunc.ReturnSoundFontFormat(Path.GetExtension(SFListFunc.StripSFZValues(line))),
                                                SFListFunc.ReturnSoundFontSize(SFListFunc.StripSFZValues(line), Path.GetExtension(SFListFunc.StripSFZValues(line)), file.Length)
                                            });
                                            SF.ForeColor = SFListFunc.SFEnabled;
                                            Lis.Items.Add(SF);
                                        }
                                        catch { }
                                    }
                                }
                                catch
                                {
                                    SF = new ListViewItem(new[] {
                                        "Unrecognizable SoundFont",
                                        "0", "0", "0", "0", "No",
                                        "Missing",
                                        "N/A"
                                    });

                                    SF.ForeColor = Color.Red;
                                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);

                                    Program.DebugToConsole(false, String.Format("{0} is missing.", line), null);
                                }
                            }
                        }
                        SFListFunc.SaveList(CurrentList);
                        SFListFunc.TriggerReload(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "Error during the import process of the list!", true, ex);
            }
        }

        private void EL_Click(object sender, EventArgs e)
        {
            ExternalListExport.FileName = "";
            ExternalListExport.InitialDirectory = LastImportExportPath;
            if (ExternalListExport.ShowDialog(this) == DialogResult.OK)
            {
                UInt32 SFCount = 1;
                SFListFunc.SetLastImportExportPath(Path.GetDirectoryName(ExternalListExport.FileNames[0]));
                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(ExternalListExport.FileName);
                SFListFunc.SetLastPath(LastBrowserPath);

                SaveFile.WriteLine("// Generated by OmniMIDI\n");
                foreach (ListViewItem item in Lis.Items)
                {
                    SaveFile.WriteLine(String.Format("// SoundFont n°{0}", SFCount));
                    SaveFile.WriteLine("sf.start");
                    SaveFile.WriteLine(String.Format("sf.path = {0}", item.Text));
                    SaveFile.WriteLine(String.Format("sf.enabled = {0}", (item.ForeColor == SFListFunc.SFEnabled) ? "1" : "0"));
                    SaveFile.WriteLine(String.Format("sf.srcp = {0}", item.SubItems[1].Text));
                    SaveFile.WriteLine(String.Format("sf.srcb = {0}", item.SubItems[2].Text));
                    SaveFile.WriteLine(String.Format("sf.desp = {0}", item.SubItems[3].Text));
                    SaveFile.WriteLine(String.Format("sf.desb = {0}", item.SubItems[4].Text));
                    SaveFile.WriteLine(String.Format("sf.xgdrums = {0}", (item.SubItems[5].Text.Equals("Yes")) ? "1" : "0"));
                    SaveFile.WriteLine("sf.end\n");
                    SFCount++;
                }
                SaveFile.WriteLine("// Generated by OmniMIDI");
                SaveFile.Close();
                Program.DebugToConsole(false, String.Format("Exported list {0} to {1}.", CurrentList, ExternalListExport.FileName), null);
                Functions.ShowErrorDialog(ErrorType.Information, System.Media.SystemSounds.Question, "Soundfont list exported!", String.Format("Soundfont list exported succesfully to \"{0}\\\"", Path.GetDirectoryName(ExternalListExport.FileName)), false, null);               
            }
        }

        // Color buttons

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
            // I bet you're wondering,
            ColorButton(AddSF, Pens.Violet, e);
        }

        private void ButtonUpDown(object sender, PaintEventArgs e)
        {
            // How can it set the color to two buttons, if you're only parsing one?
            ColorButton(MvU, Pens.Sienna, e);
        }

        private void ButtonEnable(object sender, PaintEventArgs e)
        {
            // Well, it's quite simple,
            ColorButton(EnableSF, Pens.Green, e);
        }

        private void ButtonDisable(object sender, PaintEventArgs e)
        {
            // Well, it's quite simple,
            ColorButton(DisableSF, Pens.Red, e);
        }

        private void ButtonLoad(object sender, PaintEventArgs e)
        {
            // The button parsed is only used to calculate the area of the rectangle,
            ColorButton(LoadToApp, Pens.PowderBlue, e);
        }

        private void ImportListButton(object sender, PaintEventArgs e)
        {
            // The actual button that is going to get the color is parsed through "e",
            ColorButton(IEL, new Pen(Color.FromArgb(114, 141, 208)), e);
        }

        private void ClearListButton(object sender, PaintEventArgs e)
        {
            // Ye, it's not really that easy to explain...
            ColorButton(CLi, Pens.BlueViolet, e);
        }

        // Stuff

        private void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SFListFunc.ChangeList(SelectedListBox.SelectedIndex);
            Properties.Settings.Default.LastListSelected = SelectedListBox.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void Lis_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(CurrentList, e);
        }

        private void Lis_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        // End of the soundfont lists functions
        // ------------------------------------

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        private void DeleteUserData_Click(object sender, EventArgs e)
        {
            Boolean RestartAfterDelete = false;
            DialogResult dialogResult = MessageBox.Show("Deleting the driver's user data will delete all the SoundFont lists, the DLL overrides and will also uninstall LoudMax.\nThis action is irreversible!\n\nAre you sure you want to continue?\nAfter deleting the data, the configurator will restart.", "OmniMIDI - Clear driver's user data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult2 = MessageBox.Show("Would you like to restart the configurator after the process?", "OmniMIDI - Clear driver's user data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult2 == DialogResult.Yes)
                    RestartAfterDelete = true;

                DeleteDirectory(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\OmniMIDI\\");
                if (RestartAfterDelete)
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 500, 75, 44100, 30, true, false, false, 2, true, false, false, AudioEngine.WASAPI_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 1);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            Program.DebugToConsole(false, "Settings restored.", null);
            MessageBox.Show("Settings restored to the default values!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void applySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Just save the Settings
            if (Functions.SaveSettings(this, ((ModifierKeys & Keys.Control) == Keys.Control))) EnableFade("Saved!", true);
            else EnableFade("Error!", false);
        }

        private void MSGSWSEmu_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 32, 75, 22050, 200, true, false, false, 0, false, false, false, AudioEngine.DSOUND_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(2, 0, 1, 1, 1, 0, 1);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 256, 1);
            Functions.ChangeDriverMask("Microsoft GS Wavetable Synth", 5, 0x0001, 0x001B);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("The preset has been applied!\n\nRemember to download the Microsoft GS Wavetable Synth SoundFont for the best \"experience\".", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void blackMIDIsPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 1000, 75, 44100, 20, true, false, false, 0, true, false, false, AudioEngine.WASAPI_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 1);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("The Black MIDIs preset has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lowLatencyPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 500, 80, 96000, 0, true, false, true, 2, true, false, false, AudioEngine.WASAPI_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 1);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("The low latency preset has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chiptunesRetrogamingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 16, 80, 22050, 50, true, false, false, 0, false, false, false, AudioEngine.DSOUND_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(3, 0, 1, 0, 1, 0, 1);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 384, 1);
            Functions.ChangeDriverMask("Keppy's Chiptune Emulator", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("The chiptunes/retrogaming preset has been applied!\n\n\"The NES soundfont\" is recommended for chiptunes.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void keppysSteinwayPianoRealismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 850, 80, 44100, 20, true, false, true, 3, true, false, false, AudioEngine.WASAPI_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 1);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("\"High fidelity audio (For HQ SoundFonts)\" has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SBLowLatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 750, 75, 44100, 20, true, false, false, 1, true, false, false, AudioEngine.WASAPI_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 1);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("\"SoundBlaster - Low Latency\" has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ProLowLatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 1000, 75, 48000, 20, true, false, false, 3, true, false, false, AudioEngine.ASIO_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 0);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("\"Professional environments - Low Latency\" has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MT32Mode_Click(object sender, EventArgs e)
        {
            // Set some values...
            Functions.ApplyPresetValues(10000, 1000, 75, 48000, 20, true, false, false, 0, true, false, false, AudioEngine.WASAPI_ENGINE);

            // Advanced settings here...
            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 0);
            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

            // And then...
            Functions.SaveSettings(this, true);

            // Messagebox here
            MessageBox.Show("\"Roland MT-32 Mode\" has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Now, menustrip functions here

        private void openDebugWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDebugWindow.exe");
        }

        private void openTheMixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIMixerWindow.exe");
        }

        private void openTheBlacklistManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BlacklistSystem().ShowDialog();
        }

        private void informationAboutTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, String.Format("Showing info about the driver."), null);
            InfoDialog frm = new InfoDialog(0);
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void SeeChangelog_Click(object sender, EventArgs e)
        {
            try
            {
                FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateSystem.UpdateFileVersion);
                Program.DebugToConsole(false, String.Format("Showing changelog of this release of the driver."), null);
                ChangelogWindow frm = new ChangelogWindow(Driver.FileVersion.ToString(), false);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            catch { }
        }

        private void SeeLatestChangelog_Click(object sender, EventArgs e)
        {
            try
            {
                Octokit.Release Release = UpdateSystem.UpdateClient.Repository.Release.GetLatest("KeppySoftware", "OmniMIDI").Result;
                Version x = null;
                Version.TryParse(Release.TagName, out x);
                Program.DebugToConsole(false, String.Format("Showing changelog of release {0} of the driver.", Release.TagName), null);
                ChangelogWindow frm = new ChangelogWindow(x.ToString(), false);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Information, System.Media.SystemSounds.Exclamation,
                    "Error", 
                    "An error has occured while interrogating GitHub for the latest release.\n" +
                    "This is not a serious error, it might mean that your IP has reached the maximum requests allowed to the GitHub servers.", 
                    true, ex);
            }
        }

        private void WMMPatches_Click(object sender, EventArgs e)
        {
            new WinMMPatches().ShowDialog();
        }

        private void changeDefaultMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Program.DebugToConsole(false, "Opening the MIDI out setter x86.", null);
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\midioutsetter32.exe");
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Asterisk, "Error", "Error while opening the MIDI out setter.", true, ex);
            }
        }

        private void changeDefault64bitMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Program.DebugToConsole(false, "Opening the MIDI out setter x64.", null);
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\midioutsetter64.exe");
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Asterisk, "Error", "Error while opening the MIDI out setter.", true, ex);
            }
        }

        private void openUpdaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift) UpdateSystem.CheckForUpdates(true, false, false);
            else UpdateSystem.CheckForUpdates(false, false, false);
        }

        private void SetHandCursor(object sender, EventArgs e)
        {
            Cursor = System.Windows.Forms.LinkLabelEx.SystemHandCursor;
        }

        private void SetDefaultCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void KSynthWiki_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KaleidonKep99/Keppy-s-Synthesizer/wiki");
        }

        private void GiveFeedback_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening feedback form.", null);
            Process.Start("https://goo.gl/forms/sLlghX6mzr3FWG5i2");
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to report a bug about OmniMIDI?\n\nHere are the requisites for a report:\n1) Make a video of the issue.\n2) Describe all the steps to reproduce the bug.\n3) Please give as much information as you can, to allow me (KaleidonKep99) to fix it as soon as possible.", "Report a bug...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver/issues");
            }
        }

        private void downloadTheSourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening OmniMIDI's GitHub page.", null);
            Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver");
        }

        private void donateToSupportUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening PayPal page for donation.", null);
            Process.Start("https://paypal.me/KaleidonKep99");
        }

        private void changeDirectoryOfTheOutputToWAVModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening WAV output dialog.", null);
            new OutputWAVDir().ShowDialog();
        }

        private void AASMenu_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening advanced audio settings.", null);
            new AdvancedAudioSettings().ShowDialog(this);
        }

        private void MEPSMenu_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening MIDI events parser settings.", null);
            new MIDIEventsParserSettings().ShowDialog(this);
        }

        private void changeDefaultSoundfontListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening default SoundFont list dialog.", null);
            new DefaultSFList().ShowDialog();
        }

        private void assignASoundfontListToASpecificAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening SoundFont list assign dialog.", null);
            new SFListAssign().ShowDialog();
        }

        private void manageFolderFavouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Opening folder favourites dialog.", null);
            new FavouritesManager().ShowDialog();
        }

        private void RegDriver_Click(object sender, EventArgs e)
        {
            Functions.DriverRegistry();
        }

        private void AMIDIMapInstallMenu_Click(object sender, EventArgs e)
        {
            Functions.MIDIMapRegistry(false);
        }

        private void AMIDIMapUninstallMenu_Click(object sender, EventArgs e)
        {
            Functions.MIDIMapRegistry(true);
        }

        // Priority stuff

        private void DePrio_Click(object sender, EventArgs e)
        {
            if (!DePrio.Checked)
            {
                Functions.ButtonStatus(false);
                DePrio.Checked = true;
                RTPrio.Checked = false;
                HiPrio.Checked = false;
                HNPrio.Checked = false;
                NoPrio.Checked = false;
                LNPrio.Checked = false;
                LoPrio.Checked = false;
                Functions.SetDriverPriority(0);
            }
            else
            {
                Functions.ButtonStatus(true);
                DePrio.Checked = false;
                RTPrio.Checked = true;
                HiPrio.Checked = false;
                HNPrio.Checked = false;
                NoPrio.Checked = false;
                LNPrio.Checked = false;
                LoPrio.Checked = false;
                Functions.SetDriverPriority(1);
            }
        }

        private void RTPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = true;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(1);
        }

        private void HiPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = true;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(2);
        }

        private void HNPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = true;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(3);
        }

        private void NoPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = true;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(4);
        }

        private void LNPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = true;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(5);
        }

        private void LoPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = true;
            Functions.SetDriverPriority(6);
        }

        // Priority stuff

        private void CloseConfigurator(object sender, CancelEventArgs e)
        {
            try
            {
                Bass.BASS_Free();
                Application.Exit();
            }
            catch
            {
                Application.Exit();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Bass.BASS_Free();
                Application.Exit();
            }
            catch
            {
                Application.Exit();
            }
        }

        // Guide part
        private void isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To open the configurator while playing a MIDI, press ALT+9.\nYou could also press ALT+0 to directly open the \"Settings\" tab.",
                "What are the hotkeys to open the configurator?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change the volume, simply press \"Add\" or \"Subtract\" buttons of the numeric keypad.\n\nYou can disable the hotkeys through \"Advanced settings > Volume hotkeys\".",
                "What are the hotkeys to change the volume?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void WhatsAutoPanic_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\"Automatic MIDI panic\" will tell the driver to kill all the active notes, when the CPU usage is equal or higher than 100%.", "What's the automatic MIDI panic?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StatusBuf_Click(object sender, EventArgs e)
        {
            Int32[] valuearray = new Int32[10];
            Functions.ChangeRecommendedBuffer(CurrentIndexFreq, out valuearray);
            String message = String.Format("You should.\n\nKeep the buffer size between {0} and {1}, which are the optimal values for performance and audio quality, when outputting the audio at a frequency of {2}Hz.\n\nIncreasing the buffer too much could lead to unexpected crashes during playback, while decreasing it could make the app not output any audio to the speakers.", valuearray[4], valuearray[5], Frequency.Text);
            MessageBox.Show(message, "Do I need to be careful with the buffer size setting?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SignatureCheck_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                Functions.GetSHA256OfDLLs(true);
            }
            else
            {
                Functions.GetSHA256OfDLLs(false);
            }
        }

        private void WhatIsXAudio_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Engines are used by the driver to interface with your computer's sound card and output the audio stream to your speakers or headphones.\n\n" +
                ".WAV mode allows you to export the audio to WAV files. More info by clicking \"What's .WAV mode?\".\n\n" +
                "DirectX Audio is deprecated, and I will not give support for it.\nIf you're encountering issues while using it, switch to WASAPI or ASIO.\n\n" +
                "If you are planning on doing high-end professional audio editing, you should choose ASIO, which achieves really low latencies at the cost of a bit more CPU usage.\n\n" +
                "Lastly, there is also WASAPI, which is capable of achieving low latencies with little CPU usage, thanks to its integration with the Windows kernel.",
                "What are engines?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WhatIsOutput_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("If you check this option, the driver will create a WAV file on your desktop, called \"(programname).exe - OmniMIDI Output File.wav\".\n\n" +
                "You can change the output directory by clicking \"More settings > Change WAV output directory\".\n\n" +
                "(The audio output to speakers/headphones will be disabled to avoid corrupting the audio export.)",
                "\".WAV mode\"? What is it?",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Frequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentIndexFreq = Frequency.SelectedIndex;
        }

        public void AudioEngBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.AudioEngBoxTrigger(false);
        }

        private void hotkeys_Click(object sender, EventArgs e)
        {
            if (hotkeys.Checked == false)
            {
                SynthSettings.SetValue("FastHotkeys", "1", RegistryValueKind.DWord);
                hotkeys.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("FastHotkeys", "0", RegistryValueKind.DWord);
                hotkeys.Checked = false;
            }
        }

        private void AutoLoad_Click(object sender, EventArgs e)
        {
            if (AutoLoad.Checked == false)
            {
                Properties.Settings.Default.AutoLoadList = true;
                AutoLoad.Checked = true;
            }
            else
            {
                Properties.Settings.Default.AutoLoadList = false;
                AutoLoad.Checked = false;
            }
            Properties.Settings.Default.Save();
        }

        private void enableextra8sf_Click(object sender, EventArgs e)
        {
            if (enableextra8sf.Checked == false)
            {
                SynthSettings.SetValue("extra8lists", "1", RegistryValueKind.DWord);
                enableextra8sf.Checked = true;
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
                SynthSettings.SetValue("extra8lists", "0", RegistryValueKind.DWord);
                enableextra8sf.Checked = false;
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
            }
        }

        private void DebugModePls_Click(object sender, EventArgs e)
        {
            if (DebugModePls.Checked == false)
            {
                SynthSettings.SetValue("DebugMode", "1", RegistryValueKind.DWord);
                DebugModePls.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("DebugMode", "0", RegistryValueKind.DWord);
                DebugModePls.Checked = false;
            }
        }

        private void LiveChangesTrigger_Click(object sender, EventArgs e)
        {
            if (LiveChangesTrigger.Checked == false)
            {
                if (Properties.Settings.Default.LiveChangesDisclaimer == true)
                {
                    DialogResult dialogResult = MessageBox.Show("Don't play too much with the live changes feature." +
                        "\n\nDoing so could lead to unexpected data loss and application crashes, please be careful with it." +
                        "\n\nClick \"Yes\" to enable the feature.", "OmniMIDI - Live changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Properties.Settings.Default.LiveChangesDisclaimer = false;
                        Properties.Settings.Default.LiveChanges = true;
                        Requirements.Active = false;
                        LiveChangesTrigger.Checked = true;
                    }
                }
                else
                {
                    Properties.Settings.Default.LiveChanges = true;
                    LiveChangesTrigger.Checked = true;
                    Requirements.Active = false;
                }
            }
            else
            {
                Properties.Settings.Default.LiveChanges = false;
                LiveChangesTrigger.Checked = false;
                Requirements.Active = true;
            }
            Properties.Settings.Default.Save();
        }

        private void DebugModeOpenNotepad_Click(object sender, EventArgs e)
        {
            String DirectoryDebug = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\debug\\";
            try
            {
                Process.Start(DirectoryDebug);
            }
            catch
            {
                Directory.CreateDirectory(DirectoryDebug);
                Process.Start(DirectoryDebug);
            }
        }

        private void SelfSignedCertificate_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KaleidonKep99/Keppy-s-Synthesizer#how-can-i-get-rid-of-the-annoying-smartscreen-block-screen-and-stop-chrome-from-warning-me-not-to-download-your-driver");
        }

        private void MaskSynthesizerAsAnother_Click(object sender, EventArgs e)
        {
            MaskSynthAsAnother frm = new MaskSynthAsAnother();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void SettingsPresetsBtn_Click(object sender, EventArgs e)
        {
            SettingsPresets.Show(SettingsPresetsBtn, new System.Drawing.Point(1, 20));
        }

        private void ImportPres_Click(object sender, EventArgs e)
        {
            Functions.ImportPreset(this);
        }

        private void ExportPres_Click(object sender, EventArgs e)
        {
            Functions.ExportPreset();
        }

        private void AMIDIMapCpl_Click(object sender, EventArgs e)
        {
            try
            {
                Program.DebugToConsole(false, "Opening the Alternative MIDI Mapper applet.", null);
                var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\OmniMIDI\\amidimap.cpl");
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Asterisk, "Error", "Can not open the Alternative MIDI Mapper applet!", true, ex);
            }
        }

        // Snap feature

        private const int SnapDist = 25;

        private bool DoSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= SnapDist;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            Screen scn = Screen.FromPoint(this.Location);
            if (DoSnap(this.Left, scn.WorkingArea.Left)) this.Left = scn.WorkingArea.Left;
            if (DoSnap(this.Top, scn.WorkingArea.Top)) this.Top = scn.WorkingArea.Top;
            if (DoSnap(scn.WorkingArea.Right, this.Right)) this.Left = scn.WorkingArea.Right - this.Width;
            if (DoSnap(scn.WorkingArea.Bottom, this.Bottom)) this.Top = scn.WorkingArea.Bottom - this.Height;
        }

        private void ThemeCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (VisualStyleInformation.IsEnabledByUser == true)
                    {
                        if (CurrentTheme != 1)
                        {
                            CurrentTheme = 1;
                            SoundFontTab.Invoke((MethodInvoker)delegate { SoundFontTab.BackColor = SystemColors.ControlLightLight; });
                            Settings.Invoke((MethodInvoker)delegate { Settings.BackColor = SystemColors.ControlLightLight; });
                            VolPanel.Invoke((MethodInvoker)delegate { VolPanel.BackColor = SystemColors.ControlLightLight; });
                            MixerPanel.Invoke((MethodInvoker)delegate { MixerPanel.BackColor = SystemColors.ControlLightLight; });
                            this.Invoke(new MethodInvoker(delegate { this.Refresh(); }));
                        }
                    }
                    else
                    {
                        if (CurrentTheme != 0)
                        {
                            CurrentTheme = 0;
                            SoundFontTab.Invoke((MethodInvoker)delegate { SoundFontTab.BackColor = SystemColors.Control; });
                            Settings.Invoke((MethodInvoker)delegate { Settings.BackColor = SystemColors.Control; });
                            VolPanel.Invoke((MethodInvoker)delegate { VolPanel.BackColor = SystemColors.Control; });
                            MixerPanel.Invoke((MethodInvoker)delegate { MixerPanel.BackColor = SystemColors.Control; });
                            this.Invoke(new MethodInvoker(delegate { this.Refresh(); }));
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CheckUpdates_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate 
            {
                SetDefaultCursor(sender, null);

                UpdateStatus.Click -= CheckUpdatesStartUp;
                VersionLabel.Click -= CheckUpdatesStartUp;

                UpdateStatus.Image = Properties.Resources.ReloadIcon;
                informationAboutTheDriverToolStripMenuItem.Enabled = false;
                UpdateStatus.Enabled = false;
                VersionLabel.Enabled = false;
                IsInternetAvailable = false;
                openUpdaterToolStripMenuItem.Enabled = false;
            });

            String IsUpdateAvailable = UpdateSystem.CheckForUpdatesMini();

            if (IsUpdateAvailable == "yes") this.Invoke((MethodInvoker)delegate 
            {
                UpdateStatus.Click += openUpdaterToolStripMenuItem_Click;
                VersionLabel.Click += openUpdaterToolStripMenuItem_Click;

                UpdateStatus.Image = Properties.Resources.dlready;
                informationAboutTheDriverToolStripMenuItem.Enabled = true;
                UpdateStatus.Enabled = true;
                VersionLabel.Enabled = true;
                IsInternetAvailable = true;
                openUpdaterToolStripMenuItem.Enabled = true;
            });
            else if (IsUpdateAvailable == "no") this.Invoke((MethodInvoker)delegate
            {
                UpdateStatus.Click += CheckUpdatesStartUp;
                VersionLabel.Click += CheckUpdatesStartUp;

                UpdateStatus.Image = Properties.Resources.dlnope;
                informationAboutTheDriverToolStripMenuItem.Enabled = true;
                UpdateStatus.Enabled = true;
                VersionLabel.Enabled = true;
                IsInternetAvailable = true;
                openUpdaterToolStripMenuItem.Enabled = true;
            });
            else this.Invoke((MethodInvoker)delegate {
                UpdateStatus.Click += CheckUpdatesStartUp;
                VersionLabel.Click += CheckUpdatesStartUp;

                UpdateStatus.Image = Properties.Resources.dlerror;
                informationAboutTheDriverToolStripMenuItem.Enabled = true;
                UpdateStatus.Enabled = true;
                VersionLabel.Enabled = true;
                IsInternetAvailable = false;
                openUpdaterToolStripMenuItem.Enabled = true;
            });
        }

        // Links

        private void SoftpediaPage_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.softpedia.com/get/Multimedia/Audio/Audio-Mixers-Synthesizers/Keppys-Synthesizer.shtml");
        }

        private void KepChannel_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCJeqODojIv4TdeHcBfHJRnA");
        }

        // Tools

        private void CheckIfUserPressesEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Just save the Settings
                if (Functions.SaveSettings(this, ((ModifierKeys & Keys.Control) == Keys.Control))) EnableFade("Saved!", true);
                else EnableFade("Error!", false);
            }
        }

        private bool Error = false;
        private string FadeText = "";
        private void EnableFade(string text, bool err)
        {
            FadeText = text;
            Error = err;
            SystemSounds.Question.Play();
            SavedLabel.Enabled = false;
            applyfade = 128;
            SavedLabel.Enabled = true;
        }

        private void SavedLabel_Tick(object sender, EventArgs e)
        {
            applySettingsToolStripMenuItem.Text = FadeText;
            applySettingsToolStripMenuItem.Font = new Font(applySettingsToolStripMenuItem.Font, FontStyle.Bold);
            applySettingsToolStripMenuItem.ForeColor = Color.FromArgb(Error ? 0 : applyfade, Error ? applyfade : 0, 0);
            applyfade--;
            if (applyfade <= 1)
            {
                SavedLabel.Enabled = false;
                applySettingsToolStripMenuItem.Text = "Apply settings";
                applySettingsToolStripMenuItem.Font = new Font(applySettingsToolStripMenuItem.Font, FontStyle.Regular);
                applySettingsToolStripMenuItem.ForeColor = SystemColors.ControlText;
            }
        }

        private void ResetToDefault_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to reinstall the driver?\n\nThe configurator will download the latest installer, and remove all the old registry keys.\nYou'll lose ALL the settings.", "OmniMIDI - Reinstall the driver from scratch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                p.StartInfo.Arguments = "/REI";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Application.ExitThread();
            }
        }

        // Credits

        private void HAPLink_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Showing HAP's NuGet page.", null);
            Process.Start("https://www.nuget.org/packages/HtmlAgilityPack/");
        }

        private void BASSLink_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Showing Un4seen Development's homepage.", null);
            Process.Start("http://www.un4seen.com/");
        }

        private void BASSNetLink_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Showing BASS.NET's homepage.", null);
            Process.Start("http://bass.radio42.com/");
        }

        private void FodyCredit_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Showing Fody's main GitHub page.", null);
            Process.Start("https://github.com/Fody");
        }

        private void OctokitDev_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Showing Octokit's official page.", null);
            Process.Start("https://developer.github.com/v3/libraries/");
        }

        private void KSUSJoinNow_Click(object sender, EventArgs e)
        {
            Program.DebugToConsole(false, "Creating Discord invite...", null);
            Process.Start("https://discord.gg/jUaHPrP");
        }

        private void MIDIInOutTest_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<MIDIInPlay>().Count() < 1)
                new MIDIInPlay().Show();
        }

        private void menuItem46_Click(object sender, EventArgs e)
        {
            Functions.OpenAdvancedAudioSettings("spatial", "This function requires Windows 10 Creators Update or newer.");
        }

        private void ChangeFromWindows_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Functions.OpenAdvancedAudioSettings("advanced", "An error has occurred while opening the audio settings.");
        }

        private void SetAssociationWithSFs_Click(object sender, EventArgs e)
        {
            Functions.SetAssociation();
        }

        bool alreadydone = false;
        private void VolumeCheck_Tick(object sender, EventArgs e)
        {
            try
            {
                if (AudioEngBox.SelectedIndex == 0)
                {
                    if (alreadydone != true)
                    {
                        MeterFunc.ChangeMeter(0, 0);
                        MeterFunc.ChangeMeter(1, 0);
                        MixerBox.Enabled = false;
                        alreadydone = true;
                    }
                }
                else
                {
                    if (alreadydone != false)
                    {
                        MixerBox.Enabled = true;
                        alreadydone = false;
                    }

                    try
                    {
                        if (Mixer.GetValueKind("leftvol") != RegistryValueKind.DWord ||
                            Mixer.GetValueKind("rightvol") != RegistryValueKind.DWord)
                        {
                            Mixer.SetValue("leftvol", 0, RegistryValueKind.DWord);
                            Mixer.SetValue("rightvol", 0, RegistryValueKind.DWord);
                        }
                    }
                    catch
                    {
                        try
                        {
                            Mixer.DeleteValue("leftvol");
                            Mixer.DeleteValue("rightvol");
                            Mixer.SetValue("leftvol", 0, RegistryValueKind.DWord);
                            Mixer.SetValue("rightvol", 0, RegistryValueKind.DWord);
                        }
                        catch { /* Can't do much about it */ }
                    }

                    int left = Convert.ToInt32(Mixer.GetValue("leftvol", 0));
                    int right = Convert.ToInt32(Mixer.GetValue("rightvol", 0));
                    var perc = ((double)((left + right) / 2) / 32768) * 100;

                    if (Properties.Settings.Default.ShowMixerUnder == true) VolLevel.Text = String.Format("{0}%", Math.Round(perc, 0));
                    else VolLevelS.Text = String.Format("{0}%", Math.Round(perc, 0));

                    if (Convert.ToInt32(SynthSettings.GetValue("MonoRendering", 0)) == 1)
                    {
                        MeterFunc.ChangeMeter(0, left);
                        MeterFunc.ChangeMeter(1, left);
                        MeterFunc.AverageMeter(left, left);
                    }
                    else
                    {
                        MeterFunc.ChangeMeter(0, left);
                        MeterFunc.ChangeMeter(1, right);
                        MeterFunc.AverageMeter(left, right);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        int paintReps = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (paintReps++ % 500 == 0)
                Application.DoEvents();
        }

        private void Level_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = ((Panel)sender).ClientRectangle;
            rect.Width--;
            rect.Height--;
            e.Graphics.DrawRectangle(Pens.Black, rect);
        }

        private void applySettingsToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            ColorButton(applySettingsToolStripMenuItem, Pens.Green, e);
        }

        private void SendTelemetry_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.TelemetryAlreadySeen)
            {
                DialogResult dialogResult = MessageBox.Show(String.Format("{0}\n\nClick \"Yes\" if you'd like to join, or click \"No\" if you don't want to.", Telemetry.Disclaimer),
                    "Telemetry - Disclaimer", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    Properties.Settings.Default.TelemetryAlreadySeen = true;
                    Properties.Settings.Default.Save();
                    new Telemetry().ShowDialog();
                }
            }
            else new Telemetry().ShowDialog();
        }

        private void KDMAPIDoc_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KeppySoftware/OmniMIDI/blob/master/KDMAPI.md");
        }

        private void DifferencePatches_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KeppySoftware/OmniMIDI/wiki/What's-the-difference-between-the-WinMM-patches%3F");
        }

        private void BugReport_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KeppySoftware/OmniMIDI/issues/");
        }

        // Mixer functions

        private void VolumeToolTip(string channel, TrackBar trackbar)
        {
            VolumeTip.SetToolTip(trackbar, String.Format("{0}: {1}%", channel, trackbar.Value));
        }

        private void CH16VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 16", CH16VOL);
            RegValInt[15] = CH16VOL.Value;
        }

        private void CH15VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 15", CH15VOL);
            RegValInt[14] = CH15VOL.Value;
        }

        private void CH14VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 14", CH14VOL);
            RegValInt[13] = CH14VOL.Value;
        }

        private void CH13VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 13", CH13VOL);
            RegValInt[12] = CH13VOL.Value;
        }

        private void CH12VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 12", CH12VOL);
            RegValInt[11] = CH12VOL.Value;
        }

        private void CH11VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 11", CH11VOL);
            RegValInt[10] = CH11VOL.Value;
        }

        private void CH10VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 10", CH10VOL);
            RegValInt[9] = CH10VOL.Value;
        }

        private void CH9VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 9", CH9VOL);
            RegValInt[8] = CH9VOL.Value;
        }

        private void CH8VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 8", CH8VOL);
            RegValInt[7] = CH8VOL.Value;
        }

        private void CH7VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 7", CH7VOL);
            RegValInt[6] = CH7VOL.Value;
        }

        private void CH6VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 6", CH6VOL);
            RegValInt[5] = CH6VOL.Value;
        }

        private void CH5VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 5", CH5VOL);
            RegValInt[4] = CH5VOL.Value;
        }

        private void CH4VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 4", CH4VOL);
            RegValInt[3] = CH4VOL.Value;
        }

        private void CH3VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 3", CH3VOL);
            RegValInt[2] = CH3VOL.Value;
        }

        private void CH2VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 2", CH2VOL);
            RegValInt[1] = CH2VOL.Value;
        }

        private void CH1VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 1", CH1VOL);
            RegValInt[0] = CH1VOL.Value;
        }

        private void MainVol_Scroll(object sender, EventArgs e)
        {
            CH1VOL.Value = CH2VOL.Value = CH3VOL.Value = CH4VOL.Value = CH5VOL.Value = CH6VOL.Value = CH7VOL.Value = CH8VOL.Value = CH9VOL.Value = CH10VOL.Value = CH11VOL.Value = CH12VOL.Value = CH13VOL.Value = CH14VOL.Value = CH15VOL.Value = CH16VOL.Value = MainVol.Value;
            VolumeToolTip("All", MainVol);
        }

        private void LED_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = LED.ClientRectangle;
            rect.Width--;
            rect.Height--;
            e.Graphics.DrawRectangle(Pens.White, rect);
        }

        private void ChannelVolume_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.ShowMixerUnder == true)
                {
                    if (Channels == null)
                    {
                        Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Channels");
                        Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Channels", true);
                    }
                    Channels.SetValue("ch1", CH1VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch2", CH2VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch3", CH3VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch4", CH4VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch5", CH5VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch6", CH6VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch7", CH7VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch8", CH8VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch9", CH9VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch10", CH10VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch11", CH11VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch12", CH12VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch13", CH13VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch14", CH14VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch15", CH15VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("ch16", CH16VOL.Value.ToString(), RegistryValueKind.DWord);
                    Channels.SetValue("cha", MainVol.Value.ToString(), RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not write settings to the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            System.Threading.Thread.Sleep(1);
        }

        private void OpenFullMixer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openTheMixerToolStripMenuItem_Click(sender, e);
            this.ClientSize = new System.Drawing.Size(649, 442);
            ShowOutLevel.Checked = true;
            ShowOutLevel.Enabled = true;
            ShowMixerTools.Checked = false;
            Properties.Settings.Default.ShowOutputLevel = true;
            Properties.Settings.Default.ShowMixerUnder = false;
            MixerBox.Visible = true;
            MixerPanel.Visible = false;
            VolumeCheck.Enabled = true;
            Properties.Settings.Default.Save();
        }

        // Debug list
        private string DebugListToAnalyze = null;

        private void DebugList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Functions.MonitorStop == false) Functions.MonitorStop = true;
            DebugListToAnalyze = DebugList.Text;

            using (var fs = new FileStream(DebugListToAnalyze, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default)) DebugLogShow.Text = sr.ReadToEnd();
            Thread.Sleep(10);

            DebugLogAnalyze.RunWorkerAsync();
        }

        private void RefreshDebugList_Click(object sender, EventArgs e)
        {
            DebugList.Items.Clear();

            string[] fileEntries = Directory.GetFiles(DebugTextFiles);
            foreach (string fileName in fileEntries)
            {
                if (Path.GetExtension(fileName).ToLower() == ".txt")
                    DebugList.Items.Add(fileName);
            }
        }

        private void DebugLogAnalyze_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                using (var fs = new FileStream(DebugListToAnalyze, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    while (sr.EndOfStream == false)
                    {
                        DebugLogShow.AppendText(Environment.NewLine + sr.ReadLine());
                        DebugLogShow.ScrollToCaret();
                    }
                }
            }
        }

        private void DebugLogAnalyze_DoWork(object sender, DoWorkEventArgs e)
        {
            Functions.MonitorTailOfFile(DebugListToAnalyze);
        }
    }
}