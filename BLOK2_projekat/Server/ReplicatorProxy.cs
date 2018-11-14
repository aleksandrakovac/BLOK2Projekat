using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ReplicatorProxy : ChannelFactory<IReplicator>, IReplicator, IDisposable
    {
        IReplicator factory;

        public ReplicatorProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public List<Alarm> Replicate()
        {
            List<Alarm> alarmi = null;
           try
            {
                alarmi = factory.Replicate();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return alarmi;
        }
    }
}
