using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class NetsMultiCardsDb
    {
        public int NetsDbId { get; set; }
        public string NetShortName { get; set; }
        public string NetDb { get; set; }
        public int OrganizationId { get; set; }
        public int? ShellyDefaultOrg { get; set; }
    }
}
