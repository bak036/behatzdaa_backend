using Nofshonit.BL.Limitations;
using Nofshonit.BL.RestApiGW;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using Nofshonit.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Nofshonit.Common.DTOs.Product.VariantsByBenefitResponse;
using static Nofshonit.Repositories.Helpers.ProductFunctions;

namespace Nofshonit.BL.Product.ProductUtils.VariantBuilder
{
    public class VariantBuilder : BaseBL, IVariantBuilder
    {
        private IClubRepo _clubRepo;
        private IDtsOnlineRepo _dtsOnlineRepo;
        private IContextManager _contextManager;
        private ILimitationsBL _limitationsBL;
        private IRestApiGW _restApiGW;
        private IConfigurationManager _configurationManager;

        public VariantBuilder() 
        {
            _clubRepo = Container.Resolve<IClubRepo>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _contextManager = Container.Resolve<IContextManager>();
            _limitationsBL = Container.Resolve<ILimitationsBL>();
            _restApiGW = Container.Resolve<IRestApiGW>();
            _configurationManager = Container.Resolve<IConfigurationManager>();
        }

        public Variant Build(long benefitId, ProductsVars productsVar, SimpleInt redimType, List<TypeImplementationDate> implementationTypesList, List<BusinessSubTypeSpecificationCurrent> specsCurrentList, List<BusinessSubTypeSpecificationByVariants> specsByVarsList)
        {
            Variant variant = new Variant()
            {
                Name = productsVar.ShortNameVar,
                BarCode = productsVar.FullBarCode,
                ExpireDate = productsVar.LastImplementationDate.HasValue ? DateCalculation.CalculateExpirationDate(productsVar.TypeCalcImplementationDate ?? 0, productsVar.LastImplementationDate.GetValueOrDefault()) : (DateTime?)null,
                IsEmpty = StockFunctions.IsVariantInStock(_restApiGW, _configurationManager, 1, productsVar.FullBarCode, benefitId, _contextManager.CurrentOrganization().OrgId) ? 0 : 1,
                IsSendToFriend = (productsVar.IsSendToFriend.HasValue ? productsVar.IsSendToFriend.Value ? 1 : 0 : 0),
                KupaPrice = (int)productsVar.CupaPrice.GetValueOrDefault(-1),
                RedimTypeId = redimType.Id,
                EndDate = productsVar.EndDate,
                RedimTypeName = redimType.Name,
                BenefitTypeId = ProductFunctions.GetBenefitTypeIdByVariantType(productsVar.VariantType),
                IsFavorite = productsVar.IsFavorite ? 1 : 0,
                OrderLimit = _limitationsBL.GetVariantOrderLimit(productsVar.FullBarCode, 0, false, false),
                IsCampaign = productsVar.Iscampaign.GetValueOrDefault(),
                BusinessSubTypeID = productsVar.BusinessSubTypeId,
                MemberMonthlyLimitFormula = LimitationsBL.GetMemberMonthlyLimit(productsVar, specsCurrentList),
                MemberYearlyLimitFormula = productsVar.MemberYearlyLimitFormula,
                MemberGeneralLimitFormula = productsVar.MemberGeneralLimitFormula,
                IrgunPriceFormula = productsVar.IrgunPriceFormula,
            };

            if (productsVar.CuponStockId.HasValue)
            {
                using (var dtsContext = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
                {
                    var res = dtsContext.CouponsStocksDetails.FirstOrDefault(f => f.StockId == productsVar.CuponStockId);
                    variant.isExternalCoupon = res != null && res.StockType == 0;
                }
            }


            int formulaNum = 0;
            if (int.TryParse(productsVar.ProductGeneralLimitFormula, out formulaNum) && (productsVar.CuponStockId != null && productsVar.CuponStockId > 0))
                if (formulaNum <= _clubRepo.GetSumOfTrnasactionsByBarcode(productsVar.FullBarCode))
                    variant.IsEmpty = 1;

            if (variant.IsEmpty == 1)
                variant.OrderLimit = 0;

            if (variant.BenefitTypeId == (int)BenefitType.GiftCard)
            {
                variant.GiftCardValue = productsVar.LoadingAmount;
                variant.GiftCardMaxCoins = productsVar.GiftCardMaxCoins;
                variant.GiftCardMinCoins = productsVar.GiftCardMinCoins;
            }
            else // If normal variant
            {
                var specByVar = specsByVarsList.FirstOrDefault(spec => spec.BarCode.Equals(productsVar.FullBarCode));
                var specCurrent = specsCurrentList.FirstOrDefault(spec => spec.BusinessSubTypeId == productsVar.BusinessSubTypeId);
                var isClubCreditCard = _contextManager.CurrentUser().ClubCreditCard > 0;
                variant.Price = ProductFunctions.GetVariantPrice(_contextManager.CurrentUser().PremiumType, productsVar, specByVar, specCurrent, isClubCreditCard, _contextManager.CurrentOrganization().IsNewSubsidy);
            }
            return variant;
        }

        private DateTime ReCalculateLastImplementationDate(ProductsVars productsVar, List<TypeImplementationDate> implementationTypesList)
        {
            DateTime lastImplementationDate = productsVar.LastImplementationDate.Value;
            int implementationType = productsVar.TypeCalcImplementationDate ?? 0;
            TypeImplementationDate impType = implementationTypesList.FirstOrDefault(x => x.Id == implementationType);
            DateTime result;
            if (implementationType == 1)
                result = new DateTime(lastImplementationDate.Year, lastImplementationDate.Month, lastImplementationDate.Day, 23, 59, 59);
            else if(impType != null)
            {
                switch (impType.NumToImplement)
                {
                    case 0: //days
                        result = DateTime.Now.AddDays(impType.TypeCalc);
                        break;

                    case 1: //month
                        result = DateTime.Now.AddMonths(impType.TypeCalc);
                        if (impType.TypeImplement.Trim() == "1")// שוטף
                        {
                            int days = DateTime.DaysInMonth(result.Year, result.Month);
                            result = new DateTime(result.Year, result.Month, days, 23, 59, 59);
                        }
                        break;

                    case 2: //year
                        result = DateTime.Now.AddYears(impType.TypeCalc);
                        if (impType.TypeImplement.Trim() == "1")
                        {
                            result = new DateTime(result.Year, 12, 31, 23, 59, 59);
                        }
                        break;                   
                    default:
                        result = DateTime.MaxValue;
                        break;
                }
            }
            else
                result = new DateTime(2030, 1, 1);

            return result;
        }
    }
}
