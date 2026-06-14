using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ImagesSlider
    {
        public int ImageId { get; set; }
        public string ImageUrlBig { get; set; }
        public string ImageLink { get; set; }
        public int? ImageOrder { get; set; }
        public bool? ImageActive { get; set; }
        public int? ImageOrg { get; set; }
        public string ImageMessageContext { get; set; }
        public string ImageTitle { get; set; }
        public string ImageUrlSmal { get; set; }
        public int? PopulationType { get; set; }
        public string ImageLocation { get; set; }
    }
}
