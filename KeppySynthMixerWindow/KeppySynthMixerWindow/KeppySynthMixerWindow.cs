using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Management;
using Microsoft.Win32;

namespace KeppySynthMixerWindow
{
    public partial class KeppySynthMixerWindow : Form
    {
        uint MaxStockClock;
        int dostuff = 0;

        public KeppySynthMixerWindow()
        {
            InitializeComponent();
        }

        public void CPUSpeed()
        {
            RegistryKey registrykeyHKLM = Registry.LocalMachine;
            string keyPath = @"HARDWARE\DESCRIPTION\System\CentralProcessor\0";
            RegistryKey registrykeyCPU = registrykeyHKLM.OpenSubKey(keyPath, false);
            MaxStockClock = Convert.ToUInt32(registrykeyCPU.GetValue("~MHz").ToString());
            registrykeyCPU.Close();
            registrykeyHKLM.Close();
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
            Close();
        }

        void CloseMixer(object sender, CancelEventArgs e)
        {
            Close();
        }

        private void VolumeCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    if (dostuff != 0)
                    {
                        RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer", false);
                        RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", false);

                        if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 1)
                        {
                            int left = Convert.ToInt32(Debug.GetValue("leftvol"));
                            int right = Convert.ToInt32(Debug.GetValue("rightvol"));

                            LeftChannel.Invoke((MethodInvoker)(() => LeftChannel.SetProgressNoAnimation(left)));
                            RightChannel.Invoke((MethodInvoker)(() => RightChannel.SetProgressNoAnimation(right)));

                            if (LeftChannel.Value >= 28672)
                            {
                                label1.Invoke((MethodInvoker)(() => label1.Text = "╬ L:"));
                                ModifyProgressBarColor.SetState(LeftChannel, 2);
                            }
                            else if (LeftChannel.Value >= 16384 && LeftChannel.Value <= 28671)
                            {
                                label1.Invoke((MethodInvoker)(() => label1.Text = "╦ L:"));
                                ModifyProgressBarColor.SetState(LeftChannel, 3);
                            }
                            else if (LeftChannel.Value >= 1 && LeftChannel.Value <= 16383)
                            {
                                label1.Invoke((MethodInvoker)(() => label1.Text = "L:"));
                                ModifyProgressBarColor.SetState(LeftChannel, 1);
                            }
                            else if (LeftChannel.Value <= 0)
                            {
                                label1.Invoke((MethodInvoker)(() => label1.Text = "■ L:"));
                                ModifyProgressBarColor.SetState(LeftChannel, 1);
                            }

                            if (RightChannel.Value >= 28672)
                            {
                                label2.Invoke((MethodInvoker)(() => label2.Text = "╬ R:"));
                                ModifyProgressBarColor.SetState(RightChannel, 2);
                            }
                            else if (RightChannel.Value >= 16384 && RightChannel.Value <= 28671)
                            {
                                label2.Invoke((MethodInvoker)(() => label2.Text = "╦ R:"));
                                ModifyProgressBarColor.SetState(RightChannel, 3);
                            }
                            else if (RightChannel.Value >= 1 && RightChannel.Value <= 16383)
                            {
                                label2.Invoke((MethodInvoker)(() => label2.Text = "R:"));
                                ModifyProgressBarColor.SetState(RightChannel, 1);
                            }
                            else if (RightChannel.Value <= 0)
                            {
                                label2.Invoke((MethodInvoker)(() => label2.Text = "■ R:"));
                                ModifyProgressBarColor.SetState(RightChannel, 1);
                            }
                        }
                        else
                        {
                            LeftChannel.Value = 0;
                            RightChannel.Value = 0;
                            label1.Text = "█ L:";
                            label2.Text = "█ R:";
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }          
        }

        private void KeppySynthMixerWindow_Load(object sender, EventArgs e)
        {
            try
            {
                CPUSpeed();
                VolumeCheck.RunWorkerAsync();
                RegistryKey Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Channels", true);
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", false);
                if (Channels == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Channels");
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
                    dostuff = 1;
                }
                else
                {
                    VolumeMonitor.Checked = false;
                    dostuff = 0;
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

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Program.BringToFrontMessage)
            {
                WinAPI.ShowWindow(Handle, WinAPI.SW_RESTORE);
                WinAPI.SetForegroundWindow(Handle);
            }
            base.WndProc(ref m);
        }

        private void VolumeMonitor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                if (VolumeMonitor.Checked == true)
                {

                    if (MaxStockClock < 1100)
                    {
                        DialogResult dialogResult = MessageBox.Show("Enabling a mixer on a computer with poor specs could make the driver stutter.\n\nAre you sure you want to enable it?", "Weak processor detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Settings.SetValue("volumemon", "1", RegistryValueKind.DWord);
                            dostuff = 1;
                            LeftChannel.Value = 0;
                            RightChannel.Value = 0;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            VolumeMonitor.Checked = false;
                        }
                    }
                    else if (MaxStockClock >= 1100)
                    {
                        Settings.SetValue("volumemon", "1", RegistryValueKind.DWord);
                        dostuff = 1;
                        LeftChannel.Value = 0;
                        RightChannel.Value = 0;
                    }
                }
                else
                {
                    Settings.SetValue("volumemon", "0", RegistryValueKind.DWord);
                    dostuff = 0;
                    LeftChannel.Value = 0;
                    RightChannel.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not write settings to the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ChannelVolume_Tick(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Channels", true);
                if (Channels == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Channels");
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

            System.Threading.Thread.Sleep(1);
        }

        private void MIDIVolumeOverride_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
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

        private void showTheConfiguratorWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthConfigurator.exe");
        }

        private void VolumeToolTip(string channel, TrackBar trackbar)
        {
            int percentage = (int)Math.Round((double)(100 * trackbar.Value) / 127); ;
            VolumeTip.SetToolTip(trackbar, String.Format("{0}: {1}%", channel, percentage));
        }

        private void CH16VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 16", CH16VOL);
        }

        private void CH15VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 15", CH15VOL);
        }

        private void CH14VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 14", CH14VOL);
        }

        private void CH13VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 13", CH13VOL);
        }

        private void CH12VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 12", CH12VOL);
        }

        private void CH11VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 11", CH11VOL);
        }

        private void CH10VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 10", CH10VOL);
        }

        private void CH9VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 9", CH9VOL);
        }

        private void CH8VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 8", CH8VOL);
        }

        private void CH7VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 7", CH7VOL);
        }

        private void CH6VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 6", CH6VOL);
        }

        private void CH5VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 5", CH5VOL);
        }

        private void CH4VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 4", CH4VOL);
        }

        private void CH3VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 3", CH3VOL);
        }

        private void CH2VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 2", CH2VOL);
        }

        private void CH1VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 1", CH1VOL);
        }

        private void MainVol_Scroll(object sender, EventArgs e)
        {
            CH1VOL.Value = CH2VOL.Value = CH3VOL.Value = CH4VOL.Value = CH5VOL.Value = CH6VOL.Value = CH7VOL.Value = CH8VOL.Value = CH9VOL.Value = CH10VOL.Value = CH11VOL.Value = CH12VOL.Value = CH13VOL.Value = CH14VOL.Value = CH15VOL.Value = CH16VOL.Value = MainVol.Value;
        }

        // Snap feature

        private const int SnapDist = 25;

        private bool DoSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= SnapDist;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            Screen scn = Screen.FromPoint(this.Location);
            if (DoSnap(this.Left, scn.WorkingArea.Left)) this.Left = scn.WorkingArea.Left;
            if (DoSnap(this.Top, scn.WorkingArea.Top)) this.Top = scn.WorkingArea.Top;
            if (DoSnap(scn.WorkingArea.Right, this.Right)) this.Left = scn.WorkingArea.Right - this.Width;
            if (DoSnap(scn.WorkingArea.Bottom, this.Bottom)) this.Top = scn.WorkingArea.Bottom - this.Height;
        }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Sets the progress bar value, without using 'Windows Aero' animation.
        /// This is to work around a known WinForms issue where the progress bar 
        /// is slow to update. 
        /// </summary>
        public static void SetProgressNoAnimation(this ProgressBar pb, int value)
        {
            // To get around the progressive animation, we need to move the 
            // progress bar backwards.
            if (value == pb.Maximum)
            {
                // Special case as value can't be set greater than Maximum.
                pb.Maximum = value + 1;     // Temporarily Increase Maximum
                pb.Value = value + 1;       // Move past
                pb.Maximum = value;         // Reset maximum
            }
            else
            {
                pb.Value = value + 1;       // Move past
            }
            pb.Value = value;               // Move to correct value
        }
    }

    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]

        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        public static void SetState(this ProgressBar pBar, int state)
        {
            pBar.Invoke((MethodInvoker)(() => SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero)));
            // state 1 = Normal
            // state 2 = Error
            // state 3 = Warning
        }
    }
}