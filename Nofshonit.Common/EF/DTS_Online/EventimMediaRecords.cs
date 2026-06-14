using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EventimMediaRecords
    {
        public long TicketId { get; set; }
        public int OriginalTicketId { get; set; }
        public int TicketStatus { get; set; }
        public int? EventimBusinessId { get; set; }
        public string EventimBuisnessName { get; set; }
        public string StoreEmail { get; set; }
        public decimal MarketingCommission { get; set; }
        public int EventNo { get; set; }
        public string SectionCode { get; set; }
        public string VarName { get; set; }
        public string ShortNameVar { get; set; }
        public DateTime? ShowDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public short TheaterCode { get; set; }
        public string TheaterName { get; set; }
        public bool? MarkSeats { get; set; }
        public byte? TrustProgram { get; set; }
        public byte? HatavaMultiplier { get; set; }
        public bool? Iscampaign { get; set; }
        public double CupaPrice { get; set; }
        public double VarPriceFormula { get; set; }
        public double CustomerSubside { get; set; }
        public double OrganizationPrice { get; set; }
        public double CatalogicPrice { get; set; }
        public double BasicPrice { get; set; }
        public int? PromotionLimit { get; set; }
        public int PromotionId { get; set; }
        public string PromotionText { get; set; }
        public string PromotionCodeText { get; set; }
        public string AffiliateCode { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhone { get; set; }
        public DateTime? TtransactionDateTime { get; set; }
        public int? TtransactionQuantity { get; set; }
        public string EventimMemberId { get; set; }
        public decimal OrderDeliveryFee { get; set; }
        public string OrderDeliveryFeeName { get; set; }
        public long? TtransactionOrder { get; set; }
        public int? OrderId { get; set; }
        public string SectionName { get; set; }
        public string Row { get; set; }
        public string Seat { get; set; }
        public decimal TicketFee { get; set; }
        public DateTime? LastImplementationDate { get; set; }
        public string CardOwnerId { get; set; }
        public decimal OrderValue { get; set; }
        public string TicketType { get; set; }
        public int PriceLevelCode { get; set; }
        public string PriceLevelName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int CustomerNo { get; set; }
        public string K4aeventNo { get; set; }
        public DateTime? InsertDate { get; set; }
        public int EventimFileId { get; set; }
        public string MemberId { get; set; }
        public int OrgId { get; set; }
        public string BusinessId { get; set; }
        public DateTime? OrderDateExe { get; set; }
        public int? PremiumType { get; set; }
        public string OrgName { get; set; }
        public string OrgDbname { get; set; }
        public string Tz { get; set; }
    }
}
