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
using System.Security.Principal;

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

            VolKnob.ContextMenu = VolTrackBarMenu;

            if (!(Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build >= 15063))
            {
                SpatialSound.Enabled = false;
                SpatialSound.Text += " (Not available on your version of Windows)";
            }

            this.MouseWheel += new MouseEventHandler(SettingsPanel_MouseWheel);

            LoadSettings();
        }

        private const int WM_VSCROLL = 0x0115;
        private const int WM_HSCROLL = 0x0114;

        private void SettingsPanel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Double buffering is useless, refresh it by yourself lol

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= Properties.Settings.Default.DrawControlsFaster ? 0 : 0x02000000;
                cp.Style &= ~0x02000000;
                return cp;
            }
        }

        private void CheckSincEnabled()
        {
            SincConvLab.Enabled = SincInter.Checked;
            SincConv.Enabled = SincInter.Checked;
        }

        private int PreviousValue = 2;
        private void AudioEngBoxTrigger(bool S)
        {
            int AE = AudioEngBox.SelectedIndex;
            bool NOAtWASIOXAE = (AE != AudioEngine.AUDTOWAV && AE != AudioEngine.ASIO_ENGINE && AE != AudioEngine.XA_ENGINE);
            bool NoASIOXAE = (AE != AudioEngine.ASIO_ENGINE && AE != AudioEngine.XA_ENGINE);
            bool NoASIOWAE = (AE != AudioEngine.ASIO_ENGINE && AE != AudioEngine.WASAPI_ENGINE);
            bool NoASIOWAXAE = (AE != AudioEngine.ASIO_ENGINE && AE != AudioEngine.WASAPI_ENGINE && AE != AudioEngine.XA_ENGINE);
            bool NoAtW = (AE != AudioEngine.AUDTOWAV);
            bool NoASIOE = (AE != AudioEngine.ASIO_ENGINE);
            bool NoXAE = (AE != AudioEngine.XA_ENGINE);

            switch (AudioEngBox.SelectedIndex)
            {
                case AudioEngine.XA_ENGINE:
                    ChangeDefaultOutput.Text = "Advanced...";
                    BufferText.Text = "The output buffer can be controlled through the engine's advanced settings";
                    OldBuff.Enabled = true;
                    break;
                case AudioEngine.AUDTOWAV:
                    ChangeDefaultOutput.Text = "Directory...";
                    BufferText.Text = "The output buffer isn't needed when outputting to a .WAV file";
                    OldBuff.Enabled = true;
                    break;
                case AudioEngine.ASIO_ENGINE:
                    ChangeDefaultOutput.Text = "Devices...";
                    if (DefaultASIOAudioOutput.GetASIODevicesCount() < 1 && !S)
                    {
                        DialogResult RES = 
                            Program.ShowError(
                                3,
                                "Error",
                                "You selected ASIO, but no ASIO devices are installed on your computer.\n" +
                                "Running any MIDI app with this configuration might lead to an error on startup.\n\n" +
                                "Are you sure you want to continue?\nPress Yes to keep this configuration, or No to switch back to the previous engine.",
                                null);

                        if (RES == DialogResult.No)
                        {
                            AudioEngBox.SelectedIndex = PreviousValue;
                            AudioEngBoxTrigger(true);
                        }

                        return;
                    }

                    BufferText.Text = "The output buffer is controlled by the ASIO device itself";
                    OldBuff.Enabled = !Convert.ToBoolean(Convert.ToInt32(Program.SynthSettings.GetValue("ASIODirectFeed", "0")));
                    break;
                case AudioEngine.WASAPI_ENGINE:
                case AudioEngine.BASS_OUTPUT:
                default:
                    ChangeDefaultOutput.Text = "Output...";
                    BufferText.Text = "Output buffer (in ms, from 1 to 1000. If the buffer is too small, it'll be set automatically to the lowest value possible)";
                    OldBuff.Enabled = true;
                    break;
            }

            AudioBitDepthLabel.Enabled = NoASIOWAXAE ? true : false;
            AudioBitDepth.Enabled = NoASIOWAE ? true : false;
            BufferText.Enabled = NOAtWASIOXAE ? true : false;
            DrvHzLabel.Enabled = true;
            Frequency.Enabled = true;
            MaxCPU.Enabled = NoAtW ? true : false;
            RenderingTimeLabel.Enabled = NoAtW ? true : false;
            VolLabel.Enabled = NoAtW ? true : false;
            VolSimView.Enabled = NoAtW ? true : false;
            VolKnob.Enabled = NoAtW ? true : false;
            bufsize.Enabled = NoASIOXAE ? true : false;

            PreviousValue = AudioEngBox.SelectedIndex;
            if (S) Program.SynthSettings.SetValue("CurrentEngine", AudioEngBox.SelectedIndex, RegistryValueKind.DWord);
        }

        private void LoadSettings()
        {
            try
            {
                AudioBitDepth.SelectedIndex = Convert.ToInt32(Program.SynthSettings.GetValue("AudioBitDepth", 0));
                Frequency.Text = Program.SynthSettings.GetValue("AudioFrequency", 44100).ToString();
                PolyphonyLimit.Value = Convert.ToInt32(Program.SynthSettings.GetValue("MaxVoices", 512));
                MaxCPU.Value = Convert.ToInt32(Program.SynthSettings.GetValue("MaxRenderingTime", 75));
                FastHotKeys.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FastHotkeys", 0));
                DebugMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DebugMode", 0));

                try { PrioBox.SelectedIndex = Convert.ToInt32(Program.SynthSettings.GetValue("DriverPriority", 0)); }
                catch { PrioBox.SelectedIndex = 0; }

                LiveChangesTrigger.Checked = Properties.Settings.Default.LiveChanges;

                VolumeBoost.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("VolumeBoost", 0));
                VolKnob.Maximum = VolumeBoost.Checked ? 50000 : 10000;
                VolKnob.Value = Convert.ToInt32(Program.SynthSettings.GetValue("OutputVolume", 10000));

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
                AsyncProcessing.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("BASSDSMode", 1));
                KSDAPIBox.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("KDMAPIEnabled", 1));
                HMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("HyperPlayback", 0));
                OldBuff.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("NotesCatcherWithAudio", 0));

                IgnoreAllEvents.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("IgnoreAllEvents", 0));
                IgnoreNotes.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("IgnoreNotesBetweenVel", 0));
                AudioRampIn.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("AudioRampIn", 1));
                LinAttMod.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("LinAttMod", 0));
                LinDecVol.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("LinDecVol", 1));
                NoSFGenLimits.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("NoSFGenLimits", 0));

                Int32 LV = Convert.ToInt32(Program.SynthSettings.GetValue("MinVelIgnore", 1));
                Int32 HV = Convert.ToInt32(Program.SynthSettings.GetValue("MaxVelIgnore", 2));
                if (LV > HV) LV = HV;
                if (LV < IgnoreNotesLV.Minimum | LV > IgnoreNotesLV.Maximum) LV = (int)IgnoreNotesLV.Minimum;
                if (HV < IgnoreNotesHV.Minimum | HV > IgnoreNotesHV.Maximum) HV = (int)IgnoreNotesHV.Minimum;
                IgnoreNotesLV.Value = LV;
                IgnoreNotesHV.Value = HV;

                Limit88.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("LimitTo88Keys", 0));
                FullVelocityMode.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FullVelocityMode", 0));
                OverrideNoteLength.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("OverrideNoteLength", 0));
                NoteLengthValue.Value = Convert.ToDecimal((double)Convert.ToInt32(Program.SynthSettings.GetValue("NoteLengthValue", 5)) / 1000.0);
                DelayNoteOff.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("DelayNoteOff", 0));
                NoteOffDelayValue.Value = Convert.ToDecimal((double)Convert.ToInt32(Program.SynthSettings.GetValue("DelayNoteOffValue", 5)) / 1000.0);
                HMode_CheckedChanged(null, null);

                AudioEngBox.SelectedIndexChanged -= AudioEngBox_SelectedIndexChanged;
                switch (Convert.ToInt32(Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE)))
                {
                    case AudioEngine.AUDTOWAV:
                        AudioEngBox.SelectedIndex = AudioEngine.AUDTOWAV;
                        break;
                    case AudioEngine.BASS_OUTPUT:
                        AudioEngBox.SelectedIndex = AudioEngine.BASS_OUTPUT;
                        break;
                    case AudioEngine.ASIO_ENGINE:
                        AudioEngBox.SelectedIndex = AudioEngine.ASIO_ENGINE;
                        break;
                    case AudioEngine.XA_ENGINE:
                        AudioEngBox.SelectedIndex = AudioEngine.XA_ENGINE;
                        break;
                    case AudioEngine.WASAPI_ENGINE:
                    default:
                        AudioEngBox.SelectedIndex = AudioEngine.WASAPI_ENGINE;
                        break;
                }
                PreviousValue = AudioEngBox.SelectedIndex;
                AudioEngBoxTrigger(true);
                AudioEngBox.SelectedIndexChanged += AudioEngBox_SelectedIndexChanged;

                UseTGT.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("StockWinMM", 0));
                IgnoreCloseCalls.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("KeepAlive", 0));
                ShowChangelogUpdate.Checked = Properties.Settings.Default.ShowChangelogStartUp;

                Functions.LiveChanges.PreviousEngine = (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                Functions.LiveChanges.PreviousFrequency = (int)Program.SynthSettings.GetValue("AudioFrequency", 44100);
                Functions.LiveChanges.PreviousBuffer = (int)Program.SynthSettings.GetValue("BufferLength", 50);
                Functions.LiveChanges.MonophonicRender = (int)Program.SynthSettings.GetValue("MonoRendering", 0);
                Functions.LiveChanges.AudioBitDepth = (int)Program.SynthSettings.GetValue("AudioBitDepth", 0);
                Functions.LiveChanges.NotesCatcherWithAudio = (int)Program.SynthSettings.GetValue("NotesCatcherWithAudio", 0);
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
            Program.SynthSettings.SetValue("AudioBitDepth", AudioBitDepth.SelectedIndex, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("AudioFrequency", Frequency.Text, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MaxVoices", PolyphonyLimit.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MaxRenderingTime", MaxCPU.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("FastHotkeys", Convert.ToInt32(FastHotKeys.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("DebugMode", Convert.ToInt32(DebugMode.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("NotesCatcherWithAudio", Convert.ToInt32(OldBuff.Checked), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("DriverPriority", PrioBox.SelectedIndex, RegistryValueKind.DWord);
            Program.SynthSettings.GetValue("AudioBitDepth", AudioBitDepth.SelectedIndex);

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
            Program.SynthSettings.SetValue("BASSDSMode", Convert.ToInt32(AsyncProcessing.Checked), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("HyperPlayback", Convert.ToInt32(HMode.Checked), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("IgnoreAllEvents", Convert.ToInt32(IgnoreAllEvents.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("IgnoreNotesBetweenVel", Convert.ToInt32(IgnoreNotes.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("AudioRampIn", Convert.ToInt32(AudioRampIn.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("LinAttMod", Convert.ToInt32(LinAttMod.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("LinDecVol", Convert.ToInt32(LinDecVol.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("NoSFGenLimits", Convert.ToInt32(NoSFGenLimits.Checked), RegistryValueKind.DWord);
            
            if (IgnoreNotesLV.Value > IgnoreNotesHV.Value) IgnoreNotesLV.Value = IgnoreNotesHV.Value;
            Program.SynthSettings.SetValue("MinVelIgnore", IgnoreNotesLV.Value, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("MaxVelIgnore", IgnoreNotesHV.Value, RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("LimitTo88Keys", Convert.ToInt32(Limit88.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("FullVelocityMode", Convert.ToInt32(FullVelocityMode.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("OverrideNoteLength", Convert.ToInt32(OverrideNoteLength.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("NoteLengthValue", Convert.ToInt32(NoteLengthValue.Value * 1000), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("DelayNoteOff", Convert.ToInt32(DelayNoteOff.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("DelayNoteOffValue", Convert.ToInt32(NoteOffDelayValue.Value * 1000), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("CurrentEngine", AudioEngBox.SelectedIndex, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("StockWinMM", Convert.ToInt32(UseTGT.Checked), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("KeepAlive", Convert.ToInt32(IgnoreCloseCalls.Checked), RegistryValueKind.DWord);

            Properties.Settings.Default.ShowChangelogStartUp = ShowChangelogUpdate.Checked;

            if (OV)
            {
                Functions.SignalLiveChanges();
                Functions.LiveChanges.PreviousEngine = (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                Functions.LiveChanges.PreviousFrequency = (int)Program.SynthSettings.GetValue("AudioFrequency", 44100);
                Functions.LiveChanges.PreviousBuffer = (int)Program.SynthSettings.GetValue("BufferLength", 50);
                Functions.LiveChanges.MonophonicRender = (int)Program.SynthSettings.GetValue("MonoRendering", 0);
                Functions.LiveChanges.AudioBitDepth = (int)Program.SynthSettings.GetValue("AudioBitDepth", 0);
                Functions.LiveChanges.NotesCatcherWithAudio = (int)Program.SynthSettings.GetValue("NotesCatcherWithAudio", 0);
            }
            else
            {
                if (Properties.Settings.Default.LiveChanges)
                {
                    if (Functions.LiveChanges.PreviousEngine != (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE) ||
                        Functions.LiveChanges.PreviousFrequency != (int)Program.SynthSettings.GetValue("AudioFrequency", 44100) ||
                        Functions.LiveChanges.PreviousBuffer != (int)Program.SynthSettings.GetValue("BufferLength", 50) ||
                        Functions.LiveChanges.MonophonicRender != (int)Program.SynthSettings.GetValue("MonoRendering", 0) ||
                        Functions.LiveChanges.AudioBitDepth != (int)Program.SynthSettings.GetValue("AudioBitDepth", 0) ||
                        Functions.LiveChanges.NotesCatcherWithAudio != (int)Program.SynthSettings.GetValue("NotesCatcherWithAudio", 0))
                    {
                        Functions.SignalLiveChanges();
                        Functions.LiveChanges.PreviousEngine = (int)Program.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                        Functions.LiveChanges.PreviousFrequency = (int)Program.SynthSettings.GetValue("AudioFrequency", 44100);
                        Functions.LiveChanges.PreviousBuffer = (int)Program.SynthSettings.GetValue("BufferLength", 50);
                        Functions.LiveChanges.MonophonicRender = (int)Program.SynthSettings.GetValue("MonoRendering", 0);
                        Functions.LiveChanges.AudioBitDepth = (int)Program.SynthSettings.GetValue("AudioBitDepth", 0);
                        Functions.LiveChanges.NotesCatcherWithAudio = (int)Program.SynthSettings.GetValue("NotesCatcherWithAudio", 0);
                    }
                }
            }

            System.Media.SystemSounds.Question.Play();
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
                if (VolKnob.Value > 10000)
                {
                    VolKnob.Value = 10000;
                    Program.SynthSettings.SetValue("OutputVolume", 10000, RegistryValueKind.DWord);
                }
                Program.SynthSettings.SetValue("VolumeBoost", 0, RegistryValueKind.DWord);
                VolKnob.Maximum = 10000;
                VolumeBoost.Checked = false;
                VolKnob.Refresh();
            }
            else
            {
                Program.SynthSettings.SetValue("VolumeBoost", 1, RegistryValueKind.DWord);
                VolKnob.Maximum = 50000;
                VolumeBoost.Checked = true;
                VolKnob.Refresh();
            }
        }

        private void SpatialSound_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Functions.OpenAdvancedAudioSettings("spatial", "This function requires Windows 10 Creators Update or newer.");
        }

        private void DSPSettingsBox_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new DSPSettings().ShowDialog();
        }

        private void ChangeEVBuf_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new EVBufferManager().ShowDialog();
        }

        private void PitchShifting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new PitchAndTranspose().ShowDialog();
        }

        private void WinMMSpeedDiag_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new WinMMSpeed().ShowDialog();
        }

        private void MIDIFeedbackTool_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new MIDIFeedback().ShowDialog();
        }

        private void VolTrackBar_Scroll(object sender)
        {
            if (VolKnob.Value <= 49) VolSimView.ForeColor = Color.Red;
            else VolSimView.ForeColor = Color.FromArgb(255, 53, 0, 119);

            decimal VolVal = (decimal)VolKnob.Value / 100;
            VolSimView.Text = String.Format("{0}", VolVal.ToString("000.00"));

            Program.SynthSettings.SetValue("OutputVolume", VolKnob.Value.ToString(), RegistryValueKind.DWord);
        }

        private void KSDAPIBoxWhat_Click(object sender, EventArgs e)
        {
            Program.ShowError(
                0,
                "Info",
                "If you uncheck this option, some apps might be forced to fallback to the stock Windows Multimedia API, which increases latency." +
                "\nApps that only make use of the Keppy's Direct MIDI API, with no Windows Multimedia API fallback, will probably ignore this setting." +
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
            try
            {
                switch (AudioEngBox.SelectedIndex)
                {
                    case AudioEngine.AUDTOWAV:
                        new OutputWAVDir().ShowDialog(this);
                        break;

                    case AudioEngine.BASS_OUTPUT:
                        new DefaultAudioOutput().ShowDialog(this);
                        break;

                    case AudioEngine.WASAPI_ENGINE:
                        new DefaultWASAPIAudioOutput().ShowDialog();
                        break;

                    case AudioEngine.ASIO_ENGINE:
                        new DefaultASIOAudioOutput(Control.ModifierKeys == Keys.Shift).ShowDialog();
                        OldBuff.Enabled = !Convert.ToBoolean(Convert.ToInt32(Program.SynthSettings.GetValue("ASIODirectFeed", "0")));
                        break;

                    case AudioEngine.XA_ENGINE:
                        new XAOutputSettings().ShowDialog();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MinidumpsFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String DirectoryDebug = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\dumpfiles\\";
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

        private void FineTuneKnobIt_Click(object sender, EventArgs e)
        {
            PreciseControlVol PCV = new PreciseControlVol(VolKnob.Value, VolKnob.Maximum);

            if (PCV.ShowDialog() == DialogResult.OK)
                VolKnob.Value = PCV.NewVolume;

            PCV.Dispose();
        }

        private void LiveChangesTrigger_CheckedChanged(object sender, EventArgs e)
        {
            String Desc1 = LiveChangesTrigger.Checked ? "Requires a restart of the audio stream to work." : null;
            String Desc2 = LiveChangesTrigger.Checked ? "Requires a restart of the application to work." : null;

            // Stream restart
            Requirements.SetToolTip(AudioEngBox, Desc1);
            Requirements.SetToolTip(AudioBitDepth, Desc1);
            Requirements.SetToolTip(Frequency, Desc1);
            Requirements.SetToolTip(bufsize, Desc1);
            Requirements.SetToolTip(MonophonicFunc, Desc1);
            Requirements.SetToolTip(PrioBox, Desc1);

            // App restart
            Requirements.SetToolTip(KSDAPIBox, Desc2);
            Requirements.SetToolTip(DebugMode, Desc2);
            Requirements.SetToolTip(UseTGT, Desc2);
        }

        private void SincInter_CheckedChanged(object sender, EventArgs e)
        {
            SincConvLab.Enabled = SincInter.Checked;
            SincConv.Enabled = SincInter.Checked;
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
            MinidumpsFolder.Visible = DebugMode.Checked;
        }

        private void HMode_CheckedChanged(object sender, EventArgs e)
        {
            OverrideNoteLength.Enabled = !HMode.Checked;
            NoteLengthValue.Enabled = (HMode.Checked) ? false : OverrideNoteLength.Checked;

            DelayNoteOff.Enabled = !HMode.Checked;
            NoteOffDelayValue.Enabled = (HMode.Checked) ? false : DelayNoteOff.Checked;

            PitchShifting.Enabled = !HMode.Checked;

            IgnoreNotes.Enabled = !HMode.Checked;
            IgnoreNotesLL.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;
            IgnoreNotesLV.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;
            IgnoreNotesHL.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;
            IgnoreNotesHV.Enabled = (HMode.Checked) ? false : IgnoreNotes.Checked;

            SlowDownPlayback.Enabled = !HMode.Checked;
            SysResetIgnore.Enabled = !HMode.Checked;
            FullVelocityMode.Enabled = !HMode.Checked;
            Limit88.Enabled = !HMode.Checked;
            IgnoreAllEvents.Enabled = !HMode.Checked;
            MIDIFeedbackTool.Enabled = !HMode.Checked;
        }

        private void Troubleshooter_Click(object sender, EventArgs e)
        {
            bool IsProcessElevated = false;

            using (WindowsIdentity Identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal Principal = new WindowsPrincipal(Identity);
                IsProcessElevated = Principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (!IsProcessElevated)
            {
                if (!Functions.RestartAsAdmin())
                    return;
            }

            MessageBox.Show("This tool will help you troubleshoot some various issues caused by Windows' default recovery system, which might make OmniMIDI " +
                "unavailable for use by other applications.\n\nClose any MIDI-related application you might have open, then press OK to start the troubleshooting.",
                "OmniMIDI - Troubleshooter", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult Q1 = MessageBox.Show("The configurator will now reset the Fault Tolerant Heap system, which sometimes might prevent some " +
                "libraries from loading properly.\n\nPress Yes to continue, or No to stop.", "OmniMIDI - Troubleshooter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (Q1 == DialogResult.No)
                return;

            Functions.ResetFaultTolerantHeap();

            DialogResult M1 = MessageBox.Show("OmniMIDI has reset the Fault Tolerant Heap system.\n\n" +
                "Open the MIDI application that had issues working and/or detecting OmniMIDI, and check if this fixed the issue.", 
                "OmniMIDI - Troubleshooter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (M1 == DialogResult.Yes)
                return;

            DialogResult Q2 = MessageBox.Show("The configurator will now reset all the AppCompatFlags in the system, " +
                "which sometimes might prevent OmniMIDI and its libraries from loading properly.\n\n" +
                "Press Yes to continue, or No to stop.", "OmniMIDI - Troubleshooter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (Q2 == DialogResult.No)
                return;

            Functions.ResetAppCompat();

            DialogResult M2 = MessageBox.Show("OmniMIDI has reset all the AppCompatFlags in the system.\n\n" +
                "Open the MIDI application that had issues working and/or detecting OmniMIDI, and check if this fixed the issue.",
                "OmniMIDI - Troubleshooter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (M2 == DialogResult.Yes)
                return;

            DialogResult Q3 = MessageBox.Show("The configurator will now reset all the settings and reinstall the driver from scratch.\n" +
                "This could help when dealing with a corrupted install of OmniMIDI.\nYou'll lose all your settings.\n\n" +
                "Press Yes to continue, or No to stop.", "OmniMIDI - Troubleshooter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (Q3 == DialogResult.No)
                return;

            var current = Process.GetCurrentProcess();
            Process.GetProcessesByName(current.ProcessName)
                .Where(t => t.Id != current.Id)
                .ToList()
                .ForEach(t => t.Kill());

            Properties.Settings.Default.Reset();
            UpdateSystem.CheckForTLS12ThenUpdate(FileVersionInfo.GetVersionInfo(UpdateSystem.UpdateFileVersion).FileVersion, UpdateSystem.WIPE_SETTINGS);
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
