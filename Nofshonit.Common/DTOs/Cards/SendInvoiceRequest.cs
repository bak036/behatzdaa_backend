using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Cards
{
    public class SendInvoiceRequest
    {
        public string MemberId { get; set; }
        public string Email { get; set; }
        public string RequestId { get; set; }
        public string WalletName { get; set; }
        public float TotalSum { get; set; }
        public float Discount { get; set; }
        public string CCNUM { get; set; }
        public int NumOfPayments { get; set; }
        public int OrgId { get; set; }
        public long PaymentId { get; set; }
    }
}
