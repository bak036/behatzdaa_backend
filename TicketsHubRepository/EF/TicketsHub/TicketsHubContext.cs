using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class TicketsHubContext : DbContext
    {
        public TicketsHubContext()
        {
        }

        public TicketsHubContext(DbContextOptions<TicketsHubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CancelReasons> CancelReasons { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<ExternalSystems> ExternalSystems { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<OrderTickets> OrderTickets { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Setup> Setup { get; set; }
        public virtual DbSet<TicketHubConfiguration> TicketHubConfiguration { get; set; }
        public virtual DbSet<TicketsHubJourneyLog> TicketsHubJourneyLog { get; set; }
        public virtual DbSet<TicketsHubJourneyLogDetails> TicketsHubJourneyLogDetails { get; set; }
        public virtual DbSet<TicketsHubLogChangeTypes> TicketsHubLogChangeTypes { get; set; }
        public virtual DbSet<TicketsHubSyncLog> TicketsHubSyncLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ContainerManager.Container.Resolve<IConfigurationManager>().GetConnectionStringByValue<string>("TicketsHubContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<CancelReasons>(entity =>
            {
                entity.HasKey(e => e.CancelReasonId);
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK_Events_1");

                entity.Property(e => e.EventId).ValueGeneratedNever();

                entity.Property(e => e.EventDate).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(512);

                entity.Property(e => e.VeneueName).IsRequired();
            });

            modelBuilder.Entity<ExternalSystems>(entity =>
            {
                entity.HasKey(e => e.ExternalSystemId);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<OrderTickets>(entity =>
            {
                entity.HasKey(e => e.OrderTicketId);

                entity.Property(e => e.Barcode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CancelTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.EventDateTime).HasColumnType("datetime");

                entity.Property(e => e.PriceId).HasColumnName("PriceID");

                entity.Property(e => e.PriceLlevelName)
                    .IsRequired()
                    .HasColumnName("PriceLLevelName");

                entity.Property(e => e.ShortMessager)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TicketTypeName).IsRequired();

                entity.Property(e => e.TourComment).HasMaxLength(100);

                entity.Property(e => e.VariantFullBarcode).HasMaxLength(20);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderTickets)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderTickets_Orders");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.BookingTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.CancelTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.CategoryName).IsRequired();

                entity.Property(e => e.ExternalSystemEventDate).HasColumnType("date");

                entity.Property(e => e.ExternalSystemVenueName).IsRequired();

                entity.Property(e => e.MemberId).HasMaxLength(9);

                entity.Property(e => e.ReserveTimeStamp).HasColumnType("datetime");

                entity.HasOne(d => d.CancelReason)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CancelReasonId)
                    .HasConstraintName("FK_Orders_CancelReasons");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Events");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_OrderStatus");
            });

            modelBuilder.Entity<Setup>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);

                entity.Property(e => e.OrganizationId).ValueGeneratedNever();

                entity.Property(e => e.DaysBeforeShowToAllowCancel).HasDefaultValueSql("((3))");
            });

            modelBuilder.Entity<TicketHubConfiguration>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Value).IsRequired();
            });

            modelBuilder.Entity<TicketsHubJourneyLog>(entity =>
            {
                entity.Property(e => e.EventId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.MemberId).IsRequired();

                entity.Property(e => e.OrgId).IsRequired();

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TicketsHubJourneyLogDetails>(entity =>
            {
                entity.Property(e => e.Comment).IsRequired();

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TicketsHubLogChangeTypes>(entity =>
            {
                entity.HasKey(e => e.LogChangeTypeId);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TicketsHubSyncLog>(entity =>
            {
                entity.HasKey(e => e.TicketrsHubSuncLogId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.TicketsHubSyncLog)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_TicketsHubSyncLog_Events");

                entity.HasOne(d => d.TicketsHubSyncLogType)
                    .WithMany(p => p.TicketsHubSyncLog)
                    .HasForeignKey(d => d.TicketsHubSyncLogTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketsHubSyncLog_TicketsHubLogChangeTypes");
            });
        }
    }
}
