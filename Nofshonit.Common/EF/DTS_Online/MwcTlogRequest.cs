using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcTlogRequest
    {
        public long Tid { get; set; }
        public long TransactionId { get; set; }
        public long OriginalRequest { get; set; }
        public byte MethodId { get; set; }
        public int OrganizationId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
