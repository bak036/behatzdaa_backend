using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class HoursManageUsers
    {
        public HoursManageUsers()
        {
            HoursManageReport = new HashSet<HoursManageReport>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual ICollection<HoursManageReport> HoursManageReport { get; set; }
    }
}
