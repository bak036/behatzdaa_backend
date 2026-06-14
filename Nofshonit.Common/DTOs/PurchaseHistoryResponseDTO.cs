using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class PurchaseHistoryResponseDTO
    {
        [JsonProperty(PropertyName = "Status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "ErrorDescription")]
        public string ErrorDescription { get; set; }

        [JsonProperty(PropertyName = "ErrorId")]
        public int? ErrorId { get; set; }

        [JsonProperty(PropertyName = "MemberId")]
        public string MemberId { get; set; }

        [JsonProperty(PropertyName = "FromDate")]
        public DateTime FromDate { get; set; }

        [JsonProperty(PropertyName = "ToDate")]
        public DateTime ToDate { get; set; }

        [JsonProperty(PropertyName = "Variants")]
        public List<HistoryVariant> Variants { get; set; } = new List<HistoryVariant>();

        public class HistoryVariant
        {
            [JsonProperty(PropertyName = "Name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "BenefitId")]
            public string BenefitId { get; set; }

            [JsonProperty(PropertyName = "BusinessName")]
            public string BusinessName { get; set; }

            [JsonProperty(PropertyName = "BusinessId")]
            public string BusinessId { get; set; }

            [JsonProperty(PropertyName = "BusinessSubTypeId")]
            public string BusinessSubTypeId { get; set; }

            [JsonProperty(PropertyName = "BusinessSubTypeName")]
            public string BusinessSubTypeName { get; set; }

            [JsonProperty(PropertyName = "VariantBarCode")]
            public string VariantBarCode { get; set; }

            [JsonProperty(PropertyName = "VariantName")]
            public string VariantName { get; set; }
			[JsonProperty(PropertyName = "DigitalCodeType")]
			public int DigitalCodeType { get; set; }

			[JsonProperty(PropertyName = "OrderConfirmation")]
            public string OrderConfirmation { get; set; }

            [JsonProperty(PropertyName = "BenefitStatusId")]
            public int BenefitStatusId { get; set; }

            [JsonProperty(PropertyName = "BenefitStatusName")]
            public string BenefitStatusName { get; set; }

            [JsonProperty(PropertyName = "DtsOrderId")]
            public int DtsOrderId { get; set; }

            [JsonProperty(PropertyName = "OrderGuid")]
            public string OrderGuid { get; set; }

            [JsonProperty(PropertyName = "OrderDate")]
            public DateTime OrderDate { get; set; }

            [JsonProperty(PropertyName = "CustomerPrice")]
            public decimal CustomerPrice { get; set; }

            [JsonProperty(PropertyName = "CoinsUse")]
            public int CoinsUse { get; set; }

            [JsonProperty(PropertyName = "ExpiredDate")]
            public DateTime ExpiredDate { get; set; }

            [JsonProperty(PropertyName = "EndDate")]
            public DateTime EndDate { get; set; }

            [JsonProperty(PropertyName = "IsSendToFriend")]
            public int IsSendToFriend { get; set; }

            [JsonProperty(PropertyName = "BenefitTypeId")]
            public int BenefitTypeId { get; set; }

            [JsonProperty(PropertyName = "GiftCardValue")]
            public int? GiftCardValue { get; set; }

            [JsonProperty(PropertyName = "GiftCardBalance")]
            public decimal? GiftCardBalance { get; set; }

            [JsonProperty(PropertyName = "DateOfRedim")]
            public DateTime? DateOfRedim { get; set; }

            [JsonProperty(PropertyName = "CancelDate")]
            public DateTime? CancelDate { get; set; }

            [JsonProperty(PropertyName = "IsCancelable")]
            public int IsCancelable { get; set; }

            [JsonProperty(PropertyName = "MaxCancelDate")]
            public DateTime? MaxCancelDate { get; set; }

            [JsonProperty(PropertyName = "DtsRedimCode")]
            public string DtsRedimCode { get; set; }

            [JsonProperty(PropertyName = "PaymentToken")]
            public string PaymentToken { get; set; }

            [JsonProperty(PropertyName = "CreditCardExpirey")]
            public string CreditCardExpirey { get; set; }

            [JsonProperty(PropertyName = "CreditCard16Digits")]
            public string CreditCard16Digits { get; set; }

            [JsonProperty(PropertyName = "RedimTypeId")]
            public int RedimTypeId { get; set; }

            [JsonProperty(PropertyName = "RedimTypeName")]
            public string RedimTypeName { get; set; }

            [JsonProperty(PropertyName = "IsAlreadySentToFriend")]
            public int IsAlreadySentToFriend { get; set; }

            [JsonProperty(PropertyName = "FriendName")]
            public string FriendName { get; set; }

            [JsonProperty(PropertyName = "FriendMobile")]
            public string FriendMobile { get; set; }

            [JsonProperty(PropertyName = "Slink")]
            public string Slink { get; set; }

            [JsonProperty(PropertyName = "EventsTicketLink")]
            public string EventsTicketLink { get; set; }

            [JsonProperty(PropertyName = "MovieLink")]
            public string MovieLink { get; set; }

            [JsonProperty(PropertyName = "EventRow")]
            public string EventRow { get; set; }

            [JsonProperty(PropertyName = "EventSeat")]
            public string EventSeat { get; set; }

            [JsonProperty(PropertyName = "EventDate")]
            public string EventDate { get; set; }

            [JsonProperty(PropertyName = "EventTime")]
            public string EventTime { get; set; }

            [JsonProperty(PropertyName = "VenueName")]
            public string VenueName { get; set; }

            [JsonProperty(PropertyName = "PriceLevelName")]
            public string PriceLevelName { get; set; }

            [JsonProperty(PropertyName = "EventTicketType")]
            public int EventTicketType { get; set; }

            [JsonProperty(PropertyName = "TicketTypeName")]
            public string TicketTypeName { get; set; }

            [JsonProperty(PropertyName = "OrderTicketId")]
            public int? OrderTicketId { get; set; }

            [JsonProperty(PropertyName = "Quantity")]
            public int Quantity { get; set; }

            [JsonProperty(PropertyName = "TTransactionID")]
            public long TTransactionID { get; set; }

            [JsonProperty(PropertyName = "TicketArea")]
            public string TicketArea { get; set; }

            [JsonProperty(PropertyName = "EventsGuid")]
            public string EventsGuid { get; set; }

            [JsonProperty(PropertyName = "ExternalSystemOrderId")]
            public int ExternalSystemOrderId { get; set; }

            [JsonProperty(PropertyName = "TTransactionOrder")]
            public long TTransactionOrder { get; set; }

            [JsonProperty(PropertyName = "BusinessAddress")]
            public string BusinessAddress { get; set; }
         
            [JsonProperty(PropertyName = "BusinessStreetNumber")]
            public string BusinessStreetNumber { get; set; }

            [JsonProperty(PropertyName = "BusinessCity")]
            public string BusinessCity { get; set; }
            
            [JsonProperty(PropertyName = "BusinessPhoneNumber")]
            public string BusinessPhoneNumber { get; set; }

            [JsonProperty(PropertyName = "TrackingNumber")]
            public string TrackingNumber { get; set; }

            [JsonProperty(PropertyName = "TrackingWebsite")]
            public string TrackingWebsite { get; set; }

            [JsonProperty(PropertyName = "OrderStatus")]
            public string OrderStatus { get; set; }

            [JsonProperty(PropertyName = "DeliveryModeDescription")]
            public string DeliveryModeDescription { get; set; }

            [JsonProperty(PropertyName = "DeliveryAddress")]
            public string DeliveryAddress { get; set; }

            [JsonProperty(PropertyName = "OrderCity")]
            public string OrderCity { get; set; }

            [JsonProperty(PropertyName = "OrderAddress")]
            public string OrderAddress { get; set; }

            [JsonProperty(PropertyName = "PremiumType")]
            public int? PremiumType { get; set; }
			[JsonProperty(PropertyName = "IsDisplayButton")]
			public bool IsDisplayButton { get; set; }
            [JsonProperty(PropertyName = "IsExternalCoupon")]
            public bool IsExternalCoupon { get; set; }

        }
    }
}
