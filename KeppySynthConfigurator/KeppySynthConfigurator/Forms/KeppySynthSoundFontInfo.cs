using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthSoundfontInfo : Form
    {
        public KeppySynthSoundfontInfo(String SoundFont)
        {
            InitializeComponent();
            // Here we go
            String next;
            Int32 fonthandle;
            FileInfo f;

            if (SoundFont.ToLower().IndexOf('=') != -1)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(SoundFont, "[0-9]+");
                string sf = SoundFont.Substring(SoundFont.LastIndexOf('|') + 1);
                fonthandle = BassMidi.BASS_MIDI_FontInit(sf);
                f = new FileInfo(sf);
                next = sf;
            }
            else if (SoundFont.ToLower().IndexOf('@') != -1)
            {
                string sf = SoundFont.Substring(SoundFont.LastIndexOf('@') + 1);
                fonthandle = BassMidi.BASS_MIDI_FontInit(sf);
                f = new FileInfo(sf);
                next = sf;
            }
            else
            {
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
        }

        private void KeppySynthSoundfontInfo_Load(object sender, EventArgs e)
        {
            // LAL
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
            Close();
        }
    }
}
