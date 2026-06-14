using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalSearchDataQueueTags
    {
        public long QueueId { get; set; }
        public DateTime DateInserted { get; set; }
        public int TagId { get; set; }
        public byte DmlFlag { get; set; }
    }
}
