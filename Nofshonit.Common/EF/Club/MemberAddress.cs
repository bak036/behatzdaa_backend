using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class MemberAddress
    {
        public int MemberAddressId { get; set; }

        public DateTime? InsertDate { get; set; }

        public string MemberId { get; set; }

        public string StreetName { get; set; }

        public string HouseNumber { get; set; }

        public string ApartmentNumber { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public string Zip { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string Mailbox { get; set; }

        public string Entrance { get; set; }

    }

}