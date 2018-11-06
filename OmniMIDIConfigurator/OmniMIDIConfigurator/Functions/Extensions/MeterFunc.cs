using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniMIDIConfigurator
{
    class MeterFunc
    {
        public static void ChangeMeter(int channel, int volume)
        {
            TurnOnLEDs(channel, (int)(22 + ((volume - 32768) * (double)(0 - 22) / (0 - 32768))));
        }

        public static void TurnOnLEDs(int channel, int number)
        {
            bool[] values = new bool[22];

            for (int i = 1; i <= 22; i++)
                if (i < number || i == number)
                    values[i - 1] = true;

            switch (channel)
            {
                case 0:
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
                    break;
                case 1:
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
                    break;
            }
        }

        public static void AverageMeter(int left, int right)
        {
            int average = (left + right) / 2;

            // For peak indicator
            if (average == 0)
                OmniMIDIConfiguratorMain.Delegate.LEDS.BackColor = Color.Black;
            else if (average > 0 && average <= 32767)
                OmniMIDIConfiguratorMain.Delegate.LEDS.BackColor = Color.Lime;
            else if (average > 32767)
                OmniMIDIConfiguratorMain.Delegate.LEDS.BackColor = Color.Red;
        }
    }
}
