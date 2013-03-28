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
        public int KeyLength { get; set; }
        public BigInteger MySecretKey { get; set; }
        public Point MyPublicKey { get; set; }

        public Encrypt(string textToEncrypt)
        {
            EllipticCurve = new EllipticCurve(1,1,11);
            KeyLength = 128;

            GenerateKeys();

            /*
            var k = new Random().Next(EllipticCurve.Q);
            var R = EllipticCurve.PointMultiplication(EllipticCurve.Generator, k);

            var receiverPublicKey = new Point(3, 8);

            var P = EllipticCurve.PointMultiplication(receiverPublicKey, k);
            
            var encoding = new UTF8Encoding();
            var textToEncryptInBytes = encoding.GetBytes(textToEncrypt);

            if (textToEncryptInBytes.Length > EllipticCurve.P)
            {
                for (var i = 0; i < 4*textToEncrypt.Length/EllipticCurve.P; i++)
                {
                    var partOfText = textToEncrypt.Substring(EllipticCurve.P*i/4, EllipticCurve.P/4);
                    var partOfTextInBytes = encoding.GetBytes(partOfText);
                    var partOfTextToEncrypt = new BigInteger(partOfTextInBytes);

                }
            }
             * */
        }

        private void GenerateKeys()
        {
            var randomByteGenerator = new RNGCryptoServiceProvider();
            var randomByteArray = new byte[KeyLength];
            randomByteGenerator.GetNonZeroBytes(randomByteArray);

            MySecretKey = new BigInteger(randomByteArray);
            //MyPublicKey = EllipticCurve.PointMultiplication(EllipticCurve.Generator, MySecretKey);
        }
    }
}
