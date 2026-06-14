using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcMediaPush
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string MwcdateTime { get; set; }
        public string ChainName { get; set; }
        public string BranchName { get; set; }
        public decimal Amount { get; set; }
    }
}
