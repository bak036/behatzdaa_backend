using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TeleclalDlrTable
    {
        public long MessageId { get; set; }
        public string SubscriberNumber { get; set; }
        public string MessageSource { get; set; }
        public DateTime SendTimeStamp { get; set; }
        public DateTime DeliveryTimeStamp { get; set; }
        public string Status { get; set; }
        public string Rem { get; set; }
        public int? OrganizationId { get; set; }
    }
}
