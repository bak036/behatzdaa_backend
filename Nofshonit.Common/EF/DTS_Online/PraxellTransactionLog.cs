using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PraxellTransactionLog
    {
        public int TransactionId { get; set; }
        public string PraxellTransactionId { get; set; }
        public DateTime? PraxellTransactionDateTime { get; set; }
        public string OpCodeReq { get; set; }
        public string OpCodeRes { get; set; }
        public string CardId { get; set; }
        public string Amount1Req { get; set; }
        public string Amount2Req { get; set; }
        public string Amount1Res { get; set; }
        public string Amount2Res { get; set; }
        public string TerminalId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string ErrorNumRes { get; set; }
        public string StrRequest { get; set; }
        public string StrResponse { get; set; }
    }
}
