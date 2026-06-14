using Nofshonit.Common.EF.Club;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofshonit.BL.RestApiGW;
using Nofshonit.Common.Constants;
using Nofshonit.Common;
using Nofshonit.Common.DTOs.Stock;
using Microsoft.IdentityModel.Protocols;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using static Nofshonit.Common.DTOs.RequestDTOs.PurchaseRequestDTO;
using System.Configuration;
using System.Security.Cryptography;
using Nofshonit.Common.DTOs.RequestDTOs;

namespace Nofshonit.BL.Limitations
{
    public static class StockFunctions
    {
        public static bool IsVariantInStock(IRestApiGW _restApiGW, IConfigurationManager _configurationManager, int quantity, string barcode, long catNumber, int orgId)
        {
            List<VariantData> variantList = new List<VariantData>()
            {
                new VariantData() { BenefitID = (int)catNumber, FullBarcode = barcode, Quantity = quantity }
            };
            return IsVariantInStock(_restApiGW, _configurationManager, orgId, variantList);
        }

        public static bool IsVariantInStock(IRestApiGW _restApiGW, IConfigurationManager _configurationManager, int orgId, List<VariantData> variants)
        {
            ApiRequestModel apiRequestModel = new ApiRequestModel()
            {
                baseUrl = _configurationManager.GetConfigByValue<string>(ConfigurationKey.RESTFulAPI_StockManagement_ApiUrl),
                relativeUrl = ManagementStockKeys.CheckStock,
                method = EHttpRequestType.POST,
                data = new CheckStockRequestModel()
                {
                    OrgID = orgId,
                    VariantData = variants
                }

            };
            StockBaseApiResult apiCheckStockResult = _restApiGW.ApiRequest<StockBaseApiResult>(apiRequestModel);

            //Check Result
            if (apiCheckStockResult.Code == StockBaseApiResult.HTTPResponseCode.Success &&
                  apiCheckStockResult.VariantResult.Any())
                return !apiCheckStockResult.VariantResult.Any(x => x.ResultCode != VariantResult.ResultCodeEnum.SUCCESS);

          //Error while trying to do request to the api
          return false;
        }

        public static bool IsVariantInStock(IRestApiGW _restApiGW, IConfigurationManager _configurationManager, int orgId, List<CartItem> cart)
        {
            List<VariantData> variantList = new List<VariantData>();
            foreach (var c in cart)
            {
                variantList.AddRange(
                    c.Variants.Select(x =>
                    new VariantData()
                    {
                        Quantity = x.Quantity,
                        FullBarcode = x.Barcode,
                        BenefitID = Convert.ToInt32(c.CategoryId)
                    }).ToList()
                );
            }

            return IsVariantInStock(_restApiGW, _configurationManager, orgId, variantList);
        }

        private static bool CheckDailyStock(IDtsOnlineRepo _dtsOnlineRepo, IClubRepo _clubRepo, string barcode, int quantity, string dbName, int orgId)
        {
            var categoryId = _clubRepo.CategoryIdByVariant(barcode);
            bool inStock = _dtsOnlineRepo.BudgetStockCategoriesCheck(categoryId);

            return inStock;
        }

        private static bool CheckDtsStock(IDtsOnlineRepo _dtsOnlineRepo, IClubRepo _clubRepo, string barcode, int quantity, string dbName)
        {
            bool inStock = true;

            var stockVariants = _dtsOnlineRepo.GetStockVariantBarcodes(barcode);
            var stock = stockVariants.Item1;
            var sameBarcodesByStock = stockVariants.Item2;
            if (stockVariants.Item1 != null)
            {
                var orderQty = _clubRepo.OrderQtyByBarcodes(sameBarcodesByStock);
                inStock = (stock.StockQuantity - orderQty) >= quantity;
            }

            return inStock;
        }

        private static bool CheckGeneralStock(IClubRepo _clubRepo,ProductsVars variant, int quantity, string dbName)
        {
            bool inStock = true;
            int stock = 0;
            if (variant != null && !string.IsNullOrEmpty(variant.ProductGeneralLimitFormula) && int.TryParse(variant.ProductGeneralLimitFormula, out stock))
            {
                var transactionsQuantity = _clubRepo.OrderQtyByBarcodes(new List<string> { variant.FullBarCode});
                inStock = (stock - transactionsQuantity) >= quantity;
            }

            return inStock;
        }

        private static bool CheckCouponStock(IDtsOnlineRepo _dtsOnlineRepo,ProductsVars productVar, int quantity, string dbName)
        {
            bool result = true;
            

            if (productVar != null && productVar.CuponStockId.GetValueOrDefault() > 0)
            {
                var couponQty = _dtsOnlineRepo.CouponStock(productVar.CuponStockId.GetValueOrDefault());
                result = couponQty >= quantity;
            }
            
            return result;
        }
    }
}
