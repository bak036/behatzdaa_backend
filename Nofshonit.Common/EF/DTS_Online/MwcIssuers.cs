using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcIssuers
    {
        public short IssuerId { get; set; }
        public string IssuerName { get; set; }
        public string IssuerDescription { get; set; }
        public bool Active { get; set; }
        public int? VerIssuerId { get; set; }
    }
}
