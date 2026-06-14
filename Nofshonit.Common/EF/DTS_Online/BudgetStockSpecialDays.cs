using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class BudgetStockSpecialDays
    {
        public DateTime HolydayDate { get; set; }
        public int OrgId { get; set; }
        public string Description { get; set; }
    }
}
