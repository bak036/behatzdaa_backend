using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class AmountToWallet
    {
        public decimal? Amount { get; set; }
        public long? WalletId { get; set; }
        public int Id { get; set; }
    }
}
