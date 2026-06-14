using DtsLoggger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Logs
{
    public static class LoggerHelper
    {
        private static readonly Serilog.ILogger _logger = Serilog.Log.ForContext("SourceContext", "Logger");

        public static void InfoSeq(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            DtsLoggger.Logger.Info(message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            DtsLoggger.Logger.Debug(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            DtsLoggger.Logger.Error(message, args);
            _logger.Error(message, args);
        }

        public static void Fatal(string message, params object[] args)
        {
            DtsLoggger.Logger.Fatal(message, args);
        }

        public static void Error(Exception exp, string message, params object[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(message, args);
            stringBuilder.AppendLine();
            stringBuilder.Append(exp.GetCompleteStacktrace());
            DtsLoggger.Logger.Error(exp, message, args);
            _logger.Error(exp, message, args);
        }
    }
}
