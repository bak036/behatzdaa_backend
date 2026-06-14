using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ConsumptionOrderStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string DescriptionSite { get; set; }
        public string DescriptionCustomerService { get; set; }
        public string Sms { get; set; }
    }
}
