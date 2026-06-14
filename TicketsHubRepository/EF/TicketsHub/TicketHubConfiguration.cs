using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class TicketHubConfiguration
    {
        public int TicketHubConfigurationId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
