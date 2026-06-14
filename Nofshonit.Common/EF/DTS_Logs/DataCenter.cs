using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public partial class DataCenter
    {
        public int DataCenterId { get; set; }
        public int DtsServiceId { get; set; }
        public string Command { get; set; }
        public string Ip { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? Seconds { get; set; }
        public bool IsException { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int? OrganizationId { get; set; }
        public long? RequestId { get; set; }
        public int? Milliseconds { get; set; }
        public int? BusinessId { get; set; }
        public int? TerminalNumber { get; set; }

        public virtual DtsService DtsService { get; set; }
    }
}
