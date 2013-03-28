using System;
using System.Collections.Generic;

namespace Art713.ECEC.Entities
{
    /// <summary>
    /// class EllipticCurve represent a elliptic curve
    /// </summary>
    public class EllipticCurve
    {
        /// <summary>
        /// generator point
        /// </summary>
        public Point Generator
        {
            get
            {
                return new Point(0, 1);
            }
        }

        // fields:
        private List<Point> _points;

        // properties:

        /// <summary>
        /// "A" is a first parameter of the elliptic curve
        /// </summary>
        public int A { get; set; }
        /// <summary>
        /// "B" is a second parameter of the elliptic curve
        /// </summary>
        public int B { get; set; }
        /// <summary>
        /// "P" is a modulus, third parameter of the elliptic curve
        /// </summary>
        public int P { get; set; }

        /// <summary>
        /// "Points" is a list objects of class Point
        /// </summary>
        public List<Point> Points
        {
            get
            {
                _points.Add(new Point(0, 1));
                _points.Add(new Point(0, 10));

                _points.Add(new Point(1, 5));
                _points.Add(new Point(1, 6));

                _points.Add(new Point(2, 0));

                _points.Add(new Point(3, 3));
                _points.Add(new Point(3, 8));

                _points.Add(new Point(4, 5));
                _points.Add(new Point(4, 6));

                _points.Add(new Point(6, 5));
                _points.Add(new Point(6, 6));

                _points.Add(new Point(8, 2));
                _points.Add(new Point(8, 9));

                _points.Add(new Point(0, 0));

                return _points;
            }
        }

        // methods:

        public void FillPoint()
        {
            throw new System.NotImplementedException();
        }

        public Point PointAddition(Point fPoint, Point sPoint)
        {
            var sumPoint = new Point();

            var k = Auxiliary.Math.Mod((sPoint.Ordinate - fPoint.Ordinate), P) * Auxiliary.Math.ModularMultiplicativeInverse(Auxiliary.Math.Mod((sPoint.Abscissa - fPoint.Abscissa), P), P);
            k = Auxiliary.Math.Mod(k, P);

            sumPoint.Abscissa = (k * k - fPoint.Abscissa - sPoint.Abscissa);
            sumPoint.Abscissa = Auxiliary.Math.Mod(sumPoint.Abscissa, P);

            sumPoint.Ordinate = (k * (fPoint.Abscissa - sumPoint.Abscissa) - fPoint.Ordinate);
            sumPoint.Ordinate = Auxiliary.Math.Mod(sumPoint.Ordinate, P);

            return sumPoint;
        }

        public Point PointDoubling(Point point)
        {
            var doublePoint = new Point();

            var k = Auxiliary.Math.Mod((3 * point.Abscissa * point.Abscissa + A), P) * Auxiliary.Math.ModularMultiplicativeInverse(2 * point.Ordinate, P);
            k = Auxiliary.Math.Mod(k, P);

            doublePoint.Abscissa = (k * k - 2 * point.Abscissa);
            doublePoint.Abscissa = Auxiliary.Math.Mod(doublePoint.Abscissa, P);

            doublePoint.Ordinate = (k * (point.Abscissa - doublePoint.Abscissa) - point.Ordinate);
            doublePoint.Ordinate = Auxiliary.Math.Mod(doublePoint.Ordinate, P);

            return doublePoint;
        }

        public Point PointMultiplication(Point point, int n)
        {
            var newPoint = point;
            var nBits = Auxiliary.Math.GetBits(n);
            for (var i = 1; i < nBits.Length; i++)
            {
                newPoint = PointDoubling(newPoint);
                if (nBits[i] == 1)
                    newPoint = PointAddition(newPoint, point);
            }
            return newPoint;
        }

        // constructors:
        public EllipticCurve(int a, int b, int p)
        {
            try
            {
                if (Auxiliary.Math.Mod(4 * a * a * a + 27 * b * b, p) == 0)
                    throw new Exception("singular curve!");
                A = a;
                B = b;
                P = p;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public EllipticCurve() { }
    }
}
