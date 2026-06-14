using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ManagementBusinessId
    {
        public short TypeId { get; set; }
        public int? LowBuisnessId { get; set; }
        public int? HigthBuisnessId { get; set; }
    }
}
