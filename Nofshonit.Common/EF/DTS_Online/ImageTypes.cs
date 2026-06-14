using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ImageTypes
    {
        public ImageTypes()
        {
            ImagesExtentionsToImageTypes = new HashSet<ImagesExtentionsToImageTypes>();
            ImagesTypesToOrganizations = new HashSet<ImagesTypesToOrganizations>();
        }

        public int ImageTypeId { get; set; }
        public string Name { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? MaxSizeKb { get; set; }

        public virtual ICollection<ImagesExtentionsToImageTypes> ImagesExtentionsToImageTypes { get; set; }
        public virtual ICollection<ImagesTypesToOrganizations> ImagesTypesToOrganizations { get; set; }
    }
}
