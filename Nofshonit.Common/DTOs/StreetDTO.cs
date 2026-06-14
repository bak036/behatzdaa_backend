using System;
using System.Collections.Generic;
using System.Text;
using Nofshonit.Common.EF.DTS_Online;

namespace Nofshonit.Common.DTOs
{
    public class StreetDTO
    {
        public int? CityId { get; set; }

        public int StreetId { get; set; }

        public string StreetName { get; set; }

        public string CityName { get; set; }

        public int RegionID { get; set; }

        public string RegionName { get; set; }

        public int? IsraelPostId { get; set; }

        public int? GovId { get; set; }

    }


}
