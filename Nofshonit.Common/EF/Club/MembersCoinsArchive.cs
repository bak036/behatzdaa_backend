using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class MembersCoinsArchive
    {
        public int MemberCoinsId { get; set; }
        public DateTime InsertDate { get; set; }
        public string MemberId { get; set; }
        public int Coins { get; set; }
        public string TransactionGuid { get; set; }
        public int? CoinsSourceId { get; set; }
        public bool IsFirstLoad { get; set; }
        public int? OrderId { get; set; }
        public long? TtransactionId { get; set; }
    }
}
