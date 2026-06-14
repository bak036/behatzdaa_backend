using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CampaignCategories
    {
        public byte CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryStatus { get; set; }
        public int OrganizationId { get; set; }
        public int? CategorySort { get; set; }
        public byte? LoadLimitDays { get; set; }
        public int? CategoryLimitQuantity { get; set; }
    }
}
