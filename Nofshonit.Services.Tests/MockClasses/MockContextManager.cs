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
using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Nofshonit.Services.Tests.MockClasses
{
	public class MockContextManager 
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

        public ClubContext ClubContext()
		{
			return new ClubContext("Server=172.29.92.20\\sql2005;Database=Histadrut;Trusted_Connection=True;user=sqlAdmin;password=Erg0110$;Persist Security Info=False;Integrated Security=false;");
		}

		public OrganizationDetailsDTO CurrentOrganization()
		{
			return new OrganizationDetailsDTO
			{
				OrganizationGuid = "C656566A-5D94-4DB4-85B1-3F729DCB9165                                                                                            ",
				OrgId = 48,
				DBName="Histadrut",
				ServiceMail="Test",
				OrgName="Histadrut",
			};
		}

		public ResponseUserDTO CurrentUser()
		{
			return new ResponseUserDTO
            {
                Token = "elUWUr6cin9svwE4fDLSkyAiemcvJ72LOn0QHCbbPygCItzTQoxhnnaGzipwvlJbAmrMIROtOt3dJwVL1fLhViu/9AiW3WTddkhvFCMD+ffn27p60j7OrPHbnBFInaD65NxM4UGOf97nMRwrG4fuyMTWfrlwN1DLinSxDbdZnelGj/W1UPNb/lOkbW2frTN8jj4i8/ULC+bG3pFr1qbq21LkzbaasodFb1XxMA4h4v+Wz0iYcKMOfZP+eXt4lLd2sHqUaSNEAMQC/+XkMcQRvLHCitGl7BUg3fH+QqeHflg5V7CACXd4AV0GtnaaWB/0F1mnn3eGxWX2QRk+xVCMFgM38pDge9nFBTUIj/r342s3LrmMhXYzOgXEXF6TGjx0qpg/4RIdT7yvgcTZbBRb8C/vZMSCEAC043xWrRgX4JFp93DAx76a4GPgevVoK0vZiKfXs3S2T1MldbPDsGx/CAhA4Y+bly2zFQMKrAzahFOiDKE9KT9/bwlGJ8ixJuqAQ6cYgcj84oc/iEeUGsuOIgJD4eL6Do45X5BK1jsHQyqUEczpv/RjFTp9bfKJbBocevNiXkA5veQP/1f4LQum4EaOmmHydGgj7kGjVUE64oXGDDekXg4QcgjOiKIstjWjJ3jhQCPSyY/7uqX6Y13souP7l+Ti+MWFB2z7MgbxyvI=",
                Id = "314327164",
				MemberGuid = "f6679123-2917-448d-8846-bab0afa6be4a",
                IdentityNumber = "314327164",
                PremiumType = 3,
            };
		}

        public ResponseUserDTO SetCurrentUserCache(string memberId)
        {
            var userCardNumber = ClubContext().Cards.FirstOrDefault(c => c.Idmember.Equals(memberId) && c.CardStatus == (int)ECardStatus.ACTIVE && c.CardType == (int)ECardType.VERIFONE).CardNumber;

            var hasPinCode = ClubContext().AllMembersProperties.FirstOrDefault(u => u.MemberId.Equals(memberId)) != null;

            var currentUserFromDb = ClubContext().AllMembers.Where(x => x.MemberId.Equals(memberId))
                      .Select(x => new ResponseUserDTO
                      {
                          Id = x.MemberId,
                          Token = x.UserToken,
                          FirstName = x.MemberFirstName,
                          LastName = x.MemberLastName,
                          MemberGuid = x.IdentityGuid,
                          CardNumber = userCardNumber,
                          MobilePhone = x.MobilePhone,
                          AccessID = x.AccessId,
                          PremiumType = x.PremiumType,
                          MemberSpecialID = x.MemberSpecialId,
                          Email = x.Email,
                          IdentityNumber = x.Tz,
                          CityName = x.CityName,
                          StreetName = x.StreetName,
                          BirthDate = x.BirthDate,
                          Gender = (EGender)(x.Gender ?? 0),
                          PartnerEmail = x.PartnerEmail,
                          PartnerName = x.PartnerName,
                          HasPinCode = hasPinCode && string.IsNullOrEmpty(x.PinCode),
                          PinCode = x.PinCode
                      }).FirstOrDefault();
            Cache.Set(string.Format(CacheKeys.UserId, memberId), currentUserFromDb);
            return currentUserFromDb;
        }

        public void ClearCurrentUserCache(string memberId)
        {
            Cache.Remove(string.Format(CacheKeys.UserId, memberId));
            Cache.Remove(string.Format(CacheKeys.UserTokenExp, memberId));
        }

        public void SetCurrentUserTokenLoginCache(string memberId, string token)
        {
            int expInMinutes = Container.Resolve<IConfigurationManager>().GetConfigByValue<int>(Common.ConfigurationKey.MinutesToExpired_UserToken);
            Cache.Set(string.Format(CacheKeys.UserTokenExp, memberId), token, TimeSpan.FromMinutes(expInMinutes));

        }

        public bool GetCurrentUserTokenLoginCache(string memberId, string token)
        {
            string cToken = (string)Cache.Get(string.Format(CacheKeys.UserTokenExp, memberId));
            return (cToken != null && cToken == token);

        }

        public void ClearTokenSession()
        {
            _httpContextAccessor.HttpContext.Session.Remove(CacheKeys.TokenSession);
        }

        public void SaveTokenSession(RefreshToken refreshToken)
        {
            _httpContextAccessor.HttpContext.Session.SetString(CacheKeys.TokenSession, JsonConvert.SerializeObject(refreshToken));
        }

        public RefreshToken GetTokenSession()
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(CacheKeys.TokenSession);
            return JsonConvert.DeserializeObject<RefreshToken>(value);
        }
    }
}
