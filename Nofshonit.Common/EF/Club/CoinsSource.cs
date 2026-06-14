using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class CoinsSource
    {
        public CoinsSource()
        {
            MembersCoins = new HashSet<MembersCoins>();
        }

        public int CoinsSourceId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<MembersCoins> MembersCoins { get; set; }
    }
}
