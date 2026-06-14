using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrganizationCategories
    {
        public long CategoryNumber { get; set; }
        public int FatherId { get; set; }
        public short OrganizationId { get; set; }
        public int? SortOrder { get; set; }
        public string DisplayName { get; set; }
        public bool? ShowOpeningTime { get; set; }
        public bool? ShowLocationExplain { get; set; }
        public bool? ShowExecutionSogood { get; set; }
        public string CampaignDetails { get; set; }
        public string Validity { get; set; }
        public string Remarks { get; set; }
        public string CategoryHeader { get; set; }
        public int? CategoryDesign { get; set; }
        public bool? ShowInHomePage { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryDescriptioRregularPrice { get; set; }
        public int UserTypes { get; set; }
        public bool? ShowInSpecialPopulationPage { get; set; }
        public bool? ShowInHomePageSlider { get; set; }
        public int? CategoryShowType { get; set; }
        public int? CategoryTypeMassage { get; set; }
        public bool? AllowOverrideDescWithJob { get; set; }
        public bool? ExternalIframe { get; set; }
        public string ExternalIframeAddress { get; set; }
        public int Id { get; set; }
        public string ShortMarketingDescription { get; set; }
        public bool? IsBankLeumiSpecial { get; set; }
        public bool? IsOutOfStock { get; set; }
        public int? DescriptionPriceMin { get; set; }
        public int? DescriptionPriceMax { get; set; }
        public bool? IsFilterEnabled { get; set; }
        public string FilterParameters { get; set; }
        public byte? LocationSourceType { get; set; }
        public string PricesWithOrgCard { get; set; }
        public string PricesWithoutOrgCard { get; set; }
        public string CitiesByBusiness { get; set; }
        public string CitiesByVariants { get; set; }
        public string CitiesByOther { get; set; }
        public string RegionsByBusiness { get; set; }
        public string RegionsByVariants { get; set; }
        public string RegionsByOther { get; set; }
        public string DateRanges { get; set; }
    }
}
