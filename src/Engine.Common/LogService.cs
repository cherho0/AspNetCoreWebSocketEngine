using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Engine.Common
{
    public static class LogService
    {
        private static ILogger _log;
        public static void Init(ILogger createLogger)
        {
            _log = createLogger;
        }

        public static void LogInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(info);
            _log.LogInformation(info);
        }

        public static void LogError(string info)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(info);
            _log.LogError(info);
        }
    }
}
