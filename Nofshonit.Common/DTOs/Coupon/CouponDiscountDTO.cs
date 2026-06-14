using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Coupon
{
    public class CouponDiscountDTO
    {
        public string Coupon { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }        
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountILS { get; set; }

    }
}
