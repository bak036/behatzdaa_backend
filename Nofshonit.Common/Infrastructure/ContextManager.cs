using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Extensions;
using Nofshonit.Common.Utils;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils;
using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Nofshonit.Common.Infrastructure
{


    public class ContextManager : IContextManager
    {
        private ICustomContainer Container
        {
            get
            {
                return ContainerManager.Container;
            }
        }

        private ICacheManager Cache
        {
            get
            {
                return Container.Resolve<ICacheManager>();
            }
        }

        private IHttpContextAccessor _httpContextAccessor
        {
            get
            {
                return Container.Resolve<IHttpContextAccessor>();
            }
        }

        public ContextManager()
        {
            LoadOrganizations();
        }

        public OrganizationDetailsDTO CurrentOrganization()
        {
            var organizations = Cache.Get(CacheKeys.DTS_Organizations) as List<OrganizationDetailsDTO>;
            var organizationId = _httpContextAccessor.HttpContext.Request.Headers[HeadersKeys.OrganizationId].FirstOrDefault();
            var organizationGuid = _httpContextAccessor.HttpContext.Request.Headers[HeadersKeys.OrganizationGuid].FirstOrDefault();
            var org = organizations.FirstOrDefault(o => o.OrgId.ToString() == organizationId || o.OrganizationGuid.GetTrim() == organizationGuid.GetTrim());
            return org;
        }

        public string GetAccessToken()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies[CookiesKeys.AccessToken]?.Replace("Bearer ", string.Empty);
            if (string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[HeadersKeys.Token].FirstOrDefault()))
                accessToken = _httpContextAccessor.HttpContext.Request.Headers[HeadersKeys.Token].FirstOrDefault().Replace("Bearer ", string.Empty);
            return accessToken;
        }

        public ResponseUserDTO CurrentUser()
        {
            var accessToken = this.GetAccessToken(); 
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new Exception("Token missing");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(accessToken);
            string memberId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(memberId))
            {
                return null;
            }  

            var user = Cache.Get(string.Format(CacheKeys.UserId, memberId));
            if (user == null)
            {
                var memberProp = ClubContext().AllMembersProperties.FirstOrDefault(x => x.MemberId.Equals(memberId));
                var userCard = ClubContext().Cards.Where(c => c.Idmember.Equals(memberId) && c.CardStatus == (int)ECardStatus.ACTIVE && c.CardType == (int)ECardType.VERIFONE )
                                                  .OrderBy(x => x.CardStatus).FirstOrDefault();

                var userCardNumber = userCard != null ? userCard.CardNumber : "";

                var currentUserFromDb = ClubContext().AllMembers.Where(x => x.MemberId.Equals(memberId))
                    .Select(x => new ResponseUserDTO
                    {
                        Id = x.MemberId.GetTrim(),
                        FirstName = x.MemberFirstName.GetTrim(),
                        LastName = x.MemberLastName.GetTrim(),
                        MemberGuid = x.IdentityGuid.GetTrim(),
                        CreditCardClub=x.ClubCreditCard.ToString(),
                        CardNumber = userCardNumber.GetTrim(),
                        IdentityNumber = x.Tz.GetTrim().PadLeft(9, '0'),
                        Email = x.Email.GetTrim(),
                        MobilePhone = x.MobilePhone.GetTrim(),
                        AccessID = x.AccessId.GetTrim(),
                        PremiumType = x.PremiumType,
                        MemberSpecialID = x.MemberSpecialId.GetTrim(),
                        BirthDate = x.BirthDate,
                        CityName = x.CityName.GetTrim(),
                        CityId = x.City,
                        StreetName = x.StreetName.GetTrim(),
                        Gender = (EGender)(x.Gender ?? 0),
                        PartnerEmail = x.PartnerEmail.GetTrim(),
                        PartnerName = x.PartnerName.GetTrim(),
                        PartnerPhone = x.PartnerPhone.GetTrim(),
                        HasPinCode = !string.IsNullOrEmpty(x.PinCode.GetTrim()) && memberProp != null,
                        PinCode = x.PinCode.GetTrim(),
                        ApartmentNumber = x.ApartmentNumber.GetTrim(),
                        FactorySymbol = x.FactorySymbol.GetTrim(),
                        HouseNumber = x.HouseNumber.GetTrim(),
                        NumOfChildren = x.NumOfChildren,
                        Zip = x.Zip.GetTrim(),
                        LastUpdateMember = x.LastUpdateMember,
                        LastUpdate = x.LastUpdate,
                        ClubCreditCard = x.ClubCreditCard,
                        AllowSmsAndEmail = x.AllowSmsAndMail,
                        Entrance = x.Entrance,
                        Mailbox = x.Mailbox,
                        Address = x.Address,
                        UpdateOrRegister = (x.Email == null || (x.MobilePhone == null && x.PhoneNumber == null)) ? (x.LastUpdateMember == null ? ENeedUpdateOrFinishRegistration.FinishRegistration
                        : ENeedUpdateOrFinishRegistration.NeedUpdate) : (ENeedUpdateOrFinishRegistration?)null,
                        CookiesAcceptedDate = x.CookiesAcceptedDate
                    }).FirstOrDefault();
                if (memberProp != null)
                {
                    currentUserFromDb.CreditCardClub = memberProp.CreditCardClub;
                    currentUserFromDb.CVV = memberProp.CardCode;
                }

                if (currentUserFromDb.PremiumType.HasValue && currentUserFromDb.PremiumType.Value ==(int) EIDFPremiumeType.NotApproved)
                {
                        currentUserFromDb.UpdateOrRegister = ENeedUpdateOrFinishRegistration.UserExpired;
                }
                Cache.Set(string.Format(CacheKeys.UserId, memberId), currentUserFromDb);
                _httpContextAccessor.HttpContext.Items["CurrentUser"] = currentUserFromDb;
                return currentUserFromDb;
            }
            var reponseUser = (ResponseUserDTO)user;
            return reponseUser;
        }

        public ResponseUserDTO SetCurrentUserCache(string memberId)
        {
            string userCardNumber = null;
            var card = ClubContext().Cards.Where(c => c.Idmember.Equals(memberId) && c.CardStatus == (int)ECardStatus.ACTIVE && c.CardType == (int)ECardType.VERIFONE).OrderBy(x => x.CardStatus).FirstOrDefault();
            if (card != null)
                userCardNumber = card.CardNumber;


            var membersProperties = ClubContext().AllMembersProperties.FirstOrDefault(u => u.MemberId.Equals(memberId));
            // var hasPinCode = membersProperties != null;
            //var creditCardClub = currentUserFromDb.CreditCardClub = memberProp.CreditCardClub;


            var test = ClubContext().AllMembers.Where(x => x.MemberId.Equals(memberId)).ToList();
            var currentUserFromDb = test
                      .Select(x => new ResponseUserDTO
                      {
                          Id = x.MemberId.GetTrim(),
                          FirstName = x.MemberFirstName.GetTrim(),
                          LastName = x.MemberLastName.GetTrim(),
                          MemberGuid = x.IdentityGuid.GetTrim(),
                          CardNumber = userCardNumber.GetTrim(),
                          IdentityNumber = x.Tz.GetTrim().PadLeft(9, '0'),
                          Email = x.Email.GetTrim(),
                          MobilePhone = x.MobilePhone.GetTrim(),
                          AccessID = x.AccessId.GetTrim(),
                          PremiumType = x.PremiumType,
                          MemberSpecialID = x.MemberSpecialId.GetTrim(),
                          BirthDate = x.BirthDate,
                          CityName = x.CityName.GetTrim(),
                          CityId = x.City,
                          StreetName = x.StreetName.GetTrim(),
                          Gender = (EGender)(x.Gender ?? 0),
                          PartnerEmail = x.PartnerEmail.GetTrim(),
                          PartnerName = x.PartnerName.GetTrim(),
                          HasPinCode = !string.IsNullOrEmpty(x.PinCode.GetTrim()) && membersProperties != null,
                          PinCode = x.PinCode.GetTrim(),
                          ApartmentNumber = x.ApartmentNumber.GetTrim(),
                          FactorySymbol = x.FactorySymbol.GetTrim(),
                          HouseNumber = x.HouseNumber.GetTrim(),
                          PartnerPhone = x.PartnerPhone,
                          NumOfChildren = x.NumOfChildren,
                          Zip = x.Zip.GetTrim(),
                          LastUpdateMember = x.LastUpdateMember,
                          LastUpdate = x.LastUpdate,
                          ClubCreditCard = x.ClubCreditCard,
                          Entrance = x.Entrance,
                          Mailbox = x.Mailbox,
                          UpdateOrRegister = (x.Email == null || (x.MobilePhone == null && x.PhoneNumber == null)) ? (x.LastUpdateMember == null ? ENeedUpdateOrFinishRegistration.FinishRegistration
                                : ENeedUpdateOrFinishRegistration.NeedUpdate) : (ENeedUpdateOrFinishRegistration?)null,

                          AllowSmsAndEmail = x.AllowSmsAndMail,
                          CookiesAcceptedDate = x.CookiesAcceptedDate
                      }).FirstOrDefault();
            if (membersProperties != null)
            {
                currentUserFromDb.CreditCardClub = membersProperties.CreditCardClub;
                currentUserFromDb.CVV = membersProperties.CardCode;
            }

            if (currentUserFromDb.PremiumType.HasValue && currentUserFromDb.PremiumType.Value == (int)EIDFPremiumeType.NotApproved)
            {
                    currentUserFromDb.UpdateOrRegister = ENeedUpdateOrFinishRegistration.UserExpired;
            }

            Cache.Set(string.Format(CacheKeys.UserId, memberId), currentUserFromDb);
            return currentUserFromDb;
        }
        public void ClearCurrentUserCache(string memberId )
        { 
            Cache.Remove(string.Format(CacheKeys.UserId, memberId));
        }

        public void ClearSessionByKey(string Key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(Key);
        }

        public void SetObjectInSession(string Key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(Key, JsonConvert.SerializeObject(value));
        }
        public T GetObjectFromSession<T>(string key)
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public void SaveShopingBasketPurchase(List<CartVarsDTO> ShopinBasket, string memberId)
        {
            string minuteToAddShopingBasketToCach = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.minuteToAddShopingBasketToCach);
            int minuteToAdd = 0;
            int.TryParse(minuteToAddShopingBasketToCach, out minuteToAdd);
            Cache.Set(string.Format(CacheKeys.ShopingBasketPurchase, memberId), ShopinBasket, TimeSpan.FromMinutes(minuteToAdd));
        }
        public List<CartVarsDTO> GetShopingBasketPurchaseByMember(string memberId)
        {
            var shopinBasket = Cache.Get(string.Format(CacheKeys.ShopingBasketPurchase, memberId)) as List<CartVarsDTO>;
            return shopinBasket;
        }

        private void LoadOrganizations()
        {
            if (Cache.Exists(CacheKeys.DTS_Organizations))
                return;

            var dtsOnlineContext = new DTS_OnlineContext();
            var organizations = dtsOnlineContext.Organizations.Select(o => new OrganizationDetailsDTO
            {
                DBName = o.Dbname,
                IsNewSubsidy = o.IsNewSubsidy,
                OrganizationGuid = o.OrganizationGuid,
                OrgId = o.OrganizationId,
                OrgName = o.OrganizationName,
                Password = o.Password,
                ServiceMail = o.ServiceMail
            }).ToList();

            Cache.Set(CacheKeys.DTS_Organizations, organizations);
        }



        public ClubContext ClubContext()
        {
            OrganizationDetailsDTO org = null;
            var organizations = Cache.Get(CacheKeys.DTS_Organizations) as List<OrganizationDetailsDTO>;
            var organizationId = _httpContextAccessor.HttpContext.Request.Headers[HeadersKeys.OrganizationId].FirstOrDefault();
            var organizationGuid = _httpContextAccessor.HttpContext.Request.Headers[HeadersKeys.OrganizationGuid].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(organizationId) && string.IsNullOrWhiteSpace(organizationGuid))
                throw new ApplicationException("Organization Header is mandatory.");

            if (!string.IsNullOrWhiteSpace(organizationId))
                org = organizations.FirstOrDefault(o => o.OrgId == Int32.Parse(organizationId));
            else if (!string.IsNullOrWhiteSpace(organizationGuid))
                org = organizations.FirstOrDefault(o => o.OrganizationGuid.Trim() == organizationGuid.Trim());

            if (org == null)
                throw new ApplicationException("Failed to find Organization. " + organizationId);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            var dbName = org.DBName;
            var orgConnStr = configuration.GetConnectionString("ClubContext");
            var clubConnStr = string.Format(orgConnStr, dbName);

            return new ClubContext(clubConnStr);
        }
        public List<AppConfig> GetAppConfig()
        {
            var appConfig = Cache.Get("appConfig");
            if (appConfig == null)
            {
                List<AppConfig> appConfigsFromDB = ClubContext().AppConfig.ToList();
                Cache.Set("appConfig", appConfigsFromDB);
                _httpContextAccessor.HttpContext.Items["appConfig"] = appConfigsFromDB;
                return appConfigsFromDB;
            }
            var reponseAppConfig = (List<AppConfig>)appConfig;
            return reponseAppConfig;
        }
        private void LoadMember()
        {
            // Get club from Header
            // Add all members to cace
        }


    }
}