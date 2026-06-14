using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalSearchDataQueuePortalCategories
    {
        public long QueueId { get; set; }
        public DateTime DateInserted { get; set; }
        public long? CategoryNumber { get; set; }
        public byte DmlFlag { get; set; }
    }
}
