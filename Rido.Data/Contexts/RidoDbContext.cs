﻿using Microsoft.EntityFrameworkCore;
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




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.User>)new Configurations.UserConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.DriverData>)new Configurations.DriverDataConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.DriverLocation>)new Configurations.DriverLocationConfig());
            modelBuilder.ApplyConfiguration((IEntityTypeConfiguration<Entities.RideRequest>)new Configurations.RideRequestConfig());





        }
    }
}
