using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayWallets
    {
        public int WalletId { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public int Currency { get; set; }
        public int VerId { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal? MaxBalance { get; set; }
        public decimal? MaxDeposit { get; set; }
    }
}
