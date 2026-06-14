using Nofshonit.Common.DTOs.Enums;
using System;
using static Nofshonit.Common.DTOs.Enums.ELoginType;

namespace Nofshonit.Common.DTOs
{
    public class JoinBySmsDTO
    {
        public string IdentityNumber { get; set; }

        public string code { get; set; }
    }
}
