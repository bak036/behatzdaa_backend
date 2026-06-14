using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CampaignBenefits
    {
        public long BenefitId { get; set; }
        public long CampaignId { get; set; }
        public long MerchantId { get; set; }
        public byte BenefitType { get; set; }
        public byte? BenefitAndOr { get; set; }
        public string BenefitName { get; set; }
        public byte BenefitStatus { get; set; }
        public long? BenefitGeneralLimit { get; set; }
        public long? BenefitPersonLimit { get; set; }
        public long? BenefitAccountLimit { get; set; }
        public int? BenefitPosPriority { get; set; }
        public byte? DoubleCamp { get; set; }
        public byte? SelectionType { get; set; }
        public byte? SelectionType1 { get; set; }
        public byte? SelectionType2 { get; set; }
        public byte? BenefitScreen { get; set; }
        public byte? BenefitScreen1 { get; set; }
        public byte? BenefitScreen2 { get; set; }
        public byte? BenefitBack { get; set; }
        public byte? BenefitBack1 { get; set; }
        public byte? BenefitBack2 { get; set; }
        public byte? BenefitCoupon { get; set; }
        public byte? BenefitCoupon1 { get; set; }
        public byte? BenefitCoupon2 { get; set; }
        public string Msg { get; set; }
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        public string OpenMsg { get; set; }
        public string OpenMsg1 { get; set; }
        public string OpenMsg2 { get; set; }
        public string Xmlparam { get; set; }
        public string BenefitParams { get; set; }
        public decimal? SumValue { get; set; }
        public string FieldName { get; set; }
        public string Makat { get; set; }
        public string ImageName { get; set; }
        public int? ApprovedStatus { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? Items { get; set; }
        public bool HasLimits { get; set; }
        public int? LimitClient1 { get; set; }
        public int? LimitClient2 { get; set; }
        public int? LimitClient3 { get; set; }
        public int? LimitClient4 { get; set; }
        public int? LimitClient5 { get; set; }
        public int? LimitBenefit1 { get; set; }
        public int? LimitBenefit2 { get; set; }
        public int? LimitBenefit3 { get; set; }
        public int? LimitBenefit4 { get; set; }
        public int? LimitBenefit5 { get; set; }
        public decimal? BenefitCost { get; set; }
        public decimal? SumBill { get; set; }
        public string FullDetails { get; set; }
        public decimal? CustomerBenefitCost { get; set; }
        public decimal? ProviderCommision { get; set; }
    }
}
