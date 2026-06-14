using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class HoursManageProjecTypes
    {
        public HoursManageProjecTypes()
        {
            HoursManageProjects = new HashSet<HoursManageProjects>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<HoursManageProjects> HoursManageProjects { get; set; }
    }
}
