using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class OmniMapperCpl : Form
    {
        private const String CurDrvLab = "Current device: {0}";
        private static RegistryKey ActiveMovieKey = null;
        private static RegistryKey MIDIMapperKey = null;
        private static Int32 DeviceCount;

        public OmniMapperCpl()
        {
            InitializeComponent();
            DeviceCount = WinMM.midiOutGetNumDevs();
        }

        private void OmniMapperCpl_Load(object sender, EventArgs e)
        {
            try
            {
                ActiveMovieKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\ActiveMovie\devenum\{4EFE2452-168A-11D1-BC76-00C04FB9453B}\Default MidiOut Device", true);
                MIDIMapperKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Multimedia\MIDIMap", true);

                MIDIOUTCAPS OutCaps = new MIDIOUTCAPS();
                for (uint i = 0; i < DeviceCount; i++)
                {
                    WinMM.midiOutGetDevCaps(i, out OutCaps, (uint)Marshal.SizeOf(OutCaps));
                    if (OutCaps.szPname.Equals("OmniMapper")) continue;

                    MIDIOutList.Items.Add(OutCaps.szPname);
                }

                if (Functions.CheckMIDIMapper())
                {
                    bool Found = false;
                    String SelDevice = Program.Mapper.GetValue("TrgtSynth", "Microsoft GS Wavetable Synth").ToString();
                    CurDevice.Text = String.Format(CurDrvLab, SelDevice);
                    for (int i = 0; i < MIDIOutList.Items.Count; i++)
                    {
                        if (MIDIOutList.Items[i].ToString().Equals(SelDevice))
                        {
                            MIDIOutList.SelectedIndex = i;
                            Found = true;
                            break;
                        }
                    }

                    if (!Found) MIDIOutList.SelectedIndex = 0;
                }
                else
                {
                    Text = String.Format("Change {0} settings", Functions.IsWindows8OrLater() ? "Windows Media Player MIDI output" : "MIDI mapper");
                    if (ActiveMovieKey != null) MIDIOutList.SelectedIndex = Convert.ToInt32(ActiveMovieKey.GetValue("MidiOutId"));
                    else MIDIOutList.SelectedIndex = MIDIOutList.FindStringExact(MIDIMapperKey.GetValue("szPname", "Microsoft GS Wavetable Synth").ToString());
                    CurDevice.Text = String.Format(CurDrvLab, MIDIOutList.Items[MIDIOutList.SelectedIndex].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetNormalMIDIMapperToo()
        {
            try
            {
                if (ActiveMovieKey != null)
                {
                    ActiveMovieKey.SetValue("MidiOutId", MIDIOutList.SelectedIndex, RegistryValueKind.DWord);
                    ActiveMovieKey.Close();
                }

                if (MIDIMapperKey != null)
                {
                    MIDIMapperKey.SetValue("szPname", MIDIOutList.SelectedItem.ToString(), RegistryValueKind.String);
                    MIDIMapperKey.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("An error has occured while setting the default MIDI out device output.\nError: {0}", ex.ToString()), "OmniMIDI Configurator - ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            Program.Mapper.SetValue("TrgtSynth", MIDIOutList.Items[MIDIOutList.SelectedIndex].ToString(), Microsoft.Win32.RegistryValueKind.String);
            SetNormalMIDIMapperToo();
            Close();
        }
    }
}
