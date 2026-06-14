using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CouponsStockLog
    {
        public long LogId { get; set; }
        public long? CouponId { get; set; }
        public string CouponCode { get; set; }
        public bool CouponStatus { get; set; }
        public int StockId { get; set; }
        public string MemberId { get; set; }
        public string CardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? SendingTime { get; set; }
        public bool? CouponRealizationStatus { get; set; }
        public DateTime? RealizationTime { get; set; }
        public long? Posid { get; set; }
        public long? MerchantId { get; set; }
        public string UserName { get; set; }
        public DateTime? DateReset { get; set; }
    }
}
