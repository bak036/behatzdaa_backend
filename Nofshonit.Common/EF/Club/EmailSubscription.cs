using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class EmailSubscription
    {
        public string MemberId { get; set; }
        public string Email { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int Id { get; set; }
    }
}
