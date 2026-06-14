using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtsLogggerCP
{
    public class Logger
    {
        public static void Info(string txt) { GetInstance().Info(txt); }
        public static void Debug(string txt) { GetInstance().Debug(txt); }
        public static void Error(string txt) { GetInstance().Error(txt); }

        private static ILogger _logger = null;
        private static ILogger GetInstance()
        {
            if (_logger != null) return _logger;
            _logger = LogManager.GetCurrentClassLogger();
            return _logger;
        }

    }
}
