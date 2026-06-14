using Microsoft.EntityFrameworkCore;
using Nofshonit.BL.RestApiGW;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.EF.Club;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Nofshonit.Logs;

namespace Nofshonit.BL.Limitations
{
    public class LimitationsBL : BaseBL, ILimitationsBL
    {
        private readonly IClubRepo _clubRepo;
        private readonly IDtsOnlineRepo _dtsOnlineRepo;
        private readonly IShopingBasketBL _shoppingBasketBL;
        private readonly IConfigurationManager _configuration;
        private readonly IRestApiGW _restApiGW;
        private readonly IConfigurationManager _configurationManager;
        private int organizationId;

        const int maxLimit = 999;

        public LimitationsBL() : base()
        {
            _clubRepo = Container.Resolve<IClubRepo>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _shoppingBasketBL = Container.Resolve<IShopingBasketBL>();
            _configuration = Container.Resolve<IConfigurationManager>();
            _restApiGW = Container.Resolve<IRestApiGW>();
            _configurationManager = Container.Resolve<IConfigurationManager>();
            organizationId = ContextManager.CurrentOrganization().OrgId;
        }

        public List<VariantOrderLimitDTO> ValidatePurchesAllowed(List<CartVarsDTO> cartVarList)
        {
            List<VariantOrderLimitDTO> res = new List<VariantOrderLimitDTO>();
            var byBusinessSubTypeId = cartVarList.Where(c => c.Variant != null && c.Variant.IsCampaign != true).GroupBy(x => x.Variant.BusinessSubTypeId.GetValueOrDefault());
            var subtypeCount = byBusinessSubTypeId.ToDictionary(x => x.Key, x => x.Sum(y => (int)y.Quantity));
            foreach (var item in cartVarList)
            {
                bool isEvent;
                int variantAllowed;
                isEvent = _dtsOnlineRepo.IsEvents(long.Parse(item.CategoryId), organizationId);
                int cartQtyBySubtype = 0;
                if (item.Variant != null)
                    subtypeCount.TryGetValue(item.Variant.BusinessSubTypeId.GetValueOrDefault(), out cartQtyBySubtype);
                if (isEvent)
                {
                    var ticketList = item.Tickets;
                    foreach (var ticket in ticketList)
                    {
                        int orderLimit = GetVariantOrderLimit(ticket.VariantFullBarcode, cartQtyBySubtype,true);
                        if (orderLimit == maxLimit)
                            variantAllowed = -1;
                        else
                            variantAllowed = orderLimit;
                        res.Add(new VariantOrderLimitDTO { Barcode = ticket.VariantFullBarcode, OrderLimit = variantAllowed });
                    }
                }
                else
                {
                    int orderLimit = GetVariantOrderLimit(item.Variant.BarCode, cartQtyBySubtype,true);
                    if (orderLimit == maxLimit)
                        variantAllowed = -1;
                    else
                    {

                        variantAllowed = orderLimit;
                    }

                    res.Add(new VariantOrderLimitDTO { Barcode = item.Variant.BarCode, OrderLimit = variantAllowed });
                }

            }
            res.AddRange(CartGroupLimitation(cartVarList.Where(c => c.Variant != null).ToList()));
            return res;
        }

        private List<VariantOrderLimitDTO> CartGroupLimitation(List<CartVarsDTO> cartVarList)
        {
            List<string> barcodes = new List<string>();
            List<WebServiceTransactionDTO> memberTransacionsList = _clubRepo.GetMemberTransactions(ContextManager.CurrentUser().Id, true, false);
            var groupOfVariants = cartVarList.GroupBy(c => _clubRepo.GetProductsVar(c.Variant.BarCode).GroupLimitsId).ToList();
            groupOfVariants.ForEach(group =>
            {
                group.Where(x => x != null).ToList().ForEach(item =>
                {
                    GroupLimitationDTO groupLimitDb = null;
                    if (group.Key.HasValue)
                        groupLimitDb = _clubRepo.GetGroupLimitations((long)group.Key);
                    if (groupLimitDb != null)
                    {
                        int sumOfVarInCart = group.Select(aa => aa.Quantity).ToList().Sum(d => d);

                        int quantity = 0;
                        DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        if (groupLimitDb.MonthLimit > 0)
                        {
                            quantity = memberTransacionsList.Where(r => r.Date > dt && r.TransactionGroupLimitsId == group.Key).Sum(r => r.QuantityInt);
                            if (quantity + sumOfVarInCart > groupLimitDb.MonthLimit)
                                barcodes.Add(item.Variant.BarCode);

                        }
                        dt = new DateTime(DateTime.Now.Year, 1, 1);
                        if (groupLimitDb.YearLimit > 0)
                        {
                            quantity = memberTransacionsList.Where(r => r.Date > dt && r.TransactionGroupLimitsId == group.Key).Sum(r => r.QuantityInt);
                            quantity = quantity < 0 ? 0 : quantity; // There are cases when WST Has more canclled items then the purchased ones.
                            if (quantity + sumOfVarInCart > groupLimitDb.YearLimit)
                                barcodes.Add(item.Variant.BarCode);
                        }
                    }
                });
            });
            var list = barcodes.Distinct().Select(b => new VariantOrderLimitDTO { Barcode = b, OrderLimit = 0 }).ToList();
            return list;
        }

        public VariantOrderLimitDTO ValidatePurchesAllowed(CartVarsDTO cartVar)
        {
            VariantOrderLimitDTO res = null;

            if (cartVar == null)
                throw new Exception("Variant is null");

            bool isEvent;
            int variantAllowed;
            isEvent = _dtsOnlineRepo.IsEvents(long.Parse(cartVar.CategoryId), organizationId);
            if (isEvent)
            {
                var ticketList = cartVar.Tickets;
                foreach (var ticket in ticketList)
                {
                    int orderLimit = GetVariantOrderLimit(ticket.VariantFullBarcode, 0, true, false);
                    if (orderLimit == maxLimit)
                        variantAllowed = -1;
                    else
                        variantAllowed = cartVar.Quantity > orderLimit ? 0 : orderLimit;
                    res = new VariantOrderLimitDTO { Barcode = ticket.VariantFullBarcode, OrderLimit = variantAllowed };
                }
            }
            else
            {
                int orderLimit = GetVariantOrderLimit(cartVar.Variant.BarCode, 0, true, false);

                if (orderLimit == maxLimit)
                    variantAllowed = -1;
                else
                    variantAllowed = cartVar.Quantity > orderLimit ? 0 : orderLimit;

                res = new VariantOrderLimitDTO { Barcode = cartVar.Variant.BarCode, OrderLimit = variantAllowed };
            }

            return res;
        }

        public int GetVariantOrderLimit(string productVarBarcode, int cartSubTypeQty = 0,bool isPuchase = false,bool CheckOrderLimitEqualsZero = true)
        {
            int limit = 0;
            ProductsVars productVar = _clubRepo.GetProductsVar(productVarBarcode);
            string memberId = ContextManager.CurrentUser().Id;

            try
            {
                IEnumerable<WebServiceTransactionDTO> memberTransacionsQuery = null;
                List<WebServiceTransactionDTO> memberTransacionsList = new List<WebServiceTransactionDTO>();
                List<ShopingBasket> memberShopingBasketList = new List<ShopingBasket>();
                List<BusinessSubTypeDTO> subTypeDTOs = _clubRepo.GetBussinessSubType(_dtsOnlineRepo.GetBussinessSubTypeNames());

                bool isGetMemberCancelledTransactions = true;

                if (IsCancelRequriedAproove(productVar))
                {
                    isGetMemberCancelledTransactions = false;
                }

                if (memberId != null)
                {
                    memberTransacionsList = _clubRepo.GetMemberTransactions(memberId, false,false, isGetMemberCancelledTransactions);
                    memberShopingBasketList = _clubRepo.GetShopingBasketByMemberId(memberId, productVarBarcode);
                    if (memberShopingBasketList != null && isPuchase)
                    {
                        foreach (var item in memberShopingBasketList)
                        {
                            var basketProductVar = _clubRepo.GetProductsVar(item.ProductBarcode);
                            if (basketProductVar != null && (basketProductVar.IsIgnoreBusinessSubTypeLimits || basketProductVar.Iscampaign == true))
                                continue;

                            memberTransacionsList.Add(new WebServiceTransactionDTO()
                            {
                                Date = item.CreateDate,
                                ID = item.Id,
                                Quantity = item.Quantity.ToString(),
                                ProductId = item.ProductBarcode,
                                BussinesSubTypeID = item.ProductSubType,
                                Iscampaign = basketProductVar.Iscampaign,
                                IsIgnoreBusinessSubTypeLimits = basketProductVar.IsIgnoreBusinessSubTypeLimits
                            });
                        }
                    }
                }

                //variantTransactions = GetVariantTransactions(uniqueId, productVar.FullBarCode);
                List<int> limitsArray = new List<int>();

                if (productVar != null && memberTransacionsList != null)
                {
                    memberTransacionsQuery = memberTransacionsList.Where(r => r.ProductId == productVar.FullBarCode);

                    if (productVar.ManualLimits == true)
                    {

                        if (!string.IsNullOrEmpty(productVar.MemberMonthlyLimitFormula) && int.Parse(productVar.MemberMonthlyLimitFormula) > 0)
                        {
                            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            var lim = int.Parse(productVar.MemberMonthlyLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                            limitsArray.Add(lim);
                        }

                    }

                    if (!string.IsNullOrEmpty(productVar.MemberQuarterLimitFormula) && int.Parse(productVar.MemberQuarterLimitFormula) > 0)
                    {
                        DateTime dt = GetThreeMonthsAgo();
                        var lim = int.Parse(productVar.MemberQuarterLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        limitsArray.Add(lim);
                    }

                    if (!string.IsNullOrEmpty(productVar.MemberYearlyLimitFormula) && int.Parse(productVar.MemberYearlyLimitFormula) > 0)
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                        var lim = int.Parse(productVar.MemberYearlyLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        limitsArray.Add(lim);
                    }

                    if (!string.IsNullOrEmpty(productVar.MemberGeneralLimitFormula) && int.Parse(productVar.MemberGeneralLimitFormula) > 0)
                    {
                        var lim = int.Parse(productVar.MemberGeneralLimitFormula) - memberTransacionsQuery.Sum(r => r.QuantityInt);
                        limitsArray.Add(lim);
                    }

                    int groupLimit = GetGroupLimitsByVar(productVar, memberTransacionsList);
                    if (groupLimit >= 0)
                        limitsArray.Add(groupLimit);

                    // בהטבות כמו של חול, הייתה לוגיקה שאם מסמנים חודשי ולא שמים ערך, זה מושך מהגלובלי. ועכשיו זה לא.
                    // הורד התנאי שבודק אם מסומן הצק בוקס עדכון ידני מגבלת רכישה חודשית לחבר ואין ערך במגבלה חודשית
                    if (!productVar.ManualLimits.HasValue || !productVar.ManualLimits.Value)// || (productVar.ManualLimits.Value && string.IsNullOrEmpty(productVar.MemberMonthlyLimitFormula)))
                    {
                        using (var orgDb = ContextManager.ClubContext())
                        {
                            var varLimit = orgDb.BusinessSubTypeSpecificationCurrent.AsNoTracking().Where(x => x.BusinessSubTypeId == productVar.BusinessSubTypeId).First().MonthlyLimitVariantValue;
                            if (varLimit != null)
                            {
                                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                var lim = varLimit - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                                limitsArray.Add((int)lim);
                            }

                        }

                    }
                }

                if (!productVar.IsIgnoreBusinessSubTypeLimits && (!productVar.Iscampaign.HasValue || !productVar.Iscampaign.Value))
                {
                    List<WebServiceTransactionDTO> memberTransacionsListFiltered = memberTransacionsList.Where(m => (m.IsIgnoreBusinessSubTypeLimits == false || m.IsIgnoreBusinessSubTypeLimits == null) && (m.Iscampaign == null || m.Iscampaign == false)).ToList();
                    var limits = GetMemberLimits(memberTransacionsListFiltered, isPuchase ? 0 : cartSubTypeQty);
                    var businessSubType = limits.FirstOrDefault(b => b.BusinessSubTypeId == productVar.BusinessSubTypeId);

                    if (businessSubType != null)
                    {
                        limitsArray.Add(businessSubType.Limit);
                    }
                }

                if (limitsArray.Count > 0)
                {
                    limit = limitsArray.Min();
                    if (limit < 0)
                    {
                        limit = 0;
                    }
                }
                else
                    limit = 999;

                if (limit == 0 && CheckOrderLimitEqualsZero)
                {
                    _clubRepo.RemoveProduct(productVarBarcode);

                    var message = MessagesUtil.GetMessagesByKey(new List<int> { 10272 }).FirstOrDefault().MessageText;
                    var subTypeName = subTypeDTOs.FirstOrDefault(x => x.Id == productVar.BusinessSubTypeId).Name;
                    throw new BusinessException(string.Format(message, subTypeName));
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "Error on GetVariantOrderLimit");
                throw new Exception("כישלון בטעינת המוצרים מסל הקניות", ex); throw ex;
            }

            return limit;
        }
        private bool IsCancelRequriedAproove(ProductsVars productVar)
        {
            if (productVar == null)
                return false;

            if (productVar.CuponStockId.HasValue)
            {
                using (var dtsContext = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
                {
                    var res = dtsContext.CouponsStocksDetails.FirstOrDefault(f => f.StockId == productVar.CuponStockId);
                    return (res != null && res.StockType == 0);
                }
            }

            return false;
        }
        public int GetVariantOrderLimitOld(string productVarBarcode, int cartSubTypeQty = 0)
        {
            int limit = 0;
            ProductsVars productVar = _clubRepo.GetProductsVar(productVarBarcode);
            string memberId = ContextManager.CurrentUser().Id;

            try
            {
                IEnumerable<WebServiceTransactionDTO> memberTransacionsQuery = null;
                //List<WebServiceTransactionView> variantTransactions = new List<WebServiceTransactionView>();
                List<WebServiceTransactionDTO> memberTransacionsList = new List<WebServiceTransactionDTO>();
                List<BusinessSubTypeDTO> subTypeDTOs = _clubRepo.GetBussinessSubType(_dtsOnlineRepo.GetBussinessSubTypeNames());


                if (memberId != null)
                {
                    memberTransacionsList = _clubRepo.GetMemberTransactions(memberId, false);
                }

                //variantTransactions = GetVariantTransactions(uniqueId, productVar.FullBarCode);
                List<int> limitsArray = new List<int>();

                if (productVar != null && memberTransacionsList != null)
                {
                    memberTransacionsQuery = memberTransacionsList.Where(r => r.ProductId == productVar.FullBarCode);

                    if (!string.IsNullOrEmpty(productVar.MemberMonthlyLimitFormula) && int.Parse(productVar.MemberMonthlyLimitFormula) > 0)
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var lim = int.Parse(productVar.MemberMonthlyLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        limitsArray.Add(lim);
                    }

                    if (!string.IsNullOrEmpty(productVar.MemberQuarterLimitFormula) && int.Parse(productVar.MemberQuarterLimitFormula) > 0)
                    {
                        DateTime dt = GetThreeMonthsAgo();
                        var lim = int.Parse(productVar.MemberQuarterLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        limitsArray.Add(lim);
                    }

                    if (!string.IsNullOrEmpty(productVar.MemberYearlyLimitFormula) && int.Parse(productVar.MemberYearlyLimitFormula) > 0)
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                        var lim = int.Parse(productVar.MemberYearlyLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        limitsArray.Add(lim);
                    }

                    if (!string.IsNullOrEmpty(productVar.MemberGeneralLimitFormula) && int.Parse(productVar.MemberGeneralLimitFormula) > 0)
                    {
                        var lim = int.Parse(productVar.MemberGeneralLimitFormula) - memberTransacionsQuery.Sum(r => r.QuantityInt);
                        limitsArray.Add(lim);
                    }

                    int groupLimit = GetGroupLimitsByVar(productVar, memberTransacionsList);
                    if (groupLimit >= 0)
                        limitsArray.Add(groupLimit);

                    if (!productVar.ManualLimits.HasValue || !productVar.ManualLimits.Value)
                    {
                        using (var orgDb = ContextManager.ClubContext())
                        {
                            var varLimit = orgDb.BusinessSubTypeSpecificationCurrent.AsNoTracking().Where(x => x.BusinessSubTypeId == productVar.BusinessSubTypeId).First().MonthlyLimitVariantValue;
                            if (varLimit != null)
                            {
                                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                var lim = varLimit - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                                limitsArray.Add((int)lim);
                            }

                        }

                    }
                }

                if (!productVar.IsIgnoreBusinessSubTypeLimits && (!productVar.Iscampaign.HasValue || !productVar.Iscampaign.Value))
                {
                    //List<WebServiceTransactionView> memberTransacionsListFiltered = GetMemberTransactionsFiltered(uniqueId, memberId);
                    List<WebServiceTransactionDTO> memberTransacionsListFiltered = memberTransacionsList.Where(m => m.IsIgnoreBusinessSubTypeLimits == false && m.Iscampaign == false).ToList();
                    var limits = GetMemberLimits(memberTransacionsListFiltered, cartSubTypeQty);
                    var businessSubType = limits.FirstOrDefault(b => b.BusinessSubTypeId == productVar.BusinessSubTypeId);

                    if (businessSubType != null)
                    {
                        limitsArray.Add(businessSubType.Limit);
                    }
                }

                if (limitsArray.Count > 0)
                {
                    limit = limitsArray.Min();
                    if (limit < 0)
                    {
                        limit = 0;
                    }
                }
                else
                    limit = 999;

                if (limit == 0)
                {
                    //_clubRepo.RemoveProduct(productVarBarcode);

                    var message = MessagesUtil.GetMessagesByKey(new List<int> { 10272 }).FirstOrDefault().MessageText;
                    var subTypeName = subTypeDTOs.FirstOrDefault(x => x.Id == productVar.BusinessSubTypeId).Name;
                    throw new BusinessException(string.Format(message, subTypeName));
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get limits for variant", ex); throw ex;
            }

            return limit;
        }

        /// <summary>
        /// check stock Quantity by category or by list barcodes
        /// </summary>
        public Dictionary<string, bool> GetProductStockStatus(long? categoryId, List<string> barcodes = null, int qty = 1)
        {
            List<ProductsVars> variants = new List<ProductsVars>();
            ConcurrentDictionary<string, bool> productsStock = new ConcurrentDictionary<string, bool>();

            if (categoryId.GetValueOrDefault() > 0 && (barcodes == null || barcodes.Count == 0))
            {
                variants = _clubRepo.GetVariantsByCategoryId(categoryId.GetValueOrDefault());
            }
            else
            {
                variants = _clubRepo.GetProductsVar(barcodes);
            }
            //foreach(var variant in variants)
            //  {
            //      var isInStock = StockFunctions.IsVariantInStock(_dtsOnlineRepo, _clubRepo, qty, ContextManager.CurrentOrganization().DBName, ContextManager.CurrentOrganization().OrgId, variant);
            //      productsStock.TryAdd(variant.FullBarCode, isInStock);
            //  }
            Parallel.ForEach(variants, variant =>
            {

                var isInStock = StockFunctions.IsVariantInStock(_restApiGW, _configurationManager, qty, variant.FullBarCode, categoryId.GetValueOrDefault(), ContextManager.CurrentOrganization().OrgId);
                productsStock.TryAdd(variant.FullBarCode, isInStock);
            });

            return productsStock.ToDictionary(x => x.Key, x => x.Value);
        }

        public List<VariantOrderLimitDTO> ValidatePurchesAllowed()
        {
            return ValidatePurchesAllowed(_shoppingBasketBL.GetCartVars(_shoppingBasketBL.GetCart()));
        }

        public static string GetMemberMonthlyLimit(ProductsVars productsVar, List<BusinessSubTypeSpecificationCurrent> specsCurrentList)
        {

            var manualLimits = productsVar.ManualLimits;
            var MemberMonthlyLimitFormula = String.IsNullOrEmpty(productsVar.MemberMonthlyLimitFormula) ? -1 : int.Parse(productsVar.MemberMonthlyLimitFormula);
            var isCampagin = productsVar.Iscampaign.HasValue && productsVar.Iscampaign.Value;
            var isIgnoreBusinessSubType = productsVar.IsIgnoreBusinessSubTypeLimits;

            if (manualLimits ?? false) // עם מגבלות ידניות
            {

                if (!isCampagin && !isIgnoreBusinessSubType)
                {
                    //// החזר מגבלות תחום
                    //int MonthlyLimitValue = (int)specsCurrentList.FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitValue;
                    //return Math.Min(MemberMonthlyLimitFormula, MonthlyLimitValue).ToString();
                    var MonthlyLimitValue = specsCurrentList
                        .FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitValue;
                    if (MonthlyLimitValue != null && MonthlyLimitValue.HasValue)
                    {
                        return Math.Min(MemberMonthlyLimitFormula, MonthlyLimitValue.Value).ToString();
                    }
                    else
                    {
                        //אם אין החזר מגבלות וריאנט 
                        return MemberMonthlyLimitFormula.ToString();
                    }

                }
                else
                {
                    // החזר מגבלות מוצר ידניות
                    return MemberMonthlyLimitFormula.ToString();
                }
            }
            else
            { // ללא מגבלות ידניות
                if (isCampagin || isIgnoreBusinessSubType)
                {
                    var MonthlyLimitVariantValue = specsCurrentList.FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitVariantValue;
                    if (MonthlyLimitVariantValue == null)
                    {
                        return "-1";
                    }
                    return specsCurrentList.FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitVariantValue?.ToString();
                }

                else
                {
                    var MonthlyLimitVariantValue = specsCurrentList.FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitVariantValue;
                    var MonthlyLimitValue = specsCurrentList.FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitValue;

                    if (MonthlyLimitValue == null && MonthlyLimitVariantValue == null)
                    {
                        return "-1";
                    }
                    else if (MonthlyLimitValue != null && MonthlyLimitVariantValue == null)
                    {
                        return MonthlyLimitValue.Value.ToString();
                    }
                    else if (MonthlyLimitValue == null && MonthlyLimitVariantValue != null)
                    {
                        return MonthlyLimitVariantValue.Value.ToString();
                    }
                    else
                        return Math.Min((int)specsCurrentList.FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitVariantValue,
                    (int)specsCurrentList.FirstOrDefault(s => s.BusinessSubTypeId == productsVar.BusinessSubTypeId)?.MonthlyLimitValue).ToString();
                }
                // החזר מגבלות תחום
            }
        }
        public void ValidateCategoryForMemberLimitations(long categoryNumber, bool isCategoryProductPage = false)
        {
            if (ContextManager.CurrentOrganization().OrgId == (int)EOrganizations.Behatsdaa)
            {
                if (ContextManager.CurrentUser().ClubCreditCard == 0)
                {
                    var orgCategory = _dtsOnlineRepo.GetOrganizationCategory(categoryNumber);
                    if (orgCategory.UserTypes == 6)
                    {
                        if (isCategoryProductPage)
                        {
                            var message = MessagesUtil.GetMessagesByKey(new List<int> { 10291 })?[0]?.MessageText;
                            throw new BusinessException(string.Format(message));
                        }
                        else
                        {
                            var message = MessagesUtil.GetMessagesByKey(new List<int> { 10292 })?[0]?.MessageText;
                            throw new BusinessException(string.Format(message, orgCategory.DisplayName));
                        }
                    }
                }
            }
        }

        public List<BusinessSubTypeCategoryDTO> GetMemberLimits(List<WebServiceTransactionDTO> memberTransacions, int cartSubTypesQty = 0)
        {
            try
            {
                // בוצע בגלל אם נגמרת המגבלה בדיוק חוזר 0 ולכן הוספנו אחד
                //cartSubTypesQty--;
                var limits = new List<BusinessSubTypeCategoryDTO>();
                List<BusinessSubTypeDTO> subTypeDTOs = _clubRepo.GetBussinessSubType(_dtsOnlineRepo.GetBussinessSubTypeNames());
                foreach (var item in subTypeDTOs)
                {
                    List<int> limitsArray = new List<int>();

                    var memberTransactionsOfSubType = memberTransacions.Where(r => r.BussinesSubTypeID == item.Id);

                    var bussinessType = new BusinessSubTypeCategoryDTO();

                    if (item.LimitWeekly.HasValue && item.LimitWeekly > 0)
                    {
                        DateTime startOfWeek = GetStartOfWeek();
                        var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > startOfWeek).Sum(r => r.QuantityInt);
                        var limit = item.LimitWeekly.Value - totalOrders - cartSubTypesQty;
                        limitsArray.Add(limit);
                    }

                    if (item.LimitMontly.HasValue && item.LimitMontly > 0)
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        var limit = item.LimitMontly.Value - totalOrders - cartSubTypesQty;
                        limitsArray.Add(limit);
                    }

                    if (item.LimitYearly.HasValue && item.LimitYearly > 0)
                    {
                        DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                        var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                        var limit = item.LimitYearly.Value - totalOrders - cartSubTypesQty;
                        limitsArray.Add(limit);
                    }



                    bussinessType.BusinessSubTypeId = item.Id;
                    bussinessType.BusinessSubTypeName = item.Name;

                    if (limitsArray.Count() > 0)
                        bussinessType.Limit = limitsArray.Min();
                    else
                        bussinessType.Limit = 999;

                    limits.Add(bussinessType);
                }

                return limits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetGroupLimitsByVar(ProductsVars productVar, List<WebServiceTransactionDTO> memberTransacions)
        {
            try
            {
                int limit = 0;

                using (var orgDb = ContextManager.ClubContext())
                {
                    orgDb.Database.OpenConnection();
                    using (var transaction = orgDb.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        List<int> limitsArray = new List<int>();

                        var groupLimit = orgDb.VariantGroupLimits.AsNoTracking().Where(x => x.Id == (orgDb.ProductsVars.FirstOrDefault(y => y.FullBarCode == productVar.FullBarCode).GroupLimitsId)).FirstOrDefault();

                        if (groupLimit == null)
                            return 999;

                        var barcodesInGroup = orgDb.ProductsVars.AsNoTracking().Where(y => y.GroupLimitsId == groupLimit.Id).Select(x => x.FullBarCode).ToList();

                        if (groupLimit.MonthLimit.HasValue && (int)groupLimit.MonthLimit > 0)
                        {
                            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            var lim = (int)groupLimit.MonthLimit - memberTransacions.Where(q => barcodesInGroup.Contains(q.ProductId) && q.Date > dt).Sum(r => r.QuantityInt);
                            limitsArray.Add(lim);
                        }
                        if (groupLimit.YearLimit.HasValue && (int)groupLimit.YearLimit > 0)
                        {
                            DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                            var lim = (int)groupLimit.YearLimit - memberTransacions.Where(q => barcodesInGroup.Contains(q.ProductId) && q.Date > dt).Sum(r => r.QuantityInt);
                            limitsArray.Add(lim);
                        }

                        if (limitsArray.Count > 0)
                        {
                            limit = limitsArray.Min();
                            if (limit < 0)
                            {
                                limit = 0;
                            }
                        }
                        else
                            limit = 999;

                        transaction.Commit();
                    }
                }
                return limit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DateTime GetThreeMonthsAgo()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month - 3;

            switch (DateTime.Now.Month)
            {
                case 1:
                case 2:
                case 3:
                    year--;
                    month += 12;
                    break;
            }

            return new DateTime(year, month, 1);
        }

        public static DateTime GetStartOfWeek()
        {
            int daysToStartWeek = (int)DateTime.Now.DayOfWeek;
            int year = DateTime.Now.Year, month, day;
            if (DateTime.Now.Day < daysToStartWeek + 1)
            {
                month = DateTime.Now.Month - 1;
                if (month == 0)
                {
                    month = 1;
                    day = 1;
                }
                else
                {
                    day = DateTime.DaysInMonth(year, month) - daysToStartWeek + 1;
                }
            }
            else
            {
                month = DateTime.Now.Month;
                day = DateTime.Now.Day - daysToStartWeek;
            }

            return new DateTime(year, month, day);
        }
    }
}
