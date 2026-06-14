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
using System.Threading.Tasks;

namespace Nofshonit.Services.Tests
{
	public class CardsServiceTests
	{
		private CardsService.CardsService CreateDefaultCardsService()
		{
			return new CardsService.CardsService();
		}

        //[Fact]
        //public void BlockCardTest()
        //{
        //    // Arrange
        //    var cardsService = CreateDefaultCardsService();

        //    // Act
        //    Task<string> result = cardsService.BlockCard(); // needs a real member with a wallet
        //    cardsService.ActiveateCard();
        //    // Assert
        //    Assert.NotNull(result);
        //}

        [Fact]
		public void GetCardActivitiesTest()
		{
			// Arrange
			var cardsService = CreateDefaultCardsService();

			// Act
			var result = cardsService.GetCardActivities().Result;

			// Assert
			Assert.NotNull(result);
		}

		//[Fact]
		//public void GetMemberBalanceTest()
		//{
		//	// Arrange
		//	var cardsService = CreateDefaultCardsService();

		//	// Act
		//	var result = cardsService.GetCardGeneralInfo().Result;

		//	// Assert
		//	Assert.NotNull(result);
		//}

		[Fact]
		public void GetWalletBusinessesByChainTest()
		{
			// Arrange
			var cardsService = CreateDefaultCardsService();

			// Act
			var result = cardsService.GetWalletBusinessesByChain("2237","2").Result;

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetWalletChainTest()
		{
			// Arrange
			var cardsService = CreateDefaultCardsService();

			// Act
			var result = cardsService.GetWalletChain("2237").Result;

			// Assert
			Assert.NotNull(result);
		}

		//[Fact]
		//public void LoadWalletTest()
		//{
		//	// Arrange
		//	var cardsService = CreateDefaultCardsService();

		//	// Act
		//	var result = cardsService.LoadWallet(new Common.DTOs.Cards.PayerDataDTO
		//	{
		//		WalletID="2237",
		//		AmountToCharge=10,
		//		AmountToLoad=12.5F,
		//		PinCode="",
		//		PayerCardTZ= "314327164",
		//		PayerCardNumber= "4580458045804580",
		//		PayerCardCVV=123,
		//		PayerCardExpiresMonth="10",
		//		PayerCardExpiresYear="23",
		//		SaveForQuickLoad=false
		//	}).Result;

		//	// Assert
		//	Assert.Equal("0",result);
		//}

		//[Fact]
		//public void OrderNewCardTest()
		//{
		//	// Arrange
		//	var cardsService = CreateDefaultCardsService();

  //          // Act
  //          cardsService.BlockCard();
  //          var result = cardsService.GetNewCardOrderPossible();
  //          cardsService.ActiveateCard();

		//	// Assert
		//	Assert.True(result);
		//}
	}
}
