using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TaklaTable
    {
        public long RowIndex { get; set; }
        public int NetId { get; set; }
        public DateTime DateOpen { get; set; }
        public string OpenBy { get; set; }
        public short System { get; set; }
        public string BusinessName { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public DateTime? DateClose { get; set; }
        public string CloseBy { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
    }
}
