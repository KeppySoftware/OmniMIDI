using System.Drawing;

namespace KeppySynthMixerWindow
{
    class MeterFunc
    {
        public static void ChangeMeter(int channel, int volume)
        {
            // For peak indicator
            if (volume == 0)
            {
                KeppySynthMixerWindow.Delegate.LED.BackColor = Color.Black;
            }
            else if (volume > 0 && volume <= 32767)
            {
                KeppySynthMixerWindow.Delegate.LED.BackColor = Color.LightGreen;
            }
            else if (volume > 32767)
            {
                KeppySynthMixerWindow.Delegate.LED.BackColor = Color.OrangeRed;
            }

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
