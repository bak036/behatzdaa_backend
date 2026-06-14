using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ManualOps
    {
        public int Counter { get; set; }
        public int IdItem { get; set; }
        public int? IdParenth { get; set; }
        public string NameItem { get; set; }
        public int? ValueItem { get; set; }
        public string TableXmlParams { get; set; }
        public int? OrganizationId { get; set; }
        public int? ExcludeOrganizationId { get; set; }
    }
}
