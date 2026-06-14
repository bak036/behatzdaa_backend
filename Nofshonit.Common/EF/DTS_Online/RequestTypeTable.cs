using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RequestTypeTable
    {
        public int RequestTypeId { get; set; }
        public string RequestTypeDescription { get; set; }
        public int? OrganizationId { get; set; }
        public int? ExcludeOrganizationId { get; set; }
        public string MwcDescription { get; set; }
        public bool? MwcDisplaySingle { get; set; }
        public byte? Priority { get; set; }
        public bool? MwcDisplayMulti { get; set; }
    }
}
