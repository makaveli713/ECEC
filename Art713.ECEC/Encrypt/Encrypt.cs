using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Art713.ECEC.Entities;

namespace Art713.ECEC.Encrypt
{
    class Encrypt
    {
        public EllipticCurve EllipticCurve;
        public BigInteger MySecretKey { get; set; }
        public Point MyPublicKey { get; set; }

        public BigInteger RecieverSecretKey { get; set; }
        public Point RecieverPublicKey { get; set; }

        public Encrypt(string textToEncrypt)
        {
            Console.WriteLine(textToEncrypt);

            // только для тестирования!!!
            var a = -3;
            var b = BigInteger.Parse("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b", System.Globalization.NumberStyles.HexNumber);
            var p = BigInteger.Parse("115792089210356248762697446949407573530086143415290314195533631308867097853951");
            EllipticCurve = new EllipticCurve(a, b, p);
            EllipticCurve.N =
                    BigInteger.Parse("115792089210356248762697446949407573529996955224135760342422259061068512044369");
            EllipticCurve.Q = EllipticCurve.N;
            var x = BigInteger.Parse("6b17d1f2e12c4247f8bc96e563a440f277037d812deb33a0f4a13945d898c296",
                                     System.Globalization.NumberStyles.HexNumber);
            var y = BigInteger.Parse("4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5", System.Globalization.NumberStyles.HexNumber);
            EllipticCurve.Generator = new Point(x, y);
            // 

            // метод GenerateKeys() должен вызываться в момент регистрации пользователя           

            RecieverSecretKey = RandomBigIntegerGenerator(EllipticCurve.Q);
            RecieverPublicKey = EllipticCurve.PointMultiplication(EllipticCurve.Generator, RecieverSecretKey);

            var k = RandomBigIntegerGenerator(EllipticCurve.Q);
            var R = EllipticCurve.PointMultiplication(EllipticCurve.Generator, k);
            var P = EllipticCurve.PointMultiplication(RecieverPublicKey, k);

            var encoding = new UTF8Encoding();
            var textToEncryptBytesArray = encoding.GetBytes(textToEncrypt);
            var textToEncryptBigInteger = new BigInteger(textToEncryptBytesArray);
            Console.WriteLine("text [BigInteger]: \n{0}", textToEncryptBigInteger);

            if (textToEncryptBigInteger < EllipticCurve.P)
            {
                var encryptedText = textToEncryptBigInteger * P.Abscissa;
                Console.WriteLine("encrypted text: \n{0}", encryptedText);
                encryptedText = Auxiliary.Math.Mod(encryptedText, EllipticCurve.P);
                Console.WriteLine("encrypted text [Mod]: \n{0}", encryptedText);
                Decrypt(R, encryptedText);
            }
            else
                Console.WriteLine("text length is more than modulus!");
        }

        public void Decrypt(Point rpoint, BigInteger encryptedText)
        {
            Console.WriteLine("text to decrypt: \n{0}", encryptedText);

            var Q = EllipticCurve.PointMultiplication(rpoint, RecieverSecretKey);

            var x1 = Auxiliary.Math.ModularMultiplicativeInverse(Q.Abscissa, EllipticCurve.P);

            var txt = encryptedText * x1;
            Console.WriteLine("decrypted text: \n{0}", txt);

            txt = Auxiliary.Math.Mod(encryptedText * x1, EllipticCurve.P);
            Console.WriteLine("decrypted text [Mod]: \n{0}", txt);

            var txtByteArray = txt.ToByteArray();
            var encoding = new UTF8Encoding();
            var decryptedText = encoding.GetString(txtByteArray);

            Console.WriteLine(decryptedText);
        }

        private void GenerateKeys()
        {
            MySecretKey = RandomBigIntegerGenerator(EllipticCurve.Q);
            MyPublicKey = EllipticCurve.PointMultiplication(EllipticCurve.Generator, MySecretKey);
        }

        public static BigInteger RandomBigIntegerGenerator(BigInteger bound)
        {
            var provider = new RNGCryptoServiceProvider();
            var randomBytesArray = new byte[bound.ToByteArray().Length - 1];
            provider.GetNonZeroBytes(randomBytesArray);
            var newBigInteger = new BigInteger(randomBytesArray);
            return (newBigInteger > 0) ? newBigInteger : -newBigInteger;
        }
    }
}