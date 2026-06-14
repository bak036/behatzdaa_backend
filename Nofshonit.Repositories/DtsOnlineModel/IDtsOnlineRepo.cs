using Microsoft.EntityFrameworkCore;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Business;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.DTOs.Coupon;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.Tags;
using Nofshonit.Common.EF.DTS_Online;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nofshonit.Repositories.DtsOnlineModel
{
    public interface IDtsOnlineRepo
    {
        Task<int> OrganizationIdByGuid(string guid);
        //Dictionary<int, string> GetRedimTypes();

        Task<bool> UpdateSubscriptionToDB(UpdateSubscriptionDTO updateSubscriptionDTO);

        #region Greetings 
        Task<List<GreetingDTO>> GetGreetings(int organizationId);
        Task<List<string>> GetGreetingTypes(int organizationId);
        Task<List<string>> GetMessagesByType(int organizationId, string type);
        #endregion

        #region Category 
        List<CategoryHeaderDTO> GetCategoryHeader(int organizationId);
        CategoryDetailsDTO GetCategoryDetails(long categoryNumber, int level, int organizationId);
        List<ImageSliderDTO> GetImagesSlider(int organizationId);
        bool IsEvents(long categoryId, int organizationId);
        bool IsEvents(List<long> categories, int organizationId);
        SimpleInt RedimTypeIdByCategoryId(long categoryId, int organizationId);
        Dictionary<int, string> RedimTypesByOrganization(int organizationId);
        List<TypeImplementationDate> GetImplementationTypesList();
        OrganizationCategories GetOrganizationCategory(long categoryNumber);

        #endregion
        #region Business 
        List<BusinessDTO> GetBusinessByIds(List<long> businessIdList);
        #endregion
        #region ContactUs 

        Task<bool> AddContactToCrmAsync(Crm userCrm);

        Task<bool> AddContactToEmailQueue(EmailQueue emailQueue);
        int AddContactToEmailQueueSync(EmailQueue emailQueue);

        Task<List<CrmGetTypeDTO>> GetCrmTypes();
        #endregion

        #region Addresses 
         List<StreetDTO> GetStreetsListById(int cityId);
        
         List<CityDTO>  GetCitiesList();
        
         List<RegionDTO> GetRegionsList();
        
        #endregion

        #region Coupons 
        CouponDiscountDTO GetCouponDiscount(string couponCode, decimal price);
        #endregion

        string GetOrganizationNews(int organizationId);

        Task<object> AddContactToSMSQueue(SmsQueue smsQueue);

        void AddPulseemLog(Pulseem pulseem);
        void AddSmsQueue(SmsQueue smsQueue);
        void DoCouponRollBack(long CouponStockId, string MemberId);

        #region Tags 
        TagsCategoriesDTO GetTagsById(int tagId);

        List<TagsCategoriesDTO> GetTagsByTop(int topVal,int skipTags = 0);
        #endregion
       

        List<BusinessSubTypeNameDTO> GetBussinessSubTypeNames();
        MwcSeries GetMwcSeriesBySerieId(int serieId);
        string GetCoupon(string couponID, string memberId);

        Dictionary<long, List<BusinessSubBranch>> BusinessSubBranches(List<long> businessIds);

        Task<bool> SlinkProcess(string generatedLinkCode);
        Task<bool> SlinkProcess_Join(DateTime? RegistrationDate = null);

        List<CategoryDetailsDTO> GetSearchData(string str, int selectTop, long category, string region);
        List<GetAutoCompleteResultsDTO> GetAutoCompleteData(string text, int selectTop);

            #region Stock 
            bool BudgetStockCategoriesCheck(long categoryId);
        (Stock, List<string>) GetStockVariantBarcodes(string barcode);
        int CouponStock(int cuponStockId);
        #endregion

        Task<bool> GetUserEmailSubscription(string id,string email);
        City GetCityByCityId(string cityId);
        bool CheckCartVariantBarCodeDtsStock(string barcode);
        bool ImportExistingMemberFromHonorRelease(string  tz );
        ConfirmationTemplates GetConfirmationTemplate(int orgId);
        List<CouponCancelRequest> GetCouponCancelRequestByMemberId(string memberId);
        T MwcMediasExecuteQuery<T>(Func<IQueryable<MwcMedia>, T> query);
        decimal GetGlobalSelfDischargeLimit(decimal defaultLimit = 1500m);
        decimal GetMemberMonthlySelfDischargeTotal(string cardNumber, DateTime fromDate, DateTime toDate);

        int GetFatherRedimTypeId(long benefitId);
        string GetRedimTypeNameById(int id);
        string GetFatherRedimTypeName(long benefitId);

        //REQ0143
        void SendVariantSMS(
            int variantType,
            int quantity,
            string posBarCode,
            int businessSubTypeId,
            string businessId,
            string firstName,
            string cardNumber,
            string shortNameVar,
            DateTime lastImplementationDate,
            string mobile,
            string orgId,
            string senderName,
            string memberId,
            int? smsQueueUsersId
            );
    }
}
