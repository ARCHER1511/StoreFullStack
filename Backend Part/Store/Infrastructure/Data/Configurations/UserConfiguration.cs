using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasMaxLength(36).IsRequired();

            builder.HasIndex(u => u.Id).IsUnique();

            builder.Property(u => u.UserName).IsRequired().HasMaxLength(100);

            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);

            builder.Property(u => u.LastLoginTime);
        }
    }
}
