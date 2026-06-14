using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class Events
    {
        public Events()
        {
            Orders = new HashSet<Orders>();
            TicketsHubSyncLog = new HashSet<TicketsHubSyncLog>();
        }

        public int EventId { get; set; }
        public int TourId { get; set; }
        public string MapJson { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public int EventSeriesId { get; set; }
        public bool IsReleased { get; set; }
        public bool IsSelfPrint { get; set; }
        public int VenueId { get; set; }
        public string VeneueName { get; set; }
        public bool IsSeatMap { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<TicketsHubSyncLog> TicketsHubSyncLog { get; set; }
    }
}
