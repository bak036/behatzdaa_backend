using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class BusinessSubTypeLimits
    {
        public short BusinessSubTypeId { get; set; }
        public int? MemberGeneralLimitFormula { get; set; }
        public int? MemberDailyLimitFormula { get; set; }
        public int? MemberWeeklyLimitFormula { get; set; }
        public int? MemberMonthlyLimitFormula { get; set; }
        public int? MemberQuarterLimitFormula { get; set; }
        public int? MemberYearlyLimitFormula { get; set; }
        public int? ProductGeneralLimitFormula { get; set; }
        public int? ProductDailyLimitFormula { get; set; }
        public int? ProductWeeklyLimitFormula { get; set; }
        public int? ProductMonthlyLimitFormula { get; set; }
        public int? ProductQuarterLimitFormula { get; set; }
        public int? ProductYearlyLimitFormula { get; set; }
        public int? TerminalId { get; set; }
    }
}
