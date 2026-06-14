using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CreditGuardOrders
    {
        public CreditGuardOrders()
        {
            CreditGuardOrderDetails = new HashSet<CreditGuardOrderDetails>();
        }

        public int TerminalNumber { get; set; }
        public long TranId { get; set; }
        public long? CardId { get; set; }
        public decimal Charged { get; set; }
        public string Last4Digits { get; set; }
        public string CardOwnerId { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? SentTime { get; set; }
        public DateTime InsertTime { get; set; }

        public virtual ICollection<CreditGuardOrderDetails> CreditGuardOrderDetails { get; set; }
    }
}
