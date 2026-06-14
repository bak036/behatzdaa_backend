using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
	public interface IShopingBasketBL
	{
        List<CartVarsDTO> AddProduct(ProductDTO productDto);

        List<ShopingBasket> GetCart();

        ProductsVars GetVariant(string barcode);

		List<CartVarsDTO> RemoveProduct(string productBarcode);

		List<CartVarsDTO> GetCartVars(List<ShopingBasket> cart);

		List<CartVarsDTO> RemoveProductsByCateoryNumber(string categoryNumber);

		bool RemoveAllProducts();

        List<CartVarsDTO> UpdateProductInCart(ShoppingBasketVariantDTO productToUpdate);
        List<ShopingBasket> UpdateShoppingCartPrices(int creditCardType);


    }
}
