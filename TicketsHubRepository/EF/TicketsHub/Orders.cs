using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class Orders
    {
        public Orders()
        {
            OrderTickets = new HashSet<OrderTickets>();
        }

        public int OrderId { get; set; }
        public Guid OrderGuiud { get; set; }
        public int OrganizationId { get; set; }
        public int SystemId { get; set; }
        public int? ExternalSystemOrderId { get; set; }
        public DateTime? ReserveTimeStamp { get; set; }
        public int EventId { get; set; }
        public DateTime ExternalSystemEventDate { get; set; }
        public TimeSpan ExternalSystemEventTime { get; set; }
        public int ExternalSystemVenueId { get; set; }
        public int OrderStatusId { get; set; }
        public string ExternalSystemVenueName { get; set; }
        public string ReserveJson { get; set; }
        public bool IsBooked { get; set; }
        public DateTime? BookingTimeStamp { get; set; }
        public string CategoryName { get; set; }
        public bool? IsSelfPrint { get; set; }
        public bool IsValid { get; set; }
        public DateTime? CancelTimeStamp { get; set; }
        public int? CancelReasonId { get; set; }
        public int? CancelRefId { get; set; }
        public string MemberId { get; set; }
        public int? JourneyId { get; set; }

        public virtual CancelReasons CancelReason { get; set; }
        public virtual Events Event { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<OrderTickets> OrderTickets { get; set; }
    }
}
