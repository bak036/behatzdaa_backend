using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class TicketsHubSyncLog
    {
        public int TicketrsHubSuncLogId { get; set; }
        public int? DataCenterId { get; set; }
        public int? EventId { get; set; }
        public int? TourId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int TicketsHubSyncLogTypeId { get; set; }
        public string Description { get; set; }

        public virtual Events Event { get; set; }
        public virtual TicketsHubLogChangeTypes TicketsHubSyncLogType { get; set; }
    }
}
