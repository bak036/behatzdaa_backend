using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class UserPortalCategories
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
        public byte? CategoryViewFormatToUse { get; set; }
        public short? Parking { get; set; }
        public short? CrippleAccess { get; set; }
        public short? Toilet { get; set; }
        public short? Restaurant { get; set; }
        public short? Tables { get; set; }
        public short? EquipmentRenting { get; set; }
        public short? Challenging { get; set; }
        public short? AboveAge { get; set; }
        public byte? GlobalBusiness { get; set; }
        public string CategoryColor { get; set; }
    }
}
