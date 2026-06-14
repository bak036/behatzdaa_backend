using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SecureLogin
    {
        public string Guid { get; set; }
        public string MemberId { get; set; }
        public int OrgId { get; set; }
        public bool IsActive { get; set; }
        public string Source { get; set; }
        public int MemberType { get; set; }
        public DateTime CreationDate { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ParametersJson { get; set; }
    }
}
