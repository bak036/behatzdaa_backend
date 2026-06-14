using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalSearchDataQueueOrganizationCategories
    {
        public long QueueId { get; set; }
        public DateTime DateInserted { get; set; }
        public int Id { get; set; }
        public long CategoryNumber { get; set; }
        public short OrganizationId { get; set; }
        public byte DmlFlag { get; set; }
    }
}
