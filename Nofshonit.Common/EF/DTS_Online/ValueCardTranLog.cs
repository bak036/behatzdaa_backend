using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ValueCardTranLog
    {
        public DateTime DateAdded { get; set; }
        public int OrganizationId { get; set; }
        public string MemberId { get; set; }
        public int PromoId { get; set; }
        public int? CardGroupId { get; set; }
        public byte Opcode { get; set; }
        public string Bin1 { get; set; }
        public string Bin2 { get; set; }
        public string Variable { get; set; }
        public string WaitingOrder { get; set; }
        public long ExtRequestId { get; set; }
        public decimal? AmountPaid { get; set; }
        public int QuantityOrdered { get; set; }
        public string ExtClientEmail { get; set; }
        public bool SendToValueCard { get; set; }
        public byte SeveralAttempts { get; set; }
        public DateTime? SendTime { get; set; }
        public DateTime? AnswerTime { get; set; }
        public string AnswerXml { get; set; }
        public long? AnswerResponseStatus { get; set; }
        public long? AnswerAuthNumber { get; set; }
    }
}
