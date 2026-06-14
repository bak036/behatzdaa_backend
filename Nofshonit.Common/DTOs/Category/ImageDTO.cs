using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Category
{
    public class ImageDTO
    {
        public string File { get; set; }

        public string Alt { get; set; }
        public string ExternalUrl { get; set; }

        public int ImageTypeId { get; set; }

    }
}
