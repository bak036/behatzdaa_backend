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
using Nofshonit.Services.AddressesService;

namespace Nofshonit.Services.Tests
{
	public class AddressesServiceTests
	{
		private AddressesService.AddressesService CreateDefaultAddressesService()
		{
			return new AddressesService.AddressesService();
		}

		[Fact]
		public void GetCitiesTest()
		{
			// Arrange
			var addressesService = CreateDefaultAddressesService();

			// Act
			var result = addressesService.GetCities();

			// Assert
			Assert.NotNull(result);
		}

		[Theory]
		[InlineData(7)]
		public void GetCityStreetsTest(int cityId)
		{
			// Arrange
			var addressesService = CreateDefaultAddressesService();

			// Act
			var result = addressesService.GetCityStreets(cityId);

			// Assert
			 Assert.NotNull(result);
		}

		[Fact]
		public void GetRegionsTest()
		{
			// Arrange
			var addressesService = CreateDefaultAddressesService();

			// Act
			var result = addressesService.GetRegions();

			// Assert
			Assert.NotNull(result);
		}
	}
}
