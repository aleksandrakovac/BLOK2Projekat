using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Common.Audit";
        const string LogName = "MojFajl";

        static Audit()
        {
            try
            {
                if (!EventLog.Exists(LogName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
                Console.WriteLine("Created custom logs");
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void ReplicationInitialized()
        {
            if (customLog != null)
            {
                string message = string.Format(AuditEventFile.ReplicationInitialized);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)ReplicationTypes.ReplicationInitialized));
            }
        }

        public static void ReplicationDone()
        {
            if (customLog != null)
            {
                string message = string.Format(AuditEventFile.ReplicationDone);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)ReplicationTypes.ReplicationDone));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
