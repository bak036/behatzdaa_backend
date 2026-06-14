using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SurveyQuestions
    {
        public long QuestionId { get; set; }
        public DateTime InsertedDate { get; set; }
        public int? InsertOperatorId { get; set; }
        public string Question { get; set; }
        public bool QuestionActive { get; set; }
        public int? Sort { get; set; }
        public int? OrgId { get; set; }
    }
}
