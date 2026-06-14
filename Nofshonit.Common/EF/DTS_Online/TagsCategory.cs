using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TagsCategory
    {
        public int CategoryTagId { get; set; }
        public int TagId { get; set; }
        public int CategoryTagSort { get; set; }
        public long CategoryNumber { get; set; }

        public virtual Tags Tag { get; set; }
    }
}
