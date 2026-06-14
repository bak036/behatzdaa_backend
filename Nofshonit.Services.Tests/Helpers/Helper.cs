using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NLog;

namespace Nofshonit.Services.Tests.Helpers
{
    public static class Helper
    {
        public static string ObjectToJson(object obj, bool withReplace = false)
        {
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            if (!string.IsNullOrEmpty(result) && withReplace)
            {
                result = result.Replace("{", "(").Replace("}",")");
            }
            return result;
        }

        public static void LogTrace(ILogger logger, TimeSpan timeSpan, string message, object obj = null)
        {
            var str = (message + " finish by " + string.Format("{1}.{2:d2} ", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds) )+ (obj != null ? ObjectToJson(obj, true) : "");
            logger.Trace(str);
        }
    }
}
