using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.RequestDTOs
{
    public class CancelRequestDTO
    {
        public string UniqueId { get; set; }
        public string MemberId { get; set; }
        public string OrderGuid { get; set; }
        public string VariantBarCode { get; set; }
        public string OrderConfirmation { get; set; }
    }
}