using Client;
using Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Message; 
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; 

            string address = "net.tcp://localhost:9999/WCFService";
            ServiceHost host = new ServiceHost(typeof(WCFService));
            host.AddServiceEndpoint(typeof(IWCFService), binding, address);

            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager(); 
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;


            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

            host.Open();
            
            Console.WriteLine("Server service is started.");
            Console.WriteLine("Press <enter> to stop service...");


            string name = WindowsIdentity.GetCurrent().Name;
            string authType = WindowsIdentity.GetCurrent().AuthenticationType;

            Console.WriteLine("Name: " + name);


            Console.ReadLine();
            host.Close();
        }

    }
}

