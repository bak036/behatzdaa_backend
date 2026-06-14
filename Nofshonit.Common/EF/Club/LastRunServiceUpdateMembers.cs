using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class LastRunServiceUpdateMembers
    {
        public int Id { get; set; }
        public int? RunType { get; set; }
        public DateTime? LastRunTime { get; set; }
        public string Info { get; set; }
    }
}
