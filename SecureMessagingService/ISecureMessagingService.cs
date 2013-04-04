using System.ServiceModel;

namespace SecureMessagingService
{
    [ServiceContract(CallbackContract = typeof(ISecureMessagingServiceCallback))]
    public interface ISecureMessagingService
    {
        [OperationContract]
        string Encrypt(string textToEncrypt);

        [OperationContract]
        string Decrypt(string textToDecrypt);
    }
}