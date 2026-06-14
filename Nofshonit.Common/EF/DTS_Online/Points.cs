using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Points
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Description { get; set; }
        public int NumberOfpoints { get; set; }
        public decimal CashValue { get; set; }
    }
}
