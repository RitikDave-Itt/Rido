using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Configurations
{
    public class RideRequestConfig : IEntityTypeConfiguration<RideRequest>
    {
        public void Configure(EntityTypeBuilder<RideRequest> builder)
        {
            builder.ToTable("RideRequests");

            builder.HasKey(rr => rr.Id);

            builder.Property(rr => rr.UserId)
                .IsRequired();

            builder.HasIndex(rr => rr.UserId)
                .IsUnique();

            builder.Property(rr => rr.DriverId)
                .IsRequired(false);

            builder.HasIndex(rr=>rr.DriverId)
                .IsUnique();
                

            builder.Property(rr => rr.PickupLatitude)
                   .IsRequired();

            builder.Property(rr => rr.PickupLongitude)
                   .IsRequired();

            builder.Property(rr => rr.PickupAddress)
                   .IsRequired();

            builder.Property(rr => rr.PickupTime)
                   .IsRequired();

            builder.Property(rr => rr.DestinationLatitude)
                   .IsRequired();

            builder.Property(rr => rr.DestinationLongitude)
                   .IsRequired();

            builder.Property(rr => rr.DestinationAddress)
                   .IsRequired();

            builder.Property(rr => rr.MinPrice)
                   .IsRequired();

            builder.Property(rr => rr.MaxPrice)
                   .IsRequired();

            builder.Property(rr => rr.VehicleType)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(rr => rr.GeohashCode)
                   .IsRequired();

            builder.Property(rr => rr.DistanceInKm)
                   .IsRequired();

            builder.HasIndex(rr => rr.VehicleType);
            builder.HasIndex(rr => rr.GeohashCode);

            builder.Property(rr => rr.MinPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(rr => rr.MaxPrice)
                   .HasColumnType("decimal(18,2)");
            builder.Property(rr => rr.Status)
                  .IsRequired()
                  .HasConversion<int>();

            builder.HasOne(rr => rr.User)
                .WithOne()
                .HasForeignKey<RideRequest>(rr => rr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rr => rr.Driver)
                .WithOne()
                .HasForeignKey<RideRequest>(rr => rr.DriverId)
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}
