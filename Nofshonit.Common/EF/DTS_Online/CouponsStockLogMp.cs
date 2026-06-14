using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CouponsStockLogMp
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public DateTime DateCreated { get; set; }
        public int CardId { get; set; }
        public int PinCode { get; set; }
        public int? BenefitId { get; set; }
        public DateTime? DateLoaded { get; set; }
        public int? LoadId { get; set; }
        public bool Moved { get; set; }
        public long? CouponId { get; set; }
        public DateTime? DateCanceled { get; set; }

        public virtual CouponsStock Coupon { get; set; }
        public virtual CouponsStocksDetails Stock { get; set; }
    }
}
