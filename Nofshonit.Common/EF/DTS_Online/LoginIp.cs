using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LoginIp
    {
        public string Ip { get; set; }
        public byte NumberOfTries { get; set; }
    }
}
