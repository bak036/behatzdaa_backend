using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Category
{
    public class ImageSliderDTO
    {
        public string ImageUrlBig { get; set; }
        public string ImageUrlSmall { get; set; }
        public int? SortOrder { get; set; }
        public string link { get; set; }
        public string Alt { get; set; }

    }
}
