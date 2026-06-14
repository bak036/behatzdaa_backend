using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class DwhCustomersChangeLog
    {
        public long RowId { get; set; }
        public string DataBaseName { get; set; }
        public string CustomerId { get; set; }
        public int? Unsubscribe { get; set; }
        public int? Inactive { get; set; }
        public int? CreditCardHolder { get; set; }
        public DateTime? Login { get; set; }
        public DateTime? Created { get; set; }
    }
}
