using Nofshonit.Common.DTOs;
using System;
using System.Runtime.CompilerServices;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.Repositories.DtsLogsModel;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.Common.EF.DTS_Logs;
using System.Net;
using Microsoft.AspNetCore.Http;
using Nofshonit.Common.Constants;
using Nofshonit.Common;
using System.Linq;
using Nofshonit.Infrastructure.Utils.Log;
using Microsoft.Extensions.Logging;
using Nofshonit.Infrastructure.Utils;
using System.Collections.Generic;
using Nofshonit.Logs;

namespace Nofshonit.Api.Controllers.Base
{
    public class ApiLogger : ILog
    {
        private ILogger _logger;
        private static string _serviceIpAddress;
        private static string _logLevel = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DBLogLevel);
        private static string _logLevelMethods = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DBLogLevelMethods);

        public void Init(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }
        public void Debug(string body)
        {

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

            _logger.LogDebug(body + objString);
        }
        public void Error(string body, Exception e)
        {
            _logger.LogError(e, body);
            LoggerHelper.Error(body, e);
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
            _logger.LogError(body);
            LoggerHelper.Error(body);
        }
        public void Info(string body)
        {
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
            _logger.LogInformation(body + objString);
        }


        public static void OnStart(LogDTO logItem, int orgId, object[] qp = null, object body = null, [CallerFilePath]string methodPath = "", [CallerMemberName]string methodName = "")
        {
            logItem.LogType = (int)LogType.API;
            logItem.OrganizationId = orgId;
            if (!_logLevel.Contains("OnStart"))
                return;
            if (string.IsNullOrWhiteSpace(methodName) || !_logLevelMethods.Contains(methodName) )
                return;
            if (string.IsNullOrWhiteSpace(methodPath))
            {
                logItem.MethodName = methodName;
            }
            else
            {
                int startIndex = methodPath.IndexOf("Nofshonit.Api");
                int length = methodPath.Length - 3 - startIndex; // -3 for ".cs"
                string fullMethodName = methodPath.Substring(startIndex, length);
                logItem.MethodName = $"{fullMethodName}.{methodName}";
            }
            logItem.ClientIp = ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            logItem.ServerIp = ServiceIpAddress;
            logItem.StartDate = DateTime.Now;
            logItem.Body = body == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(body);
            logItem.QueryParams = qp == null ? null : string.Join(",", qp);
        }

        public static void OnException(LogDTO logItem, string exception)
		{
            if (!_logLevel.Contains("OnException"))
                return;
            logItem.Exception = exception;
            LoggerHelper.Error(exception, logItem);
        }

		public static void OnEnd(LogDTO logItem, object response)
		{
            if (_logLevel.Contains("OnEnd") && logItem.MethodName != null && _logLevelMethods.Contains(logItem.MethodName))
            {
                logItem.Response = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                logItem.EndDate = DateTime.Now;
            }
            if (((_logLevel.Contains("OnEnd") || _logLevel.Contains("OnStart")) && !string.IsNullOrWhiteSpace(logItem.MethodName) && _logLevelMethods.Contains(logItem.MethodName)) || logItem.Exception != null) 
            {
                var ApiLogToAdd = ContainerManager.Container.Resolve<IMapperManager>().Map(logItem, typeof(ApiLogs));
                ContainerManager.Container.Resolve<IDtsLogsRepo>().AddLog(ApiLogToAdd);
            }
        }

		private static string ServiceIpAddress
		{
			get
			{
				IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
				if (string.IsNullOrWhiteSpace(_serviceIpAddress))
					_serviceIpAddress = heserver.AddressList[1].ToString();
				return _serviceIpAddress;
			}
		}
	}
}
