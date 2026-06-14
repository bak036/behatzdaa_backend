using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LogGeneralType
    {
        public LogGeneralType()
        {
            LogGeneral = new HashSet<LogGeneral>();
            LogGeneralTypeSub = new HashSet<LogGeneralTypeSub>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<LogGeneral> LogGeneral { get; set; }
        public virtual ICollection<LogGeneralTypeSub> LogGeneralTypeSub { get; set; }
    }
}
