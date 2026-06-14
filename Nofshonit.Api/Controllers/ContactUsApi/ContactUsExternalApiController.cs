using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Nofshonit.Api.Controllers.ContactUsApi
{
    [Route("api/contact")]
    [ApiController]
    public class ContactUsExternalApiController : BaseController
    {
		private readonly IContactUsService _service;

		public ContactUsExternalApiController()
		{
			_service = Container.Resolve<IContactUsService>();
		}

		[HttpPost]
        //[AllowAnonymous]
        [Route("openService")]
        public async Task<BaseResponse<object>> OpenServiceCaseRequest([FromBody]ContactUsDTO contactUsDTO)
        {

			var response = new BaseResponse<object>();
			var logItem = new LogDTO();

			try
			{
			  OnStart(logItem, null, contactUsDTO);
                response = await _service.OpenServiceCaseRequest(contactUsDTO);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }

            return response;
		}

        [HttpGet]
        //[AllowAnonymous]
        [Route("getCrmTypes")]
        public BaseResponse<List<CrmGetTypeDTO>> GetCrmTypes()
        {
			var response = new BaseResponse<List<CrmGetTypeDTO>>();

			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);
                response.Data = _service.GetCrmTypes();
			}
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }

            return response;
        }
    }
}
