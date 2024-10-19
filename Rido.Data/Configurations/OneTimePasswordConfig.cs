using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;

namespace Rido.Data.Configurations
{
    public class OneTimePasswordConfig : IEntityTypeConfiguration<OneTimePassword>
    {
        public void Configure(EntityTypeBuilder<OneTimePassword> builder)
        {
            builder.ToTable("OneTimePasswords");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.OTP)
                .IsRequired()
                .HasMaxLength(4);

            builder.HasOne(otp => otp.RideRequest)
  .WithOne(rr => rr.OneTimePassword)
  .HasForeignKey<OneTimePassword>(o => o.RideRequestId)
  .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
