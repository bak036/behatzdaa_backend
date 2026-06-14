using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SurveyAnswers
    {
        public long AnswerId { get; set; }
        public DateTime ParticipationDate { get; set; }
        public string ParticipateId { get; set; }
        public int ParticipateOrganizationId { get; set; }
        public long QuestionId { get; set; }
        public int Answer { get; set; }
    }
}
