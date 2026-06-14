using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Nofshonit.Common.EF.Club
{
    public partial class ClubContext : DbContext
    {
        public ClubContext()
        {
        }

        public ClubContext(DbContextOptions<ClubContext> options)
            : base(options)
        {
        }

        public string _connectionString { get; set; }
        public ClubContext(string connStr)
        {
            _connectionString = connStr;
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }

        public virtual DbSet<AppConfig> AppConfig { get; set; }
        public virtual DbSet<ClubCreditCardExcluded> ClubCreditCardExcluded { get; set; }
        public virtual DbSet<AllFieldMap> AllFieldMap { get; set; }
        public virtual DbSet<AllMembers> AllMembers { get; set; }
        public virtual DbSet<AllowanceHistory> AllowanceHistory { get; set; }
        public virtual DbSet<RegistrationAudits> RegistrationAudits { get; set; }

        

        public virtual DbSet<AllMembersProperties> AllMembersProperties { get; set; }
        // public virtual DbSet<Amir> Amir { get; set; }
        public virtual DbSet<AmountToWallet> AmountToWallet { get; set; }
        public virtual DbSet<BehatzdaaNoCards> BehatzdaaNoCards { get; set; }
        public virtual DbSet<Atractionsorders> Atractionsorders { get; set; }
        public virtual DbSet<BenefitFileLog> BenefitFileLog { get; set; }
        public virtual DbSet<BudgetStockCategory> BudgetStockCategory { get; set; }
        public virtual DbSet<BusinessSubTypeLimits> BusinessSubTypeLimits { get; set; }
        public virtual DbSet<BusinessSubTypeSpecificationAll> BusinessSubTypeSpecificationAll { get; set; }
        public virtual DbSet<BusinessSubTypeSpecificationByVariants> BusinessSubTypeSpecificationByVariants { get; set; }
        public virtual DbSet<BusinessSubTypeSpecificationCurrent> BusinessSubTypeSpecificationCurrent { get; set; }
        public virtual DbSet<CampaignMembersUse> CampaignMembersUse { get; set; }
        public virtual DbSet<CardRequest> CardRequest { get; set; }
        public virtual DbSet<Cards> Cards { get; set; }
        public virtual DbSet<CardsSkeleton> CardsSkeleton { get; set; }
        public virtual DbSet<CategoryVariants> CategoryVariants { get; set; }
        public virtual DbSet<CloseDaysorders> CloseDaysorders { get; set; }
        public virtual DbSet<ClubCreditCardLog> ClubCreditCardLog { get; set; }
        public virtual DbSet<CoinsSource> CoinsSource { get; set; }
        public virtual DbSet<CouponToCancel> CouponToCancel { get; set; }
        public virtual DbSet<CouponsStockAtrorders> CouponsStockAtrorders { get; set; }
        public virtual DbSet<CreditCardHolders> CreditCardHolders { get; set; }
        public virtual DbSet<DwhCustomersChangeLog> DwhCustomersChangeLog { get; set; }
        public virtual DbSet<DwhProductsChangeLog> DwhProductsChangeLog { get; set; }
        public virtual DbSet<DwhTransactions> DwhTransactions { get; set; }
        public virtual DbSet<EmailSubscription> EmailSubscription { get; set; }
        public virtual DbSet<Errors> Errors { get; set; }
        public virtual DbSet<EventimMemberIdTemp> EventimMemberIdTemp { get; set; }
        public virtual DbSet<FriendBringsFriend> FriendBringsFriend { get; set; }
        public virtual DbSet<Hanpaka> Hanpaka { get; set; }
        public virtual DbSet<HelpPopulationType> HelpPopulationType { get; set; }
        public virtual DbSet<HelpPremiumType> HelpPremiumType { get; set; }
        public virtual DbSet<MemberCodes> MemberCodes { get; set; }
        public virtual DbSet<LastRunServiceUpdateMembers> LastRunServiceUpdateMembers { get; set; }
        public virtual DbSet<LmPayRate> LmPayRate { get; set; }
        public virtual DbSet<Lminfo> Lminfo { get; set; }
        public virtual DbSet<LoadCards> LoadCards { get; set; }
        public virtual DbSet<MemberAddress> MemberAddresses { get; set; }
        public virtual DbSet<LoginFail> LoginFail { get; set; }
        public virtual DbSet<MemberTempTable> MemberTempTable { get; set; }
        public virtual DbSet<MembersAllowedLogin> MembersAllowedLogin { get; set; }
        public virtual DbSet<MembersCoins> MembersCoins { get; set; }
        public virtual DbSet<MembersCoinsArchive> MembersCoinsArchive { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<MonthlyStockAmounts> MonthlyStockAmounts { get; set; }
        public virtual DbSet<MovieOrdersToWebOrders> MovieOrdersToWebOrders { get; set; }
        public virtual DbSet<Moviesorders> Moviesorders { get; set; }
        public virtual DbSet<MultiVariant> MultiVariant { get; set; }
        public virtual DbSet<MultiVariantGroup> MultiVariantGroup { get; set; }
        public virtual DbSet<MwcHanpakaRequests> MwcHanpakaRequests { get; set; }
        public virtual DbSet<MwcMultiRequest> MwcMultiRequest { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderTransfer> OrderTransfer { get; set; }
        public virtual DbSet<OrdersStatus> OrdersStatus { get; set; }
        public virtual DbSet<OrganizationOps> OrganizationOps { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<PortalCategoriesStatusChanges> PortalCategoriesStatusChanges { get; set; }
        public virtual DbSet<PortalCategoriesStatuses> PortalCategoriesStatuses { get; set; }
        public virtual DbSet<PraxellTransactionLog> PraxellTransactionLog { get; set; }
        public virtual DbSet<ProductsVars> ProductsVars { get; set; }
        public virtual DbSet<ProviderBuisness> ProviderBuisness { get; set; }
        public virtual DbSet<RechargeResones> RechargeResones { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<RequestsHistorical> RequestsHistorical { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<SalesLines> SalesLines { get; set; }
        public virtual DbSet<ShopingBasket> ShopingBasket { get; set; }
        public virtual DbSet<ShortCardNumbers> ShortCardNumbers { get; set; }
        public virtual DbSet<SiteConfiguration> SiteConfiguration { get; set; }
        public virtual DbSet<SpaOrdersToWebOrders> SpaOrdersToWebOrders { get; set; }
        public virtual DbSet<Spaorders> Spaorders { get; set; }
        public virtual DbSet<SubsidyPrecent> SubsidyPrecent { get; set; }
        public virtual DbSet<TerminalActive> TerminalActive { get; set; }
        public virtual DbSet<TitanParseCode> TitanParseCode { get; set; }
        public virtual DbSet<TzimerOrdersToWebOrders> TzimerOrdersToWebOrders { get; set; }
        public virtual DbSet<Tzimersorders> Tzimersorders { get; set; }
        public virtual DbSet<VariantGroupLimits> VariantGroupLimits { get; set; }
        public virtual DbSet<VerifonTransactionLog> VerifonTransactionLog { get; set; }
        public virtual DbSet<WalletLoadMoney> WalletLoadMoney { get; set; }
        public virtual DbSet<WalletMembers> WalletMembers { get; set; }
        public virtual DbSet<WalletMembersDetails> WalletMembersDetails { get; set; }
        public virtual DbSet<WebServiceTransaction> WebServiceTransaction { get; set; }
        public virtual DbSet<UserPortalCategory> UserPortalCategories { get; set; }
        public virtual DbSet<AllOrdersHistory> AllOrdersHistory { get; set; }
        public virtual DbSet<v_wAllOrders> v_wAllOrders { get; set; }
        public virtual DbSet<AtractionsOrders> AtractionsOrders { get; set; }

        // Unable to generate entity type for table 'dbo.MembersDisk'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcMultiRequestFailed'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Realized_ALL'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CouponsStock_ATROrder_06072017'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MembersDisk_for_Insert'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.RequestFiles'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Media'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DTS'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EventimCodes_130916'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.UserPortalCategories_'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.RequestSearchParams'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AUDIT'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CampaignGroupMembers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductsVars_02072017'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.LC_HOLDERS_HIST'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Providers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CardHolderFromFile_11062017'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.InstallmentsRules'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpFamilyStatus'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.HelpGender'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TakalaDetails'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MemberOrder'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Takala'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ATRACTIONSOrdersandWebServiceTransaction_backup'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tmpMemberTerminalExe'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tmpATRACTIONSOrders'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MwcActivitiesLog'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductsVars20160403'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PaymentInfo'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AtractionsOrder_backUp_for_load'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MembersCoinsTemp'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.WebserviceTransactions_Order_backUp_for_load'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TTransactionOrder_FlatProject'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductsVars20161129'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.memberStatus'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._delta_xxx_CardsSkeleton'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AtractionsOrdersBackup_12112017'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductsVarsBeforePricesChanges_12022017'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.OrganizationOperator'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.neveNofess_WST'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AtractionsOrder_backUp_for_load_History'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.neveNofess_att'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.WebserviceTransactions_Order_backUp_for_load_history'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TTransactionOrder_FlatProject_history'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.OverNightDataToLeumiCard'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MessageForMember'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProductVarHelper'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ValueCardTL_ATROrders'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.v_wSumIdenticalMoneyStatic'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TMP_WebServiceTransaction_flat_fix_3011'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.addItay'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._varsclose2'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

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
            modelBuilder.Entity<AppConfig>(entity =>
            {
                entity.HasKey(e => new { e.Key })
                   .HasName("PK__AppConfig");

                entity.Property(e => e.Key)
                    .HasColumnName("Key")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                 .HasColumnName("Value")
                 .HasMaxLength(255)
                 .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClubCreditCardExcluded>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId)
                    .HasMaxLength(9)
                    .ValueGeneratedNever();
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

            });


            modelBuilder.Entity<AllMembers>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId)
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.AbsenceType).HasMaxLength(255);

                entity.Property(e => e.AccessId)
                    .HasColumnName("AccessID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ActiveCardDescription).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.AddressMail).HasMaxLength(100);

                entity.Property(e => e.ApartmentNumber).HasMaxLength(50);

                entity.Property(e => e.AprroveTakanonDate).HasColumnType("datetime");

                entity.Property(e => e.BirthDate).HasColumnType("smalldatetime");

                entity.Property(e => e.CardRequestDate).HasColumnType("datetime");

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.ClubCreditCardUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateOrder).HasColumnType("datetime");

                entity.Property(e => e.Department).HasMaxLength(255);

                entity.Property(e => e.EducationId).HasColumnName("EducationID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmployeeNum).HasMaxLength(50);

                entity.Property(e => e.EncryptedUserPassword).HasMaxLength(64);

                entity.Property(e => e.FactorySymbol).HasMaxLength(50);

                entity.Property(e => e.ForgetPasswordCreated).HasColumnType("datetime");

                entity.Property(e => e.ForgetPasswordToken).HasMaxLength(50);

                entity.Property(e => e.Hamara1Balance).HasColumnType("money");

                entity.Property(e => e.Hodaa).HasMaxLength(255);

                entity.Property(e => e.HouseNumber).HasMaxLength(50);

                entity.Property(e => e.IdentityGuid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastEncryptedUserPassword).HasMaxLength(64);

                entity.Property(e => e.LastLoginAttempts).HasColumnType("datetime");

                entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");

                entity.Property(e => e.LastUpdateMember).HasColumnType("datetime");

                entity.Property(e => e.LoginAttempts).HasDefaultValueSql("((0))");

                entity.Property(e => e.MariageDate).HasColumnType("smalldatetime");

                entity.Property(e => e.MemberCardNumber).HasMaxLength(16);

                entity.Property(e => e.MemberEndDate).HasColumnType("datetime");

                entity.Property(e => e.MemberFirstName).HasMaxLength(50);

                entity.Property(e => e.MemberIdIdentity).ValueGeneratedOnAdd();

                entity.Property(e => e.MemberLastName).HasMaxLength(50);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.MemberSpecialId)
                    .HasColumnName("MemberSpecialID")
                    .HasMaxLength(16);

                entity.Property(e => e.MemberTitle).HasMaxLength(50);

                entity.Property(e => e.MobilePhone).HasMaxLength(13);

                entity.Property(e => e.MoreDetailsXml)
                    .HasColumnName("MoreDetailsXML")
                    .HasColumnType("ntext");

                entity.Property(e => e.OccupationId).HasColumnName("OccupationID");

                entity.Property(e => e.PartnerBirthDate).HasColumnType("smalldatetime");

                entity.Property(e => e.PartnerEmail).HasMaxLength(50);

                entity.Property(e => e.PartnerFirstName).HasMaxLength(50);

                entity.Property(e => e.PartnerId)
                    .HasColumnName("PartnerID")
                    .HasMaxLength(10);

                entity.Property(e => e.PartnerLastName).HasMaxLength(50);

                entity.Property(e => e.PartnerName).HasMaxLength(50);

                entity.Property(e => e.PartnerPhone).HasMaxLength(20);

                entity.Property(e => e.PasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber).HasMaxLength(13);

                entity.Property(e => e.PinCode).HasMaxLength(4);

                entity.Property(e => e.Pobox)
                    .HasColumnName("PObox")
                    .HasMaxLength(10);

                entity.Property(e => e.RegistrationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.SortHanpaka1)
                    .HasColumnName("Sort_hanpaka_1")
                    .HasMaxLength(50);

                entity.Property(e => e.SortHanpaka2)
                    .HasColumnName("Sort_hanpaka_2")
                    .HasMaxLength(50);

                entity.Property(e => e.StreetName).HasMaxLength(255);

                entity.Property(e => e.StreetNumber).HasMaxLength(50);

                entity.Property(e => e.TicketHubPaymentDetailId).HasColumnName("TicketHubPaymentDetailID");

                entity.Property(e => e.Track2).HasMaxLength(37);

                entity.Property(e => e.Tz)
                    .HasColumnName("TZ")
                    .HasMaxLength(9);

                entity.Property(e => e.UserSiteLastLogin).HasColumnType("datetime");

                entity.Property(e => e.UserToken)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.WaitingBalance).HasColumnType("money");

                entity.Property(e => e.WaitingBalanceLeumiCard).HasColumnType("money");

                entity.Property(e => e.Zip)
                    .HasColumnName("ZIP")
                    .HasMaxLength(10);
            });

            //modelBuilder.Entity<RegistrationAudits>(entity =>
            //{
            //    entity.HasKey(e => new { e.ID })
            //        .HasName("PK_RegistrationAudits");

            //    entity.Property(e => e.CreatedDate).HasMaxLength(9);

            //    entity.Property(e => e.CardId).HasMaxLength(20);

            //    entity.Property(e => e.CardExpiration)
            //        .HasColumnName("cardExpiration")
            //        .HasMaxLength(4);

            //    entity.Property(e => e.CardNum).HasMaxLength(4);

            //    entity.Property(e => e.CreationDate).HasColumnType("smalldatetime");

            //    entity.Property(e => e.PayerTz)
            //        .HasColumnName("PayerTZ")
            //        .HasMaxLength(10);
            //});

            modelBuilder.Entity<AllMembersProperties>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.CardId })
                    .HasName("PK__AllMembersProper__1B29035F");

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.CardId).HasMaxLength(20);

                entity.Property(e => e.CardExpiration)
                    .HasColumnName("cardExpiration")
                    .HasMaxLength(4);

                entity.Property(e => e.CardNum).HasMaxLength(4);

                entity.Property(e => e.CreationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.PayerTz)
                    .HasColumnName("PayerTZ")
                    .HasMaxLength(10);
            });

            //modelBuilder.Entity<Amir>(entity =>
            //{
            //    entity.HasKey(e => e.Tz)
            //        .HasName("PK__amir__3214E471003A8DDE");

            //    entity.ToTable("amir");

            //    entity.Property(e => e.Tz)
            //        .HasColumnName("TZ")
            //        .HasMaxLength(50)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.FirstName).HasMaxLength(50);

            //    entity.Property(e => e.LastName).HasMaxLength(50);
            //});

            modelBuilder.Entity<AmountToWallet>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<BehatzdaaNoCards>(entity =>
            {
                entity.HasKey(k => k.Tz);
            });

            modelBuilder.Entity<Atractionsorders>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.BarCode, e.MemberOrderDateExe, e.MemberOrderAsmchta });

                entity.ToTable("ATRACTIONSOrders");

                entity.HasIndex(e => e.BarCode)
                    .HasName("BarCodeIndex");

                entity.HasIndex(e => e.CardNumber)
                    .HasName("idx_Nonclustered_ATRACTIONSOrders_CardNumber");

                entity.HasIndex(e => e.OrderId)
                    .HasName("OrderIdIndex");

                entity.HasIndex(e => e.TicketHubTicketId)
                    .HasName("TicketHubTicketId")
                    .IsUnique()
                    .HasFilter("([TicketHubTicketId] IS NOT NULL)");

                entity.HasIndex(e => new { e.MemberOrderAsmchta, e.OriginalAsmchta })
                    .HasName("IXNC_ATRACTIONSOrders_MemberOrderAsmchta_OriginalAsmchta");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.BarCode).HasMaxLength(50);

                entity.Property(e => e.MemberOrderDateExe)
                    .HasColumnName("MemberOrderDateEXE")
                    .HasColumnType("datetime");

                entity.Property(e => e.MemberOrderAsmchta).ValueGeneratedOnAdd();

                entity.Property(e => e.BarCodeExe)
                    .HasColumnName("BarCodeEXE")
                    .HasMaxLength(50);

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardNumber).HasMaxLength(50);

                entity.Property(e => e.CatalogicPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Hodaa).HasMaxLength(255);

                entity.Property(e => e.IrgunPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.IsInCancelProcess).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastImplementationDate).HasColumnType("datetime");

                entity.Property(e => e.MemberTerminalExe).HasMaxLength(50);

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OriginalMemberId).HasMaxLength(9);

                entity.Property(e => e.ParentMultiVariant).HasMaxLength(50);

                entity.Property(e => e.TransferToFriendDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatePriceDate).HasColumnType("datetime");

                entity.Property(e => e.XmlData).HasMaxLength(255);

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<BenefitFileLog>(entity =>
            {
                entity.HasIndex(e => e.CategoryNumber)
                    .HasName("IX_BenefitFileLog")
                    .IsUnique();

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<BudgetStockCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StockId).HasColumnName("StockID");
            });

            modelBuilder.Entity<BusinessSubTypeLimits>(entity =>
            {
                entity.HasKey(e => e.BusinessSubTypeId);

                entity.Property(e => e.BusinessSubTypeId).ValueGeneratedNever();

                entity.Property(e => e.TerminalId).HasColumnName("TerminalID");
            });

            modelBuilder.Entity<BusinessSubTypeSpecificationAll>(entity =>
            {
                entity.HasKey(e => e.IdentityId);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DaysRangeToCancel).HasDefaultValueSql("((14))");

                entity.Property(e => e.EffictiveDate).HasColumnType("datetime");

                entity.Property(e => e.MinGapBetweenOrgAndCupa)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((15.00))");

                entity.Property(e => e.MinimumProfit)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((5.00))");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<BusinessSubTypeSpecificationByVariants>(entity =>
            {
                entity.Property(e => e.BarCode).HasMaxLength(50);
            });

            modelBuilder.Entity<BusinessSubTypeSpecificationCurrent>(entity =>
            {
                entity.HasKey(e => new { e.BusinessSubTypeId, e.EffictiveDate });

                entity.Property(e => e.EffictiveDate).HasColumnType("datetime");

                entity.Property(e => e.MinGapBetweenOrgAndCupa).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MinimumProfit).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<CampaignMembersUse>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CancelTime).HasColumnType("datetime");

                entity.Property(e => e.CardNumber).HasMaxLength(16);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(16);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.Posid).HasColumnName("POSID");

                entity.Property(e => e.SaleId).HasColumnName("SaleID");

                entity.Property(e => e.SlipMumber).HasMaxLength(50);

                entity.Property(e => e.UseTime).HasColumnType("datetime");

                entity.Property(e => e.Xmldata)
                    .HasColumnName("XMLData")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<CardRequest>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.Submitted });

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.Account).HasMaxLength(9);

                entity.Property(e => e.AccountOwnerName).HasMaxLength(50);

                entity.Property(e => e.AcountOwnerName).HasMaxLength(50);

                entity.Property(e => e.AddressType).HasDefaultValueSql("((1))");

                entity.Property(e => e.Apartmernt).HasMaxLength(10);

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.BankName).HasMaxLength(50);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.BranchId).HasColumnName("BranchID");

                entity.Property(e => e.CityName).HasMaxLength(15);

                entity.Property(e => e.ClientPassword).HasMaxLength(8);

                entity.Property(e => e.ClubCode).HasMaxLength(10);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrentCardType).HasMaxLength(10);

                entity.Property(e => e.DateToLeumiCard).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EncryptedCardNumber).HasMaxLength(256);

                entity.Property(e => e.EnglishFirstName).HasMaxLength(50);

                entity.Property(e => e.EnglishLastName).HasMaxLength(50);

                entity.Property(e => e.Entrance).HasMaxLength(10);

                entity.Property(e => e.ExpirationDate).HasMaxLength(4);

                entity.Property(e => e.FigureFirstname).HasMaxLength(50);

                entity.Property(e => e.FigureLastName).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.HasPicture)
                    .HasColumnName("hasPicture")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InfoPageCode).HasMaxLength(4);

                entity.Property(e => e.IsMailingDiffrent).HasColumnName("isMailingDiffrent");

                entity.Property(e => e.IsPublicFigure).HasColumnName("isPublicFigure");

                entity.Property(e => e.IsSentRequest)
                    .HasColumnName("isSentRequest")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Last4Digits).HasMaxLength(10);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MaidenName).HasMaxLength(50);

                entity.Property(e => e.MailingApartmernt).HasMaxLength(10);

                entity.Property(e => e.MailingCity).HasMaxLength(15);

                entity.Property(e => e.MailingEntrance).HasMaxLength(10);

                entity.Property(e => e.MailingPobox)
                    .HasColumnName("MailingPOBox")
                    .HasMaxLength(10);

                entity.Property(e => e.MailingStreet).HasMaxLength(50);

                entity.Property(e => e.MailingZipCode).HasMaxLength(10);

                entity.Property(e => e.PersonalQuestion).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(10);

                entity.Property(e => e.Picturedate)
                    .HasColumnName("picturedate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Pobox)
                    .HasColumnName("POBox")
                    .HasMaxLength(10);

                entity.Property(e => e.RedirectCard1).HasMaxLength(256);

                entity.Property(e => e.RedirectCard14digits)
                    .HasColumnName("RedirectCard1_4Digits")
                    .HasMaxLength(4);

                entity.Property(e => e.RedirectCard2).HasMaxLength(256);

                entity.Property(e => e.RedirectCard24digits)
                    .HasColumnName("RedirectCard2_4Digits")
                    .HasMaxLength(4);

                entity.Property(e => e.RedirectCard3).HasMaxLength(256);

                entity.Property(e => e.RedirectCard34digits)
                    .HasColumnName("RedirectCard3_4Digits")
                    .HasMaxLength(4);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.Property(e => e.RequestId)
                    .HasColumnName("RequestID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RequestSource).HasMaxLength(10);

                entity.Property(e => e.SharedFirstame).HasMaxLength(50);

                entity.Property(e => e.SharedLastName).HasMaxLength(50);

                entity.Property(e => e.SharedMemberId)
                    .HasColumnName("SharedMemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.Street).HasMaxLength(50);

                entity.Property(e => e.SubRequestSource).HasMaxLength(10);

                entity.Property(e => e.WorkDescription).HasMaxLength(50);

                entity.Property(e => e.WorkPlace)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ללא מקום עבודה')");

                entity.Property(e => e.ZipCode).HasMaxLength(10);
            });

            modelBuilder.Entity<AllowanceHistory>(entity =>
            {
                entity.Property(e => e.MemberID).HasMaxLength(15);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.ClubCreditCard).HasColumnType("bit");

                entity.Property(e => e.IDFResponse).HasColumnType("int");

                entity.Property(e => e.PremiumType).HasColumnType("int");

                entity.Property(e => e.Id)
                   .HasColumnName("ID")
                   .ValueGeneratedOnAdd();

            });

            modelBuilder.Entity<Cards>(entity =>
            {
                entity.HasKey(e => e.CardNumber);

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

                entity.Property(e => e.EncryptedCard).HasMaxLength(256);

                entity.Property(e => e.ExpiredCard).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Idmember)
                    .IsRequired()
                    .HasColumnName("IDMember")
                    .HasMaxLength(9);

                entity.Property(e => e.Remark).HasColumnType("ntext");
            });

            modelBuilder.Entity<CardsSkeleton>(entity =>
            {
                entity.HasKey(e => e.CskeletonId);

                entity.Property(e => e.CskeletonId).HasColumnName("CSkeletonID");

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Cvv).HasColumnName("CVV");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.RcnId).HasColumnName("RcnID");

                entity.Property(e => e.SerieId).HasColumnName("SerieID");

                entity.Property(e => e.Track2Cvv).HasColumnName("Track2CVV");
            });

            modelBuilder.Entity<CategoryVariants>(entity =>
            {
                entity.HasKey(e => new { e.Barcode, e.CategoryNumber });

                entity.HasIndex(e => e.CategoryNumber)
                    .HasName("IXNC_CategoryVariants_CategoryNumber");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CloseDaysorders>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.BarCode, e.MemberOrderDateExe });

                entity.ToTable("CLOSE_DAYSOrders");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.BarCode).HasMaxLength(50);

                entity.Property(e => e.MemberOrderDateExe)
                    .HasColumnName("MemberOrderDateEXE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hodaa).HasMaxLength(255);

                entity.Property(e => e.MimushCharig).HasColumnName("mimushCharig");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<ClubCreditCardLog>(entity =>
            {
                entity.HasIndex(e => e.MemberId)
                    .HasName("ClubCreditCardLog_Ix1");

                entity.Property(e => e.CardOwnerId).HasMaxLength(9);

                entity.Property(e => e.Last4Digit).HasMaxLength(9);

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<CoinsSource>(entity =>
            {
                entity.Property(e => e.Alias).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CouponToCancel>(entity =>
            {
                entity.Property(e => e.CouponCode).HasMaxLength(10);

                entity.Property(e => e.MemberId).HasMaxLength(9);
            });

            modelBuilder.Entity<CouponsStockAtrorders>(entity =>
            {
                entity.ToTable("CouponsStock_ATROrders");

                entity.HasIndex(e => e.Asmachta)
                    .HasName("IX_CouponsStock_ATROrders__Asmachta");

                entity.HasIndex(e => e.CouponStockId)
                    .HasName("IX_CouponsStock_ATROrders__CouponStockID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CouponStockId).HasColumnName("CouponStockID");
            });

            modelBuilder.Entity<CreditCardHolders>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateSent).HasColumnType("datetime");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Tz)
                    .IsRequired()
                    .HasColumnName("TZ")
                    .HasMaxLength(9);
            });

            modelBuilder.Entity<DwhCustomersChangeLog>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("DWH_CustomersChangeLogPK");

                entity.ToTable("DWH_CustomersChangeLog");

                entity.Property(e => e.RowId).HasColumnName("RowID");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasMaxLength(50);

                entity.Property(e => e.DataBaseName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Histadrut')");

                entity.Property(e => e.Login).HasColumnType("datetime");
            });

            modelBuilder.Entity<DwhProductsChangeLog>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("DWH_ProductsChangeLogPK");

                entity.ToTable("DWH_ProductsChangeLog");

                entity.Property(e => e.RowId).HasColumnName("RowID");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataBaseName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Histadrut')");

                entity.Property(e => e.Inactive).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductProviderId).HasColumnName("ProductProviderID");
            });

            modelBuilder.Entity<DwhTransactions>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__DWH_Tran__FFEE74513CD32335");

                entity.ToTable("DWH_Transactions");

                entity.HasIndex(e => new { e.RecordId, e.ProductId, e.TerminalId, e.Quantity, e.Created, e.TableName })
                    .HasName("IX_DWH_Transactions_TableName");

                entity.Property(e => e.RowId).HasColumnName("RowID");

                entity.Property(e => e.BranchId)
                    .HasColumnName("BranchID")
                    .HasMaxLength(50);

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreditCardId).HasColumnName("CreditCardID");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasMaxLength(50);

                entity.Property(e => e.DataBaseName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Histadrut')");

                entity.Property(e => e.Expired).HasColumnType("datetime");

                entity.Property(e => e.MerchantId)
                    .HasColumnName("MerchantID")
                    .HasMaxLength(50);

                entity.Property(e => e.OtherTransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasMaxLength(50);

                entity.Property(e => e.RecordId).HasColumnName("RecordID");

                entity.Property(e => e.TableName).HasMaxLength(50);

                entity.Property(e => e.TerminalId)
                    .HasColumnName("TerminalID")
                    .HasMaxLength(50);

                entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.Vat)
                    .HasColumnName("VAT")
                    .HasDefaultValueSql("((0.17))");
            });

            modelBuilder.Entity<EmailSubscription>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Errors>(entity =>
            {
                entity.HasKey(e => e.ErrorId);

                entity.Property(e => e.Alias).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Text).HasMaxLength(255);
            });

            modelBuilder.Entity<EventimMemberIdTemp>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK__EventimM__0CF04B1829B5B4F4");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<FriendBringsFriend>(entity =>
            {
                entity.Property(e => e.AddedClubMemberFirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AddedClubMemberId)
                    .IsRequired()
                    .HasColumnName("AddedClubMemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.AddedClubMemberLastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AddedClubMemberPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.AddingClubMemberFirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AddingClubMemberId)
                    .IsRequired()
                    .HasColumnName("AddingClubMemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.AddingClubMemberLastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AddingClubMemberPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Hanpaka>(entity =>
            {
                entity.HasIndex(e => new { e.MemberId, e.CskeletonId });

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardNum).HasMaxLength(16);

                entity.Property(e => e.ChargeAmount).HasColumnType("money");

                entity.Property(e => e.CskeletonId).HasColumnName("CSkeletonID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.InsertDate)
                    .HasColumnName("insertDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemberAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MemberCiryName).HasMaxLength(50);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.MemberPhoneNumber).HasMaxLength(13);

                entity.Property(e => e.MemberZipCode).HasMaxLength(10);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<HelpPopulationType>(entity =>
            {
                entity.HasKey(e => e.PopulationTypeId);

                entity.Property(e => e.PopulationTypeId).HasColumnName("PopulationTypeID");

                entity.Property(e => e.PopulationTypeDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<HelpPremiumType>(entity =>
            {
                entity.HasKey(e => e.PremiumTypeId);

                entity.Property(e => e.PremiumTypeId)
                    .HasColumnName("PremiumTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.PremiumTypeDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<LastRunServiceUpdateMembers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Info).HasMaxLength(100);

                entity.Property(e => e.LastRunTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<LmPayRate>(entity =>
            {
                entity.ToTable("LM_PayRate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DiscountLeverage)
                    .IsRequired()
                    .HasColumnName("Discount_Leverage")
                    .HasMaxLength(2);

                entity.Property(e => e.Type).HasMaxLength(1);
            });

            modelBuilder.Entity<Lminfo>(entity =>
            {
                entity.HasKey(e => e.UniquId)
                    .HasName("PK__LMInfo__08162EEB");

                entity.ToTable("LMInfo");

                entity.Property(e => e.UniquId)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.AmountCharge).HasMaxLength(20);

                entity.Property(e => e.Amout).HasMaxLength(20);

                entity.Property(e => e.AuthNumber).HasMaxLength(20);

                entity.Property(e => e.CardAcquirer)
                    .HasColumnName("cardAcquirer")
                    .HasMaxLength(10);

                entity.Property(e => e.CardBalance).HasMaxLength(50);

                entity.Property(e => e.CardBrand)
                    .HasColumnName("cardBrand")
                    .HasMaxLength(10);

                entity.Property(e => e.CardMask).HasMaxLength(20);

                entity.Property(e => e.CardNumber).HasMaxLength(50);

                entity.Property(e => e.CardStatus).HasMaxLength(20);

                entity.Property(e => e.CardToken).HasMaxLength(20);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreditNet)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DealTime).HasColumnType("datetime");

                entity.Property(e => e.LoadingType).HasMaxLength(10);

                entity.Property(e => e.Masof).HasMaxLength(10);

                entity.Property(e => e.MemberId).HasMaxLength(50);

                entity.Property(e => e.Nshidor).HasMaxLength(10);

                entity.Property(e => e.Org).HasMaxLength(10);

                entity.Property(e => e.PersonalId).HasMaxLength(20);

                entity.Property(e => e.PraxellResult)
                    .HasColumnName("Praxell_Result")
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(10);

                entity.Property(e => e.SumDeal).HasMaxLength(20);

                entity.Property(e => e.TransactionId).HasMaxLength(50);

                entity.Property(e => e.ValueTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<LoadCards>(entity =>
            {
                entity.Property(e => e.CardNumberNew).HasMaxLength(16);

                entity.Property(e => e.CardNumberOld)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Idm)
                    .IsRequired()
                    .HasColumnName("idm")
                    .HasMaxLength(9);
            });

            modelBuilder.Entity<MemberAddress>(entity =>
            {
                entity.HasKey(e => e.MemberAddressId).HasName("PK_MemberAd_AD5BA671059BB341");

                entity.Property(e => e.ApartmentNumber).HasMaxLength(50);
                entity.Property(e => e.CityName).HasMaxLength(50);
                entity.Property(e => e.Entrance)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.HouseNumber).HasMaxLength(50);
                entity.Property(e => e.InsertDate).HasColumnType("datetime");
                entity.Property(e => e.Mailbox)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.MemberId)
                    .HasMaxLength(9)
                    .IsFixedLength();
                entity.Property(e => e.StreetName).HasMaxLength(255);
                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
                entity.Property(e => e.Zip)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .HasColumnName("ZIP");
            });

            modelBuilder.Entity<LoginFail>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId)
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MemberTempTable>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK__MemberTe__0CF04B18420DC656");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.DateCreate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MembersAllowedLogin>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(9)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<MemberCodes>(entity =>
            {
                entity.HasIndex(e => new { e.MemberId, e.Code })
                    .HasName("MemberCodes_MemberId_Code");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberCodes)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberCod__Membe__3E65B398");
            });

            modelBuilder.Entity<MembersCoins>(entity =>
            {
                entity.HasKey(e => e.MemberCoinsId);

                entity.HasIndex(e => e.TransactionGuid)
                    .HasName("IX_MembersCoins");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.TransactionGuid)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TtransactionId).HasColumnName("TTransactionId");

                entity.HasOne(d => d.CoinsSource)
                    .WithMany(p => p.MembersCoins)
                    .HasForeignKey(d => d.CoinsSourceId)
                    .HasConstraintName("FK_MembersCoins_CoinsSource");
            });

            modelBuilder.Entity<MembersCoinsArchive>(entity =>
            {
                entity.HasKey(e => e.MemberCoinsId)
                    .HasName("PK__MembersC__496E7F1A85894E82");

                entity.HasIndex(e => e.TransactionGuid)
                    .HasName("IX_MembersCoinsArchive");

                entity.Property(e => e.MemberCoinsId).ValueGeneratedNever();

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.TransactionGuid)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TtransactionId).HasColumnName("TTransactionId");
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

            modelBuilder.Entity<MonthlyStockAmounts>(entity =>
            {
                entity.HasKey(e => e.Identity);

                entity.Property(e => e.Identity).HasColumnName("identity");

                entity.Property(e => e.DateToAddStock).HasColumnType("datetime");

                entity.Property(e => e.StockAmount).HasColumnName("stockAmount");

                entity.Property(e => e.StockId).HasColumnName("stockID");
            });

            modelBuilder.Entity<MovieOrdersToWebOrders>(entity =>
            {
                entity.HasKey(e => new { e.WebOrderAsmachta, e.MovieOrderAsmachta });

                entity.HasIndex(e => new { e.MovieOrderAsmachta, e.WebOrderAsmachta })
                    .HasName("IX_indexMovieOrdersToWebOrders__MovieOrderAsmachta_WebOrderAsmachta");
            });

            modelBuilder.Entity<Moviesorders>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.OrderAsmchta });

                entity.ToTable("MOVIESOrders");

                entity.HasIndex(e => e.OrderAsmchta)
                    .HasName("IXNC_MOVIESOrders_OrderAsmchta");

                entity.HasIndex(e => new { e.MemberId, e.OrderAsmchta, e.TerminalExe, e.CatalogicPrice, e.IrgunPrice, e.OriginalAsmchta, e.OrderDateExe, e.OrderBalance })
                    .HasName("IX_MOVIESOrders_OrderDateExe_OrderBalance");

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.OrderAsmchta).ValueGeneratedOnAdd();

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BarCodeExe)
                    .HasColumnName("BarCodeEXE")
                    .HasMaxLength(50);

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardNumber).HasMaxLength(16);

                entity.Property(e => e.CatalogicPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.IrgunPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.LastImplementationDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDateExe).HasColumnType("datetime");

                entity.Property(e => e.OriginalMemberId).HasMaxLength(9);

                entity.Property(e => e.ParentMultiVariant).HasMaxLength(50);

                entity.Property(e => e.TerminalExe).HasMaxLength(50);

                entity.Property(e => e.TransferToFriendDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatePriceDate).HasColumnType("datetime");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<MultiVariant>(entity =>
            {
                entity.Property(e => e.MultiVariantId).HasColumnName("MultiVariantID");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<MultiVariantGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.GroupBarcode).HasMaxLength(50);

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MwcHanpakaRequests>(entity =>
            {
                entity.HasKey(e => e.Hrid)
                    .HasName("PK_Mwchr");

                entity.HasIndex(e => new { e.SerieId, e.HanpakaToSerie })
                    .HasName("UNIQUE_HanpakaToSerie")
                    .IsUnique();

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExecuteDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.HanpakaName).HasMaxLength(50);

                entity.Property(e => e.Hrdescription)
                    .IsRequired()
                    .HasColumnName("HRDescription")
                    .HasMaxLength(255);

                entity.Property(e => e.Hrstatus).HasColumnName("HRStatus");

                entity.Property(e => e.Opid).HasColumnName("OPid");

                entity.Property(e => e.SerieId).HasColumnName("SerieID");
            });

            modelBuilder.Entity<MwcMultiRequest>(entity =>
            {
                entity.HasKey(e => e.MwcRequestId)
                    .HasName("PK_MultiOrdersRequest");

                entity.Property(e => e.MwcRequestId).HasColumnName("MwcRequestID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ExecuteAmount).HasDefaultValueSql("((0))");

                entity.Property(e => e.ExecuteDate).HasColumnType("datetime");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.MwcRequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MwcRequestTypeId).HasColumnName("MwcRequestTypeID");

                entity.Property(e => e.Opid).HasColumnName("OPid");

                entity.Property(e => e.PopulationTypeId).HasColumnName("PopulationTypeID");

                entity.Property(e => e.RequestDescription).HasMaxLength(255);

                entity.Property(e => e.WalletId).HasColumnName("WalletID");
            });

            modelBuilder.Entity<OrderTransfer>(entity =>
            {
                entity.Property(e => e.Blessing).HasMaxLength(255);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.FromMemberId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FromMemberName).HasMaxLength(50);

                entity.Property(e => e.MediaImgUrl).HasMaxLength(255);

                entity.Property(e => e.OpenDate).HasColumnType("datetime");

                entity.Property(e => e.TimeToSend).HasColumnType("datetime");

                entity.Property(e => e.ToMemberEmail).HasMaxLength(50);

                entity.Property(e => e.ToMemberId).HasMaxLength(50);

                entity.Property(e => e.ToMemberMobilePhone).HasMaxLength(13);

                entity.Property(e => e.ToMemberName).HasMaxLength(50);
            });


            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.HasIndex(e => e.OrderId)
                    .HasName("OrderIdIndex");

                entity.HasIndex(e => e.TicketHubOrderId)
                    .HasName("TicketHubOrderId")
                    .IsUnique()
                    .HasFilter("([TicketHubOrderId] IS NOT NULL)");

                entity.Property(e => e.CreditCard16Digits)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardExpirey)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardToken)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventsGuid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalGuid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FriendMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FriendName).HasMaxLength(50);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.OrderReminderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentToken)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Slink).HasMaxLength(250);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_AllMembers");
            });

            modelBuilder.Entity<OrdersStatus>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(30);
            });

            modelBuilder.Entity<OrganizationOps>(entity =>
            {
                entity.HasKey(e => e.Opid);

                entity.ToTable("OrganizationOPs");

                entity.Property(e => e.Opid).HasColumnName("OPId");

                entity.Property(e => e.DateAdded)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.FirstName).HasMaxLength(20);

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

                entity.Property(e => e.Optype).HasColumnName("OPType");

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

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.HasIndex(e => e.TimeStamp)
                    .HasName("IX_Payments__TimeStamp");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.CardOwnerId)
                    .IsRequired()
                    .HasColumnName("CardOwnerID")
                    .HasMaxLength(9);

                entity.Property(e => e.Charged).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.ConfirmationNumber)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Last4Digits)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(16);

                entity.Property(e => e.OperatorCommission)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrganizationCommission)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PointsCashValue)
                    .HasColumnType("decimal(7, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PointsUsed).HasDefaultValueSql("((0))");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.TrustProgramCommission).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UniquId).HasMaxLength(20);

                entity.Property(e => e.Xml).HasColumnType("xml");
            });

            modelBuilder.Entity<PortalCategoriesStatusChanges>(entity =>
            {
                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnCodeDesc).HasMaxLength(500);

                entity.Property(e => e.ReturnErrorName).HasMaxLength(200);

                entity.Property(e => e.SentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PortalCategoriesStatuses>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
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

            modelBuilder.Entity<ProductsVars>(entity =>
            {
                entity.HasKey(e => e.FullBarCode)
                    .HasName("PK_ProductsVars_1");

                entity.HasIndex(e => e.FullBarCode)
                    .HasName("FullBarCodeIndex");

                entity.HasIndex(e => e.HatavaMultiplier);

                entity.HasIndex(e => new { e.Type, e.TrustProgram })
                    .HasName("IX_ProductsVars__TrustProgram");

                entity.HasIndex(e => new { e.FullBarCode, e.LastImplementationDate, e.VariantType, e.BusinessSubTypeId })
                    .HasName("IX_ProductsVars_BusinessSubTypeID");

                entity.HasIndex(e => new { e.FullBarCode, e.PresentationCode, e.VarName, e.BusinessSubTypeId })
                    .HasName("IX_ProductsVars__PresentationCode_VarName_BusinessSubTypeID");

                entity.Property(e => e.FullBarCode)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

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

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EventimPriceLevelName).HasMaxLength(255);

                entity.Property(e => e.EventimTicketTypeName).HasMaxLength(255);

                entity.Property(e => e.GroupLimitsId).HasColumnName("GroupLimitsID");

                entity.Property(e => e.HatavaMultiplier).HasDefaultValueSql("((1))");

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

                entity.Property(e => e.Opid).HasColumnName("OPid");

                entity.Property(e => e.OrgPayer).HasColumnName("orgPayer");

                entity.Property(e => e.PosbarCode)
                    .HasColumnName("POSBarCode")
                    .HasMaxLength(50);

                entity.Property(e => e.PresentTypeId).HasColumnName("PresentTypeID");

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

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.SectionCode).HasMaxLength(30);

                entity.Property(e => e.SellEndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.SendReportDate).HasColumnType("datetime");

                entity.Property(e => e.ShortNameVar)
                    .HasColumnName("shortNameVar")
                    .HasMaxLength(50);

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.Property(e => e.Slink).HasMaxLength(500);

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Type).HasMaxLength(10);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.VarComissionFormula).HasMaxLength(500);

                entity.Property(e => e.VarDiscountFormula).HasMaxLength(500);

                entity.Property(e => e.VarName).HasMaxLength(250);

                entity.Property(e => e.VarNameApp).HasMaxLength(25);

                entity.Property(e => e.VarPriceFormula).HasMaxLength(500);

                entity.Property(e => e.Vat).HasColumnName("VAT");
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

            modelBuilder.Entity<Requests>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK_RequestsB");

                entity.HasIndex(e => new { e.Id2, e.Id1 })
                    .HasName("IX__RequestsB__ID2_ID1");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Card1).HasMaxLength(16);

                entity.Property(e => e.Card2).HasMaxLength(16);

                entity.Property(e => e.FileRequestId).HasColumnName("FileRequestID");

                entity.Property(e => e.Id1)
                    .HasColumnName("ID1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id2)
                    .HasColumnName("ID2")
                    .HasMaxLength(9);

                entity.Property(e => e.LeumiCardFileIdprika).HasColumnName("LeumiCardFileIDPrika");

                entity.Property(e => e.LeumiCardFileIdteina).HasColumnName("LeumiCardFileIDTeina");

                entity.Property(e => e.ManualTime).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasColumnType("ntext");

                entity.Property(e => e.RequestTime).HasColumnType("datetime");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");

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

            modelBuilder.Entity<Sales>(entity =>
            {
                entity.HasKey(e => e.SaleId);

                entity.Property(e => e.SaleId).HasColumnName("SaleID");

                entity.Property(e => e.CardNumber).HasMaxLength(16);

                entity.Property(e => e.CashBack1Before).HasColumnType("money");

                entity.Property(e => e.CashBack1Get).HasColumnType("money");

                entity.Property(e => e.CashBack1Use).HasColumnType("money");

                entity.Property(e => e.CashBack2Before).HasColumnType("money");

                entity.Property(e => e.CashBack2Get).HasColumnType("money");

                entity.Property(e => e.CashBack2Use).HasColumnType("money");

                entity.Property(e => e.CashBack3Before).HasColumnType("money");

                entity.Property(e => e.CashBack4Before).HasColumnType("money");

                entity.Property(e => e.Discount).HasColumnType("money");

                entity.Property(e => e.FromIp)
                    .HasColumnName("FromIP")
                    .HasMaxLength(15);

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.MerchantId).HasColumnName("MerchantID");

                entity.Property(e => e.OriginalIp)
                    .HasColumnName("OriginalIP")
                    .HasMaxLength(15);

                entity.Property(e => e.Posid).HasColumnName("POSID");

                entity.Property(e => e.SaleTime).HasColumnType("datetime");

                entity.Property(e => e.SlipNo).HasMaxLength(50);

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.TransNo).HasMaxLength(10);

                entity.Property(e => e.TransSubNo).HasMaxLength(10);

                entity.Property(e => e.Xmldata)
                    .HasColumnName("XMLData")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<SalesLines>(entity =>
            {
                entity.HasKey(e => e.LineId);

                entity.Property(e => e.LineId).HasColumnName("LineID");

                entity.Property(e => e.Barcode).HasMaxLength(30);

                entity.Property(e => e.Discount).HasColumnType("money");

                entity.Property(e => e.LineTotal).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.SaleId).HasColumnName("SaleID");
            });

            modelBuilder.Entity<ShopingBasket>(entity =>
            {
                entity.HasIndex(e => e.MemberId)
                    .HasName("IXNC_ShopingBasket_MemberID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryNumber).HasMaxLength(10);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Expired).HasColumnType("datetime");

                entity.Property(e => e.FinalPrice)
                    .HasColumnName("finalPrice")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasColumnName("MemberID")
                    .HasMaxLength(9);

                entity.Property(e => e.ProductBarcode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.XmlParams).HasColumnType("ntext");
            });

            modelBuilder.Entity<ShortCardNumbers>(entity =>
            {
                entity.HasKey(e => e.ShortNumber)
                    .HasName("PK_dbo.ShortCardNumbers");

                entity.Property(e => e.AssignDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SiteConfiguration>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Context)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SpaOrdersToWebOrders>(entity =>
            {
                entity.HasKey(e => new { e.WebOrderAsmachta, e.SpaOrderAsmachta });
            });

            modelBuilder.Entity<Spaorders>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.OrderAsmchta });

                entity.ToTable("SPAOrders");

                entity.HasIndex(e => e.OrderAsmchta);

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.OrderAsmchta).ValueGeneratedOnAdd();

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BarCodeExe)
                    .HasColumnName("BarCodeEXE")
                    .HasMaxLength(50);

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardNumber).HasMaxLength(16);

                entity.Property(e => e.CatalogicPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Hodaa).HasMaxLength(255);

                entity.Property(e => e.IrgunPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.LastImplementationDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderDateExe)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)/(1))/(1900))");

                entity.Property(e => e.OriginalMemberId).HasMaxLength(9);

                entity.Property(e => e.ParentMultiVariant).HasMaxLength(50);

                entity.Property(e => e.TerminalExe).HasMaxLength(50);

                entity.Property(e => e.TransferToFriendDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatePriceDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<SubsidyPrecent>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.SubsidyPrecent1)
                    .HasColumnName("SubsidyPrecent")
                    .HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<TerminalActive>(entity =>
            {
                entity.HasKey(e => e.TerminalNo);

                entity.Property(e => e.TerminalNo)
                    .HasMaxLength(7)
                    .ValueGeneratedNever();

                entity.Property(e => e.DateActive).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<TitanParseCode>(entity =>
            {
                entity.HasKey(e => e.TitanCode);

                entity.Property(e => e.TitanCode)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.DtsType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Remark).HasMaxLength(50);
            });

            modelBuilder.Entity<TzimerOrdersToWebOrders>(entity =>
            {
                entity.HasKey(e => new { e.WebOrderAsmachta, e.TzimerOrderAsmachta });
            });

            modelBuilder.Entity<Tzimersorders>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.OrderAsmchta });

                entity.ToTable("TZIMERSOrders");

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.OrderAsmchta).ValueGeneratedOnAdd();

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BarCodeExe)
                    .HasColumnName("BarCodeEXE")
                    .HasMaxLength(50);

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CardNumber).HasMaxLength(16);

                entity.Property(e => e.CatalogicPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Hodaa).HasMaxLength(255);

                entity.Property(e => e.IrgunPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.IsInCancelProcess).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastImplementationDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDateExe).HasColumnType("datetime");

                entity.Property(e => e.OriginalMemberId).HasMaxLength(9);

                entity.Property(e => e.ParentMultiVariant).HasMaxLength(50);

                entity.Property(e => e.TerminalExe).HasMaxLength(50);

                entity.Property(e => e.TransferToFriendDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatePriceDate).HasColumnType("datetime");

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<VariantGroupLimits>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(1024)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VerifonTransactionLog>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("Verifon_TransactionLog");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.AmountReq)
                    .HasColumnName("Amount_Req")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CardId)
                    .HasColumnName("CardID")
                    .HasMaxLength(20);

                entity.Property(e => e.ErrorDesc)
                    .HasColumnName("Error_Desc")
                    .HasMaxLength(50);

                entity.Property(e => e.ReqType)
                    .HasColumnName("Req_Type")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusReq).HasColumnName("Status_Req");

                entity.Property(e => e.VerifonTransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.WalletReq).HasColumnName("Wallet_Req");
            });

            modelBuilder.Entity<WalletLoadMoney>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.CheckLeumiRespone).HasMaxLength(10);

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

                entity.Property(e => e.WalletPresentMore).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<WalletMembers>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK_MemberWallet");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.OrgBalance).HasColumnType("money");

                entity.Property(e => e.PrePayMoney).HasColumnType("money");

                entity.Property(e => e.RefundMoney).HasColumnType("money");

                entity.Property(e => e.WalletMembersId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<WalletMembersDetails>(entity =>
            {
                entity.HasIndex(e => new { e.MemberId, e.PaymentId, e.StatusPayment });

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.OrganizationMoney).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PrePayMoney)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

                entity.Property(e => e.RefundMoney).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalMoney).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<WebServiceTransaction>(entity =>
            {
                entity.HasKey(e => e.TtransactionId)
                    .HasName("PK_WebServiceTransaction_1");

                entity.HasIndex(e => e.TtransactionDateTime)
                    .HasName("IX_WebServiceTransaction_TTransactionID");

                entity.HasIndex(e => e.TtransactionOrder)
                    .HasName("IX_WebServiceTransaction");

                entity.HasIndex(e => new { e.TtransactionProductId, e.TtransactionOrder });

                entity.HasIndex(e => new { e.TtransactionDateTime, e.TtransactionMemberId, e.Ttransactionquantity, e.TtransactionOrder, e.IrgunPrice, e.CustomerPrice, e.TtransactionProductId })
                    .HasName("IX_WebServiceTransaction_TTransactionProductID");

                entity.HasIndex(e => new { e.Ttransactionquantity, e.TtransactionMemberId, e.TtransactionId, e.TtransactionProductId, e.CustomerPrice, e.TtransactionOrder, e.TtransactionDateTime, e.PaymentId, e.IrgunPrice, e.MarketingCommission })
                    .HasName("IX_WebServiceTransaction_A");

                entity.Property(e => e.TtransactionId).HasColumnName("TTransactionID");

                entity.Property(e => e.CancelCommission).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CatalogicPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CustomerDiscount).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CustomerPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DistributionComission).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.GroupLimitsId).HasColumnName("GroupLimitsID");

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.IrgunPrice).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Iscampaign).HasColumnName("ISCampaign");

                entity.Property(e => e.MarketingCommission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OperatorCommission).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.OrganizationCommission).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("PaymentID")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.TtransactionAsmcta)
                    .HasColumnName("TTransactionAsmcta")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TtransactionDateTime)
                    .HasColumnName("TTransactionDateTime")
                    .HasColumnType("datetime");

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

                entity.Property(e => e.TtransactionStatus).HasColumnName("TTransactionStatus");

                entity.Property(e => e.Ttransactionquantity)
                    .HasColumnName("TTransactionquantity")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Xmlparam)
                    .HasColumnName("XMLParam")
                    .HasColumnType("ntext");
            });

            modelBuilder.HasSequence("MemberSequence").StartsAt(100000);
        }
    }
}
