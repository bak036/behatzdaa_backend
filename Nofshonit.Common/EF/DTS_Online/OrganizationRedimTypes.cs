using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrganizationRedimTypes
    {
        public int OrgRedimTypeId { get; set; }
        public int RedimTypeId { get; set; }
        public int OrganizationId { get; set; }
        public string RedimContent { get; set; }

        public virtual Organizations Organization { get; set; }
        public virtual RedimTypes RedimType { get; set; }
    }
}
