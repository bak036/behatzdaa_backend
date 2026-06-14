using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class VarChangeLog
    {
        public int Id { get; set; }
        public string VarCurrent { get; set; }
        public string VarNew { get; set; }
        public DateTime? Dt { get; set; }
    }
}
