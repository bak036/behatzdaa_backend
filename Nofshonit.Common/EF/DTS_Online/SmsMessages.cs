using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SmsMessages
    {
        public long MessageId { get; set; }
        public int? MessageType { get; set; }
        public DateTime? SentTime { get; set; }
        public string NotifierName { get; set; }
        public string PhoneNumber { get; set; }
        public int? OrganizationId { get; set; }
    }
}
