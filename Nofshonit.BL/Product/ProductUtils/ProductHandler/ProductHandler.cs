using Dynamitey.DynamicObjects;
using Nofshonit.BL.Event;
using Nofshonit.BL.Product.ProductUtils.VariantBuilder;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Extensions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using Nofshonit.Repositories.Helpers;
using System;
using System.Collections.Generic;
using static Nofshonit.Common.DTOs.Product.VariantsByBenefitResponse;
using Nofshonit.Logs;

namespace Nofshonit.BL.Product.ProductUtils.ProductHandler
{
    public class ProductHandler : BaseBL, IProductHandler
    {
        private IClubRepo _clubRepo;
        private IDtsOnlineRepo _dtsOnlineRepo;
        private IContextManager _contextManager;
        private IVariantBuilder _variantBuilder;

        public ProductHandler()
        {
            _clubRepo = Container.Resolve<IClubRepo>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _contextManager = Container.Resolve<IContextManager>();
            _variantBuilder = Container.Resolve<IVariantBuilder>();
        }

        public List<Variant> GetVariantsByBenefitId(string benefitIdStr)
        {
            List<Variant> variants = new List<Variant>();
            try
            {
                long benefitId = 0;
                if (!long.TryParse(benefitIdStr, out benefitId))
                    throw new BusinessException("מספר ההטבה חייב להיות מספרי");
                if (EventFunctions.IsEventimBenefitByCategoryNumber(_clubRepo, (int)benefitId))
                    throw new BusinessException("לא ניתן לרכוש מופע עם מוצר מסוג שונה");
                List<ProductsVars> productsVars = _clubRepo.GetProductsVars(benefitId);
                List<TypeImplementationDate> implementationTypesList = _dtsOnlineRepo.GetImplementationTypesList();
                var specsCurrentList = _clubRepo.GetBusinessSubTypeSpecificationCurrents(productsVars);
                var specsByVarsList = _clubRepo.GetBusinessSubTypeSpecificationByVariants(productsVars);
                foreach (ProductsVars productsVar in productsVars)
                {
                    SimpleInt redimType = _dtsOnlineRepo.RedimTypeIdByCategoryId(benefitId, _contextManager.CurrentOrganization().OrgId);
                    variants.Add(_variantBuilder.Build(benefitId, productsVar, redimType, implementationTypesList, specsCurrentList, specsByVarsList));
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, $"Error in GetVariantsByBenefitId, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
            return variants;
        }
    }
}
