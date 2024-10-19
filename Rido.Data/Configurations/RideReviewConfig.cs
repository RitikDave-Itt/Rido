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
    public class RideReviewConfig :IEntityTypeConfiguration<RideReview>

    {

        public void Configure(EntityTypeBuilder<RideReview> builder)
        {
            builder.ToTable("RideReview");

            builder.HasKey(rr=>rr.Id);

            builder.Property(rr=>rr.Rating)
                .IsRequired(true)
                .HasColumnType("decimal(2,1)");

            builder.Property(rr => rr.Comment)
                .IsRequired(false);

            builder.HasOne(rr => rr.User)
                .WithMany()
                .HasForeignKey(rr => rr.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(rr=>rr.Driver)
                .WithMany()
                .HasForeignKey(rr=>rr.DriverId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(rr=>rr.Booking)
                .WithOne()
                .HasForeignKey<RideReview>(rr=>rr.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rr => rr.UserId);
            builder.HasIndex(rr => rr.DriverId);
            builder.HasIndex(rr => rr.BookingId);





        }
    }
}
