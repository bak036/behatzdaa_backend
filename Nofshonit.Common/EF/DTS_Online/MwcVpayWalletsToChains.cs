using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayWalletsToChains
    {
        public long Wtcid { get; set; }
        public int WalletId { get; set; }
        public int ChainId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
