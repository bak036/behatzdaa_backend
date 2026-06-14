using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using System;
using System.Collections.Generic;
using System.Text;
using static Nofshonit.Common.DTOs.Product.VariantsByBenefitResponse;

namespace Nofshonit.BL.Product.ProductUtils.VariantBuilder
{
    public interface IVariantBuilder
    {
        Variant Build(long benefitId, ProductsVars productsVar, SimpleInt redimType, List<TypeImplementationDate> implementationTypesList, List<BusinessSubTypeSpecificationCurrent> specsCurrentList, List<BusinessSubTypeSpecificationByVariants> specsByVarsList);
    }
}
