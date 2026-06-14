using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Cards
    {
        public long Id { get; set; }
        public string CardNumber { get; set; }
        public byte[] EncryptedCard { get; set; }
        public string Idmember { get; set; }
        public byte CardStatus { get; set; }
        public byte? LeumiCardStatus { get; set; }
        public DateTime? AddedTime { get; set; }
        public DateTime? ActivationTime { get; set; }
        public byte? ActivationType { get; set; }
        public string ActivationIp { get; set; }
        public string ActivationPhone { get; set; }
        public int? ActivationOp { get; set; }
        public DateTime? BlockTime { get; set; }
        public string Remark { get; set; }
        public byte? CardType { get; set; }
        public DateTime? ExpiredCard { get; set; }
    }
}
