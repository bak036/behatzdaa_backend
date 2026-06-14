using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RptJobs
    {
        public int JobId { get; set; }
        public string Description { get; set; }
        public string ProcedureName { get; set; }
        public string Dbname { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTo { get; set; }
        public string FileNamePrefix { get; set; }
        public string PathToCopy { get; set; }
        public bool SentEmailWithContentOnly { get; set; }
    }
}
