using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;      


namespace Rido.Data.Configurations;
public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");     

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
            .IsRequired();

        builder.HasIndex(rt => rt.Token)
            .IsUnique(true);



        builder.Property(rt => rt.Expiry)
            .IsRequired();

        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);     

        builder.HasOne(rt=>rt.User)        
            .WithOne()       
            .HasForeignKey<RefreshToken>(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);      
    }
}
