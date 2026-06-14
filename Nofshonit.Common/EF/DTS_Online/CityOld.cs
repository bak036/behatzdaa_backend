using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CityOld
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityName2 { get; set; }
        public string CityName3 { get; set; }
        public byte? RegionId { get; set; }
        public int? IsraelPostId { get; set; }
        public int? GovId { get; set; }
    }
}
