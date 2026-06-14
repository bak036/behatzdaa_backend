using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class TicketsHubJourneyLog
    {
        public int TicketsHubJourneyLogId { get; set; }
        public string EventId { get; set; }
        public string MemberId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string OrgId { get; set; }
    }
}
