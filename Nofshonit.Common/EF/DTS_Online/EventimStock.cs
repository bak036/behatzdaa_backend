using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EventimStock
    {
        public int EventimStockId { get; set; }
        public long PromotionId { get; set; }
        public string MemberId { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual EventimStockDetails EventimStockNavigation { get; set; }
    }
}
