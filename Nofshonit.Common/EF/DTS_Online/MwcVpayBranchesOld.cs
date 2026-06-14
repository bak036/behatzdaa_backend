using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayBranchesOld
    {
        public int BranchId { get; set; }
        public string Name { get; set; }
        public int ParentChainId { get; set; }
        public string Title { get; set; }
        public byte StateId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
