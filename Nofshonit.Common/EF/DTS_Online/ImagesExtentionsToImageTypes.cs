using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ImagesExtentionsToImageTypes
    {
        public int ImageTypeId { get; set; }
        public int ImageExtention { get; set; }

        public virtual ImagesExtentions ImageExtentionNavigation { get; set; }
        public virtual ImageTypes ImageType { get; set; }
    }
}
