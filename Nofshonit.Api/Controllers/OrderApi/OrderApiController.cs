using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using static Nofshonit.Common.DTOs.ResponseDTOs.CancelResponseDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860   

namespace Nofshonit.Api.Controllers.OrderApi
{

    [Route("api/order")]
    [ApiController]
    public class OrderApiController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderApiController()
        {
            _orderService = Container.Resolve<IOrderService>();
        }

        /// <summary> 
        /// Purchase API 
        /// </summary> 
        /// <param name="request"></param> 
        /// <returns></returns> 
        [HttpPost("purchase")]
        public async Task<BaseResponse<PurchaseDTO>> Purchase(PurchaseRequestDTO request)
        {
            var result = new BaseResponse<PurchaseDTO>();
            var logItem = new LogDTO();
            try
            {
                DtsLoggger.Logger.Info("Entering Purchase OrderApiController");

                OnStart(logItem, body: request, methodPath:"", methodName: "Purchase");
                PurchaseResponseDTO responseDTO = await _orderService.Purchase(request);
                result.Status = responseDTO.Status == 0 ? false : true;
                result.ErrorDescription = responseDTO.ErrorDescription;
                result.ErrorId = responseDTO.ErrorId;
                logItem.QueryParams = responseDTO.Log;
                if (!result.Status)
                {
                    throw new BusinessException(result.ErrorDescription);
                }
                else
                {
                    result.Data = responseDTO.Data;
                    // Already Called at OrderBL:141, Commented because it causes an error for zero price purchase
                    //Container.Resolve<IShopingBasketService>().RemoveAllProducts();
                }
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, result, logItem);
            }
            catch (Exception e)
            {
                OnException(e, result, logItem);
            }
            finally
            {
                OnEnd(logItem, result);
            }

          return result;
        }

        [HttpGet("IsCancelRequriedAproove")]
        public BaseResponse<bool> IsCancelRequriedAproove(string barCode)
        {
            var result = new BaseResponse<bool>() { Status=true,Data=false};
            var logItem = new LogDTO(); 
            try
            {
                result.Data= _orderService.IsCancelRequriedAproove(barCode);
            }
            catch (Exception e )
            {
                OnException(e, result, logItem);

            } 
            return result;
        }

        /// <summary> 
        /// Single variant cancelation API 
        /// </summary> 
        /// <param name="request"></param> 
        /// <returns></returns> 
        [HttpPost("cancel")]
        public async Task<BaseResponse<CancelDataDTO>> Cancel(CancelRequestDTO request)
        {

            var result = new BaseResponse<CancelDataDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, body: request, methodPath: "", methodName: "Cancel");
       
                CancelResponseDTO responseDTO = await _orderService.Cancel(request);
                result.Status = responseDTO.Status == 0 ? false : true;
                result.ErrorDescription = responseDTO.ErrorDescription;
                result.ErrorId = responseDTO.ErrorId;
                logItem.QueryParams = responseDTO.Log;
                if (!result.Status)
                {
                    throw new BusinessException(result.ErrorDescription);
                }
                result.Data = responseDTO.Data;
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, result, logItem);
            }
            catch (Exception e)
            {
                OnException(e, result, logItem);
            }
            finally
            {
                OnEnd(logItem, result);
            }

            return result;

        }
  

        [HttpPost("getConfirmationPage")]
        public async Task<BaseResponse<string>> GetConfirmationPage(ConfirmationPageDTO request)
        {
            var result = new BaseResponse<string>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                result.Data = await _orderService.GetConfirmationPage(request);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, result, logItem);
            }
            catch (Exception e)
            {
                OnException(e, result, logItem);
            }
            finally
            {
                OnEnd(logItem, result);
            }

            return result;
        }
		[HttpGet("GetPopupBarcodeDetails")]
		public async Task<BaseResponse<BarcodePopupDetails>> GetPopupBarcodeDetails(string asmachta)
		{
			var result = new BaseResponse<BarcodePopupDetails>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);
				result.Data = await _orderService.GetPopupBarcodeDetails(asmachta);
			}
			catch (BusinessException be)
			{
				OnBusinessException(be.Message, result, logItem);
			}
			catch (Exception e)
			{
				OnException(e, result, logItem);
			}
			finally
			{
				OnEnd(logItem, result);
			}

			return result;
		}
		[HttpPost("test")]
        public void Test()
        {

            Container.Resolve<IOrderBL>().Test2();

        }
    }
}