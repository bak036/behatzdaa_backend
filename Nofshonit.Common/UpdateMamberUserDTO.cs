using Nofshonit.Common.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common
{
    public class UpdateMamberUserDTO
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? AllowSmsAndMail { get; set; }

        public DateTime? BirthDate { get; set; }

        public string IdentityNumber { get; set; }

        public string MobilePhone { get; set; }

        public string PhoneNumber { get; set; }

        public string CityName { get; set; }

        public int? CityId { get; set; }

        public string StreetName { get; set; }

        public string HouseNumber { get; set; }

        public string ApartmentNumber { get; set; }

        public string Zip { get; set; }

        public string Email { get; set; }

        public int? FamilyStatus { get; set; }

        public int? NumOfChildren { get; set; }

        public EGender Gender { get; set; }

        public string PartnerEmail { get; set; }

        public ELoginType LoginType { get; set; }

        public ENewOrUpdate NewOrUpdate { get; set; }

        public string AccessID { get; set; }

        public bool EmailSubscribe { get; set; }

        public string FactorySymbol { get; set; }
        public string Entrance { get; set; }
        public string Mailbox { get; set; }
        public string PartnerPhone { get; set; }
        public bool IsUpdateDetailsApproved { get; set; }

    }
}
