using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class BudgetStockCategory
    {
        public long Id { get; set; }
        public long? StockId { get; set; }
        public long? CategoryNumber { get; set; }
    }
}
