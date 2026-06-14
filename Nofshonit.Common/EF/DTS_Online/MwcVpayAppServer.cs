using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayAppServer
    {
        public int VapyId { get; set; }
        public string VapyKey { get; set; }
        public string VapyValue { get; set; }
        public string VapyDescription { get; set; }
        public bool VapyDateTimeType { get; set; }
    }
}
