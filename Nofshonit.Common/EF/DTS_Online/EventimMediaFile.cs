using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EventimMediaFile
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public byte? FilelType { get; set; }
        public int? FileSize { get; set; }
        public int? NumOfRecords { get; set; }
        public DateTime? StartLoadDate { get; set; }
        public DateTime? EndLoadDate { get; set; }
        public bool? ReadErrors { get; set; }
        public string ErrorDescription { get; set; }
    }
}
