using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class MonthlyStockAmounts
    {
        public long Identity { get; set; }
        public int StockId { get; set; }
        public DateTime? DateToAddStock { get; set; }
        public int? StockAmount { get; set; }
    }
}
