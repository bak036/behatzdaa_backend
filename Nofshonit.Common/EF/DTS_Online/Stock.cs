using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Stock
    {
        public int StockId { get; set; }
        public string StockName { get; set; }
        public int? ProviderId { get; set; }
        public int StockQuantity { get; set; }
        public int AllertQuantity { get; set; }
        public bool Active { get; set; }
        public string BusinessId { get; set; }
        public int? OrdersQuentity { get; set; }
        public DateTime? DateUpdate { get; set; }
        public bool SendStockAlert { get; set; }
        public DateTime? AlertSendDate { get; set; }
        public int StockUsed { get; set; }
    }
}
