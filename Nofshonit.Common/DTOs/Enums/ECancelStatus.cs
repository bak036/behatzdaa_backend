using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Enums
{
    public enum ECancelStatus
    {
        InCancelProcess = 1,
        CancelledNowSuccessfully = 2,
        BlockCouponToCancel = 3,
    }
}
