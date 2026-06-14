using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860 

namespace Nofshonit.Api.Controllers.CategoryApi
{
    [EnableCors("CorsPolicy")]
    [Route("api/category")]
    public class CategoryExternalApiController : BaseController
    {
        private ICategoryService _service;
		
		Stopwatch sw = new Stopwatch();

        public CategoryExternalApiController()
        {
            _service = Container.Resolve<ICategoryService>();
		}

        /// <summary>
        /// return Category details like descriprion name images children...
        /// </summary>
        [HttpGet("GetCategoryById")]
        public BaseResponse<CategoryDetailsDTO> GetCategoryById(int categoryId)
        {
            var response = new BaseResponse<CategoryDetailsDTO>();
			var logItem = new LogDTO();

			try
			{
                sw.Restart();
                OnStart(logItem, new object[] { categoryId });

				
                var result = _service.GetCategoryDetails(categoryId);
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
        /// return a menu of application
        /// all categories in first parent level (category that fatherId = 0)
        /// </summary>
        //[AllowAnonymous]
        [HttpGet("GetCategoryHeader")]
        public BaseResponse<List<CategoryHeaderDTO>> GetCategoryHeader()
        {
            var response = new BaseResponse<List<CategoryHeaderDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);
				sw.Restart();

                var result = _service.GetCategoryHeader();
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
        /// Benefit (category without children)
        /// get Category details with ProductVars/Events
        /// </summary>
        [HttpGet("GetCategoryProducts")]
        public BaseResponse<CategoryDetailsDTO> GetCategoryProducts(long categoryId)
        {
            var response = new BaseResponse<CategoryDetailsDTO>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { categoryId });
				sw.Restart();

                var result = _service.GetCategoryProducts(categoryId);
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
