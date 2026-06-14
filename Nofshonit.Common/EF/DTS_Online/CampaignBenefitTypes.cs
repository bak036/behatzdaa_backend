using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CampaignBenefitTypes
    {
        public byte BenefitTypeId { get; set; }
        public string BenefitTypeName { get; set; }
        public int? BenefitOrder { get; set; }
        public byte? BenefitStatus { get; set; }
    }
}
