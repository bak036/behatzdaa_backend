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
using Nofshonit.Services.MediaService;

namespace Nofshonit.Services.Tests
{
	public class MediaServicesTests
	{
		private MediaService.MediaService CreateDefaultMediaService()
		{
			return new MediaService.MediaService();
		}

		[Fact]
		public void GetOrganizationTest()
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetOrganizationNews();

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetImagesSliderTest()
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetImagesSlider();

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetGreetingsTest()
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetGreetings();

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetGreetingTypesTest()
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetGreetingTypes();

			// Assert
			Assert.NotEmpty(result);
		}

		[Theory]
		[InlineData("טקסט לאישור תקנון")]
		public void GetMessagesByTypeTest(string type)
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetMessagesByType(type);

			// Assert
			Assert.NotEmpty(result);
		}

		[Theory]
		[InlineData(14,15,16)]
		public void GetMessagesByIdTest(params int[] idList)
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetMessagesById(new List<int>(idList));

			// Assert
			Assert.NotEmpty(result);
		}

		[Theory]
		[InlineData(6,16,15,20)]
		public void GetMessagesByKeyTest(params int[] keyList)
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetMessagesByKey(new List<int>(keyList));

			// Assert
			Assert.NotEmpty(result);
		}

		[Theory]
		[InlineData("UserLogin")]
		public void GetMessagesByContextTest(string context)
		{
			// Arrange
			var mediaService = CreateDefaultMediaService();

			// Act
			var result = mediaService.GetMessagesByContext(context);

			// Assert
			Assert.NotEmpty(result);
		}

        [Fact]
        public void GetCommercialTest()
        {
            // Arrange
            var mediaService = CreateDefaultMediaService();

            // Act
            var result = mediaService.GetCommercial();

            // Assert
            Assert.NotEmpty(result);
        }
    }
}
