using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ConsumptionOrderStatusReason
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ConsumptionOrderStatusId { get; set; }
    }
}
