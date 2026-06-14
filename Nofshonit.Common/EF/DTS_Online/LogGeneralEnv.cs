using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LogGeneralEnv
    {
        public LogGeneralEnv()
        {
            LogGeneral = new HashSet<LogGeneral>();
        }

        public int Id { get; set; }
        public string EnvName { get; set; }

        public virtual ICollection<LogGeneral> LogGeneral { get; set; }
    }
}
