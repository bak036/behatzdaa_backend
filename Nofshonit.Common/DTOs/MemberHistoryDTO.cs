using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class MemberHistoryDTO
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<int> BenefitStatusId { get; set; }
    }
}
