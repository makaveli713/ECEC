using System.Collections.Generic;
using System.Globalization;

namespace Art713.ECEC.Entities
{
    /// <summary>
    /// class EllipticCurve represent a elliptic curve
    /// </summary>
    public class EllipticCurve
    {
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
        public List<Point> Points { get; set; }
        
        // methods:

        public void FillPoint()
        {
            throw new System.NotImplementedException();
        }

        public Point PointAddition(Point fPoint, Point sPoint)
        {
            var sumPoint = new Point();
            var k = Auxiliary.Math.Mod((sPoint.Ordinate - fPoint.Ordinate),P)/(sPoint.Abscissa - fPoint.Abscissa);
            k = Auxiliary.Math.Mod(k,P);
            sumPoint.Abscissa = (k*k - fPoint.Abscissa - sPoint.Abscissa);
            sumPoint.Abscissa = Auxiliary.Math.Mod(sumPoint.Abscissa, P);
            sumPoint.Ordinate = (k*(fPoint.Abscissa - sumPoint.Abscissa) - fPoint.Ordinate);
            sumPoint.Ordinate = Auxiliary.Math.Mod(sumPoint.Ordinate,P);
            return sumPoint;
        }

        public Point PointDoubling(Point point)
        {
            var doublePoint = new Point();
            var k = Auxiliary.Math.Mod((3*point.Abscissa*point.Abscissa + A),P) / (2*point.Ordinate);
            k = Auxiliary.Math.Mod(k, P);
            doublePoint.Abscissa = (k * k - 2*point.Abscissa);
            doublePoint.Abscissa = Auxiliary.Math.Mod(doublePoint.Abscissa, P);
            doublePoint.Ordinate = (k * (point.Abscissa - doublePoint.Abscissa) - point.Ordinate);
            doublePoint.Ordinate = Auxiliary.Math.Mod(doublePoint.Ordinate, P);            
            return doublePoint;
        }

        public Point PointMultiplication(Point point, int n)
        {
            var newPoint = new Point(0, 0);
            var nB = byte.Parse(n.ToString(CultureInfo.InvariantCulture));
            return newPoint;
        }

        // constructors:
        public EllipticCurve(int a, int b, int p)
        {
            A = a;
            B = b;
            P = p;
        }

        public EllipticCurve(){}
    }
}
