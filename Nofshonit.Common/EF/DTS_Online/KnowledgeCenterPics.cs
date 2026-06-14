using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class KnowledgeCenterPics
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? ErrorId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public virtual KnowledgeCenterErrors Error { get; set; }
        public virtual KnowledgeCenter Project { get; set; }
    }
}
