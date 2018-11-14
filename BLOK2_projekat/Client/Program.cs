using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            string address = "net.tcp://localhost:9999/WCFService";
            EndpointAddress ea = new EndpointAddress(new Uri(address));
            string name = WindowsIdentity.GetCurrent().Name;
            string authType = WindowsIdentity.GetCurrent().AuthenticationType;
            Console.WriteLine("Name: " + name);
            ClientProxy proxy = new ClientProxy(binding, ea);

            Random random = new Random();

            Alarm a = new Alarm();
            List<Alarm> listaIspis = new List<Alarm>();
            int option = 0;
            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Generate");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. DeleteAll");
                Console.WriteLine("5. EXIT");
                Console.WriteLine();
                int.TryParse(Console.ReadLine(), out option);

                switch (option)
                {
                    case 1:
                        proxy.Generate(DateTime.Now, name, ReadResourceFile.RandomMessage(), random.Next(1,1000));
                        break;
                    case 2:
                        listaIspis = proxy.Read();                    
                        foreach (Alarm al in listaIspis)
                        {
                            Console.WriteLine("{0}|{1}|{2}|{3}", al.time, al.name, al.message, al.risk);

                        }
                        break;
                    case 3:
                        proxy.Delete();
                        break;
                    case 4:
                        proxy.DeleteAll();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid input. TRY AGAIN!");
                        break;
                }

            } while (option != 5);

            Console.ReadLine();
        }
    }
}
