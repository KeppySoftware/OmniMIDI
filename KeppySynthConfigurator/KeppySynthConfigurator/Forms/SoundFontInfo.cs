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
            LastMIDIPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathmidimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();

            // Here we go
            String next;
            Int32 fonthandle;
            FileInfo f;

            OriginalSF = SoundFont;

            if (SoundFont.ToLower().IndexOf('=') != -1)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(SoundFont, "[0-9]+");
                string sf = SoundFont.Substring(SoundFont.LastIndexOf('|') + 1);
                if (!File.Exists(sf))
                {
                    Functions.ShowErrorDialog(KeppySynthConfigurator.Properties.Resources.infoicon, System.Media.SystemSounds.Exclamation, "Error", String.Format("The SoundFont \"{0}\" doesn't exist.", SoundFont), false, null);
                    ERROR = true;
                    Close();
                }
                SoundFontT = sf;
                fonthandle = BassMidi.BASS_MIDI_FontInit(sf);
                f = new FileInfo(sf);
                next = sf;
            }
            else if (SoundFont.ToLower().IndexOf('@') != -1)
            {
                string sf = SoundFont.Substring(SoundFont.LastIndexOf('@') + 1);
                if (!File.Exists(sf))
                {
                    Functions.ShowErrorDialog(KeppySynthConfigurator.Properties.Resources.infoicon, System.Media.SystemSounds.Exclamation, "Error", String.Format("The SoundFont \"{0}\" doesn't exist.", SoundFont), false, null);
                    ERROR = true;
                    Close();
                }
                SoundFontT = sf;
                fonthandle = BassMidi.BASS_MIDI_FontInit(sf);
                f = new FileInfo(sf);
                next = sf;
            }
            else
            {
                if (!File.Exists(SoundFont))
                {
                    Functions.ShowErrorDialog(KeppySynthConfigurator.Properties.Resources.infoicon, System.Media.SystemSounds.Exclamation, "Error", String.Format("The SoundFont \"{0}\" doesn't exist.", SoundFont), false, null);
                    ERROR = true;
                    Close();
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

            SofSFLab.Text = String.Format("{0} bytes ({1}Presets: {2})",
                f.Length.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")),
                ReturnSamplesSize(fontinfo.samsize),
                String.Format("{0} bytes", (f.Length - fontinfo.samsize).ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"))));

            SFfLab.Text = ReturnFormat(Path.GetExtension(next));
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

        private string ReturnName(string name, string path)
        {
            if (name == null)
                return Path.GetFileNameWithoutExtension(path);
            else
                return name;
        }

        private string ReturnFormat(string fileext)
        {
            if (fileext.ToLowerInvariant() == ".sf1")
                return "SoundFont 1.x";
            else if (fileext.ToLowerInvariant() == ".sf2")
                return "SoundFont 2.x";
            else if (fileext.ToLowerInvariant() == ".sfz")
                return "SoundFontZ/Sforzando";
            else if (fileext.ToLowerInvariant() == ".sfpack")
                return "Compressed SoundFont 1.x/2.x";
            else if (fileext.ToLowerInvariant() == ".sfark")
                return "SfARK Compressed SoundFont 1.x/2.x";
            else
                return "Unknown or unsupported format";
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

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelStop(hStream);
            IsPreviewEnabled = false;
            Quitting = true;
            Close();
        }

        private void StartNormalPrvw_Click(object sender, EventArgs e)
        {
            MIDIPreview = String.Format("{0}{1}", Environment.GetEnvironmentVariable("windir"), "\\Media\\town.mid");
            IsPreviewEnabled = true;
            PreviewThread.RunWorkerAsync();
            PrvwBtn.Text = "Stop SoundFont preview";
            StartNormalPrvw1.Enabled = false;
            StartNormalPrvw2.Enabled = false;
            StartNormalPrvw3.Enabled = false;
            StartCustomPrvw.Enabled = false;
        }

        private void StartNormalPrvw2_Click(object sender, EventArgs e)
        {
            MIDIPreview = String.Format("{0}{1}", Environment.GetEnvironmentVariable("windir"), "\\Media\\onestop.mid");
            IsPreviewEnabled = true;
            PreviewThread.RunWorkerAsync();
            PrvwBtn.Text = "Stop SoundFont preview";
            StartNormalPrvw1.Enabled = false;
            StartNormalPrvw2.Enabled = false;
            StartNormalPrvw3.Enabled = false;
            StartCustomPrvw.Enabled = false;
        }

        private void StartNormalPrvw3_Click(object sender, EventArgs e)
        {
            MIDIPreview = String.Format("{0}{1}", Environment.GetEnvironmentVariable("windir"), "\\Media\\flourish.mid");
            IsPreviewEnabled = true;
            PreviewThread.RunWorkerAsync();
            PrvwBtn.Text = "Stop SoundFont preview";
            StartNormalPrvw1.Enabled = false;
            StartNormalPrvw2.Enabled = false;
            StartNormalPrvw3.Enabled = false;
            StartCustomPrvw.Enabled = false;
        }

        private void StartCustomPrvw_Click(object sender, EventArgs e)
        {
            CustomMIDI.InitialDirectory = LastMIDIPath;
            if (CustomMIDI.ShowDialog() == DialogResult.OK)
            {
                MIDIPreview = CustomMIDI.FileName;
                Functions.SetLastMIDIPath(Path.GetDirectoryName(CustomMIDI.FileName));
            }
            else
            {
                return;
            }
            IsPreviewEnabled = true;
            PreviewThread.RunWorkerAsync();
            PrvwBtn.Text = "Stop SoundFont preview";
            StartNormalPrvw1.Enabled = false;
            StartCustomPrvw.Enabled = false;
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

        private void PreviewThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Init BASS
                ChangePreviewButtonText("Initializing BASS...", false);
                ChangeWindowTitle("Initializing BASS...");
                Bass.BASS_StreamFree(hStream);
                Bass.BASS_Free();
                Bass.BASS_Init(-1, Convert.ToInt32(KeppySynthConfiguratorMain.Delegate.Frequency.Text), BASSInit.BASS_DEVICE_LATENCY, IntPtr.Zero);
                BASS_INFO info = Bass.BASS_GetInfo();
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, 0);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, info.minbuf + 10 + 50);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_MIDI_VOICES, Convert.ToInt32(KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value));
                System.Threading.Thread.Sleep(200);

                // Init stream
                ChangePreviewButtonText("Initializing stream...", false);
                ChangeWindowTitle("Initializing stream...");
                hStream = BassMidi.BASS_MIDI_StreamCreateFile(MIDIPreview, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_MIDI_DECAYEND | BASSFlag.BASS_SAMPLE_SOFTWARE, 0);
                Bass.BASS_ChannelSetAttribute(hStream, BASSAttribute.BASS_ATTRIB_MIDI_CPU, (int)(KeppySynthConfiguratorMain.Delegate.MaxCPU.Value / 100));
                System.Threading.Thread.Sleep(50);

                // Init SoundFont
                ChangePreviewButtonText("Loading SoundFont...", false);
                ChangeWindowTitle("Loading SoundFont...");
                BASS_MIDI_FONTEX[] fonts = new BASS_MIDI_FONTEX[1];
                List<int> termsList = new List<int>();
                termsList.Reverse();

                if (OriginalSF.ToLower().IndexOf('=') != -1)
                {
                    var matches = System.Text.RegularExpressions.Regex.Matches(OriginalSF, "[0-9]+");
                    string sf = OriginalSF.Substring(OriginalSF.LastIndexOf('|') + 1);
                    fonts[0].font = BassMidi.BASS_MIDI_FontInit(sf);
                    fonts[0].spreset = Convert.ToInt32(matches[0].ToString());
                    fonts[0].sbank = Convert.ToInt32(matches[1].ToString());
                    fonts[0].dpreset = Convert.ToInt32(matches[2].ToString());
                    fonts[0].dbank = Convert.ToInt32(matches[3].ToString());
                    BassMidi.BASS_MIDI_FontSetVolume(fonts[0].font, 1.0f);
                    BassMidi.BASS_MIDI_StreamSetFonts(hStream, fonts, 1);
                }
                else
                {
                    fonts[0].font = BassMidi.BASS_MIDI_FontInit(OriginalSF);
                    fonts[0].spreset = -1;
                    fonts[0].sbank = -1;
                    fonts[0].dpreset = -1;
                    fonts[0].dbank = 0;
                    BassMidi.BASS_MIDI_StreamSetFonts(hStream, fonts, 1);
                }

                BassMidi.BASS_MIDI_StreamLoadSamples(hStream);

                RestartStream:
                Bass.BASS_ChannelPlay(hStream, false);
                ChangePreviewButtonText("Stop SoundFont preview", true);
                ChangeWindowTitle(String.Format("Playing \"{0}\"", Path.GetFileNameWithoutExtension(MIDIPreview)));

                while (Bass.BASS_ChannelIsActive(hStream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    Bass.BASS_ChannelUpdate(hStream, 0);
                    System.Threading.Thread.Sleep(1);
                    if (!IsPreviewEnabled)
                    {
                        break;
                    }
                }

                if (!Quitting)
                {
                    if (LoopYesNo.Checked == true && IsPreviewEnabled == true)
                    {
                        goto RestartStream;
                    }

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
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(null, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex);
            }
        }

        void ChangePreviewButtonText(String Text, Boolean IsEnabled)
        {
            this.Invoke((MethodInvoker)delegate
            {
                PrvwBtn.Text = Text;
                PrvwBtn.Enabled = IsEnabled;
            });
        }

        void ChangeWindowTitle(String Text)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Text = String.Format("Keppy's Synthesizer - {0}", Text);
            });
        }

        private void LoopYesNo_Click(object sender, EventArgs e)
        {
            if (!LoopYesNo.Checked)
            {
                LoopYesNo.Checked = true;
            }
            else
            {
                LoopYesNo.Checked = false;
            }
        }
    }
}
