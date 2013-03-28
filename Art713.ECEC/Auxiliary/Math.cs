using System;
using System.Globalization;
using System.Numerics;

namespace Art713.ECEC.Auxiliary
{
    public static class Math
    {
        public static BigInteger ExtendedEuclideanAlgorithm(BigInteger firstNumber, BigInteger secondNumber)
        {
            BigInteger k1, k2, k3, j1, j2, j3, i1, i2, i3, q;
            k1 = 1; k2 = 0; k3 = firstNumber;
            j1 = 0; j2 = 1; j3 = secondNumber;

            while (j3 != 0)
            {
                q = k3 / j3;
                i1 = k1 - q * j1; i2 = k2 - q * j2; i3 = k3 - q * j3;
                k1 = j1; k2 = j2; k3 = j3;
                j1 = i1; j2 = i2; j3 = i3;
            }
            // k3 - Greatest Common Divisor
            return k2; // for Multiplicative Reverse Number
        }

        public static BigInteger ModularMultiplicativeInverse(BigInteger number, BigInteger modulus)
        {
            return Mod((int)ExtendedEuclideanAlgorithm(modulus, number),(int)modulus);
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
            var bytes = new byte[8+k];
            for (var i = 0; i < 8+k; ++i)
                bytes[i] = Byte.Parse(((number >> i) & 1).ToString(CultureInfo.InvariantCulture));
            return bytes;
        }        
    }
}
