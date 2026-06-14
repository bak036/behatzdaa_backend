using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Orders
    {
        public int OrderId { get; set; }
        public string MemberId { get; set; }
        public DateTime InsertDate { get; set; }
        public bool IsSentToFriend { get; set; }
        public string FriendName { get; set; }
        public string FriendMobile { get; set; }
        public int NumberOfRetries { get; set; }
        public string ExternalGuid { get; set; }
        public string EventsGuid { get; set; }
        public string CreditCardToken { get; set; }
        public int OrderStatusId { get; set; }
        public string CreditCard16Digits { get; set; }
        public string CreditCardExpirey { get; set; }
        public string PaymentToken { get; set; }
        public bool IsTicketHubOrder { get; set; }
        public int? TicketHubOrderId { get; set; }
        public string Slink { get; set; }
        public DateTime? OrderReminderDate { get; set; }

        public virtual AllMembers Member { get; set; }
    }
}
