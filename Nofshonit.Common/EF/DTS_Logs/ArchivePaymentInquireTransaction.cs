using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public partial class ArchivePaymentInquireTransaction
    {
        public int PaymentInquireTransactionId { get; set; }
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
        public int PaymentStatusId { get; set; }
        public int? PaymentTransactionId { get; set; }
        public bool IsFromExternalSite { get; set; }
        public int PaymentCreditCompnayId { get; set; }
    }
}
