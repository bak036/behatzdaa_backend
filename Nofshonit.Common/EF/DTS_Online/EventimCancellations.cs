using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EventimCancellations
    {
        public int EventimCancelId { get; set; }
        public int EventimOrderId { get; set; }
        public int? EventimCancellationId { get; set; }
        public DateTime? DateCanceled { get; set; }
        public bool? Status { get; set; }
        public DateTime? DateApproved { get; set; }
        public int? Opid { get; set; }
    }
}
