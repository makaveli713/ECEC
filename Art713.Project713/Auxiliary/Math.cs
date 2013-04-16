using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Art713.Project713.Auxiliary
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
            return k2; // k2 for Multiplicative Reverse Number, k3 - Greatest Common Divisor
        }

        public static BigInteger ModularMultiplicativeInverse(BigInteger number, BigInteger modulus)
        {
            return Mod(ExtendedEuclideanAlgorithm(modulus, number), modulus);
        }

        public static BigInteger Mod(BigInteger a, BigInteger b)
        {
            if (a < 0)
                return (b + (a % b)) % b;
            return a % b;
        }


        public static byte[] GetBits(int number)
        {
            var k = (number / 255 == 0) ? 0 : number / 255;
            var temp = string.Empty;
            for (var i = 8 + k; i >= 0; i--)
                temp += ((number >> i) & 1).ToString(CultureInfo.InvariantCulture);
            temp = temp.TrimStart('0');
            var bytes = new byte[temp.Length];
            for (var i = 0; i < temp.Length; i++)
                bytes[i] = Byte.Parse(temp[i].ToString());

            return bytes;
        }

        public static byte[] GetBits(BigInteger number)
        {
            var resultBaseNumberArrayDimension = 2048;
            var resultBaseNumberArray = new byte[resultBaseNumberArrayDimension];

            for (; number > 0; number /= 2)
            {
                var bit = byte.Parse((number % 2).ToString());
                resultBaseNumberArray[--resultBaseNumberArrayDimension] = bit;
            }

            var temp = resultBaseNumberArray.Aggregate(string.Empty, (current, bit) => current + bit.ToString(CultureInfo.InvariantCulture));
            temp = temp.TrimStart('0');

            var bytes = new byte[temp.Length];
            for (var i = 0; i < temp.Length; i++)
                bytes[i] = Byte.Parse(temp[i].ToString());

            return bytes;
        }
    }
}
