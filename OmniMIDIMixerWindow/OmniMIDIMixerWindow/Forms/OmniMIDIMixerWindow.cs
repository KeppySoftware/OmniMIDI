using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace OmniMIDIMixerWindow
{
    public partial class OmniMIDIMixerWindow : Form
    {
        public static OmniMIDIMixerWindow Delegate;
        public static RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI", true);
        public static RegistryKey Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Channels", true);
        public static RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Configuration", true);
        public static Boolean VUStatus = false;
        int MaxStockClock;

        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public extern static Int32 SetWindowTheme(IntPtr hWnd,
              String textSubAppName, String textSubIdList);

        public static string[] RegValName = { "ch1", "ch2", "ch3", "ch4", "ch5", "ch6", "ch7", "ch8", "ch9", "ch10", "ch11", "ch12", "ch13", "ch14", "ch15", "ch16", "cha" };
        public static int[] RegValInt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public OmniMIDIMixerWindow(string[] args)
        {
            InitializeComponent();
            foreach (String s in args)
            {
                if (s.ToLowerInvariant() == "/1980")
                {
                    ItsThe80sTheme.Visible = true;
                    break;
                }
            }
        }

        public void CPUSpeed()
        {
            ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("SELECT * FROM CIM_Processor");
            foreach (ManagementObject moProcessor in mosProcessor.Get())
            {
                MaxStockClock = int.Parse(moProcessor["maxclockspeed"].ToString());
            }
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
            MainVol.Value = 100;
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
            MainVol.Value = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        void CloseMixer(object sender, CancelEventArgs e)
        {
            Close();
        }

        private void ChangeLeftChannelSize(Size UseSize)
        {
            LV1.Size = UseSize;
            LV2.Size = UseSize;
            LV3.Size = UseSize;
            LV4.Size = UseSize;
            LV5.Size = UseSize;
            LV6.Size = UseSize;
            LV7.Size = UseSize;
            LV8.Size = UseSize;
            LV9.Size = UseSize;
            LV10.Size = UseSize;
            LV11.Size = UseSize;
            LV12.Size = UseSize;
            LV13.Size = UseSize;
            LV14.Size = UseSize;
            LV15.Size = UseSize;
            LV16.Size = UseSize;
            LV17.Size = UseSize;
            LV18.Size = UseSize;
            LV19.Size = UseSize;
            LV20.Size = UseSize;
            LV21.Size = UseSize;
            LV22.Size = UseSize;
        }

        private void VolumeCheck_Tick(object sender, EventArgs e)
        {
            try
            {
                int left = Convert.ToInt32(Debug.GetValue("leftvol", 0));
                int right = Convert.ToInt32(Debug.GetValue("rightvol", 0));
                var perc = ((double)((left + right) / 2) / 32768) * 100;

                VolLevel.Text = String.Format("{0}%", Convert.ToInt32(Math.Round(perc, 0)).ToString());

                if (Convert.ToInt32(Settings.GetValue("MonoRendering", 0)) == 1) {
                    Size UseSize = new Size(39, 5);
                    LLab.Size = new Size(39, 16);
                    LLab.Text = "LVL";
                    ChangeLeftChannelSize(UseSize);
                    MeterFunc.ChangeMeter(0, left);
                    MeterFunc.ChangeMeter(1, 0);
                    MeterFunc.AverageMeter(left, left);
                }
                else
                {
                    Size UseSize = new Size(16, 5);
                    LLab.Size = new Size(16, 16);
                    LLab.Text = "L";
                    ChangeLeftChannelSize(UseSize);
                    MeterFunc.ChangeMeter(0, left);
                    MeterFunc.ChangeMeter(1, right);
                    MeterFunc.AverageMeter(left, right);
                }

                System.Threading.Thread.Sleep(1);
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
                base.DoubleBuffered = true;
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                UpdateStyles();

                Delegate = this;
                CPUSpeed();
                if (Channels == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Channels");
                    return;
                }

                for (int i = 0; i <= 16; ++i)
                {
                    RegValInt[i] = Convert.ToInt32(Channels.GetValue(RegValName[i], 100));
                    if (RegValInt[i] > 100)
                        RegValInt[i] = 100;
                }

                CH1VOL.Value = RegValInt[0];
                CH2VOL.Value = RegValInt[1];
                CH3VOL.Value = RegValInt[2];
                CH4VOL.Value = RegValInt[3];
                CH5VOL.Value = RegValInt[4];
                CH6VOL.Value = RegValInt[5];
                CH7VOL.Value = RegValInt[6];
                CH8VOL.Value = RegValInt[7];
                CH9VOL.Value = RegValInt[8];
                CH10VOL.Value = RegValInt[9];
                CH11VOL.Value = RegValInt[10];
                CH12VOL.Value = RegValInt[11];
                CH13VOL.Value = RegValInt[12];
                CH14VOL.Value = RegValInt[13];
                CH15VOL.Value = RegValInt[14];
                CH16VOL.Value = RegValInt[15];
                MainVol.Value = RegValInt[16];

                if (Convert.ToInt32(Settings.GetValue("VolumeMonitor")) == 1)
                {
                    VolumeMonitor.Checked = true;
                    MeterFunc.EnableLEDs();
                    VUStatus = true;
                }
                else
                {
                    VolumeMonitor.Checked = false;
                    MeterFunc.DisableLEDs();
                    VUStatus = false;
                }
                ChannelVolume.Enabled = true;
                GarbageCollector.RunWorkerAsync();

                Meter.ContextMenu = PeakMeterMenu;
                VolumeCheck.Interval = Convert.ToInt32((1.0 / Properties.Settings.Default.VolUpdateHz) * 1000.0);

                if (Properties.Settings.Default.CurrentTheme == 0) ClassicTheme.PerformClick();
                else if (Properties.Settings.Default.CurrentTheme == 1) DarkTheme.PerformClick();
                else if (Properties.Settings.Default.CurrentTheme == 2) ItsThe80sTheme.PerformClick();
                else ClassicTheme.PerformClick();
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


        private void VolumeMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                if (VolumeMonitor.Checked != true)
                {
                    if (MaxStockClock < 1100)
                    {
                        DialogResult dialogResult = MessageBox.Show("Enabling a mixer on a computer with poor specs could make the driver stutter.\n\nAre you sure you want to enable it?", "Weak processor detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.Yes)
                        {
                            VolumeMonitor.Checked = true;
                            Settings.SetValue("VolumeMonitor", "1", RegistryValueKind.DWord);
                            MeterFunc.EnableLEDs();
                            VUStatus = true;
                            MeterFunc.ChangeMeter(0, 0);
                            MeterFunc.ChangeMeter(1, 0);
                        }
                    }
                    else if (MaxStockClock >= 1100)
                    {
                        VolumeMonitor.Checked = true;
                        Settings.SetValue("VolumeMonitor", "1", RegistryValueKind.DWord);
                        MeterFunc.EnableLEDs();
                        VUStatus = true;
                        MeterFunc.ChangeMeter(0, 0);
                        MeterFunc.ChangeMeter(1, 0);
                    }
                }
                else
                {
                    VolumeMonitor.Checked = false;
                    Settings.SetValue("VolumeMonitor", "0", RegistryValueKind.DWord);
                    MeterFunc.DisableLEDs();
                    VUStatus = false;
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
                if (Channels == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Channels");
                    Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Channels", true);
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
                Channels.SetValue("cha", MainVol.Value.ToString(), RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not write settings to the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            System.Threading.Thread.Sleep(1);
        }

        private void showTheConfiguratorWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthConfigurator.exe");
        }

        private void VolumeToolTip(string channel, TrackBar trackbar)
        {
            VolumeTip.SetToolTip(trackbar, String.Format("{0}: {1}%", channel, trackbar.Value));
        }

        private void CH16VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 16", CH16VOL);
            RegValInt[15] = CH16VOL.Value;
        }

        private void CH15VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 15", CH15VOL);
            RegValInt[14] = CH15VOL.Value;
        }

        private void CH14VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 14", CH14VOL);
            RegValInt[13] = CH14VOL.Value;
        }

        private void CH13VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 13", CH13VOL);
            RegValInt[12] = CH13VOL.Value;
        }

        private void CH12VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 12", CH12VOL);
            RegValInt[11] = CH12VOL.Value;
        }

        private void CH11VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 11", CH11VOL);
            RegValInt[10] = CH11VOL.Value;
        }

        private void CH10VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 10", CH10VOL);
            RegValInt[9] = CH10VOL.Value;
        }

        private void CH9VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 9", CH9VOL);
            RegValInt[8] = CH9VOL.Value;
        }

        private void CH8VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 8", CH8VOL);
            RegValInt[7] = CH8VOL.Value;
        }

        private void CH7VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 7", CH7VOL);
            RegValInt[6] = CH7VOL.Value;
        }

        private void CH6VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 6", CH6VOL);
            RegValInt[5] = CH6VOL.Value;
        }

        private void CH5VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 5", CH5VOL);
            RegValInt[4] = CH5VOL.Value;
        }

        private void CH4VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 4", CH4VOL);
            RegValInt[3] = CH4VOL.Value;
        }

        private void CH3VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 3", CH3VOL);
            RegValInt[2] = CH3VOL.Value;
        }

        private void CH2VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 2", CH2VOL);
            RegValInt[1] = CH2VOL.Value;
        }

        private void CH1VOL_Scroll(object sender, EventArgs e)
        {
            VolumeToolTip("Channel 1", CH1VOL);
            RegValInt[0] = CH1VOL.Value;
        }

        private void MainVol_Scroll(object sender, EventArgs e)
        {
            CH1VOL.Value = CH2VOL.Value = CH3VOL.Value = CH4VOL.Value = CH5VOL.Value = CH6VOL.Value = CH7VOL.Value = CH8VOL.Value = CH9VOL.Value = CH10VOL.Value = CH11VOL.Value = CH12VOL.Value = CH13VOL.Value = CH14VOL.Value = CH15VOL.Value = CH16VOL.Value = MainVol.Value;
            VolumeToolTip("All", MainVol);
        }

        private void LED_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = LED.ClientRectangle;
            rect.Width--;
            rect.Height--;
            if (VUStatus)
                e.Graphics.DrawRectangle(Pens.White, rect);
            else
                e.Graphics.DrawRectangle(Pens.Gray, rect);
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

        private void GarbageCollector_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.Threading.Thread.Sleep(1);
            }
        }

        private void CheckIfXAudio_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Settings.GetValue("CurrentEngine")) == 0)
                {
                    if (VUStatus != false)
                    {
                        MeterFunc.DisableLEDs();
                        VUStatus = false;
                    }
                }
                else
                {
                    if (Convert.ToInt32(Settings.GetValue("VolumeMonitor")) == 1)
                    {
                        if (VUStatus != true)
                        {
                            MeterFunc.EnableLEDs();
                            VUStatus = true;
                        }
                    }
                    else
                    {
                        if (VUStatus != false)
                        {
                            MeterFunc.DisableLEDs();
                            VUStatus = false;
                        }
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected new void CenterToScreen()
        {
            Screen screen = Screen.FromControl(this);

            Rectangle workingArea = screen.WorkingArea;
            this.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
        }

        private void ClassicTheme_Click(object sender, EventArgs e)
        {
            MeterFunc.ChangeStyle(Color.Black, Color.White, Color.White, Color.FromArgb(16, 16, 16), new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular), new Font("Lucida Console", 8.25f, FontStyle.Regular));
            ClassicTheme.Checked = true;
            DarkTheme.Checked = false;
            ItsThe80sTheme.Checked = false;
            SetWindowTheme(Handle, "EXPLORER", null);
            Properties.Settings.Default.CurrentTheme = 0;
            Properties.Settings.Default.Save();
        }

        private void DarkTheme_Click(object sender, EventArgs e)
        {
            MeterFunc.ChangeStyle(Color.White, Color.Black, Color.White, Color.FromArgb(24, 24, 24), new Font("Arial", 8.25f, FontStyle.Regular), new Font("Arial", 8.25f, FontStyle.Regular));
            ClassicTheme.Checked = false;
            DarkTheme.Checked = true;
            ItsThe80sTheme.Checked = false;
            SetWindowTheme(Handle, "EXPLORER", null);
            Properties.Settings.Default.CurrentTheme = 1;
            Properties.Settings.Default.Save();
        }

        private void ItsThe80sTheme_Click(object sender, EventArgs e)
        {
            MeterFunc.ChangeStyle(Color.White, Color.Purple, Color.DarkRed, Color.BlanchedAlmond, new Font("Comic Sans MS", 8.25f, FontStyle.Regular), new Font("Comic Sans MS", 8.25f, FontStyle.Regular));
            ClassicTheme.Checked = false;
            DarkTheme.Checked = false;
            ItsThe80sTheme.Checked = true;
            Properties.Settings.Default.CurrentTheme = 2;
            Properties.Settings.Default.Save();
        }

        private void OInst_Click(object sender, EventArgs e)
        {
            new OverrideInstruments().ShowDialog();
        }

        private void UpdateFreqSet_Click(object sender, EventArgs e)
        {
            new Forms.UpdateFreq().ShowDialog();
            VolumeCheck.Interval = Convert.ToInt32((1.0 / Properties.Settings.Default.VolUpdateHz) * 1000.0);
        }
    }
}