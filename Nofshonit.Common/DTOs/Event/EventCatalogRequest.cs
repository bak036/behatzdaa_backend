using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class EventCatalogRequest
    {
        public int EventId { get; set; }
        public string MemberId { get; set; }

        public int OrganizationId { get; set; }
    }
}
