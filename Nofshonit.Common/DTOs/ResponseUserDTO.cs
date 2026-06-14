using Nofshonit.Common.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class ResponseUserDTO
    {

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public long MemberIdIdentity { get; set; }

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
        public string Address { get; set; }

        public string Zip { get; set; }
        public string Entrance { get; set; }
        public string Mailbox { get; set; }

        public string Email { get; set; }

        public int? FamilyStatus { get; set; }

        public int? NumOfChildren { get; set; }

        public EGender Gender { get; set; }

        public string PartnerName { get; set; }

        public string PartnerEmail { get; set; }
        public string PartnerPhone { get; set; }

        public ELoginType LoginType { get; set; }

        public string AccessID { get; set; }

        public int? PremiumType { get; set; }
        public bool IsPremiumTypeChanged { get; set; }

        public string Token { get; set; }

        public string MemberSpecialID { get; set; }

        public string ForgetPasswordToken { get; set; }

        public DateTime? ForgetPasswordCreated { get; set; }

        public int ClubCreditCard { get; set; }

        public bool HasPinCode { get; set; }

        public string PinCode { get; set; }

        public string FactorySymbol { get; set; }

        public string AccessToken { get; set; }

        public string MemberGuid { get; set; }

        public string CardNumber { get; set; }
        public bool? AllowSmsAndEmail { get; set; }
        public DateTime? LastUpdateMember { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string CreditCardClub { get; set; }

        public string CVV { get; set; }

        public ENeedUpdateOrFinishRegistration? UpdateOrRegister { get; set; }

        public DateTime? PrivacyPolicyAcceptedDate { get; set; }
        public DateTime? CookiesAcceptedDate { get; set; }

        public ResponseUserDTO()
        {}

        public ResponseUserDTO(UserDTO userDto)
        {
            Id = userDto.Id;
            MemberIdIdentity = userDto.MemberIdIdentity;
            FirstName =userDto.FirstName;
            LastName = userDto.LastName;
            BirthDate = userDto.BirthDate;
            IdentityNumber = userDto.IdentityNumber;
            MobilePhone = userDto.MobilePhone;
            PhoneNumber = userDto.PhoneNumber;
            CityName = userDto.CityName;
            CityId = userDto.CityId;
            RegionId = userDto.RegionId;
            StreetName = userDto.StreetName;
            HouseNumber = userDto.HouseNumber;
            ApartmentNumber = userDto.ApartmentNumber;
            Zip = userDto.Zip;
            Entrance = userDto.Entrance;
            Mailbox = userDto.Mailbox;
            Email = userDto.Email;
            FamilyStatus = userDto.FamilyStatus;
            NumOfChildren = userDto.NumOfChildren;
            Gender = userDto.Gender;
            PartnerName = userDto.PartnerName;
            PartnerEmail = userDto.PartnerEmail;
            PartnerPhone = userDto.PartnerPhone;
            LoginType = userDto.LoginType;
            AccessID = userDto.AccessID;
            PremiumType = userDto.PremiumType;
            IsPremiumTypeChanged = userDto.IsPremiumTypeChanged;
            Token = userDto.Token;
            MemberSpecialID = userDto.MemberSpecialID;
			ForgetPasswordCreated = userDto.ForgetPasswordCreated;
			ForgetPasswordToken = userDto.ForgetPasswordToken;
            ClubCreditCard = userDto.ClubCreditCard;
            PinCode = userDto.PinCode;
            HasPinCode = userDto.PinCode != null;
            FactorySymbol = userDto.FactorySymbol;
            AccessToken = userDto.AccessToken;
            LastUpdateMember = userDto.LastUpdateMember;
            UpdateOrRegister = userDto.UpdateOrRegister;
            CardNumber = userDto.CardNumber;
            AllowSmsAndEmail = userDto.AllowSmsAndMail;
            CookiesAcceptedDate = userDto.CookiesAcceptedDate;
            PrivacyPolicyAcceptedDate = userDto.PrivacyPolicyAcceptedDate;
        }
    }
}
