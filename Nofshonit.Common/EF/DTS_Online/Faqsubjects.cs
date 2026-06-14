using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Faqsubjects
    {
        public int QuestionsSubjectKey { get; set; }
        public string QuestionsSubjectName { get; set; }
        public int? Sort { get; set; }
        public int? OrganizationId { get; set; }
    }
}
