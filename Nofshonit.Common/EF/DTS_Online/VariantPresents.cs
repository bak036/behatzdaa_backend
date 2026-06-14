using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class VariantPresents
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public int PropertyKey { get; set; }
        public int OrgId { get; set; }
        public string DisplayName { get; set; }
        public string MoreInfo { get; set; }
    }
}
