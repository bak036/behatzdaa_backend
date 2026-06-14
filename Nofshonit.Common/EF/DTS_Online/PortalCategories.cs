using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalCategories
    {
        public long CategoryNumber { get; set; }
        public byte? CategoryStatus { get; set; }
        public long CategoryFather { get; set; }
        public int? CategoryOrder { get; set; }
        public byte CategoryType { get; set; }
        public string CategoryName { get; set; }
        public string CategoryAlttext { get; set; }
        public string CategoryText { get; set; }
        public string CategoryHtml { get; set; }
        public string ArrivalMapImage { get; set; }
        public byte? CategoryViewFormatToUse { get; set; }
        public string BusinessId { get; set; }
        public string CategoryImg { get; set; }
        public bool? Visible { get; set; }
        public long? CategoryGeneralQuota { get; set; }
        public DateTime? CategoryStartTime { get; set; }
        public DateTime? CategoryEndTime { get; set; }
        public long? CategoryLimitPerMember { get; set; }
        public string CategoryLimitPerMemberFormula { get; set; }
        public short? TicketsType { get; set; }
        public string LocationExplain { get; set; }
        public short? Parking { get; set; }
        public short? CrippleAccess { get; set; }
        public short? Toilet { get; set; }
        public short? Restaurant { get; set; }
        public short? Tables { get; set; }
        public short? EquipmentRenting { get; set; }
        public short? Challenging { get; set; }
        public short? AboveAge { get; set; }
        public short? GlobalBusiness { get; set; }
        public string CategoryColor { get; set; }
        public string CategoryUrl { get; set; }
        public string CategoryUrlParams { get; set; }
        public string CategoryUrlParamKey { get; set; }
        public string CategoryUrlParamValue { get; set; }
        public string OpeningTime { get; set; }
        public string ExecutionSogood { get; set; }
        public string CategoryIframeHight { get; set; }
        public string CategoryDetailsName { get; set; }
        public string CategoryDetailsHtml { get; set; }
        public byte? CategoryDetailsType { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Terms { get; set; }
        public string MustKnow { get; set; }
        public string Details { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool IsTemporary { get; set; }
        public int? DefaultRedeemType { get; set; }
        public string DesignDisplay { get; set; }
        public int? RedimTypeId { get; set; }
        public bool IsRedimTypeOtherMessage { get; set; }
        public string RedimTypeOtherMessageText { get; set; }
        public int? MemberMessageType { get; set; }
        public int? MemberMessageId { get; set; }
        public string MemberMessageText { get; set; }
        public string LeumiAppName { get; set; }
        public bool? SiteLocationDisplay { get; set; }
        public string LinkToTheatre { get; set; }
    }
}
