using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.Infrastructure.Utils.Log;

namespace Nofshonit.Api.Base
{
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class BaseController : ControllerBase
    {

        [FromHeader(Name = "Token")]
        public string Token { get; set; }

        public BaseController()
        {
        
        }

        public ICustomContainer Container
        {
            get
            {
                return ContainerManager.Container;
            }
        }

        public ILog LogManager
        {
            get
            {
                return Container.Resolve<ILog>();
            }
        }

        public IMapperManager MapperManager
        {
            get
            {
                return ContainerManager.Container.Resolve<IMapperManager>();
            }
        }

        public void OnBusinessException<T>( string exceptionMessage,  BaseResponse<T> response , LogDTO logItem)
        {
			bool isInternalError = false;
			foreach (char ch in exceptionMessage)
			{
				if (ch >= 'א' && ch <= 'ת')
				{
					isInternalError = true;
					break;
				}
			}
			if (!isInternalError)
				response.ErrorDescription = "שגיאה כללית";
			else
			{
                Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
                if (tagRegex.IsMatch(exceptionMessage))
                    response.ErrorHTML = exceptionMessage;
                else
                    response.ErrorDescription = exceptionMessage;
            }
            if (response.ErrorId <= 0 || response.ErrorId == null)
                response.ErrorId = 1;
         
            ApiLogger.OnException(logItem, exceptionMessage);
        }

        public void OnBusinessException<T>(BusinessException exceptionMessage, BaseResponse<T> response, LogDTO logItem)
        {
			bool isInternalError = false;
			foreach (char ch in exceptionMessage.Message)
			{
				if (ch >= 'א' && ch <= 'ת')
				{
					isInternalError = true;
					break;
				}
			}
			if (!isInternalError)
				response.ErrorDescription = "שגיאה כללית";
            else 
            { 
				Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
                if (tagRegex.IsMatch(exceptionMessage.BusinessMessage))
                    response.ErrorHTML = exceptionMessage.BusinessMessage;
                else
                    response.ErrorDescription = exceptionMessage.BusinessMessage;
            }
            if (response.ErrorId <= 0 || response.ErrorId == null)
                response.ErrorId = exceptionMessage.BusinessMessageId;

            ApiLogger.OnException(logItem, exceptionMessage.BusinessMessage);
        }

        public void OnException<T>(Exception exception, BaseResponse<T> response, LogDTO logItem)
        {
			bool isInternalError = false;
			foreach (char ch in exception.Message)
			{
				if (ch >= 'א' && ch <= 'ת')
				{
					isInternalError = true;
					break;
				}
			}
			if (!isInternalError)
				response.ErrorDescription = "שגיאה כללית";
            else { 
				response.ErrorDescription = exception.Message;
            }
            response.ErrorId = 1;

            ApiLogger.OnException(logItem, exception.ToString());
        }

        protected void OnStart(LogDTO logItem, object[] qp = null, object body = null, [CallerFilePath]string methodPath = "", [CallerMemberName] string methodName = "")
        {
            var org = Container.Resolve<IContextManager>().CurrentOrganization(); ;
            ApiLogger.OnStart(logItem, org.OrgId, qp, body, methodPath, methodName);
        }

        protected void OnEnd<T>(LogDTO logItem, BaseResponse<T> response)
        {
            response.Status = !response.ErrorId.HasValue;
            ApiLogger.OnEnd(logItem, response.Data);
        }
    }
}
