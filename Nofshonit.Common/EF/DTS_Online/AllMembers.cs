using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class AllMembers
    {
        public string MemberId { get; set; }
        public string Track2 { get; set; }
        public string MemberCardNumber { get; set; }
        public string MemberName { get; set; }
        public short? Active { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string Address { get; set; }
        public int? City { get; set; }
        public string CityName { get; set; }
        public string Zip { get; set; }
        public string Pobox { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public short? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? PlaceOfCertificate { get; set; }
        public short? FamilyStatus { get; set; }
        public DateTime? MariageDate { get; set; }
        public string PartnerName { get; set; }
        public DateTime? PartnerBirthDate { get; set; }
        public string Hodaa { get; set; }
        public string MemberSpecialId { get; set; }
        public short? PopulationType { get; set; }
        public decimal? WaitingBalance { get; set; }
        public decimal? WaitingBalanceLeumiCard { get; set; }
        public decimal? Hamara1Balance { get; set; }
        public string MoreDetailsXml { get; set; }
        public string FactorySymbol { get; set; }
        public DateTime? UserSiteLastLogin { get; set; }
        public string Tz { get; set; }
        public int? Darga { get; set; }
        public int? YamamLastYear { get; set; }
        public bool? AllowSmsAndMail { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
