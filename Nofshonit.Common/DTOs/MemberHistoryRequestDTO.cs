using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class MemberHistoryRequestDTO
    {

        public string UniqueId { get; set; }

        public string MemberId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<int> BenefitStatusId { get; set; }
        public MemberHistoryRequestDTO(MemberHistoryDTO memberHistoryDTO, string memberId, string organizationGuid)
        {
            MemberId = memberId;
            UniqueId = organizationGuid;
            FromDate = memberHistoryDTO.FromDate;
            ToDate = memberHistoryDTO.ToDate;
            BenefitStatusId = memberHistoryDTO.BenefitStatusId;
        }
    }
}
