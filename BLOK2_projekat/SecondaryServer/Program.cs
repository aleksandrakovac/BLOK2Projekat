using Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            string address = "net.tcp://localhost:10000/Replicator";
           
            ServiceHost replicator = new ServiceHost(typeof(Replicator));
            replicator.AddServiceEndpoint(typeof(IReplicator), binding, address);

            replicator.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            replicator.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            replicator.Open();

            Console.WriteLine("Secondary server service is started.");
            Console.WriteLine("Press <enter> to stop service...");

            Console.ReadLine();
            replicator.Close();
        }
    }
}
