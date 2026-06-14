using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class WebConfigKeys
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public int? PopulationType { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }
        public string KeyDescription { get; set; }
    }
}
