using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class AllMemberHistadrut
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
        public int? YamamLastYear { get; set; }
        public string Tz { get; set; }
        public int? Darga { get; set; }
        public int? OccupationId { get; set; }
        public int? PremiumType { get; set; }
        public int? AnotherPremiumType { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool? AllowSmsAndMail { get; set; }
        public int? NumOfChildren { get; set; }
        public int? EducationId { get; set; }
        public string AddressMail { get; set; }
        public bool? IsAddresToSendMail { get; set; }
        public string PartnerEmail { get; set; }
        public DateTime? DateCreated { get; set; }
        public string EmployeeNum { get; set; }
        public long MemberIdIdentity { get; set; }
        public DateTime? LastUpdateMember { get; set; }
        public DateTime? AprroveTakanonDate { get; set; }
        public DateTime? DateOrder { get; set; }
        public string ActiveCardDescription { get; set; }
        public string PinCode { get; set; }
        public string EncryptedUserPassword { get; set; }
        public DateTime? PasswordChanged { get; set; }
        public int ClubCreditCard { get; set; }
        public DateTime? ClubCreditCardUpdateDate { get; set; }
    }
}
