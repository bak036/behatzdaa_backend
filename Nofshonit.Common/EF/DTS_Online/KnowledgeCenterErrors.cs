using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class KnowledgeCenterErrors
    {
        public KnowledgeCenterErrors()
        {
            KnowledgeCenterPics = new HashSet<KnowledgeCenterPics>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public virtual KnowledgeCenter Project { get; set; }
        public virtual ICollection<KnowledgeCenterPics> KnowledgeCenterPics { get; set; }
    }
}
