using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class BudgetStockDays
    {
        public long StockId { get; set; }
        public DateTime? Date { get; set; }
        public int? DailyAmount { get; set; }
        public int? DayImplement { get; set; }
        public int? Transmitted { get; set; }
        public long Id { get; set; }
        public bool? AssignedAsSpecialDay { get; set; }
        public int StockUsed { get; set; }
    }
}
