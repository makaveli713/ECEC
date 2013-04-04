namespace Art713.ECEC.Infrastructure
{
    interface IEncryption
    {
        string Encrypt(string textToEncrypt);
        string Decrypt(string textToDecrypt);
    }
}
