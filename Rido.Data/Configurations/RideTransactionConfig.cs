using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;

namespace Rido.Data.Configurations
{
    public class RideTransactionConfig : IEntityTypeConfiguration<RideTransaction>
    {
        public void Configure(EntityTypeBuilder<RideTransaction> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Property(rt => rt.DriverId)
                .IsRequired();    

            builder.Property(rt => rt.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();    

            builder.Property(rt => rt.Date)
                .HasDefaultValueSql("GETDATE()")       
                .ValueGeneratedOnAdd();       

            builder.Property(rt => rt.Status)
                .IsRequired();    

            builder.Property(rt => rt.Remarks)
                .IsRequired(false);    

            builder.HasOne<User>(rt=>rt.Rider)      
                .WithMany()        
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.NoAction);       

            builder.HasOne(rt=>rt.Driver)       
                .WithMany()        
                .HasForeignKey(rt => rt.DriverId)
                .OnDelete(DeleteBehavior.NoAction);         
        }
    }
}
