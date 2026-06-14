using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LogGeneralResultsCode
    {
        public LogGeneralResultsCode()
        {
            LogGeneral = new HashSet<LogGeneral>();
        }

        public int Id { get; set; }
        public int ResultsCode { get; set; }
        public string ResultsName { get; set; }
        public string ResultsDescription { get; set; }
        public string DisplayText { get; set; }

        public virtual ICollection<LogGeneral> LogGeneral { get; set; }
    }
}
