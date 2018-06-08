using System.Drawing;

namespace OmniMIDIMixerWindow
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
            OmniMIDIMixerWindow.Delegate.BackColor = BackgroundColor;
            OmniMIDIMixerWindow.Delegate.CH1.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH2.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH3.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH4.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH5.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH6.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH7.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH8.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH9.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH10.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH11.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH12.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH13.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH14.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH15.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.CH16.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.AllCh.ForeColor = TextColor;
            OmniMIDIMixerWindow.Delegate.Meter.BackColor = MeterBackgroundColor;
            OmniMIDIMixerWindow.Delegate.LLab.ForeColor = MeterTextColor;
            OmniMIDIMixerWindow.Delegate.RLab.ForeColor = MeterTextColor;
            OmniMIDIMixerWindow.Delegate.SignalLabel.ForeColor = MeterTextColor;
            OmniMIDIMixerWindow.Delegate.VolLevel.ForeColor = MeterTextColor;

            // Fonts
            OmniMIDIMixerWindow.Delegate.CH1.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH2.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH3.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH4.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH5.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH6.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH7.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH8.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH9.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH10.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH11.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH12.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH13.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH14.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH15.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.CH16.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.AllCh.Font = FontFamily;
            OmniMIDIMixerWindow.Delegate.LLab.Font = MeterFontFamily;
            OmniMIDIMixerWindow.Delegate.RLab.Font = MeterFontFamily;
            OmniMIDIMixerWindow.Delegate.SignalLabel.Font = MeterFontFamily;
            OmniMIDIMixerWindow.Delegate.VolLevel.Font = MeterFontFamily;
        }

        public static void SetMaximum(int maximum)
        {
            OmniMIDIMixerWindow.Delegate.CH1VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH2VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH3VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH4VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH5VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH6VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH7VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH8VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH9VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH10VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH11VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH12VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH13VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH14VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH15VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.CH16VOL.Maximum = maximum;
            OmniMIDIMixerWindow.Delegate.MainVol.Maximum = maximum;
        }

        public static void AverageMeter(int left, int right)
        {
            int average = (left + right) / 2;

            // For peak indicator
            if (average == 0)
            {
                OmniMIDIMixerWindow.Delegate.LED.BackColor = Color.Black;
            }
            else if (average > 0 && average <= 32767)
            {
                OmniMIDIMixerWindow.Delegate.LED.BackColor = Color.Lime;
            }
            else if (average > 32767)
            {
                OmniMIDIMixerWindow.Delegate.LED.BackColor = Color.Red;
            }
        }

        public static void DisableLEDs()
        {
            OmniMIDIMixerWindow.Delegate.Meter.Refresh();
            OmniMIDIMixerWindow.Delegate.Meter.Cursor = System.Windows.Forms.Cursors.No;
            OmniMIDIMixerWindow.Delegate.SignalLabel.Enabled = false;
            OmniMIDIMixerWindow.Delegate.LLab.Enabled = false;
            OmniMIDIMixerWindow.Delegate.RLab.Enabled = false;
            OmniMIDIMixerWindow.Delegate.LED.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV1.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV2.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV3.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV4.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV5.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV6.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV7.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV8.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV9.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV10.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV11.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV12.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV13.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV14.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV15.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV16.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV17.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV18.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV19.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV20.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV21.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV22.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV1.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV2.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV3.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV4.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV5.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV6.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV7.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV8.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV9.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV10.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV11.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV12.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV13.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV14.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV15.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV16.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV17.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV18.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV19.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV20.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV21.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.RV22.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.Meter.Refresh();
        }

        public static void EnableLEDs()
        {
            OmniMIDIMixerWindow.Delegate.Meter.Refresh();
            OmniMIDIMixerWindow.Delegate.Meter.Cursor = System.Windows.Forms.Cursors.Default;
            OmniMIDIMixerWindow.Delegate.SignalLabel.Enabled = true;
            OmniMIDIMixerWindow.Delegate.LLab.Enabled = true;
            OmniMIDIMixerWindow.Delegate.RLab.Enabled = true;
            OmniMIDIMixerWindow.Delegate.LED.BackColor = Color.Black;
            OmniMIDIMixerWindow.Delegate.LV1.BackColor = Color.DarkSlateGray;
            OmniMIDIMixerWindow.Delegate.LV2.BackColor = Color.DarkGreen;
            OmniMIDIMixerWindow.Delegate.LV3.BackColor = Color.ForestGreen;
            OmniMIDIMixerWindow.Delegate.LV4.BackColor = Color.Green;
            OmniMIDIMixerWindow.Delegate.LV5.BackColor = Color.LimeGreen;
            OmniMIDIMixerWindow.Delegate.LV6.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.LV7.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.LV8.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.LV9.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.LV10.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.LV11.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.LV12.BackColor = Color.Chartreuse;
            OmniMIDIMixerWindow.Delegate.LV13.BackColor = Color.GreenYellow;
            OmniMIDIMixerWindow.Delegate.LV14.BackColor = Color.Yellow;
            OmniMIDIMixerWindow.Delegate.LV15.BackColor = Color.Yellow;
            OmniMIDIMixerWindow.Delegate.LV16.BackColor = Color.Yellow;
            OmniMIDIMixerWindow.Delegate.LV17.BackColor = Color.Gold;
            OmniMIDIMixerWindow.Delegate.LV18.BackColor = Color.DarkOrange;
            OmniMIDIMixerWindow.Delegate.LV19.BackColor = Color.Red;
            OmniMIDIMixerWindow.Delegate.LV20.BackColor = Color.Red;
            OmniMIDIMixerWindow.Delegate.LV21.BackColor = Color.Crimson;
            OmniMIDIMixerWindow.Delegate.LV22.BackColor = Color.DeepPink;
            OmniMIDIMixerWindow.Delegate.RV1.BackColor = Color.DarkSlateGray;
            OmniMIDIMixerWindow.Delegate.RV2.BackColor = Color.DarkGreen;
            OmniMIDIMixerWindow.Delegate.RV3.BackColor = Color.ForestGreen;
            OmniMIDIMixerWindow.Delegate.RV4.BackColor = Color.Green;
            OmniMIDIMixerWindow.Delegate.RV5.BackColor = Color.LimeGreen;
            OmniMIDIMixerWindow.Delegate.RV6.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.RV7.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.RV8.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.RV9.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.RV10.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.RV11.BackColor = Color.Lime;
            OmniMIDIMixerWindow.Delegate.RV12.BackColor = Color.Chartreuse;
            OmniMIDIMixerWindow.Delegate.RV13.BackColor = Color.GreenYellow;
            OmniMIDIMixerWindow.Delegate.RV14.BackColor = Color.Yellow;
            OmniMIDIMixerWindow.Delegate.RV15.BackColor = Color.Yellow;
            OmniMIDIMixerWindow.Delegate.RV16.BackColor = Color.Yellow;
            OmniMIDIMixerWindow.Delegate.RV17.BackColor = Color.Gold;
            OmniMIDIMixerWindow.Delegate.RV18.BackColor = Color.DarkOrange;
            OmniMIDIMixerWindow.Delegate.RV19.BackColor = Color.Red;
            OmniMIDIMixerWindow.Delegate.RV20.BackColor = Color.Red;
            OmniMIDIMixerWindow.Delegate.RV21.BackColor = Color.Crimson;
            OmniMIDIMixerWindow.Delegate.RV22.BackColor = Color.DeepPink;
            OmniMIDIMixerWindow.Delegate.Meter.Refresh();
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
                OmniMIDIMixerWindow.Delegate.LV1.Visible = values[0];
                OmniMIDIMixerWindow.Delegate.LV2.Visible = values[1];
                OmniMIDIMixerWindow.Delegate.LV3.Visible = values[2];
                OmniMIDIMixerWindow.Delegate.LV4.Visible = values[3];
                OmniMIDIMixerWindow.Delegate.LV5.Visible = values[4];
                OmniMIDIMixerWindow.Delegate.LV6.Visible = values[5];
                OmniMIDIMixerWindow.Delegate.LV7.Visible = values[6];
                OmniMIDIMixerWindow.Delegate.LV8.Visible = values[7];
                OmniMIDIMixerWindow.Delegate.LV9.Visible = values[8];
                OmniMIDIMixerWindow.Delegate.LV10.Visible = values[9];
                OmniMIDIMixerWindow.Delegate.LV11.Visible = values[10];
                OmniMIDIMixerWindow.Delegate.LV12.Visible = values[11];
                OmniMIDIMixerWindow.Delegate.LV13.Visible = values[12];
                OmniMIDIMixerWindow.Delegate.LV14.Visible = values[13];
                OmniMIDIMixerWindow.Delegate.LV15.Visible = values[14];
                OmniMIDIMixerWindow.Delegate.LV16.Visible = values[15];
                OmniMIDIMixerWindow.Delegate.LV17.Visible = values[16];
                OmniMIDIMixerWindow.Delegate.LV18.Visible = values[17];
                OmniMIDIMixerWindow.Delegate.LV19.Visible = values[18];
                OmniMIDIMixerWindow.Delegate.LV20.Visible = values[19];
                OmniMIDIMixerWindow.Delegate.LV21.Visible = values[20];
                OmniMIDIMixerWindow.Delegate.LV22.Visible = values[21];
            }
            else if (channel == 1)
            {
                OmniMIDIMixerWindow.Delegate.RV1.Visible = values[0];
                OmniMIDIMixerWindow.Delegate.RV2.Visible = values[1];
                OmniMIDIMixerWindow.Delegate.RV3.Visible = values[2];
                OmniMIDIMixerWindow.Delegate.RV4.Visible = values[3];
                OmniMIDIMixerWindow.Delegate.RV5.Visible = values[4];
                OmniMIDIMixerWindow.Delegate.RV6.Visible = values[5];
                OmniMIDIMixerWindow.Delegate.RV7.Visible = values[6];
                OmniMIDIMixerWindow.Delegate.RV8.Visible = values[7];
                OmniMIDIMixerWindow.Delegate.RV9.Visible = values[8];
                OmniMIDIMixerWindow.Delegate.RV10.Visible = values[9];
                OmniMIDIMixerWindow.Delegate.RV11.Visible = values[10];
                OmniMIDIMixerWindow.Delegate.RV12.Visible = values[11];
                OmniMIDIMixerWindow.Delegate.RV13.Visible = values[12];
                OmniMIDIMixerWindow.Delegate.RV14.Visible = values[13];
                OmniMIDIMixerWindow.Delegate.RV15.Visible = values[14];
                OmniMIDIMixerWindow.Delegate.RV16.Visible = values[15];
                OmniMIDIMixerWindow.Delegate.RV17.Visible = values[16];
                OmniMIDIMixerWindow.Delegate.RV18.Visible = values[17];
                OmniMIDIMixerWindow.Delegate.RV19.Visible = values[18];
                OmniMIDIMixerWindow.Delegate.RV20.Visible = values[19];
                OmniMIDIMixerWindow.Delegate.RV21.Visible = values[20];
                OmniMIDIMixerWindow.Delegate.RV22.Visible = values[21];
            }
        }
    }
}
