using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LogGeneralUserType
    {
        public LogGeneralUserType()
        {
            LogGeneral = new HashSet<LogGeneral>();
        }

        public int Id { get; set; }
        public string UserTypeName { get; set; }

        public virtual ICollection<LogGeneral> LogGeneral { get; set; }
    }
}
