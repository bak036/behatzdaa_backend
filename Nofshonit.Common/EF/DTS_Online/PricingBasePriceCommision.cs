using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PricingBasePriceCommision
    {
        public int PricingBasePriceCommisionId { get; set; }
        public int OrganizationId { get; set; }
        public int BusinessSubTypeId { get; set; }
        public int SuppliersTypeId { get; set; }
        public decimal PercentValue { get; set; }

        public virtual BusinessSubType BusinessSubType { get; set; }
        public virtual Organizations Organization { get; set; }
    }
}
