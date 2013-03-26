using System.Collections.Generic;

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
            var k = ((sPoint.Ordinate - fPoint.Ordinate)/(sPoint.Abscissa - fPoint.Abscissa));
            k = Auxiliary.Math.Mod(k,P);
            sumPoint.Abscissa = (k*k - fPoint.Abscissa - sPoint.Abscissa);
            sumPoint.Abscissa = Auxiliary.Math.Mod(sumPoint.Abscissa, P);
            sumPoint.Ordinate = (k*(fPoint.Abscissa - sumPoint.Abscissa) - fPoint.Ordinate);
            sumPoint.Ordinate = Auxiliary.Math.Mod(sumPoint.Ordinate,P);
            return sumPoint;
        }

        public Point PointDoubling(Point point)
        {
            throw new System.NotImplementedException();
        }

        public Point PointMultiplication(Point point, int n)
        {
            throw new System.NotImplementedException();
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
