using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Histadrut
{
    public partial class PaymentRefundOld
    {
        public int PaymentRefundId { get; set; }
        public string UniqueRequestId { get; set; }
        public string ServerTransactionId { get; set; }
        public string ServerStatusCode { get; set; }
        public string Message { get; set; }
        public DateTime OpenRequestTime { get; set; }
        public DateTime? ClosingRequestTime { get; set; }
        public long TotalRequestTime { get; set; }
        public string TerminalNumber { get; set; }
        public string MemberId { get; set; }
        public int OrganizationId { get; set; }
        public int? AccountId { get; set; }
        public int? SiteId { get; set; }
        public bool IsTransactionSuccess { get; set; }
        public int StatusId { get; set; }
        public int? ReferPaymentId { get; set; }
        public double? TotalPrice { get; set; }
        public string AuthNumber { get; set; }
    }
}
