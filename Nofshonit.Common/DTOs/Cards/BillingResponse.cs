using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Cards
{
    public class BillingResponse
    {
        public int BillingNum { get; set; }
        public bool StatusID { get; set; }
        public string StatusDescription { get; set; }
    }
}
