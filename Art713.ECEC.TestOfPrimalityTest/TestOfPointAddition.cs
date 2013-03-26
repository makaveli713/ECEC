using Art713.ECEC.Entities;
using NUnit.Framework;

namespace Art713.ECEC.TestOfPrimalityTest
{
    [TestFixture]
    class TestOfPointAddition
    {
        private EllipticCurve _setup;

        [TestFixtureSetUp]
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
    }
}
