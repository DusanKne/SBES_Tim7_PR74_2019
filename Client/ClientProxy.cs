using Common;
using SecurityManager;
using System;
using System.ServiceModel;

namespace Client
{
    internal class ClientProxy : ChannelFactory<IServiceContracts>, IServiceContracts, IDisposable
    {
        IServiceContracts factory;
        public ClientProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }
        public void CancelTimer()
        {
            try
            {
                factory.CancelTimer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int ReadTimer()
        {
            try
            {
                return factory.ReadTimer();
            }
            catch (FaultException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public void SetTimer(string miliseconds)
        {
            try
            {
                factory.SetTimer(EncryptionManager.EncryptMessage(miliseconds));
                Console.WriteLine("Timer time set succesfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void StartTimer()
        {
            try
            {
                factory.StartTimer();
                Console.WriteLine("Timer started succesfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void StopTimer()
        {
            try
            {
                factory.StopTimer();
                Console.WriteLine("Timer stopped succesfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
