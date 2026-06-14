using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EmailSubscriptionActivity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DateAdded { get; set; }
        public int? OrgId { get; set; }
        public bool? FromWebsite { get; set; }
        public string MemberId { get; set; }
        public int Type { get; set; }
    }
}
