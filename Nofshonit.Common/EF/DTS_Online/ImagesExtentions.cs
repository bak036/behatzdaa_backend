using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ImagesExtentions
    {
        public ImagesExtentions()
        {
            ImagesExtentionsToImageTypes = new HashSet<ImagesExtentionsToImageTypes>();
        }

        public int ExtentionId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ImagesExtentionsToImageTypes> ImagesExtentionsToImageTypes { get; set; }
    }
}
