using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class AllClubAutomaticReportStatus
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string PresentationCode { get; set; }
        public string PresentationName { get; set; }
        public DateTime? ShowDate { get; set; }
        public bool? IsSendReport { get; set; }
        public DateTime? DateSendReport { get; set; }
    }
}
