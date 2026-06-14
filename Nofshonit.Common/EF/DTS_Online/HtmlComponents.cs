using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class HtmlComponents
    {
        public int Id { get; set; }
        public string CodeIdentity { get; set; }
        public int? SubjectId { get; set; }
        public string Title { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? Deleted { get; set; }
    }
}
