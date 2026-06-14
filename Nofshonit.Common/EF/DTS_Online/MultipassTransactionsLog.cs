using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MultipassTransactionsLog
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TransactionsCount { get; set; }
        public DateTime? LastOperationDate { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
