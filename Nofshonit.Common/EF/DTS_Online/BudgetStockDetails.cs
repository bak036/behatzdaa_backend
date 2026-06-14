using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class BudgetStockDetails
    {
        public long StockId { get; set; }
        public string StockName { get; set; }
        public int? OrgId { get; set; }
        public int? TypeOfStock { get; set; }
        public decimal? StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? SpecialDaysQuantity { get; set; }
    }
}
