using CABasicCRUD.Domain.MediaItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Configurations;

public sealed class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("MediaItems");

        builder.HasKey(media => media.Id);
        builder
            .Property(media => media.Id)
            .HasConversion(mediaId => mediaId.Value, value => (MediaId)value);

        builder.Property(media => media.StorageProvider).HasConversion<string>().IsRequired();

        builder.Property(media => media.StorageKey).IsRequired();

        builder.Property(media => media.Url).IsRequired();

        builder.Property(media => media.MediaType).HasConversion<string>();

        builder.Property(media => media.FileName).IsRequired();

        builder.Property(media => media.Size).IsRequired();

        builder.Property(media => media.ContentType).IsRequired(false);

        builder.Property(media => media.CreatedAt).IsRequired();

        builder.Property(media => media.UpdatedAt).IsRequired(false);
    }
}
