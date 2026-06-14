using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class BusinessSubType
    {
        public BusinessSubType()
        {
            PricingBasePriceCommision = new HashSet<PricingBasePriceCommision>();
        }

        public int BusinessSubTypeId { get; set; }
        public string BusinessSubTypeName { get; set; }
        public string BusinessSubTypeNameWeb { get; set; }
        public bool Active { get; set; }
        public long? MerchantId { get; set; }
        public int? SogoodGroup { get; set; }
        public int? OrderSum { get; set; }
        public bool? IsSearch { get; set; }
        public bool? AllowSoogod { get; set; }
        public bool? IsCancel { get; set; }
        public int RecordId { get; set; }
        public bool IsTicketsHub { get; set; }

        public virtual ICollection<PricingBasePriceCommision> PricingBasePriceCommision { get; set; }
    }
}
