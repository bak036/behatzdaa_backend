using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ManualStatusTable
    {
        public int ManualStatusId { get; set; }
        public string ManualStatusDescription { get; set; }
        public int? OrganizationId { get; set; }
    }
}
