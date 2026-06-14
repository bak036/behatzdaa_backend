using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nofshonit.Common;

namespace Nofshonit.Api.Controllers.CardsApi
{
    [Route("api/cards")]
    [ApiController]
    public class CardsExternalApiController : BaseController
    {
        private ICardsService _service;

        public CardsExternalApiController()
        {
            _service = Container.Resolve<ICardsService>();
        }
        /// <summary>
        /// The method return general card info base on user TZ
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCardGeneralInfo")]
        public async Task<BaseResponse<CardDTO>> GetCardGeneralInfo()
        {
            var response = new BaseResponse<CardDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                response.Data = await _service.GetCardGeneralInfo();
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
        /// The method perform loading to a wallet credit card charge by full payer info or by pincode.
        /// there is an aption to save payer data for quick charge.
        /// </summary>
        /// <param name="payerData"></param>
        /// <returns></returns>
        [HttpPost("LoadWallet")]
        public async Task<BaseResponse<string>> LoadWallet([FromBody]PayerDataDTO payerData)
        {
            var response = new BaseResponse<string>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, payerData);

                response.Data = await _service.LoadWallet(payerData);
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
        /// The method returns wallet info this method include in GetCardGeneralInfo too
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        [HttpGet("GetWalletInfo")]
        public async Task<BaseResponse<WalletDTO>> GetWalletInfo([FromQuery]string walletId)
        {

            var response = new BaseResponse<WalletDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, walletId);

                response.Data = await _service.GetWalletInfo(walletId);
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


        [HttpGet("GetCardActivities")]
        public async Task<BaseResponse<CardActivitiesDTO>> GetCardActivities()
        {
            
            var response = new BaseResponse<CardActivitiesDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                response.Data = await _service.GetCardActivities();
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
		[HttpGet("GetCardDetailsByCardNumber")]
		public async Task<BaseResponse<CardDetailsDTO>> GetCardDetailsByCardNumber([FromQuery] string cardNumber)
		{

			var response = new BaseResponse<CardDetailsDTO>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);

				response.Data = await _service.GetCardDetailsByCardNumber(cardNumber);
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
		/// The method perform card blocking for current user card in DB and verifone services
		/// </summary>
		/// <returns></returns>
		[HttpPost("BlockCard")]
        public async Task<BaseResponse<string>> BlockCard()
        {

            var response = new BaseResponse<string>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                response.Data = await _service.BlockCard();
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
        /// The method return business list base on wallet id and chainID, in case cainId is empty list won't be filtered by chain. 
        /// </summary>
        /// <param name="walletId">Mandatory</param>
        /// <param name="chainId">Optional</param>
        /// <returns></returns>
        [HttpGet("GetWalletBusinessesByChain")]
        public async Task<BaseResponse<WalletBusinessesDTO>> GetWalletBusinessesByChain([FromQuery]string walletId, [FromQuery]string chainId)
        {

            var response = new BaseResponse<WalletBusinessesDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { walletId, chainId });

                response.Data = await _service.GetWalletBusinessesByChain(walletId, chainId);
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

        [HttpGet("GetWalletChain")]
        public async Task<BaseResponse<List<WalletTagChainsData>>> GetWalletChain([FromQuery]string walletId)
        {

            

            var response = new BaseResponse<List<WalletTagChainsData>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { walletId });

                var res = await _service.GetWalletChain(walletId);
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

        [HttpGet("GetWalletChainBranches")]
        public async Task<BaseResponse<List<WalletChainBranches>>> GetWalletChainBranches([FromQuery] string walletId, [FromQuery] string chainId)
        {
            var response = new BaseResponse<List<WalletChainBranches>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { walletId, chainId });

                var res = await _service.GetWalletChainBranches(walletId, chainId);
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


        [HttpGet("GetWalletChainBranchesNearMe")]
        public async Task<BaseResponse<List<WalletChainBranches>>> GetWalletChainBranchesNearMe([FromQuery] string walletId, [FromQuery] string chainId, [FromQuery] double lat, [FromQuery] double lon)
        {
            var response = new BaseResponse<List<WalletChainBranches>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { walletId, chainId, lat, lon });

                var res = await _service.GetWalletChainBranchesNearMe(walletId, chainId, (decimal)lat, (decimal)lon);
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



        [HttpGet("GetWalletChainNearMe")]
        public async Task<BaseResponse<List<WalletTagChainsData>>> GetWalletChainNearMe([FromQuery] string walletId, double lat, double lon)
        {

            var response = new BaseResponse<List<WalletTagChainsData>>();
            var logItem = new LogDTO();

            try
            {
                var res = await _service.GetWalletChainNearMe(walletId, (decimal)lat, (decimal)lon);
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

        /// <summary>
        /// The method returns true if user can order new card and false in other case.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNewCardOrderPossible")]
        public BaseResponse<bool> OrderNewCard()
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                response.Data = _service.GetNewCardOrderPossible();
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
        /// The method returns a barcode for quick payment at checkout
        /// </summary>
        /// <returns></returns>
        [HttpGet("GeneratePayCode")]
        public async Task<BaseResponse<BarCodeDTO>> GeneratePayCode()
        {
            var response = new BaseResponse<BarCodeDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                response.Data = await _service.GeneratePayCode();
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
        /// The method moves balance from old verifone card to new card
        /// </summary>
        /// <returns></returns>
        [HttpGet("MoveBalance")]
        public async Task<BaseResponse<bool>> MoveBalance()
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                await _service.MoveBalance();
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
        /// The method gets available balance to discharge from verifone card 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAvailableBalanceForDischarge")]
        public async Task<BaseResponse<decimal>> GetAvailableBalanceForDischarge([FromQuery]string walletId)
        {
            var response = new BaseResponse<decimal>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                response.Data = await _service.GetAvailableBalanceForDischarge(int.Parse(walletId));
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

        [HttpGet("GetGlobalSelfDischargeLimit")]
public async Task<BaseResponse<decimal>> GetGlobalSelfDischargeLimit()
{
    var response = new BaseResponse<decimal>();
    var logItem = new LogDTO();

    try
    {
        OnStart(logItem);
        response.Data = await _service.GetGlobalSelfDischargeLimit();
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
        /// The method discharge from verifone card and refound credit card
        /// </summary>
        /// <returns></returns>
        [HttpGet("Discharge")]
        public async Task<BaseResponse<bool>> Discharge([FromQuery]string walletId, [FromQuery]string phoneNumber, [FromQuery]string email, [FromQuery]string creditCardNumber, [FromQuery]string expiredDate, [FromQuery]int cvv, [FromQuery]decimal dischargeAmount)
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                await _service.Discharge(int.Parse(walletId), phoneNumber, email, creditCardNumber, expiredDate, cvv, dischargeAmount);
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
            if (response.Status == true)
            {
                response.Data = true;
            }
            return response;
        }
        [Route("AllowedToDischarge")]
        public async Task<bool> AllowedToDischarge(string walletId)
        {
            var response = new BaseResponse<bool>();
            bool res = false;
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                res = await _service.AllowedToDischarge(walletId);
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
            return res;
        }
        [Route("GetTimeBetweenDischargeAndCharge")]
        public async Task<int> GetTimeBetweenDischargeAndCharge()
        {
            var response = new BaseResponse<bool>();

            var logItem = new LogDTO();
            int res = 0;

            try
            {
                OnStart(logItem, new object[] { });
                string expiryDate = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.timeBetweenDischargeAndCharge);
                if (!string.IsNullOrEmpty(expiryDate))
                {
                    Int32.TryParse(expiryDate, out int e);
                    if (e > 30)
                    {
                        res = e;
                    }
                }

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
            return res;
        }
    }
}

