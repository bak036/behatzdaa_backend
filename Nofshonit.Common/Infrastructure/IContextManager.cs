using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.EF.Club;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Infrastructure
{
    public interface IContextManager
    {
        ResponseUserDTO CurrentUser();
        OrganizationDetailsDTO CurrentOrganization();
        ClubContext ClubContext();
        ResponseUserDTO SetCurrentUserCache(string memberId);
        void ClearCurrentUserCache(string memberId);
        void ClearSessionByKey(string Key);
        void SaveShopingBasketPurchase(List<CartVarsDTO> ShopinBasket, string memberId);
        List<CartVarsDTO> GetShopingBasketPurchaseByMember(string memberId);
        void SetObjectInSession(string Key, object value);
        T GetObjectFromSession<T>(string key);
        List<AppConfig> GetAppConfig();
        string GetAccessToken();
    }
}