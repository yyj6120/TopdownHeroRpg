using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace SJ.GameServer.Utilities
{
    public class WindowEventLog
    {
        static string WindowEventLogName;
        static string WindowEventLogSourceName;

        public static void Init(string logName, string sourceName)
        {
            WindowEventLogName = logName;
            WindowEventLogSourceName = sourceName;
        }

        public static void Error(string logText,
                            [CallerFilePath] string fileName = "",
                            [CallerMemberName] string methodName = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            Write("error", logText, fileName, methodName, lineNumber);
        }

        public static void Warn(string logText,
                            [CallerFilePath] string fileName = "",
                            [CallerMemberName] string methodName = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            Write("warn", logText, fileName, methodName, lineNumber);
        }

        public static void Info(string logText,
                            [CallerFilePath] string fileName = "",
                            [CallerMemberName] string methodName = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            Write("info", logText, fileName, methodName, lineNumber);
        }



        static void Write(string logtype, string logText,
                            [CallerFilePath] string fileName = "",
                            [CallerMemberName] string methodName = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            if (WindowEventLogName == "None")
            {
                return;
            }

            //if (System.Diagnostics.EventLog.Exists("Init"))
            //{
            //    System.Diagnostics.EventLog.Delete("Init");
            //}

            string logmsg = string.Format("[{0}|{1}|{2}]: {3}", fileName, methodName, lineNumber, logText);

            // Create an instance of EventLog
            System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

            // Check if the event source exists. If not create it.
            if (!System.Diagnostics.EventLog.SourceExists(WindowEventLogSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(WindowEventLogSourceName, WindowEventLogName);
            }

            // Set the source name for writing log entries.
            eventLog.Source = WindowEventLogSourceName;

            // Create an event ID to add to the event log
            int eventID = 7;

            switch(logtype)
            {
                case "error":
                    // Write an entry to the event log.
                    eventLog.WriteEntry(logmsg, System.Diagnostics.EventLogEntryType.Error, eventID);
                    break;
                case "warn":
                    // Write an entry to the event log.
                    eventLog.WriteEntry(logmsg, System.Diagnostics.EventLogEntryType.Warning, eventID);
                    break;
                case "info":
                    // Write an entry to the event log.
                    eventLog.WriteEntry(logmsg, System.Diagnostics.EventLogEntryType.Information, eventID);
                    break;
            }
            

            // Close the Event Log
            eventLog.Close();
        }
    }
}
