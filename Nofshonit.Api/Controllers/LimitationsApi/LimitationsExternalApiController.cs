using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nofshonit.Api.Controllers.LimitationsApi
{
    [Route("api/Limitations")]
    [ApiController]
    public class LimitationsExternalApiController : BaseController
    {
        private readonly ILimitationsService _service;

		public LimitationsExternalApiController()
        {
            _service = Container.Resolve<ILimitationsService>();
		}

        [HttpGet("ValidatePurchesAllowed")]
        public BaseResponse<List<VariantOrderLimitDTO>> ValidatePurchesAllowed()
        {
            var response = new BaseResponse<List<VariantOrderLimitDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);

                response.Data =  _service.ValidatePurchesAllowed();
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

        /// <summary>
        /// check variants stock by categoryId or barcodes
        /// </summary>
        [HttpPost("GetProductStockStatus")]
        public BaseResponse<Dictionary<string, bool>>  GetProductStockStatus([FromBody]VariantStockRequestDTO request)
        {
            var response = new BaseResponse<Dictionary<string, bool>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                response.Data = _service.GetProductStockStatus(request.CategoryId, request.Barcodes, request.Qty.GetValueOrDefault(1));
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
