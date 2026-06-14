using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcNetworkDiscount
    {
        public int Id { get; set; }
        public string ActionNetworkId { get; set; }
        public short IssuerId { get; set; }
        public decimal DiscountPrecent { get; set; }
        public short DiscountMonth { get; set; }
        public short DiscountYear { get; set; }
    }
}
