using System.Numerics;

namespace Art713.ECEC.PrimalityTests.MillerRabinPrimalityTest
{
    public class Program
    {
        static void Main()
        {
            System.Console.WriteLine("Art713.ECEC.PrimalityTests.MillerRabinPrimalityTest");
            System.Console.ReadLine();
        }
    }

    public interface IPrimalityTest
    {
        bool MillerRabinPrimalityTest(BigInteger number);
    }

    public class PrimalityTest : IPrimalityTest
    {
        public bool MillerRabinPrimalityTest(BigInteger number)
        {
            if (number == 2) return true;

            double t = BigInteger.Log(number, 2.0);

            BigInteger m = BigInteger.Subtract(number, 1);

            if (BigInteger.ModPow(2, m, number) != 1) return false;

            int s = 0;
            bool flag = true;

            do
            {
                if (m % 2 == 0)
                {
                    m = BigInteger.Divide(m, 2);
                    s++;
                }
                else
                    flag = false;
            } while (flag);

            BigInteger a = 1;
            for (int j = 0; j < t; j++)
            {
                ++a;
                if (a > number - 1) return false;
                BigInteger b = BigInteger.ModPow(a, m, number);
                if (b != 1)
                {
                    for (int i = 1; i <= s; i++)
                    {
                        if (b == (number - 1))
                        {
                            return true; //prime!
                        }
                        b = BigInteger.ModPow(b, 2, number);
                    }
                    return false; //composite!
                }
            }
            return false;
        }
    }
}