using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasMaxLength(36).IsRequired();

            builder.HasIndex(p => p.Id).IsUnique();

            builder.Property(p => p.Category).IsRequired().HasMaxLength(100);

            builder.Property(p => p.ProductCode).IsRequired().HasMaxLength(50);

            builder.HasIndex(p => p.ProductCode).IsUnique();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

            builder.Property(p => p.MinimumQuantity).IsRequired();

            builder.Property(p => p.DiscountRate).HasColumnType("decimal(5,4)");
        }
    }
}
