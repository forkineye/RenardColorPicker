using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenardColorPicker_WPF
{
    class Renard
    {
        public static byte PAD = 0x7D;
        public static byte SYNC = 0x7E;
        public static byte ESCAPE = 0x7F;
        public static byte ADDR = 0x80;
        public static byte ESC_7D = 0x2F;
        public static byte ESC_7E = 0x30;
        public static byte ESC_7F = 0x31;

        public static byte[] PacketBuilder(byte R, byte G, byte B, UInt32 channels, String template) {
            byte[] packet;
            UInt32 pad, size;

            if (template == "RGBW")
                pad = 1;
            else
                pad = 0;

            size = (3+pad)*channels+2;
            packet = new byte[size];

            packet[0] = SYNC;
            packet[1] = ADDR;

            int i = 2;
            while (i < size) {
                packet[i++] = R;
                packet[i++] = G;
                packet[i++] = B;
                if (pad == 1)
                    packet[i++] = 0;
            }

            return packet;
        }
    }
}
