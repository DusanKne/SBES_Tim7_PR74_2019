using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/TimerService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(TimerService));
            host.AddServiceEndpoint(typeof(IServiceContracts), binding, address);

            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

            try
            {
                TimerService.InitiateTimer();
                host.Open();
                Console.WriteLine("Stopwatch service started successfully at: " + DateTime.Now.ToString());
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }
    }
}
