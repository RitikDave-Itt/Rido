using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;

using Rido.Data.Enums;

namespace Rido.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.LastName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(p => p.Gender)
                .HasConversion<int>();

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");


            builder.Property(p => p.Status)
                .IsRequired()
                .HasDefaultValue(UserStatus.Active);      

            builder.Property(p => p.ProfileImageId)
                .IsRequired(false);

            builder.Property(p => p.Role)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(UserRole.User);

            builder.HasOne(u=>u.ProfileImage)
                .WithOne()
                .HasForeignKey<User>(u=>u.ProfileImageId)
                .OnDelete(DeleteBehavior.Cascade);

           


           
        }

    }
}
