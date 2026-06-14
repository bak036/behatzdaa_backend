using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class DebugMessages
    {
        public long DebugId { get; set; }
        public DateTime? DebugTime { get; set; }
        public int? DebugOrganizationId { get; set; }
        public string DebugMessage { get; set; }
        public string DebugQuary { get; set; }
        public string MoreData { get; set; }
        public string Sqlfailed { get; set; }
    }
}
