using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PraxellFile
    {
        public long FileId { get; set; }
        public string FileName { get; set; }
        public int? FileSize { get; set; }
        public int? NumOfRecords { get; set; }
        public DateTime? FileStartUpload { get; set; }
        public DateTime? FileEndUpload { get; set; }
        public byte? StatusFile { get; set; }
    }
}
