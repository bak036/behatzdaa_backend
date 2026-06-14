using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class DtsDB_Context : DbContext
    {
        public DtsDB_Context()
        {
        }

        public DtsDB_Context(DbContextOptions<DtsDB_Context> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }

        public virtual DbSet<ActionTypeBusinessLog> ActionTypeBusinessLog { get; set; }
        public virtual DbSet<ActivationType> ActivationType { get; set; }
        public virtual DbSet<AdditionalFatherCategories> AdditionalFatherCategories { get; set; }
        public virtual DbSet<AgeGroup> AgeGroup { get; set; }
        public virtual DbSet<AllClubAutomaticReportStatus> AllClubAutomaticReportStatus { get; set; }
        public virtual DbSet<AllClubProductsVarsChange> AllClubProductsVarsChange { get; set; }
        public virtual DbSet<AllClubProductsVarsChangeTest> AllClubProductsVarsChangeTest { get; set; }
        public virtual DbSet<AllFieldMap> AllFieldMap { get; set; }
        public virtual DbSet<AllMainField> AllMainField { get; set; }
        public virtual DbSet<AllMembers> AllMembers { get; set; }
        public virtual DbSet<AllMembersFieldMap> AllMembersFieldMap { get; set; }
        public virtual DbSet<Amir> Amir { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<BlMsOrgs> BlMsOrgs { get; set; }
        public virtual DbSet<BlTransactionLog> BlTransactionLog { get; set; }
        public virtual DbSet<BlValuePerCoupon> BlValuePerCoupon { get; set; }
        public virtual DbSet<BlValuePerCouponNew> BlValuePerCouponNew { get; set; }
        public virtual DbSet<BudgetStockCategory> BudgetStockCategory { get; set; }
        public virtual DbSet<BudgetStockDays> BudgetStockDays { get; set; }
        public virtual DbSet<BudgetStockDetails> BudgetStockDetails { get; set; }
        public virtual DbSet<BudgetStockSpecialDays> BudgetStockSpecialDays { get; set; }
        public virtual DbSet<BusNew> BusNew { get; set; }
        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<BusinessModes> BusinessModes { get; set; }
        public virtual DbSet<BusinessSubBranch> BusinessSubBranch { get; set; }
        public virtual DbSet<BusinessSubType> BusinessSubType { get; set; }
        public virtual DbSet<BusinessType> BusinessType { get; set; }
        public virtual DbSet<CampaignBenefitTypeDescription> CampaignBenefitTypeDescription { get; set; }
        public virtual DbSet<CampaignBenefitTypes> CampaignBenefitTypes { get; set; }
        public virtual DbSet<CampaignBenefits> CampaignBenefits { get; set; }
        public virtual DbSet<CampaignCashBackBenefits> CampaignCashBackBenefits { get; set; }
        public virtual DbSet<CampaignCategories> CampaignCategories { get; set; }
        public virtual DbSet<CampaignConditionGroupTypes> CampaignConditionGroupTypes { get; set; }
        public virtual DbSet<CampaignCountType> CampaignCountType { get; set; }
        public virtual DbSet<CampaignGroups> CampaignGroups { get; set; }
        public virtual DbSet<CampaignMoneyBalance> CampaignMoneyBalance { get; set; }
        public virtual DbSet<CampaignTypes> CampaignTypes { get; set; }
        public virtual DbSet<Campaigns> Campaigns { get; set; }
        public virtual DbSet<CardStatus> CardStatus { get; set; }
        public virtual DbSet<CardTypes> CardTypes { get; set; }
        public virtual DbSet<CardsOld> CardsOld { get; set; }
        public virtual DbSet<CardsSkeleton> CardsSkeleton { get; set; }
        public virtual DbSet<CatOrg> CatOrg { get; set; }
        public virtual DbSet<CategorySeries> CategorySeries { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CityOld> CityOld { get; set; }
        public virtual DbSet<CodeTypes> CodeTypes { get; set; }
        public virtual DbSet<ConfirmationTemplates> ConfirmationTemplates { get; set; }
        public virtual DbSet<ConsumptionOrderDeliveryModes> ConsumptionOrderDeliveryModes { get; set; }
        public virtual DbSet<ConsumptionOrderShipingCompanies> ConsumptionOrderShipingCompanies { get; set; }
        public virtual DbSet<ConsumptionOrderStatus> ConsumptionOrderStatus { get; set; }
        public virtual DbSet<ConsumptionOrderStatusReason> ConsumptionOrderStatusReason { get; set; }
        public virtual DbSet<CouponCancelRequest> CouponCancelRequest { get; set; }
        public virtual DbSet<CouponCancelVariants> CouponCancelVariants { get; set; }
        public virtual DbSet<CouponsStock> CouponsStock { get; set; }
        public virtual DbSet<CouponsStockLog> CouponsStockLog { get; set; }
        public virtual DbSet<CouponsStockLogMp> CouponsStockLogMp { get; set; }
        public virtual DbSet<CouponsStockType> CouponsStockType { get; set; }
        public virtual DbSet<CouponsStocksDetails> CouponsStocksDetails { get; set; }
        public virtual DbSet<CreditGuardOld> CreditGuardOld { get; set; }
        public virtual DbSet<CreditGuardOrderDetails> CreditGuardOrderDetails { get; set; }
        public virtual DbSet<CreditGuardOrders> CreditGuardOrders { get; set; }
        public virtual DbSet<Crm> Crm { get; set; }
        public virtual DbSet<CrmDataTempToDelete> CrmDataTempToDelete { get; set; }
        public virtual DbSet<CrmDetails> CrmDetails { get; set; }
        public virtual DbSet<CrmHistory> CrmHistory { get; set; }
        public virtual DbSet<CrmOld> CrmOld { get; set; }
        public virtual DbSet<CrmTypeOrg> CrmTypeOrg { get; set; }
        public virtual DbSet<DbgenericStructure> DbgenericStructure { get; set; }
        public virtual DbSet<DebugLog> DebugLog { get; set; }
        public virtual DbSet<DebugMessages> DebugMessages { get; set; }
        public virtual DbSet<DtsNewsletter> DtsNewsletter { get; set; }
        public virtual DbSet<DtsWorkers> DtsWorkers { get; set; }
        public virtual DbSet<EmailQueue> EmailQueue { get; set; }
        public virtual DbSet<EmailQueueArchive> EmailQueueArchive { get; set; }
        public virtual DbSet<EmailSubscriptionActivity> EmailSubscriptionActivity { get; set; }
        public virtual DbSet<EmailSubscriptionTypes> EmailSubscriptionTypes { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<EventimCancellations> EventimCancellations { get; set; }
        public virtual DbSet<EventimCorruptedMediaRecords> EventimCorruptedMediaRecords { get; set; }
        public virtual DbSet<EventimMediaFile> EventimMediaFile { get; set; }
        public virtual DbSet<EventimMediaRecords> EventimMediaRecords { get; set; }
        public virtual DbSet<EventimMissingMediaRecords> EventimMissingMediaRecords { get; set; }
        public virtual DbSet<EventimMissingTickets20160301> EventimMissingTickets20160301 { get; set; }
        public virtual DbSet<EventimStock> EventimStock { get; set; }
        public virtual DbSet<EventimStockDetails> EventimStockDetails { get; set; }
        public virtual DbSet<EventimStockFile> EventimStockFile { get; set; }
        public virtual DbSet<EventimStockRecords> EventimStockRecords { get; set; }
        public virtual DbSet<EventimStockRecords201510270935> EventimStockRecords201510270935 { get; set; }
        public virtual DbSet<Faq> Faq { get; set; }
        public virtual DbSet<Faqsubjects> Faqsubjects { get; set; }
        public virtual DbSet<FileUploadDescription> FileUploadDescription { get; set; }
        public virtual DbSet<GiftCardOrgansationBusiness> GiftCardOrgansationBusiness { get; set; }
        public virtual DbSet<GiftCardOrgansationBusinessOld> GiftCardOrgansationBusinessOld { get; set; }
        public virtual DbSet<GiftcardAppConfig> GiftcardAppConfig { get; set; }
        public virtual DbSet<HelpDarga> HelpDarga { get; set; }
        public virtual DbSet<HelpMissingRecordsErrors> HelpMissingRecordsErrors { get; set; }
        public virtual DbSet<HelpSogoodGroup> HelpSogoodGroup { get; set; }
        public virtual DbSet<Holydays> Holydays { get; set; }
        public virtual DbSet<HoursManageProjecTypes> HoursManageProjecTypes { get; set; }
        public virtual DbSet<HoursManageProjects> HoursManageProjects { get; set; }
        public virtual DbSet<HoursManageReport> HoursManageReport { get; set; }
        public virtual DbSet<HoursManageUsers> HoursManageUsers { get; set; }
        public virtual DbSet<HtmlComponentBranches> HtmlComponentBranches { get; set; }
        public virtual DbSet<HtmlComponentSubjects> HtmlComponentSubjects { get; set; }
        public virtual DbSet<HtmlComponents> HtmlComponents { get; set; }
        public virtual DbSet<ImageSliderLocation> ImageSliderLocation { get; set; }
        public virtual DbSet<ImageTypes> ImageTypes { get; set; }
        public virtual DbSet<ImagesExtentions> ImagesExtentions { get; set; }
        public virtual DbSet<ImagesExtentionsToImageTypes> ImagesExtentionsToImageTypes { get; set; }
        public virtual DbSet<ImagesSlider> ImagesSlider { get; set; }
        public virtual DbSet<ImagesTypesToOrganizations> ImagesTypesToOrganizations { get; set; }
        public virtual DbSet<ImplamantationMethods> ImplamantationMethods { get; set; }
        public virtual DbSet<InvoiceRequests> InvoiceRequests { get; set; }
		public virtual DbSet<InvoiceManagementRequests> InvoiceManagementRequests { get; set; }
		public virtual DbSet<IrgunPriceForSubType> IrgunPriceForSubType { get; set; }
        public virtual DbSet<KeyValueList> KeyValueList { get; set; }
        public virtual DbSet<KnowledgeCenter> KnowledgeCenter { get; set; }
        public virtual DbSet<KnowledgeCenterErrors> KnowledgeCenterErrors { get; set; }
        public virtual DbSet<KnowledgeCenterFiles> KnowledgeCenterFiles { get; set; }
        public virtual DbSet<KnowledgeCenterKinds> KnowledgeCenterKinds { get; set; }
        public virtual DbSet<KnowledgeCenterPics> KnowledgeCenterPics { get; set; }
        public virtual DbSet<LcmediaRecords> LcmediaRecords { get; set; }
        public virtual DbSet<LcmsOrgs> LcmsOrgs { get; set; }
        public virtual DbSet<LeumiCardActivationReport> LeumiCardActivationReport { get; set; }
        public virtual DbSet<LeumiCardBinList> LeumiCardBinList { get; set; }
        public virtual DbSet<LeumiCardErrors> LeumiCardErrors { get; set; }
        public virtual DbSet<LeumiCardFiles> LeumiCardFiles { get; set; }
        public virtual DbSet<LeumiCardMedia> LeumiCardMedia { get; set; }
        public virtual DbSet<LoadFiles> LoadFiles { get; set; }
        public virtual DbSet<LoadFilesFields> LoadFilesFields { get; set; }
        public virtual DbSet<LoadFilesLog> LoadFilesLog { get; set; }
        public virtual DbSet<LoadMoneyIframe> LoadMoneyIframe { get; set; }
        public virtual DbSet<LogGeneral> LogGeneral { get; set; }
        public virtual DbSet<LogGeneralEnv> LogGeneralEnv { get; set; }
        public virtual DbSet<LogGeneralResultsCode> LogGeneralResultsCode { get; set; }
        public virtual DbSet<LogGeneralType> LogGeneralType { get; set; }
        public virtual DbSet<LogGeneralTypeSub> LogGeneralTypeSub { get; set; }
        public virtual DbSet<LogGeneralUserType> LogGeneralUserType { get; set; }
        public virtual DbSet<LoginIp> LoginIp { get; set; }
        public virtual DbSet<ManagementBusinessId> ManagementBusinessId { get; set; }
        public virtual DbSet<ManualOps> ManualOps { get; set; }
        public virtual DbSet<ManualStatusTable> ManualStatusTable { get; set; }
        public virtual DbSet<MemberMessages> MemberMessages { get; set; }
        public virtual DbSet<MembershipFeeTypes> MembershipFeeTypes { get; set; }
        public virtual DbSet<MerchantOrganizationExclude> MerchantOrganizationExclude { get; set; }
        public virtual DbSet<MerchantPoss> MerchantPoss { get; set; }
        public virtual DbSet<MerchantPossOld> MerchantPossOld { get; set; }
        public virtual DbSet<MerchantTypes> MerchantTypes { get; set; }
        public virtual DbSet<Merchants> Merchants { get; set; }
        public virtual DbSet<MessageCategories> MessageCategories { get; set; }
        public virtual DbSet<MessageRules> MessageRules { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<MoneyTypes> MoneyTypes { get; set; }
        public virtual DbSet<MultipassExceptionOnCardid> MultipassExceptionOnCardid { get; set; }
        public virtual DbSet<MultipassLastCardIdRealization> MultipassLastCardIdRealization { get; set; }
        public virtual DbSet<MultipassTransactionsLog> MultipassTransactionsLog { get; set; }
        public virtual DbSet<MultipassTransactionsMedia> MultipassTransactionsMedia { get; set; }
        public virtual DbSet<MwcActivityTypes> MwcActivityTypes { get; set; }
        public virtual DbSet<MwcCanceled> MwcCanceled { get; set; }
        public virtual DbSet<MwcClubInfo> MwcClubInfo { get; set; }
        public virtual DbSet<MwcFile> MwcFile { get; set; }
        public virtual DbSet<MwcFileStatus> MwcFileStatus { get; set; }
        public virtual DbSet<MwcHanpakaRequestsPrint> MwcHanpakaRequestsPrint { get; set; }
        public virtual DbSet<MwcIssuerChains> MwcIssuerChains { get; set; }
        public virtual DbSet<MwcIssuers> MwcIssuers { get; set; }
        public virtual DbSet<MwcMedia> MwcMedia { get; set; }
        public virtual DbSet<MwcMediaOld> MwcMediaOld { get; set; }
        public virtual DbSet<MwcMediaPush> MwcMediaPush { get; set; }
        public virtual DbSet<MwcMethods> MwcMethods { get; set; }
        public virtual DbSet<MwcNetworkDiscount> MwcNetworkDiscount { get; set; }
        public virtual DbSet<MwcRcn> MwcRcn { get; set; }
        public virtual DbSet<MwcRtypePermission> MwcRtypePermission { get; set; }
        public virtual DbSet<MwcSeries> MwcSeries { get; set; }
        public virtual DbSet<MwcTlogRequest> MwcTlogRequest { get; set; }
        public virtual DbSet<MwcVpayAppServer> MwcVpayAppServer { get; set; }
        public virtual DbSet<MwcVpayBranches> MwcVpayBranches { get; set; }
        public virtual DbSet<MwcVpayBranchesOld> MwcVpayBranchesOld { get; set; }
        public virtual DbSet<MwcVpayBranchesProp> MwcVpayBranchesProp { get; set; }
        public virtual DbSet<MwcVpayChains> MwcVpayChains { get; set; }
        public virtual DbSet<MwcVpayChainsOld> MwcVpayChainsOld { get; set; }
        public virtual DbSet<MwcVpayChainsProp> MwcVpayChainsProp { get; set; }
        public virtual DbSet<MwcVpayEntities> MwcVpayEntities { get; set; }
        public virtual DbSet<MwcVpayLinkedChains> MwcVpayLinkedChains { get; set; }
        public virtual DbSet<MwcVpayTerminals> MwcVpayTerminals { get; set; }
        public virtual DbSet<MwcVpayWallets> MwcVpayWallets { get; set; }
        public virtual DbSet<MwcVpayWalletsToChains> MwcVpayWalletsToChains { get; set; }
        public virtual DbSet<Nets> Nets { get; set; }
        public virtual DbSet<NetsMultiCardsDb> NetsMultiCardsDb { get; set; }
        public virtual DbSet<NetsRelationship> NetsRelationship { get; set; }
        public virtual DbSet<NirshamimBarcode> NirshamimBarcode { get; set; }
        public virtual DbSet<OpTypes> OpTypes { get; set; }
        public virtual DbSet<OrderQueue> OrderQueue { get; set; }
        public virtual DbSet<OrdersRemarks> OrdersRemarks { get; set; }
        public virtual DbSet<OrganizatinBusinsess> OrganizatinBusinsess { get; set; }
        public virtual DbSet<OrganizationBalances> OrganizationBalances { get; set; }
        public virtual DbSet<OrganizationCategories> OrganizationCategories { get; set; }
        public virtual DbSet<OrganizationCategoriesImages> OrganizationCategoriesImages { get; set; }
        public virtual DbSet<OrganizationHamara> OrganizationHamara { get; set; }
        public virtual DbSet<OrganizationMainField> OrganizationMainField { get; set; }
        public virtual DbSet<OrganizationOps> OrganizationOps { get; set; }
        public virtual DbSet<OrganizationRedimTypes> OrganizationRedimTypes { get; set; }
        public virtual DbSet<OrganizationUsers> OrganizationUsers { get; set; }
        public virtual DbSet<Organizations> Organizations { get; set; }
        public virtual DbSet<OrganizationsIdenticalMoney> OrganizationsIdenticalMoney { get; set; }
        public virtual DbSet<OrgnizationProductOrderSite> OrgnizationProductOrderSite { get; set; }
        public virtual DbSet<PageDesign> PageDesign { get; set; }
        public virtual DbSet<PageFields> PageFields { get; set; }
        public virtual DbSet<PayerSuppliers> PayerSuppliers { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<PaymentModel> PaymentModel { get; set; }
        public virtual DbSet<Points> Points { get; set; }
        public virtual DbSet<PortalCategories> PortalCategories { get; set; }
        public virtual DbSet<PortalCategoriesImages> PortalCategoriesImages { get; set; }
        public virtual DbSet<PortalCategoriesTicketHubTours> PortalCategoriesTicketHubTours { get; set; }
        public virtual DbSet<PortalSearchData> PortalSearchData { get; set; }
        public virtual DbSet<PortalSearchDataBefore> PortalSearchDataBefore { get; set; }
        public virtual DbSet<PortalSearchDataOld> PortalSearchDataOld { get; set; }
        public virtual DbSet<PortalSearchDataQueueBusiness> PortalSearchDataQueueBusiness { get; set; }
        public virtual DbSet<PortalSearchDataQueueOrganizationCategories> PortalSearchDataQueueOrganizationCategories { get; set; }
        public virtual DbSet<PortalSearchDataQueueOrganizationCategoriesImages> PortalSearchDataQueueOrganizationCategoriesImages { get; set; }
        public virtual DbSet<PortalSearchDataQueuePortalCategories> PortalSearchDataQueuePortalCategories { get; set; }
        public virtual DbSet<PortalSearchDataQueueTags> PortalSearchDataQueueTags { get; set; }
        public virtual DbSet<PortalSearchDataQueueTagsCategory> PortalSearchDataQueueTagsCategory { get; set; }
        public virtual DbSet<PortalSearchDataTemp> PortalSearchDataTemp { get; set; }
        public virtual DbSet<PraxellFile> PraxellFile { get; set; }
        public virtual DbSet<PraxellFileStatus> PraxellFileStatus { get; set; }
        public virtual DbSet<PraxellMedia> PraxellMedia { get; set; }
        public virtual DbSet<PraxellNetworkName> PraxellNetworkName { get; set; }
        public virtual DbSet<PraxellStoreName> PraxellStoreName { get; set; }
        public virtual DbSet<PraxellTransactionLog> PraxellTransactionLog { get; set; }
        public virtual DbSet<PricingBasePriceCommision> PricingBasePriceCommision { get; set; }
        public virtual DbSet<ProdectTypes> ProdectTypes { get; set; }
        public virtual DbSet<ProductsVarsGlobal> ProductsVarsGlobal { get; set; }
        public virtual DbSet<ProductsVarsGlobalOld> ProductsVarsGlobalOld { get; set; }
        public virtual DbSet<ProductsVarsOld> ProductsVarsOld { get; set; }
        public virtual DbSet<ProductsVarsSubBranchExcluded> ProductsVarsSubBranchExcluded { get; set; }
        public virtual DbSet<ProviderBuisness> ProviderBuisness { get; set; }
        public virtual DbSet<Providers> Providers { get; set; }
        public virtual DbSet<Pulseem> Pulseem { get; set; }
        public virtual DbSet<Queries> Queries { get; set; }
        public virtual DbSet<RechargeResones> RechargeResones { get; set; }
        public virtual DbSet<RedimTypes> RedimTypes { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<RegisterMembers> RegisterMembers { get; set; }
        public virtual DbSet<RegisterType> RegisterType { get; set; }
        public virtual DbSet<RequestFiles> RequestFiles { get; set; }
        public virtual DbSet<RequestSourceTable> RequestSourceTable { get; set; }
        public virtual DbSet<RequestStatusTable> RequestStatusTable { get; set; }
        public virtual DbSet<RequestTypeTable> RequestTypeTable { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<RequestsHistorical> RequestsHistorical { get; set; }
        public virtual DbSet<RptFrequencyTypes> RptFrequencyTypes { get; set; }
        public virtual DbSet<RptJobSchedule> RptJobSchedule { get; set; }
        public virtual DbSet<RptJobs> RptJobs { get; set; }
        public virtual DbSet<RptParamValues> RptParamValues { get; set; }
        public virtual DbSet<RptParams> RptParams { get; set; }
        public virtual DbSet<RptRequest> RptRequest { get; set; }
        public virtual DbSet<SecureLogin> SecureLogin { get; set; }
        public virtual DbSet<SecureLoginGuid> SecureLoginGuid { get; set; }
        public virtual DbSet<ShamirLoginInfo> ShamirLoginInfo { get; set; }
        public virtual DbSet<ShortCardNumbers> ShortCardNumbers { get; set; }
        public virtual DbSet<ShortCardNumbersBank> ShortCardNumbersBank { get; set; }
        public virtual DbSet<ShortCardNumbersExcludeBusiness> ShortCardNumbersExcludeBusiness { get; set; }
        public virtual DbSet<SlinkActions> SlinkActions { get; set; }
        public virtual DbSet<SlinkMembersFiles> SlinkMembersFiles { get; set; }
        public virtual DbSet<SlinkOrganization> SlinkOrganization { get; set; }
        public virtual DbSet<SlinkProductTypes> SlinkProductTypes { get; set; }
        public virtual DbSet<SlinkProductTypesToOrganizations> SlinkProductTypesToOrganizations { get; set; }
        public virtual DbSet<SlinkRequestFiles> SlinkRequestFiles { get; set; }
        public virtual DbSet<SlinkRequests> SlinkRequests { get; set; }
        public virtual DbSet<SlinkUsers> SlinkUsers { get; set; }
        public virtual DbSet<SlinkUsersToOrganizations> SlinkUsersToOrganizations { get; set; }
        public virtual DbSet<SmsMessages> SmsMessages { get; set; }
        public virtual DbSet<SmsQueue> SmsQueue { get; set; }
        public virtual DbSet<SmsQueueUser> SmsQueueUser { get; set; }
        public virtual DbSet<SmsTypes> SmsTypes { get; set; }
        public virtual DbSet<SoGood> SoGood { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<Street> Street { get; set; }
        public virtual DbSet<SuppliersTypes> SuppliersTypes { get; set; }
        public virtual DbSet<SurveyAnswers> SurveyAnswers { get; set; }
        public virtual DbSet<SurveyQuestions> SurveyQuestions { get; set; }
        public virtual DbSet<SystemTable> SystemTable { get; set; }
        public virtual DbSet<Table1> Table1 { get; set; }
        public virtual DbSet<Table3> Table3 { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<TagsBusiness> TagsBusiness { get; set; }
        public virtual DbSet<TagsCategory> TagsCategory { get; set; }
        public virtual DbSet<TagsPremium> TagsPremium { get; set; }
        public virtual DbSet<TagsStrips> TagsStrips { get; set; }
        public virtual DbSet<TaklaTable> TaklaTable { get; set; }
        public virtual DbSet<TblAuditLog> TblAuditLog { get; set; }
        public virtual DbSet<TblAuditLogin> TblAuditLogin { get; set; }
        public virtual DbSet<TeleclalDlrTable> TeleclalDlrTable { get; set; }
        public virtual DbSet<TempLifrok> TempLifrok { get; set; }
        public virtual DbSet<Terminal> Terminal { get; set; }
        public virtual DbSet<TerminalSettings> TerminalSettings { get; set; }
        public virtual DbSet<TerminalType> TerminalType { get; set; }
        public virtual DbSet<Theaters> Theaters { get; set; }
        public virtual DbSet<TitanCinemas> TitanCinemas { get; set; }
        public virtual DbSet<TitanParseCode> TitanParseCode { get; set; }
        public virtual DbSet<TmpYitrotLeumiCardAfter> TmpYitrotLeumiCardAfter { get; set; }
        public virtual DbSet<TracePele> TracePele { get; set; }
        public virtual DbSet<TypeCard> TypeCard { get; set; }
        public virtual DbSet<TypeImplementationDate> TypeImplementationDate { get; set; }
        public virtual DbSet<UserPortalCategories> UserPortalCategories { get; set; }
        public virtual DbSet<ValueCardChangesLog> ValueCardChangesLog { get; set; }
        public virtual DbSet<ValueCardErrorCodes> ValueCardErrorCodes { get; set; }
        public virtual DbSet<ValueCardPromoId> ValueCardPromoId { get; set; }
        public virtual DbSet<ValueCardServices> ValueCardServices { get; set; }
        public virtual DbSet<ValueCardSettings> ValueCardSettings { get; set; }
        public virtual DbSet<ValueCardTranLog> ValueCardTranLog { get; set; }
        public virtual DbSet<VarChangeLog> VarChangeLog { get; set; }
        public virtual DbSet<VariantBenefitInformation> VariantBenefitInformation { get; set; }
        public virtual DbSet<VariantPresents> VariantPresents { get; set; }
        public virtual DbSet<VariantProperties> VariantProperties { get; set; }
        public virtual DbSet<VariantStock> VariantStock { get; set; }
        public virtual DbSet<VariantVerifone> VariantVerifone { get; set; }
        public virtual DbSet<WalletLoadMoney> WalletLoadMoney { get; set; }
        public virtual DbSet<WebConfigKeys> WebConfigKeys { get; set; }
        public virtual DbSet<WebServiceTransactionOld> WebServiceTransactionOld { get; set; }
        public virtual DbSet<Wserrors> Wserrors { get; set; }
        public virtual DbSet<WsmethodPremission> WsmethodPremission { get; set; }
        public virtual DbSet<Wsmethods> Wsmethods { get; set; }

        // Unable to generate entity type for table 'dbo.FeaturesFlags'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.halperinTXT'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaRecords_BackUp_2112'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_AllMembersAdded'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcVpayAppServer_Orgs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Type_Insert_temp_to_delete'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Eventim_Affiliate_to_Organization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tempsmsqueue'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PraxsellActivity'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaFile_2015_10_19'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaRecords_2015_10_19'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.RestoreDeltaIDs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.OrganizationCategories_2016_02_18'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.sms_job_last_run'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.View_AllClub_PraxellCards'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimStockRecords_2015_10_19'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignBenefitsConnection'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.eventimLog'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.OrganizationBakaraTerminals'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimStockFile_2015_10_19'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.from1.9.11to1.5.12'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DischargeReasons'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignBenefitConditions'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Temp__LongCardNumbersToShort$'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.business_26122018'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.blbusns'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Data_Old_Excel_temp_to_delete'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.VerMarch'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.WalletSequence'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductGrouping'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductGroupingTypes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Temp_ImportVariants'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BusinessAccountingTypes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.city2018'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_OrdersAndRealize_21022016'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TakalaType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Cards'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Data_Subject'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.InternetBee_Orders'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TakalaStatus'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SmsProvider'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Type_OldCrm_temp_to_delete'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.FactorOpenType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DynamicFormulaForOrganization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.FactorCurrentType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.OrganizationType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Book1'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Data_Ops_temp_to_delete'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Data_Orgs_temp_to_delete'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.businessSubBranch13012019'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignConditionTypes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Praxell_ErrorMsg'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.LeumiCardRequests'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BusinessLastUpdate'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tmp'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpProductType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MessagesOld'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.LC_Cards'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BusinessLog'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.News'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EmailType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.temp'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tmp_WebServiceTransaction'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BL_MS_Params'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.OrganizationOperator'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_Realized_ALL'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PasswordHistory'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BL_Limitations'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProviderMainTerminal'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BusinessBackup_03-12-18'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.conformation_qa_yonatan'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BusinessSubBranchBackup_03-12-18'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.'{7D0F59A6-459A-E311-93A8-D89D67$''. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.upoadLogo'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.VariantPresentType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.UserType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CardRequest'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_tier_types'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TMP_YitrotLeumiCard_Befor'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Takala'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcMedia_Balance'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.LeumiCardStatuses'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.temporary_table'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TMP_RequestsUnload171108'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CardTamplate'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PortalSearchData22_07'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Installments'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Table_2'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Harycodes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.RPT_Request_OLD'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ValueCardGroupID'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.categoriesHashmal'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.codes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PortalSearchData23_07'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpFamilyStatus'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HamaraSapakim'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PortalCategoriesBackup_19-11-18'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Praxell_OrgDefinition'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_OrdersAndRealize_20012016'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpGender'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BlockReasons'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpOccupationBackup'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ICountTypes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.sms_amir'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.doublesCSV'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.businessSubBranch121218'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.businessArchive111218'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.buzzer'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TmpTbl'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpPopulationType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpOccupation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DWH_business'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AnaQaReport'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignsCatch'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EmailContactCrm'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpEducation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PortalSearchData23'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaRecords_2016_02_04'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ShvaCodes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PortalSearchDataTagsAndMessages'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DWH_businessSubBranch'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.VariantPresentsList'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MemberOrder'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SoGoodBackup'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignMerchants'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TerminalActive'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tmp_Categories'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcOrganizationCommision'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Knowledge4All_OrganizationCategories'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.IrgunHamorimActivationReport'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._vars_close'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaRecords_2015_11_30'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.LCMS_Params'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.IsraelPostCities'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BL_TransactionLog_backUp'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AnaQaReport1'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ResearchInterests'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcMediaMissingRecords'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.LoadMoneyProperty'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcMedia_05_2018'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.doubles_20130528'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.logoss'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimExhibitions'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.cities_to_be_copied'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MemberAuthorization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_Day_Surfers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.test17'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ResearchInterestsForMemers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AdvertisingSpace'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Authorization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.raun'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpAuthorization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaRecords_2015_11_16'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ResearchInterestsForMemersOther'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SoGoodTransaction'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.businessArchive'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TitanNofshonitClubs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SiteConfiguration'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.busisub'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SlinkRequestStatus'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_MemberAndShoppingData'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignStatusType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BarCodeNeveNofesh'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.resCSV'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.mc'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.coupons_leumi'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignGroupsTypes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CouponsRealization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PraxellMediaBackUp'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Presentations'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.downloaded'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TakalaDetails'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BusinessOrganization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.businessSubBranchNew'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.codes3'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CreditGuard__processing'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignLevel'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CreditGuard__org'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProtocolAtractions_TAKALOT'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_NotRealize_Orders'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductsVars'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_NotRealize_Realize'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.WebServiceTransaction'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Codes-yonatan_table_can_be_deleted'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ATRACTIONSOrders'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BlkPrc'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Billing_History'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.XXXX'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.leumimarch'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.wallets'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.LogGeneralRemarks'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SplitOrderbusinessIds'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaRecords_2016_07_06'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HO'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TagsLocation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CategoryDetailsType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PricingBasePriceCommisionUpload'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimStockRecords_2016_07_06'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ReservedSeats'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SlinkActionsDescription'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.RequestSearchParams'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Severity'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.test'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Source'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Status'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcRCNStatus'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Subject'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PricingValidationErrorsHistory'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DB_Errors'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Orders_exe'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BlockedIPs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Orders'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.burger'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcRequestBalance'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_Realized__'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpWork'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ConsumptionOrderCancelledBy'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Lalin_Temp'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Query'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CRM_Type'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BudgetStockRemoveVars'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Coment'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SmsQueueForCheck'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.halperin'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.BudgetStockVars'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CSVconvertNew'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.yoterTest'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimMediaRecords20150930'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tempSMSmessage'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AllClub_Realized'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpIcountType'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignGroupsSubTypes'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ContainerManager.Container.Resolve<IConfigurationManager>().GetConnectionStringByValue<string>("DTSOnlineContext"));

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<View_GetBusinessAndTags>().HasKey(c => new { c.ChainID, c.TagID, c.BuisnessID });

            modelBuilder.Entity<ActionTypeBusinessLog>(entity =>
            {
                entity.HasKey(e => e.ActionType);

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<ActivationType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<AdditionalFatherCategories>(entity =>
            {
                entity.HasKey(e => new { e.CategoryNumber, e.FatherCategory, e.OrgId });
            });

            modelBuilder.Entity<AgeGroup>(entity =>
            {
                entity.HasKey(e => new { e.FromAge, e.ToAge });

                entity.Property(e => e.BetweenAges)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<AllClubAutomaticReportStatus>(entity =>
            {
                entity.ToTable("AllClub_AutomaticReportStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateSendReport).HasColumnType("datetime");

                entity.Property(e => e.PresentationCode).HasMaxLength(50);

                entity.Property(e => e.PresentationName).HasMaxLength(250);

                entity.Property(e => e.ShowDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AllClubProductsVarsChange>(entity =>
            {
                entity.HasKey(e => e.IndexNum)
                    .HasName("PK__AllClub___496E7F1A571DFC04");

                entity.ToTable("AllClub_ProductsVars_Change");

                entity.HasIndex(e => e.FullBarCode)
                    .HasName("idx_Nonclustered_AllClub_ProductsVars_Change_FullBarCode");

                entity.HasIndex(e => new { e.OrgId, e.FullBarCode, e.LastUpdateDate })
                    .HasName("NonClusteredIndex-20170718-085139");

                entity.Property(e => e.BasePrice).HasMaxLength(500);

                entity.Property(e => e.BusinessId).HasMaxLength(50);

                entity.Property(e => e.BusinessList).HasMaxLength(4000);

                entity.Property(e => e.BusinessName).HasMaxLength(50);

                entity.Property(e => e.BusinessShortName).HasMaxLength(50);

                entity.Property(e => e.BusinessSubTypeId).HasColumnName("BusinessSubTypeID");

                entity.Property(e => e.CashBackMony).HasColumnType("money");

                entity.Property(e => e.CatalogicPrice).HasMaxLength(500);

                entity.Property(e => e.CupaPrice).HasColumnType("money");

                entity.Property(e => e.CupaPriceUpdateDate).HasColumnType("smalldatetime");

                entity.Property(e => e.CuponStockId).HasColumnName("CuponStockID");

                entity.Property(e => e.DateCreateRow).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.FullBarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GroupLimitsId).HasColumnName("GroupLimitsID");

                entity.Property(e => e.IrgunPriceFormula).HasMaxLength(500);

                entity.Property(e => e.Iscampaign).HasColumnName("ISCampaign");

                entity.Property(e => e.Ivrcode)
                    .HasColumnName("IVRCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ivrorder).HasColumnName("IVROrder");

                entity.Property(e => e.LastImplementationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.MemberDailyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberGeneralLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberMonthlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberQuarterLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberWeeklyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberYearlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MosoponNameVar).HasMaxLength(24);

                entity.Property(e => e.OrgDbname)
                    .IsRequired()
                    .HasColumnName("OrgDBName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgPayer).HasColumnName("orgPayer");

                entity.Property(e => e.PosbarCode)
                    .HasColumnName("POSBarCode")
                    .HasMaxLength(50);

                entity.Property(e => e.PresentTypeId).HasColumnName("PresentTypeID");

                entity.Property(e => e.PresentationCode).HasMaxLength(30);

                entity.Property(e => e.ProductDailyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductGeneralLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductMonthlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductQuarterLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductWeeklyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductYearlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProviderPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReferenceDeviation).HasMaxLength(20);

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.SectionCode).HasMaxLength(30);

                entity.Property(e => e.SellEndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.SendReportDate).HasColumnType("datetime");

                entity.Property(e => e.ShortNameVar)
                    .HasColumnName("shortNameVar")
                    .HasMaxLength(50);

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Type).HasMaxLength(10);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.VarComissionFormula).HasMaxLength(500);

                entity.Property(e => e.VarDiscountFormula).HasMaxLength(500);

                entity.Property(e => e.VarName).HasMaxLength(250);

                entity.Property(e => e.VarPriceFormula).HasMaxLength(500);

                entity.Property(e => e.Vat).HasColumnName("VAT");
            });

            modelBuilder.Entity<AllClubProductsVarsChangeTest>(entity =>
            {
                entity.HasKey(e => e.IndexNum)
                    .HasName("PK__AllClub___496E7F1AE9CBB70A");

                entity.ToTable("AllClub_ProductsVars_Change_Test");

                entity.HasIndex(e => e.FullBarCode)
                    .HasName("idx_Nonclustered_AllClub_ProductsVars_Change_FullBarCode_Test");

                entity.HasIndex(e => new { e.OrgId, e.FullBarCode, e.LastUpdateDate })
                    .HasName("NonClusteredIndex-20170718-085139_Test");

                entity.Property(e => e.BusinessId).HasMaxLength(50);

                entity.Property(e => e.BusinessList).HasMaxLength(4000);

                entity.Property(e => e.BusinessName).HasMaxLength(50);

                entity.Property(e => e.BusinessShortName).HasMaxLength(50);

                entity.Property(e => e.BusinessSubTypeId).HasColumnName("BusinessSubTypeID");

                entity.Property(e => e.CashBackMony).HasColumnType("money");

                entity.Property(e => e.CatalogicPrice).HasMaxLength(500);

                entity.Property(e => e.CupaPrice).HasColumnType("money");

                entity.Property(e => e.CupaPriceUpdateDate).HasColumnType("smalldatetime");

                entity.Property(e => e.CuponStockId).HasColumnName("CuponStockID");

                entity.Property(e => e.DateCreateRow).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.FullBarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GroupLimitsId).HasColumnName("GroupLimitsID");

                entity.Property(e => e.IrgunPriceFormula).HasMaxLength(500);

                entity.Property(e => e.Iscampaign).HasColumnName("ISCampaign");

                entity.Property(e => e.Ivrcode)
                    .HasColumnName("IVRCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ivrorder).HasColumnName("IVROrder");

                entity.Property(e => e.LastImplementationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.MemberDailyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberGeneralLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberMonthlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberQuarterLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberWeeklyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberYearlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MosoponNameVar).HasMaxLength(24);

                entity.Property(e => e.OrgDbname)
                    .IsRequired()
                    .HasColumnName("OrgDBName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgPayer).HasColumnName("orgPayer");

                entity.Property(e => e.PosbarCode)
                    .HasColumnName("POSBarCode")
                    .HasMaxLength(50);

                entity.Property(e => e.PresentTypeId).HasColumnName("PresentTypeID");

                entity.Property(e => e.PresentationCode).HasMaxLength(30);

                entity.Property(e => e.ProductDailyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductGeneralLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductMonthlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductQuarterLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductWeeklyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductYearlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProviderPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReferenceDeviation).HasMaxLength(20);

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.SectionCode).HasMaxLength(30);

                entity.Property(e => e.SellEndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.SendReportDate).HasColumnType("datetime");

                entity.Property(e => e.ShortNameVar)
                    .HasColumnName("shortNameVar")
                    .HasMaxLength(50);

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Type).HasMaxLength(10);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.VarComissionFormula).HasMaxLength(500);

                entity.Property(e => e.VarDiscountFormula).HasMaxLength(500);

                entity.Property(e => e.VarName).HasMaxLength(250);

                entity.Property(e => e.VarPriceFormula).HasMaxLength(500);

                entity.Property(e => e.Vat).HasColumnName("VAT");
            });

            modelBuilder.Entity<AllFieldMap>(entity =>
            {
                entity.HasKey(e => e.IdCounter);

                entity.HasIndex(e => e.ColumnPlace)
                    .HasName("IX_AllFieldMap")
                    .IsUnique();

                entity.Property(e => e.IdCounter)
                    .HasColumnName("idCounter")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<AllMainField>(entity =>
            {
                entity.HasKey(e => e.MainFieldId);

                entity.Property(e => e.MainFieldId).HasColumnName("MainFieldID");

                entity.Property(e => e.MainFieldName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AllMembers>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId)
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.BirthDate).HasColumnType("smalldatetime");

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FactorySymbol).HasMaxLength(10);

                entity.Property(e => e.Hamara1Balance).HasColumnType("money");

                entity.Property(e => e.Hodaa).HasMaxLength(255);

                entity.Property(e => e.MariageDate).HasColumnType("smalldatetime");

                entity.Property(e => e.MemberCardNumber).HasMaxLength(16);

                entity.Property(e => e.MemberFirstName).HasMaxLength(50);

                entity.Property(e => e.MemberLastName).HasMaxLength(50);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.MemberSpecialId)
                    .HasColumnName("MemberSpecialID")
                    .HasMaxLength(16);

                entity.Property(e => e.MobilePhone).HasMaxLength(13);

                entity.Property(e => e.MoreDetailsXml)
                    .HasColumnName("MoreDetailsXML")
                    .HasColumnType("ntext");

                entity.Property(e => e.PartnerBirthDate).HasColumnType("smalldatetime");

                entity.Property(e => e.PartnerName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(13);

                entity.Property(e => e.Pobox)
                    .HasColumnName("PObox")
                    .HasMaxLength(10);

                entity.Property(e => e.RegistrationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Track2).HasMaxLength(37);

                entity.Property(e => e.Tz)
                    .HasColumnName("TZ")
                    .HasMaxLength(9);

                entity.Property(e => e.UserSiteLastLogin).HasColumnType("datetime");

                entity.Property(e => e.WaitingBalance).HasColumnType("money");

                entity.Property(e => e.WaitingBalanceLeumiCard).HasColumnType("money");

                entity.Property(e => e.Zip)
                    .HasColumnName("ZIP")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<AllMembersFieldMap>(entity =>
            {
                entity.HasKey(e => e.IdCounter);

                entity.Property(e => e.IdCounter).HasColumnName("idCounter");

                entity.Property(e => e.AllowNull)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ContactTable)
                    .HasColumnName("contactTable")
                    .HasMaxLength(30);

                entity.Property(e => e.ContactTableColumnDescription)
                    .HasColumnName("contactTableColumnDescription")
                    .HasMaxLength(30);

                entity.Property(e => e.ContactTableColumnId)
                    .HasColumnName("contactTableColumnID")
                    .HasMaxLength(30);

                entity.Property(e => e.IdTable)
                    .HasColumnName("idTable")
                    .HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.NameColumn)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PlaceContactTable)
                    .HasColumnName("placeContactTable")
                    .HasMaxLength(30);

                entity.Property(e => e.SizeColumn).HasColumnName("sizeColumn");

                entity.Property(e => e.TypeColumn)
                    .IsRequired()
                    .HasColumnName("typeColumn")
                    .HasMaxLength(10);

                entity.Property(e => e.Vad).HasColumnName("vad");
            });

            modelBuilder.Entity<Amir>(entity =>
            {
                entity.ToTable("amir");

                entity.Property(e => e.A)
                    .HasColumnName("a")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.B)
                    .HasColumnName("b")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Column6)
                    .HasColumnName("Column_6")
                    .HasMaxLength(128);

                entity.Property(e => e.Email).HasColumnName("email");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.OrgList).HasMaxLength(200);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<BlMsOrgs>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);

                entity.ToTable("BL_MS_Orgs");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("OrganizationID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<BlTransactionLog>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("BL_TransactionLog");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CancelTransactionId).HasColumnName("CancelTransactionID");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ShellyCurrentBalance).HasColumnType("money");

                entity.Property(e => e.ShellyOriginalAmount).HasColumnType("money");

                entity.Property(e => e.ShellyPrePaidLoad).HasMaxLength(50);

                entity.Property(e => e.ShellyReferenceType).HasMaxLength(50);

                entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<BlValuePerCoupon>(entity =>
            {
                entity.HasKey(e => e.CouponGroupId);

                entity.ToTable("BL_ValuePerCoupon");

                entity.Property(e => e.CouponGroupId)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<BlValuePerCouponNew>(entity =>
            {
                entity.HasKey(e => e.CouponGroupId);

                entity.ToTable("BL_ValuePerCouponNew");

                entity.Property(e => e.CouponGroupId)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<BudgetStockCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StockId).HasColumnName("StockID");
            });

            modelBuilder.Entity<BudgetStockDays>(entity =>
            {
                entity.HasIndex(e => new { e.StockId, e.Date })
                    .HasName("uniue stock and date")
                    .IsUnique();

                entity.Property(e => e.Date).HasColumnType("smalldatetime");

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.Transmitted).HasColumnName("transmitted");
            });

            modelBuilder.Entity<BudgetStockDetails>(entity =>
            {
                entity.HasKey(e => e.StockId);

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.SpecialDaysQuantity).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StockName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StockQuantity).HasColumnType("money");
            });

            modelBuilder.Entity<BudgetStockSpecialDays>(entity =>
            {
                entity.HasKey(e => new { e.HolydayDate, e.OrgId })
                    .HasName("PK__BudgetSt__87EC3D87F46ED69B");

                entity.Property(e => e.HolydayDate).HasColumnType("smalldatetime");

                entity.Property(e => e.OrgId)
                    .HasColumnName("OrgID")
                    .HasDefaultValueSql("((31))");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<BusNew>(entity =>
            {
                entity.HasKey(e => e.BuisnessId)
                    .HasName("PK__busNew__DE92299D28727FE6");

                entity.ToTable("busNew");

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ArrivalMapImage).HasMaxLength(100);

                entity.Property(e => e.BusinessDescription)
                    .HasColumnName("businessDescription")
                    .HasColumnType("ntext");

                entity.Property(e => e.BusinessDescriptionText)
                    .HasColumnName("businessDescriptionText")
                    .HasColumnType("ntext");

                entity.Property(e => e.BusinessImg)
                    .HasColumnName("BusinessIMG")
                    .HasMaxLength(100);

                entity.Property(e => e.BusinessSubTypeId).HasColumnName("BusinessSubTypeID");

                entity.Property(e => e.BusinessTaxName).HasMaxLength(300);

                entity.Property(e => e.BusinessUniqueNumber).HasMaxLength(20);

                entity.Property(e => e.ChainId).HasColumnName("ChainID");

                entity.Property(e => e.CinemaId).HasColumnName("CinemaID");

                entity.Property(e => e.EmailCc)
                    .HasColumnName("EmailCC")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ForeignTerminal).HasMaxLength(50);

                entity.Property(e => e.GpsPointerLat)
                    .HasColumnName("GpsPointer_Lat")
                    .HasMaxLength(50);

                entity.Property(e => e.GpsPointerLon)
                    .HasColumnName("GpsPointer_Lon")
                    .HasMaxLength(50);

                entity.Property(e => e.Idprovider).HasColumnName("idprovider");

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.IsTicketsHub).HasColumnName("isTicketsHub");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LastReportDate).HasColumnType("datetime");

                entity.Property(e => e.LiedPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LocationExplain).HasColumnType("ntext");

                entity.Property(e => e.LogoForPrinting).HasColumnType("ntext");

                entity.Property(e => e.MarketingCommission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Massage).HasMaxLength(50);

                entity.Property(e => e.PasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.StoreAddress).HasMaxLength(50);

                entity.Property(e => e.StoreCloseDate).HasColumnType("smalldatetime");

                entity.Property(e => e.StoreEmail).HasMaxLength(100);

                entity.Property(e => e.StoreEmailBookkeeping).HasMaxLength(100);

                entity.Property(e => e.StoreFax).HasMaxLength(50);

                entity.Property(e => e.StoreKesher).HasMaxLength(100);

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.StorePassword).HasMaxLength(50);

                entity.Property(e => e.StorePhone1).HasMaxLength(50);

                entity.Property(e => e.StorePhone2).HasMaxLength(50);

                entity.Property(e => e.StoreStartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.StoreStreetNumber).HasMaxLength(50);

                entity.Property(e => e.StoreUserName).HasMaxLength(50);

                entity.Property(e => e.StoreZipCode).HasMaxLength(10);

                entity.Property(e => e.TempStorePassword).HasMaxLength(50);

                entity.Property(e => e.TerminalNumber).HasMaxLength(10);

                entity.Property(e => e.TerminalPassword).HasMaxLength(20);

                entity.Property(e => e.TerminalUserName).HasMaxLength(20);

                entity.Property(e => e.UpdateLoginErrCount).HasColumnType("datetime");

                entity.Property(e => e.VatBusiness).HasColumnName("VAT_Business");

                entity.Property(e => e.WebSite).HasMaxLength(500);
            });

            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => e.BuisnessId)
                    .HasName("PK__business__DE92299DBC8A0494");

                entity.ToTable("business");

                entity.HasIndex(e => e.BuisnessId)
                    .HasName("IX_businessId")
                    .IsUnique();

                entity.HasIndex(e => e.ForeignTerminal);

                entity.HasIndex(e => e.StoreUserName)
                    .HasName("IX_business")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreName, e.Region });

                entity.HasIndex(e => new { e.StoreType, e.ForeignTerminal });

                entity.HasIndex(e => new { e.BuisnessId, e.StoreName, e.StoreType })
                    .HasName("IX_business_StoreType");

                entity.HasIndex(e => new { e.BuisnessId, e.StoreName, e.Active, e.BusinessSubTypeId })
                    .HasName("IX_business_Active_BusinessSubTypeID");

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ArrivalMapImage).HasMaxLength(100);

                entity.Property(e => e.BusinessDescription)
                    .HasColumnName("businessDescription")
                    .HasColumnType("ntext");

                entity.Property(e => e.BusinessDescriptionText)
                    .HasColumnName("businessDescriptionText")
                    .HasColumnType("ntext");

                entity.Property(e => e.BusinessImg)
                    .HasColumnName("BusinessIMG")
                    .HasMaxLength(100);

                entity.Property(e => e.BusinessSubTypeId).HasColumnName("BusinessSubTypeID");

                entity.Property(e => e.BusinessTaxName).HasMaxLength(300);

                entity.Property(e => e.BusinessUniqueNumber).HasMaxLength(20);

                entity.Property(e => e.ChainId).HasColumnName("ChainID");

                entity.Property(e => e.CinemaId).HasColumnName("CinemaID");

                entity.Property(e => e.EmailCc)
                    .HasColumnName("EmailCC")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ForeignTerminal).HasMaxLength(50);

                entity.Property(e => e.GpsPointerLat)
                    .HasColumnName("GpsPointer_Lat")
                    .HasMaxLength(50);

                entity.Property(e => e.GpsPointerLon)
                    .HasColumnName("GpsPointer_Lon")
                    .HasMaxLength(50);

                entity.Property(e => e.Idprovider).HasColumnName("idprovider");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsTicketsHub).HasColumnName("isTicketsHub");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LastReportDate).HasColumnType("datetime");

                entity.Property(e => e.LiedPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LocationExplain).HasColumnType("ntext");

                entity.Property(e => e.LogoForPrinting).HasColumnType("ntext");

                entity.Property(e => e.MarketingCommission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Massage).HasMaxLength(50);

                entity.Property(e => e.PasswordChanged)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)/(1))/(1900))");

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.StoreAddress).HasMaxLength(50);

                entity.Property(e => e.StoreCloseDate).HasColumnType("smalldatetime");

                entity.Property(e => e.StoreEmail).HasMaxLength(100);

                entity.Property(e => e.StoreEmailBookkeeping).HasMaxLength(100);

                entity.Property(e => e.StoreFax).HasMaxLength(50);

                entity.Property(e => e.StoreKesher).HasMaxLength(100);

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.StorePassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StorePhone1).HasMaxLength(50);

                entity.Property(e => e.StorePhone2).HasMaxLength(50);

                entity.Property(e => e.StoreStartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.StoreStreetNumber).HasMaxLength(50);

                entity.Property(e => e.StoreUserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StoreZipCode).HasMaxLength(10);

                entity.Property(e => e.TempStorePassword).HasMaxLength(50);

                entity.Property(e => e.TerminalNumber).HasMaxLength(10);

                entity.Property(e => e.TerminalPassword).HasMaxLength(20);

                entity.Property(e => e.TerminalUserName).HasMaxLength(20);

                entity.Property(e => e.UpdateLoginErrCount).HasColumnType("datetime");

                entity.Property(e => e.VatBusiness).HasColumnName("VAT_Business");

                entity.Property(e => e.WebSite).HasMaxLength(500);

                entity.HasOne(d => d.BusinessMode)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.BusinessModeId)
                    .HasConstraintName("FK_business_BusinessModes");

                entity.HasOne(d => d.Chain)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.ChainId)
                    .HasConstraintName("FK_business_MwcVpay_Chains");

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK_business_TitanCinemas");

                entity.HasOne(d => d.IdproviderNavigation)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.Idprovider)
                    .HasConstraintName("FK_business_Providers");
            });

            modelBuilder.Entity<BusinessModes>(entity =>
            {
                entity.HasKey(e => e.BusinessModeId);

                entity.Property(e => e.BusinessModeName).HasMaxLength(50);
            });

            modelBuilder.Entity<BusinessSubBranch>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RowNumberId });

                entity.ToTable("businessSubBranch");

                entity.HasIndex(e => new { e.BusinessId, e.TerminalNo });

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RowNumberId)
                    .HasColumnName("row_number_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.BusinessTaxName).HasMaxLength(300);

                entity.Property(e => e.BusinessUniqueNumber).HasMaxLength(20);

                entity.Property(e => e.GpsPointerLat)
                    .HasColumnName("GpsPointer_Lat")
                    .HasMaxLength(50);

                entity.Property(e => e.GpsPointerLon)
                    .HasColumnName("GpsPointer_Lon")
                    .HasMaxLength(50);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.StoreAddress).HasMaxLength(50);

                entity.Property(e => e.StoreEmail).HasMaxLength(100);

                entity.Property(e => e.StoreFax).HasMaxLength(20);

                entity.Property(e => e.StoreKesher).HasMaxLength(100);

                entity.Property(e => e.StorePhone1).HasMaxLength(20);

                entity.Property(e => e.StorePhone2).HasMaxLength(20);

                entity.Property(e => e.StoreStreetNumber).HasMaxLength(50);

                entity.Property(e => e.StoreZipCode).HasMaxLength(10);

                entity.Property(e => e.SubBusinessEmail).HasMaxLength(100);

                entity.Property(e => e.SubBusinessName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TerminalNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TerminalNoDts)
                    .HasColumnName("TerminalNoDTS")
                    .HasMaxLength(50);

                entity.Property(e => e.WebSite).HasMaxLength(500);

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.BusinessSubBranch)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_businessSubBranch_MwcVpay_Branches1");

                entity.HasOne(d => d.BranchNavigation)
                    .WithMany(p => p.BusinessSubBranchBranchNavigation)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_businessSubBranch_MwcVpay_Branches");

                entity.HasOne(d => d.BusinessMode)
                    .WithMany(p => p.BusinessSubBranchBusinessMode)
                    .HasForeignKey(d => d.BusinessModeId)
                    .HasConstraintName("FK_businessSubBranch_BusinessModes");
            });

            modelBuilder.Entity<BusinessSubType>(entity =>
            {
                entity.Property(e => e.BusinessSubTypeId).ValueGeneratedNever();

                entity.Property(e => e.BusinessSubTypeName).HasMaxLength(50);

                entity.Property(e => e.BusinessSubTypeNameWeb).HasMaxLength(50);

                entity.Property(e => e.IsCancel).HasDefaultValueSql("((0))");

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.RecordId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TypeNameExact)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.WebTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<CampaignBenefitTypeDescription>(entity =>
            {
                entity.HasKey(e => e.BenefitTypesId)
                    .HasName("PK_BenefitType");

                entity.Property(e => e.BenefitTypesId).HasColumnName("BenefitTypesID");

                entity.Property(e => e.BenefitDescription)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Remark).HasColumnType("ntext");
            });

            modelBuilder.Entity<CampaignBenefitTypes>(entity =>
            {
                entity.HasKey(e => e.BenefitTypeId);

                entity.Property(e => e.BenefitTypeId).HasColumnName("BenefitTypeID");

                entity.Property(e => e.BenefitTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CampaignBenefits>(entity =>
            {
                entity.HasKey(e => e.BenefitId);

                entity.HasIndex(e => new { e.CampaignId, e.BenefitType })
                    .HasName("IX_CampaignBenefits_BenefitType");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.BenefitCost)
                    .HasColumnType("decimal(6, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.BenefitName).HasColumnType("ntext");

                entity.Property(e => e.BenefitParams)
                    .HasColumnName("benefitParams")
                    .HasColumnType("ntext");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CustomerBenefitCost).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.FieldName).HasMaxLength(20);

                entity.Property(e => e.ImageName).HasMaxLength(20);

                entity.Property(e => e.Makat).HasColumnType("ntext");

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.Msg).HasMaxLength(100);

                entity.Property(e => e.Msg1).HasMaxLength(100);

                entity.Property(e => e.Msg2).HasMaxLength(100);

                entity.Property(e => e.OpenMsg).HasMaxLength(100);

                entity.Property(e => e.OpenMsg1).HasMaxLength(100);

                entity.Property(e => e.OpenMsg2).HasMaxLength(100);

                entity.Property(e => e.ProviderCommision).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.SumBill).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.SumValue)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<CampaignCashBackBenefits>(entity =>
            {
                entity.HasKey(e => new { e.BenefitId, e.CashBackBenefitId });

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.CashBackBenefitId).HasColumnName("CashBackBenefitID");
            });

            modelBuilder.Entity<CampaignCategories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<CampaignConditionGroupTypes>(entity =>
            {
                entity.HasKey(e => e.ConditionGroupTypeId);

                entity.Property(e => e.ConditionGroupTypeId)
                    .HasColumnName("ConditionGroupTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConditionGroupTypeName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<CampaignCountType>(entity =>
            {
                entity.HasKey(e => e.CountTypeId);

                entity.Property(e => e.CountTypeId).HasColumnName("CountTypeID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<CampaignGroups>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.HasIndex(e => new { e.GroupId, e.GroupType, e.CampainId })
                    .HasName("IX_CampaignGroups_CampainID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.CampainId).HasColumnName("CampainID");

                entity.Property(e => e.GroupDescription).HasMaxLength(100);

                entity.Property(e => e.TerminalForAddMembers).HasMaxLength(7);

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<CampaignMoneyBalance>(entity =>
            {
                entity.HasKey(e => new { e.CampaignId, e.MemberId })
                    .HasName("PK_MoneyBalance");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MoneyBalance).HasColumnType("money");
            });

            modelBuilder.Entity<CampaignTypes>(entity =>
            {
                entity.HasKey(e => e.CampaignTypeId);

                entity.Property(e => e.CampaignTypeId).HasColumnName("CampaignTypeID");

                entity.Property(e => e.CampaignType).HasMaxLength(30);
            });

            modelBuilder.Entity<Campaigns>(entity =>
            {
                entity.HasKey(e => e.CampaignId);

                entity.HasIndex(e => new { e.CampaignId, e.OrganizationId, e.CampaignStatus, e.CampaignStartTime, e.CampaignEndTime })
                    .HasName("IX_Campaigns_OrganizationID_CampaignStatus_CampaignStartTime_CampaignEndTime");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CampaigNameRequired).HasDefaultValueSql("((0))");

                entity.Property(e => e.CampaignEndTime).HasColumnType("datetime");

                entity.Property(e => e.CampaignExtCode).HasColumnType("ntext");

                entity.Property(e => e.CampaignStartTime).HasColumnType("datetime");

                entity.Property(e => e.CampainShortName).HasColumnType("ntext");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreationOp).HasColumnName("CreationOP");

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.NeedCharge).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SumCharge)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<CardStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<CardTypes>(entity =>
            {
                entity.HasKey(e => e.CardType);

                entity.Property(e => e.CardDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gcdisplay).HasColumnName("GCDisplay");
            });

            modelBuilder.Entity<CardsOld>(entity =>
            {
                entity.HasKey(e => e.CardNumber)
                    .HasName("PK_Cards");

                entity.HasIndex(e => e.Idmember)
                    .HasName("IX_IDMember");

                entity.HasIndex(e => new { e.Idmember, e.CardNumber })
                    .HasName("IX__Cards__IDMember_CardNumber");

                entity.Property(e => e.CardNumber)
                    .HasMaxLength(16)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivationIp)
                    .HasColumnName("ActivationIP")
                    .HasMaxLength(15);

                entity.Property(e => e.ActivationPhone).HasMaxLength(15);

                entity.Property(e => e.ActivationTime).HasColumnType("datetime");

                entity.Property(e => e.AddedTime).HasColumnType("datetime");

                entity.Property(e => e.BlockTime).HasColumnType("datetime");

                entity.Property(e => e.Idmember)
                    .IsRequired()
                    .HasColumnName("IDMember")
                    .HasMaxLength(9);

                entity.Property(e => e.Remark).HasColumnType("ntext");
            });

            modelBuilder.Entity<CardsSkeleton>(entity =>
            {
                entity.HasKey(e => e.CskeletonId)
                    .HasName("PK_dbo.CardsSkeleton");

                entity.Property(e => e.CskeletonId).HasColumnName("CSkeletonID");

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Cvv).HasColumnName("CVV");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.RcnId).HasColumnName("RcnID");

                entity.Property(e => e.SerieId).HasColumnName("SerieID");

                entity.Property(e => e.Track2Cvv).HasColumnName("Track2CVV");
            });

            modelBuilder.Entity<CatOrg>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryDescription).HasMaxLength(61);
            });

            modelBuilder.Entity<CategorySeries>(entity =>
            {
                entity.HasKey(e => new { e.CategoryNumber, e.SerieId });

                entity.Property(e => e.SerieId).HasColumnName("SerieID");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.CityName2).HasMaxLength(50);

                entity.Property(e => e.CityName3).HasMaxLength(50);

                entity.Property(e => e.RegionId).HasColumnName("RegionID");
            });

            modelBuilder.Entity<CityOld>(entity =>
            {
                entity.HasKey(e => e.CityId)
                    .HasName("PK__City__F2D21B769659FF05");

                entity.ToTable("City_Old");

                entity.Property(e => e.CityName).HasMaxLength(255);

                entity.Property(e => e.CityName2).HasMaxLength(15);

                entity.Property(e => e.CityName3).HasMaxLength(15);

                entity.Property(e => e.RegionId).HasColumnName("RegionID");
            });

            modelBuilder.Entity<CodeTypes>(entity =>
            {
                entity.HasKey(e => e.CodeType)
                    .HasName("PK__CodeType__D07465C17CF15A9B");

                entity.Property(e => e.CodeType).ValueGeneratedNever();

                entity.Property(e => e.CodeDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ConfirmationTemplates>(entity =>
            {
                entity.Property(e => e.Cc).HasMaxLength(250);

                entity.Property(e => e.EmailFrom).HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Subject).HasMaxLength(50);
            });

            modelBuilder.Entity<ConsumptionOrderDeliveryModes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ConsumptionOrderShipingCompanies>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.WebSite).HasMaxLength(500);
            });

            modelBuilder.Entity<ConsumptionOrderStatus>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DescriptionCustomerService)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DescriptionSite)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Sms).HasColumnName("SMS");
            });

            modelBuilder.Entity<ConsumptionOrderStatusReason>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CouponCancelRequest>(entity =>
            {
                entity.Property(e => e.CouponCancelRequestId).HasColumnName("CouponCancelRequestID");

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CouponCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnName("insertDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsSent).HasDefaultValueSql("((0))");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemberMobileNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemberName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberPhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.StockName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CouponCancelVariants>(entity =>
            {
                entity.HasKey(e => new { e.FullBarCode, e.OrganizationId });

                entity.Property(e => e.FullBarCode).HasMaxLength(50);
            });

            modelBuilder.Entity<CouponsStock>(entity =>
            {
                entity.HasKey(e => e.CouponId);

                entity.HasIndex(e => new { e.CouponCode, e.StockId });

                entity.HasIndex(e => new { e.CampaignId, e.SendingTime, e.RealizationTime, e.StockId })
                    .HasName("IX_CouponsStock");

                entity.HasIndex(e => new { e.CouponCode, e.StockId, e.SendingTime, e.MemberId })
                    .HasName("IX_CouponsStock_MemberID");

                entity.HasIndex(e => new { e.CouponId, e.SendingTime, e.CouponStatus, e.StockId, e.CouponCode })
                    .HasName("IX_CouponsStock_CouponStatus_StockID_CouponCode");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CardNumber).HasMaxLength(50);

                entity.Property(e => e.CouponCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CouponStatus)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.Posid).HasColumnName("POSID");

                entity.Property(e => e.RealizationTime).HasColumnType("datetime");

                entity.Property(e => e.SendingTime).HasColumnType("datetime");

                entity.Property(e => e.SeventhCardDigit)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.UploadCouponTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<CouponsStockLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__CouponsStockLog__446B1014");

                entity.Property(e => e.CardNumber).HasMaxLength(50);

                entity.Property(e => e.CouponCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.DateReset)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.Posid).HasColumnName("POSID");

                entity.Property(e => e.RealizationTime).HasColumnType("datetime");

                entity.Property(e => e.SendingTime).HasColumnType("datetime");

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<CouponsStockLogMp>(entity =>
            {
                entity.ToTable("CouponsStockLogMP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.DateCanceled).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateLoaded).HasColumnType("datetime");

                entity.Property(e => e.LoadId).HasColumnName("LoadID");

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.CouponsStockLogMp)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK__CouponsSt__Coupo__28E7134F");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.CouponsStockLogMp)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CouponsSt__Stock__29DB3788");
            });

            modelBuilder.Entity<CouponsStockType>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CouponsStocksDetails>(entity =>
            {
                entity.HasKey(e => e.StockId);

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);

                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.OuterId).HasColumnName("OuterID");

                entity.Property(e => e.PrintPattern).HasColumnType("ntext");

                entity.Property(e => e.ShowInStation).HasDefaultValueSql("((0))");

                entity.Property(e => e.StockActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.StockName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StockRemark).HasMaxLength(255);

                entity.HasOne(d => d.StockTypeNavigation)
                    .WithMany(p => p.CouponsStocksDetails)
                    .HasForeignKey(d => d.StockType)
                    .HasConstraintName("FK__CouponsSt__Stock__7C5F50F2");
            });

            modelBuilder.Entity<CreditGuardOld>(entity =>
            {
                entity.ToTable("CreditGuard_old");

                entity.HasIndex(e => e.יוםושעתעסקה)
                    .HasName("IX_CreditGuard__YomAndTime");

                entity.HasIndex(e => e.מזההפנימי)
                    .HasName("IX_CreditGuard__MezahePnimi");

                entity.HasIndex(e => e.סכום)
                    .HasName("IX_CreditGuard__Schum");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.אופןביצוע)
                    .HasColumnName("אופן ביצוע")
                    .HasMaxLength(255);

                entity.Property(e => e.אסמכתא).HasMaxLength(255);

                entity.Property(e => e.בדיקתCvv)
                    .HasColumnName("בדיקת CVV")
                    .HasMaxLength(255);

                entity.Property(e => e.הערה).HasMaxLength(255);

                entity.Property(e => e.זיהוימועדון).HasColumnName("זיהוי מועדון");

                entity.Property(e => e.חברהמנפיקה)
                    .HasColumnName("חברה מנפיקה")
                    .HasMaxLength(255);

                entity.Property(e => e.חברהסולקת)
                    .HasColumnName("חברה סולקת")
                    .HasMaxLength(255);

                entity.Property(e => e.יוםושעתעסקה)
                    .HasColumnName("יום ושעת עסקה")
                    .HasColumnType("datetime");

                entity.Property(e => e.כמותדלק)
                    .HasColumnName("כמות דלק")
                    .HasMaxLength(255);

                entity.Property(e => e.כמותשמן)
                    .HasColumnName("כמות שמן")
                    .HasMaxLength(255);

                entity.Property(e => e.כרטיסטיסה)
                    .HasColumnName("כרטיס טיסה")
                    .HasMaxLength(255);

                entity.Property(e => e.מדמריק)
                    .HasColumnName("מד מריק")
                    .HasMaxLength(255);

                entity.Property(e => e.מזההפנימי).HasColumnName("מזהה פנימי");

                entity.Property(e => e.מזההקבוצה)
                    .HasColumnName("מזהה קבוצה")
                    .HasMaxLength(255);

                entity.Property(e => e.מטבע).HasMaxLength(255);

                entity.Property(e => e.מספרהתשלומיםהנוספים)
                    .HasColumnName("מספר התשלומים הנוספים")
                    .HasMaxLength(255);

                entity.Property(e => e.מספרכרטיס)
                    .HasColumnName("מספר כרטיס")
                    .HasMaxLength(255);

                entity.Property(e => e.מספרמסוף).HasColumnName("מספר מסוף");

                entity.Property(e => e.מספרקובץ)
                    .HasColumnName("מספר קובץ")
                    .HasMaxLength(255);

                entity.Property(e => e.מספררכב)
                    .HasColumnName("מספר רכב")
                    .HasMaxLength(255);

                entity.Property(e => e.מספרשידור)
                    .HasColumnName("מספר שידור")
                    .HasMaxLength(255);

                entity.Property(e => e.מספרתז)
                    .HasColumnName("מספר ת#ז")
                    .HasMaxLength(255);

                entity.Property(e => e.נתוניםנוספים)
                    .HasColumnName("נתונים נוספים")
                    .HasMaxLength(255);

                entity.Property(e => e.סוגאשראי)
                    .HasColumnName("סוג אשראי")
                    .HasMaxLength(255);

                entity.Property(e => e.סוגכרטיסמותג)
                    .HasColumnName("סוג כרטיס - מותג")
                    .HasMaxLength(255);

                entity.Property(e => e.סוגעסקה)
                    .HasColumnName("סוג עסקה")
                    .HasMaxLength(255);

                entity.Property(e => e.סולק).HasMaxLength(255);

                entity.Property(e => e.סטטוס).HasMaxLength(255);

                entity.Property(e => e.סטטוסבדיקתתז)
                    .HasColumnName("סטטוס בדיקת ת#ז")
                    .HasMaxLength(255);

                entity.Property(e => e.סכוםבכוכבים).HasColumnName("סכום בכוכבים");

                entity.Property(e => e.סכוםשמן)
                    .HasColumnName("סכום שמן")
                    .HasMaxLength(255);

                entity.Property(e => e.סכוםתשלומים)
                    .HasColumnName("סכום תשלומים")
                    .HasMaxLength(255);

                entity.Property(e => e.פרטיאישור)
                    .HasColumnName("פרטי אישור")
                    .HasMaxLength(255);

                entity.Property(e => e.קובץשידור)
                    .HasColumnName("קובץ שידור")
                    .HasMaxLength(255);

                entity.Property(e => e.קודדלק)
                    .HasColumnName("קוד דלק")
                    .HasMaxLength(255);

                entity.Property(e => e.רשומתמקור)
                    .HasColumnName("רשומת מקור")
                    .HasMaxLength(255);

                entity.Property(e => e.שםמשתמש)
                    .HasColumnName("שם משתמש")
                    .HasMaxLength(255);

                entity.Property(e => e.תאורפרמטרJ)
                    .HasColumnName("תאור פרמטר J")
                    .HasMaxLength(255);

                entity.Property(e => e.תאריצושעתשידור)
                    .HasColumnName("תאריצ ושעת שידור")
                    .HasMaxLength(255);

                entity.Property(e => e.תוקף).HasMaxLength(255);

                entity.Property(e => e.תיאורקוד)
                    .HasColumnName("תיאור קוד")
                    .HasMaxLength(255);

                entity.Property(e => e.תשלוםראשון)
                    .HasColumnName("תשלום ראשון")
                    .HasMaxLength(255);

                entity.Property(e => e.תתסוגכרטיס)
                    .HasColumnName("תת סוג כרטיס")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CreditGuardOrderDetails>(entity =>
            {
                entity.HasIndex(e => e.TranId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnswerEnglish).HasMaxLength(150);

                entity.Property(e => e.AnswerHebrew).HasMaxLength(150);

                entity.Property(e => e.ClearingCompany).HasMaxLength(50);

                entity.Property(e => e.CreditCompany).HasMaxLength(50);

                entity.Property(e => e.CreditType).HasMaxLength(30);

                entity.Property(e => e.CreditTypeBrand).HasMaxLength(50);

                entity.Property(e => e.CreditTypeGroup).HasMaxLength(30);

                entity.Property(e => e.Currency).HasMaxLength(30);

                entity.Property(e => e.Cvvchecked)
                    .HasColumnName("CVVChecked")
                    .HasMaxLength(30);

                entity.Property(e => e.FileNumber).HasMaxLength(30);

                entity.Property(e => e.FileSent).HasMaxLength(30);

                entity.Property(e => e.FirstPayment).HasMaxLength(30);

                entity.Property(e => e.InsertTime).HasColumnType("datetime");

                entity.Property(e => e.NumberOfPayment).HasMaxLength(50);

                entity.Property(e => e.OrderMode).HasMaxLength(50);

                entity.Property(e => e.OrderType).HasMaxLength(50);

                entity.Property(e => e.QueriesWithCode).HasMaxLength(30);

                entity.Property(e => e.ReferenceNumber).HasMaxLength(10);

                entity.Property(e => e.RequestType).HasMaxLength(50);

                entity.Property(e => e.SentNumber).HasMaxLength(30);

                entity.Property(e => e.SourceRecond).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(30);

                entity.Property(e => e.SubCardType).HasMaxLength(20);

                entity.Property(e => e.TzChecked).HasMaxLength(30);

                entity.Property(e => e.Validity).HasMaxLength(30);

                entity.Property(e => e.VoucherNumber).HasMaxLength(30);

                entity.Property(e => e.Xfield).HasMaxLength(50);

                entity.HasOne(d => d.T)
                    .WithMany(p => p.CreditGuardOrderDetails)
                    .HasForeignKey(d => new { d.TerminalNumber, d.TranId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreditGuardOrderDetails_CreditGuardOrders");
            });

            modelBuilder.Entity<CreditGuardOrders>(entity =>
            {
                entity.HasKey(e => new { e.TerminalNumber, e.TranId });

                entity.HasIndex(e => e.InsertTime);

                entity.HasIndex(e => e.TranId);

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardOwnerId)
                    .IsRequired()
                    .HasColumnName("CardOwnerID")
                    .HasMaxLength(9);

                entity.Property(e => e.Charged).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.InsertTime).HasColumnType("datetime");

                entity.Property(e => e.Last4Digits)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.SentTime).HasColumnType("datetime");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Crm>(entity =>
            {
                entity.ToTable("CRM");

                entity.HasIndex(e => new { e.MemberId, e.OrgId });

                entity.Property(e => e.CrmId).HasColumnName("CrmID");

                entity.Property(e => e.BusinessIdRelated).HasColumnName("BusinessId_related");

                entity.Property(e => e.CrmDescription)
                    .IsRequired()
                    .HasColumnName("CRM_Description");

                entity.Property(e => e.CrmResult).HasColumnName("CRM_Result");

                entity.Property(e => e.CrmSeverityId).HasColumnName("CRM_SeverityID");

                entity.Property(e => e.CrmSourceId).HasColumnName("CRM_SourceID");

                entity.Property(e => e.CrmStatusId).HasColumnName("CRM_StatusID");

                entity.Property(e => e.CrmSubjectId).HasColumnName("CRM_SubjectID");

                entity.Property(e => e.CrmTypeId).HasColumnName("CRM_TypeID");

                entity.Property(e => e.CrnEssence)
                    .HasColumnName("CRN_Essence")
                    .HasMaxLength(250);

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeNum).HasMaxLength(50);

                entity.Property(e => e.ExternalCaseId)
                    .HasColumnName("ExternalCaseID")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(16);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.OpIdCreate).HasColumnName("OpId_Create");

                entity.Property(e => e.OpIdCurrent).HasColumnName("OpId_Current");

                entity.Property(e => e.OrderConfirmation)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrderGuid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PopulationType).HasMaxLength(50);

                entity.Property(e => e.Tier).HasColumnName("tier");
            });

            modelBuilder.Entity<CrmDataTempToDelete>(entity =>
            {
                entity.HasKey(e => e.CrmId)
                    .HasName("PK_CRM1");

                entity.ToTable("CRM_Data_temp_to_delete");

                entity.Property(e => e.CrmId).HasColumnName("CrmID");

                entity.Property(e => e.CrmDescription)
                    .IsRequired()
                    .HasColumnName("CRM_Description");

                entity.Property(e => e.CrmResult).HasColumnName("CRM_Result");

                entity.Property(e => e.CrmSeverityId).HasColumnName("CRM_SeverityID");

                entity.Property(e => e.CrmSourceId).HasColumnName("CRM_SourceID");

                entity.Property(e => e.CrmStatusId).HasColumnName("CRM_StatusID");

                entity.Property(e => e.CrmSubjectId).HasColumnName("CRM_SubjectID");

                entity.Property(e => e.CrmTypeId).HasColumnName("CRM_TypeID");

                entity.Property(e => e.CrnEssence)
                    .HasColumnName("CRN_Essence")
                    .HasMaxLength(250);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeNum).HasMaxLength(50);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(16);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.OpIdCreate).HasColumnName("OpId_Create");

                entity.Property(e => e.OpIdCurrent).HasColumnName("OpId_Current");

                entity.Property(e => e.PopulationType).HasMaxLength(50);
            });

            modelBuilder.Entity<CrmDetails>(entity =>
            {
                entity.ToTable("CRM_Details");

                entity.HasIndex(e => e.CrmId)
                    .HasName("<Name of Missing Index, sysname,>");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CrmId).HasColumnName("CrmID");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.OpId).HasColumnName("OpID");
            });

            modelBuilder.Entity<CrmHistory>(entity =>
            {
                entity.ToTable("CRM_History");

                entity.HasIndex(e => e.CrmId)
                    .HasName("IX_CRM_HISTORY_CrmID");

                entity.Property(e => e.CrmHistoryId).HasColumnName("CRM_HistoryID");

                entity.Property(e => e.BusinessIdRelated).HasColumnName("BusinessId_related");

                entity.Property(e => e.CrmId).HasColumnName("CrmID");

                entity.Property(e => e.CrmResult).HasColumnName("CRM_Result");

                entity.Property(e => e.CrmSeverityId).HasColumnName("CRM_SeverityID");

                entity.Property(e => e.CrmStatusId).HasColumnName("CRM_StatusID");

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.OpIdCurrent).HasColumnName("OpId_Current");

                entity.Property(e => e.OpIdUpdate).HasColumnName("OpId_Update");

                entity.Property(e => e.Tier).HasColumnName("tier");

                entity.Property(e => e.XmlDetails).HasColumnType("xml");
            });

            modelBuilder.Entity<CrmOld>(entity =>
            {
                entity.HasKey(e => e.CrmId);

                entity.ToTable("CRM_Old");

                entity.Property(e => e.CrmId).HasColumnName("CrmID");

                entity.Property(e => e.CrmDescription)
                    .IsRequired()
                    .HasColumnName("CRM_Description");

                entity.Property(e => e.CrmResult).HasColumnName("CRM_Result");

                entity.Property(e => e.CrmSeverityId).HasColumnName("CRM_SeverityID");

                entity.Property(e => e.CrmSourceId).HasColumnName("CRM_SourceID");

                entity.Property(e => e.CrmStatusId).HasColumnName("CRM_StatusID");

                entity.Property(e => e.CrmSubjectId).HasColumnName("CRM_SubjectID");

                entity.Property(e => e.CrmTypeId).HasColumnName("CRM_TypeID");

                entity.Property(e => e.CrnEssence)
                    .HasColumnName("CRN_Essence")
                    .HasMaxLength(250);

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeNum).HasMaxLength(50);

                entity.Property(e => e.EventId)
                    .HasColumnName("eventID")
                    .HasMaxLength(50);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(16);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.OpIdCreate).HasColumnName("OpId_Create");

                entity.Property(e => e.OpIdCurrent).HasColumnName("OpId_Current");

                entity.Property(e => e.PopulationType).HasMaxLength(50);
            });

            modelBuilder.Entity<CrmTypeOrg>(entity =>
            {
                entity.ToTable("CRM_TypeOrg");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CrmTypeId).HasColumnName("CrmTypeID");
            });

            modelBuilder.Entity<DbgenericStructure>(entity =>
            {
                entity.HasKey(e => e.Counter);

                entity.ToTable("DBGenericStructure");

                entity.Property(e => e.Counter).HasColumnName("counter");

                entity.Property(e => e.AllowNullColumn).HasColumnName("allowNullColumn");

                entity.Property(e => e.ConstraintColumn)
                    .HasColumnName("constraintColumn")
                    .HasMaxLength(50);

                entity.Property(e => e.DefaultValueColumn)
                    .HasColumnName("defaultValueColumn")
                    .HasMaxLength(100);

                entity.Property(e => e.IndentityIncrementColumn).HasColumnName("indentityIncrementColumn");

                entity.Property(e => e.IndentitySeedColumn).HasColumnName("indentitySeedColumn");

                entity.Property(e => e.NameColumn)
                    .HasColumnName("nameColumn")
                    .HasMaxLength(50);

                entity.Property(e => e.NameItem)
                    .HasColumnName("nameItem")
                    .HasMaxLength(50);

                entity.Property(e => e.SizeColumn).HasColumnName("sizeColumn");

                entity.Property(e => e.TextViewAndSp)
                    .HasColumnName("textViewAndSP")
                    .HasColumnType("ntext");

                entity.Property(e => e.TypeColumn)
                    .HasColumnName("typeColumn")
                    .HasMaxLength(20);

                entity.Property(e => e.TypeItem)
                    .HasColumnName("typeItem")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DebugLog>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Value1).HasMaxLength(4000);

                entity.Property(e => e.Value2).HasMaxLength(4000);
            });

            modelBuilder.Entity<DebugMessages>(entity =>
            {
                entity.HasKey(e => e.DebugId);

                entity.Property(e => e.DebugId).HasColumnName("DebugID");

                entity.Property(e => e.DebugMessage).HasColumnType("ntext");

                entity.Property(e => e.DebugOrganizationId).HasColumnName("DebugOrganizationID");

                entity.Property(e => e.DebugQuary).HasColumnType("ntext");

                entity.Property(e => e.DebugTime).HasColumnType("datetime");

                entity.Property(e => e.MoreData).HasColumnType("ntext");

                entity.Property(e => e.Sqlfailed)
                    .HasColumnName("SQLFailed")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<DtsNewsletter>(entity =>
            {
                entity.HasKey(e => e.LinkCode);

                entity.HasIndex(e => e.CardNumber);

                entity.HasIndex(e => e.OrderId)
                    .HasName("idx_Nonclustered_DtsNewsletter_OrderId");

                entity.HasIndex(e => new { e.OrderId, e.OpenDate });

                entity.Property(e => e.LinkCode)
                    .HasMaxLength(250)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivationCode).HasMaxLength(50);

                entity.Property(e => e.ActivationDate).HasColumnType("datetime");

                entity.Property(e => e.CardNumber)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.OpenDate).HasColumnType("datetime");

                entity.Property(e => e.XmlDetails)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<DtsWorkers>(entity =>
            {
                entity.HasKey(e => e.Fullname)
                    .HasName("PK__DtsWorke__B6AFA08BF2B09295");

                entity.Property(e => e.Fullname)
                    .HasColumnName("FULLNAME")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Cell)
                    .HasColumnName("CELL")
                    .HasMaxLength(50);

                entity.Property(e => e.Company)
                    .HasColumnName("COMPANY")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(54);

                entity.Property(e => e.Shorttel)
                    .HasColumnName("SHORTTEL")
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<EmailQueue>(entity =>
            {
                entity.HasKey(e => e.EmailId)
                    .HasName("PK_EmailQueue_NEW_New");

                entity.HasIndex(e => new { e.EmailType, e.PaymentId })
                    .HasName("IX_EmailQueue__EmailType_PaymentID");

                entity.HasIndex(e => new { e.IsSendEmail, e.SeveralAttempts });

                entity.HasIndex(e => new { e.EmailTo, e.EmailId, e.EmailDateAdded, e.EmailType })
                    .HasName("IX_EmailQueue_EmailType");

                entity.HasIndex(e => new { e.EmailId, e.EmailCc, e.Attachment, e.EmailFrom, e.EmailTo, e.EmailSubject, e.EmailBody, e.IsBodyHtml, e.EmailBcc, e.IsSendEmail, e.SeveralAttempts })
                    .HasName("ix_EmailQueue_IsSendEmail_SeveralAttempts_i_many");

                entity.Property(e => e.EmailId).HasColumnName("EmailID");

                entity.Property(e => e.Attachment).HasColumnType("nvarchar(max)");

                entity.Property(e => e.EmailBcc)
                    .HasColumnName("EmailBCC")
                    .HasMaxLength(200);

                entity.Property(e => e.EmailBody)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.EmailCc)
                    .HasColumnName("EmailCC")
                    .HasMaxLength(200);

                entity.Property(e => e.EmailDateAdded).HasColumnType("datetime");

                entity.Property(e => e.EmailFrom)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.EmailSendDate).HasColumnType("datetime");

                entity.Property(e => e.EmailSubject)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.EmailTo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsBodyHtml).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSendEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.SeveralAttempts).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<EmailQueueArchive>(entity =>
            {
                entity.HasKey(e => e.EmailId)
                    .HasName("PK_EmailQueue_NEW");

                entity.ToTable("EmailQueue_Archive");

                entity.HasIndex(e => new { e.EmailType, e.PaymentId })
                    .HasName("IX_EmailQueue__EmailType_PaymentID");

                entity.HasIndex(e => new { e.IsSendEmail, e.SeveralAttempts })
                    .HasName("IX_EmailQueue_IsSendEmail_SeveralAttempts");

                entity.HasIndex(e => new { e.EmailId, e.EmailDateAdded, e.EmailTo, e.EmailType })
                    .HasName("IX_EmailQueue_EmailType");

                entity.Property(e => e.EmailId).HasColumnName("EmailID");

                entity.Property(e => e.EmailBcc)
                    .HasColumnName("EmailBCC")
                    .HasMaxLength(200);

                entity.Property(e => e.EmailBody).IsRequired();

                entity.Property(e => e.EmailCc)
                    .HasColumnName("EmailCC")
                    .HasMaxLength(200);

                entity.Property(e => e.EmailDateAdded).HasColumnType("datetime");

                entity.Property(e => e.EmailFrom)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.EmailSendDate).HasColumnType("datetime");

                entity.Property(e => e.EmailSubject).IsRequired();

                entity.Property(e => e.EmailTo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsBodyHtml).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSendEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.SeveralAttempts).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<EmailSubscriptionActivity>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");
            });

            modelBuilder.Entity<EmailSubscriptionTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.ToTable("Error_Log");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).HasMaxLength(4000);

                entity.Property(e => e.ErrorProcedure).HasMaxLength(128);
            });

            modelBuilder.Entity<EventimCancellations>(entity =>
            {
                entity.HasKey(e => e.EventimCancelId)
                    .HasName("PK__EventimC__500F4E55536E27D9");

                entity.HasIndex(e => new { e.EventimCancellationId, e.DateCanceled, e.Status, e.DateApproved, e.EventimOrderId })
                    .HasName("IX_EventimCancellations_EventimOrderID");

                entity.Property(e => e.EventimCancelId).HasColumnName("EventimCancelID");

                entity.Property(e => e.DateApproved).HasColumnType("datetime");

                entity.Property(e => e.DateCanceled).HasColumnType("datetime");

                entity.Property(e => e.EventimCancellationId).HasColumnName("EventimCancellationID");

                entity.Property(e => e.EventimOrderId).HasColumnName("EventimOrderID");

                entity.Property(e => e.Opid).HasColumnName("OPId");
            });

            modelBuilder.Entity<EventimCorruptedMediaRecords>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.Property(e => e.EventimFileId).HasColumnName("EventimFileID");

                entity.Property(e => e.MediaRecordErrorCode).HasMaxLength(50);

                entity.Property(e => e.RawText)
                    .HasColumnName("rawText")
                    .HasColumnType("ntext");

                entity.Property(e => e.RecordDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventimMediaFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.HasIndex(e => e.FileName);

                entity.HasIndex(e => new { e.StartLoadDate, e.FileName })
                    .HasName("IX_ventimMediaFile_FileName");

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.EndLoadDate).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(150);

                entity.Property(e => e.StartLoadDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventimMediaRecords>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.HasIndex(e => new { e.MemberId, e.OrgId });

                entity.HasIndex(e => new { e.TicketId, e.InsertDate })
                    .HasName("IX_EventimMediaRecords_InsertDate");

                entity.Property(e => e.TicketId)
                    .HasColumnName("TicketID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AffiliateCode)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardOwnerId)
                    .IsRequired()
                    .HasColumnName("CardOwnerID")
                    .HasMaxLength(9);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EventimBuisnessName).HasMaxLength(255);

                entity.Property(e => e.EventimBusinessId).HasColumnName("EventimBusinessID");

                entity.Property(e => e.EventimFileId).HasColumnName("EventimFileID");

                entity.Property(e => e.EventimMemberId)
                    .IsRequired()
                    .HasColumnName("EventimMemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.Iscampaign).HasColumnName("ISCampaign");

                entity.Property(e => e.K4aeventNo)
                    .HasColumnName("K4AEventNo")
                    .HasMaxLength(100);

                entity.Property(e => e.LastImplementationDate).HasColumnType("datetime");

                entity.Property(e => e.MarketingCommission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MemberFirstName).HasMaxLength(50);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MemberLastName).HasMaxLength(50);

                entity.Property(e => e.MobilePhone).HasMaxLength(30);

                entity.Property(e => e.OrderDateExe)
                    .HasColumnName("OrderDateEXE")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderDeliveryFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrderDeliveryFeeName).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrgDbname)
                    .HasColumnName("OrgDBName")
                    .HasMaxLength(50);

                entity.Property(e => e.OrgName).HasMaxLength(50);

                entity.Property(e => e.OriginalTicketId).HasColumnName("OriginalTicketID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(30);

                entity.Property(e => e.PriceLevelName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PromotionCodeText)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.PromotionText)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Row).HasMaxLength(11);

                entity.Property(e => e.Seat).HasMaxLength(11);

                entity.Property(e => e.SectionCode)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.SectionName).HasMaxLength(50);

                entity.Property(e => e.SellEndDate).HasColumnType("datetime");

                entity.Property(e => e.ShortNameVar).HasMaxLength(250);

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.Property(e => e.StoreEmail).HasMaxLength(100);

                entity.Property(e => e.TheaterName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TicketFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TicketType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TtransactionDateTime)
                    .HasColumnName("TTransactionDateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TtransactionOrder).HasColumnName("TTransactionOrder");

                entity.Property(e => e.TtransactionQuantity).HasColumnName("TTransactionQuantity");

                entity.Property(e => e.Tz)
                    .HasColumnName("TZ")
                    .HasMaxLength(10);

                entity.Property(e => e.VarName).HasMaxLength(250);

                entity.Property(e => e.Zip)
                    .HasColumnName("ZIP")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<EventimMissingMediaRecords>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.Property(e => e.TicketId)
                    .HasColumnName("TicketID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AffiliateCode)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.BusinessId).HasMaxLength(50);

                entity.Property(e => e.CardOwnerId)
                    .HasColumnName("CardOwnerID")
                    .HasMaxLength(9);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EventimBuisnessName).HasMaxLength(255);

                entity.Property(e => e.EventimBusinessId).HasColumnName("EventimBusinessID");

                entity.Property(e => e.EventimFileId).HasColumnName("EventimFileID");

                entity.Property(e => e.EventimMemberId)
                    .HasColumnName("EventimMemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.Iscampaign).HasColumnName("ISCampaign");

                entity.Property(e => e.K4aeventNo)
                    .HasColumnName("K4AEventNo")
                    .HasMaxLength(100);

                entity.Property(e => e.LastImplementationDate).HasColumnType("datetime");

                entity.Property(e => e.MarketingCommission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MediaRecordErrorCode).HasMaxLength(50);

                entity.Property(e => e.MemberFirstName).HasMaxLength(50);

                entity.Property(e => e.MemberId).HasMaxLength(50);

                entity.Property(e => e.MemberLastName).HasMaxLength(50);

                entity.Property(e => e.MobilePhone).HasMaxLength(30);

                entity.Property(e => e.OrderDateExe)
                    .HasColumnName("OrderDateEXE")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderDeliveryFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrderDeliveryFeeName).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrgDbname)
                    .HasColumnName("OrgDBName")
                    .HasMaxLength(50);

                entity.Property(e => e.OrgName).HasMaxLength(50);

                entity.Property(e => e.OriginalTicketId).HasColumnName("OriginalTicketID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(30);

                entity.Property(e => e.PriceLevelName).HasMaxLength(50);

                entity.Property(e => e.PromotionCodeText).HasMaxLength(100);

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.PromotionText).HasMaxLength(100);

                entity.Property(e => e.Row).HasMaxLength(11);

                entity.Property(e => e.Seat).HasMaxLength(11);

                entity.Property(e => e.SectionCode).HasMaxLength(30);

                entity.Property(e => e.SectionName).HasMaxLength(50);

                entity.Property(e => e.SellEndDate).HasColumnType("datetime");

                entity.Property(e => e.ShortNameVar).HasMaxLength(250);

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.Property(e => e.StoreEmail).HasMaxLength(100);

                entity.Property(e => e.TheaterName).HasMaxLength(50);

                entity.Property(e => e.TicketFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TicketType).HasMaxLength(10);

                entity.Property(e => e.TtransactionDateTime)
                    .HasColumnName("TTransactionDateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TtransactionOrder).HasColumnName("TTransactionOrder");

                entity.Property(e => e.TtransactionQuantity).HasColumnName("TTransactionQuantity");

                entity.Property(e => e.Tz)
                    .HasColumnName("TZ")
                    .HasMaxLength(10);

                entity.Property(e => e.VarName).HasMaxLength(250);

                entity.Property(e => e.Zip)
                    .HasColumnName("ZIP")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<EventimMissingTickets20160301>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.ToTable("EventimMissingTickets_20160301");

                entity.Property(e => e.TicketId).ValueGeneratedNever();
            });

            modelBuilder.Entity<EventimStock>(entity =>
            {
                entity.HasKey(e => new { e.EventimStockId, e.PromotionId });

                entity.HasIndex(e => e.MemberId);

                entity.HasIndex(e => new { e.EventimStockId, e.MemberId });

                entity.Property(e => e.EventimStockId).HasColumnName("EventimStockID");

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.EventimStockNavigation)
                    .WithMany(p => p.EventimStock)
                    .HasForeignKey(d => d.EventimStockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventimSt__Event__4EA972BC");
            });

            modelBuilder.Entity<EventimStockDetails>(entity =>
            {
                entity.HasKey(e => e.EventimStockId)
                    .HasName("PK__EventimS__A439B76A4AD8E1D8");

                entity.Property(e => e.EventimStockId)
                    .HasColumnName("EventimStockID")
                    .ValueGeneratedNever();

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PromoId).HasMaxLength(10);

                entity.Property(e => e.StockName).HasMaxLength(50);
            });

            modelBuilder.Entity<EventimStockFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.EndLoadDate).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(150);

                entity.Property(e => e.StartLoadDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventimStockRecords>(entity =>
            {
                entity.HasKey(e => new { e.EventNo, e.TicketPriceLevelCode, e.Affiliate });

                entity.Property(e => e.Affiliate).HasMaxLength(3);

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EventSalesEndDate).HasColumnType("datetime");

                entity.Property(e => e.EventSalesStartDate).HasColumnType("datetime");

                entity.Property(e => e.EventSeriesId).HasColumnName("EventSeriesID");

                entity.Property(e => e.EventSeriesTitle).HasMaxLength(100);

                entity.Property(e => e.EventTdlid).HasColumnName("EventTDLID");

                entity.Property(e => e.EventimBuisnessName).HasMaxLength(100);

                entity.Property(e => e.EventimBusinessId).HasColumnName("EventimBusinessID");

                entity.Property(e => e.EventimStockFileId).HasColumnName("EventimStockFileID");

                entity.Property(e => e.K4aeventNo)
                    .HasColumnName("K4AEventNo")
                    .HasMaxLength(100);

                entity.Property(e => e.SendReportDate).HasColumnType("datetime");

                entity.Property(e => e.StockId)
                    .HasColumnName("StockID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TicketPriceLevelName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<EventimStockRecords201510270935>(entity =>
            {
                entity.HasKey(e => new { e.EventNo, e.TicketPriceLevelCode, e.Affiliate })
                    .HasName("PK_EventimStockRecords_1");

                entity.ToTable("EventimStockRecords_2015_10_27_0935");

                entity.Property(e => e.Affiliate).HasMaxLength(3);

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EventSalesEndDate).HasColumnType("datetime");

                entity.Property(e => e.EventSalesStartDate).HasColumnType("datetime");

                entity.Property(e => e.EventSeriesId).HasColumnName("EventSeriesID");

                entity.Property(e => e.EventSeriesTitle).HasMaxLength(100);

                entity.Property(e => e.EventTdlid).HasColumnName("EventTDLID");

                entity.Property(e => e.EventimBuisnessName).HasMaxLength(100);

                entity.Property(e => e.EventimBusinessId).HasColumnName("EventimBusinessID");

                entity.Property(e => e.EventimStockFileId).HasColumnName("EventimStockFileID");

                entity.Property(e => e.K4aeventNo)
                    .HasColumnName("K4AEventNo")
                    .HasMaxLength(100);

                entity.Property(e => e.SendReportDate).HasColumnType("datetime");

                entity.Property(e => e.StockId)
                    .HasColumnName("StockID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TicketPriceLevelName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQ");

                entity.Property(e => e.Answer).HasColumnType("ntext");

                entity.Property(e => e.Question).HasColumnType("ntext");
            });

            modelBuilder.Entity<Faqsubjects>(entity =>
            {
                entity.HasKey(e => e.QuestionsSubjectKey);

                entity.ToTable("FAQSubjects");

                entity.Property(e => e.QuestionsSubjectName).HasMaxLength(50);
            });

            modelBuilder.Entity<FileUploadDescription>(entity =>
            {
                entity.HasKey(e => e.FileActionId);

                entity.Property(e => e.FileActionId).ValueGeneratedNever();

                entity.Property(e => e.HtmlContent).HasColumnName("htmlContent");

                entity.Property(e => e.Subject).HasColumnName("subject");
            });

            modelBuilder.Entity<GiftCardOrgansationBusiness>(entity =>
            {
                entity.HasKey(e => new { e.OrganisationName, e.BusinessName })
                    .HasName("PK__GiftCard__F238647C81DD2C93");

                entity.Property(e => e.OrganisationName).HasMaxLength(50);

                entity.Property(e => e.BusinessName).HasMaxLength(255);

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GiftCardOrgansationBusinessOld>(entity =>
            {
                entity.HasKey(e => new { e.OrganisationName, e.BusinessName })
                    .HasName("PK__GiftCard__F238647CBCA43E2A");

                entity.ToTable("GiftCardOrgansationBusiness_OLD");

                entity.Property(e => e.OrganisationName).HasMaxLength(50);

                entity.Property(e => e.BusinessName).HasMaxLength(255);

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GiftcardAppConfig>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HelpDarga>(entity =>
            {
                entity.HasKey(e => e.DargaId);

                entity.Property(e => e.DargaId)
                    .HasColumnName("DargaID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DargaDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HelpMissingRecordsErrors>(entity =>
            {
                entity.HasKey(e => e.MissingMediaRecordErrorCode)
                    .HasName("PK_Help_MissingRecordsErrors");

                entity.Property(e => e.MissingMediaRecordErrorCode).ValueGeneratedNever();

                entity.Property(e => e.MissingRecordErrorDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<HelpSogoodGroup>(entity =>
            {
                entity.HasKey(e => e.SogoodGroupId);

                entity.Property(e => e.SogoodGroupId)
                    .HasColumnName("SogoodGroupID")
                    .ValueGeneratedNever();

                entity.Property(e => e.SogoodGroupDescroption).HasMaxLength(50);
            });

            modelBuilder.Entity<Holydays>(entity =>
            {
                entity.HasKey(e => e.HolydayDate)
                    .HasName("PK__Holydays__87EC3D87CCEB3EA0");

                entity.Property(e => e.HolydayDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<HoursManageProjecTypes>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HoursManageProjects>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ProjectTypeNavigation)
                    .WithMany(p => p.HoursManageProjects)
                    .HasForeignKey(d => d.ProjectType)
                    .HasConstraintName("FK_HoursManageProjects_HoursManageProjecTypes");
            });

            modelBuilder.Entity<HoursManageReport>(entity =>
            {
                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Jira).HasColumnType("ntext");

                entity.Property(e => e.MonthReport).HasColumnType("date");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HoursManageReport)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__HoursMana__UserI__09A70B6E");
            });

            modelBuilder.Entity<HoursManageUsers>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HtmlComponentBranches>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.TextComponentId).HasColumnName("TextComponentID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HtmlComponentSubjects>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<HtmlComponents>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CodeIdentity)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<ImageSliderLocation>(entity =>
            {
                entity.HasIndex(e => e.ImageLocation)
                    .HasName("UQ__ImageSli__CD31EA79ED8D3801")
                    .IsUnique();

                entity.Property(e => e.ImageLocation)
                    .IsRequired()
                    .HasColumnName("image_Location")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<ImageTypes>(entity =>
            {
                entity.HasKey(e => e.ImageTypeId);

                entity.Property(e => e.MaxSizeKb).HasColumnName("MaxSizeKB");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ImagesExtentions>(entity =>
            {
                entity.HasKey(e => e.ExtentionId);

                entity.Property(e => e.Name)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImagesExtentionsToImageTypes>(entity =>
            {
                entity.HasKey(e => new { e.ImageTypeId, e.ImageExtention });

                entity.HasOne(d => d.ImageExtentionNavigation)
                    .WithMany(p => p.ImagesExtentionsToImageTypes)
                    .HasForeignKey(d => d.ImageExtention)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImagesExtentionsToImageTypes_ImagesExtentions");

                entity.HasOne(d => d.ImageType)
                    .WithMany(p => p.ImagesExtentionsToImageTypes)
                    .HasForeignKey(d => d.ImageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImagesExtentionsToImageTypes_ImageTypes");
            });

            modelBuilder.Entity<ImagesSlider>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.ImageActive).HasColumnName("image_active");

                entity.Property(e => e.ImageLink)
                    .HasColumnName("image_link")
                    .HasMaxLength(150);

                entity.Property(e => e.ImageLocation)
                    .HasColumnName("image_Location")
                    .HasMaxLength(250);

                entity.Property(e => e.ImageMessageContext)
                    .HasColumnName("image_messageContext")
                    .HasMaxLength(50);

                entity.Property(e => e.ImageOrder).HasColumnName("image_order");

                entity.Property(e => e.ImageOrg).HasColumnName("image_org");

                entity.Property(e => e.ImageTitle)
                    .HasColumnName("image_title")
                    .HasMaxLength(250);

                entity.Property(e => e.ImageUrlBig)
                    .HasColumnName("image_url_big")
                    .HasMaxLength(150);

                entity.Property(e => e.ImageUrlSmal)
                    .HasColumnName("image_url_smal")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ImagesTypesToOrganizations>(entity =>
            {
                entity.HasKey(e => new { e.ImageTypeId, e.OrganizationId });

                entity.HasOne(d => d.ImageType)
                    .WithMany(p => p.ImagesTypesToOrganizations)
                    .HasForeignKey(d => d.ImageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImagesTypesToOrganizations_ImageTypes");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ImagesTypesToOrganizations)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImagesTypesToOrganizations_Organizations");
            });

            modelBuilder.Entity<ImplamantationMethods>(entity =>
            {
                entity.HasKey(e => e.ImplamantationMethods1);

                entity.Property(e => e.ImplamantationMethods1).HasColumnName("ImplamantationMethods");

                entity.Property(e => e.Imdescription)
                    .HasColumnName("IMDescription")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<InvoiceRequests>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });
			modelBuilder.Entity<InvoiceManagementRequests>(entity =>
			{
				entity.HasKey(e => e.ID);

				entity.Property(e => e.ID)
					.HasColumnName("ID")
					.ValueGeneratedOnAdd();

				entity.Property(e => e.DateAdded)
					.HasColumnName("dateAdded");


				entity.Property(e => e.OrgId)
					.HasColumnName("orgId");

				entity.Property(e => e.StatusId)
					.HasColumnName("statusId");

				entity.Property(e => e.Exception)
					.HasColumnName("exception")
					.HasMaxLength(int.MaxValue);

				entity.Property(e => e.Attempts)
					.HasColumnName("attempts");

				entity.Property(e => e.InvoiceRequest)
					.HasColumnName("invoiceRequest")
					.HasMaxLength(int.MaxValue);

			});

			modelBuilder.Entity<IrgunPriceForSubType>(entity =>
            {
                entity.HasKey(e => new { e.BusinessSubTypeId, e.IrgunPriceVariable });

                entity.Property(e => e.BusinessSubTypeId).HasColumnName("BusinessSubTypeID");

                entity.Property(e => e.IrgunPriceVariable).HasMaxLength(10);

                entity.Property(e => e.Percentage)
                    .IsRequired()
                    .HasColumnName("percentage")
                    .HasMaxLength(500);

                entity.Property(e => e.PercentageTrust)
                    .HasColumnName("percentageTrust")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<KeyValueList>(entity =>
            {
                entity.HasIndex(e => new { e.ListId, e.ListValue })
                    .HasName("IX_ListIdListValue");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ListId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ListText)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ListValue)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<KnowledgeCenter>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Descreption)
                    .HasColumnName("descreption")
                    .IsUnicode(false);

                entity.Property(e => e.Iisname)
                    .HasColumnName("IISname")
                    .IsUnicode(false);

                entity.Property(e => e.Kind).HasColumnName("kind");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .IsUnicode(false);

                entity.Property(e => e.PreProductionSiteUrl)
                    .HasColumnName("preProductionSiteUrl")
                    .IsUnicode(false);

                entity.Property(e => e.ProductionServer)
                    .HasColumnName("productionServer")
                    .IsUnicode(false);

                entity.Property(e => e.ProductionSiteUrl)
                    .HasColumnName("productionSiteUrl")
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName)
                    .HasColumnName("projectName")
                    .IsUnicode(false);

                entity.Property(e => e.SchedulerTime)
                    .HasColumnName("schedulerTime")
                    .IsUnicode(false);

                entity.Property(e => e.SoureControlPath)
                    .HasColumnName("soureControlPath")
                    .IsUnicode(false);

                entity.Property(e => e.TestPassword)
                    .HasColumnName("testPassword")
                    .IsUnicode(false);

                entity.Property(e => e.TestSiteUrl)
                    .HasColumnName("testSiteUrl")
                    .IsUnicode(false);

                entity.Property(e => e.TestUserName)
                    .HasColumnName("testUserName")
                    .IsUnicode(false);

                entity.HasOne(d => d.KindNavigation)
                    .WithMany(p => p.KnowledgeCenter)
                    .HasForeignKey(d => d.Kind)
                    .HasConstraintName("FK_KnowledgeCenter_KnowledgeCenterKinds");
            });

            modelBuilder.Entity<KnowledgeCenterErrors>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("projectID");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.KnowledgeCenterErrors)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KnowledgeCenterErrors_KnowledgeCenter");
            });

            modelBuilder.Entity<KnowledgeCenterFiles>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FileName)
                    .HasColumnName("fileName")
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .HasColumnName("filePath")
                    .IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("projectId");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.KnowledgeCenterFiles)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KnowledgeCenterFiles_KnowledgeCenterFiles");
            });

            modelBuilder.Entity<KnowledgeCenterKinds>(entity =>
            {
                entity.HasKey(e => e.KindId)
                    .HasName("PK_KnowledgeCenterKinds_1");

                entity.Property(e => e.KindId)
                    .HasColumnName("kindID")
                    .ValueGeneratedNever();

                entity.Property(e => e.KindName)
                    .HasColumnName("kindName")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KnowledgeCenterPics>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ErrorId).HasColumnName("errorID");

                entity.Property(e => e.FileName)
                    .HasColumnName("fileName")
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .HasColumnName("filePath")
                    .IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("projectID");

                entity.HasOne(d => d.Error)
                    .WithMany(p => p.KnowledgeCenterPics)
                    .HasForeignKey(d => d.ErrorId)
                    .HasConstraintName("FK_KnowledgeCenterPics_KnowledgeCenterErrors");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.KnowledgeCenterPics)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_KnowledgeCenterPics_KnowledgeCenter");
            });

            modelBuilder.Entity<LcmediaRecords>(entity =>
            {
                entity.HasKey(e => new { e.FileName, e.MerchantNumber, e.Branch });

                entity.ToTable("LCMediaRecords");

                entity.Property(e => e.FileName).HasMaxLength(50);

                entity.Property(e => e.Branch).HasMaxLength(10);

                entity.Property(e => e.Amount).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.BussinessName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CommissionAfterVat)
                    .HasColumnName("CommissionAfterVAT")
                    .HasColumnType("decimal(9, 2)");

                entity.Property(e => e.CommissionBeforeVat)
                    .HasColumnName("CommissionBeforeVAT")
                    .HasColumnType("decimal(9, 2)");

                entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.DiscountOther).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.InsertTime).HasColumnType("datetime");

                entity.Property(e => e.Vat)
                    .HasColumnName("VAT")
                    .HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<LcmsOrgs>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);

                entity.ToTable("LCMS_Orgs");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("OrganizationID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<LeumiCardActivationReport>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("OrganizationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LeumiCardBinList>(entity =>
            {
                entity.Property(e => e.Bin)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<LeumiCardErrors>(entity =>
            {
                entity.HasKey(e => e.ErrCode);

                entity.Property(e => e.ErrDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LeumiCardFiles>(entity =>
            {
                entity.HasKey(e => e.LeumiCardFileId);

                entity.Property(e => e.LeumiCardFileId).HasColumnName("LeumiCardFileID");

                entity.Property(e => e.ActualLoadTime).HasColumnType("datetime");

                entity.Property(e => e.ActualLoadTimeRemark).HasColumnType("ntext");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CreatedServerIp)
                    .HasColumnName("CreatedServerIP")
                    .HasMaxLength(15);

                entity.Property(e => e.FileCreated).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MashovCheckedServerIp)
                    .HasColumnName("MashovCheckedServerIP")
                    .HasMaxLength(15);

                entity.Property(e => e.MashovCheckedTime).HasColumnType("datetime");

                entity.Property(e => e.MashovRemark).HasColumnType("ntext");

                entity.Property(e => e.MashovStatus).HasMaxLength(3);

                entity.Property(e => e.MashovTime).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.OriginalFileId).HasColumnName("OriginalFileID");
            });

            modelBuilder.Entity<LeumiCardMedia>(entity =>
            {
                entity.HasKey(e => e.DtsfileId)
                    .HasName("PK_LeumiCardMedia_1");

                entity.Property(e => e.DtsfileId).HasColumnName("DTSFileID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TimeFinishRead).HasColumnType("datetime");

                entity.Property(e => e.TimeRead).HasColumnType("datetime");
            });

            modelBuilder.Entity<LoadFiles>(entity =>
            {
                entity.Property(e => e.BackupErrorFiles).HasDefaultValueSql("((0))");

                entity.Property(e => e.CheckFieldsCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Filter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Header).HasDefaultValueSql("((1))");

                entity.Property(e => e.HistoryFolder)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.LoadAllOnly).HasDefaultValueSql("((1))");

                entity.Property(e => e.Path)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RunSpWhenFinished)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.SplitChar)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateInsertDate)
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LoadFilesFields>(entity =>
            {
                entity.Property(e => e.FieldName)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.IsPrimaryKey).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<LoadFilesLog>(entity =>
            {
                entity.Property(e => e.FileName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<LoadMoneyIframe>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CardImage).HasMaxLength(250);

                entity.Property(e => e.Description).HasMaxLength(250);
            });

            modelBuilder.Entity<LogGeneral>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasColumnName("___details")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DtCreatedDate)
                    .HasColumnName("dtCreatedDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FEnvId).HasColumnName("fEnvID");

                entity.Property(e => e.FTypeId).HasColumnName("fTypeID");

                entity.Property(e => e.FTypeSubId).HasColumnName("fTypeSubID");

                entity.Property(e => e.FUserTypeId).HasColumnName("fUserTypeID");

                entity.Property(e => e.Ip).HasMaxLength(50);

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.Opid).HasColumnName("OPId");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.StrDetails).HasColumnName("strDetails");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("___type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Url).HasMaxLength(4000);

                entity.Property(e => e.XmlDetails)
                    .HasColumnName("xmlDetails")
                    .HasColumnType("xml");

                entity.HasOne(d => d.FEnv)
                    .WithMany(p => p.LogGeneral)
                    .HasForeignKey(d => d.FEnvId)
                    .HasConstraintName("FK_LogGeneral_LogGeneral_Env");

                entity.HasOne(d => d.FType)
                    .WithMany(p => p.LogGeneral)
                    .HasForeignKey(d => d.FTypeId)
                    .HasConstraintName("FK_LogGeneral_LogGeneral_Type");

                entity.HasOne(d => d.FTypeSub)
                    .WithMany(p => p.LogGeneral)
                    .HasForeignKey(d => d.FTypeSubId)
                    .HasConstraintName("FK_LogGeneral_LogGeneral_TypeSub");

                entity.HasOne(d => d.FUserType)
                    .WithMany(p => p.LogGeneral)
                    .HasForeignKey(d => d.FUserTypeId)
                    .HasConstraintName("FK_LogGeneral_LogGeneral_UserType");

                entity.HasOne(d => d.ResultsCodeNavigation)
                    .WithMany(p => p.LogGeneral)
                    .HasPrincipalKey(p => p.ResultsCode)
                    .HasForeignKey(d => d.ResultsCode)
                    .HasConstraintName("FK_LogGeneral_LogGeneral_ResultsCode");
            });

            modelBuilder.Entity<LogGeneralEnv>(entity =>
            {
                entity.ToTable("LogGeneral_Env");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EnvName).HasMaxLength(50);
            });

            modelBuilder.Entity<LogGeneralResultsCode>(entity =>
            {
                entity.ToTable("LogGeneral_ResultsCode");

                entity.HasIndex(e => e.ResultsCode)
                    .HasName("IX_LogGeneral_ResultsCode__ResultsCode")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayText).HasMaxLength(50);

                entity.Property(e => e.ResultsDescription).HasMaxLength(4000);

                entity.Property(e => e.ResultsName).HasMaxLength(50);
            });

            modelBuilder.Entity<LogGeneralType>(entity =>
            {
                entity.ToTable("LogGeneral_Type");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<LogGeneralTypeSub>(entity =>
            {
                entity.ToTable("LogGeneral_TypeSub");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FTypeId).HasColumnName("fTypeID");

                entity.Property(e => e.TypeSubName).HasMaxLength(50);

                entity.HasOne(d => d.FType)
                    .WithMany(p => p.LogGeneralTypeSub)
                    .HasForeignKey(d => d.FTypeId)
                    .HasConstraintName("FK_LogGeneral_TypeSub_LogGeneral_Type");
            });

            modelBuilder.Entity<LogGeneralUserType>(entity =>
            {
                entity.ToTable("LogGeneral_UserType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<LoginIp>(entity =>
            {
                entity.HasKey(e => e.Ip);

                entity.ToTable("LoginIP");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<ManagementBusinessId>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("ManagementBusinessID");

                entity.Property(e => e.TypeId).ValueGeneratedNever();

                entity.Property(e => e.HigthBuisnessId).HasColumnName("HigthBuisnessID");

                entity.Property(e => e.LowBuisnessId).HasColumnName("LowBuisnessID");
            });

            modelBuilder.Entity<ManualOps>(entity =>
            {
                entity.HasKey(e => e.Counter);

                entity.Property(e => e.Counter)
                    .HasColumnName("counter")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExcludeOrganizationId).HasColumnName("ExcludeOrganizationID");

                entity.Property(e => e.IdItem).HasColumnName("idItem");

                entity.Property(e => e.IdParenth).HasColumnName("idParenth");

                entity.Property(e => e.NameItem)
                    .HasColumnName("nameItem")
                    .HasMaxLength(100);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.TableXmlParams).HasMaxLength(100);

                entity.Property(e => e.ValueItem).HasColumnName("valueItem");
            });

            modelBuilder.Entity<ManualStatusTable>(entity =>
            {
                entity.HasKey(e => e.ManualStatusId);

                entity.Property(e => e.ManualStatusId)
                    .HasColumnName("ManualStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ManualStatusDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<MemberMessages>(entity =>
            {
                entity.HasKey(e => e.MemberMessageId);
            });

            modelBuilder.Entity<MembershipFeeTypes>(entity =>
            {
                entity.HasKey(e => e.MembershipFeeType);

                entity.Property(e => e.MembershipFeeType).ValueGeneratedNever();

                entity.Property(e => e.MembershipFeeName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<MerchantOrganizationExclude>(entity =>
            {
                entity.HasKey(e => new { e.OrganizationId, e.MerchantId });

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");
            });

            modelBuilder.Entity<MerchantPoss>(entity =>
            {
                entity.HasKey(e => e.Posid)
                    .HasName("PK_MerchantPOSs_new1");

                entity.ToTable("MerchantPOSs");

                entity.HasIndex(e => new { e.PosTerminalId, e.Active, e.MerchantUser, e.MerchantPassword });

                entity.Property(e => e.Posid).HasColumnName("POSID");

                entity.Property(e => e.Area).HasMaxLength(50);

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.MerchantPassword).HasMaxLength(50);

                entity.Property(e => e.MerchantUser).HasMaxLength(50);

                entity.Property(e => e.OpeningSchedule).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.PosAddress).HasMaxLength(100);

                entity.Property(e => e.PosName).HasMaxLength(50);

                entity.Property(e => e.PosPhone)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.PosTerminalId)
                    .HasColumnName("PosTerminalID")
                    .HasMaxLength(50);

                entity.Property(e => e.PrinterNum).HasMaxLength(250);

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<MerchantPossOld>(entity =>
            {
                entity.HasKey(e => e.Posid)
                    .HasName("PK_MerchantPOSs");

                entity.ToTable("MerchantPOSs_Old");

                entity.Property(e => e.Posid).HasColumnName("POSID");

                entity.Property(e => e.Area).HasMaxLength(50);

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.MerchantPassword).HasMaxLength(50);

                entity.Property(e => e.MerchantUser).HasMaxLength(50);

                entity.Property(e => e.OpeningSchedule).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.PosAddress).HasMaxLength(100);

                entity.Property(e => e.PosName).HasMaxLength(50);

                entity.Property(e => e.PosPhone)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.PosTerminalId)
                    .HasColumnName("PosTerminalID")
                    .HasMaxLength(10);

                entity.Property(e => e.PrinterNum).HasMaxLength(250);

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<MerchantTypes>(entity =>
            {
                entity.HasKey(e => e.MerchantTypeId);

                entity.Property(e => e.MerchantTypeId)
                    .HasColumnName("MerchantTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.MerchantTypeName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Merchants>(entity =>
            {
                entity.HasKey(e => e.MerchantId);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.GazitMerchantId)
                    .HasColumnName("GazitMerchantID")
                    .HasMaxLength(10);

                entity.Property(e => e.MerchantName).HasMaxLength(30);

                entity.Property(e => e.Theme)
                    .HasColumnName("theme")
                    .HasMaxLength(50);

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<MessageCategories>(entity =>
            {
                entity.HasKey(e => new { e.MessageId, e.OrganizationId, e.CategoryNumber });

                entity.HasIndex(e => new { e.OrganizationId, e.CategoryNumber });

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<MessageRules>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__MessageR__FFEE7431201F2880");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK_Messages4");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.AdminDescription).HasColumnType("ntext");

                entity.Property(e => e.MessageContext).HasMaxLength(50);

                entity.Property(e => e.MessageName).HasMaxLength(50);

                entity.Property(e => e.MessageText).HasColumnType("ntext");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<MoneyTypes>(entity =>
            {
                entity.HasKey(e => e.MoneyId);

                entity.Property(e => e.MoneyId).HasColumnName("MoneyID");

                entity.Property(e => e.MoneyType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MultipassExceptionOnCardid>(entity =>
            {
                entity.HasIndex(e => e.CardId)
                    .HasName("idx_MultipassExceptionOnCardid_CarsId");

                entity.Property(e => e.CouponDateCanceled).HasColumnType("datetime");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.Dbname)
                    .HasColumnName("DBName")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LockId)
                    .HasColumnName("lockId")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NetName).HasMaxLength(128);

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.Remark)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierName).HasMaxLength(128);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MultipassLastCardIdRealization>(entity =>
            {
                entity.HasIndex(e => e.CardId)
                    .HasName("MultipassLastCardIdRealization_UQ")
                    .IsUnique();

                entity.Property(e => e.CouponDateCanceled).HasColumnType("datetime");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.Dbname)
                    .HasColumnName("DBName")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LockId)
                    .HasColumnName("lockId")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NetName).HasMaxLength(128);

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.Remark)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierName).HasMaxLength(128);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MultipassTransactionsLog>(entity =>
            {
                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastOperationDate).HasColumnType("datetime");

                entity.Property(e => e.ToDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MultipassTransactionsMedia>(entity =>
            {
                entity.HasIndex(e => new { e.TranId, e.OperationType })
                    .HasName("MultipassTransactionsMedia_UQ")
                    .IsUnique();

                entity.HasIndex(e => new { e.UpdateStatus, e.CardId, e.OperationType, e.OperationDate })
                    .HasName("IX_MultipassTransactionsMedia_CardId_OperationType_OperationDate");

                entity.HasIndex(e => new { e.UpdateStatus, e.LockId, e.InsertDate, e.SupplierId, e.SupplierName, e.NetId, e.NetName, e.TranId, e.Usedbenefit, e.Id, e.MultipassTransactionsLogId, e.OperationType, e.CardId, e.OperationDate })
                    .HasName("IX_MultipassTransactionsMedia_OperationDate_IncludeAllColumn");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LockId)
                    .HasColumnName("lockId")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NetName).HasMaxLength(128);

                entity.Property(e => e.OperationDate).HasColumnType("datetime");

                entity.Property(e => e.SupplierName).HasMaxLength(128);
            });

            modelBuilder.Entity<MwcActivityTypes>(entity =>
            {
                entity.HasKey(e => e.ActivityCode);

                entity.Property(e => e.ActivityCode).ValueGeneratedNever();

                entity.Property(e => e.ActivityDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ActivityType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MwcCanceled>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MwcClubInfo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClubId).HasColumnName("clubID");

                entity.Property(e => e.FtpHost)
                    .HasColumnName("ftpHost")
                    .HasMaxLength(150);

                entity.Property(e => e.FtpPass)
                    .HasColumnName("ftpPass")
                    .HasMaxLength(20);

                entity.Property(e => e.FtpPort)
                    .HasColumnName("ftpPort")
                    .HasMaxLength(10);

                entity.Property(e => e.FtpUserName)
                    .HasColumnName("ftpUserName")
                    .HasMaxLength(150);

                entity.Property(e => e.Mails)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MwcFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.HasIndex(e => e.FileName);

                entity.HasIndex(e => new { e.FileType, e.StatusFile, e.FileStartUpload, e.FileEndUpload });

                entity.Property(e => e.FileId).HasColumnName("fileId");

                entity.Property(e => e.ClubinfoId).HasColumnName("ClubinfoID");

                entity.Property(e => e.DumpUrl)
                    .HasColumnName("dumpUrl")
                    .IsUnicode(false);

                entity.Property(e => e.FileEndUpload)
                    .HasColumnName("fileEndUpload")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .HasColumnName("fileName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FileSize).HasColumnName("fileSize");

                entity.Property(e => e.FileStartUpload)
                    .HasColumnName("fileStartUpload")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileType).HasColumnName("fileType");

                entity.Property(e => e.NumOfRecords).HasColumnName("numOfRecords");

                entity.Property(e => e.ReportFromDate)
                    .HasColumnName("reportFromDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ReportToDate)
                    .HasColumnName("reportToDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusFile).HasColumnName("statusFile");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("totalAmount")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalRows).HasColumnName("totalRows");
            });

            modelBuilder.Entity<MwcFileStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.NameStatus)
                    .HasColumnName("nameStatus")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusRemark)
                    .HasColumnName("statusRemark")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MwcHanpakaRequestsPrint>(entity =>
            {
                entity.HasKey(e => e.IdIdentity)
                    .HasName("PK__MwcHanpa__C9A0DDC6C4630CF9");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.Error).HasMaxLength(100);

                entity.Property(e => e.FileNameMashov).HasMaxLength(100);

                entity.Property(e => e.FileNameSum).HasMaxLength(100);

                entity.Property(e => e.Filename).HasMaxLength(100);

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.MashovDate).HasColumnType("datetime");

                entity.Property(e => e.MashovId).HasMaxLength(100);
            });

            modelBuilder.Entity<MwcIssuerChains>(entity =>
            {
                entity.HasKey(e => new { e.ChainId, e.IssureId });

                entity.Property(e => e.ChainId).HasColumnName("ChainID");

                entity.Property(e => e.IssureId).HasColumnName("IssureID");
            });

            modelBuilder.Entity<MwcIssuers>(entity =>
            {
                entity.HasKey(e => e.IssuerId);

                entity.Property(e => e.IssuerId)
                    .HasColumnName("IssuerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IssuerDescription).HasMaxLength(255);

                entity.Property(e => e.IssuerName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VerIssuerId).HasColumnName("VerIssuerID");
            });

            modelBuilder.Entity<MwcMedia>(entity =>
            {
                entity.HasKey(e => e.RepTransId)
                    .HasName("PK__MwcMedia__DCC145D86D272F97");

                entity.HasIndex(e => e.ServerDataTime)
                    .HasName("IDX2_serverDataTime");

                entity.HasIndex(e => e.TransactionDateTime)
                    .HasName("IDX_TransactionDateTime");

                entity.HasIndex(e => e.TransactionId)
                    .HasName("UQ__MwcMedia__55433A4AEAC37E5D")
                    .IsUnique();

                entity.HasIndex(e => new { e.CardNumber, e.OrgId, e.ActivityId })
                    .HasName("MwcMedia_CardNumber");

                entity.HasIndex(e => new { e.CardNumber, e.WalletId, e.TransactionDateTime })
                    .HasName("IX_MwcMedia_CardNumber_WalletID_TransactionDateTime");

                entity.HasIndex(e => new { e.Amount, e.CardNumber, e.OrgId, e.ActivityId })
                    .HasName("MwcMedia_Activity");

                entity.HasIndex(e => new { e.Amount, e.CardNumber, e.WalletId, e.OrgId })
                    .HasName("MwcMedia_IX1");

                entity.HasIndex(e => new { e.Amount, e.CardExpirationDate, e.CardNumber, e.OrgId, e.ActivityId })
                    .HasName("MwcMedia_CardNumber_OrgID");

                entity.Property(e => e.RepTransId).HasColumnName("repTransId");

                entity.Property(e => e.ActionCashierId)
                    .HasColumnName("actionCashierId")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionNetworkId)
                    .HasColumnName("actionNetworkID")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionStoreId)
                    .HasColumnName("actionStoreId")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionStorePnumber)
                    .HasColumnName("actionStorePNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionTicketId)
                    .HasColumnName("actionTicketId")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionType)
                    .HasColumnName("actionType")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionVendorPnumber)
                    .HasColumnName("actionVendorPNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.BranchName).HasMaxLength(254);

                entity.Property(e => e.BusinessDateTime).HasColumnType("datetime");

                entity.Property(e => e.CardBatchNumber)
                    .HasColumnName("cardBatchNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnName("cardExpirationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CardIssuerPnumber)
                    .HasColumnName("cardIssuerPNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardIssuerSn)
                    .HasColumnName("cardIssuerSN")
                    .HasMaxLength(50);

                entity.Property(e => e.CardNumber)
                    .HasColumnName("cardNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardSerialNumber)
                    .HasColumnName("cardSerialNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardSeriesNumber)
                    .HasColumnName("cardSeriesNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.ChainName).HasMaxLength(254);

                entity.Property(e => e.CreateRow)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Currency).HasMaxLength(10);

                entity.Property(e => e.DecimalCurrency).HasColumnName("decimalCurrency");

                entity.Property(e => e.ExternalBranchId)
                    .HasColumnName("ExternalBranchID")
                    .HasMaxLength(248)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalInvoiceNumber).HasMaxLength(50);

                entity.Property(e => e.FileId).HasColumnName("fileId");

                entity.Property(e => e.IssuerChainId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LimitChargeWallet).HasColumnType("money");

                entity.Property(e => e.LimitExerciseWallet).HasColumnType("money");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.ProcessDate).HasColumnType("datetime");

                entity.Property(e => e.SerieId).HasColumnName("SerieID");

                entity.Property(e => e.ServerDataTime)
                    .HasColumnName("serverDataTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .IsRequired()
                    .HasColumnName("TransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionAccountActivityId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionCashierId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionNetworkId)
                    .HasColumnName("VoidTransactionActionNetworkID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionStoreId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionStorePnumber)
                    .HasColumnName("VoidTransactionActionStorePNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionTicketId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionType).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionVendorPnumber)
                    .HasColumnName("VoidTransactionActionVendorPNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionId)
                    .HasColumnName("VoidTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionOrderId)
                    .HasColumnName("VoidTransactionOrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionPosDate).HasColumnType("datetime");

                entity.Property(e => e.VoidTransactionServerDateTime).HasColumnType("datetime");

                entity.Property(e => e.WalledExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");
            });

            modelBuilder.Entity<MwcMediaOld>(entity =>
            {
                entity.HasKey(e => e.RepTransId)
                    .HasName("PK_MwcMedia");

                entity.HasIndex(e => e.ServerDataTime)
                    .HasName("IDX2_serverDataTime");

                entity.HasIndex(e => e.TransactionDateTime)
                    .HasName("IDX_TransactionDateTime");

                entity.HasIndex(e => e.TransactionId)
                    .HasName("TransactionID_UQ")
                    .IsUnique();

                entity.HasIndex(e => new { e.CardNumber, e.OrgId, e.ActivityId })
                    .HasName("MwcMedia_CardNumber");

                entity.HasIndex(e => new { e.Amount, e.CardNumber, e.OrgId, e.ActivityId })
                    .HasName("MwcMedia_CardNumber_OrgID");

                entity.HasIndex(e => new { e.Amount, e.CardNumber, e.WalletId, e.OrgId })
                    .HasName("MwcMedia_IX1");

                entity.Property(e => e.RepTransId).HasColumnName("repTransId");

                entity.Property(e => e.ActionCashierId)
                    .HasColumnName("actionCashierId")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionNetworkId)
                    .HasColumnName("actionNetworkID")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionStoreId)
                    .HasColumnName("actionStoreId")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionStorePnumber)
                    .HasColumnName("actionStorePNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionTicketId)
                    .HasColumnName("actionTicketId")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionType)
                    .HasColumnName("actionType")
                    .HasMaxLength(50);

                entity.Property(e => e.ActionVendorPnumber)
                    .HasColumnName("actionVendorPNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.BranchName).HasMaxLength(254);

                entity.Property(e => e.BusinessDateTime).HasColumnType("datetime");

                entity.Property(e => e.CardBatchNumber)
                    .HasColumnName("cardBatchNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnName("cardExpirationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CardIssuerPnumber)
                    .HasColumnName("cardIssuerPNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardIssuerSn)
                    .HasColumnName("cardIssuerSN")
                    .HasMaxLength(50);

                entity.Property(e => e.CardNumber)
                    .HasColumnName("cardNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardSerialNumber)
                    .HasColumnName("cardSerialNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.CardSeriesNumber)
                    .HasColumnName("cardSeriesNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.ChainName).HasMaxLength(254);

                entity.Property(e => e.CreateRow)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Currency).HasMaxLength(10);

                entity.Property(e => e.DecimalCurrency).HasColumnName("decimalCurrency");

                entity.Property(e => e.ExternalBranchId)
                    .HasColumnName("ExternalBranchID")
                    .HasMaxLength(248)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalInvoiceNumber).HasMaxLength(50);

                entity.Property(e => e.FileId).HasColumnName("fileId");

                entity.Property(e => e.LimitChargeWallet).HasColumnType("money");

                entity.Property(e => e.LimitExerciseWallet).HasColumnType("money");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.ProcessDate).HasColumnType("datetime");

                entity.Property(e => e.SerieId).HasColumnName("SerieID");

                entity.Property(e => e.ServerDataTime)
                    .HasColumnName("serverDataTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .IsRequired()
                    .HasColumnName("TransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionAccountActivityId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionCashierId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionNetworkId)
                    .HasColumnName("VoidTransactionActionNetworkID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionStoreId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionStorePnumber)
                    .HasColumnName("VoidTransactionActionStorePNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionTicketId).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionType).HasMaxLength(50);

                entity.Property(e => e.VoidTransactionActionVendorPnumber)
                    .HasColumnName("VoidTransactionActionVendorPNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionId)
                    .HasColumnName("VoidTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionOrderId)
                    .HasColumnName("VoidTransactionOrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionPosDate).HasColumnType("datetime");

                entity.Property(e => e.VoidTransactionServerDateTime).HasColumnType("datetime");

                entity.Property(e => e.WalledExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");
            });

            modelBuilder.Entity<MwcMediaPush>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BranchName).IsRequired();

                entity.Property(e => e.CardNumber).IsRequired();

                entity.Property(e => e.ChainName).IsRequired();

                entity.Property(e => e.MwcdateTime)
                    .IsRequired()
                    .HasColumnName("MWCDateTime");
            });

            modelBuilder.Entity<MwcMethods>(entity =>
            {
                entity.HasKey(e => e.MethodId);

                entity.Property(e => e.MethodId).HasColumnName("MethodID");

                entity.Property(e => e.FromTable)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MethodDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MethodName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MwcNetworkDiscount>(entity =>
            {
                entity.HasKey(e => new { e.ActionNetworkId, e.IssuerId, e.DiscountMonth, e.DiscountYear });

                entity.Property(e => e.ActionNetworkId)
                    .HasColumnName("ActionNetworkID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IssuerId).HasColumnName("IssuerID");

                entity.Property(e => e.DiscountPrecent).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MwcRcn>(entity =>
            {
                entity.HasKey(e => e.RcnId);

                entity.ToTable("MwcRCN");

                entity.Property(e => e.RcnId).HasColumnName("RcnID");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DescriptionRcn)
                    .IsRequired()
                    .HasColumnName("DescriptionRCN")
                    .HasMaxLength(255);

                entity.Property(e => e.ExecuteDate).HasColumnType("datetime");

                entity.Property(e => e.Opid).HasColumnName("OPId");

                entity.Property(e => e.SerieId).HasColumnName("SerieID");

                entity.HasOne(d => d.Serie)
                    .WithMany(p => p.MwcRcn)
                    .HasForeignKey(d => d.SerieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SerieID");
            });

            modelBuilder.Entity<MwcRtypePermission>(entity =>
            {
                entity.HasKey(e => e.Opid);

                entity.Property(e => e.Opid)
                    .HasColumnName("OPId")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<MwcSeries>(entity =>
            {
                entity.HasKey(e => e.SerieId);

                entity.HasIndex(e => e.SerieName)
                    .HasName("UNIQUE_SerieName")
                    .IsUnique();

                entity.HasIndex(e => new { e.IssuerId, e.SerieForIssuerId })
                    .HasName("UNIQUE_SerieForIssuerID")
                    .IsUnique();

                entity.Property(e => e.SerieId).HasColumnName("SerieID");

                entity.Property(e => e.CardsPrefix)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ConstantTemplate)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DescriptionSerie)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.IsShowLongNumberOnReport)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IssuerId).HasColumnName("IssuerID");

                entity.Property(e => e.Opid).HasColumnName("OPId");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SerieForIssuerId).HasColumnName("SerieForIssuerID");

                entity.Property(e => e.SerieName).HasMaxLength(50);

                entity.Property(e => e.VerId).HasColumnName("VerID");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.MwcSeries)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganizationID");
            });

            modelBuilder.Entity<MwcTlogRequest>(entity =>
            {
                entity.HasKey(e => e.Tid);

                entity.ToTable("MwcTLog_Request");

                entity.HasIndex(e => new { e.OriginalRequest, e.MethodId, e.OrganizationId })
                    .HasName("UNIQUE_Log_Request")
                    .IsUnique();

                entity.Property(e => e.Tid).HasColumnName("TID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MethodId).HasColumnName("MethodID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<MwcVpayAppServer>(entity =>
            {
                entity.HasKey(e => e.VapyId);

                entity.Property(e => e.VapyId).HasColumnName("Vapy_ID");

                entity.Property(e => e.VapyDateTimeType).HasColumnName("Vapy_DateTimeType");

                entity.Property(e => e.VapyDescription)
                    .IsRequired()
                    .HasColumnName("Vapy_Description")
                    .HasMaxLength(255);

                entity.Property(e => e.VapyKey)
                    .IsRequired()
                    .HasColumnName("Vapy_Key")
                    .HasMaxLength(50);

                entity.Property(e => e.VapyValue)
                    .IsRequired()
                    .HasColumnName("Vapy_Value")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MwcVpayBranches>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PK__MwcVpay___A1682FA5F8403AE6");

                entity.ToTable("MwcVpay_Branches");

                entity.HasIndex(e => new { e.Name, e.Title, e.ParentChainId })
                    .HasName("IX_MwcVpay_Branches_ParentChainId");

                entity.Property(e => e.BranchId)
                    .HasColumnName("BranchID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<MwcVpayBranchesOld>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PK_MwcGpp_Branches");

                entity.ToTable("MwcVpay_Branches_OLD");

                entity.Property(e => e.BranchId)
                    .HasColumnName("BranchID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<MwcVpayBranchesProp>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PK__MwcVpay___A1682FA53C8AC281");

                entity.ToTable("MwcVpay_BranchesProp");

                entity.Property(e => e.BranchId)
                    .HasColumnName("BranchID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TerminalNo).HasMaxLength(50);
            });

            modelBuilder.Entity<MwcVpayChains>(entity =>
            {
                entity.HasKey(e => e.ChainId)
                    .HasName("PK__MwcVpay___AB20BA8AB1FBF5C1");

                entity.ToTable("MwcVpay_Chains");

                entity.Property(e => e.ChainId)
                    .HasColumnName("ChainID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChainDescription).IsRequired();

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.InstructionUrl).HasMaxLength(255);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<MwcVpayChainsOld>(entity =>
            {
                entity.HasKey(e => e.ChainId)
                    .HasName("PK_MwcGpp_Chains");

                entity.ToTable("MwcVpay_Chains_OLD");

                entity.Property(e => e.ChainId)
                    .HasColumnName("ChainID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChainDescription).IsRequired();

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<MwcVpayChainsProp>(entity =>
            {
                entity.HasKey(e => e.ChainId);

                entity.ToTable("MwcVpay_ChainsProp");

                entity.Property(e => e.ChainId)
                    .HasColumnName("ChainID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);

                entity.Property(e => e.EmailContact).HasMaxLength(50);

                entity.Property(e => e.LogoUrl)
                    .HasColumnName("LogoURL")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.PercentReduction).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.PercentReductionHashmal).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ShufersalLogo)
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MwcVpayEntities>(entity =>
            {
                entity.ToTable("MwcVpay_Entities");

                entity.HasIndex(e => e.WalletId)
                    .HasName("WalletId");

                entity.HasIndex(e => new { e.ObjectType, e.ObjectId })
                    .HasName("ObjectType_ObjectId");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Linked)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MwcVpayLinkedChains>(entity =>
            {
                entity.ToTable("MwcVpay_LinkedChains");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BrancheId).HasColumnName("BrancheID");

                entity.Property(e => e.ChainId).HasColumnName("ChainID");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");
            });

            modelBuilder.Entity<MwcVpayTerminals>(entity =>
            {
                entity.HasKey(e => e.TerminalId)
                    .HasName("PK_MwcGpp_Terminals");

                entity.ToTable("MwcVpay_Terminals");

                entity.Property(e => e.TerminalId)
                    .HasColumnName("TerminalID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Authorizations)
                    .HasColumnName("Authorizations???")
                    .HasMaxLength(10);

                entity.Property(e => e.CreateDate).IsRequired();

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.TerminalDescription).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UniqueIdentifier).IsRequired();
            });

            modelBuilder.Entity<MwcVpayWallets>(entity =>
            {
                entity.HasKey(e => e.WalletId)
                    .HasName("PK_MwcAccounts");

                entity.ToTable("MwcVpay_Wallets");

                entity.Property(e => e.WalletId)
                    .HasColumnName("WalletID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.MaxBalance).HasColumnType("money");

                entity.Property(e => e.MaxDeposit).HasColumnType("money");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Priority)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VerId).HasColumnName("VerID");
            });

            modelBuilder.Entity<MwcVpayWalletsToChains>(entity =>
            {
                entity.HasKey(e => e.Wtcid);

                entity.ToTable("MwcVpay_WalletsToChains");

                entity.HasIndex(e => new { e.WalletId, e.ChainId });

                entity.Property(e => e.Wtcid).HasColumnName("WTCid");

                entity.Property(e => e.ChainId).HasColumnName("ChainID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");
            });

            modelBuilder.Entity<Nets>(entity =>
            {
                entity.HasKey(e => e.NetsCardId);

                entity.Property(e => e.NetsCardId).HasColumnName("Nets_Card_ID");

                entity.Property(e => e.CardTamplate).HasMaxLength(37);

                entity.Property(e => e.Cvv).HasColumnName("CVV");

                entity.Property(e => e.IdcodeTemplate)
                    .HasColumnName("IDCodeTemplate")
                    .HasMaxLength(20);

                entity.Property(e => e.IsFrsactive).HasColumnName("IsFRSActive");

                entity.Property(e => e.IsatrCamp).HasColumnName("ISAtrCamp");

                entity.Property(e => e.IslcCard).HasColumnName("ISLC_Card");

                entity.Property(e => e.Ivrorder).HasColumnName("IVROrder");

                entity.Property(e => e.NetDb)
                    .HasColumnName("NetDB")
                    .HasMaxLength(50);

                entity.Property(e => e.NetId).HasColumnName("NetID");

                entity.Property(e => e.NetLoginData).HasMaxLength(100);

                entity.Property(e => e.NetName).HasMaxLength(110);

                entity.Property(e => e.NetOrdersData).HasMaxLength(100);

                entity.Property(e => e.NetOrdersWebServiceTransaction).HasMaxLength(100);

                entity.Property(e => e.NetShortName).HasMaxLength(24);

                entity.Property(e => e.Rem).HasMaxLength(100);
            });

            modelBuilder.Entity<NetsMultiCardsDb>(entity =>
            {
                entity.HasKey(e => e.NetsDbId);

                entity.ToTable("Nets_MultiCardsDB");

                entity.Property(e => e.NetsDbId).HasColumnName("Nets_DB_ID");

                entity.Property(e => e.NetDb)
                    .IsRequired()
                    .HasColumnName("NetDB")
                    .HasMaxLength(50);

                entity.Property(e => e.NetShortName)
                    .IsRequired()
                    .HasMaxLength(24);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<NetsRelationship>(entity =>
            {
                entity.HasKey(e => new { e.NetsCardId, e.NetsDbId });

                entity.ToTable("Nets_Relationship");

                entity.Property(e => e.NetsCardId).HasColumnName("Nets_Card_ID");

                entity.Property(e => e.NetsDbId).HasColumnName("Nets_DB_ID");

                entity.Property(e => e.PriorityDb).HasColumnName("Priority_DB");
            });

            modelBuilder.Entity<NirshamimBarcode>(entity =>
            {
                entity.HasKey(e => new { e.FullBarCode, e.OrgId });

                entity.HasIndex(e => e.FullBarCode)
                    .HasName("IX_NirshamimBarcode");

                entity.Property(e => e.FullBarCode).HasMaxLength(50);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OpTypes>(entity =>
            {
                entity.HasKey(e => e.OpTypeId);

                entity.Property(e => e.OpTypeId).ValueGeneratedNever();

                entity.Property(e => e.AllowActions).HasMaxLength(1000);

                entity.Property(e => e.AllowBusinessSubTypes).HasMaxLength(100);

                entity.Property(e => e.AllowPages).HasMaxLength(1000);

                entity.Property(e => e.OpTypeDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderQueue>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ExeDate).HasColumnType("datetime");

                entity.Property(e => e.InsertQueueDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.SeveralAttempts).HasDefaultValueSql("((0))");

                entity.Property(e => e.TerminalExe).HasMaxLength(50);
            });

            modelBuilder.Entity<OrdersRemarks>(entity =>
            {
                entity.HasKey(e => e.IndexNum);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<OrganizatinBusinsess>(entity =>
            {
                entity.HasKey(e => new { e.OrgId, e.BuisnessId });

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrgMarketingCommission).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrganizationBalances>(entity =>
            {
                entity.HasKey(e => e.BalanceId);

                entity.Property(e => e.BalanceId).HasColumnName("BalanceID");

                entity.Property(e => e.BalanceName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<OrganizationCategories>(entity =>
            {
                entity.HasIndex(e => e.DisplayName);

                entity.HasIndex(e => new { e.CategoryNumber, e.OrganizationId })
                    .HasName("IX_OrganizationCategories");

                entity.HasIndex(e => new { e.FatherId, e.OrganizationId, e.CategoryNumber })
                    .HasName("IX_OrganizationCategories_MulA");

                entity.HasIndex(e => new { e.CategoryNumber, e.SortOrder, e.OrganizationId, e.ShowInHomePageSlider })
                    .HasName("IX_OrganizationCategories_OrganizationID_ShowInHomePageSlider");

                entity.HasIndex(e => new { e.DisplayName, e.FatherId, e.CategoryNumber, e.OrganizationId })
                    .HasName("IX_OrganizationCategories__FatherID_CategoryNumber_OrganizationID");

                entity.HasIndex(e => new { e.DisplayName, e.OrganizationId, e.CategoryNumber, e.FatherId })
                    .HasName("IX_OrganizationCategories__MulA");

                entity.HasIndex(e => new { e.ExternalIframeAddress, e.ShowOpeningTime, e.ShowExecutionSogood, e.CategoryHeader, e.CategoryDesign, e.CategoryDescription, e.ExternalIframe, e.CategoryNumber, e.SortOrder, e.DisplayName, e.FatherId, e.OrganizationId, e.UserTypes })
                    .HasName("IX_OrganizationCategories_FatherID_OrganizationID_UserTypes");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AllowOverrideDescWithJob).HasDefaultValueSql("((0))");

                entity.Property(e => e.CategoryDescription).HasColumnType("nvarchar(max)");

                entity.Property(e => e.CategoryHeader).HasColumnType("nvarchar(max)");

                entity.Property(e => e.CategoryShowType).HasDefaultValueSql("((0))");

                entity.Property(e => e.DisplayName).HasMaxLength(255);

                entity.Property(e => e.ExternalIframeAddress).HasColumnType("nvarchar(max)");

                entity.Property(e => e.FatherId).HasColumnName("FatherID");

                entity.Property(e => e.IsOutOfStock).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.SortOrder).HasDefaultValueSql("((99))");
            });

            modelBuilder.Entity<OrganizationCategoriesImages>(entity =>
            {
                entity.HasKey(e => e.OrganizationCategoriesImageId);

                entity.HasIndex(e => new { e.PortalCategoriesImageId, e.OrganizationId })
                    .HasName("IDX_OrganizationCategoriesImages1");

                entity.HasIndex(e => new { e.FileName, e.PortalCategoriesImageId, e.OrganizationId })
                    .HasName("IX_OrganizationCategoriesImages_PortalCategoriesImageId_OrganizationId");

                entity.HasIndex(e => new { e.PortalCategoriesImageId, e.FileName, e.OrganizationId })
                    .HasName("IX_OrganizationCategoriesImages_OrganizationId");

                entity.Property(e => e.Alt).HasMaxLength(50);

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.ImageCreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.OrganizationCategoriesImages)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrgId");

                entity.HasOne(d => d.PortalCategoriesImage)
                    .WithMany(p => p.OrganizationCategoriesImages)
                    .HasForeignKey(d => d.PortalCategoriesImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortalCategoriesImageId");
            });

            modelBuilder.Entity<OrganizationHamara>(entity =>
            {
                entity.HasKey(e => e.HamaraId);

                entity.Property(e => e.HamaraId).HasColumnName("HamaraID");

                entity.Property(e => e.BalanceId1).HasColumnName("BalanceID1");

                entity.Property(e => e.BalanceId2).HasColumnName("BalanceID2");
            });

            modelBuilder.Entity<OrganizationMainField>(entity =>
            {
                entity.HasKey(e => e.MainField);

                entity.Property(e => e.MainField).ValueGeneratedNever();

                entity.Property(e => e.MainFieldDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrganizationOps>(entity =>
            {
                entity.HasKey(e => e.Opid);

                entity.ToTable("OrganizationOPs");

                entity.HasIndex(e => e.OpuserName)
                    .HasName("IX_OrganizationOPs")
                    .IsUnique();

                entity.HasIndex(e => new { e.Opid, e.FirstName })
                    .HasName("IX__OrganizationOPs__OPId_FirstName");

                entity.HasIndex(e => new { e.Opid, e.OpuserName, e.FirstName, e.LastName, e.Status, e.AllowCrm })
                    .HasName("IX_OrganizationOPs_Status_AllowCrm");

                entity.Property(e => e.Opid).HasColumnName("OPId");

                entity.Property(e => e.DateAdded)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.FirstName).HasMaxLength(20);

                entity.Property(e => e.IsCustomerServiceManager).HasColumnName("isCustomerServiceManager");

                entity.Property(e => e.IsLoadFiles).HasColumnName("isLoadFiles");

                entity.Property(e => e.IsMultiClub).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsUpdate).HasColumnName("isUpdate");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(20);

                entity.Property(e => e.MobilePhone).HasMaxLength(10);

                entity.Property(e => e.OprealId)
                    .IsRequired()
                    .HasColumnName("OPRealID")
                    .HasMaxLength(9);

                entity.Property(e => e.Optype)
                    .HasColumnName("OPType")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OpuserName)
                    .IsRequired()
                    .HasColumnName("OPUserName")
                    .HasMaxLength(50);

                entity.Property(e => e.OpuserPassword)
                    .IsRequired()
                    .HasColumnName("OPUserPassword")
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.Remark).HasColumnType("ntext");

                entity.Property(e => e.Xmlparams)
                    .HasColumnName("XMLParams")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<OrganizationRedimTypes>(entity =>
            {
                entity.HasKey(e => e.OrgRedimTypeId);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.OrganizationRedimTypes)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RedimOrganizationId");

                entity.HasOne(d => d.RedimType)
                    .WithMany(p => p.OrganizationRedimTypes)
                    .HasForeignKey(d => d.RedimTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RedimTypeId");
            });

            modelBuilder.Entity<OrganizationUsers>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK_OrganizationUsers_1");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Iplist)
                    .IsRequired()
                    .HasColumnName("IPList")
                    .IsUnicode(false);

                entity.Property(e => e.OrgUserId)
                    .HasColumnName("OrgUserID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Organizations>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("OrganizationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivationSupportPhone).HasMaxLength(15);

                entity.Property(e => e.AllowNloginCardBalance)
                    .HasColumnName("AllowNLoginCardBalance")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AllowedCardNumberByTz)
                    .HasColumnName("AllowedCardNumberByTZ")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CancelingCommission).HasDefaultValueSql("((5))");

                entity.Property(e => e.CancellingCommisionTopAmount).HasDefaultValueSql("((100))");

                entity.Property(e => e.CardPicturePath).HasMaxLength(100);

                entity.Property(e => e.ClubTitle).HasMaxLength(50);

                entity.Property(e => e.DaysForRefound).HasDefaultValueSql("((14))");

                entity.Property(e => e.Dbname)
                    .HasColumnName("DBName")
                    .HasMaxLength(50);

                entity.Property(e => e.DefaultPrintPattern).HasColumnType("text");

                entity.Property(e => e.DefaultPrintPatternShow).HasColumnType("text");

                entity.Property(e => e.DefaultVarComissionFormula).HasColumnType("text");

                entity.Property(e => e.DefaultVarDiscountFormula).HasColumnType("text");

                entity.Property(e => e.DefaultVarPriceFormula).HasColumnType("text");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EndUserSupportPhone).HasMaxLength(10);

                entity.Property(e => e.FirstActivity).HasColumnType("datetime");

                entity.Property(e => e.ForceShortMarketingDescription).HasDefaultValueSql("((0))");

                entity.Property(e => e.GlobalSms)
                    .HasColumnName("GlobalSMS")
                    .HasMaxLength(70);

                entity.Property(e => e.GoogleAnalytics).HasColumnType("ntext");

                entity.Property(e => e.GoogleAnalyticsLogin).HasColumnType("ntext");

                entity.Property(e => e.InfoMail).HasMaxLength(50);

                entity.Property(e => e.IssuerId).HasColumnName("IssuerID");

                entity.Property(e => e.LastHanpakaDate).HasColumnType("datetime");

                entity.Property(e => e.LeumiCardMediaFile).HasMaxLength(10);

                entity.Property(e => e.LeumiCardMediaFileNewVersion)
                    .HasColumnName("LeumiCardMediaFile_NewVersion")
                    .HasMaxLength(50);

                entity.Property(e => e.LeumiCardPrefix).HasMaxLength(5);

                entity.Property(e => e.LimitsIp).HasColumnName("LimitsIP");

                entity.Property(e => e.LoadMoneyTerminalNumber).HasMaxLength(10);

                entity.Property(e => e.ManagerMobile).HasMaxLength(10);

                entity.Property(e => e.ManagerName).HasMaxLength(20);

                entity.Property(e => e.ManagerPhone).HasMaxLength(10);

                entity.Property(e => e.ManagerTitle).HasMaxLength(20);

                entity.Property(e => e.MaxOtb)
                    .HasColumnName("MaxOTB")
                    .HasColumnType("money");

                entity.Property(e => e.MaxPurchase).HasColumnType("money");

                entity.Property(e => e.MaxSumInCard).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MaxSumInCardForMonth)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MemberSms)
                    .HasColumnName("MemberSMS")
                    .HasMaxLength(70);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.MinPurchase).HasColumnType("money");

                entity.Property(e => e.NetworkIp).HasMaxLength(150);

                entity.Property(e => e.OpId).HasColumnName("opID");

                entity.Property(e => e.OperatorCommission)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrgSafeCard).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrganizationAddress).HasMaxLength(50);

                entity.Property(e => e.OrganizationCommission)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrganizationGuid)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationIp)
                    .HasColumnName("OrganizationIP")
                    .HasMaxLength(1024);

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationNews).HasColumnType("text");

                entity.Property(e => e.OrganizationParamsXml)
                    .HasColumnName("OrganizationParamsXML")
                    .HasColumnType("ntext");

                entity.Property(e => e.OrganizationSales).HasColumnType("text");

                entity.Property(e => e.OrganizationShortName).HasMaxLength(24);

                entity.Property(e => e.OrganizationStatusId).HasColumnName("OrganizationStatusID");

                entity.Property(e => e.OrganizationTableXml)
                    .HasColumnName("OrganizationTableXML")
                    .HasColumnType("ntext");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentTerminalNumberConsumerism).HasMaxLength(10);

                entity.Property(e => e.PaymentTerminalPassword).HasMaxLength(50);

                entity.Property(e => e.PaymentTerminalUserName).HasMaxLength(50);

                entity.Property(e => e.PicforPrinting)
                    .HasColumnName("PICForPrinting")
                    .HasColumnType("ntext");

                entity.Property(e => e.PicturePath).HasMaxLength(100);

                entity.Property(e => e.RegistrationXml).HasColumnType("ntext");

                entity.Property(e => e.Remark).HasColumnType("ntext");

                entity.Property(e => e.RemarkFromDts)
                    .HasColumnName("RemarkFromDTS")
                    .HasColumnType("ntext");

                entity.Property(e => e.SendSms).HasColumnName("SendSMS");

                entity.Property(e => e.SendSmsafterPayment).HasColumnName("SendSMSAfterPayment");

                entity.Property(e => e.SendSmscancelMessage)
                    .HasColumnName("SendSMSCancelMessage")
                    .HasMaxLength(250);

                entity.Property(e => e.SendSmsmessage)
                    .HasColumnName("SendSMSMessage")
                    .HasMaxLength(250);

                entity.Property(e => e.ServiceMail).HasMaxLength(50);

                entity.Property(e => e.ShortCardNumberStartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1980)-(1))-(1))");

                entity.Property(e => e.SignaturePic).HasMaxLength(250);

                entity.Property(e => e.SmsMobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SoGoodRalizationMethodId).HasColumnName("SoGoodRalizationMethodID");

                entity.Property(e => e.Theme)
                    .HasColumnName("theme")
                    .HasMaxLength(50);

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(50);

                entity.Property(e => e.TokenCreatedTime)
                    .HasColumnName("tokenCreatedTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TradeSitePaymentTerminalNumber).HasMaxLength(10);

                entity.Property(e => e.TradeSiteSmsconfirmation).HasColumnName("TradeSiteSMSConfirmation");

                entity.Property(e => e.TradeWebSiteUrl).HasColumnType("ntext");

                entity.Property(e => e.UseDtsgateway)
                    .HasColumnName("UseDTSGateway")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VirtualName).HasMaxLength(50);

                entity.Property(e => e.WebItubusinessReport).HasColumnName("WebITUbusinessReport");

                entity.Property(e => e.WebSite).HasMaxLength(50);
            });

            modelBuilder.Entity<OrganizationsIdenticalMoney>(entity =>
            {
                entity.HasKey(e => e.IdType);

                entity.Property(e => e.IdType).HasColumnName("idType");

                entity.Property(e => e.NameHebrew)
                    .HasColumnName("nameHebrew")
                    .HasMaxLength(20);

                entity.Property(e => e.TypeIdenticalMoney)
                    .IsRequired()
                    .HasColumnName("typeIdenticalMoney")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<OrgnizationProductOrderSite>(entity =>
            {
                entity.HasKey(e => e.OrgId);

                entity.ToTable("Orgnization_ProductOrderSite");

                entity.Property(e => e.OrgId)
                    .HasColumnName("OrgID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<PageDesign>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.Property(e => e.PageId)
                    .HasColumnName("PageID")
                    .ValueGeneratedNever();

                entity.Property(e => e.PageDescription).HasMaxLength(500);

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PageFields>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FieldId)
                    .IsRequired()
                    .HasColumnName("FieldID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PayerSuppliers>(entity =>
            {
                entity.HasKey(e => new { e.OrgId, e.BusinessSubTypeId })
                    .HasName("PK_PayerSuppliers2");

                entity.Property(e => e.BusinessSubTypeId).HasColumnName("BusinessSubTypeID");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<PaymentModel>(entity =>
            {
                entity.Property(e => e.PaymetName).HasMaxLength(50);
            });

            modelBuilder.Entity<Points>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CashValue).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.NumberOfpoints).HasColumnName("NumberOFPoints");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<PortalCategories>(entity =>
            {
                entity.HasKey(e => e.CategoryNumber);

                entity.HasIndex(e => new { e.CategoryFather, e.CategoryNumber })
                    .HasName("IX_PortalCategories_MulA");

                entity.HasIndex(e => new { e.CategoryNumber, e.CategoryName, e.Visible })
                    .HasName("IX_PortalCategories_Visible");

                entity.HasIndex(e => new { e.CategoryStatus, e.Visible, e.CategoryNumber })
                    .HasName("IX_PortalCategories__MulA");

                entity.HasIndex(e => new { e.CategoryType, e.CategoryName, e.CategoryNumber, e.CategoryStatus, e.Visible })
                    .HasName("IX_PortalCategories__CategoryNumber_CategoryStatus_Visible");

                entity.Property(e => e.ArrivalMapImage).HasMaxLength(100);

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryAlttext)
                    .HasColumnName("CategoryALTText")
                    .HasColumnType("ntext");

                entity.Property(e => e.CategoryColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryDetailsName).HasMaxLength(50);

                entity.Property(e => e.CategoryEndTime).HasColumnType("datetime");

                entity.Property(e => e.CategoryHtml)
                    .HasColumnName("CategoryHTML")
                    .HasColumnType("ntext");

                entity.Property(e => e.CategoryIframeHight).HasMaxLength(50);

                entity.Property(e => e.CategoryImg).HasMaxLength(100);

                entity.Property(e => e.CategoryLimitPerMemberFormula).HasColumnType("ntext");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryStartTime).HasColumnType("datetime");

                entity.Property(e => e.CategoryText).HasColumnType("ntext");

                entity.Property(e => e.CategoryUrlParamKey).HasMaxLength(50);

                entity.Property(e => e.CategoryUrlParamValue).HasMaxLength(50);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.LeumiAppName).HasMaxLength(25);

                entity.Property(e => e.LinkToTheatre).HasMaxLength(250);

                entity.Property(e => e.LocationExplain).HasColumnType("ntext");

                entity.Property(e => e.ShortDescription).HasMaxLength(1000);
            });

            modelBuilder.Entity<PortalCategoriesImages>(entity =>
            {
                entity.HasKey(e => e.PortalCategoriesImageId);

                entity.HasIndex(e => new { e.PortalCategoriesImageId, e.CategoryNumber, e.ImageTypeId })
                    .HasName("IX_PortalCategoriesImages_CategoryNumber_ImageTypeId");

                entity.Property(e => e.Alt).HasMaxLength(50);

                entity.Property(e => e.FileName).HasMaxLength(250);

                entity.Property(e => e.ImageCreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OriginalFileName).HasMaxLength(250);
            });

            modelBuilder.Entity<PortalCategoriesTicketHubTours>(entity =>
            {
                entity.HasKey(e => new { e.CategoryNumber, e.TicketHubTourNumber });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.TourComment).HasMaxLength(100);

                entity.Property(e => e.TourTitle).HasMaxLength(255);
            });

            modelBuilder.Entity<PortalSearchData>(entity =>
            {
                entity.Property(e => e.PortalSearchDataId).HasColumnName("PortalSearchDataID");

                entity.Property(e => e.Alt).HasMaxLength(50);

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);

                entity.Property(e => e.ConcatForSearch)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName).HasMaxLength(255);

                entity.Property(e => e.FatherId).HasColumnName("FatherID");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.FirstFatherId).HasColumnName("FirstFatherID");

                entity.Property(e => e.LastmodifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.StoreAddress).HasMaxLength(150);

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName).HasMaxLength(512);
            });

            modelBuilder.Entity<PortalSearchDataBefore>(entity =>
            {
                entity.HasKey(e => e.PortalSearchDataId)
                    .HasName("PK__PortalSe__95C50EAEA4E06AD7");

                entity.ToTable("PortalSearchData_before");

                entity.Property(e => e.PortalSearchDataId).HasColumnName("PortalSearchDataID");

                entity.Property(e => e.Alt).HasMaxLength(50);

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);

                entity.Property(e => e.ConcatForSearch)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName).HasMaxLength(255);

                entity.Property(e => e.FatherId).HasColumnName("FatherID");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.FirstFatherId).HasColumnName("FirstFatherID");

                entity.Property(e => e.LastmodifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.StoreAddress).HasMaxLength(150);

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName).HasMaxLength(512);
            });

            modelBuilder.Entity<PortalSearchDataOld>(entity =>
            {
                entity.HasKey(e => e.PortalSearchDataId)
                    .HasName("PK__PortalSe__95C50EAEF1EE5A8B");

                entity.ToTable("PortalSearchData_OLD");

                entity.Property(e => e.PortalSearchDataId).HasColumnName("PortalSearchDataID");

                entity.Property(e => e.Alt).HasMaxLength(50);

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);

                entity.Property(e => e.ConcatForSearch)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName).HasMaxLength(255);

                entity.Property(e => e.FatherId).HasColumnName("FatherID");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.FirstFatherId).HasColumnName("FirstFatherID");

                entity.Property(e => e.LastmodifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.StoreAddress).HasMaxLength(150);

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName).HasMaxLength(512);
            });

            modelBuilder.Entity<PortalSearchDataQueueBusiness>(entity =>
            {
                entity.HasKey(e => e.QueueId);

                entity.ToTable("PortalSearchDataQueue_business");

                entity.Property(e => e.QueueId).HasColumnName("QueueID");

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PortalSearchDataQueueOrganizationCategories>(entity =>
            {
                entity.HasKey(e => e.QueueId);

                entity.ToTable("PortalSearchDataQueue_OrganizationCategories");

                entity.Property(e => e.QueueId).HasColumnName("QueueID");

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<PortalSearchDataQueueOrganizationCategoriesImages>(entity =>
            {
                entity.HasKey(e => e.QueueId);

                entity.ToTable("PortalSearchDataQueue_OrganizationCategoriesImages");

                entity.Property(e => e.QueueId).HasColumnName("QueueID");

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PortalSearchDataQueuePortalCategories>(entity =>
            {
                entity.HasKey(e => e.QueueId);

                entity.ToTable("PortalSearchDataQueue_PortalCategories");

                entity.Property(e => e.QueueId).HasColumnName("QueueID");

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PortalSearchDataQueueTags>(entity =>
            {
                entity.HasKey(e => e.QueueId);

                entity.ToTable("PortalSearchDataQueue_Tags");

                entity.Property(e => e.QueueId).HasColumnName("QueueID");

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PortalSearchDataQueueTagsCategory>(entity =>
            {
                entity.HasKey(e => e.QueueId);

                entity.ToTable("PortalSearchDataQueue_TagsCategory");

                entity.Property(e => e.QueueId).HasColumnName("QueueID");

                entity.Property(e => e.CategoryTagId).HasColumnName("CategoryTagID");

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PortalSearchDataTemp>(entity =>
            {
                entity.HasKey(e => e.PortalSearchDataId)
                    .HasName("PK__PortalSe__95C50EAE17F81060");

                entity.ToTable("PortalSearchData_temp");

                entity.Property(e => e.PortalSearchDataId).HasColumnName("PortalSearchDataID");

                entity.Property(e => e.Alt).HasMaxLength(50);

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);

                entity.Property(e => e.ConcatForSearch)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName).HasMaxLength(255);

                entity.Property(e => e.FatherId).HasColumnName("FatherID");

                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.Property(e => e.FirstFatherId).HasColumnName("FirstFatherID");

                entity.Property(e => e.LastmodifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Region).HasMaxLength(50);

                entity.Property(e => e.StoreAddress).HasMaxLength(150);

                entity.Property(e => e.StoreName).HasMaxLength(255);

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName).HasMaxLength(512);
            });

            modelBuilder.Entity<PraxellFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileId).HasColumnName("fileId");

                entity.Property(e => e.FileEndUpload)
                    .HasColumnName("fileEndUpload")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .HasColumnName("fileName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FileSize).HasColumnName("fileSize");

                entity.Property(e => e.FileStartUpload)
                    .HasColumnName("fileStartUpload")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumOfRecords).HasColumnName("numOfRecords");

                entity.Property(e => e.StatusFile).HasColumnName("statusFile");
            });

            modelBuilder.Entity<PraxellFileStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.NameStatus)
                    .HasColumnName("nameStatus")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusRemark)
                    .HasColumnName("statusRemark")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PraxellMedia>(entity =>
            {
                entity.HasKey(e => e.RepTransId);

                entity.Property(e => e.RepTransId).HasColumnName("repTransId");

                entity.Property(e => e.ActionCashierId)
                    .HasColumnName("actionCashierId")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ActionNetworkId)
                    .HasColumnName("actionNetworkID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionStoreId)
                    .HasColumnName("actionStoreId")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionStorePnumber)
                    .HasColumnName("actionStorePNumber")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionTicketId)
                    .HasColumnName("actionTicketId")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionType)
                    .HasColumnName("actionType")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ActionVendorPnumber)
                    .HasColumnName("actionVendorPNumber")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CardBatchNumber)
                    .HasColumnName("cardBatchNumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnName("cardExpirationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CardIssuerPnumber)
                    .HasColumnName("cardIssuerPNumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CardIssuerSn)
                    .HasColumnName("cardIssuerSN")
                    .HasMaxLength(5);

                entity.Property(e => e.CardNumber)
                    .HasColumnName("cardNumber")
                    .HasMaxLength(20);

                entity.Property(e => e.CardSerialNumber)
                    .HasColumnName("cardSerialNumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CardSeriesNumber)
                    .HasColumnName("cardSeriesNumber")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Currency).HasMaxLength(10);

                entity.Property(e => e.DecimalCurrency).HasColumnName("decimalCurrency");

                entity.Property(e => e.FileId).HasColumnName("fileId");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasMaxLength(50);

                entity.Property(e => e.ServerDataTime)
                    .HasColumnName("serverDataTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionActionCashierId)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionActionNetworkId)
                    .HasColumnName("VoidTransactionActionNetworkID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionActionStoreId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionActionStorePnumber)
                    .HasColumnName("VoidTransactionActionStorePNumber")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionActionTicketId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionActionType)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionActionVendorPnumber)
                    .HasColumnName("VoidTransactionActionVendorPNumber")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VoidTransactionId)
                    .HasColumnName("VoidTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionOrderId)
                    .HasColumnName("VoidTransactionOrderID")
                    .HasMaxLength(50);

                entity.Property(e => e.VoidTransactionPosDate).HasColumnType("datetime");

                entity.Property(e => e.VoidTransactionServerDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PraxellNetworkName>(entity =>
            {
                entity.HasKey(e => e.NetworkId);

                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");

                entity.Property(e => e.ActionNetworkId)
                    .HasColumnName("actionNetworkID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionNetworkName)
                    .HasColumnName("actionNetworkName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PraxellStoreName>(entity =>
            {
                entity.HasKey(e => e.StoreNameId);

                entity.Property(e => e.StoreNameId).HasColumnName("StoreNameID");

                entity.Property(e => e.StoreNameFullName)
                    .HasMaxLength(70)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PraxellTransactionLog>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("Praxell_TransactionLog");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Amount1Req)
                    .IsRequired()
                    .HasColumnName("Amount1_Req")
                    .HasMaxLength(20);

                entity.Property(e => e.Amount1Res)
                    .IsRequired()
                    .HasColumnName("Amount1_Res")
                    .HasMaxLength(20);

                entity.Property(e => e.Amount2Req)
                    .IsRequired()
                    .HasColumnName("Amount2_Req")
                    .HasMaxLength(20);

                entity.Property(e => e.Amount2Res)
                    .IsRequired()
                    .HasColumnName("Amount2_Res")
                    .HasMaxLength(20);

                entity.Property(e => e.CardId)
                    .IsRequired()
                    .HasColumnName("CardID")
                    .HasMaxLength(20);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ErrorNumRes)
                    .IsRequired()
                    .HasColumnName("ErrorNum_Res")
                    .HasMaxLength(20);

                entity.Property(e => e.OpCodeReq)
                    .IsRequired()
                    .HasColumnName("OpCode_Req")
                    .HasMaxLength(20);

                entity.Property(e => e.OpCodeRes)
                    .IsRequired()
                    .HasColumnName("OpCode_Res")
                    .HasMaxLength(20);

                entity.Property(e => e.PraxellTransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.PraxellTransactionId)
                    .IsRequired()
                    .HasColumnName("PraxellTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.StrRequest)
                    .IsRequired()
                    .HasColumnName("strRequest")
                    .HasColumnType("ntext");

                entity.Property(e => e.StrResponse)
                    .IsRequired()
                    .HasColumnName("strResponse")
                    .HasColumnType("ntext");

                entity.Property(e => e.TerminalId)
                    .HasColumnName("TerminalID")
                    .HasMaxLength(20);

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<PricingBasePriceCommision>(entity =>
            {
                entity.Property(e => e.PercentValue).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.BusinessSubType)
                    .WithMany(p => p.PricingBasePriceCommision)
                    .HasForeignKey(d => d.BusinessSubTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PricingBasePriceCommision_BusinessSubType");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PricingBasePriceCommision)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PricingBasePriceCommision_Organizations");
            });

            modelBuilder.Entity<ProdectTypes>(entity =>
            {
                entity.HasKey(e => e.ProductType);

                entity.Property(e => e.ProductType).ValueGeneratedNever();

                entity.Property(e => e.Ptdescription)
                    .HasColumnName("PTDescription")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProductsVarsGlobal>(entity =>
            {
                entity.HasIndex(e => e.VarNameGlobal);

                entity.HasIndex(e => new { e.DtCreatedDate, e.FullBarCode })
                    .HasName("IX_ProductsVarsGlobal_FullBarCode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BusinessList).HasMaxLength(4000);

                entity.Property(e => e.CupaPrice).HasColumnType("money");

                entity.Property(e => e.CupaPriceUpdateDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DtCreatedDate)
                    .HasColumnName("dtCreatedDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FullBarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortNameVar)
                    .HasColumnName("shortNameVar")
                    .HasMaxLength(50);

                entity.Property(e => e.VarName).HasMaxLength(50);

                entity.Property(e => e.VarNameGlobal)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.VariantCatalogNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductsVarsGlobalOld>(entity =>
            {
                entity.ToTable("ProductsVarsGlobal_old");

                entity.HasIndex(e => e.FullBarCode)
                    .HasName("IX_ProductsVarsGlobal__FullBarCode")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BusinessList).HasMaxLength(4000);

                entity.Property(e => e.CupaPrice).HasColumnType("money");

                entity.Property(e => e.CupaPriceUpdateDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DtCreatedDate)
                    .HasColumnName("dtCreatedDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FullBarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortNameVar)
                    .HasColumnName("shortNameVar")
                    .HasMaxLength(50);

                entity.Property(e => e.VarName).HasMaxLength(50);

                entity.Property(e => e.VarNameGlobal)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsVarsOld>(entity =>
            {
                entity.HasKey(e => e.FullBarCode)
                    .HasName("PK_productsVars");

                entity.HasIndex(e => e.HatavaMultiplier)
                    .HasName("IX_ProductsVars_HatavaMultiplier");

                entity.HasIndex(e => new { e.FullBarCode, e.BusinessId, e.BusinessName, e.Iscampaign, e.HatavaMultiplier, e.SendReportDate, e.PresentationCode, e.SectionCode, e.ShowDate, e.TheaterCode, e.MarkSeats, e.SellEndDate, e.VarName, e.ShortNameVar, e.StartDate, e.EndDate, e.CatalogicPrice, e.DisabledToOrder, e.BusinessSubTypeId })
                    .HasName("IX_ProductsVars_BusinessSubTypeID");

                entity.Property(e => e.FullBarCode)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BusinessId).HasMaxLength(50);

                entity.Property(e => e.BusinessList).HasMaxLength(4000);

                entity.Property(e => e.BusinessName).HasMaxLength(50);

                entity.Property(e => e.BusinessShortName).HasMaxLength(50);

                entity.Property(e => e.BusinessSubTypeId).HasColumnName("BusinessSubTypeID");

                entity.Property(e => e.CatalogicPrice).HasMaxLength(500);

                entity.Property(e => e.CupaPriceUpdateDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.HatavaMultiplier).HasDefaultValueSql("((1))");

                entity.Property(e => e.ImplementationMethod).HasDefaultValueSql("((0))");

                entity.Property(e => e.IrgunPriceFormula).HasMaxLength(500);

                entity.Property(e => e.Iscampaign).HasColumnName("ISCampaign");

                entity.Property(e => e.Ivrcode)
                    .HasColumnName("IVRCode")
                    .HasMaxLength(10);

                entity.Property(e => e.Ivrorder).HasColumnName("IVROrder");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.MarkSeats).HasDefaultValueSql("((0))");

                entity.Property(e => e.MemberDailyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberGeneralLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberMonthlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberQuarterLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberWeeklyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.MemberYearlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.Opid).HasColumnName("OPid");

                entity.Property(e => e.PosbarCode)
                    .HasColumnName("POSBarCode")
                    .HasMaxLength(50);

                entity.Property(e => e.PresentationCode).HasMaxLength(30);

                entity.Property(e => e.PrintPattern).HasColumnType("ntext");

                entity.Property(e => e.ProductDailyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductGeneralLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductMonthlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductQuarterLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductWeeklyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProductYearlyLimitFormula).HasMaxLength(500);

                entity.Property(e => e.ProviderPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReferenceDeviation).HasMaxLength(20);

                entity.Property(e => e.SectionCode).HasMaxLength(30);

                entity.Property(e => e.SellEndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.SendReportDate).HasColumnType("datetime");

                entity.Property(e => e.ShortNameVar)
                    .HasColumnName("shortNameVar")
                    .HasMaxLength(24);

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.Property(e => e.SortOrder).HasDefaultValueSql("((99))");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Type).HasMaxLength(10);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.VarComissionFormula).HasMaxLength(500);

                entity.Property(e => e.VarDiscountFormula)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("(N'0')");

                entity.Property(e => e.VarName).HasMaxLength(100);

                entity.Property(e => e.VarPriceFormula).HasMaxLength(500);

                entity.Property(e => e.Vat).HasColumnName("VAT");
            });

            modelBuilder.Entity<ProductsVarsSubBranchExcluded>(entity =>
            {
                entity.HasKey(e => new { e.FullBarCode, e.SubBranchRowNumber, e.BusinessId });

                entity.Property(e => e.FullBarCode).HasMaxLength(50);

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProviderBuisness>(entity =>
            {
                entity.HasKey(e => e.BuisnessId)
                    .HasName("PK_ProvidBuisness");

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Providers>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);

                entity.Property(e => e.ContactPerson).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(12);
            });

            modelBuilder.Entity<Pulseem>(entity =>
            {
                entity.Property(e => e.PulseemId).HasColumnName("PulseemID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.MemberFirstName).HasMaxLength(50);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.MemberLastName).HasMaxLength(50);

                entity.Property(e => e.PartnerEmail).HasMaxLength(50);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.Telephone).HasMaxLength(13);

                entity.Property(e => e.Source).HasMaxLength(50);

            });

            modelBuilder.Entity<Queries>(entity =>
            {
                entity.HasKey(e => e.QueryId);

                entity.HasIndex(e => e.QueryOrganizationId)
                    .HasName("IX_Queries");

                entity.Property(e => e.QueryId).HasColumnName("QueryID");

                entity.Property(e => e.QueryDefineDate).HasColumnType("datetime");

                entity.Property(e => e.QueryDefineOp).HasColumnName("QueryDefineOP");

                entity.Property(e => e.QueryName).HasMaxLength(100);

                entity.Property(e => e.QueryObject).HasColumnType("ntext");

                entity.Property(e => e.QueryOrganizationId).HasColumnName("QueryOrganizationID");

                entity.Property(e => e.QuerySql)
                    .HasColumnName("QuerySQL")
                    .HasColumnType("ntext");

                entity.Property(e => e.QueryXml)
                    .HasColumnName("QueryXML")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<RechargeResones>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_RechargeReasons");

                entity.Property(e => e.Code).ValueGeneratedNever();

                entity.Property(e => e.Rem)
                    .HasColumnName("REM")
                    .HasMaxLength(255);

                entity.Property(e => e.ShortDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<RedimTypes>(entity =>
            {
                entity.HasKey(e => e.RedimTypeId);

                entity.Property(e => e.RedimName).HasMaxLength(50);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.RegionName).HasMaxLength(20);
            });

            modelBuilder.Entity<RegisterMembers>(entity =>
            {
                entity.HasKey(e => new { e.RegisterType, e.RegisteMemberId });

                entity.Property(e => e.RegisteMemberId)
                    .HasColumnName("RegisteMemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.RegisteDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RegisterType>(entity =>
            {
                entity.Property(e => e.RegisterTypeId).HasColumnName("RegisterTypeID");

                entity.Property(e => e.RegisterTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<RequestFiles>(entity =>
            {
                entity.HasKey(e => e.FileRequestId);

                entity.Property(e => e.FileRequestId).HasColumnName("FileRequestID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HandlingEndDate).HasColumnType("datetime");

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.MashovFileName).IsUnicode(false);
            });

            modelBuilder.Entity<RequestSourceTable>(entity =>
            {
                entity.HasKey(e => e.RequestSourceId);

                entity.Property(e => e.RequestSourceId).HasColumnName("RequestSourceID");

                entity.Property(e => e.RequestSourceDescription).HasMaxLength(20);
            });

            modelBuilder.Entity<RequestStatusTable>(entity =>
            {
                entity.HasKey(e => e.RequestStatusId);

                entity.Property(e => e.RequestStatusId).ValueGeneratedNever();

                entity.Property(e => e.RequestStatusColor)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'TBLWhite')");

                entity.Property(e => e.RequestStatusDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RequestTypeTable>(entity =>
            {
                entity.HasKey(e => e.RequestTypeId);

                entity.Property(e => e.RequestTypeId).ValueGeneratedNever();

                entity.Property(e => e.ExcludeOrganizationId).HasColumnName("ExcludeOrganizationID");

                entity.Property(e => e.MwcDescription).HasMaxLength(100);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.RequestTypeDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Requests>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.HasIndex(e => e.Id1)
                    .HasName("IX_RequestsID1");

                entity.HasIndex(e => e.RequestTime)
                    .HasName("IX_RequestTime");

                entity.HasIndex(e => new { e.Id2, e.Id1 })
                    .HasName("IX__Requests__ID2_ID1");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Card1).HasMaxLength(16);

                entity.Property(e => e.Card2).HasMaxLength(16);

                entity.Property(e => e.FileRequestId).HasColumnName("FileRequestID");

                entity.Property(e => e.Id1)
                    .HasColumnName("ID1")
                    .HasMaxLength(9);

                entity.Property(e => e.Id2)
                    .HasColumnName("ID2")
                    .HasMaxLength(9);

                entity.Property(e => e.LeumiCardFileIdprika).HasColumnName("LeumiCardFileIDPrika");

                entity.Property(e => e.LeumiCardFileIdteina).HasColumnName("LeumiCardFileIDTeina");

                entity.Property(e => e.ManualTime).HasColumnType("datetime");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Remark).HasColumnType("ntext");

                entity.Property(e => e.RequestTime).HasColumnType("datetime");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<RequestsHistorical>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.Property(e => e.RequestId)
                    .HasColumnName("RequestID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Card1).HasMaxLength(16);

                entity.Property(e => e.Card2).HasMaxLength(16);

                entity.Property(e => e.FileRequestId).HasColumnName("FileRequestID");

                entity.Property(e => e.Id1)
                    .HasColumnName("ID1")
                    .HasMaxLength(9);

                entity.Property(e => e.Id2)
                    .HasColumnName("ID2")
                    .HasMaxLength(9);

                entity.Property(e => e.ManualTime).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasColumnType("ntext");

                entity.Property(e => e.RequestTime).HasColumnType("datetime");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<RptFrequencyTypes>(entity =>
            {
                entity.ToTable("RPT_FrequencyTypes");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Desctiption).HasMaxLength(50);
            });

            modelBuilder.Entity<RptJobSchedule>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.ToTable("RPT_JobSchedule");

                entity.Property(e => e.JobId)
                    .HasColumnName("JobID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FrequencyTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01 00:00')");
            });

            modelBuilder.Entity<RptJobs>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK_RPT_Jobs2");

                entity.ToTable("RPT_Jobs");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Dbname)
                    .HasColumnName("DBName")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'DTS_Online')");

                entity.Property(e => e.EmailSubject).HasMaxLength(250);

                entity.Property(e => e.EmailTo).HasMaxLength(512);

                entity.Property(e => e.FileNamePrefix)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.PathToCopy)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProcedureName)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<RptParamValues>(entity =>
            {
                entity.HasKey(e => new { e.ValueId, e.ParamId });

                entity.ToTable("RPT_ParamValues");

                entity.Property(e => e.ValueId).HasColumnName("ValueID");

                entity.Property(e => e.ParamId).HasColumnName("ParamID");
            });

            modelBuilder.Entity<RptParams>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.ParamId })
                    .HasName("PK_RPT_Params1");

                entity.ToTable("RPT_Params");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.ParamId)
                    .HasColumnName("ParamID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ParamDescription).HasMaxLength(50);

                entity.Property(e => e.ParamName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ParamType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RptRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.ToTable("RPT_Request");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.ActualExeDate).HasColumnType("datetime");

                entity.Property(e => e.EmailQueueId).HasColumnName("EmailQueueID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.ParamValueId).HasColumnName("ParamValueID");

                entity.Property(e => e.RequestEmailSubject).HasMaxLength(250);

                entity.Property(e => e.RequestEmailTo).HasMaxLength(200);

                entity.Property(e => e.RequiredExeDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SecureLogin>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionName).HasMaxLength(50);

                entity.Property(e => e.ControllerName).HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ParametersJson)
                    .HasColumnName("ParametersJSON")
                    .HasMaxLength(150);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<SecureLoginGuid>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CardNumber).HasMaxLength(16);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ShamirLoginInfo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GroupName).HasMaxLength(20);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.Token).HasMaxLength(30);

                entity.Property(e => e.Username).HasMaxLength(10);
            });

            modelBuilder.Entity<ShortCardNumbers>(entity =>
            {
                entity.HasKey(e => e.ShortNumber);

                entity.HasIndex(e => e.LongCardNumber)
                    .HasName("IX_LongCardNumber")
                    .IsUnique();

                entity.HasIndex(e => new { e.CardId, e.OrganizationId })
                    .HasName("IX_CardId")
                    .IsUnique();

                entity.HasIndex(e => new { e.MemberName, e.ShortNumber, e.AssignDate, e.LongCardNumber, e.OrganizationId, e.CardId, e.MemberId })
                    .HasName("IX_ShortCardNumbers_MemberId");

                entity.Property(e => e.ShortNumber).ValueGeneratedNever();

                entity.Property(e => e.AssignDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LongCardNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ShortCardNumbers)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShortCardNumbers_Organizations");
            });

            modelBuilder.Entity<ShortCardNumbersBank>(entity =>
            {
                entity.HasKey(e => e.ShortNumber);

                entity.HasIndex(e => new { e.BulkIdentifier, e.ShortNumberOrderGuid })
                    .HasName("IX_ShortCardNumbersBank_1")
                    .IsUnique();

                entity.HasIndex(e => new { e.ShortNumber, e.InsertDate, e.ShortNumberOrderGuid, e.BulkIdentifier })
                    .HasName("IX_ShortCardNumbersBank_BulkIdentifier");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ShortNumberOrderGuid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<ShortCardNumbersExcludeBusiness>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BusinessId)
                    .IsRequired()
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SlinkActions>(entity =>
            {
                entity.Property(e => e.ActionValue).HasMaxLength(20);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SlinkMembersFiles>(entity =>
            {
                entity.HasIndex(e => new { e.PhoneNumber, e.LinkCode, e.MemberId, e.FileId, e.Remark })
                    .HasName("IX_SlinkMembersFiles_FileId_Remark");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmployeeNum).HasMaxLength(20);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LinkCode).HasMaxLength(250);

                entity.Property(e => e.MemberId).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.Remark).HasMaxLength(250);

                entity.Property(e => e.SumOfLoad)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Tz).HasMaxLength(20);
            });

            modelBuilder.Entity<SlinkOrganization>(entity =>
            {
                entity.HasKey(e => e.OrganizationId)
                    .HasName("PK_SlinkCompanies");

                entity.Property(e => e.OrganizationId).ValueGeneratedNever();

                entity.Property(e => e.ContactEmail).HasMaxLength(50);

                entity.Property(e => e.ContactName).HasMaxLength(50);

                entity.Property(e => e.ContactPhone).HasMaxLength(10);

                entity.Property(e => e.DbName).HasMaxLength(20);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SlinkProductTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SlinkProductTypesToOrganizations>(entity =>
            {
                entity.HasKey(e => new { e.TypeId, e.OrganizationId });
            });

            modelBuilder.Entity<SlinkRequestFiles>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK_SlinkRequestFile");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SlinkRequests>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK_SlinkRequest");

                entity.Property(e => e.ActivationCode).HasMaxLength(50);

                entity.Property(e => e.BillingFile).HasMaxLength(150);

                entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EmailName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FileApproveDate).HasColumnType("datetime");

                entity.Property(e => e.FileApprovedBy).HasMaxLength(150);

                entity.Property(e => e.ImageName).HasMaxLength(30);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.MessageBody).HasMaxLength(600);

                entity.Property(e => e.MessageSendTime).HasColumnType("datetime");

                entity.Property(e => e.MessageSender).HasMaxLength(10);

                entity.Property(e => e.Validity)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.VariantId).HasMaxLength(50);
            });

            modelBuilder.Entity<SlinkUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_Users");

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__SlinkUse__C9F28456B8C8EA0D")
                    .IsUnique();

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<SlinkUsersToOrganizations>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.OrganizationId });
            });

            modelBuilder.Entity<SmsMessages>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.Property(e => e.MessageId).ValueGeneratedNever();

                entity.Property(e => e.NotifierName).HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SentTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SmsQueue>(entity =>
            {
                entity.HasKey(e => e.SmsId);

                entity.HasIndex(e => e.DateAdded)
                    .HasName("idx_Nonclustered_SmsQueue_DateAdded");

                entity.HasIndex(e => new { e.SeveralAttempts, e.SmsSend });

                entity.HasIndex(e => new { e.SmsId, e.MemberId, e.OrganizationId })
                    .HasName("IDX_OrganizationID");

                entity.HasIndex(e => new { e.SmsId, e.SmsSend, e.SeveralAttempts, e.GuidTime, e.SenderGuid })
                    .HasName("IX_SmsQueue_SmsSend_SeveralAttempts_GuidTime_SenderGuid");

                entity.Property(e => e.SmsId).HasColumnName("SmsID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.DateAdded)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpirationDelayInMinutes).HasDefaultValueSql("((120))");

                entity.Property(e => e.GuidTime).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Priority).HasDefaultValueSql("((100))");

                entity.Property(e => e.ResultTime).HasColumnType("datetime");

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SendTime).HasColumnType("datetime");

                entity.Property(e => e.SenderGuid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SenderName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ServerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subscribers)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SmsQueueUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<SmsTypes>(entity =>
            {
                entity.HasKey(e => e.SmsType);

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SoGood>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.Area).HasMaxLength(50);

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Posid).HasColumnName("POSID");

                entity.Property(e => e.StoreAddress).HasMaxLength(150);

                entity.Property(e => e.StoreName).HasMaxLength(50);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasIndex(e => new { e.Active, e.SendStockAlert, e.AlertSendDate });

                entity.Property(e => e.StockId).HasColumnName("stockID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AlertSendDate).HasColumnType("datetime");

                entity.Property(e => e.AllertQuantity).HasColumnName("allertQuantity");

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

                entity.Property(e => e.StockName)
                    .IsRequired()
                    .HasColumnName("stockName")
                    .HasMaxLength(100);

                entity.Property(e => e.StockQuantity).HasColumnName("stockQuantity");
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.HasKey(e => new { e.CityId, e.StreetId });

                entity.Property(e => e.StreetName).HasMaxLength(50);
            });

            modelBuilder.Entity<SuppliersTypes>(entity =>
            {
                entity.HasKey(e => e.SupplierId);

                entity.Property(e => e.SupplierName).HasMaxLength(250);
            });

            modelBuilder.Entity<SurveyAnswers>(entity =>
            {
                entity.HasKey(e => e.AnswerId);

                entity.HasIndex(e => new { e.ParticipateId, e.ParticipateOrganizationId, e.QuestionId });

                entity.HasIndex(e => new { e.ParticipateOrganizationId, e.QuestionId, e.Answer });

                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.ParticipateId)
                    .IsRequired()
                    .HasColumnName("ParticipateID")
                    .HasMaxLength(9);

                entity.Property(e => e.ParticipateOrganizationId).HasColumnName("ParticipateOrganizationID");

                entity.Property(e => e.ParticipationDate).HasColumnType("datetime");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            });

            modelBuilder.Entity<SurveyQuestions>(entity =>
            {
                entity.HasKey(e => e.QuestionId);

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.InsertOperatorId).HasColumnName("InsertOperatorID");

                entity.Property(e => e.InsertedDate).HasColumnType("datetime");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Sort).HasColumnName("sort");
            });

            modelBuilder.Entity<SystemTable>(entity =>
            {
                entity.HasKey(e => e.SystemId);

                entity.Property(e => e.SystemId).ValueGeneratedNever();

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Table1>(entity =>
            {
                entity.ToTable("Table_1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FieldId)
                    .IsRequired()
                    .HasColumnName("FieldID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Table3>(entity =>
            {
                entity.ToTable("Table_3");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.A).HasColumnName("a");
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.TagId);

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PremiumTypeId).HasColumnName("PremiumTypeID");

                entity.Property(e => e.TagCloseDate).HasColumnType("datetime");

                entity.Property(e => e.TagContext).HasMaxLength(250);

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.TagOpenDate).HasColumnType("datetime");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrganizationID_Organizations");
            });

            modelBuilder.Entity<TagsBusiness>(entity =>
            {
                entity.HasKey(e => e.BusinessTagId);

                entity.HasIndex(e => new { e.TagId, e.BuisnessId })
                    .HasName("UC_TagsBusiness")
                    .IsUnique();

                entity.Property(e => e.BusinessTagId).HasColumnName("BusinessTagID");

                entity.Property(e => e.BuisnessId)
                    .IsRequired()
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Buisness)
                    .WithMany(p => p.TagsBusiness)
                    .HasForeignKey(d => d.BuisnessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuisnessID_business");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagsBusiness)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagsBusiness_TagID_Tags");
            });

            modelBuilder.Entity<TagsCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryTagId);

                entity.HasIndex(e => new { e.TagId, e.CategoryNumber })
                    .HasName("UC_TagsCategory")
                    .IsUnique();

                entity.Property(e => e.CategoryTagId).HasColumnName("CategoryTagID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagsCategory)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagID_Tags");
            });

            modelBuilder.Entity<TagsPremium>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.PremiumTypeId).HasColumnName("PremiumTypeID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagsPremium)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagsPremium_Tags");
            });

            modelBuilder.Entity<TagsStrips>(entity =>
            {
                entity.HasKey(e => e.StripTagId)
                    .HasName("PK__TagsStri__454FB82EE4348F12");

                entity.Property(e => e.StripTagId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaklaTable>(entity =>
            {
                entity.HasKey(e => e.RowIndex);

                entity.Property(e => e.BusinessName).HasMaxLength(25);

                entity.Property(e => e.CloseBy).HasMaxLength(20);

                entity.Property(e => e.ContactName).HasMaxLength(20);

                entity.Property(e => e.ContactPhone).HasMaxLength(13);

                entity.Property(e => e.DateClose).HasColumnType("datetime");

                entity.Property(e => e.DateOpen)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.OpenBy)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Solution).HasColumnType("ntext");
            });

            modelBuilder.Entity<TblAuditLog>(entity =>
            {
                entity.ToTable("tbl_AuditLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DatabaseName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreatedDate)
                    .HasColumnName("dtCreatedDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblAuditLogin>(entity =>
            {
                entity.ToTable("tbl_AuditLogin");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TeleclalDlrTable>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.Property(e => e.MessageId).ValueGeneratedNever();

                entity.Property(e => e.DeliveryTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.MessageSource)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Rem).HasColumnType("ntext");

                entity.Property(e => e.SendTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(15);

                entity.Property(e => e.SubscriberNumber)
                    .IsRequired()
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<TempLifrok>(entity =>
            {
                entity.HasKey(e => e.Idmember);

                entity.ToTable("Temp_Lifrok");

                entity.Property(e => e.Idmember)
                    .HasColumnName("IDMember")
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("money");

                entity.Property(e => e.BuySum).HasColumnType("money");

                entity.Property(e => e.UnloadSum)
                    .HasColumnName("Unload Sum")
                    .HasColumnType("money");
            });

            modelBuilder.Entity<Terminal>(entity =>
            {
                entity.HasKey(e => e.TerminalNo);

                entity.HasIndex(e => new { e.TerminalNo, e.BuisnessId, e.TerminalActive })
                    .HasName("IX_Terminal_BuisnessID_TerminalActive");

                entity.Property(e => e.TerminalNo)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BuisnessId)
                    .HasColumnName("BuisnessID")
                    .HasMaxLength(50);

                entity.Property(e => e.InstallDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Rem).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(20);

                entity.Property(e => e.UnInstallDate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<TerminalSettings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TerminalNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TerminalType>(entity =>
            {
                entity.Property(e => e.TerminalTypeId)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.TerminalName).HasMaxLength(50);
            });

            modelBuilder.Entity<Theaters>(entity =>
            {
                entity.HasKey(e => e.TheaterCode);

                entity.HasIndex(e => new { e.TheaterName, e.TheaterCode })
                    .HasName("IX_Theaters_TheaterCode");

                entity.Property(e => e.TheaterName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TitanCinemas>(entity =>
            {
                entity.HasKey(e => e.CinemaId);

                entity.HasIndex(e => e.DtsType)
                    .HasName("UQ__TitanCinemas__08012052")
                    .IsUnique();

                entity.Property(e => e.CinemaId).HasColumnName("CinemaID");

                entity.Property(e => e.CinemaName).HasMaxLength(50);

                entity.Property(e => e.DefaultWeekdayOrallweek)
                    .IsRequired()
                    .HasColumnName("DefaultWeekdayORAllweek")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DtsType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EndHighRatesDay).HasMaxLength(50);

                entity.Property(e => e.StartHighRatesDay).HasMaxLength(50);
            });

            modelBuilder.Entity<TitanParseCode>(entity =>
            {
                entity.HasKey(e => e.TitanCode)
                    .HasName("PK_TitanParseCode_1");

                entity.Property(e => e.TitanCode)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.DtsType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TmpYitrotLeumiCardAfter>(entity =>
            {
                entity.HasKey(e => e.CardNo);

                entity.ToTable("TMP_YitrotLeumiCard_After");

                entity.Property(e => e.CardNo).ValueGeneratedNever();

                entity.Property(e => e.Balance).HasColumnType("money");
            });

            modelBuilder.Entity<TracePele>(entity =>
            {
                entity.HasKey(e => e.RowNumber)
                    .HasName("PK__trace_pele__5EBF139D");

                entity.ToTable("trace_pele");

                entity.Property(e => e.ApplicationName).HasMaxLength(128);

                entity.Property(e => e.ClientProcessId).HasColumnName("ClientProcessID");

                entity.Property(e => e.Cpu).HasColumnName("CPU");

                entity.Property(e => e.LoginName).HasMaxLength(128);

                entity.Property(e => e.NtuserName)
                    .HasColumnName("NTUserName")
                    .HasMaxLength(128);

                entity.Property(e => e.Spid).HasColumnName("SPID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.TextData).HasColumnType("ntext");
            });

            modelBuilder.Entity<TypeCard>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).HasColumnName("typeID");

                entity.Property(e => e.NameCardOrg).HasMaxLength(250);

                entity.Property(e => e.TypeNumCard).HasColumnName("typeNumCard");
            });

            modelBuilder.Entity<TypeImplementationDate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImplementationDesc)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TypeImplement)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<UserPortalCategories>(entity =>
            {
                entity.HasKey(e => e.CategoryNumber);

                entity.HasIndex(e => e.CategoryFather)
                    .HasName("IX_UserPortalCategoriesCategoryFather");

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BusinessID")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryAlttext)
                    .HasColumnName("CategoryALTText")
                    .HasColumnType("ntext");

                entity.Property(e => e.CategoryColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryEndTime).HasColumnType("datetime");

                entity.Property(e => e.CategoryHtml)
                    .HasColumnName("CategoryHTML")
                    .HasColumnType("ntext");

                entity.Property(e => e.CategoryImg).HasMaxLength(100);

                entity.Property(e => e.CategoryLimitPerMemberFormula).HasColumnType("ntext");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryStartTime).HasColumnType("datetime");

                entity.Property(e => e.CategoryText).HasColumnType("ntext");

                entity.Property(e => e.LocationExplain).HasColumnType("ntext");
            });

            modelBuilder.Entity<ValueCardChangesLog>(entity =>
            {
                entity.HasKey(e => e.ChangesLogId);

                entity.Property(e => e.ChangesLogId).HasColumnName("ChangesLogID");

                entity.Property(e => e.AnswerAuthNumber).HasColumnName("Answer_AuthNumber");

                entity.Property(e => e.AnswerResponseStatus).HasColumnName("Answer_ResponseStatus");

                entity.Property(e => e.AnswerXml).HasColumnType("xml");

                entity.Property(e => e.Bin1)
                    .IsRequired()
                    .HasColumnName("BIN1")
                    .HasMaxLength(50);

                entity.Property(e => e.Bin2)
                    .IsRequired()
                    .HasColumnName("BIN2")
                    .HasMaxLength(50);

                entity.Property(e => e.DateAdded)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExtRequestId).HasColumnName("ExtRequestID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.Variable)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ValueCardErrorCodes>(entity =>
            {
                entity.HasKey(e => e.ErrorCode);

                entity.Property(e => e.ErrorCode).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ValueCardPromoId>(entity =>
            {
                entity.HasKey(e => e.PromoId);

                entity.ToTable("ValueCardPromoID");

                entity.Property(e => e.PromoId)
                    .HasColumnName("PromoID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Identifies).ValueGeneratedOnAdd();

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            });

            modelBuilder.Entity<ValueCardServices>(entity =>
            {
                entity.HasKey(e => e.ServiceId);

                entity.Property(e => e.ServiceId)
                    .HasColumnName("ServiceID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ValueCardSettings>(entity =>
            {
                entity.HasKey(e => e.Vckey);

                entity.Property(e => e.Vckey)
                    .HasColumnName("VCkey")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(255);
            });

            modelBuilder.Entity<ValueCardTranLog>(entity =>
            {
                entity.HasKey(e => e.ExtRequestId);

                entity.Property(e => e.ExtRequestId).HasColumnName("ExtRequestID");

                entity.Property(e => e.AmountPaid).HasColumnType("money");

                entity.Property(e => e.AnswerAuthNumber).HasColumnName("Answer_AuthNumber");

                entity.Property(e => e.AnswerResponseStatus).HasColumnName("Answer_ResponseStatus");

                entity.Property(e => e.AnswerTime).HasColumnType("datetime");

                entity.Property(e => e.AnswerXml).HasColumnType("xml");

                entity.Property(e => e.Bin1)
                    .HasColumnName("BIN1")
                    .HasMaxLength(50);

                entity.Property(e => e.Bin2)
                    .HasColumnName("BIN2")
                    .HasMaxLength(50);

                entity.Property(e => e.CardGroupId).HasColumnName("CardGroupID");

                entity.Property(e => e.DateAdded)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExtClientEmail).HasMaxLength(100);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PromoId).HasColumnName("PromoID");

                entity.Property(e => e.SendTime).HasColumnType("datetime");

                entity.Property(e => e.Variable).HasMaxLength(50);
            });

            modelBuilder.Entity<VarChangeLog>(entity =>
            {
                entity.ToTable("_VarChange_log");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Dt)
                    .HasColumnName("dt")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VarCurrent).HasMaxLength(50);

                entity.Property(e => e.VarNew).HasMaxLength(50);
            });

            modelBuilder.Entity<VariantBenefitInformation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.VariantInformation).IsRequired();
            });

            modelBuilder.Entity<VariantPresents>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.MoreInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<VariantProperties>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VariantStock>(entity =>
            {
                entity.HasKey(e => new { e.StockId, e.FullBarCode });

                entity.HasIndex(e => new { e.StockId, e.FullBarCode })
                    .HasName("IX_VariantStock_FullBarCode");

                entity.Property(e => e.StockId).HasColumnName("stockID");

                entity.Property(e => e.FullBarCode).HasMaxLength(50);

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VariantVerifone>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SerieId).HasColumnName("SerieID");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");
            });

            modelBuilder.Entity<WalletLoadMoney>(entity =>
            {
                entity.HasIndex(e => e.WalletId)
                    .HasName("UC_WalletID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.CheckLeumiRespone).HasMaxLength(10);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsLevarageMore)
                    .HasColumnName("is_Levarage_More")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLeverageCreadit)
                    .HasColumnName("is_Leverage_Creadit")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLeverageOrg)
                    .HasColumnName("is_Leverage_Org")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLoadMoney).HasDefaultValueSql("((0))");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.WalletPresentMore).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<WebConfigKeys>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.KeyDescription).HasMaxLength(255);

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.Value).HasMaxLength(255);
            });

            modelBuilder.Entity<WebServiceTransactionOld>(entity =>
            {
                entity.HasKey(e => e.TtransactionId)
                    .HasName("PK_WebServiceTransaction");

                entity.HasIndex(e => e.TtransactionProductId)
                    .HasName("IX_WebServiceTransaction_TTransactionProductID");

                entity.HasIndex(e => new { e.TtransactionProductId, e.TtransactionOrder })
                    .HasName("IX_WebServiceTransaction_TTransactionProductID_TTransactionOrder");

                entity.Property(e => e.TtransactionId).HasColumnName("TTransactionID");

                entity.Property(e => e.CancelCommission).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CatalogicPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CustomerDiscount).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CustomerPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DistributionComission).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.IrgunPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Iscampaign)
                    .HasColumnName("ISCampaign")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MarketingCommission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TtransactionAsmcta)
                    .HasColumnName("TTransactionAsmcta")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TtransactionDateTime)
                    .HasColumnName("TTransactionDateTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TtransactionMemberId)
                    .HasColumnName("TTransactionMemberID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TtransactionMetaData)
                    .HasColumnName("TTransactionMetaData")
                    .HasMaxLength(255);

                entity.Property(e => e.TtransactionOrder).HasColumnName("TTransactionOrder");

                entity.Property(e => e.TtransactionProductId)
                    .HasColumnName("TTransactionProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.TtransactionStatus)
                    .HasColumnName("TTransactionStatus")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Ttransactionquantity)
                    .HasColumnName("TTransactionquantity")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<Wserrors>(entity =>
            {
                entity.HasKey(e => e.ErrorId);

                entity.ToTable("WSErrors");

                entity.Property(e => e.ErrorId).HasColumnName("ErrorID");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<WsmethodPremission>(entity =>
            {
                entity.HasKey(e => new { e.MethodId, e.OrgUserId });

                entity.ToTable("WSMethodPremission");

                entity.Property(e => e.MethodId).HasColumnName("MethodID");

                entity.Property(e => e.OrgUserId).HasColumnName("OrgUserID");
            });

            modelBuilder.Entity<Wsmethods>(entity =>
            {
                entity.HasKey(e => e.MethodId);

                entity.ToTable("WSMethods");

                entity.Property(e => e.MethodId)
                    .HasColumnName("MethodID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MethodName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
