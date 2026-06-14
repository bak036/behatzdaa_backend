using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class MinistryOfDefenceValidationResponseDTO
    {
        public bool Status { get; set; }

        public int? MemberStatus { get; set; }

        public string MemberSpecialID { get; set; }
        public bool ShouldSkipHistadrutUpdate { get; set; }

        public MinistryOfDefenceValidationResponseDTO(bool value=false)
        {
            Status = false;
            MemberStatus = 0;
            MemberSpecialID = "0";
        }
    }

}
