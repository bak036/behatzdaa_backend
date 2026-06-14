using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class HtmlComponentBranches
    {
        public int Id { get; set; }
        public int TextComponentId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int SubjectId { get; set; }
        public string MemberName { get; set; }
    }
}
