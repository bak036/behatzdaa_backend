using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CouponsStock
    {
        public CouponsStock()
        {
            CouponsStockLogMp = new HashSet<CouponsStockLogMp>();
        }

        public long CouponId { get; set; }
        public string CouponCode { get; set; }
        public bool? CouponStatus { get; set; }
        public int StockId { get; set; }
        public string MemberId { get; set; }
        public string CardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? SendingTime { get; set; }
        public bool? CouponRealizationStatus { get; set; }
        public DateTime? RealizationTime { get; set; }
        public long? Posid { get; set; }
        public long? MerchantId { get; set; }
        public long? CampaignId { get; set; }
        public string SeventhCardDigit { get; set; }
        public bool? PrintStatus { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? UploadCouponTime { get; set; }

        public virtual ICollection<CouponsStockLogMp> CouponsStockLogMp { get; set; }
    }
}
