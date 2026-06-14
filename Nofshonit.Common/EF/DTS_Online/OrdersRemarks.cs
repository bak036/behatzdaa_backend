using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrdersRemarks
    {
        public int IndexNum { get; set; }
        public DateTime InsertDate { get; set; }
        public string MemberId { get; set; }
        public long Asmchta { get; set; }
        public string Remarks { get; set; }
    }
}
