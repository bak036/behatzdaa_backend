using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EmailQueue
    {
        public int EmailId { get; set; }
        public DateTime EmailDateAdded { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool? IsBodyHtml { get; set; }
        public string EmailBcc { get; set; }
        public string EmailCc { get; set; }
        public byte? SeveralAttempts { get; set; }
        public DateTime? EmailSendDate { get; set; }
        public bool? IsSendEmail { get; set; }
        public string FailedReason { get; set; }
        public int? EmailType { get; set; }
        public long? PaymentId { get; set; }
        public string Attachment { get; set; }
        public int? EmailQueueUsersId { get; set; }
    }
}
