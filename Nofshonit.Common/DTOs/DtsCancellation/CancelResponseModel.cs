using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.DtsCancellation
{
    public class CancelResponseModel
    {

        public bool Success { get; set; }
        public int CouponCancelRequestID { get; set; }
        public int CancelStatusbej { get; set; }
        public string CancelText { get; set; }
        public string ConfirmationNumber { get; set; }
        public long MemberOrderAsmchta { get; set; }
    }
}
