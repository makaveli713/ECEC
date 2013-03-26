using NUnit.Framework;

namespace Art713.ECEC.TestOfPrimalityTest
{
    [TestFixture]
    class TestOfPointAddition
    {
        private Point _setup;

        [TestFixtureSetUp]
        public void SetupBeforeEveryTestFunction()
        {
            _setup = new Point();
        }

        [Test]
        public void ShouldReturnPointWhenAddingOtherTwoPoint()
        {
            Point firstPoint = new Point(5,1);
            Point secondPoint = new Point(4,6);
            Assert.That(_setup.PointAddition(firstPoint,secondPoint),Is.EqualTo(new Point(2,5)));
        }
    }
}
