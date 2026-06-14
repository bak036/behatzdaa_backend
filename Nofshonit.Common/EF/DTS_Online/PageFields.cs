using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PageFields
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string FieldId { get; set; }
        public int OrganizationId { get; set; }
        public bool Visible { get; set; }
    }
}
