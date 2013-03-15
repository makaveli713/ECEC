using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using Art713.ECEC.PrimalityTests.MillerRabinPrimalityTest;
using NUnit.Framework;

namespace Art713.ECEC.TestOfPrimalityTest
{
    class Program
    {
         static void Main()
         {
         }
    }

    [TestFixture]
    class TestOfPrimalityTest
    {
        private PrimalityTest _setup;
        public static string[] PrimeNumberStrings { get; set; }
        private static readonly Dictionary<BigInteger, bool> TestDataField = new Dictionary<BigInteger, bool>();

        public static Dictionary<BigInteger, bool> TestData
        {
            get
            {
                try
                {
                    const string path = @"C:\Users\Art\Documents\Visual Studio 2012\Projects\Repository\Art713.ECEC\prime numbers.txt";
                    using (var sr = new StreamReader(path))
                    {
                        var allnumbers = sr.ReadToEnd();
                        PrimeNumberStrings = allnumbers.Split(' ');
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

                foreach (var primenum in PrimeNumberStrings.Where(primenum => TestDataField != null).Where(primenum => TestDataField != null))
                {
                    TestDataField.Add(BigInteger.Parse(primenum), true);
                    TestDataField.Add((BigInteger.Parse(primenum)) * 2, false);
                }
                return TestDataField;
            }
        }

        [TestFixtureSetUp]
        public void SetUpBeforeEveryTestFunction()
        {
            _setup = new PrimalityTest();
        }

        [Test,
        TestCaseSource("TestData")]
        public void ShouldReturnTrueIfNumberIsPrime(KeyValuePair<BigInteger, bool> keyValuePair)
        {
            Assert.That(_setup.MillerRabinPrimalityTest(keyValuePair.Key), Is.EqualTo(keyValuePair.Value));
        }
    }
}