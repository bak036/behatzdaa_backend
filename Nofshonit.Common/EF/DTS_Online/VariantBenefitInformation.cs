using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class VariantBenefitInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte? OrgId { get; set; }
        public string VariantInformation { get; set; }
    }
}
