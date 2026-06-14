using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Category
{
    public class LocationDto
    {
        public string Address { get; set; }

        public string Phone { get; set; }
        public int? RegionId { get; set; }

        public string RegionName { get; set; }

        public string FriendlyName { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public string OpenHours { get; set; }

        public int BusinessModeId { get; set; }

        public string BusinessModeName { get; set; }
        public string BusinessUniqueNumber { get; set; }
        public string BusinessTaxName { get; set; }
        public string GpsPointer_Lat { get; set; }
        public string GpsPointer_Lon { get; set; }
    }
}
