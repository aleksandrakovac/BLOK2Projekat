using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
   public class ClientProxy : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        IWCFService factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

       public bool Delete()
        {
            bool end = false;
            try
            {
                if(factory.Delete())
                    Console.WriteLine("{0}", ReadResourceFile.SuccessfullyDoneFunction(Permissions.Delete.ToString()));
                else
                    Console.WriteLine("{0}", ReadResourceFile.UnsuccessfullyDoneFunction(Permissions.Delete.ToString(), Permissions.Delete.ToString()));
                end = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return end;
        }

        public bool DeleteAll()
        {
            bool end = false;

                try
                {
                    if (factory.DeleteAll())
                        Console.WriteLine("{0}", ReadResourceFile.SuccessfullyDoneFunction(Permissions.DeleteAll.ToString()));
                    else
                        Console.WriteLine("{0}", ReadResourceFile.UnsuccessfullyDoneFunction(Permissions.DeleteAll.ToString(), Permissions.DeleteAll.ToString()));
                    end = true;
                }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return end;
        }

        public bool Generate(DateTime dateTime, string name, string msg, int risk)
        {
            bool end = false;
            try
            {
               if(factory.Generate(dateTime, name, msg, risk))
                    Console.WriteLine("{0}", ReadResourceFile.SuccessfullyDoneFunction(Permissions.Generate.ToString()));
               else
                    Console.WriteLine("{0}", ReadResourceFile.UnsuccessfullyDoneFunction(Permissions.Generate.ToString(), Permissions.Generate.ToString()));
                end = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            return end;
        }

        public List<Alarm> Read()
        {
            List<Alarm> alarms = new List<Alarm>();

            try
            {
                if ((alarms = factory.Read()).Count != 0)
                    Console.WriteLine("{0}", ReadResourceFile.SuccessfullyDoneFunction(Permissions.Read.ToString()));

                else
                    Console.WriteLine("Database is empty.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            return alarms;

        }

        
    }
}
