using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IServiceContracts
    {
        [OperationContract]
        void StartTimer();
        [OperationContract]
        void StopTimer();
        [OperationContract]
        void CancelTimer();
        [OperationContract]
        void SetTimer(string miliseconds);
        [OperationContract]
        int ReadTimer();
    }
}
