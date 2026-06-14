using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CreditGuardOrderDetails
    {
        public int Id { get; set; }
        public int TerminalNumber { get; set; }
        public long TranId { get; set; }
        public string Status { get; set; }
        public string AnswerHebrew { get; set; }
        public string AnswerEnglish { get; set; }
        public string Validity { get; set; }
        public string SubCardType { get; set; }
        public string Currency { get; set; }
        public string OrderType { get; set; }
        public string OrderMode { get; set; }
        public string RequestType { get; set; }
        public string CreditType { get; set; }
        public string Xfield { get; set; }
        public string ReferenceNumber { get; set; }
        public string VoucherNumber { get; set; }
        public string SentNumber { get; set; }
        public string Cvvchecked { get; set; }
        public string FirstPayment { get; set; }
        public string NumberOfPayment { get; set; }
        public string CreditCompany { get; set; }
        public string ClearingCompany { get; set; }
        public string CreditTypeBrand { get; set; }
        public string SourceRecond { get; set; }
        public string TzChecked { get; set; }
        public string CreditTypeGroup { get; set; }
        public string FileNumber { get; set; }
        public string FileSent { get; set; }
        public string QueriesWithCode { get; set; }
        public DateTime InsertTime { get; set; }

        public virtual CreditGuardOrders T { get; set; }
    }
}
