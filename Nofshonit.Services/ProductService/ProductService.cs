using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Services.ProductService
{
    public class ProductService: BaseService, IProductService
    {
        public ProductService()
        {
        }

        public List<VariantDTO> GetVariantsByCategoryId(long categoryId)
        {          
            return Container.Resolve<IProductBL>().GetVariantsByCategoryId(categoryId);
        }

        public Task<JObject> VariantsByBenefit(VariantsByBenefitDTO variantsByBenefitDTO)
        {
            return null;//Container.Resolve<IProductBL>().VariantsByBenefit(variantsByBenefitDTO);
        }
        public List<VariantDTO> GetProducts(long categoryId)
        {
            return Container.Resolve<IProductBL>().GetProducts(categoryId);
        }
        


    }
}
