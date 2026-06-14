using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Business
{
    public class BusinessDTO
    {
        public string Name { get; set; }
        public long BusinessId { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string StreetNumber { get; set; }
        public string Phone { get; set; }
        public string ImgUrl { get; set; }
        public int? Parking { get; set; }
        public int? CrippleAccess { get; set; }
        public int? Toilet { get; set; }
        public int? Restaurant { get; set; }

        public int? EquipmentRenting { get; set; }
        public int? Challenging { get; set; }
        public int? AboveAge { get; set; }

        public string LocationExplain { get; set; }
        public bool? HaveSubBranch { get; set; }
        public int? SumSubBranch { get; set; }
        public string WebSite { get; set; }

        public string OpenHours { get; set; }
        public string GpsPointer_Lat { get; set; }

        public string GpsPointer_Lon { get; set; }
        public string Region { get; set; }

        public string Massage { get; set; }
    }
}
