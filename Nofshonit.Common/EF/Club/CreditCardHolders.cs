using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class CreditCardHolders
    {
        public long Id { get; set; }
        public string Tz { get; set; }
        public string FullName { get; set; }
        public bool HasCreditCard { get; set; }
        public byte BenefitQuantitiy { get; set; }
        public DateTime DateSent { get; set; }
    }
}
