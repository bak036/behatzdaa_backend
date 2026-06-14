using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.ResponseDTOs
{
    public class CancelOrderResponseDTO
    {
        public int Status { get; set; }
        public int? ErrorId { get; set; }
        public CancelOrderDataDTO Data = new CancelOrderDataDTO();
        public string ErrorDescription { get; set; }
        public string Log { get; set; }
    }

    public class CancelOrderDataDTO
    {
        public string MemberId { get; set; }
        public int DtsOrderId { get; set; }
        public string OrderGuid { get; set; }
        public List<CancelOrderVariantDTO> Variants { get; set; }
        public int CoinsRestored { get; set; }
        public decimal MoneyReturn { get; set; }
        public string CancelStatusName { get; set; }
        public int CancelStatusId { get; set; }

    }

    public class CancelOrderVariantDTO
    {
        public string BarCode { get; set; }
        public string CancelationStatus { get; set; }
        public int CancelStatusId { get; set; }
        public string OrderConfirmation { get; set; }
    }
}
