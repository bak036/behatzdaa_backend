using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LoadMoneyIframe
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Guid IframeGuid { get; set; }
        public short PopulationType { get; set; }
        public int? SerieId { get; set; }
        public string CardImage { get; set; }
    }
}
