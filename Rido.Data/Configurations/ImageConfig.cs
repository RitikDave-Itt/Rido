using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;

namespace Rido.Data.Configurations
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");     

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileNAme)
                .IsRequired();

            builder.Property(x => x.Base64String)
                .IsRequired();

            builder.Property(x => x.FileType)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");      

            builder.HasIndex(x => x.FileType).IsUnique(false);       
        }
    }
}
