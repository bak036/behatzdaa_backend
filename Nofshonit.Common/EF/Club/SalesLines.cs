using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class SalesLines
    {
        public long LineId { get; set; }
        public long? SaleId { get; set; }
        public string Barcode { get; set; }
        public int? RowNumber { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? LineTotal { get; set; }
    }
}
