using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class WalletMembersDetails
    {
        public string MemberId { get; set; }
        public decimal? OrganizationMoney { get; set; }
        public decimal? RefundMoney { get; set; }
        public decimal? TotalMoney { get; set; }
        public long? PaymentId { get; set; }
        public byte? StatusPayment { get; set; }
        public long Id { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PrePayMoney { get; set; }
    }
}
