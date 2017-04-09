using System.Drawing;

namespace KeppySynthMixerWindow
{
    class MeterFunc
    {
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

        public static void ChangeStyle(Color TextColor, Color BackgroundColor, Color MeterTextColor, Color MeterBackgroundColor, Font FontFamily, Font MeterFontFamily)
        {
            // Colors
            KeppySynthMixerWindow.Delegate.BackColor = BackgroundColor;
            KeppySynthMixerWindow.Delegate.CH1.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH2.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH3.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH4.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH5.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH6.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH7.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH8.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH9.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH10.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH11.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH12.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH13.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH14.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH15.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.CH16.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.AllCh.ForeColor = TextColor;
            KeppySynthMixerWindow.Delegate.Meter.BackColor = MeterBackgroundColor;
            KeppySynthMixerWindow.Delegate.LLab.ForeColor = MeterTextColor;
            KeppySynthMixerWindow.Delegate.RLab.ForeColor = MeterTextColor;
            KeppySynthMixerWindow.Delegate.SignalLabel.ForeColor = MeterTextColor;

            // Fonts
            KeppySynthMixerWindow.Delegate.CH1.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH2.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH3.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH4.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH5.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH6.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH7.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH8.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH9.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH10.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH11.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH12.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH13.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH14.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH15.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.CH16.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.AllCh.Font = FontFamily;
            KeppySynthMixerWindow.Delegate.LLab.Font = MeterFontFamily;
            KeppySynthMixerWindow.Delegate.RLab.Font = MeterFontFamily;
            KeppySynthMixerWindow.Delegate.SignalLabel.Font = MeterFontFamily;
        }

        public static void SetMaximum(int maximum)
        {
            KeppySynthMixerWindow.Delegate.CH1VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH2VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH3VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH4VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH5VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH6VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH7VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH8VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH9VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH10VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH11VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH12VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH13VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH14VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH15VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.CH16VOL.Maximum = maximum;
            KeppySynthMixerWindow.Delegate.MainVol.Maximum = maximum;
        }

        public static void AverageMeter(int left, int right)
        {
            int average = (left + right) / 2;

            // For peak indicator
            if (average == 0)
            {
                KeppySynthMixerWindow.Delegate.LED.BackColor = Color.Black;
            }
            else if (average > 0 && average <= 32767)
            {
                KeppySynthMixerWindow.Delegate.LED.BackColor = Color.Lime;
            }
            else if (average > 32767)
            {
                KeppySynthMixerWindow.Delegate.LED.BackColor = Color.Red;
            }

        }

        public static void DisableLEDs()
        {
            KeppySynthMixerWindow.Delegate.Meter.Refresh();
            KeppySynthMixerWindow.Delegate.Meter.Cursor = System.Windows.Forms.Cursors.No;
            KeppySynthMixerWindow.Delegate.SignalLabel.Enabled = false;
            KeppySynthMixerWindow.Delegate.LLab.Enabled = false;
            KeppySynthMixerWindow.Delegate.RLab.Enabled = false;
            KeppySynthMixerWindow.Delegate.LED.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV1.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV2.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV3.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV4.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV5.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV6.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV7.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV8.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV9.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV10.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV11.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV12.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV13.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV14.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV15.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV16.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV17.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV18.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV19.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV20.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV21.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV22.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV1.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV2.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV3.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV4.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV5.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV6.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV7.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV8.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV9.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV10.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV11.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV12.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV13.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV14.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV15.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV16.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV17.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV18.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV19.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV20.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV21.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.RV22.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.Meter.Refresh();
        }

        public static void EnableLEDs()
        {
            KeppySynthMixerWindow.Delegate.Meter.Refresh();
            KeppySynthMixerWindow.Delegate.Meter.Cursor = System.Windows.Forms.Cursors.Default;
            KeppySynthMixerWindow.Delegate.SignalLabel.Enabled = true;
            KeppySynthMixerWindow.Delegate.LLab.Enabled = true;
            KeppySynthMixerWindow.Delegate.RLab.Enabled = true;
            KeppySynthMixerWindow.Delegate.LED.BackColor = Color.Black;
            KeppySynthMixerWindow.Delegate.LV1.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV2.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV3.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV4.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV5.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV6.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV7.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV8.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV9.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV10.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV11.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.LV12.BackColor = Color.LightGreen;
            KeppySynthMixerWindow.Delegate.LV13.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.LV14.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.LV15.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.LV16.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.LV17.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.LV18.BackColor = Color.Goldenrod;
            KeppySynthMixerWindow.Delegate.LV19.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.LV20.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.LV21.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.LV22.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.RV1.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV2.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV3.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV4.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV5.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV6.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV7.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV8.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV9.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV10.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV11.BackColor = Color.Lime;
            KeppySynthMixerWindow.Delegate.RV12.BackColor = Color.LightGreen;
            KeppySynthMixerWindow.Delegate.RV13.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.RV14.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.RV15.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.RV16.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.RV17.BackColor = Color.Yellow;
            KeppySynthMixerWindow.Delegate.RV18.BackColor = Color.Goldenrod;
            KeppySynthMixerWindow.Delegate.RV19.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.RV20.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.RV21.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.RV22.BackColor = Color.Red;
            KeppySynthMixerWindow.Delegate.Meter.Refresh();
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
                KeppySynthMixerWindow.Delegate.LV1.Visible = values[0];
                KeppySynthMixerWindow.Delegate.LV2.Visible = values[1];
                KeppySynthMixerWindow.Delegate.LV3.Visible = values[2];
                KeppySynthMixerWindow.Delegate.LV4.Visible = values[3];
                KeppySynthMixerWindow.Delegate.LV5.Visible = values[4];
                KeppySynthMixerWindow.Delegate.LV6.Visible = values[5];
                KeppySynthMixerWindow.Delegate.LV7.Visible = values[6];
                KeppySynthMixerWindow.Delegate.LV8.Visible = values[7];
                KeppySynthMixerWindow.Delegate.LV9.Visible = values[8];
                KeppySynthMixerWindow.Delegate.LV10.Visible = values[9];
                KeppySynthMixerWindow.Delegate.LV11.Visible = values[10];
                KeppySynthMixerWindow.Delegate.LV12.Visible = values[11];
                KeppySynthMixerWindow.Delegate.LV13.Visible = values[12];
                KeppySynthMixerWindow.Delegate.LV14.Visible = values[13];
                KeppySynthMixerWindow.Delegate.LV15.Visible = values[14];
                KeppySynthMixerWindow.Delegate.LV16.Visible = values[15];
                KeppySynthMixerWindow.Delegate.LV17.Visible = values[16];
                KeppySynthMixerWindow.Delegate.LV18.Visible = values[17];
                KeppySynthMixerWindow.Delegate.LV19.Visible = values[18];
                KeppySynthMixerWindow.Delegate.LV20.Visible = values[19];
                KeppySynthMixerWindow.Delegate.LV21.Visible = values[20];
                KeppySynthMixerWindow.Delegate.LV22.Visible = values[21];
            }
            else if (channel == 1)
            {
                KeppySynthMixerWindow.Delegate.RV1.Visible = values[0];
                KeppySynthMixerWindow.Delegate.RV2.Visible = values[1];
                KeppySynthMixerWindow.Delegate.RV3.Visible = values[2];
                KeppySynthMixerWindow.Delegate.RV4.Visible = values[3];
                KeppySynthMixerWindow.Delegate.RV5.Visible = values[4];
                KeppySynthMixerWindow.Delegate.RV6.Visible = values[5];
                KeppySynthMixerWindow.Delegate.RV7.Visible = values[6];
                KeppySynthMixerWindow.Delegate.RV8.Visible = values[7];
                KeppySynthMixerWindow.Delegate.RV9.Visible = values[8];
                KeppySynthMixerWindow.Delegate.RV10.Visible = values[9];
                KeppySynthMixerWindow.Delegate.RV11.Visible = values[10];
                KeppySynthMixerWindow.Delegate.RV12.Visible = values[11];
                KeppySynthMixerWindow.Delegate.RV13.Visible = values[12];
                KeppySynthMixerWindow.Delegate.RV14.Visible = values[13];
                KeppySynthMixerWindow.Delegate.RV15.Visible = values[14];
                KeppySynthMixerWindow.Delegate.RV16.Visible = values[15];
                KeppySynthMixerWindow.Delegate.RV17.Visible = values[16];
                KeppySynthMixerWindow.Delegate.RV18.Visible = values[17];
                KeppySynthMixerWindow.Delegate.RV19.Visible = values[18];
                KeppySynthMixerWindow.Delegate.RV20.Visible = values[19];
                KeppySynthMixerWindow.Delegate.RV21.Visible = values[20];
                KeppySynthMixerWindow.Delegate.RV22.Visible = values[21];
            }
        }
    }
}
