using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nofshonit.Repositories.ClubModel
{
    public interface IClubRepo
    {
        Task<List<AppConfig>> getValueByKeyList(List<string> keyList);
        bool MemberExistInExcludeCretitCard(string memberId);

        #region Users
        Task<List<Nofshonit.Common.EF.Club.AllMembers>> GetAllMembers(int top = 100);

        Task<bool> UpdateUserToken(string token);

        Task<bool> UpdateUserToken(string memberId, string token);

        string UpdateUserTokenSync(string memberId, string token);
        string GetOrCreateShortUrl(string longURL, string memberId);
        Task<bool> UpdateUserAccessID(ELoginType loginType);
        Task<string> GetDtsOrderGuidByFoxOrderGuid(string orderGuid);
        Task<bool> UpdateLastLogin(Common.EF.Club.AllMembers userFromDb);

        void AddMemberToAllowanceHistory(AllowanceHistory allowanceHistory);
        Task<bool> UpdateClubCreditCard(Common.EF.Club.AllMembers userFromDb, int ClubCreditCard = 1);

        Task<bool> ShouldAddCard();
        Task<bool> ShouldAddCard(string memberId);
        bool IsCancelRequriedAproove(string barCode);
        Task<Common.EF.Club.AllMembers> GetUserByUserDTO(UserDTO userDto);

        Task<Common.EF.Club.AllMembers> GetUserByUserID(string Id);

        Task<bool> UpdateMemberCookiesAcceptedDate(string Id);

        Common.EF.Club.AllMembers GetUserByUserIDSync(string Id);

        Task<Common.EF.Club.AllMembers> GetUserByUserToken(string token);

        Task<bool> CreateNewUser(Common.EF.Club.AllMembers userEF);
        Task<Common.EF.Club.AllMembers> SetIdfDataForMember(IDFValidationResponseDTO member);
        Task<bool> UpdateNewUser(Common.EF.Club.AllMembers userEF, Common.EF.Club.AllMembers updatedUser);

        Task<bool> UpdateMember(UserDTO userEF);

        string GetNextSequenceValue();

        Task<bool> UpdatePassword(UserPasswordInfoDTO userPasswordInfoDto);

        Task<Common.EF.Club.AllMembers> GetUserByEmailAsync(string email);

        Task<bool> IsMemberAllowedLogin();
        Task<bool> IsMemberAllowedRegister(string Id);
        Task<Common.EF.Club.AllMembers> IsMemberAllowedLogin(string Id);
        List<string> GetAllMembersAllowedLogin();
        Task<List<MembersAllowedLogin>> GetMemberAllowedRegisterFromCache();
        MembersAllowedLogin GetMemberFromAllowedRegister(string memberId);


        Task<bool> UpdateMemberStatusAndMemberSpecialID(int? premiumType, string memberSpecialID);

        int UpdateForRecoverPasswordAsync(UserDTO userDto);

        bool CheckIfAbleToTryLogin(Common.EF.Club.AllMembers userFromDb);
        Task<bool> UpdateLoginAttempts(bool nullify, Common.EF.Club.AllMembers userDTO);

        (string, DateTime?) ValidateRecoverPasswordToken(UserDTO userDto, string tokenCode);


        (bool, string) ValidateTokenForMember(string memberId, string token, DateTime expiredDate);

        Task<bool> UpdateLastUpdate(string memberId, DateTime? newDate, int? premiumType = null);
        Task<bool> UpdateLastUpdate(string memberId, DateTime? newDate, int? premiumType = null, int? darga = null);

        Task<Common.EF.Club.AllMembers> GetUserByTokenCode(string tokenCode);

        Task<AllMembersProperties> GetMembersPropertiesById(string memberId);

        bool AddCardToMember(string memberId, bool isPhysical);
        long AddCard(Common.EF.Club.Cards card);
        bool UpdateCardSkeleton(Common.EF.Club.CardsSkeleton cardSkeleton);
        #endregion

        #region Product
        List<VariantDTO> GetVariantDTOs(long categoryId, string memberId, bool orgIsNewSubsidy, SimpleInt categoryRedimType, Dictionary<int, string> RedimTypesByOrganization, List<Common.EF.DTS_Online.TypeImplementationDate> implementationTypesList);

        long CategoryIdByEventId(int eventId);
        long CategoryIdByVariant(string barcode);
        bool ImplementNewCardVariant(bool isDigital = false);
        #endregion

        #region Shoping Basket
        ProductsVars GetVariant(string barcode);
        Common.EF.Club.CardsSkeleton GetCardSkeleton(string serieId);
        List<ProductsVars> GetVariantsByCategoryId(long categoryId);

        Task<bool> UpdateProduct(ShopingBasket productFromDb);
        List<ShopingBasket> GetCart(string memberId);
        void SyncShoppingBasket(string memberId, List<long> productsToRemove);
        ShopingBasket GetProductByBarcode(string productBarcode);

        List<ShopingBasket> GetProductsByCategoryNumber(string categoryNumber);
        List<ShopingBasket> GetAllProducts();
        List<ShopingBasket> GetAllProductsByMemberId(string MemberId);
        bool AddProduct(ShopingBasket productFromDb);
        bool RemoveProduct(string productBarcode);
        bool RemoveProductsByCategoryNumber(string categoryNumber);
        bool RemoveAllProducts();
        bool RemoveAllProductsByMemberId(string MemberId);
        List<ShopingBasket> UpdateShoppingCartPricesGetCart(string id, int creditCardType);
        #endregion

        #region Cards

        long AddRequestRow(int status, string cardNumber, string cardNumber2, string remark, int type, long originalRequestId, decimal amount = -1, int walletID = 0);
        void UpdateRequestRow(int status, string cardNumber, string cardNumber2, decimal amount, string remark, long originalRequestId, long requestId, string xmlParam = "", int walletId = 0, long paymentId = 0);
        short GetCardCVV(string v);
        Task<long> AddVerifoneLog(decimal Amount, string Card, int wallet, string Type);
        CardInfoDTO GetCardActivationAndExpiryDate(string cardNumber);
        bool IsQuickLoadAllowdToSavedCC();
        int CheckNewCardRequestIsPossible(string memberID);
        bool BlockCard(string memberID);
        LoadWalletToFuncDto LoadWallet(string cardNumber, string walletId);
        void UpdateRequestLoadWallet(long reqId, long? paymentId, int status, string exception);
        void UpdateOrAddToAllMembersProperties(ExtandPeyerDataDTO epd, string cardId, string memberId);
        List<WalletData> GetWallets(string xmlString, string cardNumber, string memberId);
        long InsertPayment(string memberId, decimal amount, string xml, string cardOwnerId, string last4digits, string serverTransactionId, byte creditCardStatus = 0);
        int GetAmountToWallet(string walletId);
        bool ActiveateCard(string memberID);
        void SavePinCode(string pinCode, ResponseUserDTO currentUser);
        void SetMaxCardClub(string clubId, ResponseUserDTO currentUser);
        int GetMaxDepositForMonth(string walletID);
        decimal Get14DaysBalance(string cardNumber, int walletId);
        decimal GetGlobalSelfDischargeLimit();
        Task<decimal> GetMemberMonthlySelfDischargeTotal(string cardNumber);


        (string type, decimal discountRate) GetWalletRefundInfo(int walletId);


        #endregion
        List<(long categoryNumber, string categoryName, int sortIndex)> GetSearchData(string str, int selectTop, long category, string script);
        List<(long categoryNumber, string categoryName, int sortIndex)> GetSearchAutoComplete(string str, int selectTop);
        List<(long categoryNumber, string categoryName, int sortIndex)> FullTextSearchLogic(string search, string strForContains, string strForUnion);
        bool DoVariantsRollBack(Common.EF.Club.Orders order, List<AtractionsOrders> attractionsOrders);
        List<AtractionsOrders> GetAtractionsOrdersByOrderId(int orderId);
        CouponsStockAtrorders GetCouponsStock(long MemberOrderAsmachta);
        void RemoveCouponsStockAtrOrders(CouponsStockAtrorders coupon);
        bool IsCardVariant(long categoryId);
        string GetOldCardByIdentity(string memberId);
        string GetActiveCardByIdentity(string memberId);
        CardInfoVerifone GetCardInfoForVerifoneActions(string cardNumber);
        long AddMemberAdress(MemberAddress memberAddress);
        MemberAddress GetMemberAddress(Common.EF.Club.AllMembers member, string ApartmentNumber);
        bool UpdateMemberAddress(MemberAddress memberAddress);
        bool UpdateAddressForAtractionsOrders(string memberId, string productBarCode, string orderId, long memberAddressId);
        List<v_wAllOrders> GetAllOrdersByOrderId(int orderId);

        #region Limitations
        GroupLimitationDTO GetGroupLimitations(long groupLimitsId);
        ProductsVars GetProductsVar(string barcode);
        List<ProductsVars> GetProductsVar(List<string> barcode);
        List<WebServiceTransactionDTO> GetMemberTransactions(string MemberId, bool enableBusinessSubTypeLimitations = false, bool isIgnoreBusinessSubType = false, bool isGetMemberCancelledTransactions = true);
        List<ShopingBasket> GetShopingBasketByMemberId(string memberId, string barcode);
        List<WebServiceTransactionDTO> GetVariantTransactions(string barCode);
        List<BusinessSubTypeDTO> GetBussinessSubType(List<BusinessSubTypeNameDTO> bussTypeNames, bool isVariantLimit = false);

        List<BusinessSubTypeSpecificationCurrent> GetAllBusinessSubTypes();
        int OrderQtyByBarcodes(List<string> barcodes);
        #endregion

        #region Orders
        List<WebServiceTransactionDTO> GetOrderTransactionsByOrderGuid(string orderGuid);
        Orders GetOrderByGuid(string guid);

        Task<string> GetConfirmationPageByOrderGuid(string guid);


        #endregion
        Task<List<WalletTagChainsData>> GetChainsByWallet(string walletId);
        Task<List<WalletChainBranches>> GetBranchesByWalletAndChain(string walletId, string chainId);

        #region Sms Login
        List<MemberCodes> GetSmsCodes(string memberId);
        void SaveSmsCodeForLogin(string memberId, string smsCode);
        void DeleteSmsCodesByMemberId(string memberId);
        void InsertPaymentRow(decimal amount, string cardNumber, string serverTransactionId);
        long InsertPaymentRow(decimal amount, string cardNumber, string last4Digit, string serverTransactionId, string currentUserId, string currentUserIdentityNumber, string xml);
        Task<Common.EF.Club.Requests> GetRequest(string walletId, string memberID);
        #endregion
        Task<BaseResponse<bool>> UpdateBiometricToken(BiometicsInfoDTO biometicsInfoDTO);
        string getPublicKeyById(Common.EF.Club.AllMembers userFromDb);

        Task<bool> UpdateMemberActive(string memberId, bool active);
        bool UpdateCreateOrderByEvent(int dtsOrderId, int ticktsHubOrderId, List<TicketsHubRepository.EF.TicketsHub.OrderTickets> orderTickets, string eventGuid);
        Orders GetOrderByOrderId(int orderId);
        string GetCardNumByMemberId(string memberId);
        DateTime? GetRegisrationDateByMember(string memberId);

        bool CheckAccountTransactions(string memberId, int years);
        CouponsStocksDetails GetCouponsStockType(int StockType);
        Task<bool> CreateNewRegistrationAudit(RegistrationAudits regAudit);
        bool UpdateNewRegistrationAudit(RegistrationAudits regAudit);

        Task<List<Common.EF.Club.RegistrationAudits>> GetAllRegistrationAudits(int top = 500);

        Task<Common.EF.Club.RegistrationAudits> GetRegistrationAuditByIdentity(string identityNumber);
        bool IsMemebrInBehatzdaaNoCards(string memberId);

        Task<bool> UpdateJoinByOtpAttempts(bool nullify, RegistrationAudits regAudit);

        Task<bool> CanSendOtp(RegistrationAudits regAudit);
        List<ProductsVars> GetProductsVars(long benefitID);
        List<BusinessSubTypeSpecificationCurrent> GetBusinessSubTypeSpecificationCurrents(List<ProductsVars> products);
        int GetSumOfTrnasactionsByBarcode(string barcode);
        List<BusinessSubTypeSpecificationByVariants> GetBusinessSubTypeSpecificationByVariants(List<ProductsVars> products);
        bool IsEventimBenefitByCategoryNumber(long categoryNumber);
        bool IsEventimBenefitByBarcode(string barcode);
        List<AllOrdersHistory> GetHistoryForMember(MemberHistoryDTO memberHistoryDTO);
        List<v_wAllOrders> GetAllOrdersForMember(MemberHistoryDTO memberHistoryDTO);
        AtractionsOrders GetAttractionsOrder(long orderAsmchta);
        bool isHaveTsarchanutVariant(List<string> barcodes);
        bool CheckAllBenefitIdExists(List<long> benefitIds);
        bool CheckAvailableGiftCard(int SerieId);
        Orders InsertNewOrder(Orders order);
        MemberAddress GetMemberAddressForTzarchanut(PurchaseRequestDTO purchaseRequestDTO);
        MemberAddress GetMemberAdressForRegularBarCodes(PurchaseRequestDTO purchaseRequestDTO);
        string GetCardByMemberId(string MemberId);
        Common.EF.Club.Payments GetPaymentRowByPaymentId(long paymentId);
        AtractionsOrders GetAttractionsOrderByAsmachta(long orderAsmchta);
        AtractionsOrders GetAttractionsOrderByUniqueOrderIdentity(long uniqueOrderIdentity);

        List<AtractionsOrders> GetAttractionsOrderByOrderId(int orderId);
        void InsertInvoiceManagementRequests(string request, int orgId);
        Common.EF.Club.WalletLoadMoney GetWalletById(string id);
        Task<string> GenerateUrl(string longurl);
        Task<BarcodePopupDetails> GetPopupBarcodeDetails(string asmachta);
        bool CreateUpdateUserRequestLog(UserDTO oldUser, UserDTO newUser, bool isUpdateDetailsApproved);
        bool CreateNewUserRequestLog(string identityNumber, bool isUpdateDetailsApproved);
        bool CreateLogAfterLoginBlockUser(string identityNumber);
        Task<List<WebServiceTransaction>> GetWebServiceTransactionsByMemberId(string memberId);
        Task<List<WalletTagChainsData>> GetWalletChainNearMeFromDb(string walletId, decimal lat, decimal lon);
        Task<List<WalletChainBranches>> GetWalletChainBranchesNearMe(string walletId, string chainId, decimal lat, decimal lon);
    }
}