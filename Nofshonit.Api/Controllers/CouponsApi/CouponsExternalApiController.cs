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
using Nofshonit.Common.DTOs.Coupon;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils;
using Nofshonit.Infrastructure.Utils.Log;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nofshonit.Api.Controllers.CouponsApi
{
    [EnableCors("CorsPolicy")]
    [Route("api/coupons")]
    public class CouponsExternalApiController : BaseController
    {        
        private ICouponService _couponService;

		public CouponsExternalApiController()
        {
            _couponService = Container.Resolve<ICouponService>();
		}

        /// <summary>
        /// calculate Coupon Discount
        /// </summary>
        /// <param name="couponCode"></param>
        /// <param name="price">price that need to apply the Discount</param>
        /// <returns></returns>
        [HttpGet("GetCouponDiscount")]
        public BaseResponse<CouponDiscountDTO> GetCouponDiscount([FromQuery] string couponCode, [FromQuery] decimal price)
        {
            BaseResponse<CouponDiscountDTO> response = new BaseResponse<CouponDiscountDTO>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { couponCode,price });
                response.Data = _couponService.GetCouponDiscount(couponCode, price);
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
