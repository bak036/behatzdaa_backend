using AutoMapper;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Logs;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.MapperManagement
{
    public class MapperManager : Profile, IMapperManager
    {
        private IMapper _mapper { get; set; }

        public void Init(IMapper mapper)
        {
            _mapper = mapper;
        }
        public MapperManager()
        {

            CreateMap<Nofshonit.Common.EF.Club.AllMembers, UserDTO>()
            .ForMember(dest => dest.Id, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.FirstName, source => source.MapFrom(src => src.MemberFirstName))
            .ForMember(dest => dest.LastName, source => source.MapFrom(src => src.MemberLastName))
            .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
            .ForMember(dest => dest.AccessID, source => source.MapFrom(src => src.AccessId))
            .ForMember(dest => dest.IdentityNumber, source => source.MapFrom(src => src.Tz))
            .ForMember(dest => dest.Password, source => source.MapFrom(src => src.EncryptedUserPassword))
            .ForMember(dest => dest.PremiumType, source => source.MapFrom(src => src.PremiumType))
            .ForMember(dest => dest.Darga, source => source.MapFrom(src => src.Darga))
            .ForMember(dest => dest.BirthDate, source => source.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.MobilePhone, source => source.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CityName, source => source.MapFrom(src => src.CityName))
            .ForMember(dest => dest.StreetName, source => source.MapFrom(src => src.StreetName))
            .ForMember(dest => dest.HouseNumber, source => source.MapFrom(src => src.HouseNumber))
            .ForMember(dest => dest.ApartmentNumber, source => source.MapFrom(src => src.ApartmentNumber))
            .ForMember(dest => dest.Zip, source => source.MapFrom(src => src.Zip))
            .ForMember(dest => dest.FamilyStatus, source => source.MapFrom(src => src.FamilyStatus))
            .ForMember(dest => dest.NumOfChildren, source => source.MapFrom(src => src.NumOfChildren))
            .ForMember(dest => (short)dest.Gender, source => source.MapFrom(src => (EGender)(src.Gender ?? 0)))
            //.ForMember(dest => (short)dest.Education, source => source.MapFrom(src => (EEducation)src.EducationId))
            .ForMember(dest => dest.PartnerEmail, source => source.MapFrom(src => src.PartnerEmail))
            .ForMember(dest => dest.PartnerName, source => source.MapFrom(src => src.PartnerName))
            .ForMember(dest => dest.PremiumType, source => source.MapFrom(src => src.PremiumType))
            .ForMember(dest => dest.ForgetPasswordCreated, source => source.MapFrom(src => src.ForgetPasswordCreated))
            .ForMember(dest => dest.ForgetPasswordToken, source => source.MapFrom(src => src.ForgetPasswordToken))
            .ForMember(dest => dest.MemberSpecialID, source => source.MapFrom(src => src.MemberSpecialId))
            .ForMember(dest => dest.RegistrationDate, source => source.MapFrom(src => src.RegistrationDate))
            .ForMember(dest => dest.Token, source => source.MapFrom(src => src.UserToken))
            .ForMember(dest => dest.AccessID, source => source.MapFrom(src => src.AccessId))
            .ForMember(dest => dest.LastUpdateMember, source => source.MapFrom(src => src.LastUpdateMember))
            .ForMember(dest => dest.LoginAttempts, source => source.MapFrom(src => src.LoginAttempts))
            .ForMember(dest => dest.LastLoginAttempts, source => source.MapFrom(src => src.LastLoginAttempts))
            .ForMember(dest => dest.IdentityGuid, source => source.MapFrom(src => src.IdentityGuid))
            .ForMember(dest => dest.ClubCreditCard, source => source.MapFrom(src => src.ClubCreditCard))
            .ForMember(dest => dest.ClubCreditCardUpdateDate, source => source.MapFrom(src => src.ClubCreditCardUpdateDate))
            .ForMember(dest => dest.CityId, source => source.MapFrom(src => src.City))
            .ForMember(dest => dest.CookiesAcceptedDate, source => source.MapFrom(src => src.CookiesAcceptedDate))
            .ForMember(dest => dest.PrivacyPolicyAcceptedDate, source => source.MapFrom(src => src.PrivacyPolicyAcceptedDate))
            .ForMember(dest => dest.CardNumber, source => source.MapFrom(src => src.MemberCardNumber));

            CreateMap<UserDTO, Nofshonit.Common.EF.Club.AllMembers>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.Id))
            .ForMember(dest => dest.MemberFirstName, source => source.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.MemberLastName, source => source.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
            .ForMember(dest => dest.AccessId, source => source.MapFrom(src => src.AccessID))
            .ForMember(dest => dest.Tz, source => source.MapFrom(src => src.IdentityNumber))
            .ForMember(dest => dest.EncryptedUserPassword, source => source.MapFrom(src => src.Password))
            .ForMember(dest => dest.PremiumType, source => source.MapFrom(src => src.PremiumType))
            .ForMember(dest => dest.Darga, source => source.MapFrom(src => src.Darga))
            .ForMember(dest => dest.BirthDate, source => source.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.MobilePhone, source => source.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CityName, source => source.MapFrom(src => src.CityName))
            .ForMember(dest => dest.StreetName, source => source.MapFrom(src => src.StreetName))
            .ForMember(dest => dest.HouseNumber, source => source.MapFrom(src => src.HouseNumber))
            .ForMember(dest => dest.ApartmentNumber, source => source.MapFrom(src => src.ApartmentNumber))
            .ForMember(dest => dest.Zip, source => source.MapFrom(src => src.Zip))
            .ForMember(dest => dest.FamilyStatus, source => source.MapFrom(src => src.FamilyStatus))
            .ForMember(dest => dest.NumOfChildren, source => source.MapFrom(src => src.NumOfChildren))
            .ForMember(dest => (EGender)dest.Gender, source => source.MapFrom(src => (short)src.Gender))
            //.ForMember(dest => (EEducation)dest.EducationId, source => source.MapFrom(src => (short)src.Education))
            .ForMember(dest => dest.PartnerEmail, source => source.MapFrom(src => src.PartnerEmail))
            .ForMember(dest => dest.PartnerName, source => source.MapFrom(src => src.PartnerName))
            .ForMember(dest => dest.PremiumType, source => source.MapFrom(src => src.PremiumType))
            .ForMember(dest => dest.ForgetPasswordCreated, source => source.MapFrom(src => src.ForgetPasswordCreated))
            .ForMember(dest => dest.ForgetPasswordToken, source => source.MapFrom(src => src.ForgetPasswordToken))
            .ForMember(dest => dest.MemberSpecialId, source => source.MapFrom(src => src.MemberSpecialID))
            .ForMember(dest => dest.RegistrationDate, source => source.MapFrom(src => src.RegistrationDate))
            .ForMember(dest => dest.UserToken, source => source.MapFrom(src => src.Token))
            .ForMember(dest => dest.AccessId, source => source.MapFrom(src => src.AccessID))
            .ForMember(dest => dest.LastUpdateMember, source => source.MapFrom(src => src.LastUpdateMember))
            .ForMember(dest => dest.LoginAttempts, source => source.MapFrom(src => src.LoginAttempts))
            .ForMember(dest => dest.LastLoginAttempts, source => source.MapFrom(src => src.LastLoginAttempts))
            .ForMember(dest => dest.IdentityGuid, source => source.MapFrom(src => src.IdentityGuid))
            .ForMember(dest => dest.ClubCreditCard, source => source.MapFrom(src => src.ClubCreditCard))
            .ForMember(dest => dest.ClubCreditCardUpdateDate, source => source.MapFrom(src => src.ClubCreditCardUpdateDate))
            .ForMember(dest => dest.City, source => source.MapFrom(src => src.CityId))
            .ForMember(dest => dest.CookiesAcceptedDate, source => source.MapFrom(src => src.CookiesAcceptedDate))
            .ForMember(dest => dest.PrivacyPolicyAcceptedDate, source => source.MapFrom(src => src.PrivacyPolicyAcceptedDate))
            .ForMember(dest => dest.MemberCardNumber, source => source.MapFrom(src => src.CardNumber));

            CreateMap<Nofshonit.Common.EF.Club.AllMembers, ResponseUserDTO>()
            .ForMember(dest => dest.Id, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.Token, source => source.MapFrom(src => src.UserToken));

            CreateMap<ResponseUserDTO, Nofshonit.Common.EF.Club.AllMembers>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserToken, source => source.MapFrom(src => src.Token));

            CreateMap<Atractionsorders, OrderDTO>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.Type, source => source.MapFrom(src => src.GetType().Name));

            CreateMap<OrderDTO, Atractionsorders>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId));

            CreateMap<Moviesorders, OrderDTO>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.Type, source => source.MapFrom(src => src.GetType().Name));

            CreateMap<OrderDTO, Moviesorders>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId));

            CreateMap<Spaorders, OrderDTO>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.Type, source => source.MapFrom(src => src.GetType().Name));

            CreateMap<OrderDTO, Spaorders>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId));

            CreateMap<Tzimersorders, OrderDTO>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.Type, source => source.MapFrom(src => src.GetType().Name));

            CreateMap<OrderDTO, Tzimersorders>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.OrderDate, source => source.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderId, source => source.MapFrom(src => src.OrderId));

            CreateMap<UserDTO, ContactUsDTO>()
            .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdentityNumber, source => source.MapFrom(src => src.IdentityNumber))
            .ForMember(dest => dest.FullName, source => source.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(dest => dest.InputEmail, source => source.MapFrom(src => src.Email));

            CreateMap<ContactUsDTO, Crm>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.Id))
            .ForMember(dest => dest.MemberName, source => source.MapFrom(src => src.FullName))
            .ForMember(dest => dest.CrmDescription, source => source.MapFrom(src => src.Description))
            .ForMember(dest => dest.CrmTypeId, source => source.MapFrom(src => src.CrmSubjectId))
            .ForMember(dest => dest.CrmSubjectId, source => source.MapFrom(src => src.CrmType));

            CreateMap<Nofshonit.Common.EF.Club.AllMembers, Crm>()
            .ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.MemberId))
            .ForMember(dest => dest.MemberName, source => source.MapFrom(src => src.MemberFirstName + " " + src.MemberLastName))
            .ForMember(dest => dest.NotMember, source => source.MapFrom(src => false));

            CreateMap<ContactUsDTO, EmailQueue>()
            .ForMember(dest => dest.EmailQueueUsersId, source => source.MapFrom(src => Int32.Parse(src.Id)))
            .ForMember(dest => dest.EmailSubject, source => source.MapFrom(src => src.Subject))
            .ForMember(dest => dest.EmailBody, source => source.MapFrom(src => src.Description))
            .ForMember(dest => dest.EmailFrom, source => source.MapFrom(src => src.InputEmail));

			CreateMap<ProductDTO, ShopingBasket>()
				.ForMember(dest => dest.CategoryNumber, source => source.MapFrom(src => src.CategoryNumber))
				.ForMember(dest => dest.CreateDate, source => source.MapFrom(src => DateTime.Now));

            CreateMap<ShopingBasket, ProductDTO>()
                .ForMember(dest => dest.CategoryNumber, source => source.MapFrom(src => src.CategoryNumber));

            CreateMap<VariantDTO, ProductsVars>()
                .ForMember(dest => dest.FullBarCode, source => source.MapFrom(src => src.BarCode))
                .ForMember(dest => dest.ShortNameVar, source => source.MapFrom(src => src.Name))
                .ForMember(dest => dest.EndDate, source => source.MapFrom(src => src.ExpireDate))
                //.ForMember(dest => dest., source => source.MapFrom(src => src.OrderLimit))
                .ForMember(dest => dest.CupaPrice, source => source.MapFrom(src => src.KupaPrice))
                .ForMember(dest => dest.IsSendToFriend, source => source.MapFrom(src => src.IsSendToFriend))
                .ForMember(dest => dest.RedimTypeId, source => source.MapFrom(src => src.RedimTypeId));

            CreateMap<ProductsVars, VariantDTO>()
                .ForMember(dest => dest.BarCode, source => source.MapFrom(src => src.FullBarCode))
                .ForMember(dest => dest.Name, source => source.MapFrom(src => src.ShortNameVar))
                .ForMember(dest => dest.ExpireDate, source => source.MapFrom(src => src.EndDate)) // ==validDate?
                                                                                                  //.ForMember(dest => dest., source => source.MapFrom(src => src.OrderLimit))
                .ForMember(dest => dest.KupaPrice, source => source.MapFrom(src => src.CupaPrice))
                .ForMember(dest => dest.IsSendToFriend, source => source.MapFrom(src => src.IsSendToFriend))
                .ForMember(dest => dest.RedimTypeId, source => source.MapFrom(src => src.RedimTypeId));

			CreateMap<GetUserDTO, Nofshonit.Common.EF.Club.AllMembers>()
			.ForMember(dest => dest.MemberId, source => source.MapFrom(src => src.Id))
			.ForMember(dest => dest.MemberFirstName, source => source.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.MemberLastName, source => source.MapFrom(src => src.LastName))
			.ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
			.ForMember(dest => dest.ApartmentNumber, source => source.MapFrom(src => src.ApartmentNumber))
			.ForMember(dest => dest.BirthDate, source => source.MapFrom(src => src.BirthDate))
			.ForMember(dest => (EGender)dest.Gender, source => source.MapFrom(src => (short)src.Gender))
			.ForMember(dest => dest.HouseNumber, source => source.MapFrom(src => src.HouseNumber))
			.ForMember(dest => dest.NumOfChildren, source => source.MapFrom(src => src.NumberOfChildren))
			.ForMember(dest => dest.PartnerEmail, source => source.MapFrom(src => src.PartnerEmail))
			.ForMember(dest => dest.PhoneNumber, source => source.MapFrom(src => src.PhoneNumber))
			.ForMember(dest => dest.CityName, source => source.MapFrom(src => src.City))
			//.ForMember(dest => dest.PostalCode, source => source.MapFrom(src => src.))
			//.ForMember(dest => dest.WorkPlaceCity, source => source.MapFrom(src => src.))
			.ForMember(dest => dest.StreetName, source => source.MapFrom(src => src.Street));

			CreateMap<Nofshonit.Common.EF.Club.AllMembers, GetUserDTO>()
			.ForMember(dest => dest.Id, source => source.MapFrom(src => src.MemberId))
			.ForMember(dest => dest.FirstName, source => source.MapFrom(src => src.MemberFirstName))
			.ForMember(dest => dest.LastName, source => source.MapFrom(src => src.MemberLastName))
			.ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
			.ForMember(dest => dest.ApartmentNumber, source => source.MapFrom(src => src.ApartmentNumber))
			.ForMember(dest => dest.BirthDate, source => source.MapFrom(src => src.BirthDate))
			.ForMember(dest => (short)dest.Gender, source => source.MapFrom(src => (EGender)src.Gender))
			.ForMember(dest => dest.HouseNumber, source => source.MapFrom(src => src.HouseNumber))
			.ForMember(dest => dest.NumberOfChildren, source => source.MapFrom(src => src.NumOfChildren))
			.ForMember(dest => dest.PartnerEmail, source => source.MapFrom(src => src.PartnerEmail))
			.ForMember(dest => dest.PhoneNumber, source => source.MapFrom(src => src.PhoneNumber))
			.ForMember(dest => dest.City, source => source.MapFrom(src => src.CityName))
			//.ForMember(dest => dest.PostalCode, source => source.MapFrom(src => src.))
			//.ForMember(dest => dest.WorkPlaceCity, source => source.MapFrom(src => src.))
			.ForMember(dest => dest.Street, source => source.MapFrom(src => src.StreetName));

            CreateMap<UserDTO, RequestUserDTO>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdentityNumber, source => source.MapFrom(src => src.IdentityNumber))
                .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
                .ForMember(dest => dest.LoginType, source => source.MapFrom(src => src.LoginType))
                .ForMember(dest => dest.AccessID, source => source.MapFrom(src => src.AccessID));

            CreateMap<AuthenticateUserRequestDTO, UserDTO>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdentityNumber, source => source.MapFrom(src => src.IdentityNumber))
                .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
                .ForMember(dest => dest.LoginType, source => source.MapFrom(src => src.LoginType))
                .ForMember(dest => dest.AccessID, source => source.MapFrom(src => src.AccessID))
                .ForMember(dest => dest.Password, source => source.MapFrom(src => src.Password));

            CreateMap<UserDTO, AuthenticateUserRequestDTO>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdentityNumber, source => source.MapFrom(src => src.IdentityNumber))
                .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
                .ForMember(dest => dest.LoginType, source => source.MapFrom(src => src.LoginType))
                .ForMember(dest => dest.AccessID, source => source.MapFrom(src => src.AccessID))
                .ForMember(dest => dest.Password, source => source.MapFrom(src => src.Password));

            CreateMap<UpdateMamberUserDTO, UserDTO>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, source => source.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, source => source.MapFrom(src => src.LastName))
                .ForMember(dest => dest.BirthDate, source => source.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.IdentityNumber, source => source.MapFrom(src => src.IdentityNumber))
                .ForMember(dest => dest.MobilePhone, source => source.MapFrom(src => src.MobilePhone))
                .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CityName, source => source.MapFrom(src => src.CityName))
                .ForMember(dest => dest.StreetName, source => source.MapFrom(src => src.StreetName))
                .ForMember(dest => dest.HouseNumber, source => source.MapFrom(src => src.HouseNumber))
                .ForMember(dest => dest.ApartmentNumber, source => source.MapFrom(src => src.ApartmentNumber))
                .ForMember(dest => dest.Zip, source => source.MapFrom(src => src.Zip))
                .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
                .ForMember(dest => dest.FamilyStatus, source => source.MapFrom(src => src.FamilyStatus))
                .ForMember(dest => dest.NumOfChildren, source => source.MapFrom(src => src.NumOfChildren))
                .ForMember(dest => dest.Gender, source => source.MapFrom(src => src.Gender))
                .ForMember(dest => dest.PartnerEmail, source => source.MapFrom(src => src.PartnerEmail))
                .ForMember(dest => dest.LoginType, source => source.MapFrom(src => src.LoginType))
                .ForMember(dest => dest.AccessID, source => source.MapFrom(src => src.AccessID))
                .ForMember(dest => dest.CityId, source => source.MapFrom(src => src.CityId));

            CreateMap<AuthenticateUserRequestDTO, Nofshonit.Common.EF.Club.AllMembers>()
                .ForMember(dest => dest.Tz, source => source.MapFrom(src => src.IdentityNumber))
                .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
                .ForMember(dest => dest.AccessId, source => source.MapFrom(src => src.AccessID))
                .ForMember(dest => dest.EncryptedUserPassword, source => source.MapFrom(src => src.Password));

            CreateMap<Nofshonit.Common.EF.Club.AllMembers, AuthenticateUserRequestDTO>()
                .ForMember(dest => dest.IdentityNumber, source => source.MapFrom(src => src.Tz))
                .ForMember(dest => dest.Email, source => source.MapFrom(src => src.Email))
                .ForMember(dest => dest.AccessID, source => source.MapFrom(src => src.AccessId))
                .ForMember(dest => dest.Password, source => source.MapFrom(src => src.EncryptedUserPassword));

			CreateMap<LogDTO, ApiLogs>()
				.ForMember(dest => dest.Body, source => source.MapFrom(src => src.Body))
				.ForMember(dest => dest.EndDate, source => source.MapFrom(src => src.EndDate))
				.ForMember(dest => dest.Exception, source => source.MapFrom(src => src.Exception))
				.ForMember(dest => dest.MethodName, source => source.MapFrom(src => src.MethodName))
				.ForMember(dest => dest.MethodName, source => source.MapFrom(src => src.MethodName))
				.ForMember(dest => dest.OrganizationId, source => source.MapFrom(src => src.OrganizationId))
				.ForMember(dest => dest.QueryParams, source => source.MapFrom(src => src.QueryParams))
				.ForMember(dest => dest.Response, source => source.MapFrom(src => src.Response))
				.ForMember(dest => dest.StartDate, source => source.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.LogType, source => source.MapFrom(src => src.LogType));
		}

        public dynamic Map(object source, Type destType)
        {

            if (source != null && destType != null)
            {
                var ret = _mapper.Map(source, source.GetType(), destType);

                return ret;
            }
            return null;
        }
    }
}
