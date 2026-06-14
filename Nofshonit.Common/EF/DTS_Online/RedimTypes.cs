using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RedimTypes
    {
        public RedimTypes()
        {
            OrganizationRedimTypes = new HashSet<OrganizationRedimTypes>();
        }

        public int RedimTypeId { get; set; }
        public string RedimName { get; set; }

        public virtual ICollection<OrganizationRedimTypes> OrganizationRedimTypes { get; set; }
    }
}
