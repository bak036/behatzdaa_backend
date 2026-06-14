using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class MemberCodes
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string Code { get; set; }
        public DateTime? InsertDate { get; set; }

        public virtual AllMembers Member { get; set; }
    }
}
