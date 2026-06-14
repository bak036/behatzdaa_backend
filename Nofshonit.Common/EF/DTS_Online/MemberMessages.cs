using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MemberMessages
    {
        public int MemberMessageId { get; set; }
        public string Message { get; set; }
        public bool IsArchived { get; set; }
    }
}
