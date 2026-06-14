using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class VerifonTransactionLog
    {
        public long TransactionId { get; set; }
        public DateTime? VerifonTransactionDateTime { get; set; }
        public string CardId { get; set; }
        public decimal? AmountReq { get; set; }
        public int? WalletReq { get; set; }
        public int? StatusReq { get; set; }
        public string ErrorDesc { get; set; }
        public string ReqType { get; set; }
    }
}
