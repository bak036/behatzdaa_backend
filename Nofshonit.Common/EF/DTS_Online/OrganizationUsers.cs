using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrganizationUsers
    {
        public int OrgUserId { get; set; }
        public int OrganizationId { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public bool OrgStatus { get; set; }
        public string Iplist { get; set; }
        public bool CheckMethodsPremission { get; set; }
        public string Remark { get; set; }
    }
}
