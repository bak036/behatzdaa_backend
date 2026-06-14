using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RegisterMembers
    {
        public int RegisterType { get; set; }
        public string RegisteMemberId { get; set; }
        public DateTime? RegisteDate { get; set; }
        public int? OrgId { get; set; }
    }
}
