using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LeumiCardActivationReport
    {
        public int OrganizationId { get; set; }
        public string FileName { get; set; }
        public bool Active { get; set; }
    }
}
