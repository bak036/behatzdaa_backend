using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LeumiCardMedia
    {
        public long DtsfileId { get; set; }
        public string FileName { get; set; }
        public int? FileSize { get; set; }
        public DateTime? TimeRead { get; set; }
        public DateTime? TimeFinishRead { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? NumOfRecords { get; set; }
        public long? NumOfErrors { get; set; }
        public int? OrganizationId { get; set; }
        public int? MediaFileType { get; set; }
        public int? LeumiCardYearMonth { get; set; }
        public double? LeumiCardTotalPay { get; set; }
        public double? LeumiCardCommision { get; set; }
    }
}
