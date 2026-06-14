using Nofshonit.Common.Interfaces;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Nofshonit.Infrastructure;
using Nofshonit.Services.Base;
using Nofshonit.Services.ShopingBasketService;
using Nofshonit.Common.DTOs.Event;

namespace Nofshonit.Services.Tests
{

	public class ShoppingBasketServicesTests
	{

		private ShopingBasketService.ShopingBasketService CreateDefaultShopingBasketService()
		{
			return new ShopingBasketService.ShopingBasketService();
		}

		[Fact]
		public void RemoveAllProductsTest()
		{
			// Arrange 
			var shopingBasketService = CreateDefaultShopingBasketService();

			// Act 
			var result = shopingBasketService.RemoveAllProducts();

			// Assert 
			Assert.True(result);
		}

		[Fact]
		public void AddProductTest()
		{
			// Arrange 
			var shopingBasketService = CreateDefaultShopingBasketService();
			var variant = new ShoppingBasketVariantDTO
			{
				Barcode = "1001394-49",
				Quantity = 2,
				Price = 207,
				BusinessSubTypeId = 1
			};
			var productsToAdd = new ProductDTO();

			productsToAdd.CategoryNumber = "790";
			productsToAdd.CategoryName = "CategoryName";
			productsToAdd.Variants = new List<ShoppingBasketVariantDTO>();
			productsToAdd.Variants.Add(variant);

			// Act 
			var result = shopingBasketService.AddProduct
				(productsToAdd);

            // Assert 
            Assert.NotNull(result);
		}

		[Fact]
		public void GetCartTest()
		{
			// Arrange 
			var shopingBasketService = CreateDefaultShopingBasketService();

			// Act 
			AddEventProductTest();
			AddProductTest();
			var result = shopingBasketService.GetCart();

            // Assert 
            Assert.True(result.Count == 2);
            RemoveAllProductsTest();
        }

		[Theory]
		[InlineData("1001394-49")]
		public void GetVariantTest(string barcode)
		{
			// Arrange 
			var shopingBasketService = CreateDefaultShopingBasketService();

			// Act 
			var result = shopingBasketService.GetVariant(barcode);

			// Assert 
			Assert.Equal(result.FullBarCode, barcode);
		}

		[Theory]
		[InlineData("1001394-49")]
		public void RemoveProductTest(string productBarcode)
		{
			// Arrange 
			var shopingBasketService = CreateDefaultShopingBasketService();

			// Act 
			AddProductTest();
			var result = shopingBasketService.RemoveProduct(productBarcode);

			// Assert 
			Assert.NotNull(result);
		}

		[Fact]
		public void AddEventProductTest()
		{
			// Arrange 
			var shopingBasketService = CreateDefaultShopingBasketService();
			var createReservation = new CreateReservationRequest();
			var seat = new SeatObject();
			var productsToAdd = new ProductDTO();

			seat.Coins = 0;
			seat.Price = 172;
			seat.PriceID = 97377;
			seat.PriceLevelId = 18764;
			seat.TicketTypeId = 45427;
			seat.VariantFullBarcode = "1001504-10223";

			createReservation.EventId = 11778;
			createReservation.JourneyId = 1;
			createReservation.ListOfSeats = new List<SeatObject>();
			createReservation.ListOfSeats.Add(seat);

			productsToAdd.CategoryNumber = "43141";
			productsToAdd.CategoryName = "טור עם קטגוריה 43141";
            productsToAdd.CreateReservation = createReservation;

            // Act 
            RemoveAllProductsTest();
            var result = shopingBasketService.AddProduct
				(productsToAdd);

            // Assert 
            Assert.NotNull(result);
		}

		[Theory]
		[InlineData("42733")]
		public void RemoveProductsByCategoryTest(string categoryNumber)
		{
			// Arrange 
			var shopingBasketService = CreateDefaultShopingBasketService();

			// Act 
			AddEventProductTest();
			var result = shopingBasketService.RemoveProductsByCateoryNumber(categoryNumber);

			// Assert 
			Assert.NotNull(result);
		}
	}
}
