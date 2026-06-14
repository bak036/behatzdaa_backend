using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Product
{
    public class VariantsByBenefitRequestDTO
    {

        public string UniqueId { get; set; }

        public string MemberId { get; set; }

        public string BenefitId { get; set; }

        public VariantsByBenefitRequestDTO(VariantsByBenefitDTO variantsByBenefitDTO, string memberId, string uniqueId)
        {
            UniqueId = uniqueId;
            MemberId = memberId;
            BenefitId = variantsByBenefitDTO.BenefitId;
        }
    }
}
