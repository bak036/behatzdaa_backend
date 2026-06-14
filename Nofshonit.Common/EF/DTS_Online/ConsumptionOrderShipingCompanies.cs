using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ConsumptionOrderShipingCompanies
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        public byte DeliveryType { get; set; }
    }
}
