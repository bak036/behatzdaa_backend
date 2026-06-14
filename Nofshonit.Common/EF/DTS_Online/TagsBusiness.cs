using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TagsBusiness
    {
        public int BusinessTagId { get; set; }
        public int TagId { get; set; }
        public byte BusinessTagSort { get; set; }
        public string BuisnessId { get; set; }

        public virtual Business Buisness { get; set; }
        public virtual Tags Tag { get; set; }
    }
}
