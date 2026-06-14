using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CouponsStockType
    {
        public CouponsStockType()
        {
            CouponsStocksDetails = new HashSet<CouponsStocksDetails>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public bool DtsCancelation { get; set; }

        public virtual ICollection<CouponsStocksDetails> CouponsStocksDetails { get; set; }
    }
}
