using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class CampaignMembersUse
    {
        public long RecId { get; set; }
        public DateTime UseTime { get; set; }
        public long CampaignId { get; set; }
        public long BenefitId { get; set; }
        public long? GroupId { get; set; }
        public long? Posid { get; set; }
        public long? MerchantId { get; set; }
        public string Id { get; set; }
        public string CardNumber { get; set; }
        public int? NumOfUse { get; set; }
        public decimal? Amount { get; set; }
        public string SlipMumber { get; set; }
        public DateTime? CancelTime { get; set; }
        public long? SaleId { get; set; }
        public string Xmldata { get; set; }
    }
}
