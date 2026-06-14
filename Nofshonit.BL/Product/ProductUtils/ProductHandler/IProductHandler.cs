using System;
using System.Collections.Generic;
using System.Text;
using static Nofshonit.Common.DTOs.Product.VariantsByBenefitResponse;

namespace Nofshonit.BL.Product.ProductUtils.ProductHandler
{
    public interface IProductHandler
    {
        List<Variant> GetVariantsByBenefitId(string benefitId);
    }
}
