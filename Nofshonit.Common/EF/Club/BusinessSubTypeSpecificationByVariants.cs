using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class BusinessSubTypeSpecificationByVariants
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public int? BusinessSubTypeId { get; set; }
        public int? SubsidyPrecentForCardHoldersVar { get; set; }
        public int? ShekelSubsidyForCardHoldersVar { get; set; }
        public int? SubsidyPrecentRegularVar { get; set; }
        public int? ShekelSubsidyRegularVar { get; set; }
    }
}
