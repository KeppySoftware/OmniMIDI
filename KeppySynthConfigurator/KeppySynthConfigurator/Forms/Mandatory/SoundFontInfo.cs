using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

namespace KeppySynthConfigurator
{
    public partial class SoundFontInfo : Form
    {
        public static Boolean ERROR = false;
        public static Boolean Quitting = false;

        public static string LastMIDIPath { get; set; }
        bool bye;
        bool skip;
        String SoundFontT;

        // Preview
        String OriginalSF;
        String MIDIPreview;
        bool IsPreviewEnabled = false;
        int hStream;

        public SoundFontInfo(String SoundFont)
        {
            InitializeComponent();

            ERROR = false;

            LastMIDIPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathmidimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();

            // Here we go
            String next;
            String sf = "";
            Int32 fonthandle;
            FileInfo f;

            OriginalSF = SoundFont;

            if (SoundFont.ToLower().IndexOf('=') != -1)
            {
                sf = SoundFont.Substring(SoundFont.LastIndexOf('|') + 1);
                if (!File.Exists(sf))
                {
                    Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", String.Format("The SoundFont \"{0}\" doesn't exist.", SoundFont), false, null);
                    ERROR = true;
                    Close();
                    return;
                }
                SoundFontT = sf;
                fonthandle = BassMidi.BASS_MIDI_FontInit(sf);
                f = new FileInfo(sf);
                next = sf;
            }
            else
            {
                sf = SoundFont;
                if (!File.Exists(SoundFont))
                {
                    Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", String.Format("The SoundFont \"{0}\" doesn't exist.", SoundFont), false, null);
                    ERROR = true;
                    Close();
                    return;
                }
                SoundFontT = SoundFont;
                fonthandle = BassMidi.BASS_MIDI_FontInit(SoundFont);
                f = new FileInfo(SoundFont);
                next = SoundFont;
            }

            BASS_MIDI_FONTINFO fontinfo = BassMidi.BASS_MIDI_FontGetInfo(fonthandle);

            FNBox.Text = next;
            ISFBox.Text = ReturnName(fontinfo.name, next);
            CIBox.Text = ReturnCopyright(fontinfo.copyright);
            SamF.Text = String.Format("{0} ({1})", ReturnSampleType(fontinfo.samtype), fontinfo.samtype);

            if (f.Length > (long)2147483648)
            {
                SofSFLab.Font = new Font(SofSFLab.Font, FontStyle.Bold);
                SofSFLab.ForeColor = Color.DarkRed;
                SofSFLab.Cursor = Cursors.Help;
                SizeWarning.SetToolTip(SofSFLab, "SoundFonts bigger than 2GB may not load correctly\non 32-bit applications, cause audio corruptions or even cause loss of data!\n\nBe careful!");
            }

            if (Path.GetExtension(sf).ToLowerInvariant() == ".sfz")
            {
                SofSFLab.Text = String.Format("{0} (Samples: {1}, Presets: {2})",
                                Functions.ReturnLength(f.Length + SFZInfo.GetSoundFontZSize(sf), true),
                                Functions.ReturnLength(SFZInfo.GetSoundFontZSize(sf), true),
                                Functions.ReturnLength(f.Length - (long)fontinfo.samsize, false));
            }
            else
            {
                SofSFLab.Text = String.Format("{0} (Samples: {1}, Presets: {2})",
                                Functions.ReturnLength(f.Length, false),
                                Functions.ReturnLength(fontinfo.samsize, false),
                                Functions.ReturnLength(f.Length - (long)fontinfo.samsize, false));
            }

            SFfLab.Text = SFListFunc.ReturnSoundFontFormatMore(Path.GetExtension(next));
            CommentRich.Text = ReturnComment(fontinfo.comment);
            LELabel.Text = f.LastWriteTimeUtc.ToString();

            BassMidi.BASS_MIDI_FontFree(fonthandle);
        }

        private void KeppySynthSoundfontInfo_Load(object sender, EventArgs e)
        {
            // LAL
            FNBox.ContextMenu = RightClickMenu;
            ISFBox.ContextMenu = RightClickMenu;
            CIBox.ContextMenu = RightClickMenu;
            CommentRich.ContextMenu = RightClickMenu;
            ContextMenu = RightClickMenu;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelStop(hStream);
            Bass.BASS_Free();
            IsPreviewEnabled = false;
            Quitting = true;
            Close();
        }

        private void RunWorker()
        {
            IsPreviewEnabled = true;
            PreviewThread.RunWorkerAsync();
            PrvwBtn.Text = "Stop SoundFont preview";
            StartNormalPrvw1.Enabled = false;
            StartNormalPrvw2.Enabled = false;
            StartNormalPrvw3.Enabled = false;
            StartCustomPrvw.Enabled = false;
        }

        private void StartNormalPrvw_Click(object sender, EventArgs e)
        {
            MIDIPreview = String.Format("{0}{1}", Environment.GetEnvironmentVariable("windir"), "\\Media\\town.mid");
            RunWorker();
        }

        private void StartNormalPrvw2_Click(object sender, EventArgs e)
        {
            MIDIPreview = String.Format("{0}{1}", Environment.GetEnvironmentVariable("windir"), "\\Media\\onestop.mid");
            RunWorker();
        }

        private void StartNormalPrvw3_Click(object sender, EventArgs e)
        {
            MIDIPreview = String.Format("{0}{1}", Environment.GetEnvironmentVariable("windir"), "\\Media\\flourish.mid");
            RunWorker();
        }

        private void StartCustomPrvw_Click(object sender, EventArgs e)
        {
            CustomMIDI.InitialDirectory = LastMIDIPath;
            if (CustomMIDI.ShowDialog() == DialogResult.OK)
            {
                MIDIPreview = CustomMIDI.FileName;
                Functions.SetLastMIDIPath(Path.GetDirectoryName(CustomMIDI.FileName));
                RunWorker();
            }
            else return;
        }

        private void PrvwBtn_Click(object sender, EventArgs e)
        {
            if (!IsPreviewEnabled)
            {
                RightClickMenu.Show(PrvwBtn, new System.Drawing.Point(1, 20));
            }
            else
            {
                IsPreviewEnabled = false;
                StartNormalPrvw1.Enabled = true;
                StartNormalPrvw2.Enabled = true;
                StartNormalPrvw3.Enabled = true;
                StartCustomPrvw.Enabled = true;
            }
        }

        private void InitializeBASSMIDIStream()
        {
            // Init stream
            ChangePreviewButtonText("Initializing stream...", false);
            ChangeWindowTitle("Initializing stream...");
            hStream = BassMidi.BASS_MIDI_StreamCreateFile(MIDIPreview, 0L, 0L, 
                BASSFlag.BASS_SAMPLE_FLOAT |
                BASSFlag.BASS_SAMPLE_SOFTWARE |
                (KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked ? BASSFlag.BASS_DEFAULT : BASSFlag.BASS_MIDI_NOFX) |
                (LoopYesNo.Checked ? BASSFlag.BASS_SAMPLE_LOOP : BASSFlag.BASS_DEFAULT),
                0);
            Bass.BASS_ChannelSetAttribute(hStream, BASSAttribute.BASS_ATTRIB_MIDI_CPU, (int)(KeppySynthConfiguratorMain.Delegate.MaxCPU.Value / 100));
            System.Threading.Thread.Sleep(50);

            // Init SoundFont
            ChangePreviewButtonText("Loading SoundFont...", false);
            ChangeWindowTitle("Loading SoundFont...");
            BASS_MIDI_FONTEX[] fonts = new BASS_MIDI_FONTEX[1];

            if (OriginalSF.ToLower().IndexOf('=') != -1)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(OriginalSF, "[0-9]+");
                string sf = OriginalSF.Substring(OriginalSF.LastIndexOf('|') + 1);
                fonts[0].font = BassMidi.BASS_MIDI_FontInit(sf);
                fonts[0].spreset = Convert.ToInt32(matches[0].ToString());
                fonts[0].sbank = Convert.ToInt32(matches[1].ToString());
                fonts[0].dpreset = Convert.ToInt32(matches[2].ToString());
                fonts[0].dbank = Convert.ToInt32(matches[3].ToString());
            }
            else
            {
                fonts[0].font = BassMidi.BASS_MIDI_FontInit(OriginalSF);
                fonts[0].spreset = -1;
                fonts[0].sbank = -1;
                fonts[0].dpreset = -1;
                fonts[0].dbank = 0;
            }

            BassMidi.BASS_MIDI_FontSetVolume(fonts[0].font, 1.0f);
            BassMidi.BASS_MIDI_StreamSetFonts(hStream, fonts, 1);
            BassMidi.BASS_MIDI_StreamLoadSamples(hStream);
        }

        private void InitializeBASSMODStream()
        {
            // Init stream
            ChangePreviewButtonText("Initializing stream...", false);
            ChangeWindowTitle("Initializing stream...");
            hStream = Bass.BASS_MusicLoad(MIDIPreview, 0, 0, 
                BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_SAMPLE_SOFTWARE | (LoopYesNo.Checked ? BASSFlag.BASS_SAMPLE_LOOP : BASSFlag.BASS_DEFAULT),
                0);
            System.Threading.Thread.Sleep(50);
        }

        private void PreviewThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Init BASS
                ChangePreviewButtonText("Initializing BASS...", false);
                ChangeWindowTitle("Initializing BASS...");
                Bass.BASS_StreamFree(hStream);
                Bass.BASS_MusicFree(hStream);
                Bass.BASS_Free();
                Bass.BASS_Init(-1, Convert.ToInt32(KeppySynthConfiguratorMain.Delegate.Frequency.Text), BASSInit.BASS_DEVICE_LATENCY, IntPtr.Zero);
                BASS_INFO info = Bass.BASS_GetInfo();
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, 0);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, info.minbuf + 10 + 50);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_MIDI_VOICES, Convert.ToInt32(KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value));
                System.Threading.Thread.Sleep(200);

                if (Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".xm" 
                    || Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".it"
                    || Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".s3m"
                    || Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".mod"
                    || Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".mtm"
                    || Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".umx")
                {
                    InitializeBASSMODStream();
                }
                else if (Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".mid"
                    || Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".midi"
                    || Path.GetExtension(MIDIPreview).ToLowerInvariant() == ".rmi")
                {
                    InitializeBASSMIDIStream();
                }
                else
                {
                    MessageBox.Show("This is not a valid MIDI file.\n\nClick OK to abort.", "Keppy's Synthesizer - Preview error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ChangePreviewButtonText("Play SoundFont preview", true);
                    ChangeWindowTitle("Information about the SoundFont");
                    this.Invoke((MethodInvoker)delegate
                    {
                        StartNormalPrvw1.Enabled = true;
                        StartNormalPrvw2.Enabled = true;
                        StartNormalPrvw3.Enabled = true;
                        StartCustomPrvw.Enabled = true;
                    });
                    Bass.BASS_Free();
                    return;
                }

                int howmanytimes = 1;

                Bass.BASS_ChannelPlay(hStream, false);
                ChangePreviewButtonText("Stop SoundFont preview", true);
                ChangeWindowTitle(String.Format("Playing \"{0}\"", Path.GetFileNameWithoutExtension(MIDIPreview)));

                while (Bass.BASS_ChannelIsActive(hStream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    Bass.BASS_ChannelUpdate(hStream, 0);
                    if (!IsPreviewEnabled)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(1);
                }

                if (!Quitting)
                {
                    ChangePreviewButtonText("Play SoundFont preview", true);
                    ChangeWindowTitle("Information about the SoundFont");
                    this.Invoke((MethodInvoker)delegate
                    {
                        StartNormalPrvw1.Enabled = true;
                        StartNormalPrvw2.Enabled = true;
                        StartNormalPrvw3.Enabled = true;
                        StartCustomPrvw.Enabled = true;
                    });
                }

                Bass.BASS_StreamFree(hStream);
                Bass.BASS_MusicFree(hStream);
                Bass.BASS_Free();
            }
            catch { }
        }

        void ChangePreviewButtonText(String Text, Boolean IsEnabled)
        {
            if (!Quitting)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    PrvwBtn.Text = Text;
                    PrvwBtn.Enabled = IsEnabled;
                });
            }
        }

        void ChangeWindowTitle(String Text)
        {
            if (!Quitting)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = String.Format("Keppy's Synthesizer - {0}", Text);
                });
            }
        }

        private void LoopYesNo_Click(object sender, EventArgs e)
        {
            if (!LoopYesNo.Checked)
            {
                LoopYesNo.Checked = true;
                if (Bass.BASS_ChannelIsActive(hStream) == BASSActive.BASS_ACTIVE_PLAYING)
                    Bass.BASS_ChannelFlags(hStream, BASSFlag.BASS_SAMPLE_LOOP, BASSFlag.BASS_SAMPLE_LOOP);
            }
            else
            {
                LoopYesNo.Checked = false;
                if (Bass.BASS_ChannelIsActive(hStream) == BASSActive.BASS_ACTIVE_PLAYING)
                    Bass.BASS_ChannelFlags(hStream, 0, BASSFlag.BASS_SAMPLE_LOOP);
            }
        }

        private string ReturnName(string name, string path)
        {
            if (name == null)
                return Path.GetFileNameWithoutExtension(path);
            else
                return name;
        }

        private string ReturnCopyright(string copyright)
        {
            if (copyright == null)
                return "No copyright info available.";
            else
                return copyright;
        }

        private string ReturnComment(string comment)
        {
            if (comment == null)
                return "No comments available.";
            else
                return comment;
        }

        private string ReturnSamplesSize(int size)
        {
            if (size != 0)
                return String.Format("Samples: {0} bytes, ", size.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")));
            else
                return "";
        }

        private string ReturnSampleType(int type)
        {      
            if (type == 0) return "Waveform Audio File Format";
            else if (type == 256) return "MO3 format music";
            else if (type == 65536) return "User sample format";
            else if (type == 65538) return "OGG Vorbis";
            else if (type == 65539) return "MPEG-1 Audio Layer I";
            else if (type == 65540) return "MPEG-1 Audio Layer II/MPEG-2 Audio Layer II";
            else if (type == 65541) return "MPEG-2 Audio Layer III";
            else if (type == 65542) return "Audio Interchange File Format";
            else if (type == 65543) return "Apple CoreAudio";
            else if (type == 65544) return "Microsoft Media Foundation AAC";
            else if (type == 66048) return "Compact Disc Digital Audio";
            else if (type == 66304) return "Windows Media Audio";
            else if (type == 66305) return "MPEG-2 Audio Layer III over Windows Media Audio";
            else if (type == 66816) return "WavPack Lossless";
            else if (type == 66817) return "WavPack Hybrid Lossless";
            else if (type == 66818) return "WavPack Lossy";
            else if (type == 66819) return "WavPack Hybrid Lossy";
            else if (type == 66819) return "WavPack Hybrid Lossy";
            else if (type == 67072) return "OptimFROG";
            else if (type == 67328) return "Monkey's Audio";
            else if (type == 67840) return "Free Lossless Audio Codec";
            else if (type == 67841) return "Free Lossless Audio Codec Opus";
            else if (type == 68096) return "Musepack";
            else if (type == 68352) return "Advanced Audio Coding";
            else if (type == 68353) return "MPEG-4 Part 14";
            else if (type == 68608) return "Speex";
            else if (type == 69120) return "Apple Lossless Audio";
            else if (type == 69376) return "True Audio";
            else if (type == 69632) return "Dolby Digital AC-3";
            else if (type == 69888) return "Audio in video container";
            else if (type == 70144) return "OGG Opus";
            else if (type == 126976 || type == 126977) return "CRI Middleware AD-X";
            else if (type == 262144) return "Waveform Audio File Format (PCM)";
            else if (type == 327681) return "Waveform Audio File Format (PCM)";
            else if (type == 327683) return "Waveform Audio File Format (Float)";
            else return "Unknown format";
        }
    }
}
