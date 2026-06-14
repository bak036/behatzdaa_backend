using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class VariantGroupLimits
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MonthLimit { get; set; }
        public int? YearLimit { get; set; }
    }
}
