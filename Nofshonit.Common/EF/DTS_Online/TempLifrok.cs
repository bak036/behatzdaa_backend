using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TempLifrok
    {
        public string Idmember { get; set; }
        public long CardNo { get; set; }
        public decimal BuySum { get; set; }
        public decimal? Balance { get; set; }
        public decimal? UnloadSum { get; set; }
    }
}
