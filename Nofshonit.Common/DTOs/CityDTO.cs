using System;
using System.Collections.Generic;
using System.Text;
using Nofshonit.Common.EF.DTS_Online;

namespace Nofshonit.Common.DTOs
{
    public class CityDTO
    {
        public int? CityId { get; set; }
        public int? GovId { get; set; }

        public string CityName { get; set; }

        public int? RegionID { get; set; }

    }

}
