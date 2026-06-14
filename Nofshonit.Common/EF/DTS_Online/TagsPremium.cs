using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TagsPremium
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int PremiumTypeId { get; set; }

        public virtual Tags Tag { get; set; }
    }
}
