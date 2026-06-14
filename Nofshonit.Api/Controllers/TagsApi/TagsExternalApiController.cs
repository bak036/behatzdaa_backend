using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Tags;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Nofshonit.Api.Controllers.TagsApi
{

    [EnableCors("CorsPolicy")]
    //[AllowAnonymous]
    [Route("api/tags")]
    [ApiController]
    public class TagsExternalApiController : BaseController
    {
        private ITagsService _service;
      
		public TagsExternalApiController()
        {
            _service = Container.Resolve<ITagsService>();
		}
        //[HttpGet("GetTagsBusiness")]
        //[HttpGet("GetTagsCategory")]

        [HttpGet("GetCategorysByTagID")]
        public BaseResponse<TagsCategoriesDTO> GetCategorysByTagID(int tagId)
        {
            var response = new BaseResponse<TagsCategoriesDTO>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { tagId });

				response.Data = _service.GetCategorysByTagID(tagId);
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

        [HttpGet("GetCategorysByTopTag")]
        public async Task<BaseResponse<List<TagsCategoriesDTO>>> GetCategoryByTags(int selectTop, int skipTags = 0)
        {

            var response = new BaseResponse<List<TagsCategoriesDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { selectTop });

				response.Data = await _service.GetCategoryByTags(selectTop,skipTags);
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