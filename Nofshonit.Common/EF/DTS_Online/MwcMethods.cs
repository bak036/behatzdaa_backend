using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcMethods
    {
        public byte MethodId { get; set; }
        public string MethodName { get; set; }
        public string MethodDescription { get; set; }
        public string FromTable { get; set; }
    }
}
