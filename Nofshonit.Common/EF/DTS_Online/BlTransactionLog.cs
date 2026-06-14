using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class BlTransactionLog
    {
        public long TransactionId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public int OrganizationId { get; set; }
        public string MemberId { get; set; }
        public int MoneyTypeId { get; set; }
        public long OrderReferenceId { get; set; }
        public decimal Amount { get; set; }
        public long? ShellyReferenceId { get; set; }
        public string ShellyReferenceType { get; set; }
        public decimal? ShellyOriginalAmount { get; set; }
        public string ShellyPrePaidLoad { get; set; }
        public decimal? ShellyCurrentBalance { get; set; }
        public bool? ShellyFromHistory { get; set; }
        public long? CancelTransactionId { get; set; }
    }
}
