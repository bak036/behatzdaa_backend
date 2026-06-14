using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SmsTypes
    {
        public byte SmsType { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
    }
}
