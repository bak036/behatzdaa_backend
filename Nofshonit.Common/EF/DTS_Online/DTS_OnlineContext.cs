using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Nofshonit.Common.DTOs.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.DTS_Online
{
    /// <summary>
    /// An inheritance is made from the main DTS_OnlineContext object to allow use of the store procedure and not to damage the existing mapping (DB first)
    /// </summary>
    public class DTS_OnlineContext : DtsDB_Context
    {
        public DTS_OnlineContext() : base() { }
        public DTS_OnlineContext(DbContextOptions<DTS_OnlineContext> options)
         : base(ChangeOptionsType<DtsDB_Context>(options))
        {
        }

        // Manually add views
        public virtual DbSet<View_GetBusinessAndTags> View_GetBusinessAndTags { get; set; }
        public virtual DbSet<View_GetBranchesByWalletId> View_GetBranchesByWalletIds { get; set; }
        public virtual DbSet<SubBranchCityRegionData> SubBranchCityRegionData { get; set; }
        public virtual DbSet<PurchaseRestriction> PurchaseRestriction { get; set; }
        public DbSet<WalletChainBranches> WalletChainBranches { get; set; }
        public virtual DbSet<CouponsStocksDetails> CouponsStocksDetails { get; set; }

        //Convet DbContextOptions <Type>
        protected static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            var sqlExt = options.Extensions.FirstOrDefault(e => e is SqlServerOptionsExtension);

            if (sqlExt == null)
                throw (new Exception("Failed to retrieve SQL connection string for base Context"));

            return new DbContextOptionsBuilder<T>()
                        .UseSqlServer(((SqlServerOptionsExtension)sqlExt).ConnectionString)
                        .Options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubBranchCityRegionData>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });

            modelBuilder.Entity<PurchaseRestriction>(entity =>
            {
                entity.HasKey(e => e.Restrictionid);
                entity.Property(e => e.Restrictionid).ValueGeneratedOnAdd();
                entity.ToTable("PurchaseRestriction");
            });

            modelBuilder.Entity<WalletChainBranches>().HasNoKey();

        }
    }
}
