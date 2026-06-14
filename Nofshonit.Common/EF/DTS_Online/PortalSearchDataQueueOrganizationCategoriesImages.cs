using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalSearchDataQueueOrganizationCategoriesImages
    {
        public long QueueId { get; set; }
        public DateTime DateInserted { get; set; }
        public int OrganizationCategoriesImageId { get; set; }
        public int PortalCategoriesImageId { get; set; }
        public int OrganizationId { get; set; }
        public byte DmlFlag { get; set; }
    }
}
