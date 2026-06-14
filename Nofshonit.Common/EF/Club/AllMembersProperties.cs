using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class AllMembersProperties
    {
        public string MemberId { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CardNum { get; set; }
        public string CardExpiration { get; set; }
        public string CardId { get; set; }
        public string PayerTz { get; set; }
        public string CreditCardClub { get; set; }

        public string CardCode { get; set; }

    }
}
