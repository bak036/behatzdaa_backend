using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.RequestDTOs
{
    public class ConfirmationPageDTO
    {
        public string UniqueId { get; set; }
        public string MemberId { get; set; }
        public string OrderGuid { get; set; }
    }
}
