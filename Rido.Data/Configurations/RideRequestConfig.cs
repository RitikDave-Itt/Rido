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

            builder.Property(rr => rr.RiderId)
                .IsRequired(false);

            builder.HasIndex(rr => rr.RiderId);


            builder.Property(rr => rr.DriverId)
                .IsRequired(false);

            builder.HasIndex(rr => rr.DriverId);
                

            builder.Property(rr => rr.PickupLatitude)
                   .IsRequired();

            builder.Property(rr => rr.PickupLongitude)
                   .IsRequired();

            builder.HasIndex(rr => rr.IsActive);

            builder.Property(rr => rr.IsActive)
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

            builder.Property(rr => rr.Amount)
                   .IsRequired();
            


            builder.Property(rr => rr.VehicleType)
                   .IsRequired()
                   .HasConversion<int>();

           
            builder.Property(rr => rr.DistanceInKm)
                   .IsRequired();

            builder.HasIndex(rr => rr.VehicleType);

            builder.Property(rr => rr.Amount)
                   .HasColumnType("decimal(18,2)");

         
            builder.Property(rr => rr.Status)
                  .IsRequired()
                  .HasConversion<int>();

            builder.HasOne(rr=>rr.Rider)
                .WithMany()
                .HasForeignKey(rr => rr.RiderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rr => rr.RideReview)
                .WithOne()
                .HasForeignKey<RideReview>(rr => rr.RideRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rr=>rr.Driver)
                .WithMany()
                .HasForeignKey(rr => rr.DriverId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(rr=>rr.RideTransaction)
                .WithMany()
                .HasForeignKey(rr=>rr.TransactionId)
                .OnDelete(DeleteBehavior.NoAction);
                
        }
    }
}
