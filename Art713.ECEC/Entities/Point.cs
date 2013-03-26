namespace Art713.ECEC.Entities
{
    /// <summary>
    /// class Point represent a point on elliptic curve
    /// </summary>
    public class Point
    {
        /// <summary>
        /// first coordinate of the point
        /// </summary>
        public int XPoint { get; set; } // int to BigInteger ?
        /// <summary>
        /// second coordinate of the point
        /// </summary>
        public int YPoint { get; set; } // int to BigInteger ?
    }
}
