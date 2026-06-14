using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public partial class EntityChangeLog
    {
        public long EntityChangeLogId { get; set; }
        public int EntityLogTypeId { get; set; }
        public DateTime Timestamp { get; set; }
        public string PrevDescription { get; set; }
        public string NewDescription { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public long? ItemId { get; set; }
        public string ItemDescription { get; set; }

        public virtual EntityLogTypes EntityLogType { get; set; }
    }
}
