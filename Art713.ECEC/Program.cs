using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art713.ECEC.Entities;

namespace Art713.ECEC
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("ECEC");
            var firstPoint = new Point(5, 1);
            var secondPoint = new Point(4, 6); 
            var obj = new EllipticCurve(2,6,7);
            var sum = obj.PointAddition(firstPoint, secondPoint);
            Console.WriteLine("abscissa = {0}, ordinate = {1}",sum.Abscissa,sum.Ordinate);
            Console.ReadLine();
        }
    }
}
