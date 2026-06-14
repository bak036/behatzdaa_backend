using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Payments
    {
        public long PaymentId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string MemberId { get; set; }
        public string CardOwnerId { get; set; }
        public string Last4Digits { get; set; }
        public string ConfirmationNumber { get; set; }
        public byte ProductsOrderStatus { get; set; }
        public decimal Charged { get; set; }
        public byte PointsChargedStatus { get; set; }
        public int? PointsUsed { get; set; }
        public decimal? PointsCashValue { get; set; }
        public decimal? OrganizationCommission { get; set; }
        public decimal? OperatorCommission { get; set; }
        public string Xml { get; set; }
        public string UniquId { get; set; }
        public decimal? TrustProgramCommission { get; set; }
        public byte? CreditCardStatus { get; set; }
    }
}
