using System.Runtime.Serialization;
using System.ServiceModel;

namespace SecureMessagingService
{
    [ServiceContract(CallbackContract = typeof(ISecureMessagingServiceCallback))]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]

    }
}