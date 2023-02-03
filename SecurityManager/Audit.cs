using System;
using System.Diagnostics;

namespace SecurityManager
{
    public class Audit : IDisposable
    {

        private static EventLog customLog = null;
        const string SourceName = "Stopwatch.Audit";
        const string LogName = "Stopwatch";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void ActionSuccesful(string userName, string action)
        {
            if (customLog != null)
            {
                string actionSuccesful = AuditEvents.ActionSuccesful;
                string message = String.Format(actionSuccesful, userName, action);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ActionSuccessful));
            }
        }

        public static void ActionFailed(string userName, string action)
        {
            if (customLog != null)
            {
                string actionFailed = AuditEvents.ActionFailed;
                string message = String.Format(actionFailed, userName, action);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ActionFailed));
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
