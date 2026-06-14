using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class KnowledgeCenterKinds
    {
        public KnowledgeCenterKinds()
        {
            KnowledgeCenter = new HashSet<KnowledgeCenter>();
        }

        public int KindId { get; set; }
        public string KindName { get; set; }

        public virtual ICollection<KnowledgeCenter> KnowledgeCenter { get; set; }
    }
}
