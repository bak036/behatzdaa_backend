using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class EventDTO
    {
        public string Name { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public int EventId { get; set; }
        public DateTime ExpireDate { get; set; }       
        public int OrderLimit { get; set; }
        public long CategoryId { get; set; }       
        public bool IsCampaign { get; set; }
    }
}
