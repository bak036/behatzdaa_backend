using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LogGeneralTypeSub
    {
        public LogGeneralTypeSub()
        {
            LogGeneral = new HashSet<LogGeneral>();
        }

        public int Id { get; set; }
        public int? FTypeId { get; set; }
        public string TypeSubName { get; set; }

        public virtual LogGeneralType FType { get; set; }
        public virtual ICollection<LogGeneral> LogGeneral { get; set; }
    }
}
