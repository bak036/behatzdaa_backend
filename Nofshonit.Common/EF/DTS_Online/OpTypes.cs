using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OpTypes
    {
        public int OpTypeId { get; set; }
        public string OpTypeDescription { get; set; }
        public string AllowPages { get; set; }
        public string AllowBusinessSubTypes { get; set; }
        public string AllowActions { get; set; }
    }
}
