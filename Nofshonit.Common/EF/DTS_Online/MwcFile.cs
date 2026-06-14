using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcFile
    {
        public long FileId { get; set; }
        public string FileName { get; set; }
        public byte? FileType { get; set; }
        public string DumpUrl { get; set; }
        public int? FileSize { get; set; }
        public int? NumOfRecords { get; set; }
        public DateTime? FileStartUpload { get; set; }
        public DateTime? FileEndUpload { get; set; }
        public byte? StatusFile { get; set; }
        public DateTime? ReportFromDate { get; set; }
        public DateTime? ReportToDate { get; set; }
        public string ErrorDescription { get; set; }
        public int? ClubinfoId { get; set; }
        public int? TotalRows { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
