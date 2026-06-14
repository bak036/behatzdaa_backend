using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.EF.Club;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nofshonit.BL.Limitations
{
    public static class LimitFunctions
    {
        public static List<int> GetGroupLimitsArray(IClubRepo _clubRepo, IEnumerable<WebServiceTransactionDTO> memberTransacionsQuery, ProductsVars productVar)
        {

            List<int> limitsArray = new List<int>();
            if (productVar.GroupLimitsId != null)
            {
                GroupLimitationDTO gl = _clubRepo.GetGroupLimitations((long)productVar.GroupLimitsId);
                if (gl != null)
                {
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (gl.MonthLimit > 0)
                        limitsArray.Add(gl.MonthLimit - memberTransacionsQuery.Where(r => r.Date > dt && r.TransactionGroupLimitsId == productVar.GroupLimitsId).Sum(r => r.QuantityInt));

                    dt = new DateTime(DateTime.Now.Year, 1, 1);
                    if (gl.YearLimit > 0)
                        limitsArray.Add(gl.YearLimit - memberTransacionsQuery.Where(r => r.Date > dt && r.TransactionGroupLimitsId == productVar.GroupLimitsId).Sum(r => r.QuantityInt));

                }
            }
            return limitsArray;
        }



        public static List<int> GetLimitsArray(IDtsOnlineRepo _dtsOnlineRepo, IClubRepo _clubRepo, IEnumerable<WebServiceTransactionDTO> memberTransacionsQuery, ProductsVars productVar,bool isCampagin,bool isIgnoreBusinessSubType)
        {
            List<int> limitsArray = new List<int>();

            if (!string.IsNullOrEmpty(productVar.MemberMonthlyLimitFormula))
            {
                if (productVar.ManualLimits != null && productVar.ManualLimits == true)
                {
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var lim = int.Parse(productVar.MemberMonthlyLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                    limitsArray.Add(lim);
                }
            }

            if (!string.IsNullOrEmpty(productVar.MemberQuarterLimitFormula))
            {
                int quarterNumber = (DateTime.Now.Month - 1) / 3 + 1;
                DateTime firstDayOfQuarter = new DateTime(DateTime.Now.Year, (quarterNumber - 1) * 3 + 1, 1);
                var lim = int.Parse(productVar.MemberQuarterLimitFormula) - memberTransacionsQuery.Where(r => r.Date > firstDayOfQuarter).Sum(r => r.QuantityInt);
                limitsArray.Add(lim);
            }

            if (!string.IsNullOrEmpty(productVar.MemberYearlyLimitFormula))
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                var lim = int.Parse(productVar.MemberYearlyLimitFormula) - memberTransacionsQuery.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                limitsArray.Add(lim);
            }

            if (!string.IsNullOrEmpty(productVar.ProductGeneralLimitFormula))
            {
                var variantTransactions = _clubRepo.GetVariantTransactions(productVar.FullBarCode);
                var lim = int.Parse(productVar.ProductGeneralLimitFormula) - variantTransactions.Sum(r => r.QuantityInt);
                limitsArray.Add(lim);
            }
            // check Histadrut.BusinessSubTypeSpecificationCurrent.MonthlyLimitVariantValue > orderes by BusinessSubTypeID
            var checkCampagin = productVar.Iscampaign.HasValue && productVar.Iscampaign.Value;

                if (!productVar.IsIgnoreBusinessSubTypeLimits)
                {
                    if (productVar.BusinessSubTypeId.GetValueOrDefault() > 0)
                    {
                        var firstBussinessSubTypeName = _dtsOnlineRepo.GetBussinessSubTypeNames().FirstOrDefault(x => x.Id == productVar.BusinessSubTypeId.GetValueOrDefault());
                        if (firstBussinessSubTypeName != null)
                        {
                            List<BusinessSubTypeDTO> subTypeDTOs = _clubRepo.GetBussinessSubType(new List<BusinessSubTypeNameDTO> { firstBussinessSubTypeName }, true);
                            var limit = subTypeDTOs.FirstOrDefault(x => x.Id == productVar.BusinessSubTypeId.GetValueOrDefault());
                            if (limit != null && limit.LimitMonthlyVariant.HasValue)
                            {
                                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                var lim = limit.LimitMonthlyVariant.Value - memberTransacionsQuery.Where(x => x.Date >= dt && x.ProductId.Equals(productVar.FullBarCode)).Sum(r => r.QuantityInt);
                                limitsArray.Add(lim);
                            }
                        }
                    }
            }
            return limitsArray;
        }

        public static List<BusinessSubTypeCategoryDTO> GetMemberLimits(IDtsOnlineRepo _dtsOnlineRepo, IClubRepo _clubRepo, List<WebServiceTransactionDTO> memberTransacionsList)
        {
            try
            {

                var businessSubTypeList = new List<BusinessSubTypeCategoryDTO>();

                List<BusinessSubTypeDTO> subTypeDTOs = _clubRepo.GetBussinessSubType(_dtsOnlineRepo.GetBussinessSubTypeNames());
                foreach (var item in subTypeDTOs)
                {
                    List<int> limitsArray = new List<int>();

                    var memberTransactionsOfSubType = memberTransacionsList.Where(r => r.BussinesSubTypeID == item.Id);
                    if (memberTransactionsOfSubType != null)
                    {
                        limitsArray = GetBusinessLimitCount(memberTransactionsOfSubType, item);

                        businessSubTypeList.Add(new BusinessSubTypeCategoryDTO()
                        {
                            BusinessSubTypeId = item.Id,
                            BusinessSubTypeName = item.Name,
                            MonthlyLimit = item.LimitMontly,
                            YearlyLimit = item.LimitYearly,
                            Limit = limitsArray.Any() ? limitsArray.Min() : 999
                        });
                    }
                }

                return businessSubTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<int> GetBusinessLimitCount(IEnumerable<WebServiceTransactionDTO> memberTransactionsOfSubType, BusinessSubTypeDTO item)
        {

            List<int> limitsArray = new List<int>();
            if (item.LimitWeekly.HasValue)
            {
                DateTime startOfWeek = DateTime.Today.AddDays(-1 * (int)DateTime.Today.DayOfWeek);
                var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > startOfWeek).Sum(r => r.QuantityInt);
                var limit = item.LimitWeekly.Value - totalOrders;
                limitsArray.Add(limit);
            }

            if (item.LimitMontly.HasValue)
            {
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                var limit = item.LimitMontly.Value - totalOrders;
                limitsArray.Add(limit);
            }

            if (item.LimitYearly.HasValue)
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                var totalOrders = memberTransactionsOfSubType.Where(r => r.Date > dt).Sum(r => r.QuantityInt);
                var limit = item.LimitYearly.Value - totalOrders;
                limitsArray.Add(limit);
            }
            return limitsArray;
        }

    }
}
