using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CABasicCRUD.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);
        builder
            .Property(user => user.Id)
            .HasConversion(userId => userId.Value, value => (UserId)value);

        builder.Property(user => user.Name).HasMaxLength(50).IsRequired();

        builder.Property(user => user.Email).HasMaxLength(70).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.PasswordHash).IsRequired();
    }
}
