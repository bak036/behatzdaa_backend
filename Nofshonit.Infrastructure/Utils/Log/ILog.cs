using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Infrastructure.Utils.Log
{
    public interface ILog
    {
        void Init(ILoggerFactory loggerFactory);
        void Info(string body);
        void Info(string body, object obj);

        void Debug(string body);
        void Debug(string body, object obj);

        void Error(string body, Exception e);
        void Error(string body, object obj, Exception e);
        void Error(string body);
    }
}
