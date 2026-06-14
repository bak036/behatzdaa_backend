using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ProviderBuisness
    {
        public string BuisnessId { get; set; }
        public long ProviderId { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
