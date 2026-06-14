using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Merchants
    {
        public long MerchantId { get; set; }
        public string GazitMerchantId { get; set; }
        public int? MerchantType { get; set; }
        public string MerchantName { get; set; }
        public byte? Status { get; set; }
        public string Xmlparam { get; set; }
        public string Theme { get; set; }
        public bool? ShowOneOrg { get; set; }
    }
}
