using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CrmHistory
    {
        public int CrmHistoryId { get; set; }
        public int CrmId { get; set; }
        public int? OpIdUpdate { get; set; }
        public int? OpIdCurrent { get; set; }
        public int? CrmStatusId { get; set; }
        public int? CrmSeverityId { get; set; }
        public string CrmResult { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string XmlDetails { get; set; }
        public int? Tier { get; set; }
        public int? BusinessIdRelated { get; set; }
    }
}
