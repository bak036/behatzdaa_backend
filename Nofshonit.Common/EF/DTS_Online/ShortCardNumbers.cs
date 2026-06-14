using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ShortCardNumbers
    {
        public int ShortNumber { get; set; }
        public DateTime AssignDate { get; set; }
        public string LongCardNumber { get; set; }
        public int OrganizationId { get; set; }
        public string MemberId { get; set; }
        public int CardId { get; set; }
        public string MemberName { get; set; }

        public virtual Organizations Organization { get; set; }
    }
}
