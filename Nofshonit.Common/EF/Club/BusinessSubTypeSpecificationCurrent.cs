using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class BusinessSubTypeSpecificationCurrent
    {
        public int BusinessSubTypeId { get; set; }
        public DateTime EffictiveDate { get; set; }
        public int? SubsidyPrecentForCardHolders { get; set; }
        public int? MaxValueForCardHolders { get; set; }
        public int? ShekelSubsidyForCardHolders { get; set; }
        public int? SubsidyPrecentRegular { get; set; }
        public int? MaxValueRegular { get; set; }
        public int? ShekelSubsidyRegular { get; set; }
        public int? MonthlyLimitVariantValue { get; set; }
        public int? MonthlyLimitValue { get; set; }
        public int? YearlyLimitValue { get; set; }
        public int? WeeklyLimitValue { get; set; }
        public double? CoinsRate { get; set; }
        public int? DaysRangeToCancel { get; set; }
        public decimal? MinimumProfit { get; set; }
        public decimal? MinGapBetweenOrgAndCupa { get; set; }
    }
}
