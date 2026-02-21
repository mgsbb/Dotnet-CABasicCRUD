using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Configurations;

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

        builder.Property(user => user.Username).HasMaxLength(50).IsRequired(false);
        builder.HasIndex(user => user.Username).IsUnique();

        builder.Property(user => user.Email).HasMaxLength(70).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.PasswordHash).IsRequired();

        builder.Property(user => user.CreatedAt).IsRequired();

        builder.Property(user => user.UpdatedAt).IsRequired(false);

        ConfigureUserProfile(builder);
    }

    private static void ConfigureUserProfile(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(
            user => user.UserProfile,
            profileBuilder =>
            {
                profileBuilder.ToTable("UserProfiles");

                profileBuilder.HasKey(userProfile => userProfile.Id);
                profileBuilder
                    .Property(userProfile => userProfile.Id)
                    .HasConversion(userProfileId => userProfileId.Value, value => (UserId)value);

                profileBuilder
                    .Property(userProfile => userProfile.FullName)
                    .HasMaxLength(50)
                    .IsRequired(false);

                profileBuilder.Property(userProfile => userProfile.Bio).HasMaxLength(200);

                profileBuilder.Property(userProfile => userProfile.ProfileImageUrl);

                profileBuilder.Property(userProfile => userProfile.CreatedAt).IsRequired();

                profileBuilder.Property(userProfile => userProfile.UpdatedAt).IsRequired(false);
            }
        );
    }
}
