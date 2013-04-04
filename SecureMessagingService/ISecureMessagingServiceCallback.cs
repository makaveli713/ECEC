using System.ServiceModel;

namespace SecureMessagingService
{
    interface ISecureMessagingServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendEncryptedMsg(string encryptedMsg);
    }
}