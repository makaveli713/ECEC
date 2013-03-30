using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Art713.ECEC.Entities;

namespace Art713.ECEC.Cryptography
{
    /// <summary>
    /// Encryption class allow to encrypt & decrypt data 
    /// using El-Gamal encryption on elliptic curve
    /// </summary>
    class Encryption
    {
        /// <summary>
        /// Elliptic curve.
        /// For more information see <see cref="EllipticCurve"/> class.
        /// </summary>
        public EllipticCurve EllipticCurve;
        /// <summary>
        /// property: user's secret key
        /// </summary>
        public BigInteger MySecretKey { get; set; }
        /// <summary>
        /// property: user's public key
        /// </summary>
        public Point MyPublicKey { get; set; }
        /// <summary>
        /// only for testing purposes!!!
        /// </summary>
        public BigInteger RecieverSecretKey { get; set; }
        /// <summary>
        /// reciever public key from public keys database
        /// </summary>
        public Point RecieverPublicKey { get; set; }
        /// <summary>
        /// UTF-8 encoding to encode data
        /// </summary>
        private static readonly UTF8Encoding Encoding = new UTF8Encoding();
        /// <summary>
        /// method: Allow to encrypt text data.
        /// </summary>
        /// <param name="textToEncrypt">Text to encrypt.</param>
        /// <returns>Encrypted text and R point as a string</returns>
        public string Encrypt(string textToEncrypt)
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

            // only for testing purpose!!!
                RecieverSecretKey = RandomBigIntegerGenerator(EllipticCurve.Q);            
                RecieverPublicKey = EllipticCurve.PointMultiplication(EllipticCurve.Generator, RecieverSecretKey);
            //

            var k = RandomBigIntegerGenerator(EllipticCurve.Q);
            var R = EllipticCurve.PointMultiplication(EllipticCurve.Generator, k);
            var P = EllipticCurve.PointMultiplication(RecieverPublicKey, k);

            var textToEncryptBytesArray = Encoding.GetBytes(textToEncrypt);
            var textToEncryptBigInteger = new BigInteger(textToEncryptBytesArray);
                Console.WriteLine("text [BigInteger]: \n{0}", textToEncryptBigInteger);

            if (textToEncryptBigInteger < EllipticCurve.P)
            {
                var encryptedTextBigInteger = textToEncryptBigInteger*P.Abscissa;
                    Console.WriteLine("encrypted text: \n{0}", encryptedTextBigInteger);
                encryptedTextBigInteger = Auxiliary.Math.Mod(encryptedTextBigInteger, EllipticCurve.P);
                    Console.WriteLine("encrypted text [Mod]: \n{0}", encryptedTextBigInteger);
                return encryptedTextBigInteger.ToString() + " " + R.Abscissa.ToString() + " " + R.Ordinate.ToString();
            }

            var pByteArr = Encoding.GetBytes(EllipticCurve.P.ToString());
            var textByteArr = Encoding.GetBytes(textToEncryptBigInteger.ToString());// same as textToEncryptBytesArray ?

            var pByteArrLength = pByteArr.Length;
            var textByteArrLength = textByteArr.Length;
            
            var index = 0;
            var part = new BigInteger[textByteArrLength / pByteArrLength + 1];
            var partByteArr = new byte[pByteArrLength];
            for (var i = 0; i < textByteArrLength / pByteArrLength + 1; i++)
            {
                for (var j = 0; j < pByteArrLength; j++)
                {
                    partByteArr[j] = textByteArr[j + pByteArrLength * i];
                }
                part[index] = new BigInteger(partByteArr[i]);
                ++index;
            }

                // Console.WriteLine("text length is more than modulus!");
                // do smth! 
            return null;
        }        

        public string Decrypt(string encryptedText)
        {
            Console.WriteLine("text to decrypt: \n{0}", encryptedText);
            var encryptedTextArray = encryptedText.Split(' ');
            
            var rpoint = new Point(BigInteger.Parse(encryptedTextArray[1]), BigInteger.Parse(encryptedTextArray[2]));
            //var qpoint = EllipticCurve.PointMultiplication(rpoint, RecieverSecretKey);
            var qpoint = EllipticCurve.PointMultiplication(rpoint, MySecretKey);

            var x1 = Auxiliary.Math.ModularMultiplicativeInverse(qpoint.Abscissa, EllipticCurve.P);
            
            var txt = Auxiliary.Math.Mod(BigInteger.Parse(encryptedTextArray[0]) * x1, EllipticCurve.P);
                Console.WriteLine("decrypted text [Mod]: \n{0}", txt);

            var txtByteArray = txt.ToByteArray();
            var decryptedText = Encoding.GetString(txtByteArray);

            Console.WriteLine(decryptedText);
            return decryptedText;
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