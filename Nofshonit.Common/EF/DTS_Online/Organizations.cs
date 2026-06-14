using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Organizations
    {
        public Organizations()
        {
            ImagesTypesToOrganizations = new HashSet<ImagesTypesToOrganizations>();
            MwcSeries = new HashSet<MwcSeries>();
            OrganizationCategoriesImages = new HashSet<OrganizationCategoriesImages>();
            OrganizationRedimTypes = new HashSet<OrganizationRedimTypes>();
            PricingBasePriceCommision = new HashSet<PricingBasePriceCommision>();
            ShortCardNumbers = new HashSet<ShortCardNumbers>();
            Tags = new HashSet<Tags>();
        }

        public int OrganizationId { get; set; }
        public int? OrganizationType { get; set; }
        public short? MainField { get; set; }
        public int? MainFieldLength { get; set; }
        public int? MainFieldMinLength { get; set; }
        public byte MainFieldType { get; set; }
        public bool? MainFieldLeadingZeros { get; set; }
        public string OrganizationName { get; set; }
        public byte OrganizationStatusId { get; set; }
        public string Dbname { get; set; }
        public string WebSite { get; set; }
        public string OrganizationAddress { get; set; }
        public string EndUserSupportPhone { get; set; }
        public string ManagerName { get; set; }
        public string ManagerTitle { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerMobile { get; set; }
        public string Email { get; set; }
        public bool? SendSms { get; set; }
        public bool? SendEmail { get; set; }
        public bool? SendEmailForRedRequests { get; set; }
        public string LeumiCardPrefix { get; set; }
        public string LeumiCardMediaFile { get; set; }
        public string OrganizationParamsXml { get; set; }
        public string Remark { get; set; }
        public string PicturePath { get; set; }
        public string CardPicturePath { get; set; }
        public string ActivationSupportPhone { get; set; }
        public byte ActivationParam1 { get; set; }
        public byte ActivationParam2 { get; set; }
        public double? Discount { get; set; }
        public short? DiscountType { get; set; }
        public decimal? MinPurchase { get; set; }
        public decimal? MaxPurchase { get; set; }
        public decimal? MaxOtb { get; set; }
        public byte? HamaraKanfeyMeshek { get; set; }
        public byte? HamaraCheck { get; set; }
        public string RemarkFromDts { get; set; }
        public string OrganizationTableXml { get; set; }
        public string Password { get; set; }
        public byte? UserPortalLoginType { get; set; }
        public string OrganizationNews { get; set; }
        public string OrganizationSales { get; set; }
        public long? MerchantId { get; set; }
        public int? LimitsIp { get; set; }
        public string SmsMobile { get; set; }
        public string RegistrationXml { get; set; }
        public DateTime? FirstActivity { get; set; }
        public int? OrderLimitTransaction { get; set; }
        public int? OrderLimitDayly { get; set; }
        public int? OrderLimitMonthly { get; set; }
        public int? OrderLimitQuortely { get; set; }
        public int? OrderLimitYearly { get; set; }
        public int? OrdelLimit { get; set; }
        public int? OrderLimitTransactionMakat { get; set; }
        public int? OrderLimitDaylyMakat { get; set; }
        public int? OrderLimitMonthlyMakat { get; set; }
        public int? OrderLimitQuortelyMakat { get; set; }
        public int? OrderLimitYearlyMakat { get; set; }
        public int? OrdelLimitMakat { get; set; }
        public bool IdentityOnOrders { get; set; }
        public string OrganizationIp { get; set; }
        public string Token { get; set; }
        public DateTime? TokenCreatedTime { get; set; }
        public int? OpId { get; set; }
        public string GlobalSms { get; set; }
        public string MemberSms { get; set; }
        public bool SearchInBusinessReportWeb { get; set; }
        public bool ToBoundPoints { get; set; }
        public string PicforPrinting { get; set; }
        public string TradeWebSiteUrl { get; set; }
        public bool? AllowComent { get; set; }
        public bool? Allowsurvey { get; set; }
        public bool? AllowApplicationHistory { get; set; }
        public string DefaultPrintPattern { get; set; }
        public string DefaultPrintPatternShow { get; set; }
        public string DefaultVarPriceFormula { get; set; }
        public string DefaultVarDiscountFormula { get; set; }
        public string DefaultVarComissionFormula { get; set; }
        public bool CheckActiveRavChen { get; set; }
        public bool AllowSoGood { get; set; }
        public bool? ActiveCardNotNeeded { get; set; }
        public bool? TradeSiteSmsconfirmation { get; set; }
        public bool? TradeSiteChargeCreditCard { get; set; }
        public string TradeSitePaymentTerminalNumber { get; set; }
        public string PaymentTerminalUserName { get; set; }
        public string PaymentTerminalPassword { get; set; }
        public DateTime? LastHanpakaDate { get; set; }
        public string InfoMail { get; set; }
        public string ServiceMail { get; set; }
        public string GoogleAnalytics { get; set; }
        public string ClubTitle { get; set; }
        public bool? CancelCommission { get; set; }
        public decimal? OrganizationCommission { get; set; }
        public decimal? OperatorCommission { get; set; }
        public string GoogleAnalyticsLogin { get; set; }
        public bool ChargeCommission { get; set; }
        public bool UsePoints { get; set; }
        public bool? UseOnlyOrganizationCard { get; set; }
        public bool? Allowinstallments { get; set; }
        public bool? ShowFullCard { get; set; }
        public bool? ShowUniqeCampValue { get; set; }
        public bool? IsEncodedCards { get; set; }
        public bool? CheckMethodsPremission { get; set; }
        public bool? CleanShopingBasket { get; set; }
        public bool? AllowAnonymousTransactions { get; set; }
        public bool? DynamicallyAddMember { get; set; }
        public bool? DynamicallyAddCard { get; set; }
        public bool? ForceLoginWhenBuy { get; set; }
        public bool? AllowCampaignReset { get; set; }
        public byte MoneyType { get; set; }
        public bool? AllowOnLineCreditLoading { get; set; }
        public bool? AllowMoneyLoadingManually { get; set; }
        public bool? AllowCardActivation { get; set; }
        public bool? AllowCardBlock { get; set; }
        public string NetworkIp { get; set; }
        public string Theme { get; set; }
        public string VirtualName { get; set; }
        public bool? VirtualMoney { get; set; }
        public bool? CancelOrder { get; set; }
        public string LeumiCardMediaFileNewVersion { get; set; }
        public bool? OrgSafeCard { get; set; }
        public string LoadMoneyTerminalNumber { get; set; }
        public string OrganizationShortName { get; set; }
        public int? OrgChargeType { get; set; }
        public bool? IsGeneric { get; set; }
        public string PaymentTerminalNumberConsumerism { get; set; }
        public short? IssuerId { get; set; }
        public bool? AllowReport { get; set; }
        public bool? IsTradeSite { get; set; }
        public bool? WebItubusinessReport { get; set; }
        public bool? MultiVariant { get; set; }
        public bool? ExecSiteReport { get; set; }
        public bool? UseDtsgateway { get; set; }
        public bool? IsCommissionByProduct { get; set; }
        public int? TrustProgramCommission { get; set; }
        public int? ShowCommission { get; set; }
        public int? OtherCommission { get; set; }
        public int? SoGoodRalizationMethodId { get; set; }
        public int? SoGoodOrderBy { get; set; }
        public decimal? MaxSumInCard { get; set; }
        public bool? AllowNloginCardBalance { get; set; }
        public decimal? MaxSumInCardForMonth { get; set; }
        public bool? AllowedCardNumberByTz { get; set; }
        public string OrganizationGuid { get; set; }
        public bool SendSmsafterPayment { get; set; }
        public string SendSmsmessage { get; set; }
        public string SendSmscancelMessage { get; set; }
        public bool IsOrdersTable { get; set; }
        public int? DefaultIssuer { get; set; }
        public int? CategoryNameMaxLength { get; set; }
        public int? CancelingCommission { get; set; }
        public int? CancellingCommisionTopAmount { get; set; }
        public int? DaysForRefound { get; set; }
        public bool? ForceShortMarketingDescription { get; set; }
        public bool IsSupportShortCardNumber { get; set; }
        public DateTime ShortCardNumberStartDate { get; set; }
        public bool IsNewSubsidy { get; set; }
        public int? RecordId { get; set; }
        public string SignaturePic { get; set; }

        public virtual ICollection<ImagesTypesToOrganizations> ImagesTypesToOrganizations { get; set; }
        public virtual ICollection<MwcSeries> MwcSeries { get; set; }
        public virtual ICollection<OrganizationCategoriesImages> OrganizationCategoriesImages { get; set; }
        public virtual ICollection<OrganizationRedimTypes> OrganizationRedimTypes { get; set; }
        public virtual ICollection<PricingBasePriceCommision> PricingBasePriceCommision { get; set; }
        public virtual ICollection<ShortCardNumbers> ShortCardNumbers { get; set; }
        public virtual ICollection<Tags> Tags { get; set; }
    }
}
