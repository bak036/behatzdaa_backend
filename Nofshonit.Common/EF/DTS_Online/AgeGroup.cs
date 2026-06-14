using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class AgeGroup
    {
        public byte FromAge { get; set; }
        public byte ToAge { get; set; }
        public string BetweenAges { get; set; }
    }
}
