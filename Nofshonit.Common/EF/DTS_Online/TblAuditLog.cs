using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TblAuditLog
    {
        public long Id { get; set; }
        public string DatabaseName { get; set; }
        public string ObjectName { get; set; }
        public string LoginName { get; set; }
        public int? ActionType { get; set; }
        public int? RowsAffected { get; set; }
        public DateTime DtCreatedDate { get; set; }
    }
}
