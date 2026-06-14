using CreditServices.Interfaces;
using Nofshonit.Common.DTOs.GeneralDTOs;
using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.ResponseDTOs
{

    public class PurchaseResponseDTO 
    {
        public int Status { get; set; }
        public PurchaseDTO Data  = new PurchaseDTO();
        public int? ErrorId { get; set; }
        public string ErrorDescription { get; set; }
        public string Log { get; set; }
    }
    public class PurchaseDTO 
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CreditCardExpiration { get; set; }
        public int DtsOrderId { get; set; }
        public decimal? TotalPayments { get; set; }
        public int NumOfPayments { get; set; }
        public string OrderConfirmation { get; set; }
        public List<OrderVariantDTO> Variants { get; set; }
        public UserBalanceResponseDTO GetUserBalance { get; set; }

        public string OrderGuid { get; set; }
        public long PaymentId { get; set; }
        public TransactionResults CreditGuardResult { get; set; }

    }
}
