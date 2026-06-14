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
using Nofshonit.Services.PurchaseService;
using Nofshonit.Services.ShopingBasketService;
using System.Linq;
using Nofshonit.Common.DTOs.RequestDTOs;
using Newtonsoft.Json.Linq;
using Nofshonit.Repositories.ClubModel;

namespace Nofshonit.Services.Tests
{
    public class PurchaseServicesTests
    {
		private PurchaseService.PurchaseService CreateDefaultPurchaseServices()
		{
			return new PurchaseService.PurchaseService();
		}

		[Fact]
		public void PurchaseHistoryTest()
		{
			// Arrange
			var purchaseService = CreateDefaultPurchaseServices();
			var benefitStatusId = new List<int>();

			// Act
			var result = purchaseService.PurchaseHistory(
				new MemberHistoryDTO
				{
					FromDate = new DateTime(2018, 4, 24, 0, 0, 0),
					ToDate = new DateTime(2019, 4, 25, 0, 0, 0),
					BenefitStatusId = benefitStatusId,
				}).Result;

			// Assert
			Assert.NotNull(result);
		}
    }
}
