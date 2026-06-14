using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalCategoriesImages
    {
        public PortalCategoriesImages()
        {
            OrganizationCategoriesImages = new HashSet<OrganizationCategoriesImages>();
        }

        public int PortalCategoriesImageId { get; set; }
        public long CategoryNumber { get; set; }
        public int ImageTypeId { get; set; }
        public string Alt { get; set; }
        public string FileName { get; set; }
        public DateTime ImageCreationDate { get; set; }
        public string OriginalFileName { get; set; }

        public virtual ICollection<OrganizationCategoriesImages> OrganizationCategoriesImages { get; set; }
    }
}
