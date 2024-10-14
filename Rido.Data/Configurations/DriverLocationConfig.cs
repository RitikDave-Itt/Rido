using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Configurations
{
    public class DriverLocationConfig :IEntityTypeConfiguration<DriverLocation>
    {
        public void Configure(EntityTypeBuilder<DriverLocation> builder)
        {
            builder.ToTable("DriverLocations");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.DriverId)
                .IsRequired();
                
                


            builder.Property(p => p.Latitude)
                .IsRequired();
            builder.Property(p => p.Longitude)
               .IsRequired();
            builder.Property(p => p.Geohash)
               .IsRequired();

            builder.Property(p => p.VehicleType)
                .IsRequired();
            builder.HasIndex(p => p.DriverId)
                .IsUnique();
            builder.HasIndex(p => p.Geohash);
        }

    }
}
