using Nofshonit.Common.DTOs.Coupon;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Services.CouponService
{
    public class CouponService : BaseService, ICouponService
    {
        ICouponBL _couponBL;
        public CouponService()
        {
            _couponBL = Container.Resolve<ICouponBL>();
        }

        public CouponDiscountDTO GetCouponDiscount(string couponCode, decimal price)
        {
            return _couponBL.GetCouponDiscount(couponCode, price);
        }

    }
}
