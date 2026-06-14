using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class RequestsHistorical
    {
        public long RequestId { get; set; }
        public DateTime RequestTime { get; set; }
        public byte RequestSource { get; set; }
        public int RequestOp { get; set; }
        public int RequestType { get; set; }
        public int RequestStatus { get; set; }
        public int? ReasonCode { get; set; }
        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public string Card1 { get; set; }
        public string Card2 { get; set; }
        public decimal? Amount { get; set; }
        public int? AccountingTime { get; set; }
        public int? ManualStatus { get; set; }
        public DateTime? ManualTime { get; set; }
        public int? ManualOp { get; set; }
        public long? FileRequestId { get; set; }
        public long? OriginalRequestId { get; set; }
        public string Remark { get; set; }
        public string Xmlparam { get; set; }
    }
}
