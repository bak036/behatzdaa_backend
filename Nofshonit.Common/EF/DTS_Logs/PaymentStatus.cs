using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public partial class PaymentStatus
    {
        public PaymentStatus()
        {
            PaymentInquireTransaction = new HashSet<PaymentInquireTransaction>();
            PaymentTransaction = new HashSet<PaymentTransaction>();
        }

        public int PaymentStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentInquireTransaction> PaymentInquireTransaction { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransaction { get; set; }
    }
}
