using System.ServiceModel;

namespace SecureMessagingService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SecureMessagingService : ISecureMessagingService
    {
        public string Encrypt(string textToEncrypt)
        {
            var obj = new Art713.ECEC.Cryptography.Encryption();
            return obj.Encrypt(textToEncrypt);
        }

        public string Decrypt(string textToDecrypt)
        {
            var obj = new Art713.ECEC.Cryptography.Encryption();
            return obj.Decrypt(textToDecrypt);
        }
    }
}
