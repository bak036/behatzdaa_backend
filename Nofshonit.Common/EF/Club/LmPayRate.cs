using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class LmPayRate
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Percentage { get; set; }
        public string DiscountLeverage { get; set; }
        public string Type { get; set; }
    }
}
