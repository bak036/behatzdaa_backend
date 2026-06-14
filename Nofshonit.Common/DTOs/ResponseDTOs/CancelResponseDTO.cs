using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.ResponseDTOs
{
   

    public class CancelResponseDTO
    {
        public int Status { get; set; }
        public CancelDataDTO Data;
        public int? ErrorId { get; set; }
        public string ErrorDescription { get; set; }
        public string Log { get; set; }

        public class CancelDataDTO
        {
            public string MemberId { get; set; }
            public int DtsOrderId { get; set; }
            public string OrderGuid { get; set; }
            public string VariantBarCode { get; set; }
            public int CancelStatusId { get; set; }
            public string CancelStatusName { get; set; }
            public int CoinsRestored { get; set; }
            public decimal MoneyReturn { get; set; }
            public string OrderConfirmation { get; set; }

        }
    }
}
