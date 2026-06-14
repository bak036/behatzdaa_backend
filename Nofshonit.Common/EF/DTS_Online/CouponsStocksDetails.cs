using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CouponsStocksDetails
    {
        public CouponsStocksDetails()
        {
            CouponsStockLogMp = new HashSet<CouponsStockLogMp>();
        }

        public int StockId { get; set; }
        public int OrganizationId { get; set; }
        public string StockName { get; set; }
        public string StockRemark { get; set; }
        public string PrintPattern { get; set; }
        public bool? StockActive { get; set; }
        public byte? SortOrder { get; set; }
        public bool? ShowInStation { get; set; }
        public int? ShowCouponInStation { get; set; }
        public string BuisnessId { get; set; }
        public bool? IsDiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? OuterId { get; set; }
        public byte? StockType { get; set; }

        public virtual CouponsStockType StockTypeNavigation { get; set; }
        public virtual ICollection<CouponsStockLogMp> CouponsStockLogMp { get; set; }
    }
}
