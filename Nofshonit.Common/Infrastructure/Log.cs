using Microsoft.Extensions.Logging;
using Nofshonit.Common.DTOs;
using Nofshonit.Infrastructure.Utils;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.Common.EF.DTS_Logs;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Nofshonit.Logs;

namespace Nofshonit.Common.Infrastructure
{
    public class Log1 : ILog
    {

        private ILogger _logger;



        public void Init(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }
        public void Debug(string body)
        {
            if (_logger != null)
                _logger.LogDebug(body);
        }

        public void Debug(string body, object obj)
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
                LoggerHelper.Error(body + objString, e);
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

        public static void OnStart(LogDTO logItem, int orgId, object[] qp = null, object body = null, [CallerFilePath]string methodPath = "", [CallerMemberName]string methodName = "")
        {
            int startIndex = methodPath.IndexOf("Nofshonit.Api");
            int length = methodPath.Length - 3 - startIndex; // -3 for ".cs"
            string fullMethodName = methodPath.Substring(startIndex, length);
            logItem.MethodName = $"{fullMethodName}.{methodName}";
            logItem.OrganizationId = orgId;
            logItem.ClientIp = ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            logItem.ServerIp = ServiceIpAddress();
            logItem.StartDate = DateTime.Now;
            logItem.Body = body == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(body);
            logItem.QueryParams = qp == null ? null : string.Join(",", qp);
        }

        public static void OnException(LogDTO logItem, string exception)
        {
            logItem.Exception = exception;
        }

        public static void OnEnd(LogDTO logItem, object response)
        {
            logItem.Response = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            logItem.EndDate = DateTime.Now;
            var ApiLogToAdd = ContainerManager.Container.Resolve<IMapperManager>().Map(logItem, typeof(ApiLogs));
           // ContainerManager.Container.Resolve<IDtsLogsRepo>().AddLog(ApiLogToAdd);
        }

        private static string ServiceIpAddress()
        {
         
                IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());                
                    return heserver.AddressList[1].ToString();
      
            
        }
    }
}
