using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class WebServiceTransaction
    {
        public long TtransactionId { get; set; }
        public DateTime? TtransactionDateTime { get; set; }
        public string TtransactionProductId { get; set; }
        public string TtransactionMemberId { get; set; }
        public string Ttransactionquantity { get; set; }
        public string TtransactionMetaData { get; set; }
        public string TtransactionAsmcta { get; set; }
        public long? TtransactionOrder { get; set; }
        public short? TtransactionStatus { get; set; }
        public string Xmlparam { get; set; }
        public decimal? CatalogicPrice { get; set; }
        public decimal? IrgunPrice { get; set; }
        public decimal? CustomerDiscount { get; set; }
        public decimal? DistributionComission { get; set; }
        public decimal? CancelCommission { get; set; }
        public bool? Iscampaign { get; set; }
        public decimal? MarketingCommission { get; set; }
        public decimal? OrganizationCommission { get; set; }
        public decimal? OperatorCommission { get; set; }
        public long? PaymentId { get; set; }
        public int? GroupLimitsId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? OrderId { get; set; }
        public int? Coins { get; set; }
        public decimal? CustomerPrice { get; set; }
        public int? PremiumType { get; set; }

    }
}
