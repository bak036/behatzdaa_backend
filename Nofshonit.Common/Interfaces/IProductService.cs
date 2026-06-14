using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface IProductService
    {
        List<VariantDTO> GetVariantsByCategoryId(long categoryId);
        Task<JObject> VariantsByBenefit(VariantsByBenefitDTO variantsByBenefitDTO);
        List<VariantDTO> GetProducts(long categoryId);
    }
}
