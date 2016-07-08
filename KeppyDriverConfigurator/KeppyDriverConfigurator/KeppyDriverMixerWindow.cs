using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices; 
using System.Windows.Forms;
using System.Management;
using Microsoft.Win32;

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverMixerWindow : Form
    {
        uint CurrentClock, MaxStockClock;
        private static KeppyDriverMixerWindow inst;
        public static KeppyDriverMixerWindow GetForm
        {
            get
            {
                if (inst == null || inst.IsDisposed)
                {
                    inst = new KeppyDriverMixerWindow();
                }
                else
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    Application.OpenForms["KeppyDriverMixerWindow"].BringToFront();
                }
                return inst;
            }
        }

        public KeppyDriverMixerWindow()
        {
            this.Controls.Add(LeftChannel);
            InitializeComponent();
        }

        public void CPUSpeed()
        {
            using (ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'"))
            {
                CurrentClock = (uint)(Mo["CurrentClockSpeed"]);
                MaxStockClock = (uint)(Mo["MaxClockSpeed"]);
            }
        }

        private void LeftChannelText(string text)
        {
            using (Graphics gr = LeftChannel.CreateGraphics())
            {
                gr.DrawString(text,
                    SystemFonts.DefaultFont,
                    Brushes.Black,
                    new PointF(LeftChannel.Width / 2 - (gr.MeasureString(text,
                        SystemFonts.DefaultFont).Width / 2.0F),
                    LeftChannel.Height / 2 - (gr.MeasureString(text,
                        SystemFonts.DefaultFont).Height / 2.0F)));
            }
        }

        private void RightChannelText(string text)
        {
            using (Graphics gr = RightChannel.CreateGraphics())
            {
                gr.DrawString(text,
                    SystemFonts.DefaultFont,
                    Brushes.Black,
                    new PointF(RightChannel.Width / 2 - (gr.MeasureString(text,
                        SystemFonts.DefaultFont).Width / 2.0F),
                    RightChannel.Height / 2 - (gr.MeasureString(text,
                        SystemFonts.DefaultFont).Height / 2.0F)));
            }
        }

        private void fullVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CH1VOL.Value = 127;
            CH2VOL.Value = 127;
            CH3VOL.Value = 127;
            CH4VOL.Value = 127;
            CH5VOL.Value = 127;
            CH6VOL.Value = 127;
            CH7VOL.Value = 127;
            CH8VOL.Value = 127;
            CH9VOL.Value = 127;
            CH10VOL.Value = 127;
            CH11VOL.Value = 127;
            CH12VOL.Value = 127;
            CH13VOL.Value = 127;
            CH14VOL.Value = 127;
            CH15VOL.Value = 127;
            CH16VOL.Value = 127;
        }

        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CH1VOL.Value = 100;
            CH2VOL.Value = 100;
            CH3VOL.Value = 100;
            CH4VOL.Value = 100;
            CH5VOL.Value = 100;
            CH6VOL.Value = 100;
            CH7VOL.Value = 100;
            CH8VOL.Value = 100;
            CH9VOL.Value = 100;
            CH10VOL.Value = 100;
            CH11VOL.Value = 100;
            CH12VOL.Value = 100;
            CH13VOL.Value = 100;
            CH14VOL.Value = 100;
            CH15VOL.Value = 100;
            CH16VOL.Value = 100;
        }

        private void muteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CH1VOL.Value = 0;
            CH2VOL.Value = 0;
            CH3VOL.Value = 0;
            CH4VOL.Value = 0;
            CH5VOL.Value = 0;
            CH6VOL.Value = 0;
            CH7VOL.Value = 0;
            CH8VOL.Value = 0;
            CH9VOL.Value = 0;
            CH10VOL.Value = 0;
            CH11VOL.Value = 0;
            CH12VOL.Value = 0;
            CH13VOL.Value = 0;
            CH14VOL.Value = 0;
            CH15VOL.Value = 0;
            CH16VOL.Value = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VolumeCheck_Tick(object sender, EventArgs e)
        {
            RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver", false);
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", false);

            if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 1) {
                LeftChannel.Value = Convert.ToInt32(Debug.GetValue("leftvol"));
                if (LeftChannel.Value == 32768)
                {
                    LeftChannelText("Clipping!");
                }

                RightChannel.Value = Convert.ToInt32(Debug.GetValue("rightvol"));
                if (RightChannel.Value == 32768)
                {
                    RightChannelText("Clipping!");
                }
            }
            else
            {
                LeftChannel.Value = 0;
                RightChannel.Value = 0;
                LeftChannelText("N/A");
                RightChannelText("N/A");
            }
        }

        private void ChannelVolume_Tick(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Channels", true);
                if (Channels == null) {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Driver\\Channels");
                }     
                Channels.SetValue("ch1", CH1VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch2", CH2VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch3", CH3VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch4", CH4VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch5", CH5VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch6", CH6VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch7", CH7VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch8", CH8VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch9", CH9VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch10", CH10VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch11", CH11VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch12", CH12VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch13", CH13VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch14", CH14VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch15", CH15VOL.Value.ToString(), RegistryValueKind.DWord);
                Channels.SetValue("ch16", CH16VOL.Value.ToString(), RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not write settings to the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void KeppyDriverMixerWindow_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Channels", true);
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", false);
                if (Channels == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Driver\\Channels");
                    return;
                }
                CH1VOL.Value = Convert.ToInt32(Channels.GetValue("ch1"));
                CH2VOL.Value = Convert.ToInt32(Channels.GetValue("ch2"));
                CH3VOL.Value = Convert.ToInt32(Channels.GetValue("ch3"));
                CH4VOL.Value = Convert.ToInt32(Channels.GetValue("ch4"));
                CH5VOL.Value = Convert.ToInt32(Channels.GetValue("ch5"));
                CH6VOL.Value = Convert.ToInt32(Channels.GetValue("ch6"));
                CH7VOL.Value = Convert.ToInt32(Channels.GetValue("ch7"));
                CH8VOL.Value = Convert.ToInt32(Channels.GetValue("ch8"));
                CH9VOL.Value = Convert.ToInt32(Channels.GetValue("ch9"));
                CH10VOL.Value = Convert.ToInt32(Channels.GetValue("ch10"));
                CH11VOL.Value = Convert.ToInt32(Channels.GetValue("ch11"));
                CH12VOL.Value = Convert.ToInt32(Channels.GetValue("ch12"));
                CH13VOL.Value = Convert.ToInt32(Channels.GetValue("ch13"));
                CH14VOL.Value = Convert.ToInt32(Channels.GetValue("ch14"));
                CH15VOL.Value = Convert.ToInt32(Channels.GetValue("ch15"));
                CH16VOL.Value = Convert.ToInt32(Channels.GetValue("ch16"));
                if (Convert.ToInt32(Settings.GetValue("volumemon")) == 1)
                {
                    VolumeMonitor.Checked = true;
                    VolumeCheck.Enabled = true;
                }
                else
                {
                    VolumeMonitor.Checked = false;
                    VolumeCheck.Enabled = false;
                }
                if (Convert.ToInt32(Settings.GetValue("midivolumeoverride")) == 1)
                {
                    MIDIVolumeOverride.Checked = true;
                }
                else
                {
                    MIDIVolumeOverride.Checked = false;
                }
                ChannelVolume.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not read settings from the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void VolumeMonitor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                if (VolumeMonitor.Checked == true)
                {
                    CPUSpeed();
                    if (CurrentClock < 2000)
                    {
                        MessageBox.Show("A CPU running at 2GHz or more is required for the volume meters to work.", "Minimum requirements", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        VolumeMonitor.Checked = false;
                    }
                    else if (CurrentClock >= 2000)
                    {
                        Settings.SetValue("volumemon", "1", RegistryValueKind.DWord);
                        VolumeCheck.Enabled = true;
                        LeftChannel.Value = 0;
                        RightChannel.Value = 0;   
                    }
                }
                else
                {
                    Settings.SetValue("volumemon", "0", RegistryValueKind.DWord);
                    VolumeCheck.Enabled = false;
                    LeftChannel.Value = 0;
                    RightChannel.Value = 0;     
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not write settings to the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void MIDIVolumeOverride_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                if (MIDIVolumeOverride.Checked == true)
                {
                    Settings.SetValue("midivolumeoverride", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("midivolumeoverride", "0", RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not write settings to the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CH16VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH16VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH16VOL, Convert.ToString("Channel 16: " + percentage + "%"));
        }

        private void CH15VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH15VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH15VOL, Convert.ToString("Channel 15: " + percentage + "%"));
        }

        private void CH14VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH14VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH14VOL, Convert.ToString("Channel 14: " + percentage + "%"));
        }

        private void CH13VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH13VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH13VOL, Convert.ToString("Channel 13: " + percentage + "%"));
        }

        private void CH12VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH12VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH12VOL, Convert.ToString("Channel 12: " + percentage + "%"));
        }

        private void CH11VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH11VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH11VOL, Convert.ToString("Channel 11: " + percentage + "%"));
        }

        private void CH10VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH10VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH10VOL, Convert.ToString("Channel 10: " + percentage + "%"));
        }

        private void CH9VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH9VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH9VOL, Convert.ToString("Channel 9: " + percentage + "%"));
        }

        private void CH8VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH8VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH8VOL, Convert.ToString("Channel 8: " + percentage + "%"));
        }

        private void CH7VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH7VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH7VOL, Convert.ToString("Channel 7: " + percentage + "%"));
        }

        private void CH6VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH6VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH6VOL, Convert.ToString("Channel 6: " + percentage + "%"));
        }

        private void CH5VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH5VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH5VOL, Convert.ToString("Channel 5: " + percentage + "%"));
        }

        private void CH4VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH4VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH4VOL, Convert.ToString("Channel 4: " + percentage + "%"));
        }

        private void CH3VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH3VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH3VOL, Convert.ToString("Channel 3: " + percentage + "%"));
        }

        private void CH2VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH2VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH2VOL, Convert.ToString("Channel 2: " + percentage + "%"));
        }

        private void CH1VOL_Scroll(object sender, EventArgs e)
        {
            int percentage = (int)Math.Round((double)(100 * CH1VOL.Value) / 127); ;
            VolumeTip.SetToolTip(CH1VOL, Convert.ToString("Channel 1: " + percentage + "%"));
        }
    }
}
