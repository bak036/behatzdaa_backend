using System;
using System.Collections.Generic;
using System.Text;
using Nofshonit.Common.DTOs.Event;

namespace Nofshonit.Common.DTOs.ResponseDTOs
{
    public class EventsByBenefitRequestDTO
    {

        public string MemberId { get; set; }
        public string BenefitId { get; set; }
        public virtual string UniqueId { get; set; }
        public EventsByBenefitRequestDTO(EventsByBenefitDTO eventsByBenefitDTO, string memberId, string uniqueId)
        {
            MemberId = memberId;
            UniqueId = uniqueId;
            BenefitId = eventsByBenefitDTO.BenefitId;
        }
    }
}
