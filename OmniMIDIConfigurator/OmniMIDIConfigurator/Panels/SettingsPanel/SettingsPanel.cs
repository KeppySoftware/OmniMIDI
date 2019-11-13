using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace OmniMIDIConfigurator
{
    public partial class SettingsPanel : UserControl
    {
        public static SettingsPanel Delegate;

        public SettingsPanel()
        {
            InitializeComponent();

            Delegate = this;

            KSDAPIBoxWhat.Image = Properties.Resources.what;
            OverrideNoteLengthWA1.Image = Properties.Resources.wi;
            OverrideNoteLengthWA2.Image = Properties.Resources.wi;

            VolTrackBar.ContextMenu = VolTrackBarMenu;

            if (!(Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build >= 15063))
            {
                SpatialSound.Enabled = false;
                SpatialSound.Text += " (Not available on your version of Windows)";
            }

            LoadSettings();
        }

        private void CheckSincEnabled()
        {
            SincConvLab.Enabled = SincInter.Checked;
            SincConv.Enabled = SincInter.Checked;
        }

        private void AudioEngBoxTrigger(bool S)
        {
            int AE = AudioEngBox.SelectedIndex;
            bool NOAtWASIOE = (AE != AudioEngine.AUDTOWAV && AE != AudioEngine.ASIO_ENGINE);
            bool NoAtW = (AE != AudioEngine.AUDTOWAV);
            bool NoASIOE = (AE != AudioEngine.ASIO_ENGINE);

            switch (AudioEngBox.SelectedIndex)
            {
                case AudioEngine.WASAPI_ENGINE:
                case AudioEngine.DSOUND_ENGINE:
                default:
                    BufferText.Text = "Output buffer (in ms, from 1 to 1000. If the buffer is too small, it'll be set automatically to the lowest value possible)";
                    break;
                case AudioEngine.AUDTOWAV:
                    BufferText.Text = "The output buffer isn't needed when outputting to a .WAV file";
                    break;
                case AudioEngine.ASIO_ENGINE:
                    if (DefaultASIOAudioOutput.GetASIODevicesCount() < 1)
                    {
                        Program.ShowError(4, "Error", "No ASIO devices installed!\n\nClick OK to switch to WASAPI.", null);
                        AudioEngBox.SelectedIndex = 3;
                        AudioEngBoxTrigger(true);
                        return;
                    }

                    BufferText.Text = "The output buffer is controlled by the ASIO device itself";
                    break;
            }

            ChangeDefaultOutput.Enabled = NoAtW ? true : false;
            BufferText.Enabled = NOAtWASIOE ? true : false;
            DrvHzLabel.Enabled = true;
            Frequency.Enabled = true;
            MaxCPU.Enabled = NoAtW ? true : false;
            RenderingTimeLabel.Enabled = NoAtW ? true : false;
            VolLabel.Enabled = NoAtW ? true : false;
            VolSimView.Enabled = NoAtW ? true : false;
            VolTrackBar.Enabled = NoAtW ? true : false;
            bufsize.Enabled = NoASIOE ? true : false;
            OldBuff.Enabled = NoAtW ? true : false;

            if (S) Program.SynthSettings.SetValue("xaudiodisabled", AudioEngBox.SelectedIndex, RegistryValueKind.DWord);
        }

        private void LoadSettings()
        {
            try
            {
                Frequency.Text = Program.SynthSettings.GetValue("AudioFrequency", 44100).ToString();
                PolyphonyLimit.Value = Convert.ToInt32(Program.SynthSettings.GetValue("MaxVoices", 512));
                MaxCPU.Value = Convert.ToInt32(Program.SynthSettings.GetValue("MaxRenderingTime", 75));
                FastHotKeys.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FastHotkeys", 0));
                DebugMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DebugMode", 0));

                switch (Convert.ToInt32(Program.SynthSettings.GetValue("DriverPriority", 0)))
                {
                    case 0:
                    default:
                        DePrio.Checked = true;
                        DePrio_CheckedChanged(null, null);
                        break;
                    case 1:
                        RTPrio.Checked = true;
                        break;
                    case 2:
                        HiPrio.Checked = true;
                        break;
                    case 3:
                        HNPrio.Checked = true;
                        break;
                    case 4:
                        NoPrio.Checked = true;
                        break;
                    case 5:
                        LNPrio.Checked = true;
                        break;
                    case 6:
                        LoPrio.Checked = true;
                        break;
                }

                switch (Convert.ToInt32(Program.SynthSettings.GetValue("AudioBitDepth", 1)))
                {
                    default:
                    case 1:
                        AudioBitDepth.SelectedIndex = 0;
                        break;
                    case 0:
                    case 2:
                        AudioBitDepth.SelectedIndex = 1;
                        break;
                    case 3:
                        AudioBitDepth.SelectedIndex = 2;
                        break;

                }

                DisableChime.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DisableChime", 0));
                LiveChangesTrigger.Checked = Properties.Settings.Default.LiveChanges;

                VolumeBoost.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("VolumeBoost", 0));
                VolTrackBar.Maximum = VolumeBoost.Checked ? 50000 : 10000;
                VolTrackBar.Value = Convert.ToInt32(Program.SynthSettings.GetValue("OutputVolume", 10000));

                AutoLoad.Checked = Properties.Settings.Default.AutoLoadList;

                Preload.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("PreloadSoundfonts", 1));
                EnableSFX.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("EnableSFX", 1));
                NoteOffCheck.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("NoteOff1", 0));
                SysResetIgnore.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("IgnoreSysReset", 0));
                bufsize.Value = Convert.ToInt32(Program.SynthSettings.GetValue("BufferLength", 30));

                SincInter.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("SincInter", 0));
                CheckSincEnabled();
                try { SincConv.SelectedIndex = Convert.ToInt32(Program.SynthSettings.GetValue("SincConv", 0)); }
                catch { SincConv.SelectedIndex = 2; }

                FadeoutDisable.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DisableNotesFadeOut", 0));
                MonophonicFunc.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("MonoRendering", 0));
                SlowDownPlayback.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DontMissNotes", 0));
                KSDAPIBox.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("KDMAPIEnabled", 1));
                HMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("HyperPlayback", 0));
                OldBuff.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("NotesCatcherWithAudio", 0));

                ReverbV.Value = Functions.Between0And127(Convert.ToInt32(Program.SynthSettings.GetValue("Reverb", 64)));
                ChorusV.Value = Functions.Between0And127(Convert.ToInt32(Program.SynthSettings.GetValue("Chorus", 64)));
                EnableRCOverride.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("RCOverride", 0));
                EnableRCOverride_CheckedChanged(null, null);

                DisableCookedPlayer.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DisableCookedPlayer", 0));
                AllNotesIgnore.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("IgnoreAllNotes", 0));
                IgnoreNotes.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("IgnoreNotesBetweenVel", 0));
                IgnoreNotesLV.Value = Convert.ToInt32(Program.SynthSettings.GetValue("MinVelIgnore", 0));
                IgnoreNotesHV.Value = Convert.ToInt32(Program.SynthSettings.GetValue("MaxVelIgnore", 1));
                CapFram.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("CapFramerate", 1));
                Limit88.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("LimitTo88Keys", 0));
                FullVelocityMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FullVelocityMode", 0));
                OverrideNoteLength.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("OverrideNoteLength", 0));
                NoteLengthValue.Value = Convert.ToDecimal((double)Convert.ToInt32(Program.SynthSettings.GetValue("NoteLengthValue", 5)) / 1000.0);
                DelayNoteOff.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DelayNoteOff", 0));
                NoteOffDelayValue.Value = Convert.ToDecimal((double)Convert.ToInt32(Program.SynthSettings.GetValue("DelayNoteOffValue", 5)) / 1000.0);
                HMode_CheckedChanged(null, null);

                switch (Convert.ToInt32(Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE)))
                {
                    case AudioEngine.WASAPI_ENGINE:
                    default:
                        AudioEngBox.SelectedIndex = AudioEngine.WASAPI_ENGINE;
                        break;
                    case AudioEngine.AUDTOWAV:
                        AudioEngBox.SelectedIndex = AudioEngine.AUDTOWAV;
                        break;
                    case AudioEngine.DSOUND_ENGINE:
                        AudioEngBox.SelectedIndex = AudioEngine.DSOUND_ENGINE;
                        break;
                    case AudioEngine.ASIO_ENGINE:
                        AudioEngBox.SelectedIndex = AudioEngine.ASIO_ENGINE;
                        break;
                }

                Functions.LiveChanges.PreviousEngine = (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                Functions.LiveChanges.PreviousFrequency = (int)Program.SynthSettings.GetValue("AudioFrequency", 44100);
                Functions.LiveChanges.PreviousBuffer = (int)Program.SynthSettings.GetValue("BufferLength", 50);
                Functions.LiveChanges.MonophonicRender = (int)Program.SynthSettings.GetValue("MonoRendering", 0);
                Functions.LiveChanges.AudioBitDepth = (int)Program.SynthSettings.GetValue("AudioBitDepth", 1);
            }
            catch (Exception ex)
            {
                Program.ShowError(4, "FATAL ERROR", "The configurator is unable to load its settings.\n\nPress OK to quit.", ex);
                Application.ExitThread();
            }
        }

        private void SaveSettings(bool OV)
        {
            // Normal settings
            Program.SynthSettings.SetValue("AudioFrequency", Frequency.Text, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MaxVoices", PolyphonyLimit.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MaxRenderingTime", MaxCPU.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("FastHotkeys", Convert.ToInt32(FastHotKeys.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("DebugMode", Convert.ToInt32(DebugMode.Checked), RegistryValueKind.DWord);

            if (DePrio.Checked)
            {
                Program.SynthSettings.SetValue("DriverPriority", 0, RegistryValueKind.DWord);
            }
            else
            {
                if (RTPrio.Checked) Program.SynthSettings.SetValue("DriverPriority", 0, RegistryValueKind.DWord);
                else if (RTPrio.Checked) Program.SynthSettings.SetValue("DriverPriority", 1, RegistryValueKind.DWord);
                else if (HiPrio.Checked) Program.SynthSettings.SetValue("DriverPriority", 2, RegistryValueKind.DWord);
                else if (HNPrio.Checked) Program.SynthSettings.SetValue("DriverPriority", 3, RegistryValueKind.DWord);
                else if (NoPrio.Checked) Program.SynthSettings.SetValue("DriverPriority", 4, RegistryValueKind.DWord);
                else if (LNPrio.Checked) Program.SynthSettings.SetValue("DriverPriority", 5, RegistryValueKind.DWord);
                else if (LoPrio.Checked) Program.SynthSettings.SetValue("DriverPriority", 6, RegistryValueKind.DWord);
                else Program.SynthSettings.SetValue("DriverPriority", 0, RegistryValueKind.DWord);
            }

            switch (AudioBitDepth.SelectedIndex)
            {
                default:
                case 0:
                    Program.SynthSettings.GetValue("AudioBitDepth", 1);
                    break;
                case 1:
                    Program.SynthSettings.GetValue("AudioBitDepth", 2);
                    break;
                case 2:
                    Program.SynthSettings.GetValue("AudioBitDepth", 3);
                    break;
            }

            Program.SynthSettings.SetValue("DisableChime", Convert.ToInt32(DisableChime.Checked), RegistryValueKind.DWord);
            Properties.Settings.Default.LiveChanges = LiveChangesTrigger.Checked;

            Program.SynthSettings.SetValue("VolumeBoost", Convert.ToInt32(VolumeBoost.Checked), RegistryValueKind.DWord);
            Properties.Settings.Default.AutoLoadList = AutoLoad.Checked;

            Program.SynthSettings.SetValue("PreloadSoundfonts", Convert.ToInt32(Preload.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("EnableSFX", Convert.ToInt32(EnableSFX.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("NoteOff1", Convert.ToInt32(NoteOffCheck.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("IgnoreSysReset", Convert.ToInt32(SysResetIgnore.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("BufferLength", bufsize.Value, RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("SincInter", Convert.ToInt32(SincInter.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("SincConv", SincConv.SelectedIndex, RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("DisableNotesFadeOut", Convert.ToInt32(FadeoutDisable.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MonoRendering", Convert.ToInt32(MonophonicFunc.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("DontMissNotes", Convert.ToInt32(SlowDownPlayback.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("KDMAPIEnabled", Convert.ToInt32(KSDAPIBox.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("HyperPlayback", Convert.ToInt32(HMode.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("NotesCatcherWithAudio", Convert.ToInt32(OldBuff.Checked), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("Reverb", ReverbV.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("Chorus", ChorusV.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("RCOverride", Convert.ToInt32(EnableRCOverride.Checked), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("DisableCookedPlayer", Convert.ToInt32(DisableCookedPlayer.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("IgnoreAllNotes", Convert.ToInt32(AllNotesIgnore.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("IgnoreNotesBetweenVel", Convert.ToInt32(IgnoreNotes.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MinVelIgnore", IgnoreNotesLV.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MaxVelIgnore", IgnoreNotesHV.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("CapFramerate", Convert.ToInt32(CapFram.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("LimitTo88Keys", Convert.ToInt32(Limit88.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("FullVelocityMode", Convert.ToInt32(FullVelocityMode.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("OverrideNoteLength", Convert.ToInt32(OverrideNoteLength.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("NoteLengthValue", Convert.ToInt32(NoteLengthValue.Value * 1000), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("DelayNoteOff", Convert.ToInt32(DelayNoteOff.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("DelayNoteOffValue", Convert.ToInt32(NoteOffDelayValue.Value * 1000), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("CurrentEngine", AudioEngBox.SelectedIndex, RegistryValueKind.DWord);

            if (OV)
            {
                Program.SynthSettings.SetValue("LiveChanges", "1", RegistryValueKind.DWord);
                Functions.LiveChanges.PreviousEngine = (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                Functions.LiveChanges.PreviousFrequency = (int)Program.SynthSettings.GetValue("AudioFrequency", 44100);
                Functions.LiveChanges.PreviousBuffer = (int)Program.SynthSettings.GetValue("BufferLength", 50);
                Functions.LiveChanges.MonophonicRender = (int)Program.SynthSettings.GetValue("MonoRendering", 0);
                Functions.LiveChanges.AudioBitDepth = (int)Program.SynthSettings.GetValue("AudioBitDepth", 1);
            }
            else
            {
                if (Properties.Settings.Default.LiveChanges)
                {
                    if (Functions.LiveChanges.PreviousEngine != (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE) ||
                        Functions.LiveChanges.PreviousFrequency != (int)Program.SynthSettings.GetValue("AudioFrequency", 44100) ||
                        Functions.LiveChanges.PreviousBuffer != (int)Program.SynthSettings.GetValue("BufferLength", 50) ||
                        Functions.LiveChanges.MonophonicRender != (int)Program.SynthSettings.GetValue("MonoRendering", 0) ||
                        Functions.LiveChanges.AudioBitDepth != (int)Program.SynthSettings.GetValue("AudioBitDepth", 1))
                    {
                        Program.SynthSettings.SetValue("LiveChanges", 1, RegistryValueKind.DWord);
                        Functions.LiveChanges.PreviousEngine = (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                        Functions.LiveChanges.PreviousFrequency = (int)Program.SynthSettings.GetValue("AudioFrequency", 44100);
                        Functions.LiveChanges.PreviousBuffer = (int)Program.SynthSettings.GetValue("BufferLength", 50);
                        Functions.LiveChanges.MonophonicRender = (int)Program.SynthSettings.GetValue("MonoRendering", 0);
                        Functions.LiveChanges.AudioBitDepth = (int)Program.SynthSettings.GetValue("AudioBitDepth", 1);
                    }
                }
            }

            Properties.Settings.Default.Save();
        }

        private void SettingsPanel_Load(object sender, EventArgs e)
        {
            // Nothing lul
        }

        private void VolumeBoost_Click(object sender, EventArgs e)
        {
            if (VolumeBoost.Checked)
            {
                if (VolTrackBar.Value > 10000)
                {
                    VolTrackBar.Value = 10000;
                    Program.SynthSettings.SetValue("OutputVolume", 10000, RegistryValueKind.DWord);
                }
                Program.SynthSettings.SetValue("VolumeBoost", 0, RegistryValueKind.DWord);
                VolTrackBar.Maximum = 10000;
                VolumeBoost.Checked = false;
                VolTrackBar.Refresh();
            }
            else
            {
                Program.SynthSettings.SetValue("VolumeBoost", 1, RegistryValueKind.DWord);
                VolTrackBar.Maximum = 50000;
                VolumeBoost.Checked = true;
                VolTrackBar.Refresh();
            }
        }

        private void ChangeA2WOutDir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new OutputWAVDir().ShowDialog();
        }

        private void SpatialSound_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Functions.OpenAdvancedAudioSettings("spatial", "This function requires Windows 10 Creators Update or newer.");
        }

        private void ChangeEVBuf_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new EVBufferManager().ShowDialog();
        }

        private void ChangeSynthMask_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new MaskSynthAsAnother().ShowDialog();
        }

        private void VolTrackBar_Scroll(object sender)
        {
            if (VolTrackBar.Value <= 49) VolSimView.ForeColor = Color.Red;
            else VolSimView.ForeColor = Color.FromArgb(255, 53, 0, 119);

            decimal VolVal = (decimal)VolTrackBar.Value / 100;
            VolSimView.Text = String.Format("{0}", Math.Round(VolVal, MidpointRounding.AwayFromZero).ToString());

            Program.SynthSettings.SetValue("OutputVolume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
        }

        private void KSDAPIBoxWhat_Click(object sender, EventArgs e)
        {
            Program.ShowError(
                0,
                "Info",
                "If you uncheck this option, some apps might be forced to fallback to the stock Windows Multimedia API, which increases latency." +
                "\nKeep in mind that not all KDMAPI-ready apps do check for this value, and they might use it whether you want them to or not." +
                "\n\n(This value will not affect the Windows Multimedia Wrapper.)",
                null);
        }

        private void HModeWhat_Click(object sender, EventArgs e)
        {
            Program.ShowError(
                0,
                "Info",
                "Clicking this checkbox will remove all the checks done to the events, for example transposing and other settings in the configurator.\n" +
                "The events will be sent straight to the buffer, and played immediately.\n\n" +
                "The \"Slow down playback instead of skipping notes\" checkbox will not work, while this mode is enabled, along with \"Running Status\" support and other event processing-related functions.\n\n" +
                "WARNING: Playing too much with the live changes while this setting is enabled might crash the threads, rendering the synth unusable until a full restart of the application!",
                null);
        }

        private void OverrideNoteLengthWA1_Click(object sender, EventArgs e)
        {
            Program.ShowError(
                0,
                "Info",
                "This option doesn't guarantee that all the notes will be turned off immediately after the specified amount of time on the left numericbox." +
                "\nPedal hold and other special events might delay the noteoff event even more.",
                null);
        }

        private void AudioEngBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioEngBoxTrigger(false);
        }

        private void ChangeDefaultOutput_Click(object sender, EventArgs e)
        {
            switch (AudioEngBox.SelectedIndex)
            {
                case AudioEngine.DSOUND_ENGINE:
                case AudioEngine.WASAPI_ENGINE:
                    new DefaultOutput((AudioEngBox.SelectedIndex == AudioEngine.WASAPI_ENGINE)).ShowDialog(this);
                    break;
                case AudioEngine.ASIO_ENGINE:
                    new DefaultASIOAudioOutput().ShowDialog();
                    break;
                default:
                    Program.ShowError(4, "Error", "You're not supposed to see this.", null);
                    break;
            }
        }

        private void DebugModeFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void EnableRCOverride_CheckedChanged(object sender, EventArgs e)
        {
            ReverbL.Enabled = EnableRCOverride.Checked;
            ReverbV.Enabled = EnableRCOverride.Checked;
            ChorusL.Enabled = EnableRCOverride.Checked;
            ChorusV.Enabled = EnableRCOverride.Checked;
        }

        private void DePrio_CheckedChanged(object sender, EventArgs e)
        {
            PrioPal.Enabled = !DePrio.Checked;
        }

        private void IgnoreNotes_CheckedChanged(object sender, EventArgs e)
        {
            IgnoreNotesLL.Enabled = IgnoreNotes.Checked;
            IgnoreNotesLV.Enabled = IgnoreNotes.Checked;
            IgnoreNotesHL.Enabled = IgnoreNotes.Checked;
            IgnoreNotesHV.Enabled = IgnoreNotes.Checked;
        }

        private void OverrideNoteLength_CheckedChanged(object sender, EventArgs e)
        {
            NoteLengthValue.Enabled = OverrideNoteLength.Checked;
        }

        private void DelayNoteOff_CheckedChanged(object sender, EventArgs e)
        {
            NoteOffDelayValue.Enabled = DelayNoteOff.Checked;
        }

        private void DebugMode_CheckedChanged(object sender, EventArgs e)
        {
            DebugModeFolder.Visible = DebugMode.Checked;
        }

        private void HMode_CheckedChanged(object sender, EventArgs e)
        {
            OverrideNoteLength.Enabled = !HMode.Checked;
            NoteLengthValue.Enabled = (HMode.Checked) ? false : OverrideNoteLength.Checked;

            DelayNoteOff.Enabled = !HMode.Checked;
            NoteOffDelayValue.Enabled = (HMode.Checked) ? false : DelayNoteOff.Checked;

            IgnoreNotes.Enabled = !HMode.Checked;
            IgnoreNotesLL.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;
            IgnoreNotesLV.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;
            IgnoreNotesHL.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;
            IgnoreNotesHV.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;
   
            SysResetIgnore.Enabled = !HMode.Checked;
            FullVelocityMode.Enabled = !HMode.Checked;
            Limit88.Enabled = !HMode.Checked;
            AllNotesIgnore.Enabled = !HMode.Checked;
        }

        public void ButtonToSaveSettings(object sender, EventArgs e)
        {
            SaveSettings((ModifierKeys & Keys.Control) == Keys.Control);
        }

        public void ButtonToResetSettings(object sender, EventArgs e)
        {
            DialogResult RES = Program.ShowError(3, "Reset settings", "Are you sure you want to reset your settings?\n\nAll your custom values will be lost.", null);

            if (RES == DialogResult.Yes)
            {
                Functions.ResetDriverSettings();
                LoadSettings();
            }                   
        }
    }
}
