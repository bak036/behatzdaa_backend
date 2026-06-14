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
using Newtonsoft.Json.Linq;
using System.Linq;
using Nofshonit.Common.DTOs.RequestDTOs;
using System.Transactions;
using Microsoft.AspNetCore.Routing.Tree;
using Xunit.Abstractions;
using Nofshonit.Common.DTOs.ResponseDTOs;

namespace Nofshonit.Services.Tests
{
	public class OrderServiceTests
	{
        private readonly ITestOutputHelper output;

        public OrderServiceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private OrderService.OrderService CreateDefaultOrderService()
		{
			return new OrderService.OrderService();
		}

        [Fact]
        public PurchaseResponseDTO PurchaseFromShopingBasket_CreditCardInfoOnly_Test()
        {
            var request = PreparePurchaseDTO();
            request.CreditCardExpirey = "0321";
            request.CreditCard16Digits = "4580458045804580";
            request.Cvv = "123";
            request.PinCode = null;

            var orderService = new OrderService.OrderService();
            PurchaseResponseDTO result = orderService.Purchase(request).Result;

            PurchaseAssertResponse(result);
            PurchaseInspectDbData(result);

            return result;
        }

        [Fact]
        public PurchaseResponseDTO PurchaseFromShopingBasket_CreditCardInfoWithPinCode_Test()
        {

            var request = PreparePurchaseDTO();
            request.CreditCardExpirey = "0321";
            request.CreditCard16Digits = "4580458045804580";
            request.Cvv = "123";
            request.PinCode = "9834";

            var orderService = new Nofshonit.Services.OrderService.OrderService();
            PurchaseResponseDTO result = orderService.Purchase(request).Result;

            PurchaseAssertResponse(result);
            PurchaseInspectDbData(result);

            return result;
        }

        [Fact]
        public PurchaseResponseDTO PurchaseFromShopingBasket_PinCodeOnly_Test()
        {

            var request = PreparePurchaseDTO();
            request.CreditCardExpirey = null;
            request.CreditCard16Digits = null;
            request.Cvv = null;
            request.PinCode = "9834";

            var orderService = new Nofshonit.Services.OrderService.OrderService();
            PurchaseResponseDTO result = orderService.Purchase(request).Result;

            PurchaseAssertResponse(result);
            PurchaseInspectDbData(result);

            return result;
        }

        private PurchaseRequestDTO PreparePurchaseDTO()
        {
            var shopingBasketService = new ShopingBasketService.ShopingBasketService();
            var shopingBasketItems = shopingBasketService.GetCart();
            //var totalPayment = 0;

            var request = new PurchaseRequestDTO()
            {
                FirstName = "Bar",
                LastName = "Bar",
                Mobile = "0545723103",
                Email = "bar2@email.com",
                NumOfPayments = 1,
                Cart = new List<PurchaseRequestDTO.CartItem>(),
                ShoppingBasketCart = new List<CartVarsDTO>(),

            };

            var variant = new ShoppingBasketVariantDTO
            {
                Barcode = "1003049-4",
                Quantity = 1,
                Price = 93,
                BusinessSubTypeId = 24
            };
            var productsToAdd = new ProductDTO();

            productsToAdd.CategoryNumber = "19847";
            productsToAdd.CategoryName = "CategoryName";
            productsToAdd.Variants = new List<ShoppingBasketVariantDTO>();
            productsToAdd.Variants.Add(variant);

            request.ShoppingBasketCart = shopingBasketService.AddProduct(productsToAdd);
            request.TotalPayment = 93;
            return request;
        }

        private void PurchaseAssertResponse(PurchaseResponseDTO response)
        {
            if (response.Status == 1)
                output.WriteLine("Purchase Succeeded !!!!!!!!!!!!");
            else
            {
               // output.WriteLine($"Status: {response["Status"].ToString()}, ErrorId: {response["ErrorId"].ToString()}, ErrorDescription: {response["ErrorDescription"].ToString()}");
              //  output.WriteLine($"OrderId: {response["Data"]["DtsOrderId"].ToString()}, TotalPayments: {response["Data"]["TotalPayments"].ToString()}, NumOfPayments: {response["Data"]["NumOfPayments"].ToString()}, OrderConfirmation: {response["Data"]["OrderConfirmation"].ToString()}");
                Assert.True(response.ErrorId == 0, $"Status: {response.Status}, ErrorId: {response.ErrorId}, ErrorDescription: " +
                    $"{response.ErrorDescription}\n\r" + $"OrderId: {response.Data.DtsOrderId}, TotalPayments: {response.Data.TotalPayments}, " +
                    $"NumOfPayments: {response.Data.NumOfPayments}, OrderConfirmation: {response.Data.OrderConfirmation}");
            }
        }

        private void PurchaseInspectDbData(PurchaseResponseDTO response)
        {

            var orderId = response.Data.DtsOrderId.ToString();
            var totalPayment = response.Data.TotalPayments.ToString();
            var numOfPayments = response.Data.NumOfPayments.ToString();
            var orderConfirmation = response.Data.OrderConfirmation.ToString();
            var clubRepo = new Nofshonit.Repositories.ClubModel.ClubRepo();
            var variants = response.Data.Variants;

            using (var dbContext = clubRepo.ContextManager.ClubContext())
            {

                var order = dbContext.Orders.FirstOrDefault(x => x.OrderId == int.Parse(orderId));
                var transactoins = dbContext.WebServiceTransaction.Where(x => x.OrderId.Value == int.Parse(orderId));
                //var payment = dbContext.Payments.FirstOrDefault(x => x.PaymentId == transactoins.First().PaymentId);
                var atractionsOrders = dbContext.Atractionsorders.Where(x => x.OrderId.Value == int.Parse(orderId));
                var moviesOrders = dbContext.Moviesorders.Where(x => x.OrderId.Value == int.Parse(orderId));
                var spaOrders = dbContext.Spaorders.Where(x => x.OrderId.Value == int.Parse(orderId));
                var tzimerOrders = dbContext.Tzimersorders.Where(x => x.OrderId.Value == int.Parse(orderId));




                Assert.True(order != null,$"Order {orderId} exist in DB");
                
                var totalSum = transactoins.Sum(x => x.CustomerPrice);

                Assert.True(totalSum == decimal.Parse(totalPayment), $"Total payment is {totalPayment}, Total sum is {totalSum}");

                var variantsCount = variants.Count();
                var transactionsCount = transactoins.Count();
                Assert.True(variantsCount == transactionsCount, $"Number of variants in order is {variantsCount}, Number of variants in DB is {transactionsCount} ");

                var barCodesCheckWST = true;

                var responseBarCodes = new Dictionary<string, int>();
                var ordersBarCodes = new Dictionary<string, int>();

                foreach (var variant in variants)
                {
                    var barCode = variant.VariantBarCode;
                    var quantity = variant.Qty;

                    if (responseBarCodes.ContainsKey(barCode) == false)
                        responseBarCodes[barCode] = quantity;
                    else
                        responseBarCodes[barCode] = responseBarCodes[barCode] + quantity;

                    //-------------------------------------------------------------------------

                   
                }

                foreach(var barCode in responseBarCodes.Keys)
                {
                    var counter1 = atractionsOrders.Count(x => x.BarCode == barCode);
                    var counter2 = moviesOrders.Count(x => x.BarCode == barCode);
                    var counter3 = spaOrders.Count(x => x.BarCode == barCode);
                    var counter4 = tzimerOrders.Count(x => x.BarCode == barCode);
                    var tmpCounter = counter1 + counter2 + counter3 + counter4;

                    if (ordersBarCodes.ContainsKey(barCode) == false)
                        ordersBarCodes[barCode] = tmpCounter;
                    else
                        ordersBarCodes[barCode] = ordersBarCodes[barCode] + tmpCounter;
                }

                foreach(var key in responseBarCodes.Keys)
                {
                    if (responseBarCodes[key] != ordersBarCodes[key])
                        Assert.True(responseBarCodes[key] == ordersBarCodes[key], $"Check variant {key}, quantity {responseBarCodes[key]} <> {ordersBarCodes[key]} is OK");
                }

                Assert.True(barCodesCheckWST, "All variants (barcodes) are in WebServiceTransactions");

            }


        }

        //[Fact]
        //public void PurchaseTest()
        //{
        //	// Arrange
        //	var orderService = CreateDefaultOrderService();

        //	var variants = new List<Common.DTOs.RequestDTOs.PurchaseRequestDTO.Variant>();
        //	variants.Add(new Common.DTOs.RequestDTOs.PurchaseRequestDTO.Variant
        //	{
        //		Barcode = "1000019-71",
        //		Price = 76,
        //		Quantity = 2
        //	});

        //	var cart = new List<Common.DTOs.RequestDTOs.PurchaseRequestDTO.CartItem>();
        //	cart.Add(new Common.DTOs.RequestDTOs.PurchaseRequestDTO.CartItem
        //	{
        //		CategoryId = 2540,
        //		CategoryName = "קייקי כפר בלום - שייט אתגרי 2015",
        //		Variants = variants
        //	});

        //	// Act
        //	var result = orderService.Purchase(
        //		new Common.DTOs.RequestDTOs.PurchaseRequestDTO
        //		{
        //			FirstName= "asdsa",
        //			LastName= "asdasd",
        //			Mobile= "0501234567",
        //			Email= "knollyuda@gmail.com",
        //			CreditCardExpirey= "1229",
        //			CreditCard16Digits= "4580458045804580",
        //			Cvv= "123",
        //			PinCode= "6789",
        //			TotalPayment= 228,
        //			NumOfPayments =1,
        //			Cart=cart	
        //		});

        //	// Assert
        //	Assert.NotNull(result);
        //}

        [Fact]
		public void CancelTest()
		{
			// Arrange
			var orderService = CreateDefaultOrderService();

            // Act
            var purchaseService = new PurchaseService.PurchaseService();
            var benefitStatusId = new List<int>();

            var purchaseHistoryLastPurchaseItem = purchaseService.PurchaseHistory(
                new MemberHistoryDTO
                {
                    FromDate = new DateTime(2018, 4, 24, 0, 0, 0),
                    ToDate = new DateTime(2023, 7, 12, 0, 0, 0),
                    BenefitStatusId = benefitStatusId,
                }).Result.Variants
                .Where(s => !s.BenefitStatusName.Equals("Canceled")).FirstOrDefault();

            var orderGuid = purchaseHistoryLastPurchaseItem.OrderGuid;
            var variantBarCode = purchaseHistoryLastPurchaseItem.VariantBarCode;
            var orderConfirmation = purchaseHistoryLastPurchaseItem.OrderConfirmation;

            var result = orderService.Cancel(
				new Common.DTOs.RequestDTOs.CancelRequestDTO
				{
					OrderGuid = orderGuid,
					VariantBarCode = variantBarCode,
					OrderConfirmation = orderConfirmation
                }).Result;

			// Assert
			Assert.Equal(1, result.Status);
		}
	}
}
