using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class NetsRelationship
    {
        public int NetsCardId { get; set; }
        public int NetsDbId { get; set; }
        public byte PriorityDb { get; set; }
    }
}
