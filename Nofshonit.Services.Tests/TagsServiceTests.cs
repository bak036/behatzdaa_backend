using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nofshonit.Services.Tests
{
	public class TagsServiceTests
	{
		private TagsService.TagsService CreateDefaultTagsService()
		{
			return new TagsService.TagsService();
		}

		[Theory]
		[InlineData(3)]
		public void GetCategorysByTagIDTest(int tagId)
		{
			// Arrange
			var tagsService = CreateDefaultTagsService();

			// Act
			var result = tagsService.GetCategorysByTagID(tagId);

			// Assert
			Assert.NotNull(result);
		}

		[Theory]
		[InlineData(3)]
		public void GetCategoryByTagsTest(int selectTop)
		{
			// Arrange
			var tagsService = CreateDefaultTagsService();

			// Act
			var result = tagsService.GetCategoryByTags(selectTop);

			// Assert
			Assert.NotNull(result);
		}
	}
}
