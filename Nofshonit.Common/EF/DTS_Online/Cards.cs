using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Cards
    {
        public string CardNumber { get; set; }
        public string Idmember { get; set; }
        public byte CardStatus { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime? ActivationTime { get; set; }
        public byte? ActivationType { get; set; }
        public string ActivationIp { get; set; }
        public string ActivationPhone { get; set; }
        public int? ActivationOp { get; set; }
        public DateTime? BlockTime { get; set; }
        public string Remark { get; set; }
    }
}
