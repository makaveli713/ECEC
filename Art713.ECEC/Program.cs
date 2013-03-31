using System;
using System.Linq;
using System.Numerics;
using System.Text;
using Art713.ECEC.Cryptography;

namespace Art713.ECEC
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("ECEC");
            /*
            var encoding = new UTF8Encoding();
            var experiment = new BigInteger(1234567890);
            var experimentStr = experiment.ToString();
            var experimentBytes = encoding.GetBytes(experimentStr);
            var smth = experimentBytes.Aggregate(string.Empty, (current, b) => current + b.ToString());
            var bi = BigInteger.Parse()
            var tempArr = new byte[5];            
                tempArr[0] = experimentBytes[0];
                tempArr[1] = experimentBytes[1];
                tempArr[2] = experimentBytes[2];
                tempArr[3] = experimentBytes[3];
                tempArr[4] = experimentBytes[4];
            var s1 = encoding.GetString(tempArr);

                tempArr[0] = experimentBytes[2];
                tempArr[1] = experimentBytes[3];
            var temp2 = new BigInteger(tempArr);
            var s2 = temp2.ToString();
              */
                
            var obj = new Encryption();
            //Console.WriteLine(obj.Encrypt("Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn"));
            obj.Encrypt("Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn");
//            obj.Encrypt("Artem Trubitsyn");
            //var obj = new Encrypt.Encrypt(713030391);

            //var obj = new Encrypt.Encrypt("Trubitsyn Artem");

            //Console.WriteLine(Auxiliary.Math.Mod(77,17));
            //Console.WriteLine(Auxiliary.Math.Mod((int)Auxiliary.Math.ExtendedEuclideanAlgorithm(17,2),17));
            //Console.WriteLine(Auxiliary.Math.ModularMultiplicativeInverse(19, 4));

            //Console.WriteLine(Auxiliary.Math.GetBits(256));

            //Console.WriteLine(Auxiliary.Math.ModularMultiplicativeInverse(7,7));
            
            /*
            var point = new Point(5, 1);
            var ec = new EllipticCurve(2, 2, 17);
            var newPoint = ec.PointMultiplication(point, 4);
            Console.WriteLine("({0},{1})",newPoint.Abscissa,newPoint.Ordinate);
            */

            /*
            var point1 = new Point(5, 1);
            var point2 = new Point(4, 6);
            var ec = new EllipticCurve(2, 6, 7);
            Console.WriteLine("({0},{1})",ec.PointAddition(point1, point2).Abscissa, ec.PointAddition(point1, point2).Ordinate);
             * */


           

            /*
            for (var x = 0; x < 258; x++)
            {

                Console.Write("{0}: ",x);
                // получаем биты числа
                for (var i = 0; i < 8; ++i)
                    Console.Write((x >> i) & 1);
                Console.WriteLine();
            }
            

            
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
            */
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
