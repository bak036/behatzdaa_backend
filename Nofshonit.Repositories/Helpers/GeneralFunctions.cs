using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Linq;

namespace Nofshonit.Repositories.Helpers
{
    public  class GeneralFunctions : BaseFunctions
    {
        public GeneralFunctions()
        {

        }

        /// <summary>
        /// Get order confirmation (asmachta) of transaction
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="orderId">Dts order id</param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public  string GetVariantOrderConfirmation(string barcode, long orderId)
        {
            var orderConfirmation = string.Empty;

            try
            {
                using (var orgDb = new HelperFunctions().GetClubContext())
                {
                    //var variantOrders = orgDb.V_wAllOrders.Where(order => (order.OrderId.HasValue && order.OrderId.Value == orderId) && order.BarCode.Equals(barcode)).ToList();

                    //if (variantOrders.Count > 0)
                    //{
                    //    var sb = new StringBuilder();

                    //    foreach (var order in variantOrders)
                    //    {
                    //        sb.Append(order.OrderAsmchta + ", ");
                    //    }

                    //    orderConfirmation = sb.ToString().Substring(0, sb.Length - 2);
                    //}
                }
            }
            catch (Exception ex)
            {
               // Logger.Error($"Error in GetOrderConfirmation, error message: {ex.Message}");
                throw;
            }

            return orderConfirmation;
        }

        public string JsonSerializeObject<T>(T toSerialize)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(toSerialize);
            }
            catch (Exception ex)
            {
               // Logger.Error($"Error in JsonSerializeObject, Error message: {ex.Message}");
                throw;
            }
        }

        public string XmlSerializeObject<T>(T toSerialize)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(toSerialize.GetType());
                using (var textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, toSerialize);
                    return textWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                //Logger.Error($"Error in JsonSerializeObject, Error message: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Get List of variants
        /// </summary>
        /// <returns></returns>
        public List<Common.EF.Club.ProductsVars> GetVariantsByBenefitID(long benefitID)
        {
            var variants = new List<Common.EF.Club.ProductsVars>();

            try
            {
                using (var orgDb = new HelperFunctions().GetClubContext())
                {
                    //if (orgDb.UserPortalCategories.Any(x => x.CategoryNumber == benefitID))
                    {
                        var barcodes = orgDb.CategoryVariants.Where(x => x.CategoryNumber == benefitID).Select(x => x.Barcode).ToList();
                        if (barcodes.Count > 0)
                        {
                            variants = orgDb.ProductsVars.Where(r => barcodes.Contains(r.FullBarCode) && !r.DisabledToOrder && r.LastImplementationDate > DateTime.Now).ToList();
                            //variants = orgDb.ProductsVars.Where(r => barcodes.Contains(r.FullBarCode) && r.LastImplementationDate > DateTime.Now).ToList();
                        }

                        //string barcodeSplited = string.Join(",", barcodes);
                        //string variantsSplited = string.Join(",", variants.Select(r => r.FullBarCode).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
               // Logger.Error($"Error in GetVariantsByBenefitID, Error message: {ex.Message}");
                throw;
            }

            return variants;
        }


        ///// <summary>
        ///// Get List of benefits (portal categories)
        ///// </summary>
        ///// <returns></returns>
        //public  List<ProductsVar> GetFiltredVariantsForMainCategory(ActiveVariantsView benefit, List<string> prohibitedNames, string DBName, int orgId)
        //{
        //    try
        //    {
        //        var orgDb = new DbOrg(DBName);
        //        var finalVariants = new List<ProductsVar>();

        //        if (orgDb.UserPortalCategories.Any(x => x.CategoryNumber == benefit.CategoryNumber))
        //        {
        //            var barcodes = orgDb.CategoryVariants.Where(x => x.CategoryNumber == benefit.CategoryNumber).Select(x => x.Barcode).ToList();

        //            //variants = orgDb.ProductsVars.Where(r => barcodes.Contains(r.FullBarCode) && !r.DisabledToOrder && r.LastImplementationDate > DateTime.Now).ToList();
        //            //variants = orgDb.ProductsVars.Where(r => barcodes.Contains(r.FullBarCode) && r.LastImplementationDate > DateTime.Now).ToList();

        //            var variantsQuery = orgDb.ProductsVars.Where(r => barcodes.Contains(r.FullBarCode) && r.DisabledToOrder == false && r.LastImplementationDate != null && r.LastImplementationDate > DateTime.Now);

        //            // remove  prohibited Names from variants names
        //            foreach (var item in prohibitedNames)
        //            {
        //                variantsQuery = variantsQuery.Where(r => r.BusinessName.Contains(item) == false);
        //            }

        //            // take the ids to check for empty variants
        //            var variantsIds = variantsQuery.Select(r => r.FullBarCode).ToList();

        //            var VariantsNotEmptyStock = new List<string>();

        //            // take only in stock variants
        //            foreach (var varId in variantsIds)
        //            {
        //                if (ValidationFunctions.IsVariantInStock(varId, 1, benefit.CategoryNumber, DBName, orgId))
        //                {
        //                    VariantsNotEmptyStock.Add(varId);
        //                }
        //            }

        //            finalVariants = orgDb.ProductsVars.AsNoTracking().Where(r => VariantsNotEmptyStock.Contains(r.FullBarCode)).ToList();
        //        }

        //        return finalVariants;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetFilteredVariantsForMainCategory, Error message: {ex.Message}");
        //        throw;
        //    }
        //}



        //public  List<BusinessSubTypeSpecificationByVariants> GetBusinessSubTypeSpecificationByVariants(List<ProductsVar> products, string DBName)
        //{
        //    try
        //    {
        //        using (var orgDb = new DbOrg(DBName))
        //        {
        //            var FullBarCodeList = products.Select(p => p.FullBarCode).ToList();
        //            var businessSpecificationByVariants = orgDb.BusinessSubTypeSpecificationByVariants.AsNoTracking().Where(r => FullBarCodeList.Contains(r.BarCode)).ToList();

        //            return businessSpecificationByVariants;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBusinessSubTypeSpecificationByVariants, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  UserPortalCategory GetUserPortalCategory(long categoryNumber, string dbName)
        //{
        //    try
        //    {
        //        using (var orgDb = new DbOrg(dbName))
        //        {
        //            return orgDb.UserPortalCategories.FirstOrDefault(x => x.CategoryNumber == categoryNumber);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetUserPortalCategory, error message: {ex.Message}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Get redim code of a member.
        ///// If it is a coupon take coupon code else take card number.
        ///// </summary>
        ///// <param name="memberId"></param>
        ///// <param name="productVar"></param>
        ///// <param name="dBName"></param>
        ///// <returns></returns>
        //public  string GetNewRedimCode(string memberId, ProductsVar productVar, string orderConfirmation, string dBName)
        //{
        //    var redimCode = string.Empty;

        //    try
        //    {
        //        if (productVar.CuponStockID.HasValue && productVar.CuponStockID.Value > 0)
        //        {
        //            using (var db = new DbDts_Online())
        //            {
        //                using (var orgDb = new DbOrg(dBName))
        //                {
        //                    var couponStockId = productVar.CuponStockID.Value;
        //                    var couponStock = db.CouponsStock.FirstOrDefault(c => c.StockID == couponStockId && string.IsNullOrEmpty(c.MemberID));
        //                    redimCode = couponStock.CouponCode;
        //                    couponStock.MemberID = memberId;
        //                    db.SaveChanges();

        //                    //Add relevant row in couponsStockAtrOrders
        //                    orgDb.CouponsStockAtrOrders.Add(new CouponsStockAtrOrder
        //                    {
        //                        Asmachta = long.Parse(orderConfirmation),
        //                        CouponStockID = couponStock.CouponID
        //                    });

        //                    orgDb.SaveChanges();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            using (var orgDb = new DbOrg(dBName))
        //            {
        //                redimCode = orgDb.Cards.FirstOrDefault(x => x.IDMember.Equals(memberId)).CardNumber;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetNewRedimCode, error message: {ex.Message}");
        //        throw;
        //    }

        //    return redimCode;
        //}

        //public  List<BusinessSubTypeSpecificationCurrent> GetBusinessSubTypeSpecificationCurrents(List<ProductsVar> products, string DBName)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var orgDb = new DbOrg(DBName);
        //        var businessSubTypeList = products.Select(p => p.BusinessSubTypeID.HasValue ? p.BusinessSubTypeID.Value : 0).ToList();
        //        var businessSpecificationCurrents = orgDb.BusinessSubTypeSpecificationCurrents.AsNoTracking().Where(r => businessSubTypeList.Contains(r.BusinessSubTypeId)).ToList();

        //        return businessSpecificationCurrents;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBusinessSubTypeSpecificationCurrents, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  List<BenefitFileLog> GetBenefitFileLogs(List<long> categoryNumbers, string dbName)
        //{
        //    var list = new List<BenefitFileLog>();

        //    try
        //    {
        //        using (var orgDb = new DbOrg(dbName))
        //        {
        //            list = orgDb.BenefitFileLogs.Where(x => categoryNumbers.Contains(x.CategoryNumber)).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBenefitFileLogs, error message: {ex.Message}");
        //        throw;
        //    }

        //    return list;
        //}

        //public  List<RedimType> GetRedimTypes()
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();

        //        var redimTypes = db.RedimTypes.AsNoTracking().ToList();

        //        return redimTypes;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetRedimTypes, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  List<UserPortalCategory> GetMainPortalCategories(string DBName)
        //{
        //    try
        //    {
        //        var orgDb = new DbOrg(DBName);
        //        var dateYaterday = DateTime.Now.AddDays(-1);
        //        var portalCategories = orgDb.UserPortalCategories.Where(r => r.CategoryFather == 0).OrderBy(r => r.CategoryNumber).ToList();

        //        return portalCategories;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetPortalCategories, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  List<UserPortalCategory> GetPortalCategoriesLastChanged(string DBName)
        //{

        //    try
        //    {
        //        var portalCategories = new List<UserPortalCategory>();

        //        using (var orgDb = new DbOrg(DBName))
        //        {

        //            var startCheckTime = DateTime.Now.AddDays(GetDaysCheckForBenefits() * -1);
        //            portalCategories = orgDb.UserPortalCategories.Where(r => r.BusinessID != "0" && r.BusinessID != "" && r.CategoryFather != 0).OrderBy(r => r.CategoryNumber).ToList();

        //            using (var dtsOnlineDb = new DbOrg("DTS_Online"))
        //            {
        //                var businessChangedIds = (from b in dtsOnlineDb.Business join l in dtsOnlineDb.BusinessLastUpdate on b.BuisnessID equals l.BuisnessID where l.LastUpdateTimeStamp >= startCheckTime orderby l.LastUpdateTimeStamp descending group b by b.BuisnessID into pp select pp.Key).ToList();

        //                portalCategories = portalCategories.Where(x => x.LastUpdate >= startCheckTime || businessChangedIds.Contains(x.BusinessID)).ToList();
        //            }

        //        };


        //        return portalCategories;

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetPortalCategories, Error message: {ex.Message}");
        //        throw;
        //    }
        //}


        public int GetDaysCheckForBenefits()
        {
            try
            {
                var appSettings = new Repositories.Helpers.HelperFunctions().GetAppSettings();
                return int.Parse(appSettings.GetSection(Common.Constants.AppSettigsParams.ConfigParams)[Common.Constants.AppSettigsParams.DaysCheckForBenefits]);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //public  List<UserPortalCategory> GetPortalCategoriesSpesificID(string DBName, List<long> spesificID)
        //{
        //    try
        //    {
        //        var orgDb = new DbOrg(DBName);
        //        var portalCategories = orgDb.UserPortalCategories.Where(u => spesificID.Contains(u.CategoryNumber)).ToList();

        //        return portalCategories;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetPortalCategories, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Get the status of the portal category (1 = active, 0 = disabled, 2 = frozen)
        ///// </summary>
        //public  byte GetPortalCategoriesStatus(byte? PortalCategoryStatus, bool? IsBankLeumiSpecial)
        //{
        //    try
        //    {
        //        byte categoryStatus = 0;

        //        if (PortalCategoryStatus == 1)
        //        {
        //            categoryStatus = 1;
        //        }
        //        else if (PortalCategoryStatus == 0 || PortalCategoryStatus == 2)
        //        {
        //            if (IsBankLeumiSpecial.HasValue && IsBankLeumiSpecial.Value)
        //            {
        //                categoryStatus = 2;
        //            }
        //            else
        //            {
        //                categoryStatus = 0;
        //            }
        //        }

        //        return categoryStatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetPortalCategoriesStatus, Error message: {ex.Message}");
        //        return 0;
        //    }
        //}

        //public  PortalCategory GetPortalCategoryOrDefault(long id)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var portalCategory = db.PortalCategories.FirstOrDefault(r => r.CategoryNumber == id);

        //        return portalCategory;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetPortalCategory, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  List<VariantOrderConfirmation> GetAllVariantsOrderConfirmations(int dtsOrderId, string dBName)
        //{
        //    List<VariantOrderConfirmation> result = null;

        //    try
        //    {
        //        using (var orgDb = new DbOrg(dBName))
        //        {
        //            result = orgDb.AllOrders.Where(x => x.OrderId.HasValue ? x.OrderId.Value == dtsOrderId : false).Select(x => new VariantOrderConfirmation
        //            {
        //                Barcode = x.BarCode,
        //                Qty = x.OrderQuantity,
        //                Confirmation = x.OrderAsmchta.ToString()
        //            }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetAllVariantsOrderConfirmations, error message: {ex.Message}");
        //        throw;
        //    }

        //    return result;
        //}

        //public  string GetImportantToKnowText(long categoryNumber, int organizationId)
        //{
        //    var result = "";

        //    using (var context = new DbOrg("DTS_Online"))
        //    {
        //        var item = (from c in context.MessageCategories join m in context.Messages on c.MessageID equals m.MessageID where c.OrganizationID == organizationId && c.CategoryNumber == categoryNumber select new { m.MessageText }).FirstOrDefault();
        //        if (item != null)
        //            result = item.MessageText;
        //    }

        //    return result;
        //}

        //public  OrganizationCategory GetOrganizationCategory(long categoryNumber, int organizationId)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var organizationCategory = db.OrganizationCategories.FirstOrDefault(r => r.CategoryNumber == categoryNumber && r.OrganizationID == organizationId);

        //        return organizationCategory;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetOrganizationCategory, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  BusinessSubType GetBusinessSubType(int BusinessSubType)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var businessSubType = db.BusinessSubTypes.FirstOrDefault(r => r.BusinessSubTypeId == BusinessSubType);

        //        return businessSubType;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBusinessSubType, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  List<BusinessSubType> GetAllActiveBusinessSubTypes()
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var businessSubTypes = db.BusinessSubTypes.Where(r => r.Active == true).ToList();

        //        return businessSubTypes;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetMainBusinessSubTypes, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  Business GetBusiness(string BusinessID)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var business = db.Businesses.FirstOrDefault(r => r.BuisnessID == BusinessID);

        //        return business;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBusiness, Error message: {ex.Message}");
        //        throw;
        //    }
        //}


        //public  Region GetRegionByCityID(int cityID)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var city = db.Cities.FirstOrDefault(r => r.CityID == cityID);

        //        if (city != null)
        //        {
        //            var region = db.Regions.Find(city.RegionID);
        //            region.RegionName = region.RegionName.Trim();
        //            return region;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetRegionByCityID, Error message: {ex.Message}");
        //        return null;
        //    }
        //}

        //public  string GetCustomerComment(long CategoryNumber)
        //{
        //    try
        //    {

        //        var db = new DbDts_Online();
        //        var portalCategory = db.PortalCategories.AsNoTracking().First(r => r.CategoryNumber == CategoryNumber);

        //        if (portalCategory.MemberMessageType == 1)
        //        {
        //            if (portalCategory.IsRedimTypeOtherMessage == false)
        //            {
        //                var redimTypeID = portalCategory.RedimTypeId;
        //                var orgRedimType = db.OrganizationRedimTypes.AsNoTracking().FirstOrDefault(r => r.OrgRedimTypeId == redimTypeID && r.OrganizationId == 102);

        //                if (orgRedimType == null)
        //                {
        //                    return null;
        //                }
        //                else
        //                {
        //                    return orgRedimType.RedimContent;
        //                }
        //            }
        //            else
        //            {
        //                return portalCategory.RedimTypeOtherMessageText;
        //            }
        //        }
        //        else if (portalCategory.MemberMessageType == 2)
        //        {
        //            var ticketType = portalCategory.TicketsType.Value.ToString();
        //            var keyVlaue = db.KeyValueLists.AsNoTracking().FirstOrDefault(r => r.ListValue == ticketType);
        //            return keyVlaue.ListText;
        //        }
        //        else if (portalCategory.MemberMessageType == 3)
        //        {
        //            return portalCategory.MemberMessageText;
        //        }
        //        else
        //            return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public  List<BusinessSubBranch> GetBusinessSubBranch(string BusinessID)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var businessInt = int.Parse(BusinessID);
        //        var businessSubBranch = db.BusinessSubBranches.Where(r => r.BusinessID == businessInt && r.Active).ToList();

        //        foreach (var item in businessSubBranch)
        //        {
        //            if (!string.IsNullOrEmpty(item.SubBusinessName))
        //                item.SubBusinessName = item.SubBusinessName.Trim();
        //        }

        //        return businessSubBranch;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBusinessSubBranch, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        ///// <summary>
        /////  Get category father name from OrganizationCategories, but the OrganizationCategories is not exists take it from the PortalCategories
        ///// </summary>
        ///// <param name="fatherID"></param>
        ///// <param name="orgID"></param>
        ///// <returns></returns>
        //public  string GetCategoryFatherName(long fatherID, int orgID)
        //{
        //    try
        //    {
        //        ///
        //        var returnName = "";
        //        var db = new DbDts_Online();
        //        var organizationCategory = db.OrganizationCategories.FirstOrDefault(r => r.OrganizationID == orgID && r.CategoryNumber == fatherID);
        //        if (organizationCategory != null)
        //        {
        //            returnName = organizationCategory.DisplayName;
        //        }
        //        return returnName;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetCategoryFatherName, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  string GetBusinessSubTypeName(int id)
        //{
        //    var name = string.Empty;

        //    try
        //    {
        //        using (var db = new DbDts_Online())
        //        {
        //            name = db.BusinessSubTypes.FirstOrDefault(x => x.BusinessSubTypeId == id)?.BusinessSubTypeName;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBusinessSubTypeName, error message: {ex.Message}");
        //        throw;
        //    }

        //    return name;
        //}

        //public  Region GetRegion(byte RegionID)
        //{
        //    try
        //    {
        //        var dictionary = MemoryCache.Default["RegionID"] as Dictionary<byte, Region>;
        //        if (dictionary == null)
        //        {
        //            dictionary = FillRegion();
        //            MemoryCache.Default["RegionID"] = dictionary;
        //        }

        //        return dictionary[RegionID];
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetRegion, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Check if the fathers of the UserPortalCategoryId is exists and actives
        ///// </summary>

        //public  bool IsUserPortalCategoryHasActiveFathers(string DbName, long UserPortalCategoryId)
        //{
        //    var category = GetUserPortalCategoriesTree(DbName, UserPortalCategoryId);

        //    // counter to prevent while-loop continues forever
        //    var counter = 0;
        //    while (category != null && counter < 100)
        //    {
        //        // return true when the category is a main category and active
        //        if (category.CategoryStatus == 1 && category.FatherID == 0)
        //        {
        //            return true;
        //        }
        //        counter++;
        //        category = GetUserPortalCategoriesTree(DbName, category.FatherID);
        //    }

        //    return false;
        //}

        //public  UserPortalCategoriesTree GetUserPortalCategoriesTree(string DbName, long CategoryNumber)
        //{
        //    try
        //    {
        //        var dictionary = MemoryCache.Default["UserPortalCategoriesTree_" + DbName] as Dictionary<long, UserPortalCategoriesTree>;
        //        if (dictionary == null)
        //        {
        //            dictionary = FillUserPortalCategoriesTree(DbName);
        //            MemoryCache.Default["UserPortalCategoriesTree_" + DbName] = dictionary;
        //        }
        //        if (dictionary.ContainsKey(CategoryNumber))
        //        {
        //            return dictionary[CategoryNumber];
        //        }
        //        else
        //            return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetUserPortalCategoriesTree, Error message: {ex.Message}");
        //        return null;
        //    }
        //}


        //private  Dictionary<long, UserPortalCategoriesTree> FillUserPortalCategoriesTree(string DbName)
        //{
        //    try
        //    {

        //        var orgDb = new DbOrg(DbName);
        //        var dictionaryList = orgDb.UserPortalCategories.Select(s => new { s.CategoryNumber, s.CategoryFather, s.CategoryStatus }).ToList();

        //        var duplicateKeys = dictionaryList.GroupBy(x => x.CategoryNumber)
        //                .Where(group => group.Count() > 1)
        //                .Select(group => group.Key).ToList();

        //        dictionaryList.RemoveAll(r => duplicateKeys.Contains(r.CategoryNumber));

        //        var dictionary = dictionaryList.ToDictionary(t => t.CategoryNumber, t => new UserPortalCategoriesTree { CategoryNumber = t.CategoryNumber, FatherID = t.CategoryFather, CategoryStatus = t.CategoryStatus });
        //        return dictionary;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in FillUserPortalCategoriesTree, Error message: {ex.Message}");
        //        throw;
        //    }
        //}


        //private  Dictionary<byte, Region> FillRegion()
        //{
        //    try
        //    {

        //        var db = new DbDts_Online();
        //        var dictionary = db.Regions.Select(s => new { key = s.RegionId, s.RegionName })
        //           .ToDictionary(t => t.key, t => new Region { RegionId = t.key, RegionName = t.RegionName });

        //        return dictionary;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in FillRegion, Error message: {ex.Message}");
        //        throw;
        //    }
        //}





        //public  string GetCityName(int CityID)
        //{
        //    try
        //    {
        //        var dictionary = MemoryCache.Default["Cities"] as Dictionary<int, string>;
        //        if (dictionary == null)
        //        {
        //            dictionary = FillCities();
        //            MemoryCache.Default["Cities"] = dictionary;
        //        }

        //        return dictionary[CityID];
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetGeoRegion, Error message: {ex.Message}");
        //        return string.Empty;
        //    }
        //}

        //public  List<CitiesNameView> GetCitiesName()
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var citiesName = db.Cities.AsNoTracking().Select(c => new CitiesNameView()
        //        {
        //            CityID = c.CityID,
        //            Name = c.CityName
        //        }).ToList();

        //        return citiesName;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetAllCities, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  bool IsAutoImplementReport(ProductsVar productVar)
        //{
        //    var autoImplement = false;

        //    try
        //    {
        //        using (var db = new DbDts_Online())
        //        {
        //            autoImplement = db.Businesses.FirstOrDefault(x => x.BuisnessID.Equals(productVar.BusinessId)).AutoImplementaionAfterReport;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in IsAutoImplementReport, error message: {ex.Message}");
        //        throw;
        //    }

        //    return autoImplement;
        //}

        //public  List<Region> GetRegions()
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var regions = db.Regions.AsNoTracking().ToList();

        //        foreach (var item in regions)
        //        {
        //            item.RegionName = item.RegionName.Trim();
        //        }

        //        return regions;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetAllCities, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //private  Dictionary<int, string> FillCities()
        //{
        //    try
        //    {

        //        var db = new DbDts_Online();
        //        var dictionary = db.Cities.Select(t => new { t.CityID, t.CityName })
        //           .ToDictionary(t => t.CityID, t => t.CityName);

        //        return dictionary;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in FillGeoRegion, Error message: {ex.Message}");
        //        throw;
        //    }
        //}




        //#region GeoGrapic
        //public  BusinessMode GetBusinessMode(int BusinessMode)
        //{
        //    try
        //    {
        //        var dictionary = MemoryCache.Default["BusinessMode"] as Dictionary<int, BusinessMode>;
        //        if (dictionary == null)
        //        {
        //            dictionary = FillBusinessMode();
        //            MemoryCache.Default["BusinessMode"] = dictionary;
        //        }
        //        return dictionary[BusinessMode];

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBusinessMode, Error message: {ex.Message}");
        //        return null;
        //    }
        //}

        //private  Dictionary<int, BusinessMode> FillBusinessMode()
        //{
        //    try
        //    {

        //        var db = new DbDts_Online();
        //        var dictionary = db.BusinessModes.Select(s => new { key = s.BusinessModeId, s.BusinessModeName })
        //           .ToDictionary(t => t.key, t => new BusinessMode { BusinessModeId = t.key, BusinessModeName = t.BusinessModeName });

        //        return dictionary;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in FillBusinessMode, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //#endregion

        //public  string GetRedimTypeNameById(int id, OrganizationDetails org)
        //{
        //    var name = string.Empty;

        //    try
        //    {
        //        using (var db = new DbDts_Online())
        //        {
        //            name = db.OrganizationRedimTypes.AsNoTracking().FirstOrDefault(redim => redim.RedimTypeId == id && redim.OrganizationId == org.OrgId)?.RedimContent;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetRediumTypeNameById, Error message: {ex.Message}");
        //        throw;
        //    }

        //    return name;
        //}

        ///// <summary>
        ///// Get benefit type id according to variantType.
        ///// </summary>
        ///// <param name="productVar"></param>
        ///// <returns></returns>
        //public  int GetBenefitTypeId(ProductsVar productVar)
        //{
        //    var type = 0;

        //    try
        //    {
        //        type = GetBenefitTypeIdByVariantType(productVar.VariantType);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBenefitTypeId, error message: {ex.Message}");
        //        throw;
        //    }

        //    return (int)type;
        //}




        ///// <summary>
        ///// Get benefit type id according to variantType.
        ///// </summary>
        ///// <param name="productVar"></param>
        ///// <returns></returns>
        //public  int GetBenefitTypeIdByVariantType(int? variantType)
        //{
        //    var type = (int)BenefitType.NormalVariant;
        //    try
        //    {
        //        if (variantType == 11)
        //        {
        //            type = (int)BenefitType.GiftCard;
        //        }
        //        else if (variantType == 12)
        //        {
        //            type = (int)BenefitType.Shows;
        //        }
        //        else if (variantType == 2)
        //        {
        //            type = (int)BenefitType.Coupon;
        //        }
        //        else
        //        {
        //            type = (int)BenefitType.NormalVariant;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBenefitTypeId, error message: {ex.Message}");
        //        throw;
        //    }

        //    return type;
        //}

        ///// <summary>
        ///// Get benefit type id according to variantType.
        ///// </summary>
        ///// <param name="productVar"></param>
        ///// <returns></returns>
        //public  int GetBenefitTypeIdByBusinessSubTypeID(int id, int userType)
        //{
        //    var type = (int)BenefitType.NormalVariant;
        //    try
        //    {
        //        if (userType == 2)
        //        { // מדובר בחתך אוכלוסיה של מיוחד בלאומי
        //            type = (int)BenefitType.LeumiSpecial;
        //        }
        //        else
        //        {
        //            if (id == 7)
        //            {
        //                type = (int)BenefitType.GiftCard;
        //            }
        //            else if (id == 6)
        //            {
        //                type = (int)BenefitType.Shows;
        //            }
        //            //else if (id == 2)
        //            //{
        //            //    type = (int)BenefitType.Coupon;
        //            //}
        //            else
        //            {
        //                type = (int)BenefitType.NormalVariant;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBenefitTypeId, error message: {ex.Message}");
        //        throw;
        //    }

        //    return type;
        //}

        ///// <summary>
        ///// Get Order Status Id according to enum.
        ///// Searches AllOrders for relevant orders according to orderId.
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="dbName"></param>
        ///// <returns></returns>
        //public  OrderStatus GetOrderStatus(ProductsVar productVar, AllOrder allOrder)
        //{
        //    var status = OrderStatus.NotImplemented;

        //    try
        //    {
        //        var lastImplementationDate = allOrder.LastImplementationDate;
        //        var orderQuantity = allOrder.OrderQuantity;
        //        var orderBalance = allOrder.OrderBalance;
        //        var benefitTypeId = GetBenefitTypeId(productVar);

        //        if (orderQuantity == 0 && orderBalance == 0)
        //        {
        //            status = OrderStatus.Canceled;
        //        }
        //        else if (allOrder.TransferToFriendDate != null)
        //        {
        //            status = OrderStatus.TransferredAsGift;
        //        }
        //        else if (allOrder.IsInCancelProcess)
        //        {
        //            status = OrderStatus.InCancelProcess;
        //        }
        //        else if (lastImplementationDate.HasValue && lastImplementationDate.Value < DateTime.Now)
        //        {
        //            status = OrderStatus.Expired;
        //        }
        //        else if (benefitTypeId == (int)BenefitType.GiftCard)
        //        {
        //            using (var db = new DbDts_Online())
        //            {
        //                var cardNumber = allOrder.CardNumber;
        //                //TODO: change to == when mwcmedia is updated by verifone (job in tests)
        //                if (db.MwcMedias.Count(x => x.cardNumber.Equals(cardNumber)) <= 1)
        //                {
        //                    status = OrderStatus.NotImplemented;
        //                }
        //                else if (db.MwcMedias.Where(x => x.cardNumber.Equals(cardNumber)).Sum(x => x.Amount) == 0)
        //                {
        //                    status = OrderStatus.Implemented;
        //                }
        //                else
        //                {
        //                    status = OrderStatus.PartiallyImplemented;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (orderQuantity > 0 && orderBalance > 0)
        //            {
        //                status = OrderStatus.Implemented;
        //            }
        //            else if (orderQuantity > 0 && orderBalance == 0)
        //            {
        //                status = OrderStatus.NotImplemented;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetBenefitStatusId, error message: {ex.Message}");
        //        throw;
        //    }

        //    return status;
        //}

        //public  bool SendEmail(EmailDetails emailDetails)
        //{
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var email = new EmailQueue()
        //        {
        //            Attachment = emailDetails.Attachment,
        //            EmailFrom = emailDetails.EmailFrom ?? ConfigurationManager.AppSettings["emailFrom"],
        //            EmailTo = emailDetails.EmailTo ?? ConfigurationManager.AppSettings["emailTo"],
        //            EmailBCC = emailDetails.Cc,
        //            EmailSubject = emailDetails.Subject,
        //            EmailBody = emailDetails.MessageBody,
        //            IsBodyHtml = true,
        //            EmailType = 100000 + emailDetails.OrganizationId,
        //            PaymentID = 0,
        //            EmailDateAdded = DateTime.Now,
        //        };
        //        db.EmailQueues.Add(email);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Info($"error in send email: {emailDetails.Attachment}");
        //        Logger.Error(ex.Message);
        //        throw;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Calculate coins and moeny to refund for variant
        ///// </summary>
        ///// <param name="dtsOrderId"></param>
        ///// <param name="variantBarCode"></param>
        ///// <param name="coins"></param>
        ///// <param name="money"></param>
        ///// <param name="dbName"></param>
        //public  void GetRefundCoinsAndMoney(int dtsOrderId, string variantBarCode, ref int coins, ref int money, string dbName)
        //{
        //    var isAttractionOrder = false;
        //    var isMovieOrder = false;
        //    var isSpaOrder = false;

        //    try
        //    {
        //        using (var orgDb = new DbOrg(dbName))
        //        {
        //            var order = orgDb.AllOrders.FirstOrDefault(x => x.OrderId.HasValue && x.OrderId.Value == dtsOrderId && x.BarCode.Equals(variantBarCode) && x.OrderQuantity > 0 && x.OrderBalance < x.OrderQuantity);

        //            if (order != null)
        //            {
        //                WebServiceTransaction transaction;

        //                isAttractionOrder = orgDb.AttractionsOrders.Any(x => x.MemberOrderAsmchta == order.OrderAsmchta);
        //                isMovieOrder = orgDb.MoviesOrders.Any(x => x.OrderAsmchta == order.OrderAsmchta);
        //                isSpaOrder = orgDb.SpaOrders.Any(x => x.OrderAsmchta == order.OrderAsmchta);

        //                if (isAttractionOrder)
        //                {
        //                    transaction = orgDb.WebServiceTransactions.FirstOrDefault(x => x.TTransactionOrder.HasValue && order.OrderAsmchta == x.TTransactionOrder.Value && x.TTransactionProductID.Equals(variantBarCode));
        //                }
        //                else if (isMovieOrder)
        //                {
        //                    transaction = orgDb.MoviesOrders.Join(orgDb.MovieOrdersToWebOrders, x => x.OrderAsmchta, y => y.MovieOrderAsmachta, (m, mo) => new { m, mo })
        //                                                    .Join(orgDb.WebServiceTransactions, x => x.mo.WebOrderAsmachta, y => y.TTransactionID, (mm, wst) => new { mm.m, mm.mo, wst })
        //                                                    .FirstOrDefault(x => x.m.BarCode.Equals(variantBarCode) && x.m.OrderId == dtsOrderId).wst;
        //                }
        //                else if (isSpaOrder)
        //                {
        //                    transaction = orgDb.SpaOrders.Join(orgDb.SpaOrdersToWebOrders, x => x.OrderAsmchta, y => y.SpaOrderAsmachta, (s, so) => new { s, so })
        //                                                    .Join(orgDb.WebServiceTransactions, x => x.so.WebOrderAsmachta, y => y.TTransactionID, (ss, wst) => new { ss.s, ss.so, wst })
        //                                                    .FirstOrDefault(x => x.s.BarCode.Equals(variantBarCode) && x.s.OrderId == dtsOrderId).wst;
        //                }
        //                else
        //                {
        //                    transaction = orgDb.TzimersOrders.Join(orgDb.TzimerOrdersToWebOrders, x => x.OrderAsmchta, y => y.TzimerOrderAsmachta, (t, to) => new { t, to })
        //                                                    .Join(orgDb.WebServiceTransactions, x => x.to.WebOrderAsmachta, y => y.TTransactionID, (tt, wst) => new { tt.t, tt.to, wst })
        //                                                    .FirstOrDefault(x => x.t.BarCode.Equals(variantBarCode) && x.t.OrderId == dtsOrderId).wst;
        //                }

        //                if (transaction != null)
        //                {
        //                    money = Convert.ToInt32(transaction.CustomerPrice);
        //                    if (DateTime.Now.Month == transaction.TTransactionDateTime.Value.Month)
        //                    {
        //                        coins = transaction.Coins;
        //                    }
        //                    else
        //                    {
        //                        coins = 0;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetRefundCoinsAndMoney, error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  string GetErrorMessage(int id)
        //{
        //    var message = string.Empty;

        //    try
        //    {
        //        var dictionary = MemoryCache.Default["ErrorMessages"] as Dictionary<int, string>;
        //        if (dictionary == null)
        //        {
        //            dictionary = FillErrorMessages();
        //            MemoryCache.Default["ErrorMessages"] = dictionary;
        //        }

        //        message = dictionary[id];
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetErrorMessage, error message: {ex.Message}");
        //        throw;
        //    }

        //    return message;
        //}

        //private  Dictionary<int, string> FillErrorMessages()
        //{
        //    try
        //    {
        //        using (var orgDb = new DbOrg("LeumiApp"))
        //        {
        //            var dictionary = orgDb.Errors.Select(s => new { key = s.ErrorId, s.Text })
        //               .ToDictionary(t => t.key, t => t.Text);

        //            return dictionary;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in FillGeoRegion, Error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  bool ReduceStock(string variantBarCode, string dbName, int orgId)
        //{
        //    var isSuccess = false;

        //    try
        //    {
        //        var client = new DtsCoreLib.StockService.ServiceSoapClient();
        //        var variant = new DtsCoreLib.StockService.VariantStock[]
        //        {
        //        new DtsCoreLib.StockService.VariantStock
        //        {
        //            FullBarCode = variantBarCode,
        //            Amount = 1,
        //            IsTrade = true
        //        }
        //        };

        //        var result = client.GetStockResponse(variant, dbName, orgId);
        //        isSuccess = result.FirstOrDefault().CheckStockSeccsess;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in ReduceStock, error message: {ex.Message}");
        //        throw;
        //    }

        //    return isSuccess;
        //}


        //public  List<ImagePortalCategoryView> GetImagesForPortalCategory(int orgId, List<long> CategoriesNumberInts)
        //{

        //    // TODO CategoriesNumberInts 

        //    var returnImages = new List<ImagePortalCategoryView>();
        //    try
        //    {
        //        var db = new DbDts_Online();
        //        var imagesExtenstions = db.ImagesExtentions.AsNoTracking().ToList();

        //        // ImageTypeId = 3  is leumiApp pictures
        //        var portalImagesList = db.PortalCategoriesImages.AsNoTracking().Where(r => CategoriesNumberInts.Contains(r.CategoryNumber) && r.ImageTypeId == 3).ToList();
        //        var portalImagesIds = portalImagesList.Select(r => r.PortalCategoriesImageId).ToList();
        //        var portalImagesTypesIds = portalImagesList.Select(r => r.ImageTypeId).ToList();
        //        var organizationImagesList = db.OrganizationCategoriesImages.Where(r => r.OrganizationId == orgId && portalImagesIds.Contains(r.PortalCategoriesImageId) && r.ImageTypeId == 3).ToList();

        //        //var imagesOrgIds = db.ImagesTypesToOrganizations.AsNoTracking().Where(r => r.OrganizationId == orgId).Select(r => r.ImageTypeId).ToList();
        //        var ImageTypesList = db.ImageTypes.AsNoTracking().Where(r => portalImagesTypesIds.Contains(r.ImageTypeId)).ToList();
        //        var ImageTypesids = ImageTypesList.Select(r => r.ImageTypeId).ToList();
        //        //var imagesExtToTtpesList = db.ImagesExtentionsToImageTypes.AsNoTracking().Where(r => ImageTypesids.Contains(r.ImageTypeId)).ToList();
        //        //var imagesExtToTtpesIds = imagesExtToTtpesList.Select(r => r.ImageExtention).Distinct().ToList();
        //        //var imagesExtenstionsList = db.ImagesExtentions.AsNoTracking().Where(r => imagesExtToTtpesIds.Contains(r.ExtentionId)).ToList();



        //        foreach (var item in portalImagesList)
        //        {
        //            var imageView = new ImagePortalCategoryView();
        //            imageView.PortalCategoryNumber = item.CategoryNumber;

        //            var orgImage = organizationImagesList.FirstOrDefault(r => r.PortalCategoriesImageId == item.PortalCategoriesImageId);
        //            if (orgImage != null)
        //            {
        //                imageView.Name = orgImage.FileName.Contains("/") ? orgImage.FileName.Split('/').Last() : orgImage.FileName;
        //                imageView.CreateDate = orgImage.ImageCreationDate;
        //                imageView.Alt = orgImage.Alt.Trim();
        //            }
        //            else
        //            {
        //                imageView.Name = item.FileName.Contains("/") ? item.FileName.Split('/').Last() : item.FileName;
        //                imageView.Alt = item.Alt.Trim();
        //                imageView.CreateDate = item.ImageCreationDate;
        //            }
        //            imageView.PortalCategoriesImageId = item.PortalCategoriesImageId;
        //            var imageType = ImageTypesList.FirstOrDefault(r => r.ImageTypeId == item.ImageTypeId);

        //            if (imageType != null)
        //            {
        //                imageView.ImageTypeId = imageType.ImageTypeId;
        //                imageView.ImageTypeName = imageType.Name;
        //                imageView.Width = imageType.Width;
        //                imageView.Height = imageType.Height;
        //            }

        //            returnImages.Add(imageView);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in ReduceStock, error message: {ex.Message}");
        //        throw;
        //    }

        //    return returnImages;
        //}



        ///// <summary>
        ///// Get active and frozen benefits
        ///// </summary>
        ///// <param name="dbName"></param>
        ///// <returns></returns>
        //public  List<ActiveVariantsView> GetActiveAndFrozenBenefits(string dbName, List<long> testModeIds = null)
        //{
        //    try
        //    {
        //        var orgDb = new DbOrg(dbName);

        //        int days = int.Parse(ConfigurationManager.AppSettings["DaysCheckForMainVariant"]);
        //        var benefits = orgDb.UserPortalCategories.Where(r => r.CategoryFather != 0 && r.CategoryType > 0 && r.CategoryType <= 100 && (r.IsBankLeumiSpecial == true || r.CategoryStatus == 1))
        //            .Select(r => new ActiveVariantsView() { CategoryNumber = r.CategoryNumber, BusinessId = r.BusinessID, CategoryName = r.CategoryName, RedimTypeId = r.RedimTypeId, IsRedimTypeOtherMessage = r.IsRedimTypeOtherMessage, RedimTypeOtherMessageText = r.RedimTypeOtherMessageText, LeumiAppName = r.LeumiAppName, UserType = r.UserTypes })
        //            .ToList();

        //        if (testModeIds != null)
        //            benefits = benefits.Where(x => testModeIds.Contains(x.CategoryNumber)).ToList();

        //        return benefits;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetActiveAndFrozenBenefits, error message: {ex.Message}");
        //        throw;
        //    }
        //}


        ////public  List<ActiveVariantsView> GetActiveAndFrozenBenefitsSpecific(List<long> benefitsSpecificIds, string dbName)
        ////{
        ////    try
        ////    {
        ////        var orgDb = new DbOrg(dbName);


        ////        var benefits = orgDb.UserPortalCategories.Where(r => benefitsSpecificIds.Contains(r.CategoryNumber) && r.CategoryFather != 0 && (r.CategoryStatus == 1 || r.CategoryStatus == 2))
        ////            .Select(r => new ActiveVariantsView() { CategoryNumber = r.CategoryNumber, BusinessId = r.BusinessID, CategoryName = r.CategoryName, RedimTypeId = r.RedimTypeId, IsRedimTypeOtherMessage = r.IsRedimTypeOtherMessage, RedimTypeOtherMessageText = r.RedimTypeOtherMessageText, LeumiAppName = r.LeumiAppName })
        ////            .ToList();

        ////        return benefits;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Logger.Error($"Error in GetActiveAndFrozenBenefits, error message: {ex.Message}");
        ////        throw;
        ////    }
        ////}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="variants"></param>
        ///// <param name="DbName"></param>
        ///// <returns></returns>
        //public  List<ProductVarPriceView> GetProductVarPricesFromVariants(List<ProductsVar> variants, string DbName)
        //{
        //    try
        //    {
        //        var orgDetails = DtsCoreLib.Organizations.GetOrganiztionByDbName(DbName);
        //        // PaymentModel // 1 = Only Money, 2 =  Only coins, 3 = combined , 4 = Combined Dynamic
        //        var specsByVars = DtsCoreLib.Variants.GetBusinessSubTypeSpecificationByVariants(variants.Select(x => (DtsCoreLib.Models.Database.Entities.ProductsVar)x).ToList(), DbName);
        //        var specsCurrent = DtsCoreLib.Variants.GetBusinessSubTypeSpecificationCurrents(variants.Select(x => (DtsCoreLib.Models.Database.Entities.ProductsVar)x).ToList(), DbName);
        //        var varintsPrices = new List<ProductVarPriceView>();

        //        foreach (var item in variants)
        //        {
        //            var specByVar = specsByVars.FirstOrDefault(spec => spec.BarCode.Equals(item.FullBarCode));
        //            var specCurrent = specsCurrent.FirstOrDefault(spec => spec.BusinessSubTypeId == item.BusinessSubTypeID);


        //            var price = 0;
        //            var coins = 0;
        //            if (item.PaymentModelId != null)
        //            {
        //                if (item.PaymentModelId != (int)PaymentModel.CoinsOnly)
        //                    price = (int)Math.Round(DtsCoreLib.Variants.GetVariantPrice(item, specByVar, specCurrent, false, orgDetails));
        //                coins = DtsCoreLib.Variants.GetVariantCoins(item, specByVar, specCurrent, false, orgDetails);


        //                varintsPrices.Add(
        //                    new ProductVarPriceView()
        //                    {
        //                        FullBarCode = item.FullBarCode,
        //                        Price = price,
        //                        Coins = coins,
        //                        PaymentModelId = (PaymentModel)item.PaymentModelId
        //                    });
        //            }
        //        }
        //        return varintsPrices;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetProductVarPricesFromVariants, Error message: {ex.Message}");
        //        throw;
        //    }
        //}


        //public  bool UpdateOrInsertCrm(OpenServiceCaseRequest request, int OrganizationOpsId, OrganizationDetails orgDetails)
        //{
        //    var db = new DbDts_Online();
        //    var member = new AllMember();
        //    if (request.MemberId.Length == 16)
        //    {
        //        member = MembershipFunctions.GetMemberByCardId(request.MemberId, orgDetails.DBName);
        //    }
        //    else
        //    {
        //        member = MembershipFunctions.GetMemberByGuid(request.MemberId, orgDetails.DBName);
        //    }

        //    var crm = db.Crms.FirstOrDefault(r => r.ExternalCaseID == request.ExternalCaseNumber);
        //    var insert = 0;
        //    var update = 0;
        //    // insert new row
        //    if (crm == null)
        //    {
        //        crm = new Crm()
        //        {
        //            CRM_Description = request.Description,
        //            CRM_SeverityID = 2,
        //            CRM_SourceID = 9,
        //            CRM_StatusID = 1,
        //            CRM_SubjectID = null,
        //            CRM_TypeID = request.CaseCateogryId,
        //            MemberID = member.MemberId,
        //            CRN_Essence = "",
        //            DateCreate = DateTime.Now,
        //            OpId_Create = OrganizationOpsId,
        //            OrgId = orgDetails.OrgId,
        //            MemberName = member.MemberName,
        //            ExternalCaseID = request.ExternalCaseNumber,
        //            OrderGuid = request.OrderGuid,
        //            OrderConfirmation = request.OrderConfirmation,
        //            OpId_Current = null,
        //            CRM_Result = null,
        //            BusinessId_related = null,
        //        };
        //        db.Crms.Add(crm);
        //        insert = db.SaveChanges();
        //    }
        //    else
        //    {
        //        crm.CRM_StatusID = 6; // המשך טיפול
        //        //crm.CRM_Description = request.Description;
        //        //crm.CRM_TypeID = request.CaseCateogryId;
        //        crm.MemberID = member.MemberId;
        //        crm.MemberName = member.MemberName;
        //        crm.OrderGuid = request.OrderGuid;
        //        crm.OrderConfirmation = request.OrderConfirmation;

        //        // הוספת הערכה חדשה
        //        var crmDetail = new Crm_Detail()
        //        {
        //            CrmID = crm.CrmID,
        //            Date = DateTime.Now,
        //            Description = request.Description,
        //            OpID = OrganizationOpsId
        //        };
        //        db.Crm_Details.Add(crmDetail);
        //        update = db.SaveChanges();

        //    }

        //    // add crm_details


        //    return (insert > 0 || update > 0);
        //}

        //public  bool UpdateCrmForCancelOrder(string dtsMemberId, int orgId, int OrganizationOpsId, string orderGuid, string barcode, string orderConfirmation, string dbName)
        //{
        //    string memberName;

        //    try
        //    {
        //        using (var orgDb = new DbOrg(dbName))
        //        {
        //            memberName = orgDb.AllMembers.FirstOrDefault(x => x.MemberId.Equals(dtsMemberId)).MemberName;
        //            var orderId = OrderFunctions.GetOrderId(orderGuid, dbName);
        //        }

        //        using (var db = new DbDts_Online())
        //        {
        //            db.Crms.Add(new Crm()
        //            {
        //                CRM_Description = "מספר אסמכתא " + orderConfirmation + Environment.NewLine + "ביטול הזמנה - בנק לאומי",
        //                CRM_SeverityID = 1, // ???
        //                CRM_SourceID = 8,
        //                CRM_StatusID = 1,
        //                CRM_SubjectID = 145, // צרכנות - ביטול הזמנה
        //                MemberID = dtsMemberId,
        //                CRN_Essence = "",
        //                DateCreate = DateTime.Now,
        //                OpId_Create = OrganizationOpsId,
        //                MemberName = memberName,
        //                ExternalCaseID = "",
        //                OrderGuid = orderGuid,
        //                OrderConfirmation = orderConfirmation, // ????
        //                CRM_TypeID = null,
        //                OpId_Current = null,
        //                CRM_Result = null,
        //                BusinessId_related = null,
        //                OrgId = orgId
        //            });

        //            db.SaveChanges();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in UpdateCrmForCancleOrder, error message: {ex.Message}");
        //        throw;
        //    }
        //}

        //public  bool CheckMemberForHistadrutApp(string memberID)
        //{
        //    try
        //    {

        //        AllMember_Histadrut memberFromDB; // first check if member already logged today
        //        using (var db = new Histadrut_Db())
        //        {
        //            if (db.AllMembers.Any(x => x.MemberId == memberID))
        //            {
        //                memberFromDB = db.AllMembers.Where(x => x.MemberId == memberID).First();
        //                if (memberFromDB.UserSiteLastLogin != null && memberFromDB.UserSiteLastLogin.Value > DateTime.Today)
        //                    return true;
        //                try
        //                {
        //                    if (db.MembersAllowedLogins.Any(x => x.MemberID == memberID))
        //                        return true;
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, string.Format("Exception in CheckMemberForHistadrutApp (GetMemberFromDB) web service call for member id: {0}, error message: {1}", memberID, ex.Message));
        //        throw ex;
        //    }
        //    try
        //    {
        //        // if member didnt logged today go to histadrut and check him
        //        var watch = System.Diagnostics.Stopwatch.StartNew();
        //        Logger.Info(string.Format("Request to histadrut webservice for user id " + memberID));
        //        //var member = srv.IsMember(memberID, "K4A_Tests", "Knowledge10", "");
        //        var res = SOAPHelper.SendSOAPRequest(ConfigurationManager.AppSettings["HistadrutAPI"], "IsMember", new Dictionary<string, string>() { { "id", memberID } });
        //        watch.Stop();
        //        int memberStatus = GetMemberStatusFromSOAPResponse(res);
        //        Logger.Info(string.Format("Response from histadrut webservice for user id " + memberID + ", Execution Time: " + watch.Elapsed.Seconds + " seconds, " + "xml: " + Serialize(res)));
        //        using (var databaseDts_Logs = new DatabaseDts_Logs())
        //        {
        //            databaseDts_Logs.DataCenters.Add(new DataCenter() { DtsServiceId = 10, Command = "CheckHistadrutMember" = "DtsEcommerseServices", Seconds = watch.Elapsed.Seconds, Milliseconds = watch.Elapsed.Milliseconds, TimeStamp = DateTime.Now, Request = "Request to histadrut webservice for user id " + memberID + " = " + Serialize(res), Response = "Response from histadrut webservice for user id " + memberID, OrganizationId = 48 });
        //            databaseDts_Logs.SaveChanges();
        //        }
        //        var premiumType = memberStatus;

        //        if (premiumType != 4 && premiumType != 0) // member exist and active
        //        {
        //            try
        //            {
        //                using (var db = new Histadrut_Db()) // update db last login time to now
        //                {
        //                    if (db.AllMembers.Any(x => x.MemberId == memberID))
        //                    {
        //                        db.AllMembers.Where(x => x.MemberId == memberID).First().UserSiteLastLogin = DateTime.Now;
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.Error(ex, "Exception in CheckMemberForHistadrutApp UpdateDB UserSiteLastLogin for memberID: " + memberID);
        //            }
        //            return true;
        //        }
        //        else
        //            return false;

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, string.Format("Exception in CheckMemberForHistadrutApp web service call for member id: {0}, error message: {1}", memberID, ex.Message));
        //        throw ex;
        //    }
        //}

        //private  int GetMemberStatusFromSOAPResponse(string res)
        //{
        //    XDocument xDoc = XDocument.Load(new StringReader(res));

        //    var unwrappedResponse = xDoc.Descendants((XNamespace)"http://schemas.xmlsoap.org/soap/envelope/" + "Body")
        //        .First()
        //        .FirstNode;

        //    var status = xDoc.Descendants("Member_Status").First().Value;

        //    return int.Parse(status);
        //}

        //private  string Serialize<T>(T value)
        //{
        //    var serializeXml = "";
        //    if (value == null)
        //    {
        //        return serializeXml;
        //    }
        //    try
        //    {
        //        var xmlserializer = new XmlSerializer(typeof(T));
        //        var stringWriter = new StringWriter();
        //        var writer = XmlWriter.Create(stringWriter);
        //        xmlserializer.Serialize(writer, value);

        //        serializeXml = stringWriter.ToString();

        //        writer.Close();
        //        return serializeXml.Replace("\"", "'");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("exception in Serialize, Error message: " + ex.Message);
        //        throw;
        //    }
        //}

        //public  string GetRedimCode(string orderConfirmation, string dbName)
        //{
        //    var redimCode = string.Empty;

        //    try
        //    {
        //        using (var orgDb = new DbOrg(dbName))
        //        {
        //            var confirmation = long.Parse(orderConfirmation);

        //            redimCode = orgDb.AllOrders.SingleOrDefault(x => x.OrderAsmchta == confirmation).CardNumber;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetRedimCode, orderConfirmation: {orderConfirmation}, error message: {ex.Message}");
        //        throw;
        //    }

        //    return redimCode;
        //}

        //public  int GetFatherRedimTypeId(long benefitId)
        //{
        //    var redimTypeId = -1;

        //    try
        //    {
        //        using (var db = new DbDts_Online())
        //        {
        //            var portalCategory = db.PortalCategories.FirstOrDefault(x => x.CategoryNumber == benefitId);

        //            if (portalCategory.IsRedimTypeOtherMessage)
        //            {
        //                redimTypeId = 999;
        //            }
        //            else
        //            {
        //                redimTypeId = portalCategory.RedimTypeId.Value;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetFatherRedimTypeId, error message: {ex.Message}");
        //        throw;
        //    }

        //    return redimTypeId;
        //}

        ///// <summary>
        ///// If redim type id in productsVars is 0, get the name from the benefit page
        ///// </summary>
        ///// <param name="benefitId"></param>
        ///// <returns></returns>
        //public  string GetFatherRedimTypeName(long benefitId, OrganizationDetails orgDetail)
        //{
        //    var name = string.Empty;

        //    try
        //    {
        //        using (var db = new DbDts_Online())
        //        {
        //            var portalCategory = db.PortalCategories.FirstOrDefault(x => x.CategoryNumber == benefitId);
        //            if (portalCategory.IsRedimTypeOtherMessage)
        //            {
        //                name = portalCategory.RedimTypeOtherMessageText;
        //            }
        //            else
        //            {
        //                name = GetRedimTypeNameById(portalCategory.RedimTypeId.Value, orgDetail);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Error in GetFatherRedimTypeName, error message: {ex.Message}");
        //        throw;
        //    }

        //    return name;
        //}

        //public  string LogToString(StringBuilder logTracking)
        //{
        //    return logTracking != null ? logTracking.ToString().Replace("{", "(").Replace("}", ")") : "";
        //}
        //public  string StopwatchToFormat(System.Diagnostics.Stopwatch sw)
        //{
        //    if (sw == null)
        //    {
        //        return string.Empty;
        //    }
        //    var timeSpan = sw.Elapsed;
        //    return string.Format("{0}:{1}.{2:d2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        //}
    }
}
