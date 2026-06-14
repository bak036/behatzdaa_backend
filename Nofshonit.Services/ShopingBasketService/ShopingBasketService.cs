using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Services.ShopingBasketService
{
	public class ShopingBasketService : BaseService, IShopingBasketService
	{
        private readonly IShopingBasketBL _shoppingBasketBL;

        public ShopingBasketService()
        {
            _shoppingBasketBL = Container.Resolve<IShopingBasketBL>();
        }

		public List<CartVarsDTO> AddProduct(ProductDTO productDto)
		{
			return _shoppingBasketBL.AddProduct(productDto);
		}

		public List<ShopingBasket> GetCart()
		{
			return _shoppingBasketBL.GetCart();
		}

		public ProductsVars GetVariant(string barcode)
		{
			return _shoppingBasketBL.GetVariant(barcode);
		}

		public List<CartVarsDTO> GetCartVars(List<ShopingBasket> cart)
		{
             
			return _shoppingBasketBL.GetCartVars(cart);
		}

		public List<CartVarsDTO> RemoveProduct(string productBarcode)
		{
			return _shoppingBasketBL.RemoveProduct(productBarcode);
		}

		public List<CartVarsDTO> RemoveProductsByCateoryNumber(string categoryNumber)
		{
			return _shoppingBasketBL.RemoveProductsByCateoryNumber(categoryNumber);
		}

		public bool RemoveAllProducts()
		{
			return _shoppingBasketBL.RemoveAllProducts();
		}

        public List<CartVarsDTO> UpdateProductInCart(ShoppingBasketVariantDTO productToUpdate)
        {
            return _shoppingBasketBL.UpdateProductInCart(productToUpdate);
        }

        public List<ShopingBasket> UpdateShoppingCartPrices(int creditCardType)
        {
            return _shoppingBasketBL.UpdateShoppingCartPrices(creditCardType);
        }

    }
}
