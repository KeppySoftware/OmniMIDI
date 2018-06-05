using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace OmniMIDIConfigurator
{
    class WatchdogFunc
    {
        // Watchdog features in the configurator and hotkeys

        public static void LoadSoundfont(int whichone)
        {
            OmniMIDIConfiguratorMain.Watchdog.SetValue("currentsflist", whichone, RegistryValueKind.DWord);
            OmniMIDIConfiguratorMain.Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
        }

        public static void ReloadList1_Click(object sender, EventArgs e)
        {
            LoadSoundfont(1);
        }

        public static void ReloadList2_Click(object sender, EventArgs e)
        {
            LoadSoundfont(2);
        }

        public static void ReloadList3_Click(object sender, EventArgs e)
        {
            LoadSoundfont(3);
        }

        public static void ReloadList4_Click(object sender, EventArgs e)
        {
            LoadSoundfont(4);
        }

        public static void ReloadList5_Click(object sender, EventArgs e)
        {
            LoadSoundfont(5);
        }

        public static void ReloadList6_Click(object sender, EventArgs e)
        {
            LoadSoundfont(6);
        }

        public static void ReloadList7_Click(object sender, EventArgs e)
        {
            LoadSoundfont(7);
        }

        public static void ReloadList8_Click(object sender, EventArgs e)
        {
            LoadSoundfont(8);
        }

        public static void ReloadList9_Click(object sender, EventArgs e)
        {
            LoadSoundfont(9);
        }

        public static void ReloadList10_Click(object sender, EventArgs e)
        {
            LoadSoundfont(10);
        }

        public static void ReloadList11_Click(object sender, EventArgs e)
        {
            LoadSoundfont(11);
        }

        public static void ReloadList12_Click(object sender, EventArgs e)
        {
            LoadSoundfont(12);
        }

        public static void ReloadList13_Click(object sender, EventArgs e)
        {
            LoadSoundfont(13);
        }

        public static void ReloadList14_Click(object sender, EventArgs e)
        {
            LoadSoundfont(14);
        }

        public static void ReloadList15_Click(object sender, EventArgs e)
        {
            LoadSoundfont(15);
        }

        public static void ReloadList16_Click(object sender, EventArgs e)
        {
            LoadSoundfont(16);
        }

        public static void sendAMIDIResetEventToAllTheChannelsStrip_Click(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.Watchdog.SetValue("resetchannels", 1, RegistryValueKind.DWord);
        }
    }
}
