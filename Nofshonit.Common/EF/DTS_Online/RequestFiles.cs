using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RequestFiles
    {
        public long FileRequestId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int NumOfRecords { get; set; }
        public DateTime InsertDate { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? HandlingEndDate { get; set; }
        public int? UpdatedRecords { get; set; }
        public int? FailedRecords { get; set; }
        public string MashovFileName { get; set; }
    }
}
