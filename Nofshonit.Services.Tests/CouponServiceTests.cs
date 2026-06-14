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

namespace Nofshonit.Services.Tests
{
	public class CouponServiceTests
	{
		private CouponService.CouponService CreateDefaultCouponService()
		{
			return new CouponService.CouponService();
		}

		[Fact]
		public void GetCouponDiscountTest()
		{
			// Arrange
			var couponsService = CreateDefaultCouponService();

			// Act
			var result = couponsService.GetCouponDiscount("13438104", 100);

			// Assert
			Assert.Equal((decimal)74.5,result.FinalPrice);
		}
	}
}
