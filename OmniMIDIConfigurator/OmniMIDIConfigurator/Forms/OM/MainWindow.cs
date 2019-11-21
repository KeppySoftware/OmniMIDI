using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;

namespace OmniMIDIConfigurator
{
    public partial class MainWindow : Form
    {
        public static class OMWC
        {
            public static SoundFontListEditor SFLE;
            public static SettingsPanel SET;
            public static NoAvailableControl NAC;
        }

        private void DestroyNACEmbed(ref BufferedPanel P)
        {
            if (OMWC.NAC != null)
            {
                P.Controls.Remove(OMWC.NAC);
                OMWC.NAC.Dispose();
            }
        }

        private void CreateNACEmbed(ref BufferedPanel P, Exception E)
        {
            DestroyNACEmbed(ref P);

            OMWC.NAC = new NoAvailableControl(E);
            OMWC.NAC.Dock = DockStyle.Fill;
            OMWC.NAC.AutoScroll = false;
            P.Controls.Add(OMWC.NAC);

            MessageBox.Show(E.ToString());
        }

        private bool CreateSFLEEmbed(String[] SFs)
        {
            try
            {
                DestroyNACEmbed(ref SFLEPanel);

                if (OMWC.SFLE != null)
                {
                    OMWC.SFLE.CloseCSFWatcherExt();
                    SFLEPanel.Controls.Remove(OMWC.SFLE);
                    OMWC.SFLE.Dispose();
                }

                OMWC.SFLE = new SoundFontListEditor(SFs);
                OMWC.SFLE.Dock = DockStyle.Fill;
                OMWC.SFLE.AutoScroll = false;
                SFLEPanel.Controls.Add(OMWC.SFLE);

                return true;
            }
            catch (Exception ex)
            {
                CreateNACEmbed(ref SFLEPanel, ex);
            }

            return false;
        }

        private bool CreateSETEmbed()
        {
            try
            {
                DestroyNACEmbed(ref SETPanel);

                if (OMWC.SET != null)
                {
                    ApplySettings.Click -= new EventHandler(OMWC.SET.ButtonToSaveSettings);
                    RestoreDefault.Click -= new EventHandler(OMWC.SET.ButtonToResetSettings);

                    SETPanel.Controls.Remove(OMWC.SET);
                    OMWC.SET.Dispose();
                }

                OMWC.SET = new SettingsPanel();
                OMWC.SET.Dock = DockStyle.Fill;
                OMWC.SET.AutoScroll = false;
                OMWC.SET.HorizontalScroll.Enabled = false;
                OMWC.SET.HorizontalScroll.Visible = false;
                OMWC.SET.HorizontalScroll.Maximum = 0;
                OMWC.SET.AutoScroll = true;
                SETPanel.Controls.Add(OMWC.SET);

                QICombo.SelectedIndex = 0;

                ApplySettings.Click += new EventHandler(OMWC.SET.ButtonToSaveSettings);
                RestoreDefault.Click += new EventHandler(OMWC.SET.ButtonToResetSettings);

                return true;
            }
            catch (Exception ex)
            {
                CreateNACEmbed(ref SETPanel, ex);
            }

            return false;
        }

        private void SetHandCursor(object sender, EventArgs e)
        {
            Cursor = Program.SystemHandCursor;
        }

        private void SetDefaultCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        public MainWindow(String[] SFs)
        {
            // Initialize form
            InitializeComponent();

            // Fix
            MWSStrip.Padding = new Padding(
                MWSStrip.Padding.Left,
                MWSStrip.Padding.Top,
                MWSStrip.Padding.Left,
                MWSStrip.Padding.Bottom
                );
            MWTab_SelectedIndexChanged(null, null);

            // Check start location
            if (Properties.Settings.Default.LastWindowPos == new Point(-9999, -9999))
                this.StartPosition = FormStartPosition.CenterScreen;
            else
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Properties.Settings.Default.LastWindowPos;
            }

            // Check size
            if (Properties.Settings.Default.LastWindowSize != new Size(-1, -1))
                this.Size = Properties.Settings.Default.LastWindowSize;

            // Set menu
            Menu = OMMenu;

            // Set updater
            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\OmniMIDI\\OmniMIDI.dll");
            VersionLabel.Text = String.Format("Version {0}.{1}.{2}.{3}", Driver.FileMajorPart, Driver.FileMinorPart, Driver.FileBuildPart, Driver.FilePrivatePart);
 
            Shown += CheckUpdatesStartUp;
            DWCF.Checked = Properties.Settings.Default.DrawControlsFaster;

            // Check if MIDI mapper is available
            OMAPCpl.Visible = Functions.CheckMIDIMapper();

            // Add dynamic controls
            CreateSFLEEmbed(SFs);
            CreateSETEmbed();
        }

        // Wait for the resizing process to end before refreshing the window again
        protected override void OnResizeBegin(EventArgs e)
        {
            if (Properties.Settings.Default.DrawControlsFaster) SuspendLayout();
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            if (Properties.Settings.Default.DrawControlsFaster) ResumeLayout();
            base.OnResizeEnd(e);
        }
        // Wait for the resizing process to end before refreshing the window again

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == (int)Program.BringToFrontMessage)
            {
                WinAPI.ShowWindow(Handle, WinAPI.SW_RESTORE);
                WinAPI.SetForegroundWindow(Handle);
            }
        }

        private void MainWindow_ResizeEnd(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastWindowPos = this.Location;
            Properties.Settings.Default.LastWindowSize = new Size(this.Size.Width, this.Size.Height - 21);
            Properties.Settings.Default.Save();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // Nothing lul
        }

        private void MWTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            RestoreSFListEdWidth.Visible = (MWTab.SelectedIndex == 0);
        }

        private void DriverInfo_Click(object sender, EventArgs e)
        {
            new InfoWindow().ShowDialog();
        }

        private void ImportPres_Click(object sender, EventArgs e)
        {
            Functions.SettingsRegEditor(false);
            CreateSETEmbed();
        }

        private void ExportPres_Click(object sender, EventArgs e)
        {
            Functions.SettingsRegEditor(true);
        }

        private void QICombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (QICombo.SelectedIndex)
            {
                case 0:
                    OMWC.SET.ScrollControlIntoView(OMWC.SET.EnginesBox);
                    break;
                case 1:
                    OMWC.SET.ScrollControlIntoView(OMWC.SET.SynthBox);
                    break;
                case 2:
                    OMWC.SET.ScrollControlIntoView(OMWC.SET.LegacySetDia);
                    break;
                default:
                    OMWC.SET.ScrollControlIntoView(OMWC.SET.EnginesBox);
                    break;
            }
        }

        private void OpenDW_Click(object sender, EventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDebugWindow.exe");
        }

        private void OpenM_Click(object sender, EventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIMixerWindow.exe");
        }

        private void OpenBM_Click(object sender, EventArgs e)
        {
            new BlacklistSystem().ShowDialog();
        }

        private void OpenRTSSOSDM_Click(object sender, EventArgs e)
        {
            new RivaTunerSettings().ShowDialog();
        }

        private void AssignListToApp_Click(object sender, EventArgs e)
        {
            new SFListAssign().ShowDialog();
        }

        private void ChangeWAVOutput_Click(object sender, EventArgs e)
        {
            new OutputWAVDir().ShowDialog();
        }

        private void CloseConfigurator_Click(object sender, EventArgs e)
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

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConfigurator_Click(null, null);
        }

        private void OMDRegister_Click(object sender, EventArgs e)
        {
            Functions.DriverRegistry(false);
        }

        private void OMDUnregister_Click(object sender, EventArgs e)
        {
            Functions.DriverRegistry(true);
        }

        private void OMAPCpl_Click(object sender, EventArgs e)
        {
            new OmniMapperCpl().ShowDialog();
        }

        private void OMAPInstall_Click(object sender, EventArgs e)
        {
            Functions.MIDIMapRegistry(false);
            OMAPCpl.Visible = Functions.CheckMIDIMapper();
        }

        private void OMAPUninstall_Click(object sender, EventArgs e)
        {
            Functions.MIDIMapRegistry(true);
            OMAPCpl.Visible = Functions.CheckMIDIMapper();
        }

        private void InstallLM_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Program.SynthSettings.GetValue("AudioBitDepth", 1)) != 1)
            {
                DialogResult RES = Program.ShowError(3, "LoudMax", "LoudMax is useless without 32-bit float audio rendering.\nPlease enable it by going to \"Additional settings > Advanced audio settings > Audio bit depth\".\n\nDo you want to continue anyway?", null);
                if (RES == DialogResult.Yes) Functions.LoudMaxInstall();
            }
            else Functions.LoudMaxInstall();
        }

        private void UninstallLM_Click(object sender, EventArgs e)
        {
            Functions.LoudMaxUninstall();
        }

        private void WMWPatch_Click(object sender, EventArgs e)
        {
            new WinMMPatches().ShowDialog();
        }

        private void KACGuide_Click(object sender, EventArgs e)
        {
            Process.Start(
                String.Format(
                    "{0}#how-can-i-get-rid-of-the-annoying-smartscreen-block-screen-and-stop-chrome-from-warning-me-not-to-download-your-driver",
                    Properties.Settings.Default.ProjectLink
                    )
                );
        }

        private void MIDIInOutTest_Click(object sender, EventArgs e)
        {
            new MIDIInPlay().ShowDialog();
        }

        private void RestoreSFListEdWidth_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SFColumnsSize = Properties.Settings.Default.SFColumnsDefSize;
            Properties.Settings.Default.Save();
            CreateSFLEEmbed(null);
        }

        private void RestoreConfSettings_Click(object sender, EventArgs e)
        {
            DialogResult RES = Program.ShowError(3, "Reset configurator's internal settings", "Are you sure you want to reset the configurator's internal settings?\n\nYou'll lose all your custom settings, like the position of the window, the size of the columns in the SoundFonts editor etc...", null);
            if (RES == DialogResult.Yes)
            {
                String BeforeBranch = Properties.Settings.Default.UpdateBranch;

                Properties.Settings.Default.Reset();
                Properties.Settings.Default.UpdateBranch = BeforeBranch;
                Properties.Settings.Default.Save();

                new SelectBranch().ShowDialog();
                CreateSFLEEmbed(null);
                CreateSETEmbed();
                ApplySettings.PerformClick();
            }
        }

        private void DWCF_Click(object sender, EventArgs e)
        {
            switch (DWCF.Checked)
            {
                case true:
                    Properties.Settings.Default.DrawControlsFaster = false;
                    Properties.Settings.Default.Save();

                    CreateSETEmbed();

                    DWCF.Checked = false;
                    break;
                case false:
                default:
                    DialogResult RES = Program.ShowError(3, "Enable UI performance mode", "Enabling this setting could introduce flickering, but will reduce CPU usage and increase performance on weak computers.\n\nPress Yes if you want to continue and apply the setting.", null);
                    if (RES == DialogResult.Yes)
                    {
                        Properties.Settings.Default.DrawControlsFaster = true;
                        Properties.Settings.Default.Save();

                        CreateSETEmbed();

                        DWCF.Checked = true;
                    }
                    break;
            }
        }

        private void DeleteUserData_Click(object sender, EventArgs e)
        {
            DialogResult RES1 = Program.ShowError(3, "Clear user data", "Deleting the driver's user data will delete all the SoundFont lists, the DLL overrides and will also uninstall LoudMax.\nThis action is irreversible!\n\nAre you sure you want to continue?\nAfter deleting the data, the configurator will restart.", null);
            if (RES1 == DialogResult.Yes)
            {
                DialogResult RES2 = Program.ShowError(1, "Clear user data", "Would you like to restart the configurator after the process?", null);

                Functions.DeleteDirectory(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\OmniMIDI\\");

                if (RES2 == DialogResult.Yes)
                    System.Diagnostics.Process.Start(Application.ExecutablePath);

                this.Close();
            }
        }

        private void ReinstallDriver_Click(object sender, EventArgs e)
        {
            DialogResult RES = Program.ShowError(3, "Reinstall the driver", "Are you sure you want to reinstall the driver?\n\nThe configurator will download the latest installer, and remove all the old registry keys.\nYou'll lose ALL the settings.", null);
            if (RES == DialogResult.Yes)
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

        private void BugReport_Click(object sender, EventArgs e)
        {
            Process.Start(String.Format("{0}/issues/", Properties.Settings.Default.ProjectLink));
        }

        private void CFUBtn_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift) UpdateSystem.CheckForUpdates(true, false, false);
            else UpdateSystem.CheckForUpdates(false, false, false);
        }

        private void ChangelogCurrent_Click(object sender, EventArgs e)
        {
            try
            {
                FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateSystem.UpdateFileVersion);
                new ChangelogWindow(Driver.FileVersion.ToString(), false).ShowDialog();
            }
            catch { }
        }

        private void ChangelogLatest_Click(object sender, EventArgs e)
        {
            try
            {
                Octokit.Release Release = UpdateSystem.UpdateClient.Repository.Release.GetLatest("KeppySoftware", "OmniMIDI").Result;
                Version x = null;
                Version.TryParse(Release.TagName, out x);
                new ChangelogWindow(x.ToString(), false).ShowDialog();
            }
            catch (Exception ex)
            {
                Program.ShowError(
                    4, 
                    "Error", 
                    "An error has occured while interrogating GitHub for the latest release.\n" +
                    "This is not a serious error, it might mean that your IP has reached the maximum requests allowed to the GitHub servers.",
                    ex);
            }
        }

        private void KDMAPIDoc_Click(object sender, EventArgs e)
        {
            Process.Start(String.Format("{0}/blob/master/KDMAPI.md", Properties.Settings.Default.ProjectLink));
        }

        private void DLDriverSrc_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Settings.Default.ProjectLink);
        }

        private void CheckUpdatesStartUp(object sender, EventArgs e)
        {
            try { CheckUpdates.RunWorkerAsync(); } catch { }
        }

        private void CheckUpdates_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                SetDefaultCursor(sender, null);

                VersionLabel.Click -= CheckUpdatesStartUp;
                VersionLabel.Image = Properties.Resources.ReloadIcon;

                DriverInfo.Enabled = false;
                CFUBtn.Enabled = false;
                VersionLabel.Enabled = false;
            });

            String IUA = UpdateSystem.CheckForUpdatesMini().ToLowerInvariant();

            switch (IUA)
            {
                case "yes":
                    this.Invoke((MethodInvoker)delegate
                    {
                        VersionLabel.Click += CFUBtn_Click;
                        VersionLabel.Image = Properties.Resources.dlready;

                        DriverInfo.Enabled = true;
                        CFUBtn.Enabled = true;
                        VersionLabel.Enabled = true;
                    });
                    break;
                case "no":
                    this.Invoke((MethodInvoker)delegate
                    {
                        VersionLabel.Click += CheckUpdatesStartUp;
                        VersionLabel.Image = Properties.Resources.ok;

                        DriverInfo.Enabled = true;
                        CFUBtn.Enabled = true;
                        VersionLabel.Enabled = true;
                    });
                    break;
                default:
                    this.Invoke((MethodInvoker)delegate
                    {
                        VersionLabel.Click += CheckUpdatesStartUp;
                        VersionLabel.Image = Properties.Resources.dlerror;

                        DriverInfo.Enabled = true;
                        CFUBtn.Enabled = true;
                        VersionLabel.Enabled = true;
                    });
                    break;
            }
        }
    }
}
