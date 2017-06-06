using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeppySynthConfigurator
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
                KeppySynthConfiguratorMain.Delegate.LV1.Visible = values[0];
                KeppySynthConfiguratorMain.Delegate.LV2.Visible = values[1];
                KeppySynthConfiguratorMain.Delegate.LV3.Visible = values[2];
                KeppySynthConfiguratorMain.Delegate.LV4.Visible = values[3];
                KeppySynthConfiguratorMain.Delegate.LV5.Visible = values[4];
                KeppySynthConfiguratorMain.Delegate.LV6.Visible = values[5];
                KeppySynthConfiguratorMain.Delegate.LV7.Visible = values[6];
                KeppySynthConfiguratorMain.Delegate.LV8.Visible = values[7];
                KeppySynthConfiguratorMain.Delegate.LV9.Visible = values[8];
                KeppySynthConfiguratorMain.Delegate.LV10.Visible = values[9];
                KeppySynthConfiguratorMain.Delegate.LV11.Visible = values[10];
                KeppySynthConfiguratorMain.Delegate.LV12.Visible = values[11];
                KeppySynthConfiguratorMain.Delegate.LV13.Visible = values[12];
                KeppySynthConfiguratorMain.Delegate.LV14.Visible = values[13];
                KeppySynthConfiguratorMain.Delegate.LV15.Visible = values[14];
                KeppySynthConfiguratorMain.Delegate.LV16.Visible = values[15];
                KeppySynthConfiguratorMain.Delegate.LV17.Visible = values[16];
                KeppySynthConfiguratorMain.Delegate.LV18.Visible = values[17];
                KeppySynthConfiguratorMain.Delegate.LV19.Visible = values[18];
                KeppySynthConfiguratorMain.Delegate.LV20.Visible = values[19];
                KeppySynthConfiguratorMain.Delegate.LV21.Visible = values[20];
                KeppySynthConfiguratorMain.Delegate.LV22.Visible = values[21];
            }
            else if (channel == 1)
            {
                KeppySynthConfiguratorMain.Delegate.RV1.Visible = values[0];
                KeppySynthConfiguratorMain.Delegate.RV2.Visible = values[1];
                KeppySynthConfiguratorMain.Delegate.RV3.Visible = values[2];
                KeppySynthConfiguratorMain.Delegate.RV4.Visible = values[3];
                KeppySynthConfiguratorMain.Delegate.RV5.Visible = values[4];
                KeppySynthConfiguratorMain.Delegate.RV6.Visible = values[5];
                KeppySynthConfiguratorMain.Delegate.RV7.Visible = values[6];
                KeppySynthConfiguratorMain.Delegate.RV8.Visible = values[7];
                KeppySynthConfiguratorMain.Delegate.RV9.Visible = values[8];
                KeppySynthConfiguratorMain.Delegate.RV10.Visible = values[9];
                KeppySynthConfiguratorMain.Delegate.RV11.Visible = values[10];
                KeppySynthConfiguratorMain.Delegate.RV12.Visible = values[11];
                KeppySynthConfiguratorMain.Delegate.RV13.Visible = values[12];
                KeppySynthConfiguratorMain.Delegate.RV14.Visible = values[13];
                KeppySynthConfiguratorMain.Delegate.RV15.Visible = values[14];
                KeppySynthConfiguratorMain.Delegate.RV16.Visible = values[15];
                KeppySynthConfiguratorMain.Delegate.RV17.Visible = values[16];
                KeppySynthConfiguratorMain.Delegate.RV18.Visible = values[17];
                KeppySynthConfiguratorMain.Delegate.RV19.Visible = values[18];
                KeppySynthConfiguratorMain.Delegate.RV20.Visible = values[19];
                KeppySynthConfiguratorMain.Delegate.RV21.Visible = values[20];
                KeppySynthConfiguratorMain.Delegate.RV22.Visible = values[21];
            }
        }

        public static void AverageMeter(int left, int right)
        {
            int average = (left + right) / 2;

            // For peak indicator
            if (average == 0)
            {
                KeppySynthConfiguratorMain.Delegate.LED.BackColor = Color.Black;
            }
            else if (average > 0 && average <= 32767)
            {
                KeppySynthConfiguratorMain.Delegate.LED.BackColor = Color.Lime;
            }
            else if (average > 32767)
            {
                KeppySynthConfiguratorMain.Delegate.LED.BackColor = Color.Red;
            }
        }
    }
}
