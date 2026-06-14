using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class VariantStock
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public string FullBarCode { get; set; }
        public bool Active { get; set; }
    }
}
