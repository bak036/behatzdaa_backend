using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TblAuditLogin
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public bool IsExcludeAudit { get; set; }
    }
}
