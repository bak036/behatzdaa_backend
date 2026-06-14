using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Constants
{
    public static class SessionKeys
    {
        public static string OTP_KEY(string memberId)
        {
            return string.Format("OTP_ID_{0}", memberId);
        }
    }
}
