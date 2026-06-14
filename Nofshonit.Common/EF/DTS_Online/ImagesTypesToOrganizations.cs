using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ImagesTypesToOrganizations
    {
        public int ImageTypeId { get; set; }
        public int OrganizationId { get; set; }

        public virtual ImageTypes ImageType { get; set; }
        public virtual Organizations Organization { get; set; }
    }
}
