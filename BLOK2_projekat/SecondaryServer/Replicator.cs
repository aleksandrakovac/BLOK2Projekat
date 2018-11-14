using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    public class Replicator : IReplicator
    {
        string file = @"C:\Users\Administrator\Desktop\sa replikacijom\BLOK2_projekat\Server\bin\Debug\bazapodataka.txt";
       
        public List<Alarm> Replicate()
        {
            List<Alarm> buffer = new List<Alarm>();
            Alarm alarm = new Alarm();

            buffer = alarm.Deserialization(file);

            Console.WriteLine("REPLICATION!\n");
            

            if(buffer == null)
            {
                Console.WriteLine("Buffer is empty!");
            }
            else
            {
                foreach (Alarm a in buffer)
                {
                    Console.WriteLine("{0}|{1}|{2}|{3}", a.time, a.name, a.message, a.risk);

                }

            }
            alarm.Serialization(buffer);
            Audit.ReplicationDone();
            return buffer;
        }
    }
}
