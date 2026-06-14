using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Providers
    {
        public Providers()
        {
            Business = new HashSet<Business>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string BuisnessId { get; set; }

        public virtual ICollection<Business> Business { get; set; }
    }
}
