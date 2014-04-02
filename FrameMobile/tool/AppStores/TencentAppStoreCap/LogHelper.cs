using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace TencentAppStoreCap
{
    public class LogHelper
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
    }
}
