using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ValueCardPromoId
    {
        public int Identifies { get; set; }
        public int PromoId { get; set; }
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}
