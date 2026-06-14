using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Providers
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int IdentityId { get; set; }
    }
}
