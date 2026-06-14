using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Hanpaka
    {
        public long Id { get; set; }
        public string MemberId { get; set; }
        public long RequestId { get; set; }
        public long PaymentId { get; set; }
        public string CardNum { get; set; }
        public decimal? ChargeAmount { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string MemberAddress { get; set; }
        public string MemberCiryName { get; set; }
        public string MemberZipCode { get; set; }
        public string MemberPhoneNumber { get; set; }
        public long? CskeletonId { get; set; }
        public DateTime? InsertDate { get; set; }
        public byte? CardType { get; set; }
        public int? Hrid { get; set; }
    }
}
