using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rido.Data.Entities;

namespace Rido.Data.Configurations
{
    public class WalletTransactionConfig : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransactions");

            builder.HasKey(wt => wt.Id);

            builder.Property(wt => wt.UserId)
                .IsRequired();

            builder.Property(wt => wt.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");      

            builder.Property(wt => wt.Type)
                .IsRequired();       

            builder.Property(wt => wt.Status)
                .IsRequired();

            builder.Property(wt => wt.CreatedAt)
                .IsRequired();

            builder.Property(wt => wt.UpdatedAt)
                .IsRequired(false);     

            builder.Property(wt => wt.RazorPayId)
                .IsRequired(false);      

            builder.Property(wt => wt.Remarks)
                .IsRequired(false);      

            builder.HasOne(wt => wt.User)
                .WithMany()      
                .HasForeignKey(wt => wt.UserId)   
                .OnDelete(DeleteBehavior.Cascade);    
        }
    }
}
