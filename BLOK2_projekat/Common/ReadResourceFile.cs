using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum AuditEventTypes
    {
        SuccessfullyDoneFunction = 0,
        UnsuccessfullyDoneFunction = 1
    }

    public enum ReplicationTypes
    {
        ReplicationInitialized = 0,
        ReplicationDone = 1
    }
    public enum MessageTypes
    {
        Message1 = 0,
        Message2 = 1,
        Message3 = 2,
        Message4 = 3,
        Message5 = 4
    }

    public class ReadResourceFile
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager(typeof(AuditEventFile).FullName, Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string UnsuccessfullyDoneFunction(string serviceName, string reason)
        {
            string x = ResourceMgr.GetString(AuditEventTypes.UnsuccessfullyDoneFunction.ToString());
            string message = string.Format(x, serviceName, reason);

            return message;
        }

        public static string SuccessfullyDoneFunction(string serviceName)
        {
            string x = ResourceMgr.GetString(AuditEventTypes.SuccessfullyDoneFunction.ToString());
            string message = string.Format(x, serviceName);

            return message;
        }

        public static string RandomMessage()
        {
            Random random = new Random();
            int i = random.Next(1,5);

            switch (i)
            {
                case (1):
                    return ResourceMgr.GetString(MessageTypes.Message1.ToString());
                case 2:
                    return ResourceMgr.GetString(MessageTypes.Message2.ToString());
                case 3:
                    return ResourceMgr.GetString(MessageTypes.Message3.ToString());
                case 4:
                    return ResourceMgr.GetString(MessageTypes.Message4.ToString());
                case 5:
                    return ResourceMgr.GetString(MessageTypes.Message5.ToString());
                default:
                    return String.Empty;

            }         

        }

    }
}
