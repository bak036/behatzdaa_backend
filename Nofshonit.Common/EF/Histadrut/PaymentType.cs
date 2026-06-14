using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Histadrut
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            PaymentTransaction = new HashSet<PaymentTransaction>();
        }

        public int PaymentTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentTransaction> PaymentTransaction { get; set; }
    }
}
