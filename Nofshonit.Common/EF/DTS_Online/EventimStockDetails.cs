using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EventimStockDetails
    {
        public EventimStockDetails()
        {
            EventimStock = new HashSet<EventimStock>();
        }

        public int EventimStockId { get; set; }
        public int OrganizationId { get; set; }
        public string StockName { get; set; }
        public bool? StockActive { get; set; }
        public string PromoId { get; set; }

        public virtual ICollection<EventimStock> EventimStock { get; set; }
    }
}
