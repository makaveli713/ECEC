using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art713.ECEC.Auxiliary
{
    static class Math
    {
        public static int Mod(int a, int b)
        {
            if (a < 0)
                return b + (a % b);
            return a % b;
        }
        
        public static byte[] GetBits(int number)
        {
            //var k = (number/255 == 0) ? 1 : (number%255 == 0) ? number/255:number/255 + 1;
            var k = (number/255 == 0) ? 0 : number/255; //(number % 255 == 0) ? number / 255 : number / 255 + 1;
            var bytes = new byte[8+k];
            for (var i = 0; i < 8+k; ++i)
                bytes[i] = Byte.Parse(((number >> i) & 1).ToString(CultureInfo.InvariantCulture));
            return bytes;
        }
        
    }
}
