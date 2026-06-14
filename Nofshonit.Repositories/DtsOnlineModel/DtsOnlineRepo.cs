using Dynamitey;
using Google.Apis.ServiceUser.v1.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Nofshonit.BL.Utils;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Business;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.DTOs.Coupon;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.DTOs.Tags;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.Log;
using Nofshonit.Logs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using DtsCoreLib.Models.Database.Entities;

namespace Nofshonit.Repositories.DtsOnlineModel
{
    public class DtsOnlineRepo : BaseRepo, IDtsOnlineRepo
    {
        private const string AC_ALL_SCRIPT = "SearchAutoCompleteAll";
        private const string SEARCH_ALL_SCRIPT = "SearchCategoryByNameFull";
        private const string GET_CATEGORIES_ALL = "GetCategoriesAllByOnce";
        private const string PROC_TEXT_PARAM = "@Text";
        private const string PROC_SUPER_CATEGORY_PARAM = "@SuperCategory";
        private const string PROC_REGION_PARAM = "@Region";
        private const string PROC_SELECT_TOP_PARAM = "@SelectTop";
        private const string PROC_Premium_Type = "@UserType";
        private const string PROC_ORGID_PARAM = "@OrganizationID";
        private const string PROC_USER_TYPE_PARAM = "@UserType";
        private const string PROC_CATEGORY_NUMBER = "@CategoryNumber";
        private const string PROC_TOP_TAGS = "@TopTags";
        private string imagesServerPath = new ConfigurationManager().GetConfigByValue<string>("imagesServerPath");
        private ICacheManager Cache;
        private IConfigurationManager _configuration;

        public DtsOnlineRepo()
        {
            Cache = Container.Resolve<ICacheManager>();
            _configuration = Container.Resolve<IConfigurationManager>();
        }

        public async Task<int> OrganizationIdByGuid(string guid)
        {
            Organizations org = null;
            using (DTS_OnlineContext _dbContext = new DTS_OnlineContext())
            {
                org = await _dbContext.Organizations.AsNoTracking().FirstOrDefaultAsync(x => x.OrganizationGuid == guid); //.Equals()? 
            }

            return org != null ? org.OrganizationId : 0;
        }

        /// <summary> 
        /// return Dictionary list of RedimTypes, key: RedimTypeId, value: RedimName 
        /// </summary> 
        public Dictionary<int, string> GetRedimTypes()
        {
            var redimTypes = new Dictionary<int, string>();
            using (var dbContext = new DTS_OnlineContext())
            {
                redimTypes = dbContext.RedimTypes.ToDictionary(x => x.RedimTypeId, x => x.RedimName);
            }
            return redimTypes;
        }
        public string GetCoupon(string couponID, string memberId)
        {
            using (var context = new DTS_OnlineContext())
            {

                SqlConnection conn = (SqlConnection)context.Database.GetDbConnection();
                SqlCommand cmd = new SqlCommand("GetCouponFromStock", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@memberID", SqlDbType.NVarChar).Value = memberId;
                cmd.Parameters.AddWithValue("@cardNumber", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.AddWithValue("@phoneNumber", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.AddWithValue("@StockID", SqlDbType.NVarChar).Value = couponID;
                conn.Open();
                return cmd.ExecuteScalar().ToString();
            }
            /*
             *  list.Add(DataBase.GetStringParam("@memberID", SqlDbType.NVarChar, memberID));
                list.Add(DataBase.GetStringParam("@cardNumber", SqlDbType.NVarChar, string.Empty));
                list.Add(DataBase.GetStringParam("@phoneNumber", SqlDbType.NVarChar, string.Empty));
                list.Add(DataBase.GetStringParam("@StockID", SqlDbType.NVarChar, StockiD));
             */
            var dtsConnectionString = string.Format(_configuration.GetConnectionStringByValue<string>("DTSOnlineContext"), ContextManager.CurrentOrganization().DBName);
            //using (var context = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
            using (var connection = new System.Data.SqlClient.SqlConnection(dtsConnectionString))
            //using (var connection = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
            {
                List<WalletData> cats = new List<WalletData>();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetCouponFromStock";
                command.Parameters.AddWithValue("@memberID", memberId);
                command.Parameters.AddWithValue("@cardNumber", "");
                command.Parameters.AddWithValue("@phoneNumber", "");
                command.Parameters.AddWithValue("@StockID", couponID);
                return command.ExecuteScalar().ToString();
            }
        }
        /// <summary> 
        /// return Dictionary list of RedimTypes, key: RedimTypeId, value: RedimContent 
        /// </summary> 
        public Dictionary<int, string> GetOrganizationRedimTypes(int organizationId)
        {
            var redimTypes = new Dictionary<int, string>();
            using (var dbContext = new DTS_OnlineContext())
            {
                redimTypes = dbContext.OrganizationRedimTypes.AsNoTracking().Where(x => x.OrganizationId == organizationId).ToDictionary(x => x.RedimTypeId, x => x.RedimContent);
            }
            return redimTypes;
        }

        #region Messages 

        /// <summary> 
        /// return all Greetings by organization 
        /// </summary> 
        public async Task<List<GreetingDTO>> GetGreetings(int organizationId)
        {
            List<GreetingDTO> result = new List<GreetingDTO>();
            using (DTS_OnlineContext _dbContext = new DTS_OnlineContext())
            {
                var items = await _dbContext.Messages.AsNoTracking().Where(x => x.OrganizationId == organizationId
                         && x.AdminDescription != null
                        && x.MessageText != null).ToListAsync();
                if (items.Count > 0)
                {
                    result = items.GroupBy(x => x.MessageContext).ToList()
                  .ConvertAll(x => new GreetingDTO()
                  { Type = x.Key, Messages = x.ToList().Select(y => y.MessageText).ToList() });
                }
            }

            return result;
        }

        /// <summary> 
        /// return all Greetings Types by organization 
        /// </summary> 
        public async Task<List<string>> GetGreetingTypes(int organizationId)
        {
            List<string> result = new List<string>();
            using (DTS_OnlineContext _dbContext = new DTS_OnlineContext())
            {
                result = await _dbContext.Messages.AsNoTracking()
                .Where(x => x.OrganizationId == organizationId && x.AdminDescription != null &&
                x.MessageText != null).Select(x => x.AdminDescription).ToListAsync();
            }

            return result;
        }

        /// <summary> 
        /// return all Organization Messages by Type 
        /// </summary> 
        public async Task<List<string>> GetMessagesByType(int organizationId, string type)
        {
            List<string> result = new List<string>();
            using (DTS_OnlineContext _dbContext = new DTS_OnlineContext())
            {
                result = await _dbContext.Messages.AsNoTracking().Where(x => x.OrganizationId == organizationId
                && x.AdminDescription != null
                && x.MessageText != null
                && x.AdminDescription.ToString() == type).Select(x => x.MessageText).ToListAsync();
            }
            return result;
        }

        #endregion

        #region Coupons 

        /// <summary> 
        /// calculate Discount by couponCode for any pice 
        /// </summary> 
        public CouponDiscountDTO GetCouponDiscount(string couponCode, decimal price)
        {
            CouponDiscountDTO item = null;
            using (DTS_OnlineContext _dbContext = new DTS_OnlineContext())
            {
                var couponStock = _dbContext.CouponsStock.FirstOrDefault(x => x.CouponCode == couponCode);

                if (couponStock != null)
                {
                    var couponsStocksDetail = _dbContext.CouponsStocksDetails.FirstOrDefault(x => x.StockId == couponStock.StockId);
                    if (couponsStocksDetail != null)
                    {

                        item = new CouponDiscountDTO()
                        {
                            Coupon = couponStock.CouponCode,
                            DiscountILS = couponsStocksDetail.IsDiscountPercentage.HasValue && !couponsStocksDetail.IsDiscountPercentage.Value ? couponsStocksDetail.DiscountValue : null,
                            DiscountPercentage = couponsStocksDetail.IsDiscountPercentage.HasValue && couponsStocksDetail.IsDiscountPercentage.Value ? couponsStocksDetail.DiscountValue : null,
                            Price = price,
                            FinalPrice = price,
                        };
                        if (couponsStocksDetail.IsDiscountPercentage.HasValue)
                        {
                            if (couponsStocksDetail.IsDiscountPercentage.GetValueOrDefault())
                            {
                                decimal d = 100;
                                item.FinalPrice = item.FinalPrice * ((d - couponsStocksDetail.DiscountValue.GetValueOrDefault()) / d);
                            }
                            else
                            {
                                item.FinalPrice = item.FinalPrice - couponsStocksDetail.DiscountValue.GetValueOrDefault();
                                if (item.FinalPrice < 0)
                                {
                                    item.FinalPrice = 0;
                                }
                            }
                        }

                    }
                }
            }


            return item;
        }
        #endregion

        #region Category 


        /// <summary> 
        /// find top categoies (with father=0 / level=0), and theirs children 
        /// </summary> 
        public List<CategoryHeaderDTO> GetCategoryHeader(int organizationId)
        {
            List<CategoryHeaderDTO> categories = new List<CategoryHeaderDTO>();
            List<CategoryHeaderItemDTO> childs = new List<CategoryHeaderItemDTO>();
            List<AdditionalFatherCategories> additionalFatherCategories = new List<AdditionalFatherCategories>();
            using (var dbContext = new DTS_OnlineContext())
            {
                var query =

                categories = dbContext.OrganizationCategories.Where(x => x.OrganizationId == organizationId && x.FatherId == 0 && (x.UserTypes == 0 || x.UserTypes == ContextManager.CurrentUser().PremiumType))
                        .Join(dbContext.PortalCategories.Where(y => y.CategoryStatus == 1 && y.Visible == true)
                        , c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => new { c.SortOrder, p.CategoryName, c.DisplayName, p.CategoryNumber, c.FatherId, p.CategoryType, p.CategoryUrl })
                        .ToListAsync().Result.ConvertAll(x => new CategoryHeaderDTO
                        {
                            CategoryId = x.CategoryNumber,
                            CategoryName = !string.IsNullOrEmpty(x.DisplayName) ? x.DisplayName : x.CategoryName,
                            SortOrder = x.SortOrder,
                            Image = GetCategoryImages(x.CategoryNumber, organizationId),
                            CategoryType = x.CategoryType,
                            CategoryUrl = x.CategoryUrl,
                        });
                var parentIds = categories.Select(x => x.CategoryId).ToList();
                additionalFatherCategories = dbContext.AdditionalFatherCategories.AsNoTracking().Where(x => x.OrgId == organizationId && parentIds.Contains(x.FatherCategory)).ToList();
                parentIds.AddRange(additionalFatherCategories.Select(x => x.CategoryNumber));
                parentIds = parentIds.Distinct().ToList();
                var joinedList = dbContext.OrganizationCategories.Where(x => x.OrganizationId == organizationId && (x.UserTypes == 0 || x.UserTypes == ContextManager.CurrentUser().PremiumType) && (parentIds.Contains(x.FatherId) || additionalFatherCategories.Select(s => s.CategoryNumber).Contains(x.CategoryNumber)))
                                    .Join(dbContext.PortalCategories.Where(y => y.CategoryStatus == 1 && y.Visible == true)
                                    , c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => new { c.SortOrder, p.CategoryName, c.DisplayName, p.CategoryNumber, c.FatherId, p.CategoryType, p.CategoryUrl })
                                    .ToListAsync().Result;
                var categoryParentIds = joinedList.Select(x => x.CategoryNumber).ToList();
                categoryParentIds = CategoryHasChildren(categoryParentIds, organizationId);
                childs = joinedList.ConvertAll(x => new CategoryHeaderItemDTO
                {
                    CategoryId = x.CategoryNumber,
                    CategoryName = !string.IsNullOrEmpty(x.DisplayName) ? x.DisplayName : x.CategoryName,
                    SortOrder = x.SortOrder,
                    ParentId = x.FatherId,
                    HasChildren = categoryParentIds.Contains(x.CategoryNumber),
                    CategoryType = x.CategoryType,
                    CategoryUrl = x.CategoryUrl,
                });

                foreach (var child in childs.Where(x => x.HasChildren))
                {
                    var isgrandchild = false;
                    var subChilds = GetCategoriesByParentId(child.CategoryId, organizationId, child.CategoryId).Select(x => x.Id).ToList();
                    if (subChilds.Count > 0)
                    {
                        var subHasChilds = CategoryHasChildren(subChilds, organizationId);
                        if (subHasChilds.Count == 0)//subChilds.Count != subHasChilds.Count
                        {
                            isgrandchild = true;
                        }
                    }
                    child.IsAllGrandchildren = isgrandchild;
                }

            }
            foreach (var category in categories)
            {
                var children = childs.Where(x => x.ParentId == category.CategoryId || (additionalFatherCategories.Any(afc => afc.FatherCategory == category.CategoryId && x.CategoryId == afc.CategoryNumber))).OrderBy(x => x.SortOrder).ToList();
                category.Children = children;
                category.IsAllGrandchildren = children.Count > 0 && children.All(x => !x.HasChildren);
                category.IsLeaf = children.Count == 0;
            }
            //categories = categories.Where(x => x.Children.Any()).OrderBy(y => y.SortOrder).ToList(); 

            return categories;
        }


        /// <summary>
        /// Parse FilterParameters string to HashSet<int>
        /// Expected format: comma-separated integers (e.g., "1,2,3" or "1, 2, 3")
        /// </summary>
        private HashSet<int> ParseFilterParameters(string filterParameters)
        {
            var result = new HashSet<int>();
            if (string.IsNullOrWhiteSpace(filterParameters))
                return result;

            var parts = filterParameters.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (int.TryParse(part.Trim(), out int value))
                {
                    result.Add(value);
                }
            }
            return result;
        }

        /// <summary> 
        /// find current category and his children (if level = 1) 
        /// </summary> 
        public CategoryDetailsDTO GetCategoryDetails(long categoryId, int level, int organizationId)
        {
            var _logger = Container.Resolve<ILog>();
            var watch = Stopwatch.StartNew();
            CategoryDetailsDTO category;
            List<CategoryDetailsDTO> categories = null;
            List<int> ImageTypeIds = new List<int>() { 5, 6, 7, 8, 9 };

            categories = CategoryDetails(categoryId, false, organizationId, false);//current category 

            var timeSpan = watch.Elapsed;
            _logger.Info($"GetCategoryDetails' categoryId:{categoryId} CategoryDetails(categoryId:{categoryId},isChildren:{false},orgId:{organizationId}) finish by " + string.Format("{0}:{1}.{2:d2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds));

            var a1 = watch.ElapsedMilliseconds;
            watch.Restart();
            categories = ValidateCategories(categories, organizationId);
            var a2 = watch.ElapsedMilliseconds;
            watch.Restart();
            category = categories?.FirstOrDefault();

            if (category == null)
                return category;

            bool parentFilterEnabled = category.IsFilterEnabled == true;

            if (category.Images != null)
            {
                category.Images = category.Images.Where(x => ImageTypeIds.Contains(x.ImageTypeId)).OrderBy(x => x.ImageTypeId).ToList();
            }
            if (level == 1)
            {
                categories = CategoryDetails(categoryId, true, organizationId, parentFilterEnabled);//all childrens 
                timeSpan = watch.Elapsed;
                _logger.Info($"GetCategoryDetails' categoryId:{categoryId} CategoryDetails(categoryId:{categoryId},isChildren:{true},orgId:{organizationId}) finish by " + string.Format("{0}:{1}.{2:d2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds));

                var a4 = watch.ElapsedMilliseconds;
                watch.Restart();
                categories = ValidateCategories(categories, organizationId);

                category.IsAllGrandchildren = categories.Count > 0 && categories.All(x => x.IsLeaf);

                if (!category.IsAllGrandchildren.GetValueOrDefault())
                {
                    ImageTypeIds = new List<int>() { 10 };//for lobby category 
                }
                foreach (var subcategory in categories.Where(x => x.Images != null))
                {
                    subcategory.Images = subcategory.Images.Where(x => ImageTypeIds.Contains(x.ImageTypeId)).OrderBy(x => x.ImageTypeId).ToList();
                }
                category.SubCategories = categories;

                timeSpan = watch.Elapsed;
                _logger.Info($"GetCategoryDetails' categoryId:{categoryId} ValidateCategories finish by " + string.Format("{0}:{1}.{2:d2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds));
            }
            /* 
            if (level == 2 && !category.IsEvents && category.BusinessId > 0) 
            { 
                category.Locations = BusinessLocation(category.BusinessId); 
            }*/

            if (category.CategoryType == 26 || category.CategoryType == 32 || category.CategoryType == 20) // Consumption or Hotels
            {
                category.IsConsumption = true;
            }

            return category;
        }


        /// <summary> 
        /// find Business Location of SubBranches. if it doesn't have then retured strore (Business) Location 
        /// </summary> 
        private List<LocationDto> BusinessLocation(long businessId)
        {
            List<LocationDto> result = new List<LocationDto>();
            Business business = GetBusiness(businessId);
            List<BusinessSubBranch> list = new List<BusinessSubBranch>();
            using (var dbContext = new DTS_OnlineContext())
            {
                list = dbContext.BusinessSubBranch.AsNoTracking().Where(x => x.BusinessId == businessId && x.Active).ToListAsync().Result;
            }
            var cities = Cities();
            var regions = Regions();
            var businessModes = BusinessModes();
            if (list.Count > 0)
            {
                result = list.ConvertAll(x => new LocationDto()
                {
                    Address = GenerateAddress(x.StoreAddress, x.StoreStreetNumber, x.StoreCityId, cities),
                    Phone = x.StorePhone1,
                    OpenHours = x.OpenHours,
                    CityId = x.StoreCityId,
                    CityName = (cities.ContainsKey(x.StoreCityId.GetValueOrDefault()) ? cities[x.StoreCityId.GetValueOrDefault()].CityName : null),
                    RegionId = (cities.ContainsKey(x.StoreCityId.GetValueOrDefault()) ? cities[x.StoreCityId.GetValueOrDefault()].RegionId : null),
                    RegionName = (cities.ContainsKey(x.StoreCityId.GetValueOrDefault()) ?
                                            (regions.ContainsKey(cities[x.StoreCityId.GetValueOrDefault()].RegionId.GetValueOrDefault())
                                                  ? regions[cities[x.StoreCityId.GetValueOrDefault()].RegionId.GetValueOrDefault()] : null
                                            ) : null
                                ),

                    BusinessModeId = x.BusinessModeId.GetValueOrDefault(1),
                    BusinessModeName = businessModes.ContainsKey(x.BusinessModeId.GetValueOrDefault()) ? businessModes[x.BusinessModeId.GetValueOrDefault()] : null,

                    FriendlyName = x.SubBusinessName,//x.StoreName, 
                    BusinessTaxName = x.BusinessTaxName,
                    BusinessUniqueNumber = !string.IsNullOrEmpty(x.BusinessUniqueNumber) ? x.BusinessUniqueNumber.Trim() : null,
                    GpsPointer_Lat = x.GpsPointerLat,
                    GpsPointer_Lon = x.GpsPointerLon,

                });
            }
            else
            {
                result.Add(new LocationDto()
                {
                    Address = GenerateAddress(business.StoreAddress, business.StoreStreetNumber, business.StoreCityId, cities),
                    Phone = business.StorePhone1,
                    OpenHours = business.OpenHours,
                    CityId = business.StoreCityId,
                    CityName = (cities.ContainsKey(business.StoreCityId.GetValueOrDefault()) ? cities[business.StoreCityId.GetValueOrDefault()].CityName : null),
                    RegionId = (cities.ContainsKey(business.StoreCityId.GetValueOrDefault()) ? cities[business.StoreCityId.GetValueOrDefault()].RegionId : null),
                    RegionName = (cities.ContainsKey(business.StoreCityId.GetValueOrDefault()) ?
                                             (regions.ContainsKey(cities[business.StoreCityId.GetValueOrDefault()].RegionId.GetValueOrDefault())
                                                   ? regions[cities[business.StoreCityId.GetValueOrDefault()].RegionId.GetValueOrDefault()] : null
                                             ) : null
                                 ),

                    BusinessModeId = business.BusinessModeId.GetValueOrDefault(1),
                    BusinessModeName = businessModes.ContainsKey(business.BusinessModeId.GetValueOrDefault()) ? businessModes[business.BusinessModeId.GetValueOrDefault()] : null,

                    FriendlyName = business.StoreName,
                    BusinessTaxName = business.BusinessTaxName,
                    BusinessUniqueNumber = !string.IsNullOrEmpty(business.BusinessUniqueNumber) ? business.BusinessUniqueNumber.Trim() : null,
                    GpsPointer_Lat = business.GpsPointerLat,
                    GpsPointer_Lon = business.GpsPointerLon,

                });

            }

            return result;
        }

        /// <summary> 
        /// find Business Name 
        /// </summary> 
        private string GetBusinessName(string businessIdStr)
        {
            string result = null;
            long businessId = 0;

            if (long.TryParse(businessIdStr, out businessId) && businessId > 0)
            {
                var business = GetBusiness(businessId);
                if (business != null)
                {
                    result = business.StoreName;
                }
            }

            return result;
        }

        /// <summary> 
        /// return single business by businessId 
        /// </summary>> 
        private Business GetBusiness(long businessId)
        {
            Business result = null;
            using (var dbContext = new DTS_OnlineContext())
            {
                result = dbContext.Business.AsNoTracking().FirstOrDefault(x => x.BuisnessId == businessId + "");
            }

            return result;
        }

        /// <summary> 
        /// return List businesses by businessIds 
        /// </summary> 
        private List<Business> GetBusiness(List<long> businessIds)
        {
            List<Business> result = null;
            var businessIdList = businessIds.Select(x => x + "").ToList();
            using (var dbContext = new DTS_OnlineContext())
            {
                result = dbContext.Business.Where(x => businessIdList.Contains(x.BuisnessId)).ToList();
            }

            return result;
        }


        /// <summary> 
        /// return Dictionary list of BusinessModes, key: BusinessId, value: ModeName 
        /// </summary> 
        private Dictionary<int, string> BusinessModes()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            using (var dbContext = new DTS_OnlineContext())
            {
                result = dbContext.BusinessModes.AsNoTracking().ToDictionaryAsync(x => x.BusinessModeId, x => x.BusinessModeName).Result;
            }
            return result;
        }

        private string GenerateAddress(string StoreAddress, string StoreStreetNumber, int? StoreCityId, Dictionary<int, City> cities)
        {
            var result = StoreAddress + " " + StoreStreetNumber;
            if (StoreCityId.GetValueOrDefault() > 0)
            {
                City city = null;
                var cityName = "";
                if (cities.TryGetValue(StoreCityId.GetValueOrDefault(), out city))
                {
                    cityName = city.CityName;
                }

                if (!string.IsNullOrEmpty(cityName))
                {
                    if (!string.IsNullOrEmpty(result) && result.Contains(cityName) == false)
                    {
                        if (result.Substring(result.Length - 1, 1) == ",")
                            result = result.Substring(0, result.Length - 1);
                        if (string.IsNullOrEmpty(result))
                            result = cityName;
                        else
                            result += ", " + cityName;
                    }

                }
            }
            return result;
        }

        /// <summary> 
        /// return Dictionary list of regions, key: RegionId, value: RegionName 
        /// </summary> 
        private Dictionary<int, string> Regions()
        {

            Dictionary<int, string> result = new Dictionary<int, string>();
            List<Region> list = new List<Region>();
            using (var dbContext = new DTS_OnlineContext())
            {
                list = dbContext.Region.AsNoTracking().ToListAsync().Result;
            }
            if (list.Count > 0)
            {
                result = list.ToDictionary(x => (int)x.RegionId, x => x.RegionName);
            }
            return result;
        }

        public async Task<bool> UpdateSubscriptionToDB(UpdateSubscriptionDTO subscriptionDTO)
        {
            using (var context = new DTS_OnlineContext())
            {
                var row = context.EmailSubscriptionActivity.Where(m => m.MemberId.Equals(subscriptionDTO.Id)).FirstOrDefault();
                // Add new email subscription activity
                if (row == null)
                {
                    await context.EmailSubscriptionActivity.AddAsync(new EmailSubscriptionActivity
                    {
                        MemberId = subscriptionDTO.Id,
                        Email = subscriptionDTO.Email,
                        OrgId = subscriptionDTO.OrganizationId,
                        Type = subscriptionDTO.Subscribed ? 1 : 0,
                        DateAdded = DateTime.Now
                    });
                }
                else
                {
                    // Update email subscription activity
                    row.MemberId = subscriptionDTO.Id;
                    row.Email = subscriptionDTO.Email;
                    row.OrgId = subscriptionDTO.OrganizationId;
                    row.Type = subscriptionDTO.Subscribed ? 1 : 0;
                    row.DateAdded = DateTime.Now;
                }
                var result = await context.SaveChangesAsync();
                return true;
            }
        }

        private Dictionary<int, City> Cities()
        {
            Dictionary<int, City> result = new Dictionary<int, City>();
            var list = (List<City>)Cache.Get(CacheKeys.Cities);
            if (list == null)
            {
                using (var dbContext = new DTS_OnlineContext())
                {
                    list = dbContext.City.AsNoTracking().ToListAsync().Result;
                }
                Cache.Set(CacheKeys.Cities, list, TimeSpan.FromDays(1));
            }
            if (list.Count > 0)
            {
                result = list.ToDictionary(x => x.CityId);
            }
            return result;
        }

        /// <summary> 
        /// remove categories that any of parent is disabled 
        /// </summary> 
        /// <returns></returns> 
        private List<CategoryDetailsDTO> ValidateCategories(List<CategoryDetailsDTO> categories, int organizationId)
        {
            if (categories != null)
            {
                var categoryIds = categories.Select(x => x.CategoryId).ToList();
                ConcurrentBag<long> enabled = new ConcurrentBag<long>();
                Parallel.ForEach(categories, category =>
                {
                    enabled.Add(CheckCategoryIsEnabled(category.CategoryId, organizationId));
                });
                return categories.Where(x => enabled.Contains(x.CategoryId)).ToList();
            }
            return categories;
        }

        /// <summary> 
        /// check all parents for status 1 
        /// </summary> 
        /// <returns></returns> 
        private long CheckCategoryIsEnabled(long categoryNumber, int organizationId)
        {
            bool result = true;
            using (var dbContext = new DTS_OnlineContext())
            {
                var currents = dbContext.OrganizationCategories.AsNoTracking().Where(x => x.OrganizationId == organizationId)// && x.CategoryNumber == categoryNumber 
                    .Join(dbContext.PortalCategories.Where(y => y.CategoryStatus == 1 && y.Visible == true), c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => new { p.CategoryStatus, CategoryFather = c.FatherId, c.CategoryNumber });
                var current = currents.FirstOrDefault(x => x.CategoryNumber == categoryNumber);
                long parentId = 0;
                if (current != null)
                {
                    parentId = current.CategoryFather;
                }
                while (result && parentId > 0)
                {
                    var next = currents.FirstOrDefault(x => x.CategoryNumber == parentId);
                    parentId = 0;
                    if (next != null)
                    {
                        if (next.CategoryStatus != 1)
                        {
                            result = false;
                        }
                        parentId = next.CategoryFather;
                    }
                }
            }

            return result ? categoryNumber : 0;
        }

        private List<long> GetAdditionalFatherChildrenCategories(long parentCategoryId, int organizationId)
        {
            var additionalFatherCategories = new List<long>();
            using (var dbContext = new DTS_OnlineContext())
            {
                additionalFatherCategories = dbContext.AdditionalFatherCategories.AsNoTracking().Where(x => x.OrgId == organizationId && x.FatherCategory == parentCategoryId)
                    .Join(dbContext.PortalCategories.Where(y => y.CategoryStatus == 1 && y.Visible == true), a => a.CategoryNumber, p => p.CategoryNumber, (a, p) => a.CategoryNumber)
                    .Distinct().ToList();
            }
            return additionalFatherCategories;
        }
        private List<long> GetAdditionalFatherParentCategories(long childCategoryId, int organizationId)
        {
            var additionalFatherCategories = new List<long>();
            using (var dbContext = new DTS_OnlineContext())
            {
                additionalFatherCategories = dbContext.AdditionalFatherCategories.AsNoTracking().Where(x => x.OrgId == organizationId && x.CategoryNumber == childCategoryId)
                    .Join(dbContext.PortalCategories.AsNoTracking().Where(y => y.CategoryStatus == 1 && y.Visible == true), a => a.CategoryNumber, p => p.CategoryNumber, (a, p) => (long)a.FatherCategory)
                    .Distinct().ToList();
            }
            return additionalFatherCategories;
        }

        /// <summary> 
        /// get category or category children Details (by "isChildren" flag) 
        /// some information we don't need in case of children, like Breadcrumbs 
        /// </summary> 
        private List<CategoryDetailsDTO> CategoryDetails(long categoryId, bool isChildren, int organizationId, bool parentFilterEnabled)
        {

            List<CategoryDetailsDTO> categories = null;
            var redimTypes = GetOrganizationRedimTypes(organizationId);
            var additionalFatherCategories = new List<long>();
            using (var dbContext = new DTS_OnlineContext())
            {
                if (isChildren)
                {
                    additionalFatherCategories = GetAdditionalFatherChildrenCategories(categoryId, organizationId);
                }
                var joinedList = dbContext.OrganizationCategories.AsNoTracking().Where(x => x.OrganizationId == organizationId && (x.UserTypes == 0 || x.UserTypes == ContextManager.CurrentUser().PremiumType) && ((isChildren && (x.FatherId == categoryId || additionalFatherCategories.Contains(x.CategoryNumber))) || (!isChildren && x.CategoryNumber == categoryId)))
                               .Join(dbContext.PortalCategories.AsNoTracking().Where(y => y.CategoryStatus == 1 && y.Visible == true),
                               c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => new
                               {
                                   BusinessId = string.IsNullOrEmpty(p.BusinessId) ? 0 : long.Parse(p.BusinessId),
                                   c.CampaignDetails,
                                   CategoryDescription = ContextManager.CurrentUser().PremiumType == 3 ? c.CategoryDescription : c.CategoryDescriptioRregularPrice,
                                   CategoryDesign = c.CategoryDesign.HasValue ? c.CategoryDesign.Value : 0,
                                   p.DesignDisplay,
                                   CategoryName = !string.IsNullOrEmpty(c.DisplayName.Trim()) ? c.DisplayName.Trim() : p.CategoryName.Trim(),
                                   CategoryId = c.CategoryNumber,
                                   c.Remarks,
                                   c.SortOrder,
                                   p.CategoryType,
                                   p.Terms,
                                   p.IsRedimTypeOtherMessage,
                                   p.RedimTypeOtherMessageText,
                                   RedimTypeId = p.RedimTypeId.GetValueOrDefault(),
                                   ShortDescription = c.ShortMarketingDescription,
                                   ParentId = c.FatherId,
                                   CategoryUrl = p.CategoryUrl,
                                   IsFilterEnabled = c.IsFilterEnabled ?? false,
                                   FilterParameters = c.FilterParameters,
                                   PricesWithOrgCard = c.PricesWithOrgCard,
                                   PricesWithoutOrgCard = c.PricesWithoutOrgCard,
                                   RegionsByBusiness = c.RegionsByBusiness,
                                   RegionsByVariants = c.RegionsByVariants,
                                   CitiesByBusiness = c.CitiesByBusiness,
                                   CitiesByVariants = c.CitiesByVariants,
                                   DateRanges = c.DateRanges,
                                   CitiesByOther = c.CitiesByOther,
                                   RegionsByOther = c.RegionsByOther,
                                   LocationSourceType = c.LocationSourceType

                               })
                               .ToList();
                //var categoryParentIds = joinedList.Select(x => x.p.CategoryNumber).ToList(); 
                //categoryParentIds = CategoryHasChildren(categoryParentIds, organizationId); 
                var businesses = GetBusinessByIds(joinedList.Where(x => x.BusinessId > 0).Select(x => x.BusinessId).Distinct().ToList()).ToDictionary(x => x.BusinessId, x => x);
                var getVariantByCategoryId = Container.Resolve<IProductService>().GetVariantsByCategoryId(categoryId);
                string minimumInventoryForSale = null;

                if (getVariantByCategoryId.Count != 0)
                {
                    int? variantSubType = Container.Resolve<IProductService>().GetVariantsByCategoryId(categoryId)[0].BusinessSubTypeId;
                    List<string> minimumInventoryForSaleList = dbContext.Messages.AsNoTracking()
                                            .Where(mc => mc.OrganizationId == organizationId &&
                                                         dbContext.MessageRules
                                                             .Where(mr => mr.RuleType == 1 && mr.RuleNumber == variantSubType)
                                                             .Select(mr => mr.MessageId)
                                                             .Contains(mc.MessageId))
                                            .Select(mc => mc.MessageText)
                                            .ToList();
                    minimumInventoryForSale = string.Join("\n", minimumInventoryForSaleList);
                }

                categories = joinedList.ConvertAll(x =>
                {
                    bool shouldLoadFilterData = parentFilterEnabled;

                    return new CategoryDetailsDTO
                    {
                        BusinessId = x.BusinessId,
                        SupplierName = GetBusinessName(x.BusinessId + ""),
                        CampaignDetails = x.CampaignDetails,
                        Description = x.CategoryDescription,
                        CategoryDesign = x.CategoryDesign,
                        CategoryHTML = x.DesignDisplay,
                        CategoryName = x.CategoryName,
                        CategoryId = x.CategoryId,
                        Remarks = x.Remarks,
                        MinimumInventoryForSale = minimumInventoryForSale,
                        ShortDescription = x.ShortDescription,
                        SortOrder = x.SortOrder,
                        TermsOfUse = x.Terms,
                        CategoryType = x.CategoryType,
                        RedimType = x.IsRedimTypeOtherMessage == true ? x.RedimTypeOtherMessageText
                                                                                  : (redimTypes.ContainsKey(x.RedimTypeId) ? redimTypes[x.RedimTypeId] : ""),
                        Images = GetCategoryImages(x.CategoryId, organizationId),//!isChildren ? GetCategoryImages(x.p.CategoryNumber) : null,  
                        IsLeaf = !CategoryHasChildren(x.CategoryId, organizationId),
                        IsEvents = IsEvents(categoryId, organizationId),
                        Variants = null,
                        Events = null,
                        Locations = null,
                        Business = businesses.ContainsKey(x.BusinessId) ? businesses[x.BusinessId] : null, //GetBusinessByIds(new List<long>() { x.BusinessId }).FirstOrDefault() ,//: null, 
                        Breadcrumbs = !isChildren ? GetBreadcrumbsByCategoryId(x.CategoryId, organizationId) : null,
                        SameLevelCategories = null,//GetCategoriesByParentId(x.ParentId, organizationId, x.CategoryId),
                        ParentId = x.ParentId,
                        MustKnow = !isChildren ? GetMessageByCategoryId(x.CategoryId, organizationId) : null,
                        CategoryUrl = x.CategoryUrl,
                        IsConsumption = (x.CategoryType == 26 || x.CategoryType == 32 || x.CategoryType == 20) ? true : false,
                        IsFilterEnabled = !isChildren ? x.IsFilterEnabled : false,
                        FilterParameters = !isChildren ? ParseFilterParameters(x.FilterParameters) : null,
                        Prices = shouldLoadFilterData ? ParseDecimalHashSet(ContextManager.CurrentUser().ClubCreditCard > 0 ? x.PricesWithOrgCard : x.PricesWithoutOrgCard) : null,
                        RegionIds = shouldLoadFilterData ? ResolveLocationIds(x.LocationSourceType, x.RegionsByBusiness, x.RegionsByVariants, x.RegionsByOther) : null,
                        CityIds = shouldLoadFilterData ? ResolveLocationIds(x.LocationSourceType, x.CitiesByBusiness, x.CitiesByVariants, x.CitiesByOther) : null,
                        DateRanges = shouldLoadFilterData ? ParseDateRanges(x.DateRanges) : null,
                    };

                }).OrderBy(x => x.SortOrder).ToList();
            }
            if (!isChildren)
            {
                var first = categories.FirstOrDefault();
                if (first != null && first.Breadcrumbs != null && first.Breadcrumbs.Count >= 1)
                {
                    first.SameLevelCategories = GetCategoriesByParentId(first.Breadcrumbs.First().Id, organizationId, first.CategoryId);
                }

                var needFindBuisiness = categories.Where(x => !x.IsEvents && x.BusinessId > 0);
                ConcurrentBag<long> businessIds = new ConcurrentBag<long>(needFindBuisiness.Select(x => x.BusinessId).Distinct());
                ConcurrentDictionary<long, List<LocationDto>> locationList = new ConcurrentDictionary<long, List<LocationDto>>();
                Parallel.ForEach(businessIds, businessId =>
                {
                    var location = BusinessLocation(businessId);
                    if (location != null)
                    {
                        locationList.TryAdd(businessId, location);
                    }
                });
                foreach (var category in needFindBuisiness)
                {
                    List<LocationDto> locations;
                    if (locationList.TryGetValue(category.BusinessId, out locations))
                    {
                        category.Locations = locations;
                    }
                }
            }
            return categories;




        }

        private HashSet<int> ResolveLocationIds(int? locationSourceType, string byBusiness, string byVariants, string byOther)
        {
            string source = null;

            if (locationSourceType.HasValue)
            {
                switch (locationSourceType.Value)
                {
                    case 1:
                        source = byBusiness;
                        break;

                    case 2:
                        source = byVariants;
                        break;

                    case 3:
                        source = byOther;
                        break;
                }
            }

            return ParseIntHashSet(source);
        }



        /// <summary> 
        /// return list of images by category 
        /// </summary> 
        private List<ImageDTO> GetCategoryImages(long categoryId, int organizationId, List<int> imageTypeIds = null)
        {
            List<ImageDTO> images = new List<ImageDTO>();
            if (imageTypeIds == null || imageTypeIds.Count == 0)
            {
                imageTypeIds = new List<int>() { 5, 6, 7, 8, 9, 10, 16, 18, 17, 19, 20 };
            }

            using (var dbContext = new DTS_OnlineContext())
            {
                var portalImages = dbContext.PortalCategoriesImages.Where(x => x.CategoryNumber == categoryId && imageTypeIds.Contains(x.ImageTypeId)).OrderBy(x => x.ImageTypeId).ToList();
                //if(portalImages.Count > 0)
                //{
                //    var organizationImages = dbContext.OrganizationCategoriesImages.Where(x => x.OrganizationId == organizationId && portalImages.Select(r => r.PortalCategoriesImageId).Contains(x.PortalCategoriesImageId)).ToList();
                //    if (organizationImages.Count() == 0)
                //        organizationImages = dbContext.OrganizationCategoriesImages.Where(x => portalImages.Select(r => r.PortalCategoriesImageId).Contains(x.PortalCategoriesImageId)).ToList();
                //    images = organizationImages.Select(x => new ImageDTO()
                //    {
                //        Alt = x.Alt,
                //        File = x.FileName.ToLower().Contains("") ? $"{imagesServerPath}/{x.FileName}" : x.FileName,
                //        ImageTypeId = x.ImageTypeId
                //    }).ToList();
                //}

                foreach (var pi in portalImages)
                {
                    var newImage = new ImageDTO();

                    var organizationImage = dbContext.OrganizationCategoriesImages.FirstOrDefault(x => x.OrganizationId == organizationId && x.PortalCategoriesImageId == pi.PortalCategoriesImageId);
                    if (organizationImage != null)
                    {
                        newImage.Alt = organizationImage.Alt;
                        newImage.File = organizationImage.FileName;
                        newImage.ImageTypeId = organizationImage.ImageTypeId;
                        newImage.ExternalUrl = organizationImage.ExternalUrl;

                        if (!newImage.File.ToLower().Contains("newuploads"))
                            newImage.File = imagesServerPath + "/" + newImage.File;

                        images.Add(newImage);
                    }
                    //else
                    //{
                    //    newImage.Alt = pi.Alt;
                    //    newImage.File = pi.FileName;
                    //    newImage.ImageTypeId = pi.ImageTypeId;
                    //}
                }
                if (images.Count() == 0)
                {
                    foreach (var pi in portalImages)
                    {
                        var newImage = new ImageDTO();
                        newImage.Alt = pi.Alt;
                        newImage.File = pi.FileName;
                        newImage.ImageTypeId = pi.ImageTypeId;

                        if (!newImage.File.ToLower().Contains("newuploads"))
                            newImage.File = imagesServerPath + "/" + newImage.File;

                        images.Add(newImage);
                    }
                }
            }
            return images;
        }


        /// <summary> 
        /// check if category has children 
        /// </summary> 
        private bool CategoryHasChildren(long categoryId, int organizationId)
        {
            bool result = false;
            var additionalFatherCategories = new List<long>();
            additionalFatherCategories = GetAdditionalFatherChildrenCategories(categoryId, organizationId);
            if (additionalFatherCategories.Count > 0)
            {
                result = true;
            }
            if (!result)
            {
                using (var dbContext = new DTS_OnlineContext())
                {
                    var categoriesByOrganization = dbContext.OrganizationCategories.AsNoTracking().Where(x => x.OrganizationId == organizationId && x.CategoryNumber != categoryId)
                    .Join(dbContext.PortalCategories.AsNoTracking().Where(y => y.CategoryStatus == 1 && y.Visible == true),
                    c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => c);
                    result = categoriesByOrganization.Any(x => x.FatherId == categoryId);
                }
            }

            return result;
        }

        /// <summary> 
        /// return categories that has children 
        /// </summary> 
        /// <returns></returns> 
        private List<long> CategoryHasChildren(List<long> categoryParentIds, int organizationId)
        {
            List<long> result = new List<long>();
            var addition = new ConcurrentBag<long>();
            Parallel.ForEach(categoryParentIds, categoryParentId =>
            {
                var additionalFatherCategories = GetAdditionalFatherChildrenCategories(categoryParentId, organizationId);
                if (additionalFatherCategories.Count > 0)
                {
                    addition.Add(categoryParentId);
                }
            });

            using (var dbContext = new DTS_OnlineContext())
            {
                result = (from o in dbContext.OrganizationCategories
                          join p in dbContext.PortalCategories on o.CategoryNumber equals p.CategoryNumber
                          where o.OrganizationId == organizationId && (categoryParentIds.Contains(o.FatherId))
                          && p.CategoryStatus == 1
                          && p.Visible == true
                          select o.FatherId)
                          .Distinct()
                          .ConvertAll<long>()
                          .ToList();
            }
            if (addition.Count > 0)
            {
                result.AddRange(addition);
                result = result.Distinct().ToList();
            }

            return result;
        }

        /// <summary> 
        /// check if category type is Events by organization 
        /// </summary> 
        public bool IsEvents(long categoryId, int organizationId)
        {
            bool result = false;

            using (var dbContext = new DTS_OnlineContext())
            {
                var buissness = dbContext.PortalCategories.Where(x => x.CategoryNumber == categoryId)
                    .Join(dbContext.Business.Where(x => x.Active.Value && x.IsTicketsHub.HasValue && x.IsTicketsHub.Value == true), pc => pc.BusinessId, b => b.BuisnessId, (pc, b) => new { pc, b })
                    .Join(dbContext.OrganizationCategories.Where(o => o.OrganizationId == organizationId), bPc => bPc.pc.CategoryNumber, oc => oc.CategoryNumber, (bPc, oc) => new { bPc.b.IsTicketsHub })
                    .FirstOrDefault();
                if (buissness != null)
                {
                    result = buissness.IsTicketsHub.GetValueOrDefault();
                }
            }

            return result;
        }

        /// <summary> 
        /// check if category type is Events by organization 
        /// </summary> 
        public bool IsEvents(List<long> categories, int organizationId)
        {
            bool result = false;

            using (var dbContext = new DTS_OnlineContext())
            {
                var buissness = dbContext.PortalCategories.Where(x => categories.Contains(x.CategoryNumber))
                    .Join(dbContext.Business.Where(x => x.Active.Value && x.IsTicketsHub.HasValue && x.IsTicketsHub.Value == true), pc => pc.BusinessId, b => b.BuisnessId, (pc, b) => new { pc, b })
                    .Join(dbContext.OrganizationCategories.Where(o => o.OrganizationId == organizationId), bPc => bPc.pc.CategoryNumber, oc => oc.CategoryNumber, (bPc, oc) => new { bPc.b.IsTicketsHub })
                    .FirstOrDefault();
                if (buissness != null)
                {
                    result = buissness.IsTicketsHub.GetValueOrDefault();
                }
            }

            return result;
        }

        /// <summary> 
        /// find all category parents, up to level = 0 
        /// Order by ASC 
        /// </summary> 
        private List<SimpleLong> GetBreadcrumbsByCategoryId(long categoryId, int organizationId)
        {

            List<SimpleLong> model = new List<SimpleLong>();
            var currentId = categoryId;
            using (var dbContext = new DTS_OnlineContext())
            {
                var categoriesByOrganization = dbContext.OrganizationCategories.AsNoTracking().Where(x => x.OrganizationId == organizationId)
                .Join(dbContext.PortalCategories.Where(y => y.CategoryStatus == 1 && y.Visible == true),
                c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => new { Name = !string.IsNullOrEmpty(c.DisplayName.Trim()) ? c.DisplayName.Trim() : p.CategoryName.Trim(), CategoryFather = c.FatherId, CategoryNumber = c.CategoryNumber });
                while (currentId > 0)
                {
                    var first = categoriesByOrganization.FirstOrDefault(x => x.CategoryNumber == currentId);
                    if (first != null)
                    {
                        model.Add(new SimpleLong() { Id = currentId, Name = first.Name });
                        first = categoriesByOrganization.FirstOrDefault(x => x.CategoryNumber == currentId);
                        currentId = 0;
                        if (first != null)
                        {
                            currentId = first.CategoryFather;
                        }
                    }
                    else
                    {
                        currentId = 0;
                        var fatherKeys = GetAdditionalFatherParentCategories(currentId, organizationId);

                        if (fatherKeys.Count > 0)
                        {
                            var keys = fatherKeys.Where(x => !model.Select(y => y.Id).Contains(x)).ToList();
                            if (keys.Count > 0)
                            {
                                var categoriesByOrganizationByFather = dbContext.OrganizationCategories.AsNoTracking().Where(x => x.OrganizationId == organizationId && keys.Contains(x.CategoryNumber))
                                    .Join(dbContext.PortalCategories.Where(y => y.CategoryStatus == 1 && y.Visible == true),
                                    c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => new { Name = !string.IsNullOrEmpty(c.DisplayName.Trim()) ? c.DisplayName.Trim() : p.CategoryName.Trim(), CategoryFather = c.FatherId, CategoryNumber = c.CategoryNumber }).OrderBy(x => x.CategoryNumber).FirstOrDefault();
                                if (categoriesByOrganizationByFather != null)
                                {
                                    currentId = categoriesByOrganizationByFather.CategoryFather;
                                }
                            }
                        }

                    }
                }
            }
            model.Reverse();
            return model;
        }

        /// <summary> 
        /// all category messages 
        /// </summary> 
        /// <returns></returns> 
        private string GetMessageByCategoryId(long categoryId, int organizationId)
        {
            var result = "";
            using (var dbContext = new DTS_OnlineContext())
            {
                result = dbContext.MessageCategories.AsNoTracking().Where(x => x.OrganizationId == organizationId && x.CategoryNumber == categoryId).Join(dbContext.Messages.AsNoTracking().Where(x => x.OrganizationId == organizationId), mc => mc.MessageId, m => m.MessageId, (mc, m) => m.MessageText).FirstOrDefault();
            }
            return result;
        }

        /// <summary> 
        /// return category list by parentId 
        /// </summary> 
        /// <returns></returns> 
        private List<SimpleLong> GetCategoriesByParentId(long parentiId, int organizationId, long categoryId = 0)
        {
            List<SimpleLong> model = new List<SimpleLong>();
            var additionalFatherCategories = new List<long>();
            additionalFatherCategories = GetAdditionalFatherChildrenCategories(parentiId, organizationId);
            using (var dbContext = new DTS_OnlineContext())
            {
                model = dbContext.OrganizationCategories.AsNoTracking().Where(x => x.OrganizationId == organizationId && (x.FatherId == parentiId || additionalFatherCategories.Contains(x.CategoryNumber)) && x.CategoryNumber != categoryId)
                .Join(dbContext.PortalCategories.AsNoTracking().Where(y => y.CategoryStatus == 1 && y.Visible == true),
                c => c.CategoryNumber, p => p.CategoryNumber, (c, p) => new SimpleLong { Name = !string.IsNullOrEmpty(c.DisplayName.Trim()) ? c.DisplayName.Trim() : p.CategoryName.Trim(), Id = c.CategoryNumber }).OrderBy(x => x.Id).ToList();
            }

            return model;
        }

        public SimpleInt RedimTypeIdByCategoryId(long categoryId, int organizationId)
        {
            SimpleInt simple = new SimpleInt() { Id = -1 };

            using (var dbContext = new DTS_OnlineContext())
            {
                var portalCategory = dbContext.PortalCategories.AsNoTracking().FirstOrDefault(x => x.CategoryNumber == categoryId);
                if (portalCategory != null)
                {
                    if (portalCategory.IsRedimTypeOtherMessage)
                    {
                        simple.Id = 999;
                        simple.Name = portalCategory.RedimTypeOtherMessageText;
                    }
                    else
                    {
                        simple.Id = portalCategory.RedimTypeId.GetValueOrDefault(simple.Id);
                        simple.Name = dbContext.OrganizationRedimTypes.AsNoTracking().FirstOrDefault(redim => redim.RedimTypeId == simple.Id && redim.OrganizationId == organizationId)?.RedimContent;
                    }
                }

            }
            return simple;
        }
        public Dictionary<int, string> RedimTypesByOrganization(int organizationId)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            using (var dbContext = new DTS_OnlineContext())
            {
                data = dbContext.OrganizationRedimTypes.AsNoTracking().Where(x => x.OrganizationId == organizationId).AsNoTracking().ToDictionary(x => x.RedimTypeId, x => x.RedimContent);
            }

            return data;
        }

        public List<TypeImplementationDate> GetImplementationTypesList()
        {
            var result = new List<TypeImplementationDate>();

            using (var dbContext = new DTS_OnlineContext())
            {
                result = dbContext.TypeImplementationDate.ToList();
            }

            return result;
        }

        public OrganizationCategories GetOrganizationCategory(long categoryNumber)
        {
            using (var dbContext = new DTS_OnlineContext())
            {
                return dbContext.OrganizationCategories.FirstOrDefault(x => x.CategoryNumber == categoryNumber && x.OrganizationId.ToString() == ContextManager.CurrentOrganization().OrgId.ToString());
            }
        }
        #endregion


        #region Address 

        public List<StreetDTO> GetStreetsListById(int cityId)
        {
            using (DTS_OnlineContext dbContext = new DTS_OnlineContext())
            {

                var streetList = dbContext.City.AsNoTracking().Where(c => c.CityId == cityId)
                            .Join(dbContext.Street, c => c.GovId, s => s.CityId, (c, s) => new { s, c })
                            .Join(dbContext.Region, sc => sc.c.RegionId, r => r.RegionId, (sc, r) => new { sc, r })
                            .Where(t => t.sc.c.CityId == cityId)
                            .Select(m => new StreetDTO
                            {
                                CityId = m.sc.c.CityId,
                                CityName = m.sc.c.CityName,
                                GovId = m.sc.c.GovId,
                                IsraelPostId = m.sc.c.IsraelPostId,
                                RegionID = m.r.RegionId,
                                RegionName = m.r.RegionName,
                                StreetId = m.sc.s.StreetId,
                                StreetName = m.sc.s.StreetName
                            }).ToList();

                if (streetList == null)
                    throw new Exception("Any problem with GetStreetsById");

                return streetList;
            }

        }
        public List<CityDTO> GetCitiesList()
        {
            using (DTS_OnlineContext dbContext = new DTS_OnlineContext())
            {
                var streetList = dbContext.City.AsNoTracking().Join(dbContext.Region, c => c.RegionId, r => r.RegionId, (c, r) => new { c, r })
                              .Select(m => new CityDTO
                              {
                                  CityId = m.c.CityId,
                                  GovId = m.c.GovId,
                                  CityName = m.c.CityName,
                                  RegionID = m.r.RegionId
                              }).ToList();

                if (streetList == null)
                    throw new Exception("Any problem with GetCities");

                return streetList;
            }
        }

        public List<RegionDTO> GetRegionsList()
        {
            using (DTS_OnlineContext dbContext = new DTS_OnlineContext())
            {
                var regionsList = dbContext.Region.AsNoTracking()
                              .Select(m => new RegionDTO
                              {
                                  RegionId = m.RegionId,
                                  RegionName = m.RegionName
                              }).ToList().FindAll(r => !r.RegionName.Contains("אחר"));

                if (regionsList == null)
                    throw new Exception("Any problem with GetRegion");

                return regionsList;
            }
        }



        #endregion


        public async Task<object> AddContactToSMSQueue(SmsQueue smsQueue)
        {
            using (var context = new DTS_OnlineContext())
            {

                SqlConnection conn = (SqlConnection)context.Database.GetDbConnection();
                SqlCommand cmd = new SqlCommand("[SmsQueue_Insert]", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("OrganizationID", SqlDbType.Int).Value = smsQueue.OrganizationId;
                cmd.Parameters.AddWithValue("MemberID", SqlDbType.NVarChar).Value = smsQueue.MemberId;
                cmd.Parameters.AddWithValue("CouponID", SqlDbType.BigInt).Value = smsQueue.CouponId;
                cmd.Parameters.AddWithValue("SenderName", SqlDbType.NVarChar).Value = smsQueue.SenderName;
                cmd.Parameters.AddWithValue("Subscribers", SqlDbType.VarChar).Value = smsQueue.Subscribers;
                cmd.Parameters.AddWithValue("Message", SqlDbType.NVarChar).Value = smsQueue.Message;
                cmd.Parameters.AddWithValue("MessageLengh", SqlDbType.Int).Value = smsQueue.MessageLengh;
                cmd.Parameters.AddWithValue("DeliveryDelayInMinutes", SqlDbType.TinyInt).Value = smsQueue.DeliveryDelayInMinutes;
                cmd.Parameters.AddWithValue("ExpirationDelayInMinutes", SqlDbType.TinyInt).Value = smsQueue.ExpirationDelayInMinutes;
                cmd.Parameters.AddWithValue("SmsType", SqlDbType.TinyInt).Value = smsQueue.SmsType;
                cmd.Parameters.AddWithValue("Priority", SqlDbType.TinyInt).Value = smsQueue.Priority;
                cmd.Parameters.AddWithValue("SmsID", SqlDbType.BigInt);
                conn.Open();
                cmd.ExecuteNonQuery();

                //await context.SmsQueue.AddAsync(smsQueue);
                //await context.SaveChangesAsync();
                return new
                {
                    Success = true,
                };
            }
        }

        public void AddSmsQueue(SmsQueue smsQueue)
        {
            using (var context = new DTS_OnlineContext())
            {
                SqlConnection conn = (SqlConnection)context.Database.GetDbConnection();
                SqlCommand cmd = new SqlCommand("[SmsQueue_Insert]", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("OrganizationID", SqlDbType.Int).Value = smsQueue.OrganizationId;
                cmd.Parameters.AddWithValue("MemberID", SqlDbType.NVarChar).Value = smsQueue.MemberId;
                cmd.Parameters.AddWithValue("CouponID", SqlDbType.BigInt).Value = smsQueue.CouponId;
                cmd.Parameters.AddWithValue("SenderName", SqlDbType.NVarChar).Value = smsQueue.SenderName;
                cmd.Parameters.AddWithValue("Subscribers", SqlDbType.VarChar).Value = smsQueue.Subscribers;
                cmd.Parameters.AddWithValue("Message", SqlDbType.NVarChar).Value = smsQueue.Message;
                cmd.Parameters.AddWithValue("MessageLengh", SqlDbType.Int).Value = smsQueue.MessageLengh;
                cmd.Parameters.AddWithValue("DeliveryDelayInMinutes", SqlDbType.TinyInt).Value = smsQueue.DeliveryDelayInMinutes;
                cmd.Parameters.AddWithValue("ExpirationDelayInMinutes", SqlDbType.TinyInt).Value = smsQueue.ExpirationDelayInMinutes;
                cmd.Parameters.AddWithValue("SmsType", SqlDbType.TinyInt).Value = smsQueue.SmsType;
                cmd.Parameters.AddWithValue("Priority", SqlDbType.TinyInt).Value = smsQueue.Priority;
                cmd.Parameters.AddWithValue("SmsID", SqlDbType.BigInt);
                conn.Open();
                cmd.ExecuteNonQuery();
                //context.SmsQueue.Add(smsQueue);
                //context.SaveChanges();
            }
        }
        public MwcSeries GetMwcSeriesBySerieId(int serieId)
        {
            using (var context = new DTS_OnlineContext())
            {
                return context.MwcSeries.FirstOrDefault(s => s.SerieId == serieId);
            }
        }

        #region ContactUS 
        public async Task<bool> AddContactToCrmAsync(Crm userCrm)
        {
            using (var context = new DTS_OnlineContext())
            {
                await context.Crm.AddAsync(userCrm);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> AddContactToEmailQueue(EmailQueue emailQueue)
        {
            using (var context = new DTS_OnlineContext())
            {
                await context.EmailQueue.AddAsync(emailQueue);
                await context.SaveChangesAsync();

                return true;
            }
        }
        public int AddContactToEmailQueueSync(EmailQueue emailQueue)
        {
            using (var context = new DTS_OnlineContext())
            {
                context.EmailQueue.Add(emailQueue);
                context.SaveChanges();

                return emailQueue.EmailId;
            }
        }

        public void AddPulseemLog(Pulseem pulseem)
        {
            using (var context = new DTS_OnlineContext())
            {
                context.Pulseem.Add(pulseem);
                context.SaveChanges();

            }
        }

        public async Task<List<CrmGetTypeDTO>> GetCrmTypes()
        {
            using (var context = new DTS_OnlineContext())
            {
                SqlConnection conn = (SqlConnection)context.Database.GetDbConnection();
                SqlCommand cmd = new SqlCommand("[CRM_Type_Get_Org]", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(PROC_ORGID_PARAM, SqlDbType.Int).Value = 20;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<CrmGetTypeDTO> CRMList = new List<CrmGetTypeDTO>();
                while (await reader.ReadAsync())
                    CRMList.Add(new CrmGetTypeDTO { CrmTypeID = (int)reader[0], CrmTypeDescription = reader[1].ToString() });

                return await Task.FromResult(CRMList);
            }

        }

        #endregion

        #region Media 
        public string GetOrganizationNews(int organizationId)
        {
            using (DTS_OnlineContext _dbContext = new DTS_OnlineContext())
            {
                var org = _dbContext.Organizations.FirstOrDefault(o => o.OrganizationId == organizationId);
                if (org != null)
                    return org.OrganizationNews;
                return null;
            }
        }

        /// <summary> 
        /// get images by organization 
        /// </summary> 
        public List<ImageSliderDTO> GetImagesSlider(int organizationId)
        {
            List<ImageSliderDTO> images = null;
            List<int?> populationTypes = new List<int?>() { null, 0, ContextManager.CurrentUser().PremiumType.Value };
            using (var dbContext = new DTS_OnlineContext())
            {
                images = dbContext.ImagesSlider.Where(x => x.ImageOrg == organizationId && x.ImageActive == true && populationTypes.Contains(x.PopulationType)).ToList().ConvertAll(x => new ImageSliderDTO()
                {
                    Alt = x.ImageTitle,
                    ImageUrlBig = x.ImageUrlBig,
                    link = x.ImageLink,
                    SortOrder = x.ImageOrder,
                    ImageUrlSmall = x.ImageUrlSmal
                });
            }
            return images;
        }

        #endregion

        #region Tags 



        private CategoryDetailsDTO GetCategoryByNumber(long categoryNumber)
        {
            try
            {
                var allCategories = (List<CategoryDetailsDTO>)Cache.Get(CacheKeys.GetCategoryByNumberAll);

                if (allCategories == null)
                {
                    allCategories = new List<CategoryDetailsDTO>();

                    int orgId = ContextManager.CurrentOrganization().OrgId;
                    using (var connection = (SqlConnection)new DTS_OnlineContext().Database.GetDbConnection())
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = GET_CATEGORIES_ALL;
                            command.Parameters.AddWithValue("@OrganizationID", orgId);

                            using (var da = new SqlDataAdapter(command))
                            using (var dt = new DataTable())
                            {
                                da.Fill(dt);

                                foreach (DataRow row in dt.Rows)
                                {
                                    allCategories.Add(GetCategory(row, false));
                                }
                            }
                        }
                    }

                    Cache.Set(CacheKeys.GetCategoryByNumberAll, allCategories, TimeSpan.FromHours(1));
                }

                return allCategories.FirstOrDefault(c => c.CategoryId == categoryNumber);
            }
            catch
            {
                throw;
            }
        }


        public TagsCategoriesDTO GetTagsById(int tagId)
        {
            using (var dbContext = new DTS_OnlineContext())
            {

                var selectedTag = dbContext.Tags.AsNoTracking().Where(
                    t => t.TagId == tagId &&
                         t.OrganizationId == ContextManager.CurrentOrganization().OrgId &&
                         t.TagEnable
                ).Include(x => x.TagsCategory).FirstOrDefault();
                if (selectedTag == null)
                    throw new Exception("Tag not found");

                return GetCategoryByTag(selectedTag);
            }

        }



        public List<TagsCategoriesDTO> GetTagsByTop(int topVal, int skipTags = 0)
        {
            using (var dbContext = new DTS_OnlineContext())
            {
                List<TagsCategoriesDTO> tagsCategories = new List<TagsCategoriesDTO>();
                var selectedTags = dbContext.Tags.AsNoTracking()
                    .Where(t => t.OrganizationId == ContextManager.CurrentOrganization().OrgId && t.TagEnable && t.PremiumTypeId == ContextManager.CurrentUser().PremiumType)
                    .OrderBy(t => t.TagSort).Where(t => t.TagSort <= topVal).Skip(skipTags).Include(x => x.TagsCategory).ToList();

                foreach (var item in selectedTags)
                    tagsCategories.Add(GetCategoryByTag(item));

                return tagsCategories;
            }
        }


        /// <summary>
        /// Parse comma-separated string of integers to HashSet<int>
        /// </summary>
        private HashSet<int> ParseIntHashSet(string value)
        {
            var result = new HashSet<int>();
            if (string.IsNullOrWhiteSpace(value))
                return result;

            var parts = value.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (int.TryParse(part.Trim(), out int intValue))
                {
                    result.Add(intValue);
                }
            }
            return result;
        }

        /// <summary>
        /// Parse comma-separated string of decimals to HashSet<decimal>
        /// </summary>
        private HashSet<decimal> ParseDecimalHashSet(string value)
        {
            var result = new HashSet<decimal>();
            if (string.IsNullOrWhiteSpace(value))
                return result;

            var parts = value.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (decimal.TryParse(part.Trim(), out decimal decimalValue))
                {
                    result.Add(decimalValue);
                }
            }
            return result;
        }

        /// <summary>
        /// Parse DateRanges string to HashSet<DateRange>
        /// Expected format: "StartDate1,EndDate1;StartDate2,EndDate2" or similar
        /// </summary>
        private HashSet<DateRange> ParseDateRanges(string dateRangesStr)
        {
            var result = new HashSet<DateRange>();
            if (string.IsNullOrWhiteSpace(dateRangesStr))
                return result;

            // Try to parse as JSON first, then fall back to comma/semicolon separated format
            try
            {
                var parts = dateRangesStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var part in parts)
                {
                    var dateParts = part.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                    if (dateParts.Length >= 2 &&
                        DateTime.TryParse(dateParts[0].Trim(), out DateTime startDate) &&
                        DateTime.TryParse(dateParts[1].Trim(), out DateTime endDate))
                    {
                        result.Add(new DateRange
                        {
                            StartDate = startDate,
                            EndDate = endDate
                        });
                    }
                }

            }
            catch
            {
                // If parsing fails, return empty set
            }

            return result;
        }


        private TagsCategoriesDTO GetCategoryByTag(Tags tags)
        {

            List<CategotyTagInfoDTO> tagInfoDTOs = new List<CategotyTagInfoDTO>();

            bool tagFilterEnabled = tags.IsFilterEnabled == true;

            foreach (var cat in tags.TagsCategory)
            {
                CategoryDetailsDTO categoryDetailsDTO = GetCategoryByNumber(cat.CategoryNumber);


                if (categoryDetailsDTO == null)
                    continue;

                tagInfoDTOs.Add(new CategotyTagInfoDTO
                {
                    Categories = categoryDetailsDTO,// GetCategoryDetails((long)cat.FkCategoryNumberPortalCategories, 0, ContextManager.CurrentOrganization().OrgId),
                    CategoryId = (int)cat.CategoryNumber,
                    CategoryTagSort = cat.CategoryTagSort
                });
            }

            return new TagsCategoriesDTO
            {
                TagName = tags.TagName,
                TagId = tags.TagId,
                TagCategoryInfo = tagInfoDTOs.OrderBy(a => a.CategoryTagSort).ToList(),
                IsFilterEnabled = (bool)tags.IsFilterEnabled,
                FilterParameters = ParseFilterParameters(tags.FilterParameters)

            };

        }
        #endregion
        public void DoCouponRollBack(long CouponStockId, string MemberId)
        {
            try
            {
                using (var db = new DTS_OnlineContext())
                {
                    CouponsStock coupon = db.CouponsStock.FirstOrDefault(c => c.CouponId == CouponStockId && c.MemberId == MemberId);
                    if (coupon != null)
                    {
                        coupon.MemberId = null;
                        coupon.SendingTime = null;
                        coupon.PhoneNumber = null;
                        coupon.CouponStatus = false;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in DoCouponRollBack, error message: {ex.Message}");
                throw;
            }
        }
        public List<BusinessSubTypeNameDTO> GetBussinessSubTypeNames()
        {
            try
            {
                using (var dbContext = new DTS_OnlineContext())
                {
                    return dbContext.BusinessSubType.AsNoTracking().Where(r => r.Active).Select(r => new BusinessSubTypeNameDTO()
                    {
                        Id = r.BusinessSubTypeId,
                        Name = r.BusinessSubTypeNameWeb
                    }).ToList();
                }
            }
            catch
            {
                throw new Exception("Failed to get Bussiness SubType Names");
            }
        }

        #region Business 

        /// <summary> 
        /// find Businesses details by BusinessByIds 
        /// </summary> 
        public List<BusinessDTO> GetBusinessByIds(List<long> businessIdList)
        {
            Dictionary<long, List<BusinessSubBranch>> subBranches = new Dictionary<long, List<BusinessSubBranch>>();
            List<Business> businesesList = new List<Business>();
            Dictionary<int, City> cities = new Dictionary<int, City>();
            if (businessIdList != null && businessIdList.Count > 0)
            {
                subBranches = BusinessSubBranches(businessIdList);
                businesesList = GetBusiness(businessIdList);
                cities = Cities();
            }

            return businesesList.ConvertAll(x => new BusinessDTO
            {
                AboveAge = x.AboveAge,
                Address = GenerateAddress(x.StoreAddress, x.StoreStreetNumber, x.StoreCityId.GetValueOrDefault(), cities),
                Challenging = x.Challenging,
                CityId = x.StoreCityId,
                CrippleAccess = x.CrippleAccess,
                EquipmentRenting = x.EquipmentRenting,
                GpsPointer_Lat = x.GpsPointerLat,
                GpsPointer_Lon = x.GpsPointerLon,
                HaveSubBranch = x.HaveSubBranch,
                ImgUrl = x.BusinessImg,
                LocationExplain = x.LocationExplain,
                Massage = x.Massage,
                Name = x.StoreName,
                OpenHours = x.OpenHours,
                Parking = x.Parking,
                Phone = !string.IsNullOrEmpty(x.StorePhone1) ? x.StorePhone1 : x.StorePhone2,
                Region = x.Region,
                Restaurant = x.Restaurant,
                StreetNumber = x.StoreStreetNumber,
                SumSubBranch = subBranches[long.Parse(x.BuisnessId)].Count,
                Toilet = x.Toilet,
                WebSite = x.WebSite,
                BusinessId = long.Parse(x.BuisnessId)
            });
        }

        /*private string GetBusinessAddress(Dictionary<int,City> cities,int cityId, string storeAddress, int storeStreetNumber)
        {
            var res = "";
            City city = null;
            cities.TryGetValue(cityId, out city);           
            res = storeAddress;
            if (!string.IsNullOrEmpty(res) && city != null)
            {
                res += ", ";
            }
            if (city != null)
            {
                res += city.CityName;
            }
			if(storeStreetNumber != 0)
			{
				res += storeStreetNumber;
			}
            return res;
        }*/

        /// <summary> 
        /// return list SubBranches by businessIds 
        /// key: BusinessId -> value: list of SubBranches 
        /// </summary> 
        public Dictionary<long, List<BusinessSubBranch>> BusinessSubBranches(List<long> businessIds)
        {
            List<BusinessSubBranch> result = new List<BusinessSubBranch>();
            Dictionary<long, List<BusinessSubBranch>> model = new Dictionary<long, List<BusinessSubBranch>>();
            using (var dbContext = new DTS_OnlineContext())
            {
                result = dbContext.BusinessSubBranch.AsNoTracking().Where(x => businessIds.Contains(x.BusinessId)).ToList();
            }
            foreach (var businessId in businessIds.Distinct())
            {
                model.Add(businessId, result.Where(x => x.BusinessId == businessId).ToList());
            }

            return model;
        }
        #endregion


        public async Task<bool> SlinkProcess(string generatedLinkCode)
        {
            using (var dtsContext = new DTS_OnlineContext())
            {
                var currentUser = ContextManager.CurrentUser();
                var dtsNewsletter = dtsContext.DtsNewsletter.FirstOrDefault(c => c.CardNumber.Equals(currentUser.CardNumber));
                if (dtsNewsletter != null && !string.IsNullOrEmpty(dtsNewsletter.LinkCode))
                {
                    var detailsXml = XmlGenerator.XmlToObject<DetailsXml>(dtsNewsletter.XmlDetails);
                    if (detailsXml.PhoneNumber == null || !detailsXml.PhoneNumber.Equals(currentUser.MobilePhone))
                    {
                        detailsXml.PhoneNumber = currentUser.MobilePhone;
                        var xmlStringFormat = XmlGenerator.ObjectToXml<DetailsXml>(detailsXml);
                        dtsContext.Attach(dtsNewsletter);
                        dtsNewsletter.XmlDetails = xmlStringFormat;
                        await dtsContext.SaveChangesAsync();
                    }
                    return await SendSms(dtsNewsletter.LinkCode);
                }
                else
                {
                    var details = new DetailsXml
                    {
                        MemberId = currentUser.Id,
                        PhoneNumber = currentUser.MobilePhone,
                        FirstName = currentUser.FirstName,
                        LastName = currentUser.LastName,
                        Parameters = new DetailsXml.DetailsParameters { param1 = currentUser.CardNumber }
                    };
                    var stringXmlDetails = XmlGenerator.ObjectToXml<DetailsXml>(details);
                    dtsContext.DtsNewsletter.Add(new DtsNewsletter
                    {
                        DateCreated = DateTime.Now,
                        LinkCode = generatedLinkCode,
                        //TODO REPLACE AND EXPORT TO CONFIG
                        ImageName = Container.Resolve<IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.ClubCradSlinkImage),
                        XmlDetails = stringXmlDetails,
                        IsLoadMoney = false,
                        OrgId = ContextManager.CurrentOrganization().OrgId,
                        OrderId = null,
                        CardNumber = currentUser.CardNumber,
                        ActivationCode = null
                    });
                    await dtsContext.SaveChangesAsync();

                    return await SendSms(generatedLinkCode);
                }
            }
        }


        public async Task<bool> SlinkProcess_Join(DateTime? RegistrationDate = null)
        {
            var currentUser = ContextManager.CurrentUser();
            return await SendSms_Join(currentUser.CardNumber, RegistrationDate);
        }

        private async Task<bool> SendSms_Join(string cardNumber, DateTime? RegistrationDate = null)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            List<int> messagesForPoupRegister = new List<int>() { 61238, 61237 };
            var messages = MessagesUtil.GetMessagesByKey(messagesForPoupRegister);
            string messageToSend = RegistrationDate == null ? messages.FirstOrDefault(m => m.MessageKey == 61237).MessageText : messages.FirstOrDefault(m => m.MessageKey == 61238).MessageText;
            if (currentUser.PremiumType == 4)
                messageToSend = string.Format(messageToSend, " לממשיכים", cardNumber);
            else
                messageToSend = string.Format(messageToSend, "", cardNumber);
            AddSmsQueue(new SmsQueue
            {
                DateAdded = DateTime.Now,
                OrganizationId = ContextManager.CurrentOrganization().OrgId,
                MemberId = currentUser.Id,
                SenderName = "Behatsdaa",
                Subscribers = currentUser.MobilePhone,    //subscribers is MobilePhone? 
                Message = messageToSend,
                MessageLengh = messageToSend.Length,
                DeliveryDelayInMinutes = 0,
                ExpirationDelayInMinutes = 120,
                SmsType = 51,
                SeveralAttempts = 0,
                Priority = 100,
                SmsSend = false
            });
            return true;

        }
        private async Task<bool> SendSms(string linkCode)
        {
            var link = Container.Resolve<IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.Slink_Link);
            ResponseUserDTO currentUser = ContextManager.CurrentUser();

            var message = "לצפייה בפרטי כרטיס המועדון של \"בהצדעה\" לחץ על הקישור: " + link + linkCode;
            AddSmsQueue(new SmsQueue
            {
                DateAdded = DateTime.Now,
                OrganizationId = ContextManager.CurrentOrganization().OrgId,
                MemberId = currentUser.Id,
                SenderName = "Behatsdaa",
                Subscribers = currentUser.MobilePhone,    //subscribers is MobilePhone? 
                Message = message,
                MessageLengh = message.Length,
                DeliveryDelayInMinutes = 0,
                ExpirationDelayInMinutes = 120,
                SmsType = 51,
                SeveralAttempts = 0,
                Priority = 0,
                SmsSend = false
            });
            return true;

        }


        #region Stock 

        /// <summary> 
        /// find out if BudgetStockCategory is not empty 
        /// </summary> 
        public bool BudgetStockCategoriesCheck(long categoryId)
        {
            bool inStock = true;
            using (var dtsContext = new DTS_OnlineContext())
            {
                //inStock = dtsContext.BudgetStockCategory.Any(x => x.CategoryNumber.Value == categoryId); 
            }
            return inStock;
        }

        /// <summary> 
        /// find stock by variant barcode and return variant Barcodes by the stock 
        /// </summary> 
        public (Stock, List<string>) GetStockVariantBarcodes(string barcode)
        {
            Stock stock = null;
            List<string> sameStockBarcodes = new List<string>();
            using (var dtsContext = new DTS_OnlineContext())
            {
                var variantStock = dtsContext.VariantStock.FirstOrDefault(x => x.FullBarCode.Equals(barcode));
                if (variantStock != null)
                {
                    stock = dtsContext.Stock.FirstOrDefault(x => x.StockId == variantStock.StockId);
                    if (stock != null)
                    {
                        sameStockBarcodes = dtsContext.VariantStock.Where(x => x.StockId == stock.StockId).Select(x => x.FullBarCode).ToList();
                    }
                }
            }
            return (stock, sameStockBarcodes);
        }

        /// <summary> 
        /// find qty of active coupons by stock  
        /// </summary> 
        public int CouponStock(int cuponStockId)
        {
            var couponsQty = 0;
            using (var dtsContext = new DTS_OnlineContext())
            {
                couponsQty = dtsContext.CouponsStock.Count(x => x.StockId == cuponStockId && !x.CouponStatus.GetValueOrDefault());
            }
            return couponsQty;
        }

        public bool CheckCartVariantBarCodeDtsStock(string barcode)
        {
            using (var dbContext = new DTS_OnlineContext())
            {


                var stocks = dbContext.VariantStock.Where(vs => barcode.Equals(vs.FullBarCode)).GroupBy(g => g.StockId).ToList();

                int cartCount = 0;

                foreach (var item in stocks)
                {
                    var barcodeForStock = item.Select(i => i.FullBarCode).ToList();


                    var stock = dbContext.Stock.FirstOrDefault(vs => vs.StockId == item.Key && vs.Active);

                    if (((stock?.StockQuantity ?? 0) - (stock?.OrdersQuentity ?? 0)) < cartCount)
                        return false;

                }
                return true;

            }

        }


        public bool ImportExistingMemberFromHonorRelease(string tz)
        {

            using (var context = new DTS_OnlineContext())
            {

                SqlConnection conn = (SqlConnection)context.Database.GetDbConnection();
                SqlCommand cmd = new SqlCommand("SP_GetHonorReleasesMember", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TZ", SqlDbType.NVarChar).Value = tz;
                conn.Open();
                int res = 0;
                return int.TryParse(cmd.ExecuteScalar().ToString(), out res) && res > 0;
            }

        }


        #endregion

        #region Search


        public List<GetAutoCompleteResultsDTO> GetAutoCompleteData(string text, int selectTop)
        {
            var response = (List<GetAutoCompleteResultsDTO>)Cache.Get(CacheKeys.AutoCompleteAll);
            if (response == null)
            {
                response = new List<GetAutoCompleteResultsDTO>();

                using (var connection = (SqlConnection)new DTS_OnlineContext().Database.GetDbConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = AC_ALL_SCRIPT;
                    command.Parameters.AddWithValue(PROC_ORGID_PARAM, ContextManager.CurrentOrganization().OrgId);
                    command.Parameters.AddWithValue(PROC_Premium_Type, ContextManager.CurrentUser().PremiumType);

                    var da = new SqlDataAdapter(command);
                    var dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        response.Add(new GetAutoCompleteResultsDTO
                        {
                            categoryNumber = (long)row.ItemArray[0],
                            categoryName = GetStringValue(row.ItemArray[1]),
                            concatForSearch = (row.ItemArray[2]).ToString()
                        });
                    }

                    Cache.Set(CacheKeys.AutoCompleteAll, response, TimeSpan.FromHours(1));
                }
            }

            return response.Where(x => !string.IsNullOrWhiteSpace(x.concatForSearch) &&
                    x.concatForSearch.Contains(text, StringComparison.OrdinalIgnoreCase))
                        .DistinctBy(x => x.categoryNumber).Take(selectTop).ToList();

        }



        public List<CategoryDetailsDTO> GetSearchData(string str, int selectTop, long category, string region)
        {
            var response = (List<DataRow>)Cache.Get(CacheKeys.SearchCategoryAll);

            if (response == null)
            {
                using (var connection = (SqlConnection)new DTS_OnlineContext().Database.GetDbConnection())
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = SEARCH_ALL_SCRIPT;
                    command.Parameters.AddWithValue(PROC_ORGID_PARAM, ContextManager.CurrentOrganization().OrgId);
                    command.Parameters.AddWithValue(PROC_USER_TYPE_PARAM, ContextManager.CurrentUser().PremiumType);

                    var da = new SqlDataAdapter(command);
                    var dt = new DataTable();
                    da.Fill(dt);

                    response = dt.AsEnumerable().ToList();
                    Cache.Set(CacheKeys.SearchCategoryAll, response, TimeSpan.FromHours(1));
                }
            }
            var filtered = response.AsEnumerable();

            if (!string.IsNullOrEmpty(str))
                filtered = filtered.Where(r =>
                    r.Field<string>(nameof(CategoryDetailsDTO.ConcatForSearch))?
                    .IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0);

            if (category >= 0)
                filtered = filtered.Where(r =>
                    r.Field<long>(nameof(CategoryDetailsDTO.FirstFatherId)) == category);

            if (!string.IsNullOrEmpty(region))
                filtered = filtered.Where(r =>
                    r.Field<string>(nameof(CategoryDetailsDTO.Region)) == region &&
                    r.Field<int>(nameof(CategoryDetailsDTO.HaveSubBranch)) == 0);

            var sorted = filtered
                .OrderBy(r => r.Field<int?>(nameof(CategoryDetailsDTO.SortOrder)) ?? int.MaxValue)
                .Take(selectTop)
                .ToList();

            var categories = sorted
             .Select(r => GetCategory(r, true))
             .GroupBy(c => c.CategoryId)
             .Select(g => g.FirstOrDefault())
             .ToList();


            return categories;
        }

        private CategoryDetailsDTO GetCategory(DataRow r, bool searchResult)
        {

            List<ImageDTO> images = new List<ImageDTO>();
            if (!string.IsNullOrEmpty(GetStringValue(r.ItemArray[3])))
                images.Add(new ImageDTO { File = GetStringValue(r.ItemArray[3]), Alt = GetStringValue(r.ItemArray[4]), ImageTypeId = r.ItemArray[5] is DBNull ? 0 : (int)r.ItemArray[5] });

            var categoryDetails = new CategoryDetailsDTO
            {
                CategoryId = (long)r.ItemArray[0],
                CategoryName = GetStringValue(r.ItemArray[1]),
                Business = new BusinessDTO { Address = GetStringValue(r.ItemArray[2]) },
                Images = images,
                Description = GetStringValue(ContextManager.CurrentUser().PremiumType == 3 ? r.ItemArray[6] : r.ItemArray[12]),
                ShortDescription = GetStringValue(r.ItemArray[7]),
            };
            // Include CategoryType and CategoryUrl
            if (r.ItemArray.Count() > 8)
            {
                categoryDetails.CategoryType = (byte)r.ItemArray[8];
                categoryDetails.CategoryUrl = GetStringValue(r.ItemArray[9]);
            }
            if (r.ItemArray.Length > 13 && !searchResult)
            {
                var hasClubCard = ContextManager.CurrentUser().ClubCreditCard > 0;
                var priceField = hasClubCard ? r["PricesWithOrgCard"] : r["PricesWithoutOrgCard"];
                categoryDetails.Prices = string.IsNullOrWhiteSpace(priceField?.ToString()) ? new HashSet<decimal>() : ParseDecimalHashSet(priceField.ToString());
                categoryDetails.RegionIds = ResolveLocationIdsFromRow(r, "RegionsByBusiness", "RegionsByVariants", "RegionsByOther", "LocationSourceType");
                categoryDetails.CityIds = ResolveLocationIdsFromRow(r, "CitiesByBusiness", "CitiesByVariants", "CitiesByOther", "LocationSourceType");
                categoryDetails.DateRanges = string.IsNullOrWhiteSpace(r["DateRanges"]?.ToString()) ? new HashSet<DateRange>() : ParseDateRanges(r["DateRanges"].ToString());

            }

            return categoryDetails;




        }

        private HashSet<int> ResolveLocationIdsFromRow(DataRow r, string businessColumn, string variantsColumn, string otherColumn, string locationTypeColumn)
        {
            if (r[locationTypeColumn] == DBNull.Value)
                return new HashSet<int>();

            int locationType = Convert.ToInt32(r[locationTypeColumn]);

            //int locationType = 3;

            string source = null;

            switch (locationType)
            {
                case 1:
                    source = r[businessColumn]?.ToString();
                    break;

                case 2:
                    source = r[variantsColumn]?.ToString();
                    break;

                case 3:
                    source = r[otherColumn]?.ToString();
                    break;
            }

            return string.IsNullOrWhiteSpace(source) ? new HashSet<int>() : ParseIntHashSet(source);
        }

        private string GetStringValue(object dbVal)
        {
            return dbVal is DBNull ? string.Empty : (string)dbVal;
        }


        #endregion
        public City GetCityByCityId(string cityId)
        {
            try
            {
                using (var context = new DTS_OnlineContext())
                {
                    return context.City.Where(c => c.CityId.ToString() == cityId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetFatherRedimTypeId(long benefitId)
        {
            var redimTypeId = -1;

            try
            {
                using (var db = new DTS_OnlineContext())
                {
                    var portalCategory = db.PortalCategories.FirstOrDefault(x => x.CategoryNumber == benefitId);

                    if (portalCategory.IsRedimTypeOtherMessage)
                    {
                        redimTypeId = 999;
                    }
                    else
                    {
                        redimTypeId = portalCategory.RedimTypeId.HasValue ? portalCategory.RedimTypeId.Value : redimTypeId;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in GetFatherRedimTypeId, error message: {ex.Message}");
                throw;
            }
            if (redimTypeId < 1)
            {
                LoggerHelper.Error($"Warning! in GetFatherRedimTypeId, cat't find redimTypeId.  benefitId:{benefitId}");
            }
            return redimTypeId;
        }
        public string GetFatherRedimTypeName(long benefitId)
        {
            var name = string.Empty;

            try
            {
                using (var db = new DTS_OnlineContext())
                {
                    var portalCategory = db.PortalCategories.FirstOrDefault(x => x.CategoryNumber == benefitId);
                    if (portalCategory.IsRedimTypeOtherMessage)
                    {
                        name = portalCategory.RedimTypeOtherMessageText;
                    }
                    else if (portalCategory.RedimTypeId.HasValue)
                    {
                        name = GetRedimTypeNameById(portalCategory.RedimTypeId.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in GetFatherRedimTypeName, error message: {ex.Message}");
                throw;
            }
            if (string.IsNullOrEmpty(name))
            {
                LoggerHelper.Error($"Warning! in GetFatherRedimTypeName, cat't find typeName.  benefitId:{benefitId}");
            }
            return name;
        }
        public string GetRedimTypeNameById(int id)
        {
            var name = string.Empty;
            try
            {
                using (var db = new DTS_OnlineContext())
                {
                    name = db.OrganizationRedimTypes.AsNoTracking().FirstOrDefault(redim => redim.RedimTypeId == id && redim.OrganizationId == ContextManager.CurrentOrganization().OrgId)?.RedimContent;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in GetRediumTypeNameById, Error message: {ex.Message}");
                throw;
            }
            if (string.IsNullOrEmpty(name))
            {
                LoggerHelper.Error($"Warning! in GetRediumTypeNameById, can't find RedimTypesName. RedimTypeId:{id}");
            }
            return name;
        }
        public async Task<bool> GetUserEmailSubscription(string id, string email)
        {
            using (var context = new DTS_OnlineContext())
            {
                var res = await context.EmailSubscriptionActivity
                    .Where(e => e.MemberId.Equals(id) && e.Email.Equals(email))
                    .OrderByDescending(o => o.DateAdded).Select(s => s.Type).FirstOrDefaultAsync();
                return res == 0;
            }
        }

        private List<ConfirmationTemplates> LoadOrgConfirmationTemplate()
        {
            if (Cache.Exists(CacheKeys.ConfirmationTemplates))
                return Cache.Get(CacheKeys.ConfirmationTemplates) as List<ConfirmationTemplates>;

            using (var dtsOnlineContext = new DTS_OnlineContext())
            {
                List<ConfirmationTemplates> templates = dtsOnlineContext.ConfirmationTemplates.ToList();
                Cache.Set(CacheKeys.ConfirmationTemplates, templates);
                return Cache.Get(CacheKeys.ConfirmationTemplates) as List<ConfirmationTemplates>;
            }

        }

        public ConfirmationTemplates GetConfirmationTemplate(int orgId)
        {
            return LoadOrgConfirmationTemplate().FirstOrDefault(f => f.OrganizationId == orgId);
        }

        public List<CouponCancelRequest> GetCouponCancelRequestByMemberId(string memberId)
        {
            List<CouponCancelRequest> couponCancelRequests = new List<CouponCancelRequest>();
            try
            {
                using (var dtsOnlineContext = new DTS_OnlineContext())
                {
                    couponCancelRequests = dtsOnlineContext.CouponCancelRequest.Where(c => c.MemberId.Equals(memberId)).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in GetCouponCancelRequestByMemberId, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
            return couponCancelRequests;
        }

        public decimal GetGlobalSelfDischargeLimit(decimal defaultLimit = 1500m)
        {
            try
            {
                using (var context = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
                {
                    var restriction = context.PurchaseRestriction
                        .AsNoTracking()
                        .Where(x => x.Restrictionid == 6
                                 && x.OrgId == 0
                                 && x.TableUiId == 0
                                 && x.Moneytypeid == 1
                                 && x.CustomersTypeid == 1)
                        .OrderByDescending(x => x.RowGuid)
                        .Select(x => (decimal?)x.Value)
                        .FirstOrDefault();

                    return restriction.HasValue && restriction.Value > 0
                        ? restriction.Value
                        : defaultLimit;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in GetGlobalSelfDischargeLimit, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return defaultLimit;
            }
        }

        public decimal GetMemberMonthlySelfDischargeTotal(string cardNumber, DateTime fromDate, DateTime toDate)
        {
            return 0m; // not used - implementation moved to ClubRepo
        }

        public T MwcMediasExecuteQuery<T>(Func<IQueryable<MwcMedia>, T> query)
        {
            try
            {
                using (var dtsOnlineContext = new DTS_OnlineContext())
                {
                    var dbSet = dtsOnlineContext.Set<MwcMedia>();
                    return query(dbSet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in MwcMediasExecuteQuery, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return default;
            }
        }

        //REQ0143 - Send SMS for variant purchase

        public void SendVariantSMS(
            int variantType,
            int quantity,
            string posBarCode,
            int businessSubTypeId,
            string businessId,
            string firstName,
            string cardNumber,
            string shortNameVar,
            DateTime lastImplementationDate,
            string mobile,
            string orgId,
            string senderName,
            string memberId,
            int? smsQueueUsersId)
        {
            try
            {
                using (var context = new DTS_OnlineContext())
                {
                    var conn = (SqlConnection)context.Database.GetDbConnection();
                    using (var cmd = new SqlCommand("[dbo].[sp_SendVariantSMS]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@VariantType", SqlDbType.Int).Value = variantType;
                        cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                        cmd.Parameters.Add("@PosBarCode", SqlDbType.NVarChar, 50).Value = (object)posBarCode ?? DBNull.Value;
                        cmd.Parameters.Add("@BusinessSubTypeId", SqlDbType.Int).Value = businessSubTypeId;
                        cmd.Parameters.Add("@BusinessId", SqlDbType.NVarChar, 20).Value = (object)businessId ?? DBNull.Value;
                        cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = (object)firstName ?? DBNull.Value;
                        cmd.Parameters.Add("@CardNumber", SqlDbType.NVarChar, 100).Value = (object)cardNumber ?? DBNull.Value;
                        cmd.Parameters.Add("@ShortNameVar", SqlDbType.NVarChar, 500).Value = (object)shortNameVar ?? DBNull.Value;
                        cmd.Parameters.Add("@LastImplementationDate", SqlDbType.DateTime).Value = lastImplementationDate;
                        cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar, 20).Value = (object)mobile ?? DBNull.Value;
                        cmd.Parameters.Add("@OrgId", SqlDbType.NVarChar, 50).Value = (object)orgId ?? DBNull.Value;
                        cmd.Parameters.Add("@SenderName", SqlDbType.NVarChar, 11).Value = (object)senderName ?? DBNull.Value;
                        cmd.Parameters.Add("@MemberId", SqlDbType.NVarChar, 50).Value = (object)memberId ?? DBNull.Value;
                        cmd.Parameters.Add("@SmsQueueUsersId", SqlDbType.Int).Value = (object)smsQueueUsersId ?? DBNull.Value;

                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // חובה: לא להפיל רכישה
                // תחליף ללוגר שלכם
                LoggerHelper.Error("SendVariantSMS failed (sp_SendVariantSMS)", ex);
            }
        }
    }
}
