using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Security.Cryptography;
using System.Text;
using Art713.Project713.Entities;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Art713.Project713.Cryptography
{
    /// <summary>
    /// Encryption class allow to encrypt & decrypt data 
    /// using El-Gamal encryption on elliptic curve
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// When a numeric value of the text (which user want to encrypt)
        /// is less than zero this flag has 'true' value
        /// and 'false' value otherwise (default).
        /// </summary>
        public bool NegativeSignFlag = false;
        /// <summary>
        /// When a numeric value of the text (which user want to encrypt)
        /// is greater than modulus of the given elliptic curve
        /// text divides on parts and PartListWithSign contains 
        /// every part of this text with boolean value, 
        /// which show if the BigInteger representation of the part 
        /// is less than zero
        /// </summary>
        public Dictionary<string, bool> PartListWithSign = new Dictionary<string, bool>();
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
        /// This method allow to divide text
        /// in the case if BigInteger representation
        /// of this text is greater than the modulus
        /// of the given elliptic curve.
        /// </summary>
        /// <param name="text">Text to encrypt.</param>
        public void GetParts(string text)
        {            
            var textCharArr = text.ToCharArray();
            var textLength = textCharArr.Length;

            if (textLength % 2 != 0) ++textLength;

            var part1 = text.Substring(0, textLength / 2);
            var part2 = text.Substring(textLength / 2, text.Length - textLength/2);

            var partsBytesArray = Encoding.GetBytes(part1);
            var part1BigInteger = new BigInteger(partsBytesArray);
            partsBytesArray = Encoding.GetBytes(part2);
            var part2BigInteger = new BigInteger(partsBytesArray);

            var f1 = false;
            var f2 = false;
            if (part1BigInteger < 0)
            {
                f1 = true;
                part1BigInteger *= -1;
            }
            if (part2BigInteger < 0)
            {
                f2 = true;
                part2BigInteger *= -1;
            }

            if (part1BigInteger > EllipticCurve.P && part2BigInteger > EllipticCurve.P)
            {
                GetParts(part1);
                GetParts(part2);
            }
            else
            {
                if (part1BigInteger < EllipticCurve.P && part2BigInteger < EllipticCurve.P)
                {
                    PartListWithSign.Add(part1, f1);
                    PartListWithSign.Add(part2, f2);
                }
                else
                {
                    if (part1BigInteger > EllipticCurve.P && part2BigInteger < EllipticCurve.P)
                    {                        
                        GetParts(part1);
                        PartListWithSign.Add(part2, f2);
                    }
                    if (part2BigInteger > EllipticCurve.P && part1BigInteger < EllipticCurve.P)
                    {
                        PartListWithSign.Add(part1, f1);
                        GetParts(part2);                        
                    }                    
                }               
            }
        }
        /// <summary>
        /// method: Allow to encrypt text data.
        /// </summary>
        /// <param name="textToEncrypt">Text to encrypt.</param>
        /// <returns>Encrypted text and R point as a string</returns>
        public async void Encrypt(string textToEncrypt)
        {
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine(textToEncrypt);
            //Console.ResetColor();

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
            if (textToEncryptBigInteger < 0)
            {
                textToEncryptBigInteger *= -1;
                NegativeSignFlag = true;
            }
            //Console.WriteLine("text [BigInteger]: \n{0}", textToEncryptBigInteger);

            if (textToEncryptBigInteger < EllipticCurve.P)
            {
                var encryptedTextBigInteger = textToEncryptBigInteger * pPoint.Abscissa;
                encryptedTextBigInteger = Auxiliary.Math.Mod(encryptedTextBigInteger, EllipticCurve.P);
                //Console.WriteLine("encrypted text [Mod]: \n{0}", encryptedTextBigInteger);
                var e = (NegativeSignFlag)
                           ? ("1-" + encryptedTextBigInteger + " " + rPoint.Abscissa + " " + rPoint.Ordinate)
                           : ("0-" + encryptedTextBigInteger + " " + rPoint.Abscissa + " " + rPoint.Ordinate);
                //Decrypt(e);
                //return e;
            }

            GetParts(textToEncrypt);
            var s = PartListWithSign
                .Select(sign => sign.Value)
                .Aggregate(string.Empty, (current, sign) => sign ? current + "1" : current + "0") + "-";
            s += PartListWithSign
                .Select(part => (part.Value) ? -1 * (new BigInteger(Encoding.GetBytes(part.Key))) : new BigInteger(Encoding.GetBytes(part.Key)))
                .Select(part => part * pPoint.Abscissa)
                .Select(part => Auxiliary.Math.Mod(part, EllipticCurve.P))
                .Aggregate(string.Empty, (current, part) => current + part.ToString() + "+");

            s = s.Remove(s.Length - 1, 1);
            s += " " + rPoint.Abscissa + " " + rPoint.Ordinate;
            //Decrypt(s);
            
            
            //DataWriter dw = new DataWriter()           
            //dw.WriteBytes();

            IStorageFile file = new StorageFile();
            StorageFolder localFolder = KnownFolders.DocumentsLibrary;
            file  = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.OpenIfExists);
            //FileIO.WriteBytesAsync

            //return s;
        }

        public string Decrypt(string encryptedText)
        {
            //Console.WriteLine("text to decrypt: \n{0}", encryptedText);
            var signes = encryptedText.Split('-');
            var encryptedTextStringArray = signes[1].Split(' ');

            BigInteger encryptedTextBigInteger;
            var parsed = BigInteger.TryParse(encryptedTextStringArray[0], out encryptedTextBigInteger);

            var rpoint = new Point(BigInteger.Parse(encryptedTextStringArray[1]), BigInteger.Parse(encryptedTextStringArray[2]));
            var qpoint = EllipticCurve.PointMultiplication(rpoint, RecieverSecretKey);
            var x1 = Auxiliary.Math.ModularMultiplicativeInverse(qpoint.Abscissa, EllipticCurve.P);

            if (parsed)
            {
                // this is ain't right in real life! var qpoint = EllipticCurve.PointMultiplication(rpoint, MySecretKey);

                var txt = Auxiliary.Math.Mod(encryptedTextBigInteger * x1, EllipticCurve.P);
                //Console.WriteLine("decrypted text [Mod]: \n{0}", txt);

                if (signes[0] == "1")
                    txt *= -1;

                var txtByteArray = txt.ToByteArray();
                var decryptedText = Encoding.GetString(txtByteArray,0,txtByteArray.Length);

                //Console.WriteLine("decrypted TEXT: {0}", decryptedText);
                return decryptedText;
            }
            //var signesCharArr = signes[0].ToArray<char>();
            var signesCharArr = signes[0].ToCharArray();
            var parts = encryptedTextStringArray[0].Split('+');
            var s = string.Empty;
            for (var i = 0; i < parts.Length; i++)
            {
                var part = Auxiliary.Math.Mod(BigInteger.Parse(parts[i]) * x1, EllipticCurve.P);
                if (signesCharArr[i] == '1')
                    part *= -1;
                s += Encoding.GetString(part.ToByteArray(),0,part.ToByteArray().Length);
            }
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("Decrypted text: {0}", s);
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
            //var provider = new  RNGCryptoServiceProvider();
            var randomBytesArray = new byte[bound.ToByteArray().Length - 1];
            IBuffer randomBuffer = CryptographicBuffer.GenerateRandom((uint)randomBytesArray.Length);
            CryptographicBuffer.CopyToByteArray(randomBuffer, out randomBytesArray);            
            //provider.GetNonZeroBytes(randomBytesArray);
            var newBigInteger = new BigInteger(randomBytesArray);
            return (newBigInteger > 0) ? newBigInteger : -newBigInteger;
        }
    }
}