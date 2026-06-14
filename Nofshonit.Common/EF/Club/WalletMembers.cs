using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class WalletMembers
    {
        public long WalletMembersId { get; set; }
        public string MemberId { get; set; }
        public decimal? OrgBalance { get; set; }
        public decimal RefundMoney { get; set; }
        public decimal? PrePayMoney { get; set; }
    }
}
