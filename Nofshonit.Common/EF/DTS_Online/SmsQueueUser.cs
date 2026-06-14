using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SmsQueueUser
    {
        public int Id { get; set; }
        public int? Tunnel { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
