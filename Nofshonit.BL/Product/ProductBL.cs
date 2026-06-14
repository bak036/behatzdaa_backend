using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Nofshonit.BL.BLHelper;
using Nofshonit.BL.Product.ProductUtils.ProductHandler;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Nofshonit.Common.DTOs.Product.VariantsByBenefitResponse;

namespace Nofshonit.BL.Product
{
    public class ProductBL : BaseBL, IProductBL
    {
        IDtsOnlineRepo _dtsOnlineRepo;
        private IClubRepo _clubRepo;
        private IContextManager _contextManager;
        private ILimitationsBL _limitationsBL;
        private IProductHandler _productHandler;
        public ProductBL(IProductHandler productHandler): base()
        {
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _clubRepo = Container.Resolve<IClubRepo>();
            _contextManager = Container.Resolve<IContextManager>();
            _limitationsBL = Container.Resolve<ILimitationsBL>();
            _productHandler = productHandler;
        }

        public List<VariantDTO> GetVariantsByCategoryId(long categoryId)
        {
            //return GetProducts(categoryId);
            List<VariantDTO> result = null;
            var request = new VariantsByBenefitDTO()
            {
                BenefitId = categoryId.ToString(),
                MemberId = _contextManager.CurrentUser().MemberGuid.TrimEnd(),
                UniqueId = _contextManager.CurrentOrganization().OrganizationGuid.TrimEnd(),
            };

            List<Variant> variants = VariantsByBenefit(request);                      
            result = variants.ConvertAll(x => new VariantDTO()
            {
                BarCode = x.BarCode,
                BenefitTypeId = x.BenefitTypeId,
                KupaPrice = x.KupaPrice,
                Business = null,//TODO
                ExpireDate = (int)x.BusinessSubTypeID.GetValueOrDefault() == 26 ? DateTime.MinValue : x.ExpireDate.GetValueOrDefault(),
                EndDate = x.EndDate.GetValueOrDefault(),
                GiftCardValue = x.GiftCardValue,
                IsEmpty = x.IsEmpty == 1,
                IsSendToFriend = x.IsSendToFriend == 1,
                Name = x.Name,
                OrderLimit = x.OrderLimit,
                RedimTypeId = x.RedimTypeId,
                RedimTypeName = x.RedimTypeName,
                IsCampaign = x.IsCampaign,
                BusinessSubTypeId = x.BusinessSubTypeID,
                Price = x.Price,
                MonthlyLimit = !string.IsNullOrEmpty(x.MemberMonthlyLimitFormula) && (x.MemberMonthlyLimitFormula.All(char.IsNumber)) ? int.Parse(x.MemberMonthlyLimitFormula) : -1,
                YearlyLimit = !string.IsNullOrEmpty(x.MemberYearlyLimitFormula) && (x.MemberYearlyLimitFormula.All(char.IsNumber)) ? int.Parse(x.MemberYearlyLimitFormula) : -1,
                GeneralLimit = !string.IsNullOrEmpty(x.MemberGeneralLimitFormula) && (x.MemberGeneralLimitFormula.All(char.IsNumber)) ? int.Parse(x.MemberGeneralLimitFormula) : -1,
                IrgunPrice = decimal.Parse(x.IrgunPriceFormula),
                isExternalCoupon = x.isExternalCoupon
            }).OrderByDescending(x => x.IsCampaign).ThenBy(x => x.Price).ThenBy(x => x.KupaPrice).ToList();
            return result;
        }

        public List<Variant> VariantsByBenefit(VariantsByBenefitDTO variantsByBenefitDTO)
        {
            return _productHandler.GetVariantsByBenefitId(variantsByBenefitDTO.BenefitId);
        }

        public List<VariantDTO> GetProducts(long categoryId)
        {
            var isEvents = _dtsOnlineRepo.IsEvents(categoryId, ContextManager.CurrentOrganization().OrgId);//.Resolve<IEventService>().GetEventsByCategoryId(categoryId);
            if (isEvents)
            {
                throw new BusinessException($"{categoryId} is event category");
            }
            var categoryRedimType = _dtsOnlineRepo.RedimTypeIdByCategoryId(categoryId, ContextManager.CurrentOrganization().OrgId);
            var organizationRedimTypes = _dtsOnlineRepo.RedimTypesByOrganization(ContextManager.CurrentOrganization().OrgId);
            var implementationTypesList = _dtsOnlineRepo.GetImplementationTypesList();
            var variants = _clubRepo.GetVariantDTOs(categoryId, ContextManager.CurrentUser().Id, ContextManager.CurrentOrganization().IsNewSubsidy, categoryRedimType, organizationRedimTypes, implementationTypesList);
            var stockVariants = _limitationsBL.GetProductStockStatus(null, variants.Select(x => x.BarCode).ToList());
            Parallel.ForEach(variants,variant=>{
                variant.OrderLimit = _limitationsBL.GetVariantOrderLimit(variant.BarCode);
                bool isEmpty = false;
                stockVariants.TryGetValue(variant.BarCode, out isEmpty);
                variant.IsEmpty = !isEmpty;        
            });
            

            return variants.OrderByDescending(x => x.IsCampaign).ThenBy(x => x.Price).ThenBy(x => x.KupaPrice).ToList();
        }
    }
}
