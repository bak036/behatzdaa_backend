using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Net;

namespace Nofshonit.Api.Controllers.SearchApi
{
    [Route("api/search")]
    [ApiController]
    public class SearchExternalAPiController : BaseController
    {
        private ISearchService _service;

		public SearchExternalAPiController()
        {
            _service = Container.Resolve<ISearchService>();
		}

        [HttpGet("GetSearchData")]
        public  BaseResponse<List<CategoryDetailsDTO>> GetSearchData([FromQuery]string text, [FromQuery]int selectTop, [FromQuery]long superCategory, [FromQuery]string region)
        {
            var response = new BaseResponse<List<CategoryDetailsDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { text, selectTop, superCategory, region });

				response.Data = _service.GetSearchData(text,selectTop, superCategory, region);
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


        [HttpGet("GetAutoCompleteResults")]
        public BaseResponse<List<GetAutoCompleteResultsDTO>> GetAutoCompleteResults([FromQuery]string text, [FromQuery]int selectTop)
        {
            var response = new BaseResponse<List<GetAutoCompleteResultsDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { text, selectTop});

				response.Data = _service.GetAutoCompleteResults(text, selectTop);
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

