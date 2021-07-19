using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class MIDIFeedback : Form
    {
        public string FeedbackWhitelistLocation = String.Format("{0}\\{1}", Environment.GetEnvironmentVariable("USERPROFILE"), "OmniMIDI\\MIDIFeedback.whitelist");

        public MIDIFeedback()
        {
            InitializeComponent();

            bool DevFound = false;
            int NumDevs = WinMM.midiOutGetNumDevs();
            string OldDev = (string)Program.SynthSettings.GetValue("FeedbackDevice", "Microsoft GS Wavetable Synth");
            MIDIOUTCAPS Caps = new MIDIOUTCAPS();

            for (int i = 0; i < NumDevs; i++)
            {
                WinMM.midiOutGetDevCaps((uint)i, out Caps, (uint)Marshal.SizeOf(Caps));
                if (!Caps.szPname.Equals("OmniMIDI"))
                    MIDIOutDevs.Items.Add(Caps.szPname);
            }

            if (MIDIOutDevs.Items.Count < 1)
            {
                MessageBox.Show("No MIDI output devices available!\n\nPress OK to continue.", "OmniMIDI - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int c = 0; c < MIDIOutDevs.Items.Count; c++)
            {
                if (MIDIOutDevs.Items[c].Equals(OldDev))
                { 
                    MIDIOutDevs.SelectedIndex = c;
                    DevFound = true;
                    break;
                }
            }

            if (!DevFound)
            {
                int index = MIDIOutDevs.FindStringExact("Microsoft GS Wavetable Synth");
                if (index != -1) MIDIOutDevs.SelectedIndex = index;
            }

            EnableFeedback.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FeedbackEnabled", 0));

            if (!File.Exists(FeedbackWhitelistLocation))
                File.Create(FeedbackWhitelistLocation).Dispose();

            try
            {
                // Import the blacklist file
                using (StreamReader r = new StreamReader(FeedbackWhitelistLocation))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        FeedbackWhitelist.Items.Add(line);
                    }
                }
            }
            catch { File.Create(FeedbackWhitelistLocation).Dispose(); }
        }

        private void MIDIFeedback_Load(object sender, EventArgs e)
        {
            // Nothing
        }

        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.MinimizeBox = false;
            inputBox.MaximizeBox = false;
            inputBox.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "App name/path";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        private void SaveWhitelist()
        {
            using (StreamWriter sw = new StreamWriter(FeedbackWhitelistLocation))
            {
                try
                {
                    foreach (var item in FeedbackWhitelist.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while saving the whitelist!\n\n.NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                String Program = null;
                ShowInputDialog(ref Program);
                if (Program != null || Program != "")
                {
                    FeedbackWhitelist.Items.Add(Program);
                    SaveWhitelist();
                }
            }
            else
            {
                WhitelistPicker.FileName = "";
                if (WhitelistPicker.ShowDialog() == DialogResult.OK)
                {
                    foreach (string str in WhitelistPicker.FileNames)
                    {
                        RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);

                        if (SynthPaths == null)
                            SynthPaths = Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Paths");

                        SynthPaths.SetValue("lastpathblacklist", Path.GetDirectoryName(str), RegistryValueKind.String);
                        SynthPaths.Close();
                        FeedbackWhitelist.Items.Add(str);
                    }
                    SaveWhitelist();
                }
            }
        }

        private void RmvBtn_Click(object sender, EventArgs e)
        {
            int SI = FeedbackWhitelist.SelectedIndices.Count;

            for (int i = SI - 1; i >= 0; i--)
                FeedbackWhitelist.Items.RemoveAt(FeedbackWhitelist.SelectedIndices[i]);

            if (SI != 0)
                SaveWhitelist();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("FeedbackDevice", MIDIOutDevs.Items.Count > 0 ? MIDIOutDevs.SelectedItem.ToString() : "None", RegistryValueKind.String);
            Program.SynthSettings.SetValue("FeedbackEnabled", EnableFeedback.Checked ? 1 : 0, RegistryValueKind.DWord);

            SaveWhitelist();

            Functions.SignalLiveChanges();

            Close();
        }
    }
}
