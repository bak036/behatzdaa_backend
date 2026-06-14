using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class HoursManageProjects
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public int? ProjectType { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? InsertDate { get; set; }

        public virtual HoursManageProjecTypes ProjectTypeNavigation { get; set; }
    }
}
