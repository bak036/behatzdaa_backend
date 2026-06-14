using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDll
{
    public class GeneralOrdersItem
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
    }
}
