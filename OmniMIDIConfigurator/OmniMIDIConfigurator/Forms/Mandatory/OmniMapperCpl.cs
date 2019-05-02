﻿using Microsoft.Win32;
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
                RegistryKey CLSID = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Drivers32", false);
                bool OmniMapperInstalled = (CLSID.GetValue("midimapper", "midimap.dll").ToString() == "OmniMIDI\\OmniMapper.dll");
                CLSID.Close();

                ActiveMovieKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\ActiveMovie\devenum\{4EFE2452-168A-11D1-BC76-00C04FB9453B}\Default MidiOut Device", true);

                MIDIOUTCAPS OutCaps = new MIDIOUTCAPS();
                for (uint i = 0; i < DeviceCount; i++)
                {
                    WinMM.midiOutGetDevCaps(i, out OutCaps, (uint)Marshal.SizeOf(OutCaps));
                    if (OutCaps.szPname.Equals("OmniMapper")) continue;

                    MIDIOutList.Items.Add(OutCaps.szPname);
                }

                if (OmniMapperInstalled)
                {
                    bool Found = false;
                    String SelDevice = OmniMIDIConfiguratorMain.Mapper.GetValue("TrgtSynth", "Microsoft GS Wavetable Synth").ToString();
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
                    MIDIOutList.SelectedIndex = Convert.ToInt32(ActiveMovieKey.GetValue("MidiOutId"));
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
                ActiveMovieKey.SetValue("MidiOutId", MIDIOutList.SelectedIndex, RegistryValueKind.DWord);
                ActiveMovieKey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("An error has occured while setting the default MIDI out device output.\nError: {0}", ex.ToString()), "OmniMIDI Configurator - ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.Mapper.SetValue("TrgtSynth", MIDIOutList.Items[MIDIOutList.SelectedIndex].ToString(), Microsoft.Win32.RegistryValueKind.String);
            SetNormalMIDIMapperToo();
            Close();
        }
    }
}
