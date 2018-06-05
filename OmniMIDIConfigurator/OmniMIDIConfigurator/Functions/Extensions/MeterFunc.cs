using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    class MeterFunc
    {
        public static bool CheckIfDedicatedMixerIsRunning(bool isitstartup)
        {
            bool ok;
            EventWaitHandle m = new EventWaitHandle(false, EventResetMode.ManualReset, "OmniMIDIMixerWindow", out ok);
            if (!ok)
            {
                if (!isitstartup) MessageBox.Show("The dedicated mixer applet is already running!", "OmniMIDI - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return !ok;
        }

        public static void LoadChannelValues()
        {
            try
            {
                if (OmniMIDIConfiguratorMain.Channels == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Channels");
                    return;
                }

                if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("VolumeBoost")) == 1)
                {
                    MeterFunc.SetMaximum(200);
                    for (int i = 0; i <= 16; ++i)
                    {
                        OmniMIDIConfiguratorMain.RegValInt[i] = Convert.ToInt32(OmniMIDIConfiguratorMain.Channels.GetValue(OmniMIDIConfiguratorMain.RegValName[i], 100));
                        if (OmniMIDIConfiguratorMain.RegValInt[i] > 200)
                            OmniMIDIConfiguratorMain.RegValInt[i] = 200;
                    }
                }
                else
                {
                    for (int i = 0; i <= 16; ++i)
                    {
                        OmniMIDIConfiguratorMain.RegValInt[i] = Convert.ToInt32(OmniMIDIConfiguratorMain.Channels.GetValue(OmniMIDIConfiguratorMain.RegValName[i], 100));
                        if (OmniMIDIConfiguratorMain.RegValInt[i] > 100)
                            OmniMIDIConfiguratorMain.RegValInt[i] = 100;
                    }
                }

                OmniMIDIConfiguratorMain.Delegate.CH1VOL.Value = OmniMIDIConfiguratorMain.RegValInt[0];
                OmniMIDIConfiguratorMain.Delegate.CH2VOL.Value = OmniMIDIConfiguratorMain.RegValInt[1];
                OmniMIDIConfiguratorMain.Delegate.CH3VOL.Value = OmniMIDIConfiguratorMain.RegValInt[2];
                OmniMIDIConfiguratorMain.Delegate.CH4VOL.Value = OmniMIDIConfiguratorMain.RegValInt[3];
                OmniMIDIConfiguratorMain.Delegate.CH5VOL.Value = OmniMIDIConfiguratorMain.RegValInt[4];
                OmniMIDIConfiguratorMain.Delegate.CH6VOL.Value = OmniMIDIConfiguratorMain.RegValInt[5];
                OmniMIDIConfiguratorMain.Delegate.CH7VOL.Value = OmniMIDIConfiguratorMain.RegValInt[6];
                OmniMIDIConfiguratorMain.Delegate.CH8VOL.Value = OmniMIDIConfiguratorMain.RegValInt[7];
                OmniMIDIConfiguratorMain.Delegate.CH9VOL.Value = OmniMIDIConfiguratorMain.RegValInt[8];
                OmniMIDIConfiguratorMain.Delegate.CH10VOL.Value = OmniMIDIConfiguratorMain.RegValInt[9];
                OmniMIDIConfiguratorMain.Delegate.CH11VOL.Value = OmniMIDIConfiguratorMain.RegValInt[10];
                OmniMIDIConfiguratorMain.Delegate.CH12VOL.Value = OmniMIDIConfiguratorMain.RegValInt[11];
                OmniMIDIConfiguratorMain.Delegate.CH13VOL.Value = OmniMIDIConfiguratorMain.RegValInt[12];
                OmniMIDIConfiguratorMain.Delegate.CH14VOL.Value = OmniMIDIConfiguratorMain.RegValInt[13];
                OmniMIDIConfiguratorMain.Delegate.CH15VOL.Value = OmniMIDIConfiguratorMain.RegValInt[14];
                OmniMIDIConfiguratorMain.Delegate.CH16VOL.Value = OmniMIDIConfiguratorMain.RegValInt[15];
                OmniMIDIConfiguratorMain.Delegate.MainVol.Value = OmniMIDIConfiguratorMain.RegValInt[16];

                OmniMIDIConfiguratorMain.Delegate.ChannelVolume.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not read settings from the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public static void ChangeMeter(int channel, int volume)
        {
            // For normal meter
            if (volume == 0)
            {
                TurnOnLEDs(channel, 0);
            }
            else if (volume > 0 && volume <= 1489)
            {
                TurnOnLEDs(channel, 1);
            }
            else if (volume >= 1490 && volume <= 2979)
            {
                TurnOnLEDs(channel, 2);
            }
            else if (volume >= 2980 && volume <= 4469)
            {
                TurnOnLEDs(channel, 3);
            }
            else if (volume >= 4470 && volume <= 5959)
            {
                TurnOnLEDs(channel, 4);
            }
            else if (volume >= 5960 && volume <= 7449)
            {
                TurnOnLEDs(channel, 5);
            }
            else if (volume >= 7450 && volume <= 8939)
            {
                TurnOnLEDs(channel, 6);
            }
            else if (volume >= 8940 && volume <= 10429)
            {
                TurnOnLEDs(channel, 7);
            }
            else if (volume >= 10430 && volume <= 11919)
            {
                TurnOnLEDs(channel, 8);
            }
            else if (volume >= 11920 && volume <= 13409)
            {
                TurnOnLEDs(channel, 9);
            }
            else if (volume >= 13410 && volume <= 14899)
            {
                TurnOnLEDs(channel, 10);
            }
            else if (volume >= 14900 && volume <= 16389)
            {
                TurnOnLEDs(channel, 11);
            }
            else if (volume >= 16390 && volume <= 17879)
            {
                TurnOnLEDs(channel, 12);
            }
            else if (volume >= 17880 && volume <= 19369)
            {
                TurnOnLEDs(channel, 13);
            }
            else if (volume >= 19370 && volume <= 20859)
            {
                TurnOnLEDs(channel, 14);
            }
            else if (volume >= 20860 && volume <= 22349)
            {
                TurnOnLEDs(channel, 15);
            }
            else if (volume >= 22350 && volume <= 23839)
            {
                TurnOnLEDs(channel, 16);
            }
            else if (volume >= 23840 && volume <= 25329)
            {
                TurnOnLEDs(channel, 17);
            }
            else if (volume >= 25530 && volume <= 26819)
            {
                TurnOnLEDs(channel, 18);
            }
            else if (volume >= 26820 && volume <= 28309)
            {
                TurnOnLEDs(channel, 19);
            }
            else if (volume >= 28310 && volume <= 29799)
            {
                TurnOnLEDs(channel, 20);
            }
            else if (volume >= 29800 && volume <= 31289)
            {
                TurnOnLEDs(channel, 21);
            }
            else if (volume >= 31290)
            {
                TurnOnLEDs(channel, 22);
            }
        }

        public static void TurnOnLEDs(int channel, int number)
        {
            bool[] values = new bool[22];
            for (int i = 1; i <= 22; i++)
            {
                if (i < number || i == number)
                {
                    values[i - 1] = true;
                }
            }
            if (channel == 0)
            {
                if (Properties.Settings.Default.ShowMixerUnder == true)
                {
                    OmniMIDIConfiguratorMain.Delegate.LV1.Visible = values[0];
                    OmniMIDIConfiguratorMain.Delegate.LV2.Visible = values[1];
                    OmniMIDIConfiguratorMain.Delegate.LV3.Visible = values[2];
                    OmniMIDIConfiguratorMain.Delegate.LV4.Visible = values[3];
                    OmniMIDIConfiguratorMain.Delegate.LV5.Visible = values[4];
                    OmniMIDIConfiguratorMain.Delegate.LV6.Visible = values[5];
                    OmniMIDIConfiguratorMain.Delegate.LV7.Visible = values[6];
                    OmniMIDIConfiguratorMain.Delegate.LV8.Visible = values[7];
                    OmniMIDIConfiguratorMain.Delegate.LV9.Visible = values[8];
                    OmniMIDIConfiguratorMain.Delegate.LV10.Visible = values[9];
                    OmniMIDIConfiguratorMain.Delegate.LV11.Visible = values[10];
                    OmniMIDIConfiguratorMain.Delegate.LV12.Visible = values[11];
                    OmniMIDIConfiguratorMain.Delegate.LV13.Visible = values[12];
                    OmniMIDIConfiguratorMain.Delegate.LV14.Visible = values[13];
                    OmniMIDIConfiguratorMain.Delegate.LV15.Visible = values[14];
                    OmniMIDIConfiguratorMain.Delegate.LV16.Visible = values[15];
                    OmniMIDIConfiguratorMain.Delegate.LV17.Visible = values[16];
                    OmniMIDIConfiguratorMain.Delegate.LV18.Visible = values[17];
                    OmniMIDIConfiguratorMain.Delegate.LV19.Visible = values[18];
                    OmniMIDIConfiguratorMain.Delegate.LV20.Visible = values[19];
                    OmniMIDIConfiguratorMain.Delegate.LV21.Visible = values[20];
                    OmniMIDIConfiguratorMain.Delegate.LV22.Visible = values[21];
                }
                else
                {
                    OmniMIDIConfiguratorMain.Delegate.LV1S.Visible = values[0];
                    OmniMIDIConfiguratorMain.Delegate.LV2S.Visible = values[1];
                    OmniMIDIConfiguratorMain.Delegate.LV3S.Visible = values[2];
                    OmniMIDIConfiguratorMain.Delegate.LV4S.Visible = values[3];
                    OmniMIDIConfiguratorMain.Delegate.LV5S.Visible = values[4];
                    OmniMIDIConfiguratorMain.Delegate.LV6S.Visible = values[5];
                    OmniMIDIConfiguratorMain.Delegate.LV7S.Visible = values[6];
                    OmniMIDIConfiguratorMain.Delegate.LV8S.Visible = values[7];
                    OmniMIDIConfiguratorMain.Delegate.LV9S.Visible = values[8];
                    OmniMIDIConfiguratorMain.Delegate.LV10S.Visible = values[9];
                    OmniMIDIConfiguratorMain.Delegate.LV11S.Visible = values[10];
                    OmniMIDIConfiguratorMain.Delegate.LV12S.Visible = values[11];
                    OmniMIDIConfiguratorMain.Delegate.LV13S.Visible = values[12];
                    OmniMIDIConfiguratorMain.Delegate.LV14S.Visible = values[13];
                    OmniMIDIConfiguratorMain.Delegate.LV15S.Visible = values[14];
                    OmniMIDIConfiguratorMain.Delegate.LV16S.Visible = values[15];
                    OmniMIDIConfiguratorMain.Delegate.LV17S.Visible = values[16];
                    OmniMIDIConfiguratorMain.Delegate.LV18S.Visible = values[17];
                    OmniMIDIConfiguratorMain.Delegate.LV19S.Visible = values[18];
                    OmniMIDIConfiguratorMain.Delegate.LV20S.Visible = values[19];
                    OmniMIDIConfiguratorMain.Delegate.LV21S.Visible = values[20];
                    OmniMIDIConfiguratorMain.Delegate.LV22S.Visible = values[21];
                }
            }
            else if (channel == 1)
            {
                if (Properties.Settings.Default.ShowMixerUnder == true)
                {
                    OmniMIDIConfiguratorMain.Delegate.RV1.Visible = values[0];
                    OmniMIDIConfiguratorMain.Delegate.RV2.Visible = values[1];
                    OmniMIDIConfiguratorMain.Delegate.RV3.Visible = values[2];
                    OmniMIDIConfiguratorMain.Delegate.RV4.Visible = values[3];
                    OmniMIDIConfiguratorMain.Delegate.RV5.Visible = values[4];
                    OmniMIDIConfiguratorMain.Delegate.RV6.Visible = values[5];
                    OmniMIDIConfiguratorMain.Delegate.RV7.Visible = values[6];
                    OmniMIDIConfiguratorMain.Delegate.RV8.Visible = values[7];
                    OmniMIDIConfiguratorMain.Delegate.RV9.Visible = values[8];
                    OmniMIDIConfiguratorMain.Delegate.RV10.Visible = values[9];
                    OmniMIDIConfiguratorMain.Delegate.RV11.Visible = values[10];
                    OmniMIDIConfiguratorMain.Delegate.RV12.Visible = values[11];
                    OmniMIDIConfiguratorMain.Delegate.RV13.Visible = values[12];
                    OmniMIDIConfiguratorMain.Delegate.RV14.Visible = values[13];
                    OmniMIDIConfiguratorMain.Delegate.RV15.Visible = values[14];
                    OmniMIDIConfiguratorMain.Delegate.RV16.Visible = values[15];
                    OmniMIDIConfiguratorMain.Delegate.RV17.Visible = values[16];
                    OmniMIDIConfiguratorMain.Delegate.RV18.Visible = values[17];
                    OmniMIDIConfiguratorMain.Delegate.RV19.Visible = values[18];
                    OmniMIDIConfiguratorMain.Delegate.RV20.Visible = values[19];
                    OmniMIDIConfiguratorMain.Delegate.RV21.Visible = values[20];
                    OmniMIDIConfiguratorMain.Delegate.RV22.Visible = values[21];
                }
                else
                {
                    OmniMIDIConfiguratorMain.Delegate.RV1S.Visible = values[0];
                    OmniMIDIConfiguratorMain.Delegate.RV2S.Visible = values[1];
                    OmniMIDIConfiguratorMain.Delegate.RV3S.Visible = values[2];
                    OmniMIDIConfiguratorMain.Delegate.RV4S.Visible = values[3];
                    OmniMIDIConfiguratorMain.Delegate.RV5S.Visible = values[4];
                    OmniMIDIConfiguratorMain.Delegate.RV6S.Visible = values[5];
                    OmniMIDIConfiguratorMain.Delegate.RV7S.Visible = values[6];
                    OmniMIDIConfiguratorMain.Delegate.RV8S.Visible = values[7];
                    OmniMIDIConfiguratorMain.Delegate.RV9S.Visible = values[8];
                    OmniMIDIConfiguratorMain.Delegate.RV10S.Visible = values[9];
                    OmniMIDIConfiguratorMain.Delegate.RV11S.Visible = values[10];
                    OmniMIDIConfiguratorMain.Delegate.RV12S.Visible = values[11];
                    OmniMIDIConfiguratorMain.Delegate.RV13S.Visible = values[12];
                    OmniMIDIConfiguratorMain.Delegate.RV14S.Visible = values[13];
                    OmniMIDIConfiguratorMain.Delegate.RV15S.Visible = values[14];
                    OmniMIDIConfiguratorMain.Delegate.RV16S.Visible = values[15];
                    OmniMIDIConfiguratorMain.Delegate.RV17S.Visible = values[16];
                    OmniMIDIConfiguratorMain.Delegate.RV18S.Visible = values[17];
                    OmniMIDIConfiguratorMain.Delegate.RV19S.Visible = values[18];
                    OmniMIDIConfiguratorMain.Delegate.RV20S.Visible = values[19];
                    OmniMIDIConfiguratorMain.Delegate.RV21S.Visible = values[20];
                    OmniMIDIConfiguratorMain.Delegate.RV22S.Visible = values[21];
                }
            }
        }

        public static void AverageMeter(int left, int right)
        {
            int average = (left + right) / 2;

            // For peak indicator
            if (average == 0)
            {
                if (Properties.Settings.Default.ShowMixerUnder == true) OmniMIDIConfiguratorMain.Delegate.LED.BackColor = Color.Black;
                else OmniMIDIConfiguratorMain.Delegate.LEDS.BackColor = Color.Black;
            }
            else if (average > 0 && average <= 32767)
            {
                if (Properties.Settings.Default.ShowMixerUnder == true) OmniMIDIConfiguratorMain.Delegate.LED.BackColor = Color.Lime;
                else OmniMIDIConfiguratorMain.Delegate.LEDS.BackColor = Color.Lime;
            }
            else if (average > 32767)
            {
                if (Properties.Settings.Default.ShowMixerUnder == true) OmniMIDIConfiguratorMain.Delegate.LED.BackColor = Color.Red;
                else OmniMIDIConfiguratorMain.Delegate.LEDS.BackColor = Color.Red;
            }
        }

        public static void SetMaximum(int maximum)
        {
            OmniMIDIConfiguratorMain.Delegate.CH1VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH2VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH3VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH4VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH5VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH6VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH7VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH8VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH9VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH10VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH11VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH12VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH13VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH14VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH15VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.CH16VOL.Maximum = maximum;
            OmniMIDIConfiguratorMain.Delegate.MainVol.Maximum = maximum;
        }
    }
}
