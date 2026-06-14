using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TracePele
    {
        public int RowNumber { get; set; }
        public int? EventClass { get; set; }
        public string TextData { get; set; }
        public string NtuserName { get; set; }
        public int? ClientProcessId { get; set; }
        public string ApplicationName { get; set; }
        public string LoginName { get; set; }
        public int? Spid { get; set; }
        public long? Duration { get; set; }
        public DateTime? StartTime { get; set; }
        public long? Reads { get; set; }
        public long? Writes { get; set; }
        public int? Cpu { get; set; }
    }
}
