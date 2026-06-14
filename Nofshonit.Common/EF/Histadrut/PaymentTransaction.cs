using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Histadrut
{
    public partial class PaymentTransaction
    {
        public PaymentTransaction()
        {
            PaymentInquireTransaction = new HashSet<PaymentInquireTransaction>();
        }

        public int PaymentTransactionId { get; set; }
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
        public double TotalPrice { get; set; }
        public int PaymentStatusId { get; set; }
        public bool IsTransactionSuccess { get; set; }
        public string Last4DigitCard { get; set; }
        public string CardToken { get; set; }
        public int PaymentTypeId { get; set; }
        public int PaymentCreditCompnayId { get; set; }
        public string RefundServerTransactionId { get; set; }

        public virtual PaymentCreditCompany PaymentCreditCompnay { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual ICollection<PaymentInquireTransaction> PaymentInquireTransaction { get; set; }
    }
}
