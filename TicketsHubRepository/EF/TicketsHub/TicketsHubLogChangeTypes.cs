using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class TicketsHubLogChangeTypes
    {
        public TicketsHubLogChangeTypes()
        {
            TicketsHubSyncLog = new HashSet<TicketsHubSyncLog>();
        }

        public int LogChangeTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TicketsHubSyncLog> TicketsHubSyncLog { get; set; }
    }
}
