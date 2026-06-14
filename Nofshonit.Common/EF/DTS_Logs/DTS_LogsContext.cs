using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public partial class DTS_LogsContext : DbContext
    {
        public DTS_LogsContext()
        {
        }

        public DTS_LogsContext(DbContextOptions<DTS_LogsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiLogs> ApiLogs { get; set; }
        public virtual DbSet<ArchivePaymentInquireTransaction> ArchivePaymentInquireTransaction { get; set; }
        public virtual DbSet<ArchivePaymentTransaction> ArchivePaymentTransaction { get; set; }
        public virtual DbSet<DataCenter> DataCenter { get; set; }
        public virtual DbSet<DtsService> DtsService { get; set; }
        public virtual DbSet<EntityChangeLog> EntityChangeLog { get; set; }
        public virtual DbSet<EntityLogTypes> EntityLogTypes { get; set; }
        public virtual DbSet<PaymentCreditCompany> PaymentCreditCompany { get; set; }
        public virtual DbSet<PaymentInquireTransaction> PaymentInquireTransaction { get; set; }
        public virtual DbSet<PaymentRefundOld> PaymentRefundOld { get; set; }
        public virtual DbSet<PaymentStatus> PaymentStatus { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransaction { get; set; }
        public virtual DbSet<PaymentType> PaymentType { get; set; }
        public virtual DbSet<ReferralHistory> ReferralHistory { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ContainerManager.Container.Resolve<IConfigurationManager>().GetConnectionStringByValue<string>("DTSLogsContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<ApiLogs>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__ApiLogs__5E5486487C905E13");

                entity.Property(e => e.ClientIp)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.MethodName)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.QueryParams).HasMaxLength(1024);

                entity.Property(e => e.ServerIp)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ArchivePaymentInquireTransaction>(entity =>
            {
                entity.HasKey(e => e.PaymentInquireTransactionId);

                entity.ToTable("Archive_PaymentInquireTransaction");

                entity.Property(e => e.PaymentInquireTransactionId)
                    .HasColumnName("PaymentInquireTransactionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ClosingRequestTime).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(100);

                entity.Property(e => e.OpenRequestTime).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PaymentCreditCompnayId).HasColumnName("PaymentCreditCompnayID");

                entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");

                entity.Property(e => e.PaymentTransactionId).HasColumnName("PaymentTransactionID");

                entity.Property(e => e.ServerStatusCode).HasMaxLength(10);

                entity.Property(e => e.ServerTransactionId)
                    .HasColumnName("ServerTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.TerminalNumber).HasMaxLength(50);

                entity.Property(e => e.UniqueRequestId)
                    .IsRequired()
                    .HasColumnName("UniqueRequestID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ArchivePaymentTransaction>(entity =>
            {
                entity.HasKey(e => e.PaymentTransactionId);

                entity.ToTable("Archive_PaymentTransaction");

                entity.Property(e => e.PaymentTransactionId)
                    .HasColumnName("PaymentTransactionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CardToken).HasMaxLength(100);

                entity.Property(e => e.ClosingRequestTime).HasColumnType("datetime");

                entity.Property(e => e.Last4DigitCard).HasMaxLength(4);

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(100);

                entity.Property(e => e.OpenRequestTime).HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PaymentCreditCompnayId).HasColumnName("PaymentCreditCompnayID");

                entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.RefundServerTransactionId)
                    .HasColumnName("RefundServerTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.ServerStatusCode).HasMaxLength(10);

                entity.Property(e => e.ServerTransactionId)
                    .HasColumnName("ServerTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.TerminalNumber).HasMaxLength(50);

                entity.Property(e => e.UniqueRequestId)
                    .IsRequired()
                    .HasColumnName("UniqueRequestID")
                    .HasMaxLength(19);
            });

            modelBuilder.Entity<DataCenter>(entity =>
            {
                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("IP")
                    .HasMaxLength(50);

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DtsService)
                    .WithMany(p => p.DataCenter)
                    .HasForeignKey(d => d.DtsServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DataCenter_DtsService");
            });

            modelBuilder.Entity<DtsService>(entity =>
            {
                entity.Property(e => e.DtsServiceId).ValueGeneratedNever();

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<EntityChangeLog>(entity =>
            {
                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.EntityLogType)
                    .WithMany(p => p.EntityChangeLog)
                    .HasForeignKey(d => d.EntityLogTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntityChangeLog_EntityLogTypes");
            });

            modelBuilder.Entity<EntityLogTypes>(entity =>
            {
                entity.HasKey(e => e.EntityLogTypeId);

                entity.HasIndex(e => e.Alias)
                    .HasName("IX_EntityLogTypes")
                    .IsUnique();

                entity.Property(e => e.Alias).HasMaxLength(50);

                entity.Property(e => e.EntityIdDescription).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentCreditCompany>(entity =>
            {
                entity.Property(e => e.PaymentCreditCompanyId).HasColumnName("PaymentCreditCompanyID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentInquireTransaction>(entity =>
            {
                entity.Property(e => e.PaymentInquireTransactionId).HasColumnName("PaymentInquireTransactionID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ClosingRequestTime).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(100);

                entity.Property(e => e.OpenRequestTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PaymentCreditCompnayId).HasColumnName("PaymentCreditCompnayID");

                entity.Property(e => e.PaymentStatusId)
                    .HasColumnName("PaymentStatusID")
                    .HasDefaultValueSql("((4))");

                entity.Property(e => e.PaymentTransactionId).HasColumnName("PaymentTransactionID");

                entity.Property(e => e.ServerStatusCode).HasMaxLength(10);

                entity.Property(e => e.ServerTransactionId)
                    .HasColumnName("ServerTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.TerminalNumber).HasMaxLength(50);

                entity.Property(e => e.UniqueRequestId)
                    .IsRequired()
                    .HasColumnName("UniqueRequestID")
                    .HasMaxLength(50);

                entity.HasOne(d => d.PaymentCreditCompnay)
                    .WithMany(p => p.PaymentInquireTransaction)
                    .HasForeignKey(d => d.PaymentCreditCompnayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentInquireTransaction_PaymentCreditCompany");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.PaymentInquireTransaction)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentInquireTransaction_PaymentStatus");

                entity.HasOne(d => d.PaymentTransaction)
                    .WithMany(p => p.PaymentInquireTransaction)
                    .HasForeignKey(d => d.PaymentTransactionId)
                    .HasConstraintName("FK_PaymentInquireTransaction_PaymentTransaction");
            });

            modelBuilder.Entity<PaymentRefundOld>(entity =>
            {
                entity.HasKey(e => e.PaymentRefundId)
                    .HasName("PK_PaymentRefund");

                entity.ToTable("PaymentRefund_old");

                entity.Property(e => e.PaymentRefundId).HasColumnName("PaymentRefundID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AuthNumber).HasMaxLength(50);

                entity.Property(e => e.ClosingRequestTime).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(100);

                entity.Property(e => e.OpenRequestTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.ReferPaymentId).HasColumnName("ReferPaymentID");

                entity.Property(e => e.ServerStatusCode).HasMaxLength(10);

                entity.Property(e => e.ServerTransactionId)
                    .HasColumnName("ServerTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((4))");

                entity.Property(e => e.TerminalNumber).HasMaxLength(50);

                entity.Property(e => e.UniqueRequestId)
                    .IsRequired()
                    .HasColumnName("UniqueRequestID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentStatus>(entity =>
            {
                entity.Property(e => e.PaymentStatusId)
                    .HasColumnName("PaymentStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasIndex(e => e.UniqueRequestId)
                    .HasName("IX_PaymentTransaction")
                    .IsUnique();

                entity.Property(e => e.PaymentTransactionId).HasColumnName("PaymentTransactionID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CardToken).HasMaxLength(100);

                entity.Property(e => e.ClosingRequestTime).HasColumnType("datetime");

                entity.Property(e => e.Last4DigitCard).HasMaxLength(4);

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberID")
                    .HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(100);

                entity.Property(e => e.OpenRequestTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.PaymentCreditCompnayId).HasColumnName("PaymentCreditCompnayID");

                entity.Property(e => e.PaymentStatusId)
                    .HasColumnName("PaymentStatusID")
                    .HasDefaultValueSql("((4))");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.RefundServerTransactionId)
                    .HasColumnName("RefundServerTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.ServerStatusCode).HasMaxLength(10);

                entity.Property(e => e.ServerTransactionId)
                    .HasColumnName("ServerTransactionID")
                    .HasMaxLength(50);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.TerminalNumber).HasMaxLength(50);

                entity.Property(e => e.UniqueRequestId)
                    .IsRequired()
                    .HasColumnName("UniqueRequestID")
                    .HasMaxLength(19);

                entity.HasOne(d => d.PaymentCreditCompnay)
                    .WithMany(p => p.PaymentTransaction)
                    .HasForeignKey(d => d.PaymentCreditCompnayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentTransaction_PaymentCreditCompany");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.PaymentTransaction)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentTransaction_PaymentStatus");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.PaymentTransaction)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentTransaction_PaymentType");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
