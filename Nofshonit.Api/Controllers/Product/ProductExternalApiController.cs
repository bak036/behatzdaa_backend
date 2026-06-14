using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nofshonit.Api.Controllers.Product
{
    [EnableCors("CorsPolicy")]
    [Route("api/product")]
    public class ProductExternalApiController : BaseController
    {
        private IProductService _service;

		public ProductExternalApiController()
        {
            _service = Container.Resolve<IProductService>();
		}

        /// <summary>
        /// Find variants by categoryId
        /// </summary>
        [HttpGet("GetVariantsByCategoryNumber")]
        public BaseResponse<List<VariantDTO>> GetVariantsByCategoryNumber([FromQuery]long categoryNumber)
        {
			var response = new BaseResponse<List<VariantDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { categoryNumber });

				var result = _service.GetVariantsByCategoryId(categoryNumber);
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
        /// Find products by categoryId
        /// </summary>
        [HttpGet("GetProductsByCategoryId")]
        public  BaseResponse<List<VariantDTO>> GetProductsByCategoryId([FromQuery]long categoryId)
        {
            var response = new BaseResponse<List<VariantDTO>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { categoryId });

                var result = _service.GetProducts(categoryId);
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
