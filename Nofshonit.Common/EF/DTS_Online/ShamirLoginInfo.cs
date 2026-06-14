using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ShamirLoginInfo
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string GroupName { get; set; }
    }
}
