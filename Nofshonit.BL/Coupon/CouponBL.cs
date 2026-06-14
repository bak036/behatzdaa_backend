using Nofshonit.Common.DTOs.Coupon;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.DtsOnlineModel;

namespace Nofshonit.BL.Coupon
{
    public class CouponBL: BaseBL, ICouponBL
    {
        IDtsOnlineRepo _dtsOnlineRepo;
        public CouponBL()
        {
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
        }
        public CouponDiscountDTO GetCouponDiscount(string couponCode, decimal price)
        {
            return _dtsOnlineRepo.GetCouponDiscount(couponCode, price);
        }
    }
}
