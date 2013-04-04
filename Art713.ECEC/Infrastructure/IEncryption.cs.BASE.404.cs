using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art713.ECEC.Infrastructure
{
    interface IEncryption
    {
        string Encrypt(string textToEncrypt);
        string Decrypt(string textToDecrypt);
    }
}
