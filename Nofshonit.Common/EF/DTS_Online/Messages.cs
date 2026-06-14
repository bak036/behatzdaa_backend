using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Messages
    {
        public int MessageId { get; set; }
        public string MessageName { get; set; }
        public string MessageContext { get; set; }
        public int? MessageKey { get; set; }
        public int? OrganizationId { get; set; }
        public string AdminDescription { get; set; }
        public string MessageText { get; set; }
        public int? GroupKey { get; set; }
        public byte? Sort { get; set; }
    }
}
