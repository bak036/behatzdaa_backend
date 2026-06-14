using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class DebugLog
    {
        public int DebugLogId { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte Status { get; set; }
    }
}
