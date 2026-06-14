using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Histadrut
{
    public partial class ApiLogs
    {
        public int LogId { get; set; }
        public string MethodName { get; set; }
        public int OrganizationId { get; set; }
        public string ClientIp { get; set; }
        public string ServerIp { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Exception { get; set; }
        public string Response { get; set; }
        public string Body { get; set; }
        public string QueryParams { get; set; }
        public int? LogType { get; set; }
    }
}
