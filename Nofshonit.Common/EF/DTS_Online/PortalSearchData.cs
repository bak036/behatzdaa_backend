using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalSearchData
    {
        public long PortalSearchDataId { get; set; }
        public DateTime DateInserted { get; set; }
        public long CategoryNumber { get; set; }
        public byte CategoryType { get; set; }
        public byte CategoryStatus { get; set; }
        public string CategoryUrl { get; set; }
        public string BusinessId { get; set; }
        public int FatherId { get; set; }
        public int? FirstFatherId { get; set; }
        public short? OrganizationId { get; set; }
        public string CategoryDescription { get; set; }
        public string ShortMarketingDescription { get; set; }
        public int? SortOrder { get; set; }
        public string DisplayName { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string Region { get; set; }
        public bool? HaveSubBranch { get; set; }
        public int? TagId { get; set; }
        public string TagName { get; set; }
        public string FileName { get; set; }
        public string Alt { get; set; }
        public int? ImageTypeId { get; set; }
        public byte DmlFlag { get; set; }
        public DateTime? LastmodifyDate { get; set; }
        public string ConcatForSearch { get; set; }
    }
}
