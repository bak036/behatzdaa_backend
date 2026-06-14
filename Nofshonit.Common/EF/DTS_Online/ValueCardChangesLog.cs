using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ValueCardChangesLog
    {
        public long ChangesLogId { get; set; }
        public long ExtRequestId { get; set; }
        public byte ServiceId { get; set; }
        public DateTime DateAdded { get; set; }
        public string Bin1 { get; set; }
        public string Bin2 { get; set; }
        public string Variable { get; set; }
        public string AnswerXml { get; set; }
        public long? AnswerResponseStatus { get; set; }
        public long? AnswerAuthNumber { get; set; }
    }
}
