using Microsoft.EntityFrameworkCore;
using Rido.Data.Configurations;
using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Contexts
{
    public class RidoDbContext : DbContext
    {
        public RidoDbContext(DbContextOptions<RidoDbContext> options) : base(options) { }

        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.DriverData> DriverData { get; set; }
        public DbSet<Entities.DriverLocation> DriverLocations { get; set; }
        public DbSet<Entities.RideRequest> RideRequests { get; set; }


        public DbSet<Entities.Wallet> Wallets { get; set; }
        public DbSet<Entities.RideTransaction> RideTransactions { get; set; }

        public DbSet<Entities.RideReview> RideReviews { get; set; }
        public DbSet<Entities.WalletTransaction> WalletTransactions { get; set; }
        public DbSet<Entities.RefreshToken> RefreshTokens { get; set; }

        public DbSet<Entities.Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.User>)new Configurations.UserConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.DriverData>)new Configurations.DriverDataConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.DriverLocation>)new Configurations.DriverLocationConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.RideRequest>)new Configurations.RideRequestConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.Wallet>)new Configurations.WalletConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.RideTransaction>)new Configurations.RideTransactionConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.RideReview>)new Configurations.RideReviewConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.WalletTransaction>)new Configurations.WalletTransactionConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.RefreshToken>)new Configurations.RefreshTokenConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.Image>)new Configurations.ImageConfig());




        }
    }
}
