using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;

namespace Nofshonit.Api.Controllers.ShopingBasketApi
{
	[Route("api/shoppingBasket")]
	[ApiController]
	public class ShopingBasketExternalApi : BaseController
	{
		private readonly IShopingBasketService _service;
		

		public ShopingBasketExternalApi()
		{
			_service = Container.Resolve<IShopingBasketService>();
			
		}

		[HttpPost]
		[Route("add")]
        /// <summary> 
        ///  The method adds variants of the same category or
        ///  tickets of an event, to the shoppingbasket table 
        /// </summary> 
        /// <param name="productDto">
        ///  The Dto with the information on the category and the list of variants to 
        ///  add to the list, or the tickets information of an event
        ///	</param> 
        /// <returns>
        ///  Returns a list of products(CartVarsDTO), that each product contains which   
        ///  category it belongs and more details about the variant, or details about  
        ///  the ticket
        /// </returns>
        public BaseResponse<List<CartVarsDTO>> AddProduct([FromBody]ProductDTO proudctDto)
		{
			var response = new BaseResponse<List<CartVarsDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, null, proudctDto);

				var res = _service.AddProduct(proudctDto);
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
		[Route("getCart")]
		/// <summary> 
		///  The method will return a list of products(CartVarsDTO), 
		///  that each product contains which category it belongs
		///  and more details about the variant
		/// </summary> 
		/// <returns> 
		///  Returns a list of products(CartVarsDTO), that each product contains which  
		///  category it belongs and more details about the variant 
		/// </returns> 
		public BaseResponse<List<CartVarsDTO>> GetCart()
		{
			var response = new BaseResponse<List<CartVarsDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);

				var cart = _service.GetCart();
				var productsList = _service.GetCartVars(cart);
				response.Data = productsList;

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

		[HttpPost]
		[Route("remove")]
		/// <summary>
		///  The method removes a product from the shoppingbasket table in the db,
		///  and the returns the list of the user other products from the GetCartVars method
		/// </summary>
		/// <param name="barcode">
		///  The variant/product barcode that will be removed from the shoppingbasket table
		///  </param>
		/// <returns>
		///  Returns the list of the user other products from the GetCartVars method
		/// </returns>
		public BaseResponse<List<CartVarsDTO>> RemoveProduct([FromQuery]string barcode)
		{
			var response = new BaseResponse<List<CartVarsDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { barcode});

				var res = _service.RemoveProduct(barcode);
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

		[HttpPost]
		[Route("removeByCategory")]
		/// <summary>
		///  The method removes all the products of the user that belongs to the category given
		///  and the returns the list of the user other products from the GetCartVars method
		/// </summary>
		/// <param name="categoryNumber">
		///  The category of of the products that its products will be removed
		/// </param>
		/// <returns>
		///  Returns the list of the user other products from the GetCartVars method
		/// </returns>
		public BaseResponse<List<CartVarsDTO>> RemoveProductsByCateoryNumber([FromQuery]string categoryNumber)
		{
			var response = new BaseResponse<List<CartVarsDTO>>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { categoryNumber });
                response.Data = _service.RemoveProductsByCateoryNumber(categoryNumber);
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

		[HttpPost]
		[Route("removeCart")]
		/// <summary>
		///  The method removes all the products of the user
		/// </summary>
		/// <returns>
		///  Returns true if the removal was successful, false otherwise
		/// </returns>
		public BaseResponse<bool> RemoveAllProducts()
		{
			var response = new BaseResponse<bool>();
			var logItem = new LogDTO();

			try
			{
				OnStart(logItem);

				var res = _service.RemoveAllProducts();
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
        [Route("UpdateShoppingCartPrices")]

        public BaseResponse<List<CartVarsDTO>> UpdateShoppingCartPrices(int CreditCardType)
        {

            var response = new BaseResponse<List<CartVarsDTO>>();

            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                var cart = _service.UpdateShoppingCartPrices(CreditCardType);
                var productsList = _service.GetCartVars(cart);
                response.Data = productsList;

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


        [HttpPost]
        [Route("update")]
        /// <summary>
        ///  The method adds variants of the same category or
        ///  tickets of an event, to the shoppingbasket table 
        /// </summary>
        /// <param name="productDto">
        ///  The product to add with the new quantity
        ///	</param> 
        /// <returns>
        ///  Returns a list of products(CartVarsDTO), that each product contains which   
        ///  category it belongs and more details about the variant, or details about  
        ///  the ticket
        /// </returns>
        public BaseResponse<List<CartVarsDTO>> UpdateProductInCart([FromBody]ShoppingBasketVariantDTO proudctDto)
        {
            var response = new BaseResponse<List<CartVarsDTO>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, proudctDto);

                var res = _service.UpdateProductInCart(proudctDto);
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