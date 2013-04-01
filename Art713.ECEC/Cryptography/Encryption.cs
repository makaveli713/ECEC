﻿using System;
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
        public List<string> PartsList = new List<string>();
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

        public void GetParts(string text)
        {
            var textLength = text.Length;
            if (textLength % 2 != 0) ++textLength;

            var part1 = text.Substring(0, textLength / 2);
            var part2 = text.Substring(textLength / 2, textLength / 2 - 1);

            var partsBytesArray = Encoding.GetBytes(part1);
            var part1BigInteger = new BigInteger(partsBytesArray);
            partsBytesArray = Encoding.GetBytes(part2);
            var part2BigInteger = new BigInteger(partsBytesArray);

            if (part1BigInteger > EllipticCurve.P && part2BigInteger > EllipticCurve.P)
            {
                GetParts(part1);
                GetParts(part2);
            }
            else
            {
                if (part1BigInteger > EllipticCurve.P)
                    GetParts(part2);
                else
                {
                    if (part2BigInteger > EllipticCurve.P)
                    {
                        GetParts(part1);
                    }
                }
            }
            if (part1BigInteger < EllipticCurve.P)
                {
                    PartsList.Add(part1);
                }
                if (part2BigInteger < EllipticCurve.P)
                {
                    PartsList.Add(part2);
                }            
        }
        /// <summary>
        /// method: Allow to encrypt text data.
        /// </summary>
        /// <param name="textToEncrypt">Text to encrypt.</param>
        /// <returns>Encrypted text and R point as a string</returns>
        public string Encrypt(string textToEncrypt)
        {
            Console.WriteLine(textToEncrypt);

            // только для тестирования!!!
            const int a = -3;
            var b = BigInteger.Parse("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b", System.Globalization.NumberStyles.HexNumber);
            var p = BigInteger.Parse("115792089210356248762697446949407573530086143415290314195533631308867097853951");
            EllipticCurve = new EllipticCurve(a, b, p)
                {
                    N = BigInteger.Parse("115792089210356248762697446949407573529996955224135760342422259061068512044369")                   
                };
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
            var rPoint = EllipticCurve.PointMultiplication(EllipticCurve.Generator, k);
            var pPoint = EllipticCurve.PointMultiplication(RecieverPublicKey, k);

            var textToEncryptBytesArray = Encoding.GetBytes(textToEncrypt);
            var textToEncryptBigInteger = new BigInteger(textToEncryptBytesArray);
            Console.WriteLine("text [BigInteger]: \n{0}", textToEncryptBigInteger);

            if (textToEncryptBigInteger < EllipticCurve.P)
            {
                var encryptedTextBigInteger = textToEncryptBigInteger * pPoint.Abscissa;
                    Console.WriteLine("encrypted text: \n{0}", encryptedTextBigInteger);
                encryptedTextBigInteger = Auxiliary.Math.Mod(encryptedTextBigInteger, EllipticCurve.P);
                    Console.WriteLine("encrypted text [Mod]: \n{0}", encryptedTextBigInteger);
                Decrypt(encryptedTextBigInteger.ToString() + " " + rPoint.Abscissa.ToString() + " " + rPoint.Ordinate.ToString());
                return encryptedTextBigInteger.ToString() + " " + rPoint.Abscissa.ToString() + " " + rPoint.Ordinate.ToString();
            }
            GetParts(textToEncrypt);
            var s = PartsList
                .Select(part => Encoding.GetBytes(part))
                .Select(part => new BigInteger(part))
                .Select(part => part*pPoint.Abscissa)
                .Select(part => Auxiliary.Math.Mod(part,EllipticCurve.P))
                .Aggregate(string.Empty, (current, part) => current + part.ToString()+"+");
           
            s = s.Remove(s.Length-1, 1);
            s += " " + rPoint.Abscissa.ToString() + " " + rPoint.Ordinate.ToString();
            Decrypt(s);
            return s;
        }

        public string Decrypt(string encryptedText)
        {
            Console.WriteLine("text to decrypt: \n{0}", encryptedText);
            var encryptedTextStringArray = encryptedText.Split(' ');
            var encryptedTextBigInteger = EllipticCurve.P + 1;
            if (encryptedTextStringArray.Contains("+")) 
                encryptedTextBigInteger = BigInteger.Parse(encryptedTextStringArray[0]);
            
            //
            var rpoint = new Point(BigInteger.Parse(encryptedTextStringArray[1]), BigInteger.Parse(encryptedTextStringArray[2]));
            var qpoint = EllipticCurve.PointMultiplication(rpoint, RecieverSecretKey);
            var x1 = Auxiliary.Math.ModularMultiplicativeInverse(qpoint.Abscissa, EllipticCurve.P);
            //

            if (encryptedTextBigInteger < EllipticCurve.P)
            {
                // this is ain't right in real life! var qpoint = EllipticCurve.PointMultiplication(rpoint, MySecretKey);

                var txt = Auxiliary.Math.Mod(encryptedTextBigInteger*x1, EllipticCurve.P);
                Console.WriteLine("decrypted text [Mod]: \n{0}", txt);

                var txtByteArray = txt.ToByteArray();
                var decryptedText = Encoding.GetString(txtByteArray);

                Console.WriteLine("decrypted TEXT: {0}", decryptedText);
                return decryptedText;
            }

            var parts = encryptedTextStringArray[0].Split('+');                               
            var s = parts
                .Select(part => BigInteger.Parse(part)*x1)
                .Select(part => Auxiliary.Math.Mod(part,EllipticCurve.P))
                .Aggregate(string.Empty, (current, part) => current + Encoding.GetString(part.ToByteArray()));
            Console.WriteLine("Decrypted text: {0}",s);
            return s;
        }

/*
        private void GenerateKeys()
        {
            MySecretKey = RandomBigIntegerGenerator(EllipticCurve.Q);
            MyPublicKey = EllipticCurve.PointMultiplication(EllipticCurve.Generator, MySecretKey);
        }
*/

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