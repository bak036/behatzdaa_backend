using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class KnowledgeCenter
    {
        public KnowledgeCenter()
        {
            KnowledgeCenterErrors = new HashSet<KnowledgeCenterErrors>();
            KnowledgeCenterFiles = new HashSet<KnowledgeCenterFiles>();
            KnowledgeCenterPics = new HashSet<KnowledgeCenterPics>();
        }

        public int Id { get; set; }
        public int? Kind { get; set; }
        public string ProjectName { get; set; }
        public string Iisname { get; set; }
        public string Descreption { get; set; }
        public string SoureControlPath { get; set; }
        public string ProductionServer { get; set; }
        public string ProductionSiteUrl { get; set; }
        public string PreProductionSiteUrl { get; set; }
        public string TestSiteUrl { get; set; }
        public string TestUserName { get; set; }
        public string TestPassword { get; set; }
        public string SchedulerTime { get; set; }
        public string Notes { get; set; }

        public virtual KnowledgeCenterKinds KindNavigation { get; set; }
        public virtual ICollection<KnowledgeCenterErrors> KnowledgeCenterErrors { get; set; }
        public virtual ICollection<KnowledgeCenterFiles> KnowledgeCenterFiles { get; set; }
        public virtual ICollection<KnowledgeCenterPics> KnowledgeCenterPics { get; set; }
    }
}
