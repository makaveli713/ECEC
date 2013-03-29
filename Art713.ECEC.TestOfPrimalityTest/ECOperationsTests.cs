using System.Collections.Generic;
using System.Numerics;
using Art713.ECEC.Entities;
using NUnit.Framework;

namespace Art713.ECEC.TestOfPrimalityTest
{
    [TestFixture]
    class ECOperationsTests
    {
        private EllipticCurve _setup;
        private Dictionary<int, Point> _points = new Dictionary<int, Point>();
        private Dictionary<BigInteger, byte[]> _big = new Dictionary<BigInteger, byte[]>();
        
        [SetUp]
        public void SetupBeforeEveryTestFunction()
        {
            _setup = new EllipticCurve(2, 6, 7);
        }

        [Test]
        public void ShouldReturnPointWhenAddingOtherTwoPoint()
        {
            var firstPoint = new Point(5, 1);
            var secondPoint = new Point(4, 6);
            var sumPoint = new Point(2, 5);
            Assert.That(_setup.PointAddition(firstPoint, secondPoint).Abscissa, Is.EqualTo(sumPoint.Abscissa));
            Assert.That(_setup.PointAddition(firstPoint, secondPoint).Ordinate, Is.EqualTo(sumPoint.Ordinate));
        }

        [Test]
        public void ShouldReturnPointWhenDoublingPoint()
        {
            var firstPoint = new Point(5, 1);
            var doublePoint = new Point(4, 6); 
            Assert.That(_setup.PointDoubling(firstPoint).Abscissa, Is.EqualTo(doublePoint.Abscissa));
            Assert.That(_setup.PointDoubling(firstPoint).Ordinate, Is.EqualTo(doublePoint.Ordinate));
        }

        public Dictionary<int,Point> Points
        {
            get
            {
                _points.Add(2, new Point(6, 3));
                _points.Add(3, new Point(10, 6));
                _points.Add(4, new Point(3, 1));
                _points.Add(5, new Point(9, 16));
                _points.Add(6, new Point(16, 13));
                _points.Add(7, new Point(0, 6));
                _points.Add(8, new Point(13, 7));
                _points.Add(9, new Point(7, 6));
                _points.Add(10, new Point(7, 11));
                return _points;
            }
        }

        [Test,
        TestCaseSource("Points")]
        public void ShouldReturnAnotherPointWhenMultiplyPoints(KeyValuePair<int,Point> keyValuePair )
        {
            _setup = new EllipticCurve(2,2,17);
            var point = new Point(5,1);
            var newPoint = _setup.PointMultiplication(point, keyValuePair.Key);
            Assert.That(newPoint.Abscissa,Is.EqualTo(keyValuePair.Value.Abscissa));
            Assert.That(newPoint.Ordinate, Is.EqualTo(keyValuePair.Value.Ordinate));           
        }

        [TestCase(7, Result = new byte[] { 1, 1, 1 })]
        [TestCase(10, Result = new byte[] { 1, 0, 1, 0 })]
        [TestCase(129, Result = new byte[] { 1, 0, 0, 0, 0, 0, 0, 1 })]
        [TestCase(256, Result = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(514, Result = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 })]
        public byte[] ShouldReturnBitsOfGivenNumber(int number)
        {
            return Auxiliary.Math.GetBits(number);
        }

        [Test,
        TestCaseSource("BigIntegerNumbers")]
        public void ShouldReturnBitsOfGivenBigIntegerNumber(KeyValuePair<BigInteger,byte[]> keyValuePair)
        {
            Assert.That(Auxiliary.Math.GetBits(keyValuePair.Key),Is.EqualTo(keyValuePair.Value));
        }

        public Dictionary<BigInteger,byte[]> BigIntegerNumbers
        {
            get
            {
                _big.Add(7, new byte[] { 1, 1, 1 });
                _big.Add(10, new byte[] { 1, 0, 1, 0 });
                _big.Add(129, new byte[] { 1, 0, 0, 0, 0, 0, 0, 1 });
                _big.Add(256, new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0 });
                _big.Add(514, new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 });
                return _big;
            }
        }

    }
}
