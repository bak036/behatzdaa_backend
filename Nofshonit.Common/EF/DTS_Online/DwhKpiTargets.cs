using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class DwhKpiTargets
    {
        public int OrganizationId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public double? TotalSalePrice { get; set; }
        public double? TotalSaleAmount { get; set; }
    }
}
