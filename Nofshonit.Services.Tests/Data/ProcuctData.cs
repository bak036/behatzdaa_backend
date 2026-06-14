using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Services.Tests.Data
{
    public static class ProcuctData
    {
        #region Data for Testing

        /// <summary>
        /// key: categoryId, value : product count
        /// </summary>
        public static Dictionary<long, int> CategoryVariantsQuantityTest_Data()
        {
            Dictionary<long, int> model = new Dictionary<long, int>();
            model.Add(227, 26);
            model.Add(790, 23);
            model.Add(42376, 3);
            model.Add(25485, 17);
            model.Add(40480, 9);
            model.Add(40955, 6);

            return model;
        }

        public static Dictionary<string, Common.DTOs.Product.VariantDTO> VariantDetailsCheck_Data()
        {
            Dictionary<string, Common.DTOs.Product.VariantDTO> model = new Dictionary<string, Common.DTOs.Product.VariantDTO>();
            model.Add("100157-53", new Common.DTOs.Product.VariantDTO()
            {
                BarCode = "100157-53",
                Price = 38,
                KupaPrice = 59,
                Name = "גן גורו - כניסה",
            });
            model.Add("1408-277", new Common.DTOs.Product.VariantDTO()
            {
                BarCode = "1408-277",
                Price = 322,
                KupaPrice = 480,
                Name = "רגעים קסומים - ספא מחיר מוזל",
            });
            model.Add("1003950-1", new Common.DTOs.Product.VariantDTO()
            {
                BarCode = "1003950-1",
                Price = 69,
                KupaPrice = 100,
                Name = "שובר 100",
            });


            return model;
        }

        #endregion
    }
}
