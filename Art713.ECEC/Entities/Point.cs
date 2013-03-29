using System.Numerics;

namespace Art713.ECEC.Entities
{
    /// <summary>
    /// class Point represent a point on elliptic curve
    /// </summary>
    public class Point
    {
        //public EllipticCurve ellipticCurve = new EllipticCurve();
        /// <summary>
        /// first coordinate of the point
        /// </summary>
        public BigInteger Abscissa { get; set; } // int to BigInteger ?
        /// <summary>
        /// second coordinate of the point
        /// </summary>
        public BigInteger Ordinate { get; set; } // int to BigInteger ?
        /// <summary>
        /// Point's class constructor
        /// </summary>
        /// <param name="xPoint">Abscissa</param>
        /// <param name="yPoint">Ordinate</param>
        public Point(BigInteger xPoint, BigInteger yPoint)
        {           
            Abscissa = xPoint;
            Ordinate = yPoint;
        }
        /// <summary>
        /// Point's class default constructor
        /// </summary>
        public Point(){}
    }
}
