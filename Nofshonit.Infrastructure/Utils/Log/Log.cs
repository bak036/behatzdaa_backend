using Microsoft.Extensions.Logging;
using Nofshonit.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Infrastructure.Utils.Log
{
    public class Log : ILog
    {

        private ILogger _logger;

     

        public void Init(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }
        public void Debug(string body)
        {
            if(_logger != null)
            _logger.LogDebug(body);         
        }

        public void Debug(string body, object obj)
        {
            var objString = "";
            if(obj != null)
            {
                objString = " object:";
                try {
                    objString += SerializerHelper.JsonObjectToString(obj);
                }
                catch { objString += obj.ToString(); }
            }
            if (_logger != null)
                _logger.LogDebug(body + objString);          
        }

        public void Error(string body, Exception e)
        {
            if (_logger != null)
            {
                _logger.LogError(e, body);
                LoggerHelper.Error(e, body);
            }
        }
        public void Error(string body, object obj, Exception e)
        {
            var objString = "";
            if (obj != null)
            {
                objString = " object:";
                try
                {
                    objString += SerializerHelper.JsonObjectToString(obj);
                }
                catch { objString += obj.ToString(); }
            }
            if (_logger != null)
            {
                _logger.LogError(body + objString, e);
                LoggerHelper.Error(body +  objString, e);
            }
        }
        public void Error(string body)
        {
            if (_logger != null)
            {
                _logger.LogError(body);
                LoggerHelper.Error(body);
            }
        }

        public void Info(string body)
        {
            if (_logger != null)
                _logger.LogInformation(body);           
        }

        public void Info(string body, object obj)
        {
            var objString = "";
            if (obj != null)
            {
                objString = " object:";
                try
                {
                    objString += SerializerHelper.JsonObjectToString(obj);
                }
                catch { objString += obj.ToString(); }
            }
            if (_logger != null)
                _logger.LogInformation(body + objString);           
        }
    }
}
