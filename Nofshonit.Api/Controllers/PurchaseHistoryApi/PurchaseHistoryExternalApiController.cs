using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Api.Controllers.PurchaseHistoryApi
{
    [EnableCors("CorsPolicy")]
    [Authorize]
    [Route("api/purchases")]
    [ApiController]
    public class PurchaseHistoryExternalApiController : BaseController
    {
		private readonly IPurchaseService _service;

		public PurchaseHistoryExternalApiController()
		{
			_service = Container.Resolve<IPurchaseService>();
		}



        [HttpPost]
        [Route("purchaseHistory")]
        public async Task<BaseResponse<PurchaseHistoryResponseDTO>> PurchaseHistory([FromBody]MemberHistoryDTO memberHistoryDTO)
        {

            var response = new BaseResponse<PurchaseHistoryResponseDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, memberHistoryDTO);

                var res = await _service.PurchaseHistory(memberHistoryDTO);
                response.Data = res;

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
        [Route("purchaseHistoryOld")]
        public BaseResponse<string> PurchaseHistoryOld()
        {

			var response = new BaseResponse<string>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);

				string res = _service.PurchaseHistoryOld();
				response.Data = res;

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


        [AllowAnonymous]
        [HttpGet]
        [Route("SendOldOrdersLink")]
        public async Task<BaseResponse<string>> SendOldOrdersLinkToUnauthorizedUser([FromQuery] string member )
        {

            var response = new BaseResponse<string>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, member);
                var res = await _service.SendOldOrdersLinkToUnauthorizedUser(member);
                response.Data = res;

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
