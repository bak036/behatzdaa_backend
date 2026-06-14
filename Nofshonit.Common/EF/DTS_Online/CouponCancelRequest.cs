using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CouponCancelRequest
    {
        public int CouponCancelRequestId { get; set; }
        public DateTime InsertDate { get; set; }
        public int OrganizationId { get; set; }
        public long Asmachta { get; set; }
        public string CouponCode { get; set; }
        public string StockName { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string BusinessId { get; set; }
        public bool? IsSent { get; set; }
        public string MemberPhoneNumber { get; set; }
        public string MemberMobileNumber { get; set; }
        public bool? OuterCoupon { get; set; }
    }
}
