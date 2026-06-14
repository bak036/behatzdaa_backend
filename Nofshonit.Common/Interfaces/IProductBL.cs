using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Nofshonit.Common.DTOs.Product.VariantsByBenefitResponse;

namespace Nofshonit.Common.Interfaces
{
    public interface IProductBL
    {
        List<VariantDTO> GetVariantsByCategoryId(long categoryId);

        List<Variant> VariantsByBenefit(VariantsByBenefitDTO variantsByBenefitDTO);

        List<VariantDTO> GetProducts(long categoryId);
    }
}
