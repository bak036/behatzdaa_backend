using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EventimCorruptedMediaRecords
    {
        public int RecordId { get; set; }
        public int? EventimFileId { get; set; }
        public DateTime? RecordDate { get; set; }
        public string RawText { get; set; }
        public string MediaRecordErrorCode { get; set; }
    }
}
