using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayChainsOld
    {
        public int ChainId { get; set; }
        public string Name { get; set; }
        public string ChainDescription { get; set; }
        public byte StateId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
