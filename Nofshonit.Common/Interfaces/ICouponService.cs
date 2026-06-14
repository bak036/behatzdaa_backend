using Nofshonit.Common.DTOs.Coupon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Interfaces
{
    public interface ICouponService
    {
        CouponDiscountDTO GetCouponDiscount(string couponCode, decimal price);
    }
}
