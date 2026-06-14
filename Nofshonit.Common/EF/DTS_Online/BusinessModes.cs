using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class BusinessModes
    {
        public BusinessModes()
        {
            Business = new HashSet<Business>();
            BusinessSubBranchBranchNavigation = new HashSet<BusinessSubBranch>();
            BusinessSubBranchBusinessMode = new HashSet<BusinessSubBranch>();
        }

        public int BusinessModeId { get; set; }
        public string BusinessModeName { get; set; }

        public virtual ICollection<Business> Business { get; set; }
        public virtual ICollection<BusinessSubBranch> BusinessSubBranchBranchNavigation { get; set; }
        public virtual ICollection<BusinessSubBranch> BusinessSubBranchBusinessMode { get; set; }
    }
}
