using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class InvoiceRequests
    {
        public int Id { get; set; }
        public int? OrgId { get; set; }
        public long? TransactionId { get; set; }
        public bool? Status { get; set; }
    }
}
