using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class MemberTempTable
    {
        public string MemberId { get; set; }
        public int? PremiumType { get; set; }
        public DateTime? DateCreate { get; set; }
    }
}
