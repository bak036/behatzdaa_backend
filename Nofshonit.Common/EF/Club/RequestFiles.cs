using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class RequestFiles
    {
        public long FileRequestId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int NumOfRecords { get; set; }
        public DateTime InsertDate { get; set; }
        public int Id { get; set; }
    }
}
