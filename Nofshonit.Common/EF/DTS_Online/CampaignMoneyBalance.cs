using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CampaignMoneyBalance
    {
        public long CampaignId { get; set; }
        public string MemberId { get; set; }
        public decimal MoneyBalance { get; set; }
    }
}
