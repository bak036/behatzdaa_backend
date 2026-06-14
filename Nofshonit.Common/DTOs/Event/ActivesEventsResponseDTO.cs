using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class ActivesEventsResponseDTO
    {
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public string VeneueName { get; set; }
        public string Title { get; set; }
        public int VenueId { get; set; }
    }
}
