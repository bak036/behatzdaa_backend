using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Campaigns
    {
        public long CampaignId { get; set; }
        public int OrganizationId { get; set; }
        public short? CampainLevel { get; set; }
        public byte CampaignStatus { get; set; }
        public string CampaignName { get; set; }
        public string CampainShortName { get; set; }
        public string CampaignExtCode { get; set; }
        public long CampaignLimit { get; set; }
        public byte CampaignType { get; set; }
        public int CreationOp { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? CampaignStartTime { get; set; }
        public DateTime? CampaignEndTime { get; set; }
        public string Xmlparam { get; set; }
        public byte? BakaraPrint { get; set; }
        public byte? CategoryId { get; set; }
        public string ImgUrl { get; set; }
        public string SmsText { get; set; }
        public bool? SendSms { get; set; }
        public bool? AddCoupon { get; set; }
        public int? StockCoupon { get; set; }
        public bool? NeedCharge { get; set; }
        public decimal? SumCharge { get; set; }
        public bool? SendEmail { get; set; }
        public bool? ReturnCoupon { get; set; }
        public int? ReturnCouponStock { get; set; }
        public byte? CampaigNameRequired { get; set; }
    }
}
