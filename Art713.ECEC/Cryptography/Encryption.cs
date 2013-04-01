using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<string> l = new List<string>();
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

        public void f(string text)
        {
            var tl1 = text.Length;
            if (tl1 % 2 != 0) ++tl1;

            var part1 = text.Substring(0, tl1 / 2);
            var part2 = text.Substring(tl1 / 2, tl1 / 2 - 1);

            var partsBytesArray = Encoding.GetBytes(part1);
            var p1BI = new BigInteger(partsBytesArray);
            partsBytesArray = Encoding.GetBytes(part2);
            var p2BI = new BigInteger(partsBytesArray);

            if (p1BI > EllipticCurve.P && p2BI > EllipticCurve.P)
            {
                f(part1);
                f(part2);
            }
            else
            {
                if (p1BI > EllipticCurve.P)
                    f(part2);
                else
                {
                    if (p2BI > EllipticCurve.P)
                    {
                        f(part1);
                    }
                }
            }
            if (p1BI < EllipticCurve.P)
                {
                    l.Add(part1);
                }
                if (p2BI < EllipticCurve.P)
                {
                    l.Add(part2);
                }            
        }
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
                var encryptedTextBigInteger = textToEncryptBigInteger * P.Abscissa;
                    Console.WriteLine("encrypted text: \n{0}", encryptedTextBigInteger);
                encryptedTextBigInteger = Auxiliary.Math.Mod(encryptedTextBigInteger, EllipticCurve.P);
                    Console.WriteLine("encrypted text [Mod]: \n{0}", encryptedTextBigInteger);
                Decrypt(encryptedTextBigInteger.ToString() + " " + R.Abscissa.ToString() + " " + R.Ordinate.ToString());
                return encryptedTextBigInteger.ToString() + " " + R.Abscissa.ToString() + " " + R.Ordinate.ToString();
            }
            else
            {
                f(textToEncrypt);
                var s = string.Empty;
                for (var i = 0; i < l.Count; i++)
                {
                    var tArray = Encoding.GetBytes(l[i]);
                    var tBigInteger = new BigInteger(tArray);
                    var eBigInteger = tBigInteger * P.Abscissa;
                    eBigInteger = Auxiliary.Math.Mod(eBigInteger, EllipticCurve.P);
                    s += eBigInteger.ToString() + "+";
                }
                s += " " + R.Abscissa.ToString() + " " + R.Ordinate.ToString();
                Decrypt(s);
                return s;
                /*
                var pLength = Encoding.GetBytes(EllipticCurve.P.ToString()).Length;
                var s = textToEncryptBytesArray[0];//.ToString();
                var part = new List<byte[]>();

                for (var i = 1; i < textToEncryptBytesArray.Length; i++)
                {
                    if (BigInteger.Parse(s + textToEncryptBytesArray[i].ToString()) < EllipticCurve.P)
                    {
                        s += textToEncryptBytesArray[i].ToString();
                    }
                    else
                    {
                        part.Add(BigInteger.Parse(s));
                        s = textToEncryptBytesArray[i].ToString();
                    }
                }

                /*
                for (var i = 1; i < textToEncryptBytesArray.Length; i++)
                {
                    if (BigInteger.Parse(s + textToEncryptBytesArray[i].ToString()) < EllipticCurve.P)
                    {
                        s += textToEncryptBytesArray[i].ToString();
                    }
                    else
                    {
                        part.Add(BigInteger.Parse(s));
                        s = textToEncryptBytesArray[i].ToString();
                    }
                }
                

                var e = part
                    .Select(t => t*P.Abscissa)
                    .Select(encryptedTextBigInteger => Auxiliary.Math.Mod(encryptedTextBigInteger, EllipticCurve.P))
                    .Aggregate(string.Empty, (current, encryptedTextBigInteger) => current + encryptedTextBigInteger.ToString() + "+");
                e += " " + R.Abscissa.ToString() + " " + R.Ordinate.ToString();
                Decrypt(e);
                return e;
                 */
                //return null;


                /*
                var pByteArr = Encoding.GetBytes(EllipticCurve.P.ToString());
                //var textByteArr = textToEncryptBigInteger.ToByteArray();
                var textByteArr = Encoding.GetBytes(textToEncrypt);

                var pByteArrLength = pByteArr.Length;
                var textByteArrLength = textByteArr.Length;

                var index = 0;
                var part = new string[textByteArrLength/pByteArrLength + 1];
                var partByteArr = new byte[pByteArrLength];


                for (var i = 0; i < textByteArrLength/pByteArrLength + 1; i++)
                {
                    for (var j = 0; j < pByteArrLength; j++)
                    {
                        if (j + pByteArrLength*i < textByteArrLength)
                            partByteArr[j] = textByteArr[j + pByteArrLength*i];
                        else
                            partByteArr[j] = 0;
                    }
                    //part[index] = new BigInteger(partByteArr);
                    part[index] = Encoding.GetString(partByteArr);
                    ++index;
                }
                
                var partBigInteger = new BigInteger[part.Length];
                for (var i = 0; i < partBigInteger.Length; i++)
                {                    
                    var partTextToEncryptBytesArray = Encoding.GetBytes(part[i]);
                    partBigInteger[i] = new BigInteger(partTextToEncryptBytesArray);
                }

                var encryptedTextString = partBigInteger
                    .Select(t => t*P.Abscissa)
                    .Select(encryptedTextBigInteger => Auxiliary.Math.Mod(encryptedTextBigInteger, EllipticCurve.P))
                    .Aggregate(string.Empty,
                               (current, encryptedTextBigInteger) => current + encryptedTextBigInteger.ToString());

                encryptedTextString += " " + R.Abscissa.ToString() + " " + R.Ordinate.ToString();

                Decrypt(encryptedTextString);
                return encryptedTextString;
                 */
            }
        }

        public string Decrypt(string encryptedText)
        {
            Console.WriteLine("text to decrypt: \n{0}", encryptedText);
            var encryptedTextArray = encryptedText.Split(' ');
            BigInteger encryptedTextBi = EllipticCurve.P + 1;
            if (encryptedTextArray.Contains("+")) 
                encryptedTextBi = BigInteger.Parse(encryptedTextArray[0]);
            
            //
            var rpoint = new Point(BigInteger.Parse(encryptedTextArray[1]), BigInteger.Parse(encryptedTextArray[2]));
            var qpoint = EllipticCurve.PointMultiplication(rpoint, RecieverSecretKey);
            var x1 = Auxiliary.Math.ModularMultiplicativeInverse(qpoint.Abscissa, EllipticCurve.P);



            //

            if (encryptedTextBi < EllipticCurve.P)
            {
                // this is ain't right in real life! //var qpoint = EllipticCurve.PointMultiplication(rpoint, MySecretKey);

                var txt = Auxiliary.Math.Mod(encryptedTextBi*x1, EllipticCurve.P);
                Console.WriteLine("decrypted text [Mod]: \n{0}", txt);

                var txtByteArray = txt.ToByteArray();
                var decryptedText = Encoding.GetString(txtByteArray);

                Console.WriteLine("decrypted TEXT: {0}", decryptedText);
                return decryptedText;
            }
            else
            {
                var parts = encryptedTextArray[0].Split('+');
                var s = string.Empty;
                for (int i = 0; i < parts.Length-1; i++)
                {
                    var txt = Auxiliary.Math.Mod(BigInteger.Parse(parts[i])*x1, EllipticCurve.P);
                    var txtByteArray = txt.ToByteArray();
                    var decryptedText = Encoding.GetString(txtByteArray);
                    s += decryptedText;
                }
                s += "***";

                /*
                var eArr = encryptedTextArray[0].Split('+');
                var eArrBi = new BigInteger[eArr.Length - 1];
                for (var index = 0; index < eArrBi.Length; index++)
                    eArrBi[index] = BigInteger.Parse(eArr[index]);
                var text = string.Empty;
                var s = string.Empty;
                for (var i = 0; i < eArrBi.Length; i++)
                {
                    var txt = Auxiliary.Math.Mod(eArrBi[i] * x1, EllipticCurve.P);
                    s += txt.ToString();
                }
                */
                //var s1 = Encoding.GetBytes(s);
                //var s2 = Encoding.GetString(s1);   

                //text = Encoding.GetString(ss);

                //return text;
                /*
                var pByteArr = Encoding.GetBytes(EllipticCurve.P.ToString());
                //var textToDecryptByteArr = encryptedTextBi.ToByteArray();
                var textToDecryptByteArr = Encoding.GetBytes(encryptedTextArray[0]);

                var pByteArrLength = pByteArr.Length;
                var textToDecryptByteArrLength = textToDecryptByteArr.Length;

                var index = 0;
                var part = new BigInteger[textToDecryptByteArrLength / pByteArrLength + 1];
                var partByteArr = new byte[pByteArrLength];

                for (var i = 0; i < textToDecryptByteArrLength / pByteArrLength + 1; i++)
                {
                    for (var j = 0; j < pByteArrLength; j++)
                    {
                        if (j + pByteArrLength * i < textToDecryptByteArrLength)
                            partByteArr[j] = textToDecryptByteArr[j + pByteArrLength * i];
                        else
                            partByteArr[j] = 0;
                    }
                    part[index] = new BigInteger(partByteArr);
                    ++index;
                }
                var decryptedText = string.Empty;
                for (var i = 0; i < part.Length; i++)
                {
                    var txt = Auxiliary.Math.Mod(part[i]*x1, EllipticCurve.P);
                    //Console.WriteLine("decrypted text [Mod]: \n{0}", txt);
                    var txtByteArray = txt.ToByteArray();
                    decryptedText += Encoding.GetString(txtByteArray);                    
                }

                Console.WriteLine("decrypted TEXT: {0}", decryptedText);
                return decryptedText;
                 */
                //}

                //return null;
            }
            return null;
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