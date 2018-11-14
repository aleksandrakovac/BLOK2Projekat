using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    [DataContract]
    public class Alarm
    {
        public DateTime time;
        public string name;
        public string message;
        public int risk;
        public Alarm()
        { }
        public Alarm(DateTime v, string i, string p, int r)
        {
            this.time = v;
            this.name = i;
            this.message = p;
            this.risk = r;
        }

        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        [DataMember]
        public int Risk
        {
            get { return risk; }
            set { risk = value; }
        }
        public void Serialization(List<Alarm> alarm)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter("bazapodataka.txt");
                foreach (Alarm a in alarm)
                {
                    sw.WriteLine(String.Format("{0}|{1}|{2}|{3}", a.time, a.name, a.message, a.risk));
                }

                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public List<Alarm> Deserialization(string file)
        {
            string line;

            List<Alarm> alarms = new List<Alarm>();
            StreamReader sr = new StreamReader(file);

            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split('|');
                alarms.Add(new Alarm(DateTime.Parse(words[0]), words[1], words[2], Int32.Parse(words[3])));
            }

            sr.Close();
            return alarms;
        }
    }
}
