using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Sales
    {
        public long SaleId { get; set; }
        public byte? SaleStatus { get; set; }
        public string FromIp { get; set; }
        public string OriginalIp { get; set; }
        public DateTime? SaleTime { get; set; }
        public string TransNo { get; set; }
        public string TransSubNo { get; set; }
        public string SlipNo { get; set; }
        public string MemberId { get; set; }
        public long? MerchantId { get; set; }
        public long? Posid { get; set; }
        public string CardNumber { get; set; }
        public byte? CardUsedType { get; set; }
        public decimal? Total { get; set; }
        public decimal? Discount { get; set; }
        public decimal? CashBack1Before { get; set; }
        public decimal? CashBack2Before { get; set; }
        public decimal? CashBack3Before { get; set; }
        public decimal? CashBack4Before { get; set; }
        public decimal? CashBack1Use { get; set; }
        public decimal? CashBack2Use { get; set; }
        public decimal? CashBack1Get { get; set; }
        public decimal? CashBack2Get { get; set; }
        public string Xmldata { get; set; }
    }
}
