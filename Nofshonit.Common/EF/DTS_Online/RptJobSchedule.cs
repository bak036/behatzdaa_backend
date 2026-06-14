using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RptJobSchedule
    {
        public int JobId { get; set; }
        public byte FrequencyType { get; set; }
        public DateTime FrequencyTime { get; set; }
    }
}
