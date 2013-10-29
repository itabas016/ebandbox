using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace FrameMobile.Core
{
    public class NLogHelper
    {
        public static void WriteInfo(string content, ConsoleColor color = ConsoleColor.Gray)
        {

            Console.ForegroundColor = color;
            Console.WriteLine(content);

            LogManager.GetLogger("InfoLogger").Info(content);

        }

        public static void WriteError(string content)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(content);

            LogManager.GetLogger("ErrorLogger").Error(content);

        }

        public static void WriteTrace(string content)
        {
            LogManager.GetLogger("TraceLogger").Trace(content);
        }
    }
}
