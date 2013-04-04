using System.ServiceModel;

namespace SecureMessagingService
{
    [ServiceContract(CallbackContract = typeof(ISecureMessagingServiceCallback))]
    public interface ISecureMessagingService
    {
        [OperationContract(IsOneWay = true)]
        string Encrypt(string textToEncrypt);

        [OperationContract(IsOneWay = true)]
        string Decrypt(string textToDecrypt);
    }
}