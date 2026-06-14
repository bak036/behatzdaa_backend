using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Business;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nofshonit.Api.Controllers.BusinessApi
{
    [EnableCors("CorsPolicy")]
    [Route("api/Business")]
    public class BusinessExternalApiController : BaseController
    {
        private IBusinessService _service;
		Stopwatch sw = new Stopwatch();

        public BusinessExternalApiController()
        {        
            _service = Container.Resolve<IBusinessService>();
		}

        /// <summary>
        /// Get Business list by businessIds
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetBusinessByIds")]
        public BaseResponse<List<BusinessDTO>> GetBusinessByIds([FromBody] List<long> businessIds)
        {
            var response = new BaseResponse<List<BusinessDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, null,businessIds);

				sw.Restart();
                var result = _service.GetBusinessByIds(businessIds);
                response.Data = result;
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
        /// return SubBranches by businessId
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBusinessSubBranches")]
        public BaseResponse<List<BusinessSubBranchDTO>> GetBusinessSubBranches(long businessId)
        {
            var response = new BaseResponse<List<BusinessSubBranchDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { businessId});

				sw.Restart();
                var result = _service.GetBusinessSubBranches(businessId);
                response.Data = result;
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
