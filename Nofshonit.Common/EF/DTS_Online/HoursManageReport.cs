using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class HoursManageReport
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? Hours { get; set; }
        public DateTime? MonthReport { get; set; }
        public DateTime? InsertDate { get; set; }
        public string Jira { get; set; }

        public virtual HoursManageUsers User { get; set; }
    }
}
