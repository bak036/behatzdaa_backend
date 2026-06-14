using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SlinkOrganization
    {
        public int OrganizationId { get; set; }
        public string OrgName { get; set; }
        public string DbName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public DateTime? InsertDate { get; set; }
        public bool Status { get; set; }
    }
}
