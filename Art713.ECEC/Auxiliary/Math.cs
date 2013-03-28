using System;
using System.Globalization;
using System.Numerics;

namespace Art713.ECEC.Auxiliary
{
    public static class Math
    {
        public static int ExtendedEuclideanAlgorithm(int firstNumber, int secondNumber)
        {
            int k1, k2, k3, j1, j2, j3, i1, i2, i3, q;
            k1 = 1; k2 = 0; k3 = firstNumber;
            j1 = 0; j2 = 1; j3 = secondNumber;

            while (j3 != 0)
            {
                q = k3 / j3;
                i1 = k1 - q * j1; i2 = k2 - q * j2; i3 = k3 - q * j3;
                k1 = j1; k2 = j2; k3 = j3;
                j1 = i1; j2 = i2; j3 = i3;
            }            
            return k2; // k2 for Multiplicative Reverse Number, k3 - Greatest Common Divisor
        }

        public static int ModularMultiplicativeInverse(int number, int modulus)
        {
            return Mod(ExtendedEuclideanAlgorithm(modulus, number),modulus);
        }

        public static int Mod(int a, int b)
        {
            if (a < 0)
                //return b + (a % b);
                return (b + (a % b)) % b;
            return a % b;
        }
        
        public static byte[] GetBits(int number)
        {
            var k = (number/255 == 0) ? 0 : number/255; 
            var temp = string.Empty;
            for (var i = 8 + k; i >= 0; i--)
                temp += ((number >> i) & 1).ToString(CultureInfo.InvariantCulture);
            temp = temp.TrimStart('0');
            var bytes = new byte[temp.Length];
            for (var i = 0; i < temp.Length; i++)            
                bytes[i] = Byte.Parse(temp[i].ToString(CultureInfo.InvariantCulture));            
            
            return bytes;
        }        
    }
}
