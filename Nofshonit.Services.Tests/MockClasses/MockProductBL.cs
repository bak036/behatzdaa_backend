using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Nofshonit.BL.BLHelper;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.Common.Http_Procedures;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Base;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Nofshonit.Common.DTOs.Product.VariantsByBenefitResponse;

namespace Nofshonit.Services.Tests.MockClasses
{
	public class MockProductBL : BaseBL, IProductBL
	{
		IDtsOnlineRepo _dtsOnlineRepo;
		private IClubRepo _clubRepo;
		private int organizationId;

		public MockProductBL()
		{
			_dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
			_clubRepo = Container.Resolve<IClubRepo>();
			organizationId = 48;
		}
              

        public List<VariantDTO> GetVariantsByCategoryId(long categoryId)
		{
			List<VariantDTO> result = null;
			var request = new VariantsByBenefitDTO()
			{
				BenefitId = categoryId.ToString(),
				MemberId = Container.Resolve<IContextManager>().CurrentUser().MemberGuid.TrimEnd(),
				UniqueId = "tempid",
			};

			var variantsByBenefitJResult = VariantsByBenefit(request);
			//var responseVariant = variantsByBenefitJResult.ToObject<VariantsByBenefitResponse>();            
			var json = variantsByBenefitJResult.ToString();
			var responseVariant = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseData<VariantsByBenefitResponse>>(json).data;


			if (responseVariant != null && responseVariant.Variants != null)
			{
				result = responseVariant.Variants.ConvertAll(x => new VariantDTO()
				{
					BarCode = x.BarCode,
					BenefitTypeId = x.BenefitTypeId,
					KupaPrice = x.KupaPrice,
					Business = null,//TODO
					ExpireDate = x.ExpireDate.GetValueOrDefault(),
					GiftCardValue = x.GiftCardValue,
					IsEmpty = x.IsEmpty == 1,
					IsSendToFriend = x.IsSendToFriend == 1,
					Name = x.Name,
					OrderLimit = x.OrderLimit,
					Price = x.Price,
					RedimTypeId = x.RedimTypeId,
					RedimTypeName = x.RedimTypeName,
					IsCampaign = x.IsCampaign,
                    BusinessSubTypeId = x.BusinessSubTypeID,
                    MonthlyLimit = !string.IsNullOrEmpty(x.MemberMonthlyLimitFormula) && (x.MemberMonthlyLimitFormula.All(char.IsNumber)) ? int.Parse(x.MemberMonthlyLimitFormula) : -1,
                    IrgunPrice = decimal.Parse(x.IrgunPriceFormula)
                });
			}
			return result;
		}

        public List<Variant> VariantsByBenefit(VariantsByBenefitDTO variantsByBenefitDTO)
		{
			var variantsByBenefitRequestDTO = new VariantsByBenefitRequestDTO(variantsByBenefitDTO,
																			  Container.Resolve<IContextManager>().CurrentUser().MemberGuid,
																			  "tempid");

			return null;
		}

        public List<VariantDTO> GetProducts(long categoryId)
        {
            throw new NotImplementedException();
        }


    }
}

