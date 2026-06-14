using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalSearchDataQueueBusiness
    {
        public long QueueId { get; set; }
        public DateTime DateInserted { get; set; }
        public string BuisnessId { get; set; }
        public byte DmlFlag { get; set; }
    }
}
