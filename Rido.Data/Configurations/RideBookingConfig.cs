using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;

namespace Rido.Data.Configurations
{
    public class RideBookingConfig : IEntityTypeConfiguration<RideBooking>
    {
        public void Configure(EntityTypeBuilder<RideBooking> builder)
        {
            builder.HasKey(rb => rb.Id);

            builder.Property(rb => rb.UserId)
                .IsRequired(false);

            builder.Property(rb => rb.DriverId)
                .IsRequired(false);

            builder.Property(rb => rb.TransactionId)
               .IsRequired(false);

            builder.HasIndex(rb => rb.UserId).IsUnique(false);
            builder.HasIndex(rb => rb.DriverId).IsUnique(false);


            builder.Property(rb => rb.PickupTime)
                .IsRequired();

            builder.Property(rb => rb.DropoffTime)
                .IsRequired();

            builder.Property(rb => rb.PickupLatitude)
                .IsRequired();

            builder.Property(rb => rb.PickupLongitude)
                .IsRequired();

            builder.Property(rb => rb.PickupAddress)
                .IsRequired();

            builder.Property(rb => rb.DestinationLatitude)
                .IsRequired();

            builder.Property(rb => rb.DestinationLongitude)
                .IsRequired();

            builder.Property(rb => rb.DestinationAddress)
                .IsRequired();

            builder.Property(rb => rb.DistanceInKm)
                .IsRequired();

            builder.Property(rb => rb.VehicleType)
                .IsRequired();           

            builder.Property(rb => rb.GeohashCode)
                .IsRequired(false);        

            builder.Property(rb => rb.Amount)
                .HasColumnType("decimal(18,2)")

                .IsRequired();

            builder.HasOne(rb=>rb.User)        
                .WithMany()     
                .HasForeignKey(rb => rb.UserId)
                .OnDelete(DeleteBehavior.NoAction);       

            builder.HasOne(rb=>rb.Driver)        
                .WithMany()     
                .HasForeignKey(rb => rb.DriverId)
                .OnDelete(DeleteBehavior.NoAction);         

            builder.HasOne(rb=>rb.RideTransaction)        
                .WithOne()
                .HasForeignKey<RideBooking>(rb=>rb.TransactionId)
                .OnDelete(DeleteBehavior.NoAction);       

            builder.Property(rb => rb.CreatedAt)
                .HasDefaultValueSql("GETDATE()");     

            builder.Property(rb => rb.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")     
                .ValueGeneratedOnUpdate();     
        }
    }
}
