using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalSearchDataQueueImageFiles
    {
        public long QueueId { get; set; }
        public DateTime DateInserted { get; set; }
        public long PortalSearchDataId { get; set; }
        public string FileName { get; set; }
        public string Alt { get; set; }
        public int? ImageTypeId { get; set; }
        public byte DmlFlag { get; set; }
    }
}
