using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrderQueue
    {
        public long OrderId { get; set; }
        public string MemberId { get; set; }
        public string BarCode { get; set; }
        public short Quentity { get; set; }
        public DateTime? OrderDate { get; set; }
        public string TerminalExe { get; set; }
        public short OrgId { get; set; }
        public bool? IsSucces { get; set; }
        public int? Asmachta { get; set; }
        public short? WsError { get; set; }
        public DateTime? ExeDate { get; set; }
        public int? SeveralAttempts { get; set; }
        public DateTime? InsertQueueDate { get; set; }
    }
}
