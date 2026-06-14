using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class CancelReasons
    {
        public CancelReasons()
        {
            Orders = new HashSet<Orders>();
        }

        public int CancelReasonId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
