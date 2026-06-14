
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.GeneralDTOs
{

    public class OrderVariantDTO
    {
        public string VariantName { get; set; }
        public int Qty { get; set; }
        public string VariantBarCode { get; set; }
        public decimal MoneyPay { get; set; }
        public int CoinsPay { get; set; }
        public string IsGiftCard { get; set; }
        public int? GiftCardValue { get; set; }
        public string DtsRedimCode { get; set; }
        public string Slink { get; set; }
        public string MovieLink { get; set; }
        public int RedimTypeId { get; set; }
        public string RedimTypeName { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BenefitTypeId { get; set; }
        public string OrderConfirmation { get; set; }
        public string EventsTicketLink { get; set; }
        public int BusinessSubTypeId { get; set; }
        public bool IsCoupon { get; set; }
    }
}
