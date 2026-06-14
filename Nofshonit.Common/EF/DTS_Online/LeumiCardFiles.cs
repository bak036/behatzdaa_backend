using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LeumiCardFiles
    {
        public long LeumiCardFileId { get; set; }
        public long? OriginalFileId { get; set; }
        public int OrganizationId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public int NumOfRecords { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? FileCreated { get; set; }
        public string CreatedServerIp { get; set; }
        public DateTime? MashovTime { get; set; }
        public DateTime? MashovCheckedTime { get; set; }
        public string MashovStatus { get; set; }
        public string MashovCheckedServerIp { get; set; }
        public string MashovRemark { get; set; }
        public DateTime? ActualLoadTime { get; set; }
        public string ActualLoadTimeRemark { get; set; }
    }
}
