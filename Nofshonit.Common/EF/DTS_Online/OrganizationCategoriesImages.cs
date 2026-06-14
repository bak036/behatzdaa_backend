using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrganizationCategoriesImages
    {
        public int OrganizationCategoriesImageId { get; set; }
        public int PortalCategoriesImageId { get; set; }
        public int OrganizationId { get; set; }
        public string FileName { get; set; }
        public string Alt { get; set; }
        public string ExternalUrl { get; set; }
        public DateTime ImageCreationDate { get; set; }
        public int ImageTypeId { get; set; }

        public virtual Organizations Organization { get; set; }
        public virtual PortalCategoriesImages PortalCategoriesImage { get; set; }
    }
}
