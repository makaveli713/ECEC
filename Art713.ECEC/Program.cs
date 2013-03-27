using System;
using System.Collections.Generic;
using System.Globalization;
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
            /*
            for (var x = 0; x < 258; x++)
            {

                Console.Write("{0}: ",x);
                // получаем биты числа
                for (var i = 0; i < 8; ++i)
                    Console.Write((x >> i) & 1);
                Console.WriteLine();
            }
            */

            
            for (int i = 0; i < 260; i++)
            {
                int x = i;
                // получаем биты числа

                var arr = Auxiliary.Math.GetBits(x);
                Console.Write("{0}: ",x);
                for (int index = arr.Length-1; index >= 0; index--)
                {
                    byte b = arr[index];
                    Console.Write(b);
                }
                Console.WriteLine();
            }
            
            //var firstPoint = new Point(5, 1);
            ////var secondPoint = new Point(4, 6); 
            //var obj = new EllipticCurve(2,6,7);
            ////var sum = obj.PointAddition(firstPoint, secondPoint);
            //var sum = obj.PointDoubling(firstPoint);
            //Console.WriteLine("abscissa = {0}, ordinate = {1}",sum.Abscissa,sum.Ordinate);
            Console.ReadLine();
        }
    }
}
