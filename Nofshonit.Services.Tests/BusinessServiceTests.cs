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
	public class BusinessServiceTests
	{
		private BusinessService.BusinessService CreateDefaultBusinessService()
		{
			return new BusinessService.BusinessService();
		}

		[Theory]
		[InlineData(1000019L, 1003048L)]
		public void GetBusinessByIdsTest(params long[] idList)
		{
			// Arrange
			var businessService = CreateDefaultBusinessService();

            // Act
            var result = businessService.GetBusinessByIds(new List<long>(idList));

			// Assert
			Assert.True(result.Count==2);
		}

		[Theory]
		[InlineData(1003048L)]
		public void GetBusinessSubBranchesTest(long businessId)
		{
			// Arrange
			var businessService = CreateDefaultBusinessService();

			// Act
			var result = businessService.GetBusinessSubBranches(businessId);

			// Assert
			Assert.True(result.Count==10);
		}
	}
}
