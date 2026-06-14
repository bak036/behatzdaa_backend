using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class EventsByBenefitDTO
    {
        public string MemberId { get; set; }
        public string BenefitId { get; set; }
        public virtual string UniqueId { get; set; }
    }
}
