using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;

namespace Rido.Data.Configurations
{
    public class DriverDataConfig : IEntityTypeConfiguration<DriverData>
    {
        public void Configure(EntityTypeBuilder<DriverData> builder)
        {
            builder.ToTable("DriverData");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.UserId)
                .IsRequired();

            builder.HasIndex(d => d.UserId)
                .IsUnique();

            builder.Property(d => d.LicenseType)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(d => d.LicenseNumber)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(d => d.VehicleType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(d => d.VehicleRegistrationNumber)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(d => d.VehicleModel)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(d => d.VehicleMake)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(d => d.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");     

            builder.Property(d => d.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(d=>d.User)
                .WithOne(u=>u.DriverData)
                .HasForeignKey<DriverData>(d=>d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            



            
        }
    }
}
