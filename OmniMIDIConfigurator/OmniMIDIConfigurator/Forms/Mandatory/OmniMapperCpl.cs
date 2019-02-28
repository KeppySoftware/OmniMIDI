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
        private const string CurDrvLab = "Current device: {0}";
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
                MIDIOUTCAPS OutCaps = new MIDIOUTCAPS();
                for (uint i = 0; i < DeviceCount; i++)
                {
                    WinMM.midiOutGetDevCaps(i, out OutCaps, (uint)Marshal.SizeOf(OutCaps));
                    if (OutCaps.szPname.Equals("OmniMapper")) continue;

                    MIDIOutList.Items.Add(OutCaps.szPname);
                }

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.Mapper.SetValue("TrgtSynth", MIDIOutList.Items[MIDIOutList.SelectedIndex].ToString(), Microsoft.Win32.RegistryValueKind.String);
            Close();
        }
    }
}
