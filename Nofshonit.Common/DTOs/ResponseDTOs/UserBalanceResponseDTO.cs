using Nofshonit.Common.DTOs.GeneralDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.ResponseDTOs
{
    public class UserBalanceResponseDTO
    {

        public byte Status { get; set; }
        public string ErrorDescription { get; set; }
        public int? ErrorId { get; set; }
        public string MemberId { get; set; }
        public string DtsId { get; set; }
        public int CoinsBalance { get; set; }
        public LimitsDTO Limits { get; set; }

    }
}
