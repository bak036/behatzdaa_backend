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
using System.Linq;
using Nofshonit.Services.Tests.Data;

namespace Nofshonit.Services.Tests
{
	public class ProductServiceTests
	{
		private ProductService.ProductService CreateDefaultProductService()
		{
            
			return new ProductService.ProductService();
		}

		[Fact]
		public void GetVariantsByCategoryIdTest()
		{
			// Arrange
			var productService = CreateDefaultProductService();

			// Act
			var result = productService.GetVariantsByCategoryId(2540);

			// Assert
			Assert.NotNull(result);
		}

        [Theory]
        [InlineData(790)]
        [InlineData(25485)]
        [InlineData(40480)]
        [InlineData(40955)]
        public void CategoryVariantsQuantityTest(long catagoryId)
        {
            // Arrange
            var data = ProcuctData.CategoryVariantsQuantityTest_Data();
            var productService = CreateDefaultProductService();
            var count = 0;
            data.TryGetValue(catagoryId, out count);
            // Act
            var result = productService.GetVariantsByCategoryId(catagoryId);

            // Assert
            Assert.Equal(count, result.Count);
        }


        [Theory]
        [InlineData(227, "100157-53")]
        [InlineData(790, "1408-277")]
        public void VariantDetailsCheck(long catagoryId,string barcode)
        {
            // Arrange
            var data = ProcuctData.VariantDetailsCheck_Data();
            var productService = CreateDefaultProductService();
            Common.DTOs.Product.VariantDTO item = null;
            data.TryGetValue(barcode, out item);
            // Act
            var result = productService.GetVariantsByCategoryId(catagoryId);
            var find = result.First(x => x.BarCode == barcode);
            // Assert
            Assert.Equal(item.Name.Trim(), find.Name.Trim());
            Assert.Equal(item.Price, find.Price);
            Assert.Equal(item.KupaPrice, find.KupaPrice);
           
        }
         

     
    }
}
