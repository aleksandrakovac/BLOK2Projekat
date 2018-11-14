using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class WCFService : IWCFService
    {
        ReplicatorProxy proxy = Replication();
        static ReplicatorProxy Replication() 
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            string address = "net.tcp://localhost:10000/Replicator";
            EndpointAddress ea = new EndpointAddress(new Uri(address));
            string name = WindowsIdentity.GetCurrent().Name;
            string authType = WindowsIdentity.GetCurrent().AuthenticationType;

            Console.WriteLine("Name proxy replicator: " + name);
            //Console.WriteLine("Type: " + authType);

            ReplicatorProxy proxy = new ReplicatorProxy(binding, ea);

            return proxy;

        }

        static List<Alarm> alarms = new List<Alarm>();
        string file = "bazapodataka.txt";
        public bool Delete()
        {
            bool end = false;
            CustomPrincipal cp = Thread.CurrentPrincipal as CustomPrincipal;            
            Alarm alarm = new Alarm();

            if (cp.IsInRole(Permissions.Delete.ToString()))
            {
                Console.WriteLine("Deleting alarm!\n");

                List<Alarm> a;
                a = alarm.Deserialization(file);
                string alarmName;
                string name = cp.Identity.Name.ToString();
                name = Formatter.ParseName(name.ToString());
                List<Alarm> newL = new List<Alarm>();

                foreach (Alarm al in a)
                {
                    alarmName = Formatter.ParseName(al.name.ToString());
                    while (alarmName.Equals(name))
                    {
                        newL.Add(al);                            
                        break;
                    }
                }

                foreach(Alarm user in newL)
                {
                    a.Remove(user);
                    alarms.Remove(user);
                }

                alarm.Serialization(a);

                Console.WriteLine("Deleting alarm is done!\n");
                end = true;   
            }
            else
            {
                Console.WriteLine("Deleting alarm not successed because of wrong permission!\n");
            }

            return end;
        }

        public bool DeleteAll()
        {
            bool end = false;
            Alarm alarm = new Alarm();

            CustomPrincipal cp = Thread.CurrentPrincipal as CustomPrincipal;

            if (cp.IsInRole(Permissions.DeleteAll.ToString()))
            {
                Console.WriteLine("Deleting all alarms...\n");

                alarms.Clear();
                alarm.Serialization(alarms);

                Console.WriteLine("Deleting all alarms is done!\n");
                end = true;
            }
            else
            {
                Console.WriteLine("Deleting all not successed because of wrong permission!\n");
            }

            return end;
        }

        public bool Generate(DateTime dateTime, string name, string message, int risk)
        {
            bool end = false;
            Alarm a = new Alarm();
            List<Alarm> alarmReplicator = new List<Alarm>();

            CustomPrincipal cp = Thread.CurrentPrincipal as CustomPrincipal;
      
            if (cp.IsInRole(Permissions.Generate.ToString()))
            {
                Console.WriteLine("Generating alarm...\n");
                Alarm alarm = new Alarm(dateTime, name, message, risk);
                alarms.Add(alarm);           
                alarm.Serialization(alarms);
                Audit.ReplicationInitialized();
                alarmReplicator = proxy.Replicate();
                alarmReplicator.Clear();

                Console.WriteLine("Generating is done!\n");

                end = true;
            }
            else
            {
                Console.WriteLine("Generating not successed because of wrong permission!\n");
            }

            return end;
        }
            
        public List<Alarm> Read()
        {
            CustomPrincipal cp = Thread.CurrentPrincipal as CustomPrincipal;
            List<Alarm> alarmi = new List<Alarm>();
            Alarm a = new Alarm();

            if(cp.IsInRole(Permissions.Read.ToString()))
            {
                Console.WriteLine("Reading...\n");

                alarmi = a.Deserialization(file);
                //za ispis na konzolu
                foreach(Alarm al in alarmi)
                {
                    Console.WriteLine("{0}|{1}|{2}|{3}", al.time, al.name, al.message, al.risk);
                }
                Console.WriteLine("Reading is done!\n");
          
            }
            else
            {
                Console.WriteLine("Reading not successed because of wrong permission!\n");
            }

            return alarmi;
        }

    }
}
