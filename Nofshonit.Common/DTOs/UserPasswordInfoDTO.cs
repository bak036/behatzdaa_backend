using Nofshonit.Common.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class UserPasswordInfoDTO
    {
        public string MemberId { get; set; }

        public ENewOrUpdate NewOrUpdate { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ForgetPasswordToken { get; set; }

    }
}
