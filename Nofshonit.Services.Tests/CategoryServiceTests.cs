using Xunit;

namespace Nofshonit.Services.Tests
{
    public class CategoryServiceTests
    {
        private CategoryService.CategoryService CreateDefaultCategoryService()
        {
            return new CategoryService.CategoryService();
        }

        [Fact]
        public void GetCategoryHeaderTest()
        {
            // Arrange
            var categoryService = CreateDefaultCategoryService();

            // Act
            var result = categoryService.GetCategoryHeader();

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(42338)]
        public void GetCategoryDetailsTest(long catagoryId)
        {
            // Arrange
            var categoryService = CreateDefaultCategoryService();

            // Act
            var result = categoryService.GetCategoryDetails(catagoryId);

            // Assert
            Assert.Equal(4, result.Breadcrumbs.Count);
        }

        [Theory]
        [InlineData(42338)]
        public void GetCategoryProductsTest(long catagoryId)
        {
            // Arrange
            var categoryService = CreateDefaultCategoryService();

            // Act
            var result = categoryService.GetCategoryProducts(catagoryId);

            // Assert
            Assert.Equal(4,result.Breadcrumbs.Count);
        }

    }
}
