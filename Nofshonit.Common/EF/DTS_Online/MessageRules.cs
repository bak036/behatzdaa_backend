using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MessageRules
    {
        public int RowId { get; set; }
        public byte RuleType { get; set; }
        public int MessageId { get; set; }
        public long RuleNumber { get; set; }
        public byte? RuleMessageSort { get; set; }
        public int OrganizationId { get; set; }
    }
}
