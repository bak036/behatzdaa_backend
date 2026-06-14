using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayLinkedChains
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public int WalletId { get; set; }
        public int ChainId { get; set; }
        public int? BrancheId { get; set; }
    }
}
