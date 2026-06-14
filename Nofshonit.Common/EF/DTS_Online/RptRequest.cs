using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RptRequest
    {
        public int RequestId { get; set; }
        public int JobId { get; set; }
        public DateTime RequiredExeDate { get; set; }
        public DateTime? ActualExeDate { get; set; }
        public int? EmailQueueId { get; set; }
        public int? ParamValueId { get; set; }
        public string RequestEmailTo { get; set; }
        public string RequestEmailSubject { get; set; }
    }
}
