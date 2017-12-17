using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace SJ.GameServer.Utilities
{
    public class VSLog
    {
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

        public static void trace(string logText,
                            [CallerFilePath] string fileName = "",
                            [CallerMemberName] string methodName = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            Write("trace", logText, fileName, methodName, lineNumber);
        }



        static void Write(string logtype, string logText,
                            [CallerFilePath] string fileName = "",
                            [CallerMemberName] string methodName = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0}- [{1}|{2}|{3}]: {4}", logtype, fileName, methodName, lineNumber, logText));
        }
    }
}
