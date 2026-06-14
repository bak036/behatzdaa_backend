using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Theaters
    {
        public short TheaterCode { get; set; }
        public string TheaterName { get; set; }
        public int? CityId { get; set; }
    }
}
