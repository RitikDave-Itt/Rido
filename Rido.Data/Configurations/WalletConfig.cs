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
    public class WalletConfig :IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallet");

            builder.HasKey(x => x.Id);

            builder.Property(w => w.UserId).IsRequired();
            builder.HasIndex(x => x.UserId);
            builder.Property(x => x.Balance)
                .HasColumnType("decimal(7,2)");

                builder.HasOne(x=>x.User)
                .WithOne(u=>u.Wallet)
                .HasForeignKey<Wallet>(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
