using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class SubsidyPrecent
    {
        public int SubTypeId { get; set; }
        public int PremiumTypeId { get; set; }
        public decimal SubsidyPrecent1 { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int SubsidyPrecentId { get; set; }
    }
}
