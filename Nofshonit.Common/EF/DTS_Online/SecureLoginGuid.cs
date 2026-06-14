using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SecureLoginGuid
    {
        public string Guid { get; set; }
        public string MemberId { get; set; }
        public string CardNumber { get; set; }
        public string PageName { get; set; }
        public bool Status { get; set; }
        public DateTime InsertDate { get; set; }
        public int? OrganizationId { get; set; }
    }
}
