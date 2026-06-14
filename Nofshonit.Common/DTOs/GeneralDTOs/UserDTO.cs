using Nofshonit.Common.DTOs.Enums;
using System;
using static Nofshonit.Common.DTOs.Enums.ELoginType;

namespace Nofshonit.Common.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public long MemberIdIdentity { get; set; }
        public string FirstName { get; set; }
        public bool? AllowSmsAndMail { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string IdentityNumber { get; set; }

        public string MobilePhone { get; set; }

        public string PhoneNumber { get; set; }

        public string CityName { get; set; }

        public int? CityId { get; set; }

        public int? RegionId { get; set; }

        public string StreetName { get; set; }

        public string HouseNumber { get; set; }

        public string ApartmentNumber { get; set; }

        public string Zip { get; set; }

        public string Email { get; set; }

        public int? FamilyStatus { get; set; }

        public int? NumOfChildren { get; set; }

        public EGender Gender { get; set; }

        public EEducation? Education { get; set; }

        public string PartnerName { get; set; }

        public string PartnerEmail { get; set; }
        public string PartnerPhone { get; set; }


        public string Username { get; set; }

        public string Password { get; set; }

        public ELoginType LoginType { get; set; }

        public string AccessID { get; set; }

        public string Token { get; set; }

        public int? PremiumType { get; set; }
        public int? Darga { get; set; }

		public string ForgetPasswordToken { get; set; }

		public DateTime? ForgetPasswordCreated { get; set; }

        public string MemberSpecialID { get; set; }

        public DateTime? LastUpdateMember { get; set; }

        public int? LoginAttempts { get; set; }

        public DateTime? LastLoginAttempts { get; set; }

        public string IdentityGuid { get; set; }

        public int ClubCreditCard { get; set; }
        public DateTime? ClubCreditCardUpdateDate { get; set; }

        public string PinCode { get; set; }

        public string FactorySymbol { get; set; }

        public string AccessToken { get; set; }

        public string CardNumber { get; set; }
        public string Entrance { get; set; }
        public string Mailbox { get; set; }

        public short? Active { get; set; }
        public bool IsPremiumTypeChanged { get; set; } = false;

        public DateTime? PrivacyPolicyAcceptedDate { get; set; }
        public DateTime? CookiesAcceptedDate { get; set; }

        public ENeedUpdateOrFinishRegistration? UpdateOrRegister { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public UserDTO()
        {

        }

    }
}
