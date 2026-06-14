using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Enums
{
    public enum ELoginType
    {
        MemberIdEqualsPassword = 0,
        EmailAndPass = 1,
        MemberIdAndPassword = 2,
        UserNameAndPass = 3,
        Facebook = 4,
        Google = 5,
        IdentityAndPassword = 6,
        OTPShortCode=7,
        BiometricToken = 8,
    };
}
