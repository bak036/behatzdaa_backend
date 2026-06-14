using Microsoft.AspNetCore.Http;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.Common.EF.DTS_Logs;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.Repositories.DtsLogsModel;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using Nofshonit.Infrastructure.Utils;

namespace Nofshonit.BL.BLHelper
{
    public class ApiLoggerBL
    {
        private static string _serviceIpAddress;
        private static string _logLevel = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DBLogLevel);
        private static string _logLevelMethods = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DBLogLevelMethods);


        public static void OnStart(LogDTO logItem, object[] body, string methodName)
        {
            logItem.LogType = (int)LogType.ThirdParty;
            logItem.OrganizationId = ContainerManager.Container.Resolve<IContextManager>().CurrentOrganization().OrgId;
            if (!_logLevel.Contains("OnStart"))
                return;
            if (string.IsNullOrWhiteSpace(methodName) || !_logLevelMethods.Contains(methodName))
                return;
            logItem.MethodName = methodName;
            logItem.ClientIp = ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            logItem.ServerIp = ServiceIpAddress;
            logItem.StartDate = DateTime.Now;
            logItem.Body = body[0].ToString();
            logItem.QueryParams = body.Length > 1 ? Newtonsoft.Json.JsonConvert.SerializeObject(body[1]) : null;
        }

        public static void OnException(LogDTO logItem, string exception)
        {
            if (!_logLevel.Contains("OnException"))
                return;
            logItem.Exception = exception;
        }

        public static void OnEnd(LogDTO logItem, object response)
        {

            if (_logLevel.Contains("OnEnd"))
            {
                logItem.EndDate = DateTime.Now;
                logItem.Response = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            if (((_logLevel.Contains("OnEnd") || _logLevel.Contains("OnStart")) && !string.IsNullOrWhiteSpace(logItem.MethodName) && _logLevelMethods.Contains(logItem.MethodName)) || logItem.Exception != null)
            {
                var ApiLogToAdd = ContainerManager.Container.Resolve<IMapperManager>().Map(logItem, typeof(ApiLogs));
                ContainerManager.Container.Resolve<IDtsLogsRepo>().AddLog(ApiLogToAdd);
            }
        }

        public static void Log(string methodName, string body, string qp)
        {
            var log = new LogDTO
            {
                MethodName = methodName,
                Body = body,
                QueryParams = qp,
                LogType = 5,
                OrganizationId = ContainerManager.Container.Resolve<IContextManager>().CurrentOrganization().OrgId,
                ClientIp = ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                ServerIp = ServiceIpAddress,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            };
            var ApiLogToAdd = ContainerManager.Container.Resolve<IMapperManager>().Map(log, typeof(ApiLogs));
            ContainerManager.Container.Resolve<IDtsLogsRepo>().AddLog(ApiLogToAdd);
        }

        public static void LogConnectorData(string memberId, [CallerMemberName] string methodName = "")
        {
            string clientIp = ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Request.Headers.GetUserIP();

            string msgBody = $"[Method]{methodName}-[IP]{clientIp}-[MemberId]{memberId}";

            var log = new LogDTO
            {
                MethodName = methodName,
                Body = msgBody,
                QueryParams = null,
                LogType = 5,
                OrganizationId = ContainerManager.Container.Resolve<IContextManager>().CurrentOrganization().OrgId,
                ClientIp = clientIp,
                ServerIp = ServiceIpAddress,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            };

            var ApiLogToAdd = ContainerManager.Container.Resolve<IMapperManager>().Map(log, typeof(ApiLogs));
            ContainerManager.Container.Resolve<IDtsLogsRepo>().AddLog(ApiLogToAdd);
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
