using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Limitations
{
   
    public class BusinessSubTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LimitDaily { get; set; }
        public int? LimitWeekly { get; set; }
        public int? LimitMontly { get; set; }
        public int? LimitMonthlyVariant { get; set; }
        public int? LimitQuarter { get; set; }
        public int? LimitYearly { get; set; }
    }
}
